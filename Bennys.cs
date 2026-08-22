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
        public Bennys()
        {
            Tick += OnTick;
            Aborted += OnAborted;

            LoadSettings();
            Logger.Initialize();
            Logger.Log("Bennys initialized.");
            bennyIntID = Helper.GetInteriorID(new Vector3(-211.798f, -1324.292f, 30.37535f));
            CreateBlip();
        }

        private void OnTick(object sender, EventArgs e)
        {
            try
            {
                if (optEnableMouse && Helper._menuPool != null && Helper._menuPool.AreAnyVisible)
                {
                    EnableWorkshopMenuMouseControls();
                }

                ProcessPendingMPDLCMapLoad();
                veh = Game.Player.Character.LastVehicle;
                ply = Game.Player.Character;

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

                int currentInteriorId = GetInteriorID(ply.Position);
                bool isMenuVisible = Helper._menuPool != null && Helper._menuPool.AreAnyVisible;
                string vehicleDenialMessage = GetWorkshopVehicleDenialMessage(veh);
                bool isWorkshopVehicleAllowed = !unWelcome.Contains(veh.ClassType) && vehicleDenialMessage == null;
                bool isNearGarageDoor = veh.Position.DistanceTo(new Vector3(-205.6828f, -1310.683f, 30.29572f)) <= 15.0f;

                bool isInsideWorkshop = isCutscene
                    || isMenuVisible
                    || (isWorkshopVehicleAllowed && currentInteriorId == bennyIntID);

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
                    Function.Call((Hash)0x9B12F9A24FABEDB0UL, -427498890, -205.6828f, -1310.683f, 30.29572f, openDoor ? 0 : 1, 0.0f, 50.0f, 0);
                }

                if (currentInteriorId == bennyIntID && !IsArenaWarDLCInstalled())
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

                if (currentInteriorId == bennyIntID && isWorkshopVehicleAllowed)
                {
                    if (!isExiting)
                    {
                        if (CanTriggerEnterCutscene())
                        {
                            UpdateTitleName();
                            PlayEnterCutScene();
                        }
                        else if (veh.Position.DistanceTo(new Vector3(-211.798f, -1324.292f, 30.37535f)) <= 5.0f)
                        {
                            camera.Update();
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
