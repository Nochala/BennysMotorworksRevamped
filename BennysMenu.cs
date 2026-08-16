using GTA;
using GTA.Native;
using LemonUI;
using LemonUI.Scaleform;
using System;
using System.Drawing;
using static BennysMotorworksRevamped.Helper;
using static BennysMotorworksRevamped.MenuHelper;

namespace BennysMotorworksRevamped
{
    public class BennysMenu : Script
    {
        public bool IsScriptLoaded { get; private set; }

        public BennysMenu()
        {
            Tick += OnTick;

            MenuHelper._menuPool = new ObjectPool();
            Helper._menuPool = MenuHelper._menuPool;
            Logger.Initialize();
            Logger.Log("BennysMenu initialized.");
            camera = new WorkshopCamera();
            BtnFirstPerson = new LemonUI.Scaleform.InstructionalButton(Game.GetLocalizedString("MO_ZOOM_FIRST"), fpcKey);
            BtnZoom = new LemonUI.Scaleform.InstructionalButton(Game.GetLocalizedString("INPUT_CREATOR_ZOOM_IN_DISPLAYONLY"), zinKey);
            BtnZoomOut = new LemonUI.Scaleform.InstructionalButton(Game.GetLocalizedString("INPUT_CREATOR_ZOOM_OUT_DISPLAYONLY"), zoutKey);

            Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "VEHICLE_SHOP_HUD_1", false, -1);
            Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "VEHICLE_SHOP_HUD_2", false, -1);
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (!IsScriptLoaded)
            {
                if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
                {
                    Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
                    Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
                    return;
                }

                CreateMenus();
                Logger.Log("CreateMenus completed.");
                IsScriptLoaded = true;
            }

            try
            {
                BennysMotorworksRevamped.Compat.UIMenu.EnsureSingleVisibleMenu();
                MenuHelper.RefreshMenuMouseBehavior();
                MenuHelper._menuPool?.Process();
                BennysMotorworksRevamped.Compat.UIMenu.EnsureSingleVisibleMenu();

                bool isMenuVisible = MenuHelper._menuPool != null && MenuHelper._menuPool.AreAnyVisible;

                if (veh != null)
                {
                    vehStats = GetVehicleStats(veh);
                }

                if (isMenuVisible)
                {
                    Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                }

                // Only show the vehicle title once the workshop entry cutscene starts,
                // and keep it visible while a Benny's menu is open.
                if (veh != null && (isCutscene || isMenuVisible))
                {
                    Helper.DisplayVehicleInfoBottomRight(veh.DisplayName, GetClassDisplayName(veh.ClassType));
                }

                if (isCutscene && veh != null)
                {
                    Game.DisableAllControlsThisFrame();
                    Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                }

                if (isRepairing)
                {
                    MenuHelper.HideAllMenus();
                    if (MainMenu != null)
                    {
                        MainMenu.Visible = true;
                    }

                    isRepairing = false;
                }

                if (isMenuVisible)
                {
                    SuspendKeys();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
