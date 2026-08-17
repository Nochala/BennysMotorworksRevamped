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
            GTA.Native.GlobalVariable.Get((int)Helper.GetGlobalValue()).Write(1);
        }

        private void OnTick(object sender, EventArgs e)
        {
            try
            {
                veh = Game.Player.Character.LastVehicle;
                ply = Game.Player.Character;

                if (veh != null && veh.IsVehicleAttachedToTrailer())
                {
                    tra = veh.GetVehicleTrailerVehicle();
                }

                if (veh == null || ply == null)
                {
                    return;
                }

                ProcessWorkshopCutscene();

                bool isInsideWorkshop = isCutscene
                    || (Helper._menuPool != null && Helper._menuPool.AreAnyVisible)
                    || GetInteriorID(ply.Position) == bennyIntID;
                if (isInsideWorkshop)
                {
                    Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 10);
                }

                if (fixDoor == 1 && !unWelcome.Contains(veh.ClassType))
                {
                    bool openDoor = veh.Position.DistanceTo(new Vector3(-205.6828f, -1310.683f, 30.29572f)) <= 15.0f;
                    Function.Call((Hash)0x9B12F9A24FABEDB0UL, -427498890, -205.6828f, -1310.683f, 30.29572f, openDoor ? 0 : 1, 0.0f, 50.0f, 0);
                }

                if (GetInteriorID(ply.Position) == bennyIntID && !IsArenaWarDLCInstalled())
                {
                    Helper.DisplayHelpTextThisFrame("Un-supported GTA V version detected! SPB may not work properly on this version.");
                }

                if (Helper.GetInteriorID(ply.Position) == bennyIntID && !unWelcome.Contains(veh.ClassType))
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
                if (Game.IsControlPressed(zinKey) && camera.MainCameraPosition != CameraPosition.Interior)
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

                if (Game.IsControlPressed(zoutKey) && camera.MainCameraPosition != CameraPosition.Interior)
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

                if (Game.IsControlJustReleased(fpcKey))
                {
                    CameraPosition previousCameraPosition = lastCameraPos;
                    lastCameraPos = camera.MainCameraPosition;

                    if (camera.MainCameraPosition == CameraPosition.Interior)
                    {
                        camera.MainCameraPosition = previousCameraPosition == CameraPosition.Interior
                            ? CameraPosition.Car
                            : previousCameraPosition;
                    }
                    else
                    {
                        camera.MainCameraPosition = CameraPosition.Interior;
                    }
                }
            }
        }

        private void OnAborted(object sender, EventArgs e)
        {
            BennysBlip?.Delete();
            GTA.UI.Screen.FadeIn(1000);

            if (bennyPed != null)
            {
                bennyPed.Delete();
            }
        }
    }
}
