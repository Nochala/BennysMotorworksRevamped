using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Scaleform;
using LemonUI.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using Font = GTA.UI.Font;
using MenuPool = LemonUI.ObjectPool;
using UIMenu = LemonUI.Menus.NativeMenu;
using VehicleMod = GTA.VehicleModType;
using VehicleToggleMod = GTA.VehicleToggleModType;

namespace BennysMotorworksRevamped
{
    internal static class Decor
    {
        internal enum eDecorType
        {
            Float = 1,
            Bool = 2,
            Int = 3,
            Time = 5,
        }

        internal static bool Registered(string name, eDecorType type)
        {
            try
            {
                return Function.Call<bool>((Hash)0x4F14F9F870D6FBC8UL, name, (int)type);
            }
            catch
            {
                return false;
            }
        }
    }

    public static class Helper
    {
        public static string nitroMod = "inm_nitro_active";

        public static List<Model> lowriders = new()
        {
            "banshee", "brioso2", "buccaneer", "chino", "diablous", "comet2", "faction", "faction2", "fcr", "italigtb",
            "minivan", "moonbeam", "nero", "primo", "sabregt", "sentinel3", "slamvan", "specter", "sultan", "tenf",
            "tornado", "tornado2", "tornado3", "virgo3", "voodoo2", "weevil", "elegy2", "youga2", "yosemite", "peyote",
            "manana", "glendale", "gauntlet3",
        };

        private static readonly Dictionary<Model, Tuple<Model, int>> lowriderUpgrades = new()
        {
            { new Model("banshee"), Tuple.Create(new Model("banshee2"), 565000) },
            { new Model("brioso2"), Tuple.Create(new Model("brioso3"), 585000) },
            { new Model("buccaneer"), Tuple.Create(new Model("buccaneer2"), 390000) },
            { new Model("chino"), Tuple.Create(new Model("chino2"), 180000) },
            { new Model("diablous"), Tuple.Create(new Model("diablous2"), 245000) },
            { new Model("comet2"), Tuple.Create(new Model("comet3"), 645000) },
            { new Model("faction"), Tuple.Create(new Model("faction2"), 335000) },
            { new Model("faction2"), Tuple.Create(new Model("faction3"), 695000) },
            { new Model("fcr"), Tuple.Create(new Model("fcr2"), 196000) },
            { new Model("italigtb"), Tuple.Create(new Model("italigtb2"), 495000) },
            { new Model("minivan"), Tuple.Create(new Model("minivan2"), 330000) },
            { new Model("moonbeam"), Tuple.Create(new Model("moonbeam2"), 370000) },
            { new Model("nero"), Tuple.Create(new Model("nero2"), 605000) },
            { new Model("primo"), Tuple.Create(new Model("primo2"), 400000) },
            { new Model("sabregt"), Tuple.Create(new Model("sabregt2"), 490000) },
            { new Model("sentinel3"), Tuple.Create(new Model("sentinel4"), 700000) },
            { new Model("slamvan"), Tuple.Create(new Model("slamvan3"), 415000) },
            { new Model("specter"), Tuple.Create(new Model("specter2"), 252000) },
            { new Model("sultan"), Tuple.Create(new Model("sultanrs"), 795000) },
            { new Model("tenf"), Tuple.Create(new Model("tenf2"), 575000) },
            { new Model("tornado"), Tuple.Create(new Model("tornado5"), 375000) },
            { new Model("tornado2"), Tuple.Create(new Model("tornado5"), 375000) },
            { new Model("tornado3"), Tuple.Create(new Model("tornado5"), 375000) },
            { new Model("virgo3"), Tuple.Create(new Model("virgo2"), 240000) },
            { new Model("voodoo2"), Tuple.Create(new Model("voodoo"), 420000) },
            { new Model("weevil"), Tuple.Create(new Model("weevil2"), 980000) },
            { new Model("elegy2"), Tuple.Create(new Model("elegy"), 904000) },
            { new Model("youga2"), Tuple.Create(new Model("youga3"), 1288000) },
            { new Model("yosemite"), Tuple.Create(new Model("yosemite3"), 700000) },
            { new Model("peyote"), Tuple.Create(new Model("peyote3"), 620000) },
            { new Model("manana"), Tuple.Create(new Model("manana2"), 925000) },
            { new Model("glendale"), Tuple.Create(new Model("glendale2"), 520000) },
            { new Model("gauntlet3"), Tuple.Create(new Model("gauntlet5"), 815000) },
        };

        public static List<Model> arenawar = new()
        {
            "glendale", "gargoyle", "dominator", "dominator2", "impaler", "issi3", "ratloader", "ratloader2", "slamvan", "slamvan2", "slamvan3",
        };

        public static List<Model> bennysvehicle = new()
        {
            "banshee2", "brioso3", "buccaneer2", "chino2", "diablous2", "comet3", "faction2", "faction3", "fcr2", "italigtb2",
            "minivan2", "moonbeam2", "nero2", "primo2", "sabregt2", "sentinel4", "slamvan3", "specter2", "sultanrs", "tenf2",
            "tornado5", "virgo2", "voodoo", "weevil2", "elegy", "youga3", "yosemite3", "peyote3", "manana2", "glendale2",
            "gauntlet5",
        };

        public static List<Model> arenavehicle = new()
        {
            "bruiser", "bruiser2", "bruiser3", "cerberus", "cerberus2", "cerberus3", "deathbike", "deathbike2", "deathbike3",
            "dominator4", "dominator5", "dominator6", "impaler2", "impaler3", "impaler4", "imperator", "imperator2", "imperator3",
            "issi4", "issi5", "issi6", "monster3", "monster4", "monster5", "slamvan4", "slamvan5", "slamvan6", "brutus", "brutus2",
            "brutus3", "scarab", "scarab2", "scarab3", "zr380", "zr3802", "zr3803",
        };

        public static Vehicle veh, tra;
        public static Ped ply;
        public static int onlineMap = 1;
        public static int fixDoor = 1;
        public static int bennyIntID;
        public static bool isExiting = false;
        public static Memory lastVehMemory;
        public static Blip BennysBlip;
        public static Ped bennyPed;
        public static bool isCutscene = false;
        public static bool optLogging = true;
        private static bool _pendingShopInit = false;
        private static int _shopInitDelayTime = 0;
        public static Camera scriptCam; // ScriptedCamera
        public static List<VehicleClass> unWelcome = new() { VehicleClass.Boats, VehicleClass.Cycles, VehicleClass.Helicopters, VehicleClass.Planes };
        public static GTA.Control fpcKey, zoutKey, zinKey;
        public static CameraPosition lastCameraPos;

        public static InstructionalButton BtnZoom, BtnZoomOut, BtnFirstPerson;
        public static MenuPool _menuPool;
        public static WorkshopCamera camera;
        public static bool isRepairing = false;
        public static VehicleStats vehStats;
        public static float vehicleStatsOffsetX = 0f;
        public static float vehicleStatsOffsetY = -10f;
        public static string arenaVehImage = "brusier_apoc";

        private static float cachedExitHeading = 0f;

        private static string Gxt(string key) => Game.GetLocalizedString(key);

        public static void DisplayVehicleInfoBottomRight(string vehicleName, string vehicleClass)
        {
            float safeZoneMargin = GetSafeZoneMargin();
            float rightX = 0.995f - safeZoneMargin;
            float classY = 0.928f - safeZoneMargin;
            float nameY = classY - 0.040f;

            Font titleFont = Font.ChaletComprimeCologne;
            switch (Game.Language.ToString())
            {
                case "Chinese":
                case "Korean":
                case "Japanese":
                case "ChineseSimplified":
                    titleFont = Font.ChaletLondon;
                    break;
            }

            DrawTextNormalized(vehicleName, rightX, nameY, 0.64f, titleFont, Color.White, true);
            DrawTextNormalized(vehicleClass, rightX, classY, 0.40f, Font.ChaletLondon, Color.DodgerBlue, true);
        }

        private static float GetSafeZoneMargin()
        {
            try
            {
                float safeZoneSize = Function.Call<float>(Hash.GET_SAFE_ZONE_SIZE);
                if (safeZoneSize <= 0f || safeZoneSize > 1f)
                {
                    return 0f;
                }

                return (1f - safeZoneSize) * 0.5f;
            }
            catch
            {
                return 0f;
            }
        }

        private static void DrawTextNormalized(string value, float x, float y, float scale, Font font, Color color, bool rightAligned = false)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            Function.Call(Hash.SET_TEXT_FONT, (int)font);
            Function.Call(Hash.SET_TEXT_SCALE, 1.0f, scale);
            Function.Call(Hash.SET_TEXT_COLOUR, color.R, color.G, color.B, color.A);
            Function.Call(Hash.SET_TEXT_DROPSHADOW, 0, 0, 0, 0, 255);
            Function.Call(Hash.SET_TEXT_EDGE, 1, 0, 0, 0, 205);
            Function.Call(Hash.SET_TEXT_OUTLINE);

            if (rightAligned)
            {
                Function.Call(Hash.SET_TEXT_JUSTIFICATION, 2);
                Function.Call(Hash.SET_TEXT_WRAP, 0.0f, x);
                Function.Call(Hash.SET_TEXT_RIGHT_JUSTIFY, true);
            }
            else
            {
                Function.Call(Hash.SET_TEXT_JUSTIFICATION, 1);
                Function.Call(Hash.SET_TEXT_WRAP, x, 1.0f);
                Function.Call(Hash.SET_TEXT_RIGHT_JUSTIFY, false);
            }

            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, value);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0);
        }

        public static void InstallModKit(this Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return;
            }

            vehicle.Mods.InstallModKit();
        }

        public static int GetMod(this Vehicle vehicle, VehicleMod modType)
        {
            try
            {
                if (vehicle == null)
                {
                    return -1;
                }

                return vehicle.Mods[modType].Index;
            }
            catch
            {
                return -1;
            }
        }

        public static int GetModCount(this Vehicle vehicle, VehicleMod modType)
        {
            try
            {
                if (vehicle == null)
                {
                    return 0;
                }

                return vehicle.Mods[modType].Count;
            }
            catch
            {
                return 0;
            }
        }

        public static void ToggleMod(this Vehicle vehicle, VehicleToggleMod modType, bool enabled)
        {
            try
            {
                if (vehicle != null)
                {
                    vehicle.Mods[modType].IsInstalled = enabled;
                }
            }
            catch
            {
            }
        }

        public static bool IsToggleModOn(this Vehicle vehicle, VehicleToggleMod modType)
        {
            try
            {
                return vehicle != null && vehicle.Mods[modType].IsInstalled;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsNeonLightsOn(this Vehicle vehicle, VehicleNeonLight light)
        {
            if (vehicle == null)
            {
                return false;
            }

            return Function.Call<bool>(Hash.GET_VEHICLE_NEON_ENABLED, vehicle.Handle, (int)light);
        }

        public static void OpenDoor(this Vehicle vehicle, int doorIndex, bool loose, bool instantly)
        {
            if (vehicle == null)
            {
                return;
            }

            Function.Call(Hash.SET_VEHICLE_DOOR_OPEN, vehicle.Handle, doorIndex, loose, instantly);
        }

        public static void OpenDoor(this Vehicle vehicle, VehicleDoorIndex doorIndex, bool loose, bool instantly)
        {
            OpenDoor(vehicle, (int)doorIndex, loose, instantly);
        }

        public static void CloseDoor(this Vehicle vehicle, int doorIndex, bool instantly)
        {
            if (vehicle == null)
            {
                return;
            }

            Function.Call(Hash.SET_VEHICLE_DOOR_SHUT, vehicle.Handle, doorIndex, instantly);
        }

        public static void CloseDoor(this Vehicle vehicle, VehicleDoorIndex doorIndex, bool instantly)
        {
            CloseDoor(vehicle, (int)doorIndex, instantly);
        }

        public static Vehicle CreateVehicle(string VehicleModel, int VehicleHash, Vector3 Position, float Heading = 0)
        {
            Vehicle Result = null;
            if (VehicleModel == "")
            {
                var model = new Model(VehicleHash);
                model.Request(250);
                if (model.IsInCdImage && model.IsValid)
                {
                    while (!model.IsLoaded)
                    {
                        Script.Wait(50);
                    }
                    Result = WorldCreateVehicle(model, Position, Heading);
                }
                model.MarkAsNoLongerNeeded();
            }
            else
            {
                var model = new Model(VehicleModel);
                model.Request(250);
                if (model.IsInCdImage && model.IsValid)
                {
                    while (!model.IsLoaded)
                    {
                        Script.Wait(50);
                    }
                    Result = WorldCreateVehicle(model, Position, Heading);
                }
                model.MarkAsNoLongerNeeded();
            }
            return Result;
        }

        public static Vehicle WorldCreateVehicle(Model model, Vector3 position, float heading = 0F)
        {
            if (!model.IsVehicle || !model.Request(1000))
            {
                return null;
            }

            return World.CreateVehicle(model, position, heading);
        }

        public static void LoadMPDLCMap()
        {
            try
            {
                Function.Call((Hash)0x0888C3502DBBEEF5UL);
            }
            catch (Exception ex)
            {
                Logger.Log("LoadMPDLCMap: failed to load MP DLC maps. " + ex.Message + " " + ex.StackTrace);
            }

            LoadMPDLCMapMissingObjects();
        }

        public static void LoadMPDLCMapMissingObjects()
        {
            int TID2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1155.31005, -1518.5699, 10.6300001); //Floyd Apartment
            int MID = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -802.31097, 175.05599, 72.84459); //Michael House
            int FID1 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -9.96562, -1438.54003, 31.101499); //Franklin Aunt House
            int FID2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 0.91675, 528.48498, 174.628005); //Franklin House

            int WODID = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -172.983001, 494.032989, 137.654006); //3655 Wild Oats
            int NCAID1 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 340.941009, 437.17999, 149.389999); //2044 North Conker
            int NCAID2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 373.0230102, 416.1050109, 145.70100402); //2045 North Conker
            int HCAID1 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -676.1270141, 588.6119995, 145.16999816); //2862 Hillcrest Avenue
            int HCAID2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -763.10699462, 615.90600585, 144.139999); //2868 Hillcrest Avenue
            int HCAID3 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -857.79797363, 682.56298828, 152.6529998); //2874 Hillcrest Avenue
            int MRID = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -572.60998535, 653.13000488, 145.63000488); //2117 Milton Road
            int WMDID = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 120.5, 549.952026367, 184.09700012207); //3677 Whispymound Drive
            int MWTDID = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1288, 440.74798583, 97.694602966); //2113 Mad Wayne Thunder Drive

            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, FID1, "V_57_FranklinStuff");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, TID2, "swap_clean_apt");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, TID2, "layer_whiskey");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, TID2, "layer_sextoys_a");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, TID2, "swap_mrJam_A");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, TID2, "swap_sofa_A");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MID, "V_Michael_bed_tidy");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MID, "V_Michael_L_Items");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MID, "V_Michael_S_Items");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MID, "V_Michael_D_Items");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MID, "V_Michael_M_Items");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MID, "Michael_premier");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MID, "V_Michael_plane_ticket");
            //Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, FID2, "showhome_only")
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, FID2, "franklin_settled");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, FID2, "franklin_unpacking");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, FID2, "bong_and_wine");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, FID2, "progress_flyer");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, FID2, "progress_tshirt");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, FID2, "progress_tux");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, FID2, "unlocked");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, WODID, "Stilts_Kitchen_Window");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, NCAID1, "Stilts_Kitchen_Window");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, NCAID2, "Stilts_Kitchen_Window");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, HCAID1, "Stilts_Kitchen_Window");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, HCAID2, "Stilts_Kitchen_Window");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, HCAID3, "Stilts_Kitchen_Window");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MRID, "Stilts_Kitchen_Window");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, WMDID, "Stilts_Kitchen_Window");
            Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, MWTDID, "Stilts_Kitchen_Window");
            Function.Call(Hash.REFRESH_INTERIOR, FID1);
            Function.Call(Hash.REFRESH_INTERIOR, TID2);
            Function.Call(Hash.REFRESH_INTERIOR, MID);
            Function.Call(Hash.REFRESH_INTERIOR, FID2);
            Function.Call(Hash.REFRESH_INTERIOR, WODID);
            Function.Call(Hash.REFRESH_INTERIOR, NCAID1);
            Function.Call(Hash.REFRESH_INTERIOR, NCAID2);
            Function.Call(Hash.REFRESH_INTERIOR, HCAID1);
            Function.Call(Hash.REFRESH_INTERIOR, HCAID2);
            Function.Call(Hash.REFRESH_INTERIOR, HCAID3);
            Function.Call(Hash.REFRESH_INTERIOR, MRID);
            Function.Call(Hash.REFRESH_INTERIOR, WMDID);
            Function.Call(Hash.REFRESH_INTERIOR, MWTDID);
        }

        public static void DisplayHelpTextThisFrame(string helpText, int Shape = -1)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
            const int maxStringLength = 99;

            int i = 0;
            while (i < helpText.Length)
            {
                Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, System.Math.Min(maxStringLength, helpText.Length - i)));
                i += maxStringLength;
            }
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, false, true, Shape);
        }

        public static int GetInteriorID(Vector3 interior)
        {
            return Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, interior.X, interior.Y, interior.Z);
        }

        public static Model LowriderUpgrade(Model model)
        {
            if (lowriderUpgrades.TryGetValue(model, out Tuple<Model, int> upgrade))
            {
                return upgrade.Item1;
            }

            return model;
        }

        public static bool TryGetLowriderBaseModel(Model upgradedModel, out Model baseModel)
        {
            foreach (Model candidate in lowriders)
            {
                if (LowriderUpgrade(candidate) == upgradedModel)
                {
                    baseModel = candidate;
                    return true;
                }
            }

            baseModel = upgradedModel;
            return false;
        }

        public enum ScreenEffect
        {
            SwitchHudIn,
            SwitchHudOut,
            FocusIn,
            FocusOut,
            MinigameEndNeutral,
            MinigameEndTrevor,
            MinigameEndFranklin,
            MinigameEndMichael,
            MinigameTransitionOut,
            MinigameTransitionIn,
            SwitchShortNeutralIn,
            SwitchShortFranklinIn,
            SwitchShortTrevorIn,
            SwitchShortMichaelIn,
            SwitchOpenMichaelIn,
            SwitchOpenFranklinIn,
            SwitchOpenTrevorIn,
            SwitchHudMichaelOut,
            SwitchHudFranklinOut,
            SwitchHudTrevorOut,
            SwitchShortFranklinMid,
            SwitchShortMichaelMid,
            SwitchShortTrevorMid,
            DeathFailOut,
            CamPushInNeutral,
            CamPushInFranklin,
            CamPushInMichael,
            CamPushInTrevor,
            SwitchSceneFranklin,
            SwitchSceneTrevor,
            SwitchSceneMichael,
            SwitchSceneNeutral,
            MpCelebWin,
            MpCelebWinOut,
            MpCelebLose,
            MpCelebLoseOut,
            DeathFailNeutralIn,
            DeathFailMpDark,
            DeathFailMpIn,
            MpCelebPreloadFade,
            PeyoteEndOut,
            PeyoteEndIn,
            PeyoteIn,
            PeyoteOut,
            MpRaceCrash,
            SuccessFranklin,
            SuccessTrevor,
            SuccessMichael,
            DrugsMichaelAliensFightIn,
            DrugsMichaelAliensFight,
            DrugsMichaelAliensFightOut,
            DrugsTrevorClownsFightIn,
            DrugsTrevorClownsFight,
            DrugsTrevorClownsFightOut,
            HeistCelebPass,
            HeistCelebPassBw,
            HeistCelebEnd,
            HeistCelebToast,
            MenuMgHeistIn,
            MenuMgTournamentIn,
            MenuMgSelectionIn,
            ChopVision,
            DmtFlightIntro,
            DmtFlight,
            DrugsDrivingIn,
            DrugsDrivingOut,
            SwitchOpenNeutralFib5,
            HeistLocate,
            MpJobLoad,
            RaceTurbo,
            MpIntroLogo,
            HeistTripSkipFade,
            MenuMgHeistOut,
            MpCoronaSwitch,
            MenuMgSelectionTint,
            SuccessNeutral,
            ExplosionJosh3,
            SniperOverlay,
            RampageOut,
            Rampage,
            DontTazemeBro,
        }

        public static void ScreenEffectStart(ScreenEffect effectName, int duration = 0, bool looped = false)
        {
            Function.Call((Hash)0x2206BF9A37B7F724UL, Enum.GetName(typeof(ScreenEffect), effectName), duration, looped);
        }

        public static string LocalizedModTypeName(VehicleMod modType)
        {
            if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
            {
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
                Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
            }
            string cur = null;
            switch (modType)
            {
                case VehicleMod.FrontBumper:
                    cur = Gxt("CMOD_MOD_BUMF");
                    break;
                case VehicleMod.RearBumper:
                    cur = Gxt("CMOD_MOD_BUMR");
                    break;
                case VehicleMod.SideSkirt:
                    cur = Gxt("CMOD_MOD_SKI");
                    break;
                case VehicleMod.Armor:
                    cur = Gxt("CMOD_MOD_ARM");
                    break;
                case VehicleMod.Brakes:
                    cur = Gxt("CMOD_MOD_BRA");
                    break;
                case VehicleMod.Engine:
                    cur = Gxt("CMOD_MOD_ENG");
                    break;
                case VehicleMod.Suspension:
                    cur = Gxt("CMOD_MOD_SUS");
                    break;
                case VehicleMod.Transmission:
                    cur = Gxt("CMOD_MOD_TRN");
                    break;
                case VehicleMod.Horns:
                    cur = Gxt("CMOD_MOD_HRN");
                    break;
                case VehicleMod.FrontWheel:
                    if (!veh.Model.IsBike && veh.Model.IsBicycle)
                    {
                        cur = Gxt("CMOD_MOD_WHEM");
                        if (cur == "")
                        {
                            return "Wheels";
                        }
                    }
                    else
                    {
                        cur = Gxt("CMOD_WHE0_0");
                    }
                    break;
                case VehicleMod.RearWheel:
                    cur = Gxt("CMOD_WHE0_1");
                    break; //Bennys
                case VehicleMod.PlateHolder:
                    cur = Gxt("CMM_MOD_S0");
                    break;
                case VehicleMod.VanityPlates:
                    if (veh.Model == "elegy")
                    {
                        cur = Gxt("CMM_MOD_S40");
                    }
                    else if (arenavehicle.Contains(veh.Model))
                    {
                        cur = Gxt("collision_yzkcrh").ToLower().UppercaseFirstLetter(); //Rear Wibbles
                    }
                    else
                    {
                        cur = Gxt("CMM_MOD_S1");
                    }
                    break;
                case VehicleMod.TrimDesign:
                    if (veh.Model == "sultanrs")
                    {
                        cur = Gxt("CMM_MOD_S2b");
                    }
                    else
                    {
                        cur = Gxt("CMM_MOD_S2");
                    }
                    break;
                case VehicleMod.Ornaments:
                    if (arenavehicle.Contains(veh.Model))
                    {
                        cur = Gxt("CMM_MOD_S27");
                    }
                    else
                    {
                        cur = Gxt("CMM_MOD_S3");
                    }
                    break;
                case VehicleMod.Dashboard:
                    cur = Gxt("CMM_MOD_S4");
                    break;
                case VehicleMod.DialDesign:
                    cur = Gxt("CMM_MOD_S5");
                    break;
                case VehicleMod.DoorSpeakers:
                    cur = Gxt("CMM_MOD_S6");
                    break;
                case VehicleMod.Seats:
                    cur = Gxt("CMM_MOD_S7");
                    break;
                case VehicleMod.SteeringWheels:
                    cur = Gxt("CMM_MOD_S8");
                    break;
                case VehicleMod.ColumnShifterLevers:
                    cur = Gxt("CMM_MOD_S9");
                    break;
                case VehicleMod.Plaques:
                    if (arenavehicle.Contains(veh.Model))
                    {
                        cur = Gxt("collision_8itszix").ToLower().UppercaseFirstLetter(); //Decorations
                    }
                    else
                    {
                        cur = Gxt("CMM_MOD_S10");
                    }
                    break;
                case VehicleMod.Speakers:
                    if (arenavehicle.Contains(veh.Model))
                    {
                        cur = Gxt("MNU_WBAR"); //Wheelie Bar
                    }
                    else
                    {
                        cur = Gxt("CMM_MOD_S11");
                    }
                    break;
                case VehicleMod.Trunk:
                    cur = Gxt("CMM_MOD_S12");
                    break;
                case VehicleMod.Hydraulics:
                    cur = Gxt("CMM_MOD_S13");
                    break;
                case VehicleMod.EngineBlock:
                    cur = Gxt("CMM_MOD_S14");
                    break;
                case VehicleMod.AirFilter:
                    if (arenavehicle.Contains(veh.Model))
                    {
                        cur = Gxt("WT_BOOST");
                    }
                    else
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "sultanrs":
                            case "elegy":
                                cur = Gxt("CMM_MOD_S15b");
                                break;
                            default:
                                cur = Gxt("CMM_MOD_S15");
                                break;
                        }
                    }
                    break;
                case VehicleMod.Struts:
                    if (arenavehicle.Contains(veh.Model))
                    {
                        cur = Gxt("collision_64bkrs4"); //vertical jump
                    }
                    else
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "sultanrs":
                            case "banshee2":
                                cur = Gxt("CMM_MOD_S16b");
                                break;
                            default:
                                cur = Gxt("CMM_MOD_S16");
                                break;
                        }
                    }
                    break;
                case VehicleMod.ArchCover:
                    if (veh.Model == "sultanrs")
                    {
                        cur = Gxt("CMM_MOD_S17b");
                    }
                    else if (arenavehicle.Contains(veh.Model))
                    {
                        cur = Gxt("collision_h1pzbg").ToLower().UppercaseFirstLetter();
                    }
                    else
                    {
                        cur = Gxt("CMM_MOD_S17");
                    }
                    break;
                case VehicleMod.Aerials:
                    if (veh.Model == "sultanrs")
                    {
                        cur = Gxt("CMM_MOD_S18b");
                    }
                    else if (veh.Model == "btype3")
                    {
                        cur = Gxt("CMM_MOD_S18c");
                    }
                    else if (arenavehicle.Contains(veh.Model))
                    {
                        cur = Gxt("BLIP_320"); //spikes
                    }
                    else
                    {
                        cur = Gxt("CMM_MOD_S18");
                    }
                    break;
                case VehicleMod.Trim:
                    if (veh.Model == "sultanrs")
                    {
                        cur = Gxt("CMM_MOD_S19b");
                    }
                    else if (veh.Model == "btype3")
                    {
                        cur = Gxt("CMM_MOD_S19c");
                    }
                    else if (veh.Model == "virgo2")
                    {
                        cur = Gxt("CMM_MOD_S19d");
                    }
                    else if (arenavehicle.Contains(veh.Model))
                    {
                        cur = Gxt("collision_84p91l0").ToLower().UppercaseFirstLetter(); //blades
                    }
                    else
                    {
                        cur = Gxt("CMM_MOD_S19");
                    }
                    break;
                case VehicleMod.Tank:
                    if (veh.Model == "slamvan3")
                    {
                        cur = Gxt("CMM_MOD_S27");
                    }
                    else if (arenavehicle.Contains(veh.Model))
                    {
                        cur = Gxt("collision_6w0cd59");
                    }
                    else
                    {
                        cur = Gxt("CMM_MOD_S20");
                    }
                    break;
                case VehicleMod.Windows:
                    if (veh.Model == "btype3")
                    {
                        cur = Gxt("CMM_MOD_S21b");
                    }
                    else
                    {
                        cur = Gxt("CMM_MOD_S21");
                    }
                    break;
                case (VehicleMod)47:
                    if (veh.Model == "slamvan3")
                    {
                        cur = Gxt("SLVAN3_RDOOR");
                    }
                    else
                    {
                        cur = Gxt("CMM_MOD_S22");
                    }
                    break;
                case VehicleMod.Livery:
                    cur = Gxt("CMM_MOD_S23");
                    break; //I'm Not MentaL
                case VehicleMod.Fender:
                    if (veh.ClassType == VehicleClass.Motorcycles)
                    {
                        cur = Gxt("CMOD_SHIFTER_0");
                    }
                    else
                    {
                        cur = Gxt("CMOD_MOD_FEN");
                    }
                    break;
                case VehicleMod.Spoilers:
                    if (veh.ClassType == VehicleClass.Motorcycles)
                    {
                        if (veh.Model == "faggio3")
                        {
                            cur = Gxt("TOP_ANTENNA");
                        }
                        else
                        {
                            cur = Gxt("CMOD_MOD_BLT");
                        }
                    }
                    else
                    {
                        if (veh.Model == "btype3")
                        {
                            cur = Gxt("BT_SPARE2");
                        }
                        else
                        {
                            cur = Gxt("CMOD_MOD_SPO");
                        }
                    }
                    break;
                case VehicleMod.Frame:
                    if (veh.ClassType == VehicleClass.Motorcycles)
                    {
                        if (arenavehicle.Contains(veh.Model))
                        {
                            cur = Gxt("CMOD_ARMPL_N"); //Armor Plating
                        }
                        else
                        {
                            cur = Gxt("CMM_MOD_S14");
                        }
                    }
                    else if (arenavehicle.Contains(veh.Model))
                    {
                        cur = Gxt("CMOD_ARMPL_N"); //Armor Plating
                    }
                    else
                    {
                        if (veh.Model == "sultanrs")
                        {
                            cur = Gxt("TOP_CAGE");
                        }
                        else
                        {
                            cur = Gxt("CMOD_MOD_CHA");
                        }
                    }
                    break;
                case VehicleMod.Exhaust:
                    cur = Gxt("CMOD_MOD_MUF");
                    break;
                case VehicleMod.Grille:
                    switch (veh.Model.ToString().ToLowerInvariant())
                    {
                        case "avarus":
                            cur = Gxt("TOP_OIL");
                            break;
                        case "zr3802":
                            cur = Gxt("collision_832uimd"); //rear windshield
                            break;
                        default:
                            cur = Gxt("CMOD_MOD_GRL");
                            break;
                    }

                    break;
                case VehicleMod.Hood:
                    if (veh.ClassType == VehicleClass.Motorcycles)
                    {
                        cur = Gxt("CMM_MOD_S7");
                    }
                    else
                    {
                        cur = Gxt("CMOD_MOD_HOD");
                    }
                    break;
                case VehicleMod.Roof:
                    if (veh.ClassType == VehicleClass.Motorcycles)
                    {
                        if (veh.Model == "faggio3")
                        {
                            cur = Gxt("TOP_ANTENNAR");
                        }
                        else
                        {
                            cur = Gxt("CMOD_MOD_TNK");
                        }
                    }
                    else
                    {
                        if (veh.Model == "penetrator")
                        {
                            cur = Gxt("CMM_MOD_S43");
                        }
                        else if (veh.Model == "blazer4")
                        {
                            cur = Gxt("CMM_MOD_S17");
                        }
                        else if (arenavehicle.Contains(veh.Model))
                        {
                            cur = Gxt("CMOD_SEWEAP_N");
                        }
                        else
                        {
                            cur = Gxt("CMOD_MOD_ROF");
                        }
                    }
                    break;
                default:
                    cur = Function.Call<string>(Hash.GET_MOD_SLOT_NAME, veh.Handle, (int)modType);
                    if (DoesGXTEntryExist(cur))
                    {
                        cur = Gxt(cur);
                    }
                    break;
            }
            if (cur == "")
            {
                //would only happen if the text isnt loaded
                cur = $"*{Enum.GetName(typeof(VehicleMod), modType)}";
            }

            return cur;
        }

        //Public Function LocalizeModTitleName(title As String) As String
        //    Dim langConf As ScriptSettings = ScriptSettings.Load("scripts\BennysLang-" & Game.Language.ToString & ".ini")
        //    return langConf.GetValue("TITLE", title, "NULL")
        //End Function

        public enum GroupName
        {
            NeonKits,
            NeonLayout,
            NeonColor,
            Headlights,
            Lights,
            Bumpers,
            Respray,
            Extras,
            Plate,
            License,
            Tires,
            WheelColor,
            WheelType,
            Turbo,
            Wheels,
            Windows,
            Upgrade,
            Upgrade2,
            Door,
            Bodyworks,
            Interior,
            Plates,
            Engine,
            PrimaryColor,
            SecondaryColor,
            LightColor,
            TertiaryColor,
            TrimColor,
            AccentColor,
            Repair,
            Livery,
            Weapons,
        }

        public static string LocalizedModGroupName(GroupName groupName)
        {
            if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
            {
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
                Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
            }
            string cur = null;
            switch (groupName)
            {
                case GroupName.NeonKits:
                    cur = Gxt("CMOD_MOD_LGT_N");
                    break;
                case GroupName.Headlights:
                    cur = Gxt("CMOD_MOD_LGT_H");
                    break;
                case GroupName.Lights:
                    cur = Gxt("CMOD_MOD_LGT");
                    break;
                case GroupName.Bumpers:
                    if (veh.Model == "blazer4")
                    {
                        cur = Gxt("TOP_MUDFR");
                    }
                    else
                    {
                        cur = Gxt("CMOD_MOD_BUM");
                    }
                    break;
                case GroupName.Respray:
                    cur = Gxt("CMOD_MOD_COL");
                    break;
                case GroupName.Extras:
                    cur = Gxt("CMOD_MOD_GLD2");
                    break;
                case GroupName.Plate:
                    cur = Gxt("CMOD_MOD_PLA");
                    break;
                case GroupName.License:
                    cur = Gxt("CMOD_MOD_PLA2");
                    break;
                case GroupName.Tires:
                    cur = Gxt("CMOD_MOD_TYR");
                    break;
                case GroupName.WheelColor:
                    cur = Gxt("CMOD_MOD_WCL");
                    break;
                case GroupName.Turbo:
                    cur = Gxt("CMOD_MOD_TUR");
                    break;
                case GroupName.Wheels:
                    cur = Gxt("CMOD_MOD_WHEM");
                    break;
                case GroupName.Windows:
                    cur = Gxt("CMOD_MOD_WIN");
                    break;
                case GroupName.Upgrade:
                    cur = Gxt("CMM_MOD_LOW");
                    break;
                case GroupName.Upgrade2:
                    cur = Gxt("collision_85z9vzf");
                    break;
                case GroupName.Door:
                    cur = Gxt("CMM_MOD_S21");
                    break;
                case GroupName.NeonLayout:
                    cur = Gxt("CMOD_NEON_0");
                    break;
                case GroupName.NeonColor:
                    cur = Gxt("CMOD_NEON_1");
                    break;
                case GroupName.Bodyworks:
                    cur = Gxt("CMM_MOD_BODY_W");
                    break;
                case GroupName.Interior:
                    cur = Gxt("CMM_MOD_G1");
                    break;
                case GroupName.Plates:
                    cur = Gxt("CMM_MOD_G2");
                    break;
                case GroupName.Engine:
                    cur = Gxt("CMM_MOD_G3");
                    break;
                case GroupName.PrimaryColor:
                    cur = Gxt("CMOD_COL0_0");
                    break;
                case GroupName.SecondaryColor:
                    cur = Gxt("CMOD_COL0_1");
                    break;
                case GroupName.LightColor:
                    cur = Gxt("CMM_MOD_S26");
                    break;
                case GroupName.Repair:
                    cur = Gxt("CMOD_MOD_MNT");
                    break;
                case GroupName.TertiaryColor:
                    cur = Gxt("CMOD_COL0_5");
                    break;
                case GroupName.TrimColor:
                    cur = Gxt("CMOD_MOD_TRIM2");
                    break;
                case GroupName.AccentColor:
                    cur = Gxt("CMOD_MOD_TRIM3");
                    break;
                case GroupName.WheelType:
                    cur = Gxt("CMOD_MOD_WHE");
                    break;
                case GroupName.Livery:
                    cur = Gxt("CMM_MOD_S23");
                    break;
                case GroupName.Weapons:
                    cur = Gxt("PM_INF_WEPT");
                    break;
            }

            return cur;
        }

        public enum ColorType
        {
            Chrome,
            Classic,
            Metallic,
            Metals,
            Matte,
            Pearlescent,
            Crew,
        }

        public static string LocalizedColorGroupName(ColorType colorTypeName)
        {
            if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
            {
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
                Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
            }
            string cur = null;
            switch (colorTypeName)
            {
                case ColorType.Chrome:
                    cur = Gxt("CMOD_COL1_0");
                    break;
                case ColorType.Classic:
                    cur = Gxt("CMOD_COL1_1");
                    break;
                case ColorType.Crew:
                    cur = Gxt("CMOD_COL1_2");
                    break;
                case ColorType.Metallic:
                    cur = Gxt("CMOD_COL1_3");
                    break;
                case ColorType.Metals:
                    cur = Gxt("CMOD_COL1_4");
                    break;
                case ColorType.Matte:
                    cur = Gxt("CMOD_COL1_5");
                    break;
                case ColorType.Pearlescent:
                    cur = Gxt("CMOD_COL1_6");
                    break;
            }
            return cur;
        }
        public static string LocalizedModTypeName(VehicleToggleMod toggleModType, bool stock = false)
        {
            string result = null;
            if (stock == true)
            {
                result = Gxt("CMOD_ARM_0");
            }
            else
            {
                //result = Function.Call(Of String)(Hash.GET_MOD_SLOT_NAME, veh.Handle, toggleModType)
                switch (toggleModType)
                {
                    case VehicleToggleMod.Turbo:
                        result = Gxt("CMOD_MOD_TUR");
                        break;
                    case VehicleToggleMod.XenonHeadlights:
                        result = Gxt("CMOD_LGT_1");
                        break;
                    case VehicleToggleMod.TireSmoke:
                        result = Gxt("CMOD_MOD_TYR3");
                        break;
                }
                if (result == "")
                {
                    //would only happen if the text isnt loaded
                    result = Enum.GetName(typeof(VehicleToggleMod), toggleModType);
                }
            }
            return result;
        }

        public static bool DoesGXTEntryExist(string entry)
        {
            return Function.Call<bool>(Hash.DOES_TEXT_LABEL_EXIST, entry);
        }

        public static string GetLocalizedModName(int index, int modCount, VehicleMod modType)
        {
            //this still needs a little more work, but its better than what it used to be
            if (modCount == 0)
            {
                return "";
            }
            if (index < -1 || index >= modCount)
            {
                return "";
            }
            if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
            {
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
                Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
            }
            string cur;
            if (modType == VehicleMod.Horns)
            {
                if (_hornNames.ContainsKey(index))
                {
                    if (DoesGXTEntryExist(_hornNames[index].Item1))
                    {
                        return Gxt(_hornNames[index].Item1);
                    }
                    return _hornNames[index].Item2;
                }
                return LocalizedModTypeName(modType) + " " + (index + 1).ToString();
            }
            if (modType == VehicleMod.FrontWheel || modType == VehicleMod.RearWheel)
            {
                if (index == -1)
                {
                    if (!veh.Model.IsBike && veh.Model.IsBicycle)
                    {
                        return Gxt("CMOD_WHE_0");
                    }
                    else
                    {
                        return Gxt("CMOD_WHE_B_0");
                    }
                }

                string wheelLabel = Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, veh.Handle, (int)modType, index);
                if (!string.IsNullOrWhiteSpace(wheelLabel) && DoesGXTEntryExist(wheelLabel))
                {
                    string localizedWheelName = Gxt(wheelLabel);
                    if (!string.IsNullOrWhiteSpace(localizedWheelName) && !localizedWheelName.Equals("NULL", StringComparison.OrdinalIgnoreCase))
                    {
                        return localizedWheelName;
                    }
                }

                return LocalizedModTypeName(modType) + " " + (index + 1).ToString();
            }

            switch (modType)
            {
                case VehicleMod.Armor:
                    return Gxt("CMOD_ARM_" + (index + 1).ToString());
                case VehicleMod.Brakes:
                    return Gxt("CMOD_BRA_" + (index + 1).ToString());
                case VehicleMod.Engine:
                    if (index == -1)
                    {
                        //Engine doesn't list anything in LSC for no parts, but there is a setting with no part. so just use armours none
                        return Gxt("CMOD_ARM_0");
                    }
                    return Gxt("CMOD_ENG_" + (index + 2).ToString());
                case VehicleMod.Suspension:
                    return Gxt("CMOD_SUS_" + (index + 1).ToString());
                case VehicleMod.Transmission:
                    return Gxt("CMOD_GBX_" + (index + 1).ToString());
            }
            if (index > -1)
            {
                cur = Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, veh.Handle, (int)modType, index);
                if (DoesGXTEntryExist(cur))
                {
                    cur = Gxt(cur);
                    if (cur == "" || cur == "NULL")
                    {
                        return LocalizedModTypeName(modType) + " " + (index + 1).ToString();
                    }
                    return cur;
                }
                return LocalizedModTypeName(modType) + " " + (index + 1).ToString();
            }
            else
            {
                switch (modType)
                {
                    case VehicleMod.AirFilter:
                        if (veh.Model == VehicleHash.Tornado)
                        {
                        }
                        break;
                    case VehicleMod.Struts:
                        if (veh.Model == VehicleHash.Banshee || veh.Model == VehicleHash.Banshee2 || veh.Model == VehicleHash.SultanRS)
                        {
                            return Gxt("CMOD_COL5_41");
                        }
                        break;

                }
                return Gxt("CMOD_DEF_0");
            }
        }

        public static string LocalizedLicensePlate(int plateType)
        {
            string result;
            string fallback;

            switch (plateType)
            {
                case 0:
                    result = Gxt("CMOD_PLA_0");
                    fallback = "Blue on White 2";
                    break;
                case 3:
                    result = Gxt("CMOD_PLA_1");
                    fallback = "Blue on White 1";
                    break;
                case 4:
                    result = Gxt("CMOD_PLA_2");
                    fallback = "Blue on White 3";
                    break;
                case 5:
                    result = Gxt("CMOD_MOD_GLD2");
                    fallback = "North Yankton";
                    break;
                case 1:
                    result = Gxt("CMOD_PLA_4");
                    fallback = "Yellow on Black";
                    break;
                case 2:
                    result = Gxt("CMOD_PLA_3");
                    fallback = "Yellow on Blue";
                    break;
                case 6:
                    result = Gxt("CMOD_PLA_6");
                    fallback = "eCola";
                    break;
                case 7:
                    result = Gxt("CMOD_PLA_7");
                    fallback = "Las Venturas";
                    break;
                case 8:
                    result = Gxt("CMOD_PLA_8");
                    fallback = "Liberty City";
                    break;
                case 9:
                    result = Gxt("CMOD_PLA_9");
                    fallback = "LS Car Meet";
                    break;
                case 10:
                    result = Gxt("CMOD_PLA_10");
                    fallback = "LS Panic";
                    break;
                case 11:
                    result = Gxt("CMOD_PLA_11");
                    fallback = "LS Pounders";
                    break;
                case 12:
                    result = Gxt("CMOD_PLA_12");
                    fallback = "Sprunk";
                    break;
                default:
                    result = null;
                    fallback = "License Plate " + plateType.ToString();
                    break;
            }

            return string.IsNullOrWhiteSpace(result) || result.Equals("NULL", StringComparison.OrdinalIgnoreCase)
                ? fallback
                : result;
        }

        public static string LocalizedT5RoofName(int roofID)
        {
            return Gxt("T5_ROOF" + roofID);
        }

        public static string LocalizedWindowsTint(GTA.VehicleWindowTint tint)
        {
            string result = null;

            switch (tint)
            {
                case VehicleWindowTint.DarkSmoke:
                    result = Gxt("CMOD_WIN_2");
                    break;
                case VehicleWindowTint.Green:
                    result = Gxt("GREEN");
                    break;
                case VehicleWindowTint.LightSmoke:
                    result = Gxt("CMOD_WIN_1");
                    break;
                case VehicleWindowTint.Limo:
                    result = Gxt("CMOD_WIN_3");
                    break;
                case VehicleWindowTint.None:
                    result = Gxt("CMOD_WIN_0");
                    break;
                case VehicleWindowTint.PureBlack:
                    result = Gxt("CMOD_WIN_5");
                    break;
                case VehicleWindowTint.Stock:
                    result = Gxt("CMOD_WIN_4");
                    break;
            }

            return result;
        }

        public static string GetLocalizedWheelTypeName(VehicleWheelType wheelType)
        {
            if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
            {
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
                Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
            }

            if (_wheelNames.ContainsKey(wheelType))
            {
                if (DoesGXTEntryExist(_wheelNames[wheelType].Item1))
                {
                    return Gxt(_wheelNames[wheelType].Item1);
                }

                return _wheelNames[wheelType].Item2;
            }

            throw new ArgumentException("Wheel Type == undefined", nameof(wheelType));
        }

        public static string GetLocalizedColorName(VehicleColor vehColor)
        {
            if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
            {
                Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
                Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
            }

            if (_colorNames.ContainsKey(vehColor))
            {
                if (DoesGXTEntryExist(_colorNames[vehColor].Item1))
                {
                    return Gxt(_colorNames[vehColor].Item1);
                }

                return System.Text.RegularExpressions.Regex.Replace(_colorNames[vehColor].Item2, "[A-Z]", " $0").Trim();
            }

            throw new ArgumentException("Vehicle Color == undefined", nameof(vehColor));
        }

        public static readonly List<VehicleColor> ClassicColor = new List<VehicleColor>
        {
            (VehicleColor)0, (VehicleColor)147, (VehicleColor)1, (VehicleColor)11, (VehicleColor)2, (VehicleColor)3, (VehicleColor)4, (VehicleColor)5, (VehicleColor)6, (VehicleColor)7, (VehicleColor)8, (VehicleColor)9, (VehicleColor)10, (VehicleColor)27, (VehicleColor)28, (VehicleColor)29, (VehicleColor)150, (VehicleColor)30, (VehicleColor)31, (VehicleColor)32, (VehicleColor)33, (VehicleColor)34, (VehicleColor)143, (VehicleColor)35, (VehicleColor)135, (VehicleColor)137, (VehicleColor)136, (VehicleColor)36, (VehicleColor)38, (VehicleColor)138, (VehicleColor)99, (VehicleColor)90, (VehicleColor)88, (VehicleColor)89, (VehicleColor)91, (VehicleColor)49, (VehicleColor)50, (VehicleColor)51, (VehicleColor)52, (VehicleColor)53, (VehicleColor)54, (VehicleColor)92, (VehicleColor)141, (VehicleColor)61, (VehicleColor)62, (VehicleColor)63, (VehicleColor)64, (VehicleColor)65, (VehicleColor)66, (VehicleColor)67, (VehicleColor)68, (VehicleColor)69, (VehicleColor)73, (VehicleColor)70, (VehicleColor)74, (VehicleColor)96, (VehicleColor)101, (VehicleColor)95, (VehicleColor)94, (VehicleColor)97, (VehicleColor)103, (VehicleColor)104, (VehicleColor)98, (VehicleColor)100, (VehicleColor)102, (VehicleColor)99, (VehicleColor)105, (VehicleColor)106, (VehicleColor)71, (VehicleColor)72, (VehicleColor)142, (VehicleColor)145, (VehicleColor)107, (VehicleColor)111, (VehicleColor)112
        };

        public static readonly List<VehicleColor> MatteColor = new List<VehicleColor>
        {
            (VehicleColor)12, (VehicleColor)13, (VehicleColor)14, (VehicleColor)131, (VehicleColor)83, (VehicleColor)82, (VehicleColor)84, (VehicleColor)149, (VehicleColor)148, (VehicleColor)39, (VehicleColor)40, (VehicleColor)41, (VehicleColor)42, (VehicleColor)55, (VehicleColor)128, (VehicleColor)151, (VehicleColor)155, (VehicleColor)152, (VehicleColor)153, (VehicleColor)154
        };

        public static readonly List<VehicleColor> MetalColor = new List<VehicleColor>
        {
            (VehicleColor)117, (VehicleColor)118, (VehicleColor)119, (VehicleColor)158, (VehicleColor)159, (VehicleColor)160
        };

        public static readonly List<VehicleColor> ChromeColor = new List<VehicleColor>
        {
            (VehicleColor)120
        };

        public static readonly List<VehicleColor> PearlescentColor = new List<VehicleColor>
        {
            (VehicleColor)0, (VehicleColor)147, (VehicleColor)1, (VehicleColor)11, (VehicleColor)2, (VehicleColor)3, (VehicleColor)4, (VehicleColor)5, (VehicleColor)6, (VehicleColor)7, (VehicleColor)8, (VehicleColor)9, (VehicleColor)10, (VehicleColor)27, (VehicleColor)28, (VehicleColor)29, (VehicleColor)150, (VehicleColor)30, (VehicleColor)31, (VehicleColor)32, (VehicleColor)33, (VehicleColor)34, (VehicleColor)143, (VehicleColor)35, (VehicleColor)135, (VehicleColor)137, (VehicleColor)136, (VehicleColor)36, (VehicleColor)38, (VehicleColor)138, (VehicleColor)99, (VehicleColor)90, (VehicleColor)88, (VehicleColor)89, (VehicleColor)91, (VehicleColor)49, (VehicleColor)50, (VehicleColor)51, (VehicleColor)52, (VehicleColor)53, (VehicleColor)54, (VehicleColor)92, (VehicleColor)141, (VehicleColor)61, (VehicleColor)62, (VehicleColor)63, (VehicleColor)64, (VehicleColor)65, (VehicleColor)66, (VehicleColor)67, (VehicleColor)68, (VehicleColor)69, (VehicleColor)73, (VehicleColor)70, (VehicleColor)74, (VehicleColor)96, (VehicleColor)101, (VehicleColor)95, (VehicleColor)94, (VehicleColor)97, (VehicleColor)103, (VehicleColor)104, (VehicleColor)98, (VehicleColor)100, (VehicleColor)102, (VehicleColor)99, (VehicleColor)105, (VehicleColor)106, (VehicleColor)71, (VehicleColor)72, (VehicleColor)142, (VehicleColor)145, (VehicleColor)107, (VehicleColor)111, (VehicleColor)112, (VehicleColor)117, (VehicleColor)118, (VehicleColor)119, (VehicleColor)158, (VehicleColor)159, (VehicleColor)160
        };

        private static readonly Dictionary<VehicleColor, Tuple<string, string>> _colorNames =
            new Dictionary<VehicleColor, Tuple<string, string>>
            {
                [(VehicleColor)0] = Tuple.Create("BLACK", "MetallicBlack"),
                [(VehicleColor)1] = Tuple.Create("GRAPHITE", "MetallicGraphiteBlack"),
                [(VehicleColor)2] = Tuple.Create("BLACK_STEEL", "MetallicBlackSteel"),
                [(VehicleColor)3] = Tuple.Create("DARK_SILVER", "MetallicDarkSilver"),
                [(VehicleColor)4] = Tuple.Create("SILVER", "MetallicSilver"),
                [(VehicleColor)5] = Tuple.Create("BLUE_SILVER", "MetallicBlueSilver"),
                [(VehicleColor)6] = Tuple.Create("ROLLED_STEEL", "MetallicSteelGray"),
                [(VehicleColor)7] = Tuple.Create("SHADOW_SILVER", "MetallicShadowSilver"),
                [(VehicleColor)8] = Tuple.Create("STONE_SILVER", "MetallicStoneSilver"),
                [(VehicleColor)9] = Tuple.Create("MIDNIGHT_SILVER", "MetallicMidnightSilver"),
                [(VehicleColor)10] = Tuple.Create("CAST_IRON_SIL", "MetallicGunMetal"),
                [(VehicleColor)11] = Tuple.Create("ANTHR_BLACK", "MetallicAnthraciteGray"),
                [(VehicleColor)12] = Tuple.Create("BLACK", "MatteBlack"),
                [(VehicleColor)13] = Tuple.Create("GREY", "MatteGray"),
                [(VehicleColor)14] = Tuple.Create("LIGHT_GREY", "MatteLightGray"),
                [(VehicleColor)15] = Tuple.Create("BLACK", "UtilBlack"),
                [(VehicleColor)16] = Tuple.Create("BLACK", "UtilBlackPoly"),
                [(VehicleColor)17] = Tuple.Create("DARK_SILVER", "UtilDarksilver"),
                [(VehicleColor)18] = Tuple.Create("SILVER", "UtilSilver"),
                [(VehicleColor)19] = Tuple.Create("CAST_IRON_SIL", "UtilGunMetal"),
                [(VehicleColor)20] = Tuple.Create("SHADOW_SILVER", "UtilShadowSilver"),
                [(VehicleColor)21] = Tuple.Create("BLACK", "WornBlack"),
                [(VehicleColor)22] = Tuple.Create("GRAPHITE", "WornGraphite"),
                [(VehicleColor)23] = Tuple.Create("ROLLED_STEEL", "WornSilverGray"),
                [(VehicleColor)24] = Tuple.Create("SILVER", "WornSilver"),
                [(VehicleColor)25] = Tuple.Create("BLUE_SILVER", "WornBlueSilver"),
                [(VehicleColor)26] = Tuple.Create("SHADOW_SILVER", "WornShadowSilver"),
                [(VehicleColor)27] = Tuple.Create("RED", "MetallicRed"),
                [(VehicleColor)28] = Tuple.Create("TORINO_RED", "MetallicTorinoRed"),
                [(VehicleColor)29] = Tuple.Create("FORMULA_RED", "MetallicFormulaRed"),
                [(VehicleColor)30] = Tuple.Create("BLAZE_RED", "MetallicBlazeRed"),
                [(VehicleColor)31] = Tuple.Create("GRACE_RED", "MetallicGracefulRed"),
                [(VehicleColor)32] = Tuple.Create("GARNET_RED", "MetallicGarnetRed"),
                [(VehicleColor)33] = Tuple.Create("SUNSET_RED", "MetallicDesertRed"),
                [(VehicleColor)34] = Tuple.Create("CABERNET_RED", "MetallicCabernetRed"),
                [(VehicleColor)35] = Tuple.Create("CANDY_RED", "MetallicCandyRed"),
                [(VehicleColor)36] = Tuple.Create("SUNRISE_ORANGE", "MetallicSunriseOrange"),
                [(VehicleColor)37] = Tuple.Create("GOLD", "MetallicClassicGold"),
                [(VehicleColor)38] = Tuple.Create("ORANGE", "MetallicOrange"),
                [(VehicleColor)39] = Tuple.Create("RED", "MatteRed"),
                [(VehicleColor)40] = Tuple.Create("DARK_RED", "MatteDarkRed"),
                [(VehicleColor)41] = Tuple.Create("ORANGE", "MatteOrange"),
                [(VehicleColor)42] = Tuple.Create("YELLOW", "MatteYellow"),
                [(VehicleColor)43] = Tuple.Create("RED", "UtilRed"),
                [(VehicleColor)44] = Tuple.Create("NULL", "UtilBrightRed"),
                [(VehicleColor)45] = Tuple.Create("GARNET_RED", "UtilGarnetRed"),
                [(VehicleColor)46] = Tuple.Create("RED", "WornRed"),
                [(VehicleColor)47] = Tuple.Create("NULL", "WornGoldenRed"),
                [(VehicleColor)48] = Tuple.Create("DARK_RED", "WornDarkRed"),
                [(VehicleColor)49] = Tuple.Create("DARK_GREEN", "MetallicDarkGreen"),
                [(VehicleColor)50] = Tuple.Create("RACING_GREEN", "MetallicRacingGreen"),
                [(VehicleColor)51] = Tuple.Create("SEA_GREEN", "MetallicSeaGreen"),
                [(VehicleColor)52] = Tuple.Create("OLIVE_GREEN", "MetallicOliveGreen"),
                [(VehicleColor)53] = Tuple.Create("BRIGHT_GREEN", "MetallicGreen"),
                [(VehicleColor)54] = Tuple.Create("PETROL_GREEN", "MetallicGasolineBlueGreen"),
                [(VehicleColor)55] = Tuple.Create("LIME_GREEN", "MatteLimeGreen"),
                [(VehicleColor)56] = Tuple.Create("DARK_GREEN", "UtilDarkGreen"),
                [(VehicleColor)57] = Tuple.Create("GREEN", "UtilGreen"),
                [(VehicleColor)58] = Tuple.Create("DARK_GREEN", "WornDarkGreen"),
                [(VehicleColor)59] = Tuple.Create("GREEN", "WornGreen"),
                [(VehicleColor)60] = Tuple.Create("NULL", "WornSeaWash"),
                [(VehicleColor)61] = Tuple.Create("GALAXY_BLUE", "MetallicMidnightBlue"),
                [(VehicleColor)62] = Tuple.Create("DARK_BLUE", "MetallicDarkBlue"),
                [(VehicleColor)63] = Tuple.Create("SAXON_BLUE", "MetallicSaxonyBlue"),
                [(VehicleColor)64] = Tuple.Create("BLUE", "MetallicBlue"),
                [(VehicleColor)65] = Tuple.Create("MARINER_BLUE", "MetallicMarinerBlue"),
                [(VehicleColor)66] = Tuple.Create("HARBOR_BLUE", "MetallicHarborBlue"),
                [(VehicleColor)67] = Tuple.Create("DIAMOND_BLUE", "MetallicDiamondBlue"),
                [(VehicleColor)68] = Tuple.Create("SURF_BLUE", "MetallicSurfBlue"),
                [(VehicleColor)69] = Tuple.Create("NAUTICAL_BLUE", "MetallicNauticalBlue"),
                [(VehicleColor)70] = Tuple.Create("ULTRA_BLUE", "MetallicBrightBlue"),
                [(VehicleColor)71] = Tuple.Create("PURPLE", "MetallicPurpleBlue"),
                [(VehicleColor)72] = Tuple.Create("SPIN_PURPLE", "MetallicSpinnakerBlue"),
                [(VehicleColor)73] = Tuple.Create("RACING_BLUE", "MetallicUltraBlue"),
                [(VehicleColor)74] = Tuple.Create("LIGHT_BLUE", "MetallicLightBlue"),
                [(VehicleColor)75] = Tuple.Create("DARK_BLUE", "UtilDarkBlue"),
                [(VehicleColor)76] = Tuple.Create("MIDNIGHT_BLUE", "UtilMidnightBlue"),
                [(VehicleColor)77] = Tuple.Create("BLUE", "UtilBlue"),
                [(VehicleColor)78] = Tuple.Create("NULL", "UtilSeaFoamBlue"),
                [(VehicleColor)79] = Tuple.Create("LIGHT_BLUE", "UtilLightningBlue"),
                [(VehicleColor)80] = Tuple.Create("NULL", "UtilMauiBluePoly"),
                [(VehicleColor)81] = Tuple.Create("NULL", "UtilBrightBlue"),
                [(VehicleColor)82] = Tuple.Create("DARK_BLUE", "MatteDarkBlue"),
                [(VehicleColor)83] = Tuple.Create("BLUE", "MatteBlue"),
                [(VehicleColor)84] = Tuple.Create("MIDNIGHT_BLUE", "MatteMidnightBlue"),
                [(VehicleColor)85] = Tuple.Create("DARK_BLUE", "WornDarkBlue"),
                [(VehicleColor)86] = Tuple.Create("BLUE", "WornBlue"),
                [(VehicleColor)87] = Tuple.Create("LIGHT_BLUE", "WornLightBlue"),
                [(VehicleColor)88] = Tuple.Create("YELLOW", "MetallicTaxiYellow"),
                [(VehicleColor)89] = Tuple.Create("RACE_YELLOW", "MetallicRaceYellow"),
                [(VehicleColor)90] = Tuple.Create("BRONZE", "MetallicBronze"),
                [(VehicleColor)91] = Tuple.Create("FLUR_YELLOW", "MetallicYellowBird"),
                [(VehicleColor)92] = Tuple.Create("LIME_GREEN", "MetallicLime"),
                [(VehicleColor)93] = Tuple.Create("NULL", "MetallicChampagne"),
                [(VehicleColor)94] = Tuple.Create("UMBER_BROWN", "MetallicPuebloBeige"),
                [(VehicleColor)95] = Tuple.Create("CREEK_BROWN", "MetallicDarkIvory"),
                [(VehicleColor)96] = Tuple.Create("CHOCOLATE_BROWN", "MetallicChocoBrown"),
                [(VehicleColor)97] = Tuple.Create("MAPLE_BROWN", "MetallicGoldenBrown"),
                [(VehicleColor)98] = Tuple.Create("SADDLE_BROWN", "MetallicLightBrown"),
                [(VehicleColor)99] = Tuple.Create("STRAW_BROWN", "MetallicStrawBeige"),
                [(VehicleColor)100] = Tuple.Create("MOSS_BROWN", "MetallicMossBrown"),
                [(VehicleColor)101] = Tuple.Create("BISON_BROWN", "MetallicBistonBrown"),
                [(VehicleColor)102] = Tuple.Create("WOODBEECH_BROWN", "MetallicBeechwood"),
                [(VehicleColor)103] = Tuple.Create("NULL", "MetallicDarkBeechwood"),
                [(VehicleColor)104] = Tuple.Create("SIENNA_BROWN", "MetallicChocoOrange"),
                [(VehicleColor)105] = Tuple.Create("SANDY_BROWN", "MetallicBeachSand"),
                [(VehicleColor)106] = Tuple.Create("BLEECHED_BROWN", "MetallicSunBleechedSand"),
                [(VehicleColor)107] = Tuple.Create("CREAM", "MetallicCream"),
                [(VehicleColor)108] = Tuple.Create("BROWN", "UtilBrown"),
                [(VehicleColor)109] = Tuple.Create("NULL", "UtilMediumBrown"),
                [(VehicleColor)110] = Tuple.Create("NULL", "UtilLightBrown"),
                [(VehicleColor)111] = Tuple.Create("WHITE", "MetallicWhite"),
                [(VehicleColor)112] = Tuple.Create("FROST_WHITE", "MetallicFrostWhite"),
                [(VehicleColor)113] = Tuple.Create("NULL", "WornHoneyBeige"),
                [(VehicleColor)114] = Tuple.Create("BROWN", "WornBrown"),
                [(VehicleColor)115] = Tuple.Create("DARK_BROWN", "WornDarkBrown"),
                [(VehicleColor)116] = Tuple.Create("STRAW_BROWN", "WornStrawBeige"),
                [(VehicleColor)117] = Tuple.Create("BR_STEEL", "BrushedSteel"),
                [(VehicleColor)118] = Tuple.Create("BR BLACK_STEEL", "BrushedBlackSteel"),
                [(VehicleColor)119] = Tuple.Create("BR_ALUMINIUM", "BrushedAluminium"),
                [(VehicleColor)120] = Tuple.Create("CHROME", "Chrome"),
                [(VehicleColor)121] = Tuple.Create("NULL", "WornOffWhite"),
                [(VehicleColor)122] = Tuple.Create("NULL", "UtilOffWhite"),
                [(VehicleColor)123] = Tuple.Create("ORANGE", "WornOrange"),
                [(VehicleColor)124] = Tuple.Create("NULL", "WornLightOrange"),
                [(VehicleColor)125] = Tuple.Create("NULL", "MetallicSecuricorGreen"),
                [(VehicleColor)126] = Tuple.Create("YELLOW", "WornTaxiYellow"),
                [(VehicleColor)127] = Tuple.Create("NULL", "PoliceCarBlue"),
                [(VehicleColor)128] = Tuple.Create("GREEN", "MatteGreen"),
                [(VehicleColor)129] = Tuple.Create("BROWN", "MatteBrown"),
                [(VehicleColor)130] = Tuple.Create("NULL", "SteelBlue"),
                [(VehicleColor)131] = Tuple.Create("WHITE", "MatteWhite"),
                [(VehicleColor)132] = Tuple.Create("WHITE", "WornWhite"),
                [(VehicleColor)133] = Tuple.Create("OLIVE_GREEN", "WornOliveArmyGreen"),
                [(VehicleColor)134] = Tuple.Create("WHITE", "PureWhite"),
                [(VehicleColor)135] = Tuple.Create("HOT PINK", "HotPink"),
                [(VehicleColor)136] = Tuple.Create("SALMON_PINK", "Salmonpink"),
                [(VehicleColor)137] = Tuple.Create("PINK", "MetallicVermillionPink"),
                [(VehicleColor)138] = Tuple.Create("BRIGHT_ORANGE", "Orange"),
                [(VehicleColor)139] = Tuple.Create("GREEN", "Green"),
                [(VehicleColor)140] = Tuple.Create("BLUE", "Blue"),
                [(VehicleColor)141] = Tuple.Create("MIDNIGHT_BLUE", "MettalicBlackBlue"),
                [(VehicleColor)142] = Tuple.Create("MIGHT_PURPLE", "MetallicBlackPurple"),
                [(VehicleColor)143] = Tuple.Create("WINE_RED", "MetallicBlackRed"),
                [(VehicleColor)144] = Tuple.Create("NULL", "HunterGreen"),
                [(VehicleColor)145] = Tuple.Create("BRIGHT_PURPLE", "MetallicPurple"),
                [(VehicleColor)146] = Tuple.Create("MIGHT_PURPLE", "MetaillicVDarkBlue"),
                [(VehicleColor)147] = Tuple.Create("BLACK_GRAPHITE", "ModshopBlack1"),
                [(VehicleColor)148] = Tuple.Create("PURPLE", "MattePurple"),
                [(VehicleColor)149] = Tuple.Create("MIGHT_PURPLE", "MatteDarkPurple"),
                [(VehicleColor)150] = Tuple.Create("LAVA_RED", "MetallicLavaRed"),
                [(VehicleColor)151] = Tuple.Create("MATTE_FOR", "MatteForestGreen"),
                [(VehicleColor)152] = Tuple.Create("MATTE_OD", "MatteOliveDrab"),
                [(VehicleColor)153] = Tuple.Create("MATTE_DIRT", "MatteDesertBrown"),
                [(VehicleColor)154] = Tuple.Create("MATTE_DESERT", "MatteDesertTan"),
                [(VehicleColor)155] = Tuple.Create("MATTE_FOIL", "MatteFoliageGreen"),
                [(VehicleColor)156] = Tuple.Create("NULL", "DefaultAlloyColor"),
                [(VehicleColor)157] = Tuple.Create("NULL", "EpsilonBlue"),
                [(VehicleColor)158] = Tuple.Create("GOLD_P", "PureGold"),
                [(VehicleColor)159] = Tuple.Create("GOLD_S", "BrushedGold"),
                [(VehicleColor)160] = Tuple.Create("NULL", "SecretGold")
            };

        private static readonly Dictionary<int, Tuple<string, string>> _hornNames =
            new Dictionary<int, Tuple<string, string>>
            {
                [-1] = Tuple.Create("CMOD_HRN_0", "Stock Horn"),
                [0] = Tuple.Create("CMOD_HRN_TRK", "Truck Horn"),
                [1] = Tuple.Create("CMOD_HRN_COP", "Cop Horn"),
                [2] = Tuple.Create("CMOD_HRN_CLO", "Clown Horn"),
                [3] = Tuple.Create("CMOD_HRN_MUS1", "Musical Horn 1"),
                [4] = Tuple.Create("CMOD_HRN_MUS2", "Musical Horn 2"),
                [5] = Tuple.Create("CMOD_HRN_MUS3", "Musical Horn 3"),
                [6] = Tuple.Create("CMOD_HRN_MUS4", "Musical Horn 4"),
                [7] = Tuple.Create("CMOD_HRN_MUS5", "Musical Horn 5"),
                [8] = Tuple.Create("CMOD_HRN_SAD", "Sad Trombone"),
                [9] = Tuple.Create("HORN_CLAS1", "Classical Horn 1"),
                [10] = Tuple.Create("HORN_CLAS2", "Classical Horn 2"),
                [11] = Tuple.Create("HORN_CLAS3", "Classical Horn 3"),
                [12] = Tuple.Create("HORN_CLAS4", "Classical Horn 4"),
                [13] = Tuple.Create("HORN_CLAS5", "Classical Horn 5"),
                [14] = Tuple.Create("HORN_CLAS6", "Classical Horn 6"),
                [15] = Tuple.Create("HORN_CLAS7", "Classical Horn 7"),
                [16] = Tuple.Create("HORN_CNOTE_C0", "Scale Do"),
                [17] = Tuple.Create("HORN_CNOTE_D0", "Scale Re"),
                [18] = Tuple.Create("HORN_CNOTE_E0", "Scale Mi"),
                [19] = Tuple.Create("HORN_CNOTE_F0", "Scale Fa"),
                [20] = Tuple.Create("HORN_CNOTE_G0", "Scale Sol"),
                [21] = Tuple.Create("HORN_CNOTE_A0", "Scale La"),
                [22] = Tuple.Create("HORN_CNOTE_B0", "Scale Ti"),
                [23] = Tuple.Create("HORN_CNOTE_C1", "Scale Do (High)"),
                [24] = Tuple.Create("HORN_HIPS1", "Jazz Horn 1"),
                [25] = Tuple.Create("HORN_HIPS2", "Jazz Horn 2"),
                [26] = Tuple.Create("HORN_HIPS3", "Jazz Horn 3"),
                [27] = Tuple.Create("HORN_HIPS4", "Jazz Horn Loop"),
                [28] = Tuple.Create("HORN_INDI_1", "Star Spangled Banner 1"),
                [29] = Tuple.Create("HORN_INDI_2", "Star Spangled Banner 2"),
                [30] = Tuple.Create("HORN_INDI_3", "Star Spangled Banner 3"),
                [31] = Tuple.Create("HORN_INDI_4", "Star Spangled Banner 4"),
                [32] = Tuple.Create("HORN_LUXE2", "Classical Horn Loop 1"),
                [33] = Tuple.Create("HORN_LUXE1", "Classical Horn 8"),
                [34] = Tuple.Create("HORN_LUXE3", "Classical Horn Loop 2"),
                [35] = Tuple.Create("HORN_LUXE2", "Classical Horn Loop 1"),
                [36] = Tuple.Create("HORN_LUXE1", "Classical Horn 8"),
                [37] = Tuple.Create("HORN_LUXE3", "Classical Horn Loop 2"),
                [38] = Tuple.Create("HORN_HWEEN1", "Halloween Loop 1"),
                [39] = Tuple.Create("HORN_HWEEN1", "Halloween Loop 1"),
                [40] = Tuple.Create("HORN_HWEEN2", "Halloween Loop 2"),
                [41] = Tuple.Create("HORN_HWEEN2", "Halloween Loop 2"),
                [42] = Tuple.Create("HORN_LOWRDER1", "San Andreas Loop"),
                [43] = Tuple.Create("HORN_LOWRDER1", "San Andreas Loop"),
                [44] = Tuple.Create("HORN_LOWRDER2", "Liberty City Loop"),
                [45] = Tuple.Create("HORN_LOWRDER2", "Liberty City Loop"),
                [46] = Tuple.Create("HORN_XM15_1", "Festive Loop 1"),
                [47] = Tuple.Create("HORN_XM15_1", "Festive Loop 1"),
                [48] = Tuple.Create("HORN_XM15_2", "Festive Loop 2"),
                [49] = Tuple.Create("HORN_XM15_2", "Festive Loop 2"),
                [50] = Tuple.Create("HORN_XM15_3", "Festive Loop 3"),
                [51] = Tuple.Create("HORN_XM15_3", "Festive Loop 3"),
                [52] = Tuple.Create("CMOD_AIRHORN_01", "Airhorn 1"),
                [53] = Tuple.Create("CMOD_AIRHORN_01", "Airhorn 1"),
                [54] = Tuple.Create("CMOD_AIRHORN_02", "Airhorn 2"),
                [55] = Tuple.Create("CMOD_AIRHORN_02", "Airhorn 2"),
                [56] = Tuple.Create("CMOD_AIRHORN_03", "Airhorn 3"),
                [57] = Tuple.Create("CMOD_AIRHORN_03", "Airhorn 3")
            };

        private static readonly Dictionary<VehicleWheelType, Tuple<string, string>> _wheelNames =
            new Dictionary<VehicleWheelType, Tuple<string, string>>
            {
                [VehicleWheelType.BikeWheels] = Tuple.Create("CMOD_WHE1_0", "Bike"),
                [VehicleWheelType.HighEnd] = Tuple.Create("CMOD_WHE1_1", "High End"),
                [VehicleWheelType.Lowrider] = Tuple.Create("CMOD_WHE1_2", "Lowrider"),
                [VehicleWheelType.Muscle] = Tuple.Create("CMOD_WHE1_3", "Muscle"),
                [VehicleWheelType.Offroad] = Tuple.Create("CMOD_WHE1_4", "Offroad"),
                [VehicleWheelType.Sport] = Tuple.Create("CMOD_WHE1_5", "Sport"),
                [VehicleWheelType.SUV] = Tuple.Create("CMOD_WHE1_6", "SUV"),
                [VehicleWheelType.Tuner] = Tuple.Create("CMOD_WHE1_7", "Tuner"),
                [(VehicleWheelType)8] = Tuple.Create("CMOD_WHE1_8", "Benny's Originals"),
                [(VehicleWheelType)9] = Tuple.Create("CMOD_WHE1_9", "Benny's Bespoke"),
                [(VehicleWheelType)10] = Tuple.Create("CMOD_WHE1_10", "Racing"),
                [(VehicleWheelType)11] = Tuple.Create("CMOD_WHE1_11", "Street")
            };

        public static bool IsCustomWheels()
        {
            return Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, veh, VehicleMod.FrontWheel);
        }

        internal enum EnumTypes
        {
            NumberPlateType,
            VehicleColorPrimary,
            VehicleColorSecondary,
            VehicleColorTrim,
            VehicleColorDashboard,
            VehicleColorRim,
            VehicleColorAccent,
            vehicleColorPearlescent,
            VehicleWindowTint
        }

        public enum NeonLayouts
        {
            None,
            Sides = 3,
            Front,
            FrontAndSides = 7,
            Back,
            BackAndSides = 11,
            FrontAndBack,
            FrontBackAndSides = 15
        }

        public static NeonLayouts NeonLayout()
        {
            Vehicle v = veh;
            bool back = v.IsNeonLightsOn(VehicleNeonLight.Back);
            bool front = v.IsNeonLightsOn(VehicleNeonLight.Front);
            bool left = v.IsNeonLightsOn(VehicleNeonLight.Left);
            bool right = v.IsNeonLightsOn(VehicleNeonLight.Right);
            NeonLayouts result = NeonLayouts.None;

            if (!back && !front && !left && !right)
            {
                result = NeonLayouts.None;
            }
            else if (!back && front && !left && !right)
            {
                result = NeonLayouts.Front;
            }
            else if (back && !front && !left && !right)
            {
                result = NeonLayouts.Back;
            }
            else if (!back && !front && left && right)
            {
                result = NeonLayouts.Sides;
            }
            else if (back && front && !left && !right)
            {
                result = NeonLayouts.FrontAndBack;
            }
            else if (!back && front && left && right)
            {
                result = NeonLayouts.FrontAndSides;
            }
            else if (back && !front && left && right)
            {
                result = NeonLayouts.BackAndSides;
            }
            else if (back && front && left && right)
            {
                result = NeonLayouts.FrontBackAndSides;
            }

            return result;
        }

        public static string GetClassDisplayName(VehicleClass vehicleClass)
        {
            return Gxt("VEH_CLASS_" + Convert.ToInt32(vehicleClass));
        }

        public static bool IsUpgradeModExist(this string vehDispName)
        {
            ScriptSettings config = ScriptSettings.Load("scripts\\BennysMotorworksRevamped.ini");
            string v = config.GetValue<string>("UPGRADE", vehDispName.ToLower() + "_Model", null);
            return v != null;
        }

        public static Tuple<string, int> GetUpgradeModVehicleInfo(this string vehDispName)
        {
            ScriptSettings config = ScriptSettings.Load("scripts\\BennysMotorworksRevamped.ini");
            string newModel = config.GetValue<string>("UPGRADE", vehDispName.ToLower() + "_Model", null);
            int newPrice = config.GetValue<int>("UPGRADE", vehDispName.ToLower() + "_Price", 0);
            return Tuple.Create(newModel, newPrice);
        }

        public static int GetRepairPrice(this Vehicle vehicle)
        {
            int price = (int)Math.Round((double)(vehicle.MaxHealth - vehicle.Health)) * 10;
            if (price == 0)
            {
                price = 1;
            }
            return price;
        }

        public static int GetUpgradePrice(this Model vehicleModel)
        {
            if (lowriderUpgrades.TryGetValue(vehicleModel, out Tuple<Model, int> upgrade))
            {
                return upgrade.Item2;
            }

            return 0;
        }

        public static void PlaySpeech(string speechName)
        {
            if (string.IsNullOrEmpty(speechName))
            {
                speechName = "LR_UPGRADE_GENERIC";
            }
            Function.Call((Hash)0x3523634255FC3318UL, bennyPed, speechName, "BENNY", "SPEECH_PARAMS_FORCE_SHOUTED", 0);
        }

        public static void SetLivery2(this Vehicle veh, int liv)
        {
            Function.Call((Hash)0xA6D3A8750DC73270UL, veh.Handle, liv);
        }

        public static int GetLivery2(this Vehicle veh)
        {
            return Function.Call<int>((Hash)0x60190048C0764A26UL, veh.Handle);
        }

        public static int Livery2Count(this Vehicle veh)
        {
            return Function.Call<int>((Hash)0x5ECB40269053C0D4UL, veh.Handle);
        }

        public static int GetBennysOriginalRim(int curRim)
        {
            int totalWheelsCount = veh.GetModCount(VehicleMod.FrontWheel);
            int howMany = totalWheelsCount / 7;
            return curRim <= howMany ? curRim : curRim % 31;
        }

        public static bool CanEnterBennysMotorwork(Vehicle veh)
        {
            return Function.Call<bool>((Hash)0x8D474C8FAEFF6CDEUL, veh);
        }

        public static bool IsVehicleAttachedToTrailer(this Vehicle veh)
        {
            return Function.Call<bool>(Hash.IS_VEHICLE_ATTACHED_TO_TRAILER, veh);
        }

        public static Vehicle GetVehicleTrailerVehicle(this Vehicle veh)
        {
            OutputArgument arg = new OutputArgument();
            Function.Call(Hash.GET_VEHICLE_TRAILER_VEHICLE, veh, arg);
            return arg.GetResult<Vehicle>();
        }

        public enum EngineLoc
        {
            front,
            rear,
            unk,
        }

        public static bool HasBone(this Vehicle veh, string boneName)
        {
            return Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, veh.Handle, boneName) != -1;
        }

        public static Vector3 GetBoneCoord(this Vehicle veh, string boneName)
        {
            int boneIndex = Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, veh.Handle, boneName);
            if (boneIndex == -1)
            {
                return veh.Position;
            }
            return Function.Call<Vector3>(Hash.GET_WORLD_POSITION_OF_ENTITY_BONE, veh.Handle, boneIndex);
        }

        public static float GetVehicleEnginePositionSingle(Vehicle veh)
        {
            Vector3 lfwheel = veh.GetBoneCoord("wheel_lf");
            Vector3 engine = veh.GetBoneCoord("engine");
            return Vector3.Distance(lfwheel, engine);
        }

        public static float GetVehicleHoodPositionSingle(Vehicle veh)
        {
            Vector3 lfwheel = veh.GetBoneCoord("wheel_lf");
            Vector3 bonnet = veh.GetBoneCoord("bonnet");
            return Vector3.Distance(lfwheel, bonnet);
        }

        public static float GetVehicleTrunkPositionSingle(Vehicle veh)
        {
            Vector3 lfwheel = veh.GetBoneCoord("wheel_lf");
            Vector3 boot = veh.GetBoneCoord("boot");
            return Vector3.Distance(lfwheel, boot);
        }

        public static EngineLoc GetVehEnginePos(this Vehicle veh)
        {
            float dist = Vector3.Distance(veh.GetBoneCoord("wheel_lf"), veh.GetBoneCoord("engine"));
            if (dist >= 0.0f && dist <= 1.9f)
            {
                return EngineLoc.front;
            }
            if (dist >= 2.0f && dist <= 5.0f)
            {
                return EngineLoc.rear;
            }
            return EngineLoc.unk;
        }

        public static EngineLoc GetVehHoodPos(this Vehicle veh)
        {
            float dist = Vector3.Distance(veh.GetBoneCoord("bonnet"), veh.GetBoneCoord("wheel_lf"));
            if (dist >= 0.0f && dist <= 1.69f)
            {
                return EngineLoc.front;
            }
            if (dist >= 1.7f && dist <= 5.0f)
            {
                return EngineLoc.rear;
            }
            return EngineLoc.unk;
        }

        public static EngineLoc GetVehTrunkPos(this Vehicle veh)
        {
            float dist = Vector3.Distance(veh.GetBoneCoord("boot"), veh.GetBoneCoord("wheel_lf"));
            if (dist >= 0.0f && dist <= 1.99f)
            {
                return EngineLoc.front;
            }
            if (dist >= 2.0f && dist <= 5.0f)
            {
                return EngineLoc.rear;
            }
            return EngineLoc.unk;
        }

        public static VehicleStats GetVehicleStats(Vehicle veh)
        {
            VehicleStats stats = new VehicleStats();
            stats.TopSpeed = ((Function.Call<float>((Hash)0x53AF99BAA671CA47UL, veh) * 3600f) / 1609.344f) * 1.9f;
            stats.Braking = Function.Call<float>(Hash.GET_VEHICLE_MAX_BRAKING, veh) * 70f;
            stats.Acceleration = (Function.Call<float>(Hash.GET_VEHICLE_ACCELERATION, veh) * 100f) * 4.4f;
            stats.Traction = Function.Call<float>(Hash.GET_VEHICLE_MAX_TRACTION, veh) * 6.5f;
            if (stats.TopSpeed >= 200f) stats.TopSpeed = 200f;
            if (stats.Braking >= 200f) stats.Braking = 200f;
            if (stats.Acceleration >= 200f) stats.Acceleration = 200f;
            if (stats.Traction >= 200f) stats.Traction = 200f;
            return stats;
        }

        public static void SetXenonHeadlightsColor(this Vehicle veh, int colorID, bool toggleXenon)
        {
            if (toggleXenon)
            {
                veh.ToggleMod(VehicleToggleMod.XenonHeadlights, true);
            }
            Function.Call((Hash)0xE41033B25D003A07UL, veh.Handle, colorID);
        }

        public static int GetXenonHeadlightsColor(this Vehicle veh)
        {
            return Function.Call<int>((Hash)0x3DFF319A831E0CDBUL, veh.Handle);
        }

        public static string Brand(this Vehicle veh)
        {
            return Gxt(Function.Call<string>((Hash)0xF7AF4F159FF99F97UL, veh.Model.Hash));
        }

        public static int GetHashKey(this string str)
        {
            return Function.Call<int>(Hash.GET_HASH_KEY, str);
        }

        public static bool IsArenaWarDLCInstalled()
        {
            return Function.Call<bool>(Hash.IS_DLC_PRESENT, "mpchristmas2018".GetHashKey());
        }

        public static void UpdateTitleCaption(this object menu, string newCaption, bool upper = false)
        {
            if (menu == null)
            {
                return;
            }
        }

        public static void UpdateTitleCaption(this object menu, VehicleWheelType wheeltype, bool upper = false)
        {
            if (menu == null)
            {
                return;
            }
        }

        public static Point GetUIMenuOffset(this UIMenu menu)
        {
            return Point.Empty;
        }

        public static string UppercaseFirstLetter(this string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return val;
            }
            char[] array = val.ToCharArray();
            array[0] = char.ToUpper(array[0]);
            return new string(array);
        }

        public static void HoodCamera()
        {
            if (veh.HasBone("bonnet") && veh.HasBone("boot"))
            {
                if (veh.GetVehEnginePos() == EngineLoc.front)
                {
                    if (veh.GetVehHoodPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.Hood;
                    }
                    else if (veh.GetVehHoodPos() == EngineLoc.rear)
                    {
                        camera.MainCameraPosition = CameraPosition.RearHood;
                    }
                }
                else if (veh.GetVehEnginePos() == EngineLoc.rear)
                {
                    if (veh.GetVehHoodPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.Hood;
                    }
                    else if (veh.GetVehHoodPos() == EngineLoc.rear)
                    {
                        if (veh.GetVehTrunkPos() == EngineLoc.front)
                        {
                            camera.MainCameraPosition = CameraPosition.FrontTrunk;
                        }
                        else
                        {
                            camera.MainCameraPosition = CameraPosition.FrontBumper;
                        }
                    }
                }
            }
            else if (veh.HasBone("bonnet"))
            {
                if (veh.GetVehEnginePos() == EngineLoc.front)
                {
                    if (veh.GetVehHoodPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.Hood;
                    }
                    else if (veh.GetVehHoodPos() == EngineLoc.rear)
                    {
                        camera.MainCameraPosition = CameraPosition.RearHood;
                    }
                }
                else if (veh.GetVehEnginePos() == EngineLoc.rear)
                {
                    if (veh.GetVehHoodPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.Hood;
                    }
                    else if (veh.GetVehHoodPos() == EngineLoc.rear)
                    {
                        camera.MainCameraPosition = CameraPosition.FrontBumper;
                    }
                }
            }
            else if (veh.HasBone("boot"))
            {
                if (veh.GetVehEnginePos() == EngineLoc.front)
                {
                    if (veh.GetVehTrunkPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.FrontTrunk;
                    }
                    else if (veh.GetVehTrunkPos() == EngineLoc.rear)
                    {
                        camera.MainCameraPosition = CameraPosition.FrontBumper;
                    }
                }
                else if (veh.GetVehEnginePos() == EngineLoc.rear)
                {
                    if (veh.GetVehTrunkPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.FrontTrunk;
                    }
                    else if (veh.GetVehTrunkPos() == EngineLoc.rear)
                    {
                        camera.MainCameraPosition = CameraPosition.FrontBumper;
                    }
                }
            }
            else
            {
                if (veh.GetVehEnginePos() == EngineLoc.rear)
                {
                    camera.MainCameraPosition = CameraPosition.FrontBumper;
                }
                else if (veh.GetVehEnginePos() == EngineLoc.front)
                {
                    camera.MainCameraPosition = CameraPosition.Engine;
                }
            }
        }

        public static void HoodCamera(bool opendoor)
        {
            switch (veh.Model.ToString().ToLowerInvariant())
            {
                case "monster3":
                case "monster4":
                case "monster5":
                    camera.MainCameraPosition = CameraPosition.Car;
                    return;
                default:
                    if (!opendoor)
                    {
                        HoodCamera();
                        return;
                    }
                    break;
            }

            if (veh.HasBone("bonnet") && veh.HasBone("boot"))
            {
                if (veh.GetVehEnginePos() == EngineLoc.front)
                {
                    if (opendoor) veh.OpenDoor(4, false, false);
                    if (veh.GetVehHoodPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.Hood;
                    }
                    else if (veh.GetVehHoodPos() == EngineLoc.rear)
                    {
                        camera.MainCameraPosition = CameraPosition.RearHood;
                    }
                }
                else if (veh.GetVehEnginePos() == EngineLoc.rear)
                {
                    if (opendoor) veh.OpenDoor(4, false, false);
                    if (veh.GetVehHoodPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.Hood;
                    }
                    else if (veh.GetVehHoodPos() == EngineLoc.rear)
                    {
                        if (veh.GetVehTrunkPos() == EngineLoc.front)
                        {
                            camera.MainCameraPosition = CameraPosition.FrontTrunk;
                        }
                        else
                        {
                            camera.MainCameraPosition = CameraPosition.FrontBumper;
                        }
                    }
                }
            }
            else if (veh.HasBone("bonnet"))
            {
                if (opendoor) veh.OpenDoor(4, false, false);
                if (veh.GetVehEnginePos() == EngineLoc.front)
                {
                    if (veh.GetVehHoodPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.Hood;
                    }
                    else if (veh.GetVehHoodPos() == EngineLoc.rear)
                    {
                        camera.MainCameraPosition = CameraPosition.RearHood;
                    }
                }
                else if (veh.GetVehEnginePos() == EngineLoc.rear)
                {
                    if (veh.GetVehHoodPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.Hood;
                    }
                    else if (veh.GetVehHoodPos() == EngineLoc.rear)
                    {
                        camera.MainCameraPosition = CameraPosition.FrontBumper;
                    }
                }
            }
            else if (veh.HasBone("boot"))
            {
                if (opendoor) veh.OpenDoor(5, false, false);
                if (veh.GetVehEnginePos() == EngineLoc.front)
                {
                    if (veh.GetVehTrunkPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.FrontTrunk;
                    }
                    else if (veh.GetVehTrunkPos() == EngineLoc.rear)
                    {
                        camera.MainCameraPosition = CameraPosition.FrontBumper;
                    }
                }
                else if (veh.GetVehEnginePos() == EngineLoc.rear)
                {
                    if (veh.GetVehTrunkPos() == EngineLoc.front)
                    {
                        camera.MainCameraPosition = CameraPosition.FrontTrunk;
                    }
                    else if (veh.GetVehTrunkPos() == EngineLoc.rear)
                    {
                        camera.MainCameraPosition = CameraPosition.FrontBumper;
                    }
                }
            }
            else
            {
                if (veh.GetVehEnginePos() == EngineLoc.rear)
                {
                    camera.MainCameraPosition = CameraPosition.FrontBumper;
                }
                else if (veh.GetVehEnginePos() == EngineLoc.front)
                {
                    camera.MainCameraPosition = CameraPosition.Engine;
                }
            }
        }

        public enum GlobalValue
        {
            b1_0_757_4 = 0x271803,
            b1_0_791_2 = 0x272A34,
            b1_0_877_1 = 0x2750BD,
            b1_0_944_2 = 0x279476,
            b1_0_1032_1 = 2593970,
            b1_0_1103_2 = 2599337,
            b1_0_1180_2 = 2606794,
            b1_0_1365_1 = 4265719,
            b1_0_1493_1 = 4266042,
            b1_0_1604_1 = 4266905,
            b1_0_1737_0 = 4267883,
            b1_0_1868_0 = 4268190,
            b1_0_2060_0 = 4268340,
        }

        public static GlobalValue GetGlobalValue()
        {
            return GlobalValue.b1_0_2060_0;
        }

        public static void SuspendKeys()
        {
            Game.DisableControlThisFrame(Control.MoveUpDown);
            Game.DisableControlThisFrame(Control.MoveLeftRight);
            Game.DisableControlThisFrame(Control.MoveDown);
            Game.DisableControlThisFrame(Control.MoveDownOnly);
            Game.DisableControlThisFrame(Control.MoveLeft);
            Game.DisableControlThisFrame(Control.MoveLeftOnly);
            Game.DisableControlThisFrame(Control.MoveRight);
            Game.DisableControlThisFrame(Control.MoveRightOnly);
            Game.DisableControlThisFrame(Control.MoveUp);
            Game.DisableControlThisFrame(Control.MoveUpOnly);
            Game.DisableControlThisFrame(Control.Jump);
            Game.DisableControlThisFrame(Control.Cover);
            Game.DisableControlThisFrame(Control.Context);
            Game.DisableControlThisFrame(Control.VehicleAccelerate);
            Game.DisableControlThisFrame(Control.VehicleAim);
            Game.DisableControlThisFrame(Control.VehicleAttack);
            Game.DisableControlThisFrame(Control.VehicleAttack2);
            Game.DisableControlThisFrame(Control.VehicleBrake);
            Game.DisableControlThisFrame(Control.VehicleCinCam);
            Game.DisableControlThisFrame(Control.VehicleDuck);
            Game.DisableControlThisFrame(Control.VehicleExit);
            Game.DisableControlThisFrame(Control.VehicleHeadlight);
            Game.DisableControlThisFrame(Control.VehicleHorn);
            Game.DisableControlThisFrame(Control.VehicleMoveLeftOnly);
            Game.DisableControlThisFrame(Control.VehicleMoveRightOnly);
            Game.DisableControlThisFrame(Control.VehicleMoveLeft);
            Game.DisableControlThisFrame(Control.VehicleMoveRight);
            Game.DisableControlThisFrame(Control.VehicleSubTurnLeftRight);
            Game.DisableControlThisFrame(Control.VehicleSubTurnLeftOnly);
            Game.DisableControlThisFrame(Control.VehicleSubTurnRightOnly);
            Game.DisableControlThisFrame(Control.VehicleSubTurnHardLeft);
            Game.DisableControlThisFrame(Control.VehicleSubTurnHardRight);
            Game.DisableControlThisFrame(Control.VehicleMoveLeftRight);
            Game.DisableControlThisFrame(Control.VehicleLookLeft);
            Game.DisableControlThisFrame(Control.VehicleLookRight);
            Game.DisableControlThisFrame(Control.VehicleHotwireLeft);
            Game.DisableControlThisFrame(Control.VehicleHotwireRight);
            Game.DisableControlThisFrame(Control.VehicleGunLeftRight);
            Game.DisableControlThisFrame(Control.VehicleGunLeft);
            Game.DisableControlThisFrame(Control.VehicleGunRight);
            Game.DisableControlThisFrame(Control.VehicleCinematicLeftRight);
            Game.DisableControlThisFrame(Control.NextCamera);
            Game.DisableControlThisFrame(Control.VehicleRocketBoost);
            Game.DisableControlThisFrame(Control.VehicleJump);
            Game.DisableControlThisFrame(Control.VehicleCarJump);
        }

        public static void PlayerVehicleHalt(float distance = 1.0f)
        {
            if (Game.Player?.LastVehicle != null)
            {
                Game.Player.LastVehicle.Speed = 0f;
            }
        }

        public static bool IsNitroModInstalled()
        {
            return Decor.Registered(nitroMod, Decor.eDecorType.Int);
        }

        public static bool HasRam(this Vehicle v) => false;
        public static bool HasScoop(this Vehicle v) => false;
        public static bool HasSpike(this Vehicle v) => false;

        public static int GetInt(this Vehicle v, string decorName)
        {
            try
            {
                return Function.Call<int>((Hash)0xA06C969B02A97298UL, v.Handle, decorName);
            }
            catch
            {
                return 0;
            }
        }

        public static bool CanInstallNitroMod(this Vehicle v)
        {
            bool result = true;
            if (v.HasRocketBoost) result = false;
            if (v.HasRam()) result = false;
            if (v.HasScoop()) result = false;
            if (v.HasSpike()) result = false;
            if (v.GetModCount(VehicleMod.AirFilter) >= 2 && !bennysvehicle.Contains(v.Model)) result = false;
            if (!IsNitroModInstalled()) result = false;
            return result;
        }

        public static void UpdateTitleName()
        {
            if (MenuHelper.MainMenu == null)
            {
                return;
            }
        }

        public static void LoadSettings()
        {
            ScriptSettings config = ScriptSettings.Load("scripts\\BennysMotorworksRevamped.ini");
            optLogging = config.GetValue("SETTINGS", "LOGGING", true);
            onlineMap = config.GetValue<int>("SETTINGS", "OnlineMap", 1);
            fixDoor = config.GetValue<int>("SETTINGS", "FixDoor", 1);
            vehicleStatsOffsetX = config.GetValue<float>("VEHICLE_STATS", "OffsetX", 0f);
            vehicleStatsOffsetY = config.GetValue<float>("VEHICLE_STATS", "OffsetY", -10f);
            fpcKey = config.GetValue<GTA.Control>("CONTROLS", "FirstPerson", GTA.Control.NextCamera);
            zoutKey = config.GetValue<GTA.Control>("CONTROLS", "ZoomOut", GTA.Control.FrontendLt);
            zinKey = config.GetValue<GTA.Control>("CONTROLS", "ZoomIn", GTA.Control.FrontendRt);
            if (onlineMap == 1)
            {
                LoadMPDLCMap();
            }
        }

        public static void CreateBlip()
        {
            BennysBlip = World.CreateBlip(new Vector3(-205.5417f, -1307.118f, 30.26981f));
            BennysBlip.Sprite = BlipSprite.DollarSignSquared | BlipSprite.ArrowDownOutlined;
            BennysBlip.Color = BlipColor.Yellow;
            BennysBlip.IsShortRange = true;
            BennysBlip.Name = Gxt("S_MO_09");
        }


        private enum WorkshopCutsceneType
        {
            None,
            Enter,
            Exit,
        }

        private static readonly Vector3 EnterTriggerPosition = new Vector3(-205.553f, -1316.169f, 30.890f);
        private static readonly Vector3 EnterCutsceneStartPosition = new Vector3(-205.698f, -1312.353f, 31.203f);
        private static readonly Vector3 EnterCutsceneWaypointA = new Vector3(-207.155f, -1320.521f, 30.8904f);
        private static readonly Vector3 ShopVehiclePosition = new Vector3(-211.801f, -1324.290f, 30.37535f);
        private static readonly Vector3 ExitCutsceneLanePosition = new Vector3(-205.8678f, -1321.805f, 30.41191f);
        private static readonly Vector3 ExitCutsceneWaypointA = new Vector3(-205.714f, -1309.399f, 31.249f);
        private static readonly Vector3 ExitCutsceneWaypointB = new Vector3(-200.2561f, -1303.021f, 30.66544f);
        private static readonly Vector3 EnterCutsceneCameraPosition = new Vector3(-200.7804f, -1316.474f, 32.08001f);
        private static readonly Vector3 ExitCutsceneCameraPosition = new Vector3(-197.5533f, -1297.754f, 32.29234f);
        private static WorkshopCutsceneType activeWorkshopCutscene = WorkshopCutsceneType.None;
        private static int activeWorkshopCutsceneStage = -1;
        private static int activeWorkshopCutsceneStageStartedAt;
        private static int activeWorkshopCutsceneLastDriveTaskAt;
        private static int activeWorkshopCutsceneLastProgressAt;
        private static Vector3 activeWorkshopCutsceneTarget;
        private static float activeWorkshopCutsceneTargetRadius;
        private static float activeWorkshopCutsceneTargetSpeed;
        private static float activeWorkshopCutsceneLastDistanceSq;
        private static int enterCutsceneBlockedUntil;
        private static Vector3 lastGarageTriggerSamplePosition = Vector3.Zero;
        private static bool hasGarageTriggerSample;

        private static Camera CreateScriptCamera(Vector3 position, Vector3 rotation, float fieldOfView)
        {
            int handle = Function.Call<int>(Hash.CREATE_CAM_WITH_PARAMS,
                "DEFAULT_SCRIPTED_CAMERA",
                position.X, position.Y, position.Z,
                rotation.X, rotation.Y, rotation.Z,
                fieldOfView, true, 2);

            return new Camera(handle);
        }

        private static void EnsurePlayerInVehicleForCutscene()
        {
            if (ply == null || veh == null) return;

            if (ply.CurrentVehicle != veh)
            {
                Logger.Log("EnsurePlayerInVehicle: warping player into vehicle");
                Function.Call(Hash.SET_PED_INTO_VEHICLE, ply.Handle, veh.Handle, (int)VehicleSeat.Driver);
            }
            // No log for "already in vehicle" – removes spam
        }

        private static void StartWorkshopAudioScene()
        {
            if (!Function.Call<bool>(Hash.IS_AUDIO_SCENE_ACTIVE, "CAR_MOD_RADIO_MUTE_SCENE"))
            {
                Function.Call(Hash.START_AUDIO_SCENE, "CAR_MOD_RADIO_MUTE_SCENE");
            }
        }

        private static void StopWorkshopAudioScene()
        {
            if (Function.Call<bool>(Hash.IS_AUDIO_SCENE_ACTIVE, "CAR_MOD_RADIO_MUTE_SCENE"))
            {
                Function.Call(Hash.STOP_AUDIO_SCENE, "CAR_MOD_RADIO_MUTE_SCENE");
            }
        }

        private static void ResetWorkshopCutsceneCamera()
        {
            try
            {
                Function.Call(Hash.RENDER_SCRIPT_CAMS, false, false, 0, true, false, 0);
            }
            catch
            {
            }

            if (scriptCam != null)
            {
                try
                {
                    scriptCam.IsActive = false;
                }
                catch
                {
                }

                try
                {
                    scriptCam.Delete();
                }
                catch
                {
                }

                scriptCam = null;
            }
        }
        private static void CleanupEnterCutsceneWithoutCameraReset()
        {
            try
            {
                if (ply != null)
                    Function.Call(Hash.CLEAR_PED_TASKS, ply.Handle);
            }
            catch { }

            try
            {
                if (veh != null)
                {
                    Function.Call(Hash.SET_ENTITY_VELOCITY, veh.Handle, 0.0f, 0.0f, 0.0f);
                    Function.Call(Hash.SET_VEHICLE_FORWARD_SPEED, veh.Handle, 0.0f);
                }
            }
            catch { }

            // Stop the audio scene (radio mute)
            StopWorkshopAudioScene();

            // Clear cutscene state but leave the camera intact
            activeWorkshopCutscene = WorkshopCutsceneType.None;
            activeWorkshopCutsceneStage = -1;
            activeWorkshopCutsceneLastDriveTaskAt = 0;
            activeWorkshopCutsceneLastProgressAt = 0;
            activeWorkshopCutsceneLastDistanceSq = float.MaxValue;
            activeWorkshopCutsceneTarget = Vector3.Zero;
            activeWorkshopCutsceneTargetRadius = 0.0f;
            activeWorkshopCutsceneTargetSpeed = 0.0f;
            isExiting = false;
            isCutscene = false;
        }

        private static void StartWorkshopCutsceneCamera(Vector3 position)
        {
            ResetWorkshopCutsceneCamera();
            scriptCam = CreateScriptCamera(position, Vector3.Zero, GameplayCamera.FieldOfView);
            scriptCam.PointAt(veh);
            scriptCam.Shake(CameraShake.Hand, 0.3f);
            scriptCam.IsActive = true;
            Function.Call(Hash.RENDER_SCRIPT_CAMS, true, false, 0, true, false, 0);
        }

        private static void SetEnterCutsceneCooldown(int milliseconds = 6500)
        {
            int until = Game.GameTime + milliseconds;
            if (until > enterCutsceneBlockedUntil)
            {
                enterCutsceneBlockedUntil = until;
            }
        }

        private static void AdvanceWorkshopCutsceneStage(int nextStage)
        {
            activeWorkshopCutsceneStage = nextStage;
            activeWorkshopCutsceneStageStartedAt = Game.GameTime;
            activeWorkshopCutsceneLastProgressAt = Game.GameTime;
            activeWorkshopCutsceneLastDistanceSq = float.MaxValue;
        }

        private static bool IsWorkshopCutsceneStageTimedOut(int milliseconds)
        {
            return Game.GameTime - activeWorkshopCutsceneStageStartedAt >= milliseconds;
        }

        private static bool IsNear(Vector3 position, Vector3 target, float radius)
        {
            return position.DistanceToSquared(target) <= radius * radius;
        }

        private static float Dot(Vector3 left, Vector3 right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        private static Vector3 NormalizeOrZero(Vector3 value)
        {
            if (value.Length() <= 0.001f)
            {
                return Vector3.Zero;
            }

            value.Normalize();
            return value;
        }

        private static float GetHeadingToward(Vector3 position, Vector3 target, float fallbackHeading)
        {
            Vector3 direction = target - position;
            if (direction.Length() <= 0.001f)
            {
                return fallbackHeading;
            }

            return (float)(Math.Atan2(direction.X, direction.Y) * 180.0 / Math.PI);
        }

        private static void SetCutsceneVehicleTransform(Vector3 position, float heading)
        {
            if (veh == null) return;
            Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, veh.Handle, position.X, position.Y, position.Z, false, false, false);
            veh.Heading = heading;
            Function.Call(Hash.SET_ENTITY_VELOCITY, veh.Handle, 0.0f, 0.0f, 0.0f);
            Function.Call(Hash.SET_VEHICLE_FORWARD_SPEED, veh.Handle, 0.0f);
            // Force ground and freeze briefly to settle
            Function.Call(Hash.SET_VEHICLE_ON_GROUND_PROPERLY, veh.Handle);
            Function.Call(Hash.FREEZE_ENTITY_POSITION, veh.Handle, true);
            Script.Wait(50);
            Function.Call(Hash.FREEZE_ENTITY_POSITION, veh.Handle, false);
        }

        private static void QueueCutsceneDrive(Vector3 target, float radius, float speed)
        {
            if (ply == null || veh == null)
            {
                Logger.Log("QueueCutsceneDrive: ply or veh is null");
                return;
            }

            Logger.Log($"QueueCutsceneDrive: target={target}, radius={radius}, speed={speed}");

            activeWorkshopCutsceneTarget = target;
            activeWorkshopCutsceneTargetRadius = radius;
            activeWorkshopCutsceneTargetSpeed = speed;
            activeWorkshopCutsceneLastDriveTaskAt = Game.GameTime;

            EnsurePlayerInVehicleForCutscene();

            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, veh.Handle, true, true, false);
            Function.Call(Hash.SET_VEHICLE_HANDBRAKE, veh.Handle, false);
            Function.Call(Hash.SET_DRIVER_ABILITY, ply.Handle, 1.0f);
            Function.Call(Hash.SET_DRIVER_AGGRESSIVENESS, ply.Handle, 0.0f);

            // Use default driving style (0) – smoother parking
            uint drivingStyle = 0;

            Logger.Log($"Issuing TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE with style {drivingStyle:X}");

            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE,
                ply.Handle,
                veh.Handle,
                target.X, target.Y, target.Z,
                speed,
                drivingStyle,
                radius);

            // Small nudge to overcome inertia if completely stopped
            if (veh.Speed < 0.1f)
            {
                Function.Call(Hash.SET_VEHICLE_FORWARD_SPEED, veh.Handle, 0.5f);
            }
        }

        private static bool UpdateCutsceneProgress()
        {
            if (veh == null || activeWorkshopCutsceneTarget.Length() <= 0.001f)
            {
                return false;
            }

            float distanceSq = veh.Position.DistanceToSquared(activeWorkshopCutsceneTarget);
            if (distanceSq + 0.5f < activeWorkshopCutsceneLastDistanceSq)
            {
                activeWorkshopCutsceneLastDistanceSq = distanceSq;
                activeWorkshopCutsceneLastProgressAt = Game.GameTime;
                return true;
            }

            return false;
        }

        private static void RefreshCutsceneDriveTaskIfNeeded()
        {
            if (activeWorkshopCutscene == WorkshopCutsceneType.None || ply == null || veh == null)
                return;

            if (activeWorkshopCutsceneTarget.Length() <= 0.001f)
                return;

            if (IsNear(veh.Position, activeWorkshopCutsceneTarget, activeWorkshopCutsceneTargetRadius + 0.5f))
                return;

            UpdateCutsceneProgress();

            int now = Game.GameTime;

            // Do not re-issue too often – increase to 3 seconds
            if (now - activeWorkshopCutsceneLastDriveTaskAt < 3000)
                return;

            // Relaxed stuck detection: speed below 0.5 for 4 seconds, or no progress for 6 seconds
            bool isStuck = veh.Speed < 0.5f && (now - activeWorkshopCutsceneLastProgressAt) > 4000;
            bool noProgress = (now - activeWorkshopCutsceneLastProgressAt) > 6000;

            if (isStuck || noProgress)
            {
                Logger.Log($"Refresh: re-issuing drive, stuck={isStuck}, noProgress={noProgress}");
                float boostedSpeed = activeWorkshopCutsceneTargetSpeed * 1.2f;
                QueueCutsceneDrive(activeWorkshopCutsceneTarget, activeWorkshopCutsceneTargetRadius, boostedSpeed);
            }
        }

        private static void EnsureWorkshopGreeter()
        {
            if (bennyPed != null)
            {
                try
                {
                    bennyPed.Delete();
                }
                catch
                {
                }
            }

            bennyPed = World.CreatePed(PedHash.Benny, new Vector3(-216.0945f, -1319.185f, 30.89038f), 219.5891f);
            if (bennyPed != null)
            {
                bennyPed.Task.LookAt(veh);
                bennyPed.KeepTaskWhenMarkedAsNoLongerNeeded = true;
            }
        }

        private static void AbortWorkshopCutscene(bool stopAudioScene)
        {
            try
            {
                if (ply != null)
                {
                    Function.Call(Hash.CLEAR_PED_TASKS, ply.Handle);
                }
            }
            catch
            {
            }

            try
            {
                if (veh != null)
                {
                    Function.Call(Hash.SET_ENTITY_VELOCITY, veh.Handle, 0.0f, 0.0f, 0.0f);
                    Function.Call(Hash.SET_VEHICLE_FORWARD_SPEED, veh.Handle, 0.0f);
                }
            }
            catch
            {
            }

            ResetWorkshopCutsceneCamera();
            activeWorkshopCutscene = WorkshopCutsceneType.None;
            activeWorkshopCutsceneStage = -1;
            activeWorkshopCutsceneLastDriveTaskAt = 0;
            activeWorkshopCutsceneLastProgressAt = 0;
            activeWorkshopCutsceneLastDistanceSq = float.MaxValue;
            activeWorkshopCutsceneTarget = Vector3.Zero;
            activeWorkshopCutsceneTargetRadius = 0.0f;
            activeWorkshopCutsceneTargetSpeed = 0.0f;
            isExiting = false;
            isCutscene = false;

            if (stopAudioScene)
            {
                StopWorkshopAudioScene();
            }
        }

        private static void CompleteEnterCutscene()
        {
            EnsurePlayerInVehicleForCutscene();
            PlayerVehicleHalt();
            CleanupEnterCutsceneWithoutCameraReset();
            PutVehIntoShop();
        }

        private static void CompleteExitCutscene()
        {
            EnsurePlayerInVehicleForCutscene();
            PlayerVehicleHalt();

            if (veh != null)
            {
                if (IsNear(veh.Position, ExitCutsceneWaypointB, 3.5f))
                {
                    veh.Heading = 312.8701f;
                }
                else if (veh.Position.DistanceToSquared(ExitCutsceneWaypointB) <= 20.25f)
                {
                    SetCutsceneVehicleTransform(ExitCutsceneWaypointB, 312.8701f);
                }

                veh.Repair();
            }

            SetEnterCutsceneCooldown(12000);
            AbortWorkshopCutscene(true);
        }

        private static void ProcessEnterCutscene()
        {
            switch (activeWorkshopCutsceneStage)
            {
                case 0:
                    StartWorkshopAudioScene();
                    EnsureWorkshopGreeter();
                    EnsurePlayerInVehicleForCutscene();
                    Function.Call(Hash.SET_ENTITY_ALPHA, Game.Player.Character.Handle, 255, false);
                    camera?.Stop();

                    // Start the cutscene camera FIRST so the player doesn't see the vehicle warp
                    StartWorkshopCutsceneCamera(EnterCutsceneCameraPosition);

                    // Then warp the vehicle to the starting position
                    SetCutsceneVehicleTransform(EnterCutsceneStartPosition, 180.3224f);

                    // Advance to the settle stage (allows the vehicle to settle before driving)
                    AdvanceWorkshopCutsceneStage(-1);
                    break;

                case -1:
                    // Small settle delay (100ms) then start driving
                    if (IsWorkshopCutsceneStageTimedOut(100))
                    {
                        QueueCutsceneDrive(EnterCutsceneWaypointA, 3.5f, 6.5f);
                        PlaySpeech("SHOP_NICE_VEHICLE");
                        AdvanceWorkshopCutsceneStage(1);
                    }
                    break;

                case 1:
                    RefreshCutsceneDriveTaskIfNeeded();
                    if (IsNear(veh.Position, EnterCutsceneWaypointA, 3.0f))
                    {
                        QueueCutsceneDrive(ShopVehiclePosition, 0.5f, 5.0f);
                        AdvanceWorkshopCutsceneStage(2);
                    }
                    else if (IsWorkshopCutsceneStageTimedOut(10000))
                    {
                        SetCutsceneVehicleTransform(EnterCutsceneWaypointA, 180.3224f);
                        QueueCutsceneDrive(ShopVehiclePosition, 0.5f, 5.0f);
                        AdvanceWorkshopCutsceneStage(2);
                    }
                    break;

                case 2:
                    RefreshCutsceneDriveTaskIfNeeded();
                    if (IsNear(veh.Position, ShopVehiclePosition, activeWorkshopCutsceneTargetRadius + 0.5f))
                    {
                        CompleteEnterCutscene();
                    }
                    else if (IsWorkshopCutsceneStageTimedOut(8000))
                    {
                        CompleteEnterCutscene();
                    }
                    break;
            }
        }

        private static void ProcessExitCutscene()
        {
            // Define the intermediate waypoint
            Vector3 intermediatePoint = new Vector3(-205.714f, -1309.399f, 31.249f);

            switch (activeWorkshopCutsceneStage)
            {
                case 0:
                    StartWorkshopAudioScene();
                    EnsurePlayerInVehicleForCutscene();
                    Function.Call(Hash.SET_ENTITY_ALPHA, Game.Player.Character.Handle, 255, false);
                    camera?.Stop();

                    // Warp to the internal waypoint (inside the garage, near the entrance)
                    SetCutsceneVehicleTransform(EnterCutsceneWaypointA, 190.3224f);

                    // Face the first destination before starting the drive task.
                    cachedExitHeading = GetHeadingToward(veh.Position, ExitCutsceneLanePosition, 190.3224f);
                    veh.Heading = cachedExitHeading;
                    Function.Call(Hash.SET_VEHICLE_ON_GROUND_PROPERLY, veh.Handle);

                    StartWorkshopCutsceneCamera(ExitCutsceneCameraPosition);
                    // Drive to the lane (just outside the garage)
                    QueueCutsceneDrive(ExitCutsceneLanePosition, 1.8f, 4.5f);
                    PlaySpeech("SHOP_GOODBYE");
                    AdvanceWorkshopCutsceneStage(1);
                    break;

                case 1:
                    RefreshCutsceneDriveTaskIfNeeded();
                    if (IsNear(veh.Position, ExitCutsceneLanePosition, 2.0f))
                    {
                        // Reached lane → drive to intermediate point
                        if (veh.Speed < 0.5f)
                        {
                            veh.Heading = GetHeadingToward(veh.Position, intermediatePoint, veh.Heading);
                        }
                        QueueCutsceneDrive(intermediatePoint, 0.5f, 5.75f);
                        AdvanceWorkshopCutsceneStage(2);
                    }
                    else if (IsWorkshopCutsceneStageTimedOut(10000))
                    {
                        // Fallback: teleport to lane, then drive to intermediate
                        float headingToIntermediate = GetHeadingToward(ExitCutsceneLanePosition, intermediatePoint, cachedExitHeading);
                        SetCutsceneVehicleTransform(ExitCutsceneLanePosition, headingToIntermediate);
                        QueueCutsceneDrive(intermediatePoint, 0.5f, 5.75f);
                        AdvanceWorkshopCutsceneStage(2);
                    }
                    break;

                case 2:
                    RefreshCutsceneDriveTaskIfNeeded();
                    if (IsNear(veh.Position, intermediatePoint, activeWorkshopCutsceneTargetRadius + 0.5f))
                    {
                        // Reached intermediate → drive to final waypoint B
                        if (veh.Speed < 0.5f)
                        {
                            veh.Heading = GetHeadingToward(veh.Position, ExitCutsceneWaypointB, veh.Heading);
                        }
                        QueueCutsceneDrive(ExitCutsceneWaypointB, 0.5f, 5.75f);
                        AdvanceWorkshopCutsceneStage(3);
                    }
                    else if (IsWorkshopCutsceneStageTimedOut(10000))
                    {
                        // Fallback: teleport to intermediate, head toward B, then drive to B
                        float headingToB = GetHeadingToward(intermediatePoint, ExitCutsceneWaypointB, cachedExitHeading);
                        SetCutsceneVehicleTransform(intermediatePoint, headingToB);
                        QueueCutsceneDrive(ExitCutsceneWaypointB, 1.5f, 5.75f);
                        AdvanceWorkshopCutsceneStage(3);
                    }
                    break;

                case 3:
                    RefreshCutsceneDriveTaskIfNeeded();
                    if (IsNear(veh.Position, ExitCutsceneWaypointB, 2.0f))
                    {
                        CompleteExitCutscene();
                    }
                    else if (IsWorkshopCutsceneStageTimedOut(10000))
                    {
                        SetCutsceneVehicleTransform(ExitCutsceneWaypointB, 312.8701f);
                        CompleteExitCutscene();
                    }
                    break;
            }
        }

        public static void ProcessWorkshopCutscene()
        {
            try
            {
                // Handle pending shop init (deferred heavy work)
                if (_pendingShopInit && Game.GameTime >= _shopInitDelayTime)
                {
                    FinishShopInit();
                }

                if (activeWorkshopCutscene == WorkshopCutsceneType.None)
                {
                    return;
                }

                if (ply == null || veh == null || !veh.Exists() || !ply.Exists())
                {
                    AbortWorkshopCutscene(true);
                    return;
                }

                EnsurePlayerInVehicleForCutscene();

                switch (activeWorkshopCutscene)
                {
                    case WorkshopCutsceneType.Enter:
                        ProcessEnterCutscene();
                        break;
                    case WorkshopCutsceneType.Exit:
                        ProcessExitCutscene();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
                AbortWorkshopCutscene(true);
            }
        }
        public static bool CanTriggerEnterCutscene()
        {
            if (veh == null || ply == null)
            {
                return false;
            }

            Vector3 garageApproachDirection = Vector3.Zero;
            if (hasGarageTriggerSample)
            {
                garageApproachDirection = NormalizeOrZero(veh.Position - lastGarageTriggerSamplePosition);
            }

            lastGarageTriggerSamplePosition = veh.Position;
            hasGarageTriggerSample = true;

            if (activeWorkshopCutscene != WorkshopCutsceneType.None || isExiting || isCutscene)
            {
                return false;
            }

            if (Game.GameTime < enterCutsceneBlockedUntil)
            {
                return false;
            }

            if (ply.CurrentVehicle != veh)
            {
                return false;
            }

            if (!IsNear(veh.Position, EnterTriggerPosition, 5.0f))
            {
                return false;
            }

            if (veh.Speed < 0.8f)
            {
                return false;
            }

            Vector3 inwardDirection = NormalizeOrZero(EnterCutsceneWaypointA - veh.Position);
            Vector3 approachDirection = garageApproachDirection.Length() > 0.001f
                ? garageApproachDirection
                : NormalizeOrZero(veh.Velocity);

            if (approachDirection.Length() <= 0.001f)
            {
                approachDirection = NormalizeOrZero(veh.ForwardVector);
            }

            if (veh.Position.DistanceToSquared(ShopVehiclePosition) <= 36.0f)
            {
                return false;
            }

            return inwardDirection.Length() > 0.001f && approachDirection.Length() > 0.001f && Dot(approachDirection, inwardDirection) > 0.55f;
        }

        public static void PlayEnterCutScene()
        {
            try
            {
                if (veh == null || ply == null || activeWorkshopCutscene != WorkshopCutsceneType.None || isCutscene)
                {
                    return;
                }

                SetEnterCutsceneCooldown(9000);
                isExiting = true;
                isCutscene = true;
                activeWorkshopCutscene = WorkshopCutsceneType.Enter;
                AdvanceWorkshopCutsceneStage(0);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void PlayExitCutScene()
        {
            try
            {
                if (veh == null || ply == null || activeWorkshopCutscene != WorkshopCutsceneType.None)
                {
                    return;
                }

                SetEnterCutsceneCooldown(9000);
                isExiting = true;
                isCutscene = true;
                activeWorkshopCutscene = WorkshopCutsceneType.Exit;
                AdvanceWorkshopCutsceneStage(0);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void PutVehIntoShop()
        {
            try
            {
                Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, veh.Handle, ShopVehiclePosition.X, ShopVehiclePosition.Y, ShopVehiclePosition.Z, false, false, false);
                veh.Heading = 150.2801f;
                Function.Call(Hash.SET_ENTITY_VELOCITY, veh.Handle, 0.0f, 0.0f, 0.0f);
                Function.Call(Hash.SET_VEHICLE_FORWARD_SPEED, veh.Handle, 0.0f);
                Function.Call(Hash.SET_VEHICLE_ON_GROUND_PROPERLY, veh.Handle);

                // Switch from the entrance camera to the workshop camera in the
                // same tick as the final positioning snap.
                ResetWorkshopCutsceneCamera();
                camera.RepositionFor(veh);

                // Finish menu initialization on the next tick without an
                // artificial settling delay.
                _pendingShopInit = true;
                _shopInitDelayTime = Game.GameTime;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        private static void FinishShopInit()
        {
            try
            {
                _pendingShopInit = false;

                veh.InstallModKit();
                MenuHelper.RefreshMenus();
                VehicleWindowTint currentWindowTint = veh.Mods.WindowTint;
                if (currentWindowTint == VehicleWindowTint.Invalid)
                {
                    currentWindowTint = VehicleWindowTint.None;
                }
                lastVehMemory = new Memory
                {
                    Aerials = veh.GetMod(VehicleMod.Aerials),
                    Trim = veh.GetMod(VehicleMod.Trim),
                    FrontBumper = veh.GetMod(VehicleMod.FrontBumper),
                    RearBumper = veh.GetMod(VehicleMod.RearBumper),
                    SideSkirt = veh.GetMod(VehicleMod.SideSkirt),
                    ColumnShifterLevers = veh.GetMod(VehicleMod.ColumnShifterLevers),
                    Dashboard = veh.GetMod(VehicleMod.Dashboard),
                    DialDesign = veh.GetMod(VehicleMod.DialDesign),
                    Ornaments = veh.GetMod(VehicleMod.Ornaments),
                    Seats = veh.GetMod(VehicleMod.Seats),
                    SteeringWheels = veh.GetMod(VehicleMod.SteeringWheels),
                    TrimDesign = veh.GetMod(VehicleMod.TrimDesign),
                    LightsColor = veh.Mods.DashboardColor,
                    TrimColor = veh.Mods.TrimColor,
                    WheelType = Function.Call<VehicleWheelType>(Hash.GET_VEHICLE_WHEEL_TYPE, veh.Handle),
                    AirFilter = veh.GetMod(VehicleMod.AirFilter),
                    EngineBlock = veh.GetMod(VehicleMod.EngineBlock),
                    Struts = veh.GetMod(VehicleMod.Struts),
                    NumberPlate = (LicensePlateStyle)Function.Call<int>(Hash.GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, veh.Handle),
                    PlateHolder = veh.GetMod(VehicleMod.PlateHolder),
                    VanityPlates = veh.GetMod(VehicleMod.VanityPlates),
                    Armor = veh.GetMod(VehicleMod.Armor),
                    Brakes = veh.GetMod(VehicleMod.Brakes),
                    Engine = veh.GetMod(VehicleMod.Engine),
                    Transmission = veh.GetMod(VehicleMod.Transmission),
                    BackNeon = veh.IsNeonLightsOn(VehicleNeonLight.Back),
                    FrontNeon = veh.IsNeonLightsOn(VehicleNeonLight.Front),
                    LeftNeon = veh.IsNeonLightsOn(VehicleNeonLight.Left),
                    RightNeon = veh.IsNeonLightsOn(VehicleNeonLight.Right),
                    BackWheels = veh.GetMod(VehicleMod.RearWheel),
                    FrontWheels = veh.GetMod(VehicleMod.FrontWheel),
                    Headlights = veh.IsToggleModOn(VehicleToggleMod.XenonHeadlights),
                    WheelsVariation = IsCustomWheels(),
                    ArchCover = veh.GetMod(VehicleMod.ArchCover),
                    Exhaust = veh.GetMod(VehicleMod.Exhaust),
                    Fender = veh.GetMod(VehicleMod.Fender),
                    RightFender = veh.GetMod(VehicleMod.RightFender),
                    DoorSpeakers = veh.GetMod(VehicleMod.DoorSpeakers),
                    Frame = veh.GetMod(VehicleMod.Frame),
                    Grille = veh.GetMod(VehicleMod.Grille),
                    Hood = veh.GetMod(VehicleMod.Hood),
                    Horns = veh.GetMod(VehicleMod.Horns),
                    Hydraulics = veh.GetMod(VehicleMod.Hydraulics),
                    Livery = veh.GetMod(VehicleMod.Livery),
                    Livery2 = veh.GetLivery2(),
                    Plaques = veh.GetMod(VehicleMod.Plaques),
                    Roof = veh.GetMod(VehicleMod.Roof),
                    Speakers = veh.GetMod(VehicleMod.Speakers),
                    Spoilers = veh.GetMod(VehicleMod.Spoilers),
                    Tank = veh.GetMod(VehicleMod.Tank),
                    Trunk = veh.GetMod(VehicleMod.Trunk),
                    Turbo = veh.IsToggleModOn(VehicleToggleMod.Turbo),
                    Windows = veh.GetMod(VehicleMod.Windows),
                    Tint = currentWindowTint,
                    PearlescentColor = veh.Mods.PearlescentColor,
                    PrimaryColor = veh.Mods.PrimaryColor,
                    RimColor = veh.Mods.RimColor,
                    SecondaryColor = veh.Mods.SecondaryColor,
                    TireSmokeColor = veh.Mods.TireSmokeColor,
                    NeonLightsColor = veh.Mods.NeonLightsColor,
                    PlateNumbers = veh.Mods.LicensePlate,
                    HeadlightsColor = veh.GetXenonHeadlightsColor(),
                    Suspension = veh.GetMod(VehicleMod.Suspension),
                    Nitro = Helper.GetInt(veh, nitroMod),
                    BulletProofTires = veh.CanTiresBurst,
                };

                if (MenuHelper.MainMenu != null)
                {
                    MenuHelper.MainMenu.Visible = true;
                }
                else
                {
                    Logger.Log("PutVehIntoShop: MainMenu was null.");
                }

                StartWorkshopAudioScene();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

    }
}
