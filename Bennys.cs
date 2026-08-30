using System;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;
using static BennysMotorworksRevamped.Helper;
using static BennysMotorworksRevamped.MenuHelper;

namespace BennysMotorworksRevamped
{
    public class Bennys : Script
    {
        private const int StartupNativeDelayMs = 1000;
        private const int StartupNativeRetryDelayMs = 500;
        private const int PlayerInteriorProbeDelayMs = 250;
        private const float PlayerInteriorProbeRadius = 100.0f;
        private static readonly Vector3 WorkshopInteriorPosition = new Vector3(-211.798f, -1324.292f, 30.37535f);

        private bool _garageDoorStateKnown;
        private bool _garageDoorOpen;
        private bool _startupNativeInitializationScheduled;
        private bool _startupNativeInitialized;
        private int _startupNativeInitializeAt;
        private int _nextPlayerInteriorProbeTime;
        private int _cachedPlayerInteriorId;

        public Bennys()
        {
            Tick += OnTick;
            Aborted += OnAborted;

            LoadSettings();
            Logger.Initialize();
            Logger.Log("Bennys startup queued.");
        }

        private bool EnsureGameNativeStartupInitialized()
        {
            if (_startupNativeInitialized)
            {
                return true;
            }

            if (!_startupNativeInitializationScheduled)
            {
                _startupNativeInitializationScheduled = true;
                _startupNativeInitializeAt = Game.GameTime + StartupNativeDelayMs;
                return false;
            }

            if (Game.GameTime < _startupNativeInitializeAt)
            {
                return false;
            }

            try
            {
                Ped player = Game.Player.Character;
                if (player == null || !player.Exists())
                {
                    _startupNativeInitializeAt = Game.GameTime + StartupNativeRetryDelayMs;
                    return false;
                }

                int detectedInteriorId = Helper.GetInteriorID(WorkshopInteriorPosition);
                if (detectedInteriorId != 0)
                {
                    bennyIntID = detectedInteriorId;
                }

                if (BennysBlip == null || !BennysBlip.Exists())
                {
                    CreateBlip();
                }

                _startupNativeInitialized = true;
                Logger.Log("Bennys initialized. Game-native startup completed. interiorId=" + bennyIntID);
                return true;
            }
            catch (Exception ex)
            {
                _startupNativeInitializeAt = Game.GameTime + StartupNativeRetryDelayMs;
                Logger.Debug("Bennys game-native startup deferred. " + ex.Message);
                return false;
            }
        }

        private int GetCurrentPlayerInteriorId(bool isMenuVisible)
        {
            if (ply == null)
            {
                return 0;
            }

            bool shouldProbe = isCutscene
                || isMenuVisible
                || ply.Position.DistanceTo(WorkshopInteriorPosition) <= PlayerInteriorProbeRadius;

            if (!shouldProbe)
            {
                _cachedPlayerInteriorId = 0;
                _nextPlayerInteriorProbeTime = 0;
                return 0;
            }

            if (Game.GameTime >= _nextPlayerInteriorProbeTime)
            {
                _cachedPlayerInteriorId = GetInteriorID(ply.Position);
                _nextPlayerInteriorProbeTime = Game.GameTime + PlayerInteriorProbeDelayMs;
            }

            return _cachedPlayerInteriorId;
        }

        private void OnTick(object sender, EventArgs e)
        {
            try
            {
                if (!EnsureGameNativeStartupInitialized())
                {
                    return;
                }

                if (optEnableMouse && Helper._menuPool != null && Helper._menuPool.AreAnyVisible)
                {
                    EnableWorkshopMenuMouseControls();
                }

                ProcessPendingMPDLCMapLoad();
                ply = Game.Player.Character;
                veh = ply?.LastVehicle;

                if (veh != null && veh.IsVehicleAttachedToTrailer())
                {
                    tra = veh.GetVehicleTrailerVehicle();
                }

                if (veh == null || ply == null)
                {
                    SetWorkshopPlayerControlSuppressed(false);
                    SetWorkshopCarModShopState(false);
                    return;
                }

                ProcessWorkshopCutscene();

                bool isMenuVisible = Helper._menuPool != null && Helper._menuPool.AreAnyVisible;
                int currentInteriorId = GetCurrentPlayerInteriorId(isMenuVisible);
                bool isInBennysInterior = bennyIntID != 0 && currentInteriorId == bennyIntID;
                string vehicleDenialMessage = GetWorkshopVehicleDenialMessage(veh);
                bool isWorkshopVehicleAllowed = !unWelcome.Contains(veh.ClassType) && vehicleDenialMessage == null;
                bool isNearGarageDoor = veh.Position.DistanceTo(new Vector3(-205.6828f, -1310.683f, 30.29572f)) <= 15.0f;

                bool isInsideWorkshop = isCutscene
                    || isMenuVisible
                    || (isWorkshopVehicleAllowed && isInBennysInterior);

                SetWorkshopCarModShopState(
                    isInsideWorkshop
                    && isWorkshopVehicleAllowed
                    && ply.CurrentVehicle == veh);

                if (isInsideWorkshop)
                {
                    Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 10);
                }

                if (fixDoor == 1 || !isWorkshopVehicleAllowed)
                {
                    bool openDoor = fixDoor == 1 && isWorkshopVehicleAllowed && isNearGarageDoor;
                    if (!_garageDoorStateKnown || _garageDoorOpen != openDoor)
                    {
                        Function.Call((Hash)0x9B12F9A24FABEDB0UL, -427498890, -205.6828f, -1310.683f, 30.29572f, openDoor ? 0 : 1, 0.0f, 50.0f, 0);
                        _garageDoorOpen = openDoor;
                        _garageDoorStateKnown = true;
                    }
                }
                else
                {
                    _garageDoorStateKnown = false;
                }

                if (isInBennysInterior && !IsArenaWarDLCInstalled())
                {
                    Helper.DisplayHelpTextThisFrame("Un-supported GTA V version detected! SPB may not work properly on this version.");
                }

                if (!string.IsNullOrEmpty(vehicleDenialMessage)
                    && isNearGarageDoor
                    && ply.CurrentVehicle == veh
                    && !isCutscene
                    && !isMenuVisible)
                {
                    Helper.DisplayHelpTextThisFrame(vehicleDenialMessage);
                }

                if (isInBennysInterior && isWorkshopVehicleAllowed)
                {
                    if (!isExiting)
                    {
                        if (CanTriggerEnterCutscene())
                        {
                            UpdateTitleName();
                            PlayEnterCutScene();
                        }
                        else if (veh.Position.DistanceTo(WorkshopInteriorPosition) <= 5.0f)
                        {
                            camera?.Update();
                            Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                            Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, 3);
                            Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, 4);
                            Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, 5);
                            Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, 13);
                        }
                    }

                    if (isExiting)
                    {
                        Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                        Game.DisableAllControlsThisFrame();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }

            if (Helper._menuPool != null && Helper._menuPool.AreAnyVisible)
            {
                bool doorControlReleased = Game.IsControlJustReleased(doorKey)
                    || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 0, (int)doorKey);

                if (doorControlReleased)
                {
                    if (veh.Doors[VehicleDoorIndex.FrontLeftDoor].IsOpen)
                    {
                        Function.Call(Hash.SET_VEHICLE_DOORS_SHUT, veh, false);
                    }
                    else
                    {
                        veh.OpenDoor(VehicleDoorIndex.BackLeftDoor, false, false);
                        veh.OpenDoor(VehicleDoorIndex.BackRightDoor, false, false);
                        veh.OpenDoor(VehicleDoorIndex.FrontLeftDoor, false, false);
                        veh.OpenDoor(VehicleDoorIndex.FrontRightDoor, false, false);
                        veh.OpenDoor(VehicleDoorIndex.Hood, false, false);
                        veh.OpenDoor(VehicleDoorIndex.Trunk, false, false);
                    }
                }

                if ((Game.IsControlPressed(zinKey)
                    || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (int)zinKey))
                    && camera.MainCameraPosition != CameraPosition.Interior)
                {
                    PointF max = new PointF(6.0f, 3.0f);
                    if (camera.CameraZoom > max.Y)
                    {
                        camera.CameraZoom -= 0.1f;
                    }
                    else
                    {
                        camera.CameraZoom = max.Y;
                    }
                }

                if ((Game.IsControlPressed(zoutKey)
                    || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (int)zoutKey))
                    && camera.MainCameraPosition != CameraPosition.Interior)
                {
                    PointF max = new PointF(6.0f, 3.0f);
                    if (camera.CameraZoom < max.X)
                    {
                        camera.CameraZoom += 0.1f;
                    }
                    else
                    {
                        camera.CameraZoom = max.X;
                    }
                }

            }
        }

        private void OnAborted(object sender, EventArgs e)
        {
            SetWorkshopPlayerControlSuppressed(false);
            SetWorkshopCarModShopState(false);
            Helper.CleanupMPDLCMapLoad();
            BennysBlip?.Delete();
            GTA.UI.Screen.FadeIn(1000);

            if (bennyPed != null)
            {
                bennyPed.Delete();
            }
        }
    }
}
