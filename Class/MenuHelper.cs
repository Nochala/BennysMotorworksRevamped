using System.Drawing;
using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using Reflection = System.Reflection;
using RegularExpressions = System.Text.RegularExpressions;
using VehicleMod = GTA.VehicleModType;
using VehicleToggleMod = GTA.VehicleToggleModType;
using static BennysMotorworksRevamped.Helper;
using BennysMotorworksRevamped.Compat;

namespace BennysMotorworksRevamped
{
    internal static class MenuHelper
    {
        static MenuHelper()
        {
            iSecondaryPearlescentColor = null;
            iWheels = null;
            iTuner = null;
            gmTrailer = null;
            iLights = null;
            iSport = null;
            iPrimaryCol = null;
            iSecondaryCol = null;
            giTrailer = null;
            iBikeWheels = null;
            iPlate = null;
            iLowrider = null;
            iHighEnd = null;
            iMuscle = null;
            iOffroad = null;
            iSUV = null;
            iRollcage = null;
            iRespray = null;
        }

        public static UIMenu QuitMenu, MainMenu, gmBodywork, gmBodyworkArena, gmEngine, gmInterior, gmPlate, gmLights, gmRespray, gmWheels, gmBumper, gmWheelType, gmNeonKits, gmWeapon;
        public static UIMenu mAerials, mSuspension, mArmor, mBrakes, mEngine, mTransmission, mFBumper, mRBumper, mSSkirt, mTrim, mEngineBlock, mAirFilter, mStruts, mColumnShifterLevers, mDashboard, mDialDesign, mOrnaments, mSeats, mSteeringWheels, mTrimDesign, mPlateHolder, mVanityPlates, mNumberPlate, gmBikeWheels, gmHighEnd, gmLowrider, gmMuscle, gmOffroad, gmSport, gmSUV, gmTuner, mBennysOriginals, mBespoke, mRacing, mStreet, mTires, mHeadlights, mNeon, mNeonColor, mArchCover, mExhaust, mFender, mRFender, mDoor, mFrame, mGrille, mHood, mHorn, mHydraulics, mLivery, mPlaques, mRoof, mSpeakers, mSpoilers, mTank, mTrunk, mWindow, mTurbo, mTint, mLightsColor, mTrimColor, mRimColor, mPrimaryClassicColor, mPrimaryChromeColor, mPrimaryMetallicColor, mPrimaryMetalsColor, mPrimaryMatteColor, mPrimaryPearlescentColor, mPrimaryColor, mSecondaryColor, mSecondaryClassicColor, mSecondaryChromeColor, mSecondaryMetallicColor, mSecondaryMetalsColor, mSecondaryMatteColor, mTireSmoke, mTornadoC, mSBikeWheels, mCBikeWheels, mSHighEnd, mCHighEnd, mSLowrider, mCLowrider, mSMuscle, mCMuscle, mSOffroad, mCOffroad, mSSport, mCSport, mSSUV, mCSUV, mSTuner, mCTuner, mUpgradeAW, mNitro;
        public static UIMenuItem iRepair, iHorn, iArmor, iBrakes, iFBumper, iExhaust, iFender, iRollcage, iRoof, iTransmission, iEngine, iPlate, iLights, iTint, iTurbo, iRespray, iWheels, iSuspension, iEngineBlock, iAerials, iAirFilter, iArchCover, iDoor, iFrame, iGrille, iHood, iHydraulics, iLivery, iPlaques, iRFender, iSpeaker, iSpoilers, iTank, iTrunk, iWindows, iTrim, iUpgrade, iRemoveUpgrade, iUpgradeMod, iUpgradeAW, iUpgradeAWV, iStruts, iTrimColor, iColumnShifterLevers, iDashboard, iDialDesign, iOrnaments, iSeats, iSteeringWheels, iTrimDesign, iRBumper, iSideSkirt, iRimColor, iPlateHolder, iVanityPlates, iHeadlights, iDashboardColor, iNumberPlate, iBikeWheels, iHighEnd, iLowrider, iMuscle, iOffroad, iSport, iSUV, iTuner, iBennys, iBespoke, iRacing, iStreet, iTires, iBPTires, iNeon, iTireSmoke, iNeonColor, iLightsColor, iPrimaryCol, iSecondaryCol, iPrimaryChromeColor, iPrimaryClassicColor, iPrimaryMetallicColor, iPrimaryMetalsColor, iPrimaryMatteColor, iPrimaryPearlescentColor, iSecondaryChromeColor, iSecondaryClassicColor, iSecondaryMetallicColor, iSecondaryMetalsColor, iSecondaryMatteColor, iSecondaryPearlescentColor, iTornadoC, iNitro;
        public static UIMenuItem giBodywork, giBodyworkArena, giEngine, giInterior, giPlate, giLights, giRespray, giWheels, giBumper, giWheelType, giTires, giNeonKits, giPrimaryCol, giSecondaryCol, giBikeWheels, giHighEndWheels, giDoor, giLowriderWheels, giMuscleWheels, giOffroadWheels, giSportWheels, giSUVWheels, giTunerWheels, giBennysWheels, giBespokeWheels, giRacingWheels, giStreetWheels, giFBumper, giRBumper, giSSkirt, giNumberPlate, giVanityPlate, giPlateHolder, giExhaust, giBrakes, giGrille, giHood, giHydraulics, giPlaques, giSpoilers, giTank, giTrunk, giStruts, iSBikeWheels, iCBikeWheels, iSHighEnd, iCHighEnd, iSLowrider, iCLowrider, iSMuscle, iCMuscle, iSOffroad, iCOffroad, iSSport, iCSport, iSSUV, iCSUV, iSTuner, iCTuner, giTrailer, giWeapon, giArchCover, giRoof, giAirfilter, giOrnaments;
        public static UIMenuItem iShifter, iFMudguard, iBSeat, iOilTank, iRMudguard, iFuelTank, iBeltDriveCovers, iBEngineBlock, iBAirFilter, iBTank;
        public static UIMenuItem giShifter, giFMudguard, giOilTank, giRMudguard, giFuelTank, giBeltDriveCovers, giBEngineBlock, giBAirFilter, giBTank;
        public static UIMenu mShifter, mFMudguard, mBSeat, mOilTank, mRMudguard, mFuelTank, mBeltDriveCovers, mBEngineBlock, mBAirFilter, mBTank, gmTrailer;
        public static LemonUI.ObjectPool _menuPool;
        private static bool _suppressMenuRestoreOnClose;

        public static void HideAllMenus()
        {
            _suppressMenuRestoreOnClose = true;
            try
            {
                UIMenu.HideAll();
            }
            finally
            {
                _suppressMenuRestoreOnClose = false;
            }
        }


        #region Refresh Menus
        private static int GetIndexedModPrice(int index, int priceStep)
        {
            // Stock uses index -1. It should cost the base tier whenever it is
            // not currently equipped instead of appearing as a blank row.
            return priceStep * Math.Max(1, index + 1);
        }

        private static readonly int[] SuspensionPrices = { 500, 1000, 2000, 3400, 4400, 4600 };
        private static readonly int[] EnginePrices = { 1000, 9000, 12500, 18000, 33500 };
        private static readonly int[] ArmorPrices = { 1000, 7500, 12000, 20000, 35000, 50000 };
        private static readonly int[] BrakePrices = { 1000, 20000, 27000, 35000 };
        private static readonly int[] TransmissionPrices = { 1000, 29500, 32500, 40000 };

        private static int GetPerformanceModPrice(VehicleMod modType, int index)
        {
            int[] prices;

            switch (modType)
            {
                case VehicleMod.Suspension:
                    prices = SuspensionPrices;
                    break;
                case VehicleMod.Engine:
                    prices = EnginePrices;
                    break;
                case VehicleMod.Armor:
                    prices = ArmorPrices;
                    break;
                case VehicleMod.Brakes:
                    prices = BrakePrices;
                    break;
                case VehicleMod.Transmission:
                    prices = TransmissionPrices;
                    break;
                default:
                    return GetIndexedModPrice(index, 2000);
            }

            int priceIndex = index + 1;
            return priceIndex >= 0 && priceIndex < prices.Length
                ? prices[priceIndex]
                : GetIndexedModPrice(index, 2000);
        }

        public static void RefreshMenus()
        {
            if (arenavehicle.Contains(veh.Model))
            {
                RefreshBodyworkArenaMenu();
                RefreshWeaponMenu();
            }
            else if (arenawar.Contains(veh.Model))
            {
                RefreshArenaWarMenu();
            }
            else
            {
                RefreshBodyworkMenu();
            }
            RefreshModMenuFor(ref mAerials, ref iAerials, VehicleMod.Aerials);
            RefreshModMenuFor(ref mTrim, ref iTrim, VehicleMod.Trim);
            RefreshModMenuFor(ref mWindow, ref iWindows, VehicleMod.Windows);
            RefreshModMenuFor(ref mArchCover, ref iArchCover, VehicleMod.ArchCover);
            RefreshEngineMenu();
            RefreshPerformanceMenuFor(ref mEngine, ref iEngine, VehicleMod.Engine, "CMOD_ENG_");
            RefreshNitroMenu();
            RefreshModMenuFor(ref mEngineBlock, ref iEngineBlock, VehicleMod.EngineBlock);
            RefreshModMenuFor(ref mAirFilter, ref iAirFilter, VehicleMod.AirFilter);
            RefreshModMenuFor(ref mStruts, ref iStruts, VehicleMod.Struts);
            RefreshInteriorMenu();
            RefreshModMenuFor(ref mColumnShifterLevers, ref iColumnShifterLevers, VehicleMod.ColumnShifterLevers);
            RefreshModMenuFor(ref mDashboard, ref iDashboard, VehicleMod.Dashboard);
            RefreshEnumModMenuFor(ref mLightsColor, ref iLightsColor, EnumTypes.VehicleColorDashboard);
            RefreshModMenuFor(ref mDialDesign, ref iDialDesign, VehicleMod.DialDesign);
            RefreshModMenuFor(ref mOrnaments, ref iOrnaments, VehicleMod.Ornaments);
            RefreshModMenuFor(ref mSeats, ref iSeats, VehicleMod.Seats);
            RefreshModMenuFor(ref mSteeringWheels, ref iSteeringWheels, VehicleMod.SteeringWheels);
            RefreshModMenuFor(ref mTrimDesign, ref iTrimDesign, VehicleMod.TrimDesign);
            RefreshEnumModMenuFor(ref mTrimColor, ref iTrimColor, EnumTypes.VehicleColorTrim);
            RefreshModMenuFor(ref mDoor, ref iDoor, VehicleMod.DoorSpeakers);
            RefreshBumperMenu();
            RefreshModMenuFor(ref mFBumper, ref iFBumper, VehicleMod.FrontBumper);
            RefreshModMenuFor(ref mRBumper, ref iRBumper, VehicleMod.RearBumper);
            RefreshModMenuFor(ref mSSkirt, ref iSideSkirt, VehicleMod.SideSkirt);
            RefreshWheelsMenu();
            RefreshWheelTypeMenu();

            RefreshWheelRimMenu(ref gmBikeWheels, ref mSBikeWheels, ref mCBikeWheels, ref iSBikeWheels, ref iCBikeWheels);
            RefreshBikeWheelsModMenuFor(ref mSBikeWheels, ref iSBikeWheels, VehicleMod.RearWheel, false);
            RefreshBikeWheelsModMenuFor(ref mCBikeWheels, ref iCBikeWheels, VehicleMod.RearWheel, true);

            RefreshWheelRimMenu(ref gmHighEnd, ref mSHighEnd, ref mCHighEnd, ref iSHighEnd, ref iCHighEnd);
            RefreshStockWheelsModMenuFor(ref mSHighEnd, ref iSHighEnd, VehicleMod.FrontWheel);
            RefreshChromeWheelsModMenuFor(ref mCHighEnd, ref iCHighEnd, VehicleMod.FrontWheel);
            RefreshWheelRimMenu(ref gmLowrider, ref mSLowrider, ref mCLowrider, ref iSLowrider, ref iCLowrider);
            RefreshStockWheelsModMenuFor(ref mSLowrider, ref iSLowrider, VehicleMod.FrontWheel);
            RefreshChromeWheelsModMenuFor(ref mCLowrider, ref iCLowrider, VehicleMod.FrontWheel);
            RefreshWheelRimMenu(ref gmMuscle, ref mSMuscle, ref mCMuscle, ref iSMuscle, ref iCMuscle);
            RefreshStockWheelsModMenuFor(ref mSMuscle, ref iSMuscle, VehicleMod.FrontWheel);
            RefreshChromeWheelsModMenuFor(ref mCMuscle, ref iCMuscle, VehicleMod.FrontWheel);
            RefreshWheelRimMenu(ref gmOffroad, ref mSOffroad, ref mCOffroad, ref iSOffroad, ref iCOffroad);
            RefreshStockWheelsModMenuFor(ref mSOffroad, ref iSOffroad, VehicleMod.FrontWheel);
            RefreshChromeWheelsModMenuFor(ref mCOffroad, ref iCOffroad, VehicleMod.FrontWheel);
            RefreshWheelRimMenu(ref gmSport, ref mSSport, ref mCSport, ref iSSport, ref iCSport);
            RefreshStockWheelsModMenuFor(ref mSSport, ref iSSport, VehicleMod.FrontWheel);
            RefreshChromeWheelsModMenuFor(ref mCSport, ref iCSport, VehicleMod.FrontWheel);
            RefreshWheelRimMenu(ref gmSUV, ref mSSUV, ref mCSUV, ref iSSUV, ref iCSUV);
            RefreshStockWheelsModMenuFor(ref mSSUV, ref iSSUV, VehicleMod.FrontWheel);
            RefreshChromeWheelsModMenuFor(ref mCSUV, ref iCSUV, VehicleMod.FrontWheel);
            RefreshWheelRimMenu(ref gmTuner, ref mSTuner, ref mCTuner, ref iSTuner, ref iCTuner);
            RefreshStockWheelsModMenuFor(ref mSTuner, ref iSTuner, VehicleMod.FrontWheel);
            RefreshChromeWheelsModMenuFor(ref mCTuner, ref iCTuner, VehicleMod.FrontWheel);
            RefreshLowriderDLCWheelsModMenuFor(ref mBennysOriginals, ref iBennys, VehicleMod.FrontWheel);
            RefreshLowriderDLCWheelsModMenuFor(ref mBespoke, ref iBespoke, VehicleMod.FrontWheel);
            RefreshLowriderDLCWheelsModMenuFor(ref mRacing, ref iRacing, VehicleMod.FrontWheel);
            RefreshLowriderDLCWheelsModMenuFor(ref mStreet, ref iStreet, VehicleMod.FrontWheel);
            RefreshEnumModMenuFor(ref mRimColor, ref iRimColor, EnumTypes.VehicleColorRim);
            RefreshTyresMenu();
            RefreshRGBColorMenuFor(ref mTireSmoke, ref iTireSmoke, "Smoke");
            RefreshPlateMenu();
            RefreshModMenuFor(ref mPlateHolder, ref iPlateHolder, VehicleMod.PlateHolder);
            RefreshModMenuFor(ref mVanityPlates, ref iVanityPlates, VehicleMod.VanityPlates);
            RefreshEnumModMenuFor(ref mNumberPlate, ref iNumberPlate, EnumTypes.NumberPlateType);
            RefreshLightsMenu();
            RefreshModMenuForHeadlightsColor(ref mHeadlights, ref iHeadlights);
            RefreshNeonKitsMenu();
            RefreshNeonMenu();
            RefreshRGBColorMenuFor(ref mNeonColor, ref iNeonColor, "Neon");
            RefreshResprayMenu();
            RefreshPrimaryColorMenu();
            RefreshColorMenuFor(ref mPrimaryChromeColor, ref iPrimaryChromeColor, ChromeColor, "Primary");
            RefreshColorMenuFor(ref mPrimaryClassicColor, ref iPrimaryClassicColor, ClassicColor, "Primary");
            RefreshColorMenuFor(ref mPrimaryMetallicColor, ref iPrimaryMetallicColor, ClassicColor, "Primary");
            RefreshColorMenuFor(ref mPrimaryMetalsColor, ref iPrimaryMetalsColor, MetalColor, "Primary");
            RefreshColorMenuFor(ref mPrimaryMatteColor, ref iPrimaryMatteColor, MatteColor, "Primary");
            RefreshColorMenuFor(ref mPrimaryPearlescentColor, ref iPrimaryPearlescentColor, PearlescentColor, "Pearlescent");
            RefreshSecondaryColorMenu();
            RefreshColorMenuFor(ref mSecondaryChromeColor, ref iSecondaryChromeColor, ChromeColor, "Secondary");
            RefreshColorMenuFor(ref mSecondaryClassicColor, ref iSecondaryClassicColor, ClassicColor, "Secondary");
            RefreshColorMenuFor(ref mSecondaryMetallicColor, ref iSecondaryMetallicColor, ClassicColor, "Secondary");
            RefreshColorMenuFor(ref mSecondaryMetalsColor, ref iSecondaryMetalsColor, MetalColor, "Secondary");
            RefreshColorMenuFor(ref mSecondaryMatteColor, ref iSecondaryMatteColor, MatteColor, "Secondary");
            RefreshModMenuFor(ref mExhaust, ref iExhaust, VehicleMod.Exhaust);
            RefreshModMenuFor(ref mFender, ref iFender, VehicleMod.Fender);
            RefreshModMenuFor(ref mRFender, ref iRFender, VehicleMod.RightFender);
            RefreshModMenuFor(ref mFrame, ref iFrame, VehicleMod.Frame);
            RefreshModMenuFor(ref mGrille, ref iGrille, VehicleMod.Grille);
            RefreshModMenuFor(ref mHood, ref iHood, VehicleMod.Hood);
            RefreshModMenuFor(ref mHorn, ref iHorn, VehicleMod.Horns);
            RefreshModMenuFor(ref mHydraulics, ref iHydraulics, VehicleMod.Hydraulics);
            RefreshModMenuFor(ref mLivery, ref iLivery, VehicleMod.Livery);
            RefreshModMenuForLivery2(ref mTornadoC, ref iTornadoC);
            RefreshModMenuFor(ref mPlaques, ref iPlaques, VehicleMod.Plaques);
            RefreshModMenuFor(ref mRoof, ref iRoof, VehicleMod.Roof);
            RefreshModMenuFor(ref mSpeakers, ref iSpeaker, VehicleMod.Speakers);
            RefreshModMenuFor(ref mSpoilers, ref iSpoilers, VehicleMod.Spoilers);
            RefreshModMenuFor(ref mTank, ref iTank, VehicleMod.Tank);
            RefreshModMenuFor(ref mTrunk, ref iTrunk, VehicleMod.Trunk);
            RefreshModMenuFor(ref mTurbo, ref iTurbo, VehicleToggleMod.Turbo);
            RefreshPerformanceMenuFor(ref mSuspension, ref iSuspension, VehicleMod.Suspension, "CMOD_SUS_");
            RefreshPerformanceMenuFor(ref mArmor, ref iArmor, VehicleMod.Armor, "CMOD_ARM_");
            RefreshPerformanceMenuFor(ref mBrakes, ref iBrakes, VehicleMod.Brakes, "CMOD_BRA_");
            RefreshPerformanceMenuFor(ref mTransmission, ref iTransmission, VehicleMod.Transmission, "CMOD_GBX_");
            RefreshEnumModMenuFor(ref mTint, ref iTint, EnumTypes.VehicleWindowTint);
            // Motorcycles
            RefreshModMenuFor(ref mShifter, ref iShifter, VehicleMod.Fender);
            RefreshModMenuFor(ref mFMudguard, ref iFMudguard, VehicleMod.FrontBumper);
            RefreshModMenuFor(ref mBSeat, ref iBSeat, VehicleMod.Hood);
            RefreshModMenuFor(ref mOilTank, ref iOilTank, VehicleMod.Grille);
            RefreshModMenuFor(ref mRMudguard, ref iRMudguard, VehicleMod.RearBumper);
            RefreshModMenuFor(ref mFuelTank, ref iFuelTank, VehicleMod.Roof);
            RefreshModMenuFor(ref mBeltDriveCovers, ref iBeltDriveCovers, VehicleMod.Spoilers);
            RefreshModMenuFor(ref mBEngineBlock, ref iBEngineBlock, VehicleMod.Frame);
            RefreshModMenuFor(ref mBAirFilter, ref iBAirFilter, VehicleMod.SideSkirt);
            RefreshModMenuFor(ref mBTank, ref iBTank, VehicleMod.Tank);
            RefreshMainMenu();
        }

        private static void AddEngineItemsToMainMenu(bool motorcycle)
        {
            bool hasEngineUpgrade = veh.GetModCount(VehicleMod.Engine) != 0;
            bool hasNitro = veh.CanInstallNitroMod();
            bool hasBennysEngineOptions = motorcycle
                ? veh.GetModCount(VehicleMod.Frame) != 0 || veh.GetModCount(VehicleMod.SideSkirt) != 0
                : veh.GetModCount(VehicleMod.EngineBlock) != 0 || veh.GetModCount(VehicleMod.AirFilter) != 0 || veh.GetModCount(VehicleMod.Struts) != 0;

            if (!hasEngineUpgrade && !hasNitro && !hasBennysEngineOptions)
            {
                return;
            }

            if (bennysvehicle.Contains(veh.Model))
            {
                giEngine = new UIMenuItem(LocalizedModGroupName(GroupName.Engine), Game.GetLocalizedString("CMOD_SMOD_2_D"));
                MainMenu.AddItem(giEngine);
                MainMenu.BindMenuToItem(gmEngine, giEngine);
                return;
            }

            // A stock vehicle goes straight to its engine upgrades. The extra
            // Engine -> Engine level is reserved for converted Benny's vehicles,
            // where several engine-customization categories actually exist.
            if (hasEngineUpgrade)
            {
                giEngine = new UIMenuItem(LocalizedModGroupName(GroupName.Engine), Game.GetLocalizedString("CMOD_SMOD_2_D"));
                MainMenu.AddItem(giEngine);
                MainMenu.BindMenuToItem(mEngine, giEngine);
            }

            if (motorcycle)
            {
                if (veh.GetModCount(VehicleMod.Frame) != 0)
                {
                    giBEngineBlock = new UIMenuItem(LocalizedModTypeName(VehicleMod.Frame), Game.GetLocalizedString("SMOD_ENGINE_1"));
                    MainMenu.AddItem(giBEngineBlock);
                    MainMenu.BindMenuToItem(mBEngineBlock, giBEngineBlock);
                }
                if (veh.GetModCount(VehicleMod.SideSkirt) != 0)
                {
                    giBAirFilter = new UIMenuItem(LocalizedModTypeName(VehicleMod.SideSkirt), Game.GetLocalizedString("CMOD_SMOD_2_D"));
                    MainMenu.AddItem(giBAirFilter);
                    MainMenu.BindMenuToItem(mBAirFilter, giBAirFilter);
                }
            }
            else
            {
                if (veh.GetModCount(VehicleMod.EngineBlock) != 0)
                {
                    iEngineBlock = new UIMenuItem(LocalizedModTypeName(VehicleMod.EngineBlock), Game.GetLocalizedString("SMOD_ENGINE_1"));
                    MainMenu.AddItem(iEngineBlock);
                    MainMenu.BindMenuToItem(mEngineBlock, iEngineBlock);
                }
                if (veh.GetModCount(VehicleMod.AirFilter) != 0)
                {
                    giAirfilter = new UIMenuItem(LocalizedModTypeName(VehicleMod.AirFilter), Game.GetLocalizedString("SMOD_ENGINE_2"));
                    MainMenu.AddItem(giAirfilter);
                    MainMenu.BindMenuToItem(mAirFilter, giAirfilter);
                }
                if (veh.GetModCount(VehicleMod.Struts) != 0)
                {
                    giStruts = new UIMenuItem(LocalizedModTypeName(VehicleMod.Struts), Game.GetLocalizedString("SMOD_ENGINE_3b"));
                    MainMenu.AddItem(giStruts);
                    MainMenu.BindMenuToItem(mStruts, giStruts);
                }
            }

            if (hasNitro)
            {
                iNitro = new UIMenuItem(Game.GetLocalizedString("CMM_MOD_NJBOS"), Game.GetLocalizedString("SMOD_ENGINE_2"));
                MainMenu.AddItem(iNitro);
                MainMenu.BindMenuToItem(mNitro, iNitro);
            }
        }

        private static void MoveMainMenuItemToEnd(UIMenuItem item)
        {
            if (item != null && MainMenu.MenuItems.Remove(item))
            {
                MainMenu.AddItem(item);
            }
        }

        private static void ApplyMainMenuCategoryOrder()
        {
            // Keep Benny's upgrade/weapon entries at the top, then match the
            // category flow used by Los Santos Customs.
            MoveMainMenuItemToEnd(iArmor);
            MoveMainMenuItemToEnd(giBrakes);
            MoveMainMenuItemToEnd(giBumper);
            MoveMainMenuItemToEnd(giBodywork);
            MoveMainMenuItemToEnd(giEngine);
            MoveMainMenuItemToEnd(giAirfilter);
            MoveMainMenuItemToEnd(giStruts);
            MoveMainMenuItemToEnd(giExhaust);
            MoveMainMenuItemToEnd(iFender);
            MoveMainMenuItemToEnd(iRFender);
            MoveMainMenuItemToEnd(iFrame);
            MoveMainMenuItemToEnd(giGrille);
            MoveMainMenuItemToEnd(giHood);
            MoveMainMenuItemToEnd(iHorn);
            MoveMainMenuItemToEnd(giHydraulics);
            MoveMainMenuItemToEnd(giInterior);
            MoveMainMenuItemToEnd(iSpeaker);
            MoveMainMenuItemToEnd(giLights);
            MoveMainMenuItemToEnd(iLivery);
            MoveMainMenuItemToEnd(iTornadoC);
            MoveMainMenuItemToEnd(giPlate);
            MoveMainMenuItemToEnd(giNumberPlate);
            MoveMainMenuItemToEnd(giPlateHolder);
            MoveMainMenuItemToEnd(giRespray);
            MoveMainMenuItemToEnd(iRoof);
            MoveMainMenuItemToEnd(giTank);
            MoveMainMenuItemToEnd(giPlaques);
            MoveMainMenuItemToEnd(giSpoilers);
            MoveMainMenuItemToEnd(iSuspension);
            MoveMainMenuItemToEnd(iTransmission);
            MoveMainMenuItemToEnd(giTrunk);
            MoveMainMenuItemToEnd(iTurbo);
            MoveMainMenuItemToEnd(giWheels);
            MoveMainMenuItemToEnd(iTint);
        }

        public static void RefreshMainMenu()
        {
            try
            {
                MainMenu.MenuItems.Clear();

                if (veh.HasDamageDecals && !isRepairing)
                {
                    iRepair = new UIMenuItem(LocalizedModGroupName(GroupName.Repair), Game.GetLocalizedString("CMOD_MOD_0_D"));
                    {
                        var __with1 = iRepair;
                        __with1.SetRightLabel("$" + veh.GetRepairPrice());
                        __with1.Tag = veh.GetRepairPrice();
                    }
                    MainMenu.AddItem(iRepair);
                    MainMenu.RefreshIndex();
                    PlaySpeech("SHOP_SELL_REPAIR");
                }
                else if (veh.ClassType == VehicleClass.Motorcycles || veh.Model.ToString().Equals("blazer4", StringComparison.OrdinalIgnoreCase))
                {
                    // Specials
                    if (lowriders.Contains(veh.Model))
                    {
                        iUpgrade = new UIMenuItem(LocalizedModGroupName(GroupName.Upgrade), Game.GetLocalizedString("CMOD_MOD_100_D"));
                        {
                            var __with1 = iUpgrade;
                            __with1.SetRightLabel("$" + veh.Model.GetUpgradePrice());
                            __with1.Tag = veh.Model.GetUpgradePrice();
                        }
                        MainMenu.AddItem(iUpgrade);
                    }
                    if (TryGetLowriderBaseModel(veh.Model, out Model motorcycleBaseModel))
                    {
                        iRemoveUpgrade = new UIMenuItem("Remove Upgrade", "Restore the original vehicle model.");
                        iRemoveUpgrade.SetRightLabel("$0");
                        iRemoveUpgrade.Tag = motorcycleBaseModel;
                        MainMenu.AddItem(iRemoveUpgrade);
                    }
                    if (veh.DisplayName.IsUpgradeModExist())
                    {
                        Tuple<string, int> upgrade2 = veh.DisplayName.GetUpgradeModVehicleInfo();
                        iUpgradeMod = new UIMenuItem(LocalizedModGroupName(GroupName.Upgrade), Game.GetLocalizedString("CMOD_MOD_100_D"));
                        {
                            var __with1 = iUpgradeMod;
                            __with1.SetRightLabel("$" + upgrade2.Item2);
                            __with1.Tag = upgrade2.Item2;
                        }
                        MainMenu.AddItem(iUpgradeMod);
                    }

                    if (arenavehicle.Contains(veh.Model))
                    {
                        // Groups
                        if ((veh.GetModCount(VehicleMod.ArchCover) != 0 || veh.GetModCount(VehicleMod.RightFender) != 0 || veh.GetModCount(VehicleMod.Tank) != 0 || veh.GetModCount(VehicleMod.Roof) != 0))
                        {
                            giWeapon = new UIMenuItem(LocalizedModGroupName(GroupName.Weapons), Game.GetLocalizedString("CMOD_WEAPO_D"));
                            MainMenu.AddItem(giWeapon);
                            MainMenu.BindMenuToItem(gmWeapon, giWeapon);
                        }
                        if ((veh.GetModCount(VehicleMod.Plaques) != 0 || veh.GetModCount(VehicleMod.Frame) != 0 || veh.GetModCount(VehicleMod.Aerials) != 0 || veh.GetModCount(VehicleMod.Trim) != 0 || veh.GetModCount(VehicleMod.VanityPlates) != 0 || veh.GetModCount(VehicleMod.Ornaments) != 0))
                        {
                            giBodyworkArena = new UIMenuItem(LocalizedModGroupName(GroupName.Bodyworks), Game.GetLocalizedString("IE_BO_DT1"));
                            MainMenu.AddItem(giBodyworkArena);
                            MainMenu.BindMenuToItem(gmBodyworkArena, giBodyworkArena);
                        }
                        if ((veh.GetModCount(VehicleMod.Engine) != 0 || veh.GetModCount(VehicleMod.EngineBlock) != 0))
                        {
                            giEngine = new UIMenuItem(LocalizedModGroupName(GroupName.Engine), Game.GetLocalizedString("CMOD_SMOD_2_D"));
                            MainMenu.AddItem(giEngine);
                            MainMenu.BindMenuToItem(gmEngine, giEngine);
                        }

                        // Single Item
                        if (veh.GetModCount(VehicleMod.AirFilter) != 0)
                        {
                            giAirfilter = new UIMenuItem(LocalizedModTypeName(VehicleMod.AirFilter), Game.GetLocalizedString("SMOD_ENGINE_2"));
                            MainMenu.AddItem(giAirfilter);
                            MainMenu.BindMenuToItem(mAirFilter, giAirfilter);
                        }
                        if (veh.GetModCount(VehicleMod.Struts) != 0)
                        {
                            giStruts = new UIMenuItem(LocalizedModTypeName(VehicleMod.Struts), Game.GetLocalizedString("SMOD_ENGINE_3b"));
                            MainMenu.AddItem(giStruts);
                            MainMenu.BindMenuToItem(mStruts, giStruts);
                        }
                        if (veh.GetModCount(VehicleMod.PlateHolder) != 0)
                        {
                            giPlateHolder = new UIMenuItem(LocalizedModTypeName(VehicleMod.PlateHolder), Game.GetLocalizedString("CMOD_MOD_49_D"));
                            MainMenu.AddItem(giPlateHolder);
                            MainMenu.BindMenuToItem(mPlateHolder, giPlateHolder);
                        }
                        if (veh.GetModCount(VehicleMod.Speakers) != 0)
                        {
                            iSpeaker = new UIMenuItem(LocalizedModTypeName(VehicleMod.Speakers), Game.GetLocalizedString("CMOD_MOD_58_D"));
                            MainMenu.AddItem(iSpeaker);
                            MainMenu.BindMenuToItem(mSpeakers, iSpeaker);
                        }
                        giNumberPlate = new UIMenuItem(LocalizedModGroupName(GroupName.License), Game.GetLocalizedString("CMOD_MOD_18_D"));
                        MainMenu.AddItem(giNumberPlate);
                        MainMenu.BindMenuToItem(mNumberPlate, giNumberPlate);
                    }
                    else
                    {
                        // Groups
                        if ((veh.GetModCount(VehicleMod.Fender) != 0 || veh.GetModCount(VehicleMod.FrontBumper) != 0 || veh.GetModCount(VehicleMod.Hood) != 0 || veh.GetModCount(VehicleMod.Grille) != 0 || veh.GetModCount(VehicleMod.RearBumper) != 0 || veh.GetModCount(VehicleMod.Roof) != 0 || veh.GetModCount(VehicleMod.Spoilers) != 0))
                        {
                            giBodywork = new UIMenuItem(LocalizedModGroupName(GroupName.Bodyworks), Game.GetLocalizedString("IE_BO_DT1"));
                            MainMenu.AddItem(giBodywork);
                            MainMenu.BindMenuToItem(gmBodywork, giBodywork);
                        }
                        AddEngineItemsToMainMenu(true);
                        giPlate = new UIMenuItem(LocalizedModGroupName(GroupName.Plate), Game.GetLocalizedString("CMOD_MOD_18_D"));
                        MainMenu.AddItem(giPlate);
                        MainMenu.BindMenuToItem(gmPlate, giPlate);
                    }

                    giWheels = new UIMenuItem(LocalizedModGroupName(GroupName.Wheels), Game.GetLocalizedString("CMOD_MOD_60_D"));
                    MainMenu.AddItem(giWheels);
                    MainMenu.BindMenuToItem(gmWheels, giWheels);
                    giLights = new UIMenuItem(LocalizedModGroupName(GroupName.Lights), Game.GetLocalizedString("CMOD_MOD_15_D"));
                    MainMenu.AddItem(giLights);
                    MainMenu.BindMenuToItem(gmLights, giLights);
                    giRespray = new UIMenuItem(LocalizedModGroupName(GroupName.Respray), Game.GetLocalizedString("CMOD_MOD_6_D"));
                    MainMenu.AddItem(giRespray);
                    MainMenu.BindMenuToItem(gmRespray, giRespray);

                    // Single Item
                    if (veh.GetModCount(VehicleMod.Armor) != 0)
                    {
                        iArmor = new UIMenuItem(LocalizedModTypeName(VehicleMod.Armor), Game.GetLocalizedString("CMOD_MOD_1_D"));
                        MainMenu.AddItem(iArmor);
                        MainMenu.BindMenuToItem(mArmor, iArmor);
                    }
                    if (veh.GetModCount(VehicleMod.Brakes) != 0)
                    {
                        giBrakes = new UIMenuItem(LocalizedModTypeName(VehicleMod.Brakes), Game.GetLocalizedString("CMOD_MOD_3_D"));
                        MainMenu.AddItem(giBrakes);
                        MainMenu.BindMenuToItem(mBrakes, giBrakes);
                    }
                    if (veh.GetModCount(VehicleMod.Exhaust) != 0)
                    {
                        giExhaust = new UIMenuItem(LocalizedModTypeName(VehicleMod.Exhaust), Game.GetLocalizedString("CMOD_MOD_16_D"));
                        MainMenu.AddItem(giExhaust);
                        MainMenu.BindMenuToItem(mExhaust, giExhaust);
                    }
                    if (veh.GetModCount(VehicleMod.Horns) != 0)
                    {
                        iHorn = new UIMenuItem(LocalizedModTypeName(VehicleMod.Horns), Game.GetLocalizedString("CMOD_MOD_14_D"));
                        MainMenu.AddItem(iHorn);
                        MainMenu.BindMenuToItem(mHorn, iHorn);
                    }
                    if (veh.GetModCount(VehicleMod.Hydraulics) != 0)
                    {
                        giHydraulics = new UIMenuItem(LocalizedModTypeName(VehicleMod.Hydraulics), Game.GetLocalizedString("CMOD_SMOD_5_D"));
                        MainMenu.AddItem(giHydraulics);
                        MainMenu.BindMenuToItem(mHydraulics, giHydraulics);
                    }
                    if (veh.GetModCount(VehicleMod.Livery) != 0)
                    {
                        iLivery = new UIMenuItem(LocalizedModTypeName(VehicleMod.Livery), Game.GetLocalizedString("CMOD_SMOD_6_D"));
                        MainMenu.AddItem(iLivery);
                        MainMenu.BindMenuToItem(mLivery, iLivery);
                    }
                    if (veh.Livery2Count() != 0)
                    {
                        iTornadoC = new UIMenuItem(LocalizedModTypeName(VehicleMod.Roof), Game.GetLocalizedString("CMOD_SMOD_6_D"));
                        MainMenu.AddItem(iTornadoC);
                        MainMenu.BindMenuToItem(mTornadoC, iTornadoC);
                    }
                    if (veh.GetModCount(VehicleMod.Plaques) != 0)
                    {
                        giPlaques = new UIMenuItem(LocalizedModTypeName(VehicleMod.Plaques), Game.GetLocalizedString("SMOD_IN_PLAQUE"));
                        MainMenu.AddItem(giPlaques);
                        MainMenu.BindMenuToItem(mPlaques, giPlaques);
                    }
                    if (veh.GetModCount(VehicleMod.Suspension) != 0)
                    {
                        iSuspension = new UIMenuItem(LocalizedModTypeName(VehicleMod.Suspension), Game.GetLocalizedString("CMOD_MOD_24_D"));
                        MainMenu.AddItem(iSuspension);
                        MainMenu.BindMenuToItem(mSuspension, iSuspension);
                    }
                    if (veh.GetModCount(VehicleMod.Transmission) != 0)
                    {
                        iTransmission = new UIMenuItem(LocalizedModTypeName(VehicleMod.Transmission), Game.GetLocalizedString("CMOD_MOD_26_D"));
                        MainMenu.AddItem(iTransmission);
                        MainMenu.BindMenuToItem(mTransmission, iTransmission);
                    }
                    if (veh.GetModCount(VehicleMod.Trunk) != 0)
                    {
                        giTrunk = new UIMenuItem(LocalizedModTypeName(VehicleMod.Trunk), Game.GetLocalizedString("CMOD_MOD_62_D"));
                        MainMenu.AddItem(giTrunk);
                        MainMenu.BindMenuToItem(mTrunk, giTrunk);
                    }
                    iTurbo = new UIMenuItem(LocalizedModTypeName(VehicleToggleMod.Turbo), Game.GetLocalizedString("CMOD_MOD_27_D"));
                    MainMenu.AddItem(iTurbo);
                    MainMenu.BindMenuToItem(mTurbo, iTurbo);
                    MainMenu.RefreshIndex();
                }
                else
                {
                    // Specials
                    if (lowriders.Contains(veh.Model))
                    {
                        iUpgrade = new UIMenuItem(LocalizedModGroupName(GroupName.Upgrade), Game.GetLocalizedString("CMOD_MOD_100_D"));
                        {
                            var __with1 = iUpgrade;
                            __with1.SetRightLabel("$" + veh.Model.GetUpgradePrice());
                            __with1.Tag = Convert.ToInt32(veh.Model.GetUpgradePrice());
                        }
                        MainMenu.AddItem(iUpgrade);
                    }
                    if (TryGetLowriderBaseModel(veh.Model, out Model baseModel))
                    {
                        iRemoveUpgrade = new UIMenuItem("Remove Upgrade", "Restore the original vehicle model.");
                        iRemoveUpgrade.SetRightLabel("$0");
                        iRemoveUpgrade.Tag = baseModel;
                        MainMenu.AddItem(iRemoveUpgrade);
                    }
                    if (veh.DisplayName.IsUpgradeModExist())
                    {
                        Tuple<string, int> upgrade2 = veh.DisplayName.GetUpgradeModVehicleInfo();
                        iUpgradeMod = new UIMenuItem(LocalizedModGroupName(GroupName.Upgrade), Game.GetLocalizedString("CMOD_MOD_100_D"));
                        {
                            var __with1 = iUpgradeMod;
                            __with1.SetRightLabel("$" + upgrade2.Item2);
                            __with1.Tag = upgrade2.Item2;
                        }
                        MainMenu.AddItem(iUpgradeMod);
                    }


                    if (arenavehicle.Contains(veh.Model))
                    {
                        // Groups
                        if ((veh.GetModCount(VehicleMod.ArchCover) != 0 || veh.GetModCount(VehicleMod.RightFender) != 0 || veh.GetModCount(VehicleMod.Tank) != 0 || veh.GetModCount(VehicleMod.Roof) != 0))
                        {
                            giWeapon = new UIMenuItem(LocalizedModGroupName(GroupName.Weapons), Game.GetLocalizedString("CMOD_WEAPO_D"));
                            MainMenu.AddItem(giWeapon);
                            MainMenu.BindMenuToItem(gmWeapon, giWeapon);
                        }
                        if ((veh.GetModCount(VehicleMod.Plaques) != 0 || veh.GetModCount(VehicleMod.Frame) != 0 || veh.GetModCount(VehicleMod.Aerials) != 0 || veh.GetModCount(VehicleMod.Trim) != 0 || veh.GetModCount(VehicleMod.VanityPlates) != 0 || veh.GetModCount(VehicleMod.Ornaments) != 0))
                        {
                            giBodyworkArena = new UIMenuItem(LocalizedModGroupName(GroupName.Bodyworks), Game.GetLocalizedString("IE_BO_DT1"));
                            MainMenu.AddItem(giBodyworkArena);
                            MainMenu.BindMenuToItem(gmBodyworkArena, giBodyworkArena);
                        }
                        if ((veh.GetModCount(VehicleMod.Engine) != 0 || veh.GetModCount(VehicleMod.EngineBlock) != 0))
                        {
                            giEngine = new UIMenuItem(LocalizedModGroupName(GroupName.Engine), Game.GetLocalizedString("CMOD_SMOD_2_D"));
                            MainMenu.AddItem(giEngine);
                            MainMenu.BindMenuToItem(gmEngine, giEngine);
                        }

                        // Single Item
                        if (veh.GetModCount(VehicleMod.AirFilter) != 0)
                        {
                            giAirfilter = new UIMenuItem(LocalizedModTypeName(VehicleMod.AirFilter), Game.GetLocalizedString("SMOD_ENGINE_2"));
                            MainMenu.AddItem(giAirfilter);
                            MainMenu.BindMenuToItem(mAirFilter, giAirfilter);
                        }
                        if (veh.GetModCount(VehicleMod.Struts) != 0)
                        {
                            giStruts = new UIMenuItem(LocalizedModTypeName(VehicleMod.Struts), Game.GetLocalizedString("SMOD_ENGINE_3b"));
                            MainMenu.AddItem(giStruts);
                            MainMenu.BindMenuToItem(mStruts, giStruts);
                        }
                        if (veh.GetModCount(VehicleMod.PlateHolder) != 0)
                        {
                            giPlateHolder = new UIMenuItem(LocalizedModTypeName(VehicleMod.PlateHolder), Game.GetLocalizedString("CMOD_MOD_49_D"));
                            MainMenu.AddItem(giPlateHolder);
                            MainMenu.BindMenuToItem(mPlateHolder, giPlateHolder);
                        }
                        if (veh.GetModCount(VehicleMod.Speakers) != 0)
                        {
                            iSpeaker = new UIMenuItem(LocalizedModTypeName(VehicleMod.Speakers), Game.GetLocalizedString("CMOD_MOD_58_D"));
                            MainMenu.AddItem(iSpeaker);
                            MainMenu.BindMenuToItem(mSpeakers, iSpeaker);
                        }
                        giNumberPlate = new UIMenuItem(LocalizedModGroupName(GroupName.License), Game.GetLocalizedString("CMOD_MOD_18_D"));
                        MainMenu.AddItem(giNumberPlate);
                        MainMenu.BindMenuToItem(mNumberPlate, giNumberPlate);
                    }
                    else
                    {
                        // Groups
                        if ((veh.GetModCount(VehicleMod.Aerials) != 0 || veh.GetModCount(VehicleMod.Trim) != 0 || veh.GetModCount(VehicleMod.Windows) != 0 || veh.GetModCount(VehicleMod.ArchCover) != 0))
                        {
                            giBodywork = new UIMenuItem(LocalizedModGroupName(GroupName.Bodyworks), Game.GetLocalizedString("IE_BO_DT1"));
                            MainMenu.AddItem(giBodywork);
                            MainMenu.BindMenuToItem(gmBodywork, giBodywork);
                        }
                        AddEngineItemsToMainMenu(false);
                        if ((veh.GetModCount(VehicleMod.ColumnShifterLevers) != 0 || veh.GetModCount(VehicleMod.Dashboard) != 0 || veh.GetModCount(VehicleMod.DialDesign) != 0 || veh.GetModCount(VehicleMod.Ornaments) != 0 || veh.GetModCount(VehicleMod.Seats) != 0 || veh.GetModCount(VehicleMod.SteeringWheels) != 0 || veh.GetModCount(VehicleMod.TrimDesign) != 0 || veh.GetModCount(VehicleMod.DoorSpeakers) != 0 || veh.GetModCount(VehicleMod.Speakers) != 0))
                        {
                            giInterior = new UIMenuItem(LocalizedModGroupName(GroupName.Interior), Game.GetLocalizedString("SMOD_IN_1"));
                            MainMenu.AddItem(giInterior);
                            MainMenu.BindMenuToItem(gmInterior, giInterior);
                        }
                        giPlate = new UIMenuItem(LocalizedModGroupName(GroupName.Plate), Game.GetLocalizedString("CMOD_MOD_18_D"));
                        MainMenu.AddItem(giPlate);
                        MainMenu.BindMenuToItem(gmPlate, giPlate);

                        // Single Item
                        if (veh.GetModCount(VehicleMod.Frame) != 0)
                        {
                            iFrame = new UIMenuItem(LocalizedModTypeName(VehicleMod.Frame), Game.GetLocalizedString("SMOD_ROLLCAGE_1"));
                            MainMenu.AddItem(iFrame);
                            MainMenu.BindMenuToItem(mFrame, iFrame);
                        }
                        if (veh.GetModCount(VehicleMod.RightFender) != 0)
                        {
                            iRFender = new UIMenuItem(LocalizedModTypeName(VehicleMod.RightFender), Game.GetLocalizedString("CMOD_MOD_9_D"));
                            MainMenu.AddItem(iRFender);
                            MainMenu.BindMenuToItem(mRFender, iRFender);
                        }
                        if (veh.GetModCount(VehicleMod.Roof) != 0)
                        {
                            iRoof = new UIMenuItem(LocalizedModTypeName(VehicleMod.Roof), Game.GetLocalizedString("CMOD_MOD_73_D"));
                            MainMenu.AddItem(iRoof);
                            MainMenu.BindMenuToItem(mRoof, iRoof);
                        }
                        if (veh.GetModCount(VehicleMod.Tank) != 0)
                        {
                            giTank = new UIMenuItem(LocalizedModTypeName(VehicleMod.Tank), Game.GetLocalizedString("CMOD_MOD_45_D"));
                            MainMenu.AddItem(giTank);
                            MainMenu.BindMenuToItem(mTank, giTank);
                        }
                        if (veh.GetModCount(VehicleMod.Plaques) != 0)
                        {
                            giPlaques = new UIMenuItem(LocalizedModTypeName(VehicleMod.Plaques), Game.GetLocalizedString("SMOD_IN_PLAQUE"));
                            MainMenu.AddItem(giPlaques);
                            MainMenu.BindMenuToItem(mPlaques, giPlaques);
                        }
                    }

                    if ((veh.GetModCount(VehicleMod.FrontBumper) != 0 || veh.GetModCount(VehicleMod.RearBumper) != 0 || veh.GetModCount(VehicleMod.SideSkirt) != 0))
                    {
                        giBumper = new UIMenuItem(LocalizedModGroupName(GroupName.Bumpers), Game.GetLocalizedString("CMOD_MOD_4_D"));
                        MainMenu.AddItem(giBumper);
                        MainMenu.BindMenuToItem(gmBumper, giBumper);
                    }

                    giWheels = new UIMenuItem(LocalizedModGroupName(GroupName.Wheels), Game.GetLocalizedString("CMOD_MOD_60_D"));
                    MainMenu.AddItem(giWheels);
                    MainMenu.BindMenuToItem(gmWheels, giWheels);
                    giLights = new UIMenuItem(LocalizedModGroupName(GroupName.Lights), Game.GetLocalizedString("CMOD_MOD_15_D"));
                    MainMenu.AddItem(giLights);
                    MainMenu.BindMenuToItem(gmLights, giLights);
                    giRespray = new UIMenuItem(LocalizedModGroupName(GroupName.Respray), Game.GetLocalizedString("CMOD_MOD_6_D"));
                    MainMenu.AddItem(giRespray);
                    MainMenu.BindMenuToItem(gmRespray, giRespray);

                    // Single Item
                    if (veh.GetModCount(VehicleMod.Armor) != 0)
                    {
                        iArmor = new UIMenuItem(LocalizedModTypeName(VehicleMod.Armor), Game.GetLocalizedString("CMOD_MOD_1_D"));
                        MainMenu.AddItem(iArmor);
                        MainMenu.BindMenuToItem(mArmor, iArmor);
                    }
                    if (veh.GetModCount(VehicleMod.Brakes) != 0)
                    {
                        giBrakes = new UIMenuItem(LocalizedModTypeName(VehicleMod.Brakes), Game.GetLocalizedString("CMOD_MOD_3_D"));
                        MainMenu.AddItem(giBrakes);
                        MainMenu.BindMenuToItem(mBrakes, giBrakes);
                    }

                    if (veh.GetModCount(VehicleMod.Exhaust) != 0)
                    {
                        giExhaust = new UIMenuItem(LocalizedModTypeName(VehicleMod.Exhaust), Game.GetLocalizedString("CMOD_MOD_16_D"));
                        MainMenu.AddItem(giExhaust);
                        MainMenu.BindMenuToItem(mExhaust, giExhaust);
                    }
                    if (veh.GetModCount(VehicleMod.Fender) != 0)
                    {
                        iFender = new UIMenuItem(LocalizedModTypeName(VehicleMod.Fender), Game.GetLocalizedString("CMOD_MOD_9_D"));
                        MainMenu.AddItem(iFender);
                        MainMenu.BindMenuToItem(mFender, iFender);
                    }
                    if (veh.GetModCount(VehicleMod.Grille) != 0)
                    {
                        giGrille = new UIMenuItem(LocalizedModTypeName(VehicleMod.Grille), Game.GetLocalizedString("SMOD_CHASS_2c"));
                        MainMenu.AddItem(giGrille);
                        MainMenu.BindMenuToItem(mGrille, giGrille);
                    }
                    if (veh.GetModCount(VehicleMod.Hood) != 0)
                    {
                        giHood = new UIMenuItem(LocalizedModTypeName(VehicleMod.Hood), Game.GetLocalizedString("CMOD_MOD_72_D"));
                        MainMenu.AddItem(giHood);
                        MainMenu.BindMenuToItem(mHood, giHood);
                    }
                    if (veh.GetModCount(VehicleMod.Horns) != 0)
                    {
                        iHorn = new UIMenuItem(LocalizedModTypeName(VehicleMod.Horns), Game.GetLocalizedString("CMOD_MOD_14_D"));
                        MainMenu.AddItem(iHorn);
                        MainMenu.BindMenuToItem(mHorn, iHorn);
                    }
                    if (veh.GetModCount(VehicleMod.Hydraulics) != 0)
                    {
                        giHydraulics = new UIMenuItem(LocalizedModTypeName(VehicleMod.Hydraulics), Game.GetLocalizedString("CMOD_SMOD_5_D"));
                        MainMenu.AddItem(giHydraulics);
                        MainMenu.BindMenuToItem(mHydraulics, giHydraulics);
                    }
                    if (veh.GetModCount(VehicleMod.Livery) != 0)
                    {
                        iLivery = new UIMenuItem(LocalizedModTypeName(VehicleMod.Livery), Game.GetLocalizedString("CMOD_SMOD_6_D"));
                        MainMenu.AddItem(iLivery);
                        MainMenu.BindMenuToItem(mLivery, iLivery);
                    }
                    if (veh.Livery2Count() != 0)
                    {
                        iTornadoC = new UIMenuItem(LocalizedModTypeName(VehicleMod.Roof), Game.GetLocalizedString("CMOD_MOD_73_D"));
                        MainMenu.AddItem(iTornadoC);
                        MainMenu.BindMenuToItem(mTornadoC, iTornadoC);
                    }

                    if (veh.GetModCount(VehicleMod.Spoilers) != 0)
                    {
                        giSpoilers = new UIMenuItem(LocalizedModTypeName(VehicleMod.Spoilers), Game.GetLocalizedString("CMOD_MOD_37_D"));
                        MainMenu.AddItem(giSpoilers);
                        MainMenu.BindMenuToItem(mSpoilers, giSpoilers);
                    }
                    if (veh.GetModCount(VehicleMod.Suspension) != 0)
                    {
                        iSuspension = new UIMenuItem(LocalizedModTypeName(VehicleMod.Suspension), Game.GetLocalizedString("CMOD_MOD_24_D"));
                        MainMenu.AddItem(iSuspension);
                        MainMenu.BindMenuToItem(mSuspension, iSuspension);
                    }

                    if (veh.GetModCount(VehicleMod.Transmission) != 0)
                    {
                        iTransmission = new UIMenuItem(LocalizedModTypeName(VehicleMod.Transmission), Game.GetLocalizedString("CMOD_MOD_26_D"));
                        MainMenu.AddItem(iTransmission);
                        MainMenu.BindMenuToItem(mTransmission, iTransmission);
                    }
                    if (veh.GetModCount(VehicleMod.Trunk) != 0)
                    {
                        giTrunk = new UIMenuItem(LocalizedModTypeName(VehicleMod.Trunk), Game.GetLocalizedString("CMOD_MOD_62_D"));
                        MainMenu.AddItem(giTrunk);
                        MainMenu.BindMenuToItem(mTrunk, giTrunk);
                    }
                    iTurbo = new UIMenuItem(LocalizedModTypeName(VehicleToggleMod.Turbo), Game.GetLocalizedString("CMOD_MOD_27_D"));
                    MainMenu.AddItem(iTurbo);
                    MainMenu.BindMenuToItem(mTurbo, iTurbo);
                    if (veh.HasBone("windscreen"))
                    {
                        iTint = new UIMenuItem(LocalizedModGroupName(GroupName.Windows), Game.GetLocalizedString("CMOD_MOD_29_D"));
                        MainMenu.AddItem(iTint);
                        MainMenu.BindMenuToItem(mTint, iTint);
                    }
                    // If IsVehicleAttachedToTrailer(veh) Then
                    // giTrailer = New UIMenuItem(Game.GetGXTEntry("TRAILER"))
                    // MainMenu.AddItem(giTrailer)
                    // MainMenu.BindMenuToItem(gmTrailer, giTrailer)
                    // End If
                    MainMenu.RefreshIndex();
                }

                ApplyMainMenuCategoryOrder();
                MainMenu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshBodyworkArenaMenu()
        {
            try
            {
                gmBodyworkArena.MenuItems.Clear();

                if (veh.ClassType == VehicleClass.Motorcycles)
                {
                    if (veh.GetModCount(VehicleMod.Plaques) != 0)
                    {
                        iPlaques = new UIMenuItem(LocalizedModTypeName(VehicleMod.Plaques), Game.GetLocalizedString("collision_di2ru"));
                        gmBodyworkArena.AddItem(iPlaques);
                        gmBodyworkArena.BindMenuToItem(mPlaques, iPlaques);
                    }
                    if (veh.GetModCount(VehicleMod.Frame) != 0)
                    {
                        iFrame = new UIMenuItem(LocalizedModTypeName(VehicleMod.Frame), Game.GetLocalizedString("CMOD_ARMPL_D"));
                        gmBodyworkArena.AddItem(iFrame);
                        gmBodyworkArena.BindMenuToItem(mFrame, iFrame);
                    }
                    if (veh.GetModCount(VehicleMod.Aerials) != 0)
                    {
                        iAerials = new UIMenuItem(LocalizedModTypeName(VehicleMod.Aerials), Game.GetLocalizedString("collision_37l2i4l"));
                        gmBodyworkArena.AddItem(iAerials);
                        gmBodyworkArena.BindMenuToItem(mAerials, iAerials);
                    }
                    if (veh.GetModCount(VehicleMod.Trim) != 0)
                    {
                        iTrim = new UIMenuItem(LocalizedModTypeName(VehicleMod.Trim), Game.GetLocalizedString("collision_8t77hko"));
                        gmBodyworkArena.AddItem(iTrim);
                        gmBodyworkArena.BindMenuToItem(mTrim, iTrim);
                    }
                    if (veh.GetModCount(VehicleMod.VanityPlates) != 0)
                    {
                        giVanityPlate = new UIMenuItem(LocalizedModTypeName(VehicleMod.VanityPlates), Game.GetLocalizedString("collision_7we93ne"));
                        gmBodyworkArena.AddItem(giVanityPlate);
                        gmBodyworkArena.BindMenuToItem(mVanityPlates, giVanityPlate);
                    }
                }
                else
                {
                    if (veh.GetModCount(VehicleMod.Plaques) != 0)
                    {
                        iPlaques = new UIMenuItem(LocalizedModTypeName(VehicleMod.Plaques), Game.GetLocalizedString("collision_di2ru"));
                        gmBodyworkArena.AddItem(iPlaques);
                        gmBodyworkArena.BindMenuToItem(mPlaques, iPlaques);
                    }
                    if (veh.GetModCount(VehicleMod.Frame) != 0)
                    {
                        iFrame = new UIMenuItem(LocalizedModTypeName(VehicleMod.Frame), Game.GetLocalizedString("CMOD_ARMPL_D"));
                        gmBodyworkArena.AddItem(iFrame);
                        gmBodyworkArena.BindMenuToItem(mFrame, iFrame);
                    }
                    if (veh.GetModCount(VehicleMod.Aerials) != 0)
                    {
                        iAerials = new UIMenuItem(LocalizedModTypeName(VehicleMod.Aerials), Game.GetLocalizedString("collision_37l2i4l"));
                        gmBodyworkArena.AddItem(iAerials);
                        gmBodyworkArena.BindMenuToItem(mAerials, iAerials);
                    }
                    if (veh.GetModCount(VehicleMod.Trim) != 0)
                    {
                        iTrim = new UIMenuItem(LocalizedModTypeName(VehicleMod.Trim), Game.GetLocalizedString("collision_8t77hko"));
                        gmBodyworkArena.AddItem(iTrim);
                        gmBodyworkArena.BindMenuToItem(mTrim, iTrim);
                    }
                    if (veh.GetModCount(VehicleMod.VanityPlates) != 0)
                    {
                        giVanityPlate = new UIMenuItem(LocalizedModTypeName(VehicleMod.VanityPlates), Game.GetLocalizedString("collision_7we93ne"));
                        gmBodyworkArena.AddItem(giVanityPlate);
                        gmBodyworkArena.BindMenuToItem(mVanityPlates, giVanityPlate);
                    }
                    if (veh.GetModCount(VehicleMod.Ornaments) != 0)
                    {
                        giOrnaments = new UIMenuItem(LocalizedModTypeName(VehicleMod.Ornaments), Game.GetLocalizedString("CMOD_MOD_53_D"));
                        gmBodyworkArena.AddItem(giOrnaments);
                        gmBodyworkArena.BindMenuToItem(mOrnaments, giOrnaments);
                    }
                }

                gmBodyworkArena.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshWeaponMenu()
        {
            try
            {
                gmWeapon.MenuItems.Clear();

                if (veh.ClassType == VehicleClass.Motorcycles)
                {
                    if (veh.GetModCount(VehicleMod.Tank) != 0)
                    {
                        giTank = new UIMenuItem(LocalizedModTypeName(VehicleMod.Tank), Game.GetLocalizedString("collision_255bdwf"));
                        gmWeapon.AddItem(giTank);
                        gmWeapon.BindMenuToItem(mTank, giTank);
                    }
                }
                else
                {
                    if (veh.GetModCount(VehicleMod.ArchCover) != 0)
                    {
                        giArchCover = new UIMenuItem(LocalizedModTypeName(VehicleMod.ArchCover), Game.GetLocalizedString("collision_835p5rm"));
                        gmWeapon.AddItem(giArchCover);
                        gmWeapon.BindMenuToItem(mArchCover, giArchCover);
                    }
                    if (veh.GetModCount(VehicleMod.RightFender) != 0)
                    {
                        iRFender = new UIMenuItem(LocalizedModTypeName(VehicleMod.RightFender), Game.GetLocalizedString("CMOD_PROMI_D"));
                        gmWeapon.AddItem(iRFender);
                        gmWeapon.BindMenuToItem(mRFender, iRFender);
                    }
                    if (veh.GetModCount(VehicleMod.Tank) != 0)
                    {
                        giTank = new UIMenuItem(LocalizedModTypeName(VehicleMod.Tank), Game.GetLocalizedString("collision_255bdwf"));
                        gmWeapon.AddItem(giTank);
                        gmWeapon.BindMenuToItem(mTank, giTank);
                    }
                    if (veh.GetModCount(VehicleMod.Roof) != 0)
                    {
                        giRoof = new UIMenuItem(LocalizedModTypeName(VehicleMod.Roof), Game.GetLocalizedString("CMOD_SEWEAP_D"));
                        gmWeapon.AddItem(giRoof);
                        gmWeapon.BindMenuToItem(mRoof, giRoof);
                    }
                }

                gmWeapon.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshArenaWarMenu()
        {
            try
            {
                mUpgradeAW.MenuItems.Clear();

                void addArenaItem(string labelKey, string model, string image, int price)
                {
                    iUpgradeAWV = new UIMenuItem(Game.GetLocalizedString(labelKey));
                    iUpgradeAWV.Tag = new ArenaWarVehicle(model, image, price);
                    iUpgradeAWV.SetRightLabel($"${price}");
                    mUpgradeAW.AddItem(iUpgradeAWV);
                }

                switch (veh.Model.ToString().ToLowerInvariant())
                {
                    case "glendale":
                        addArenaItem("bruiser", "bruiser", "bruiser_apoc", 1609000);
                        addArenaItem("bruiser2", "bruiser2", "bruiser_scifi", 1609000);
                        addArenaItem("bruiser3", "bruiser3", "bruiser_cons", 1609000);
                        break;

                    case "gargoyle":
                        addArenaItem("deathbike", "deathbike", "deathbike_apoc", 1269000);
                        addArenaItem("deathbike2", "deathbike2", "deathbike_scifi", 1269000);
                        addArenaItem("deathbike3", "deathbike3", "deathbike_cons", 1269000);
                        break;

                    case "dominator":
                    case "dominator2":
                        addArenaItem("dominator4", "dominator4", "dominator_apoc", 1132000);
                        addArenaItem("dominator5", "dominator5", "dominator_scifi", 1132000);
                        addArenaItem("dominator6", "dominator6", "dominator_cons", 1132000);
                        break;

                    case "impaler":
                        addArenaItem("impaler2", "impaler2", "impaler_apoc", 1209500);
                        addArenaItem("impaler3", "impaler3", "impaler_scifi", 1209500);
                        addArenaItem("impaler4", "impaler4", "impaler_cons", 1209500);
                        break;

                    case "issi3":
                        addArenaItem("issi4", "issi4", "issi_apoc", 1089000);
                        addArenaItem("issi5", "issi5", "issi_scifi", 1089000);
                        addArenaItem("issi6", "issi6", "issi_cons", 1089000);
                        break;

                    case "ratloader":
                    case "ratloader2":
                        addArenaItem("monster3", "monster3", "sasquatch_apoc", 1530875);
                        addArenaItem("monster4", "monster4", "sasquatch_scifi", 1530875);
                        addArenaItem("monster5", "monster5", "sasquatch_cons", 1530875);
                        break;

                    case "slamvan":
                    case "slamvan2":
                    case "slamvan3":
                        addArenaItem("slamvan4", "slamvan4", "slamvan_apoc", 1321875);
                        addArenaItem("slamvan5", "slamvan5", "slamvan_scifi", 1321875);
                        addArenaItem("slamvan6", "slamvan6", "slamvan_cons", 1321875);
                        break;
                }

                mUpgradeAW.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshBodyworkMenu()
        {
            try
            {
                gmBodywork.MenuItems.Clear();

                if (veh.ClassType == VehicleClass.Motorcycles || veh.Model.ToString().Equals("blazer4", StringComparison.OrdinalIgnoreCase))
                {
                    if (veh.GetModCount(VehicleMod.Fender) != 0)
                    {
                        giShifter = new UIMenuItem(LocalizedModTypeName(VehicleMod.Fender), Game.GetLocalizedString("CMOD_MOD_SHI_D"));
                        gmBodywork.AddItem(giShifter);
                        gmBodywork.BindMenuToItem(mShifter, giShifter);
                    }
                    if (veh.GetModCount(VehicleMod.FrontBumper) != 0)
                    {
                        giFMudguard = new UIMenuItem(LocalizedModTypeName(VehicleMod.FrontBumper), Game.GetLocalizedString("CMOD_MOD_43_D"));
                        gmBodywork.AddItem(giFMudguard);
                        gmBodywork.BindMenuToItem(mFMudguard, giFMudguard);
                    }
                    if (veh.GetModCount(VehicleMod.Hood) != 0)
                    {
                        iBSeat = new UIMenuItem(LocalizedModTypeName(VehicleMod.Hood), Game.GetLocalizedString("CMOD_MOD_44_D"));
                        gmBodywork.AddItem(iBSeat);
                        gmBodywork.BindMenuToItem(mBSeat, iBSeat);
                    }
                    if (veh.GetModCount(VehicleMod.Grille) != 0)
                    {
                        giOilTank = new UIMenuItem(LocalizedModTypeName(VehicleMod.Grille), Game.GetLocalizedString("CMOD_MOD_OT_D"));
                        gmBodywork.AddItem(giOilTank);
                        gmBodywork.BindMenuToItem(mOilTank, giOilTank);
                    }
                    if (veh.GetModCount(VehicleMod.RearBumper) != 0)
                    {
                        giRMudguard = new UIMenuItem(LocalizedModTypeName(VehicleMod.RearBumper), Game.GetLocalizedString("CMOD_MOD_43_D"));
                        gmBodywork.AddItem(giRMudguard);
                        gmBodywork.BindMenuToItem(mRMudguard, giRMudguard);
                    }
                    if (veh.GetModCount(VehicleMod.Roof) != 0)
                    {
                        giFuelTank = new UIMenuItem(LocalizedModTypeName(VehicleMod.Roof), Game.GetLocalizedString("CMOD_MOD_FUT_D"));
                        gmBodywork.AddItem(giFuelTank);
                        gmBodywork.BindMenuToItem(mFuelTank, giFuelTank);
                    }
                    if (veh.GetModCount(VehicleMod.Spoilers) != 0)
                    {
                        giBeltDriveCovers = new UIMenuItem(LocalizedModTypeName(VehicleMod.Spoilers), Game.GetLocalizedString("CMOD_MOD_BEC_D"));
                        gmBodywork.AddItem(giBeltDriveCovers);
                        gmBodywork.BindMenuToItem(mBeltDriveCovers, giBeltDriveCovers);
                    }
                    if (veh.GetModCount(VehicleMod.RightFender) != 0)
                    {
                        iRFender = new UIMenuItem(LocalizedModTypeName(VehicleMod.RightFender), Game.GetLocalizedString("CMOD_MOD_41_D"));
                        gmBodywork.AddItem(iRFender);
                        gmBodywork.BindMenuToItem(mRFender, iRFender);
                    }
                    if (veh.GetModCount(VehicleMod.Tank) != 0)
                    {
                        giBTank = new UIMenuItem(LocalizedModTypeName(VehicleMod.Tank), Game.GetLocalizedString("CMOD_MOD_45_D"));
                        gmBodywork.AddItem(giBTank);
                        gmBodywork.BindMenuToItem(mBTank, giBTank);
                    }
                }
                else
                {
                    if (veh.GetModCount(VehicleMod.Aerials) != 0)
                    {
                        iAerials = new UIMenuItem(LocalizedModTypeName(VehicleMod.Aerials), Game.GetLocalizedString("SMOD_CHASS_6"));
                        gmBodywork.AddItem(iAerials);
                        gmBodywork.BindMenuToItem(mAerials, iAerials);
                    }
                    if (veh.GetModCount(VehicleMod.Trim) != 0)
                    {
                        iTrim = new UIMenuItem(LocalizedModTypeName(VehicleMod.Trim), Game.GetLocalizedString("SMOD_CHASS_1b"));
                        gmBodywork.AddItem(iTrim);
                        gmBodywork.BindMenuToItem(mTrim, iTrim);
                    }
                    if (veh.GetModCount(VehicleMod.Windows) != 0)
                    {
                        iWindows = new UIMenuItem(LocalizedModTypeName(VehicleMod.Windows), Game.GetLocalizedString("SMOD_CHASS_5"));
                        gmBodywork.AddItem(iWindows);
                        gmBodywork.BindMenuToItem(mWindow, iWindows);
                    }
                    if (veh.GetModCount(VehicleMod.ArchCover) != 0)
                    {
                        iArchCover = new UIMenuItem(LocalizedModTypeName(VehicleMod.ArchCover), Game.GetLocalizedString("SMOD_CHASS_1c"));
                        gmBodywork.AddItem(iArchCover);
                        gmBodywork.BindMenuToItem(mArchCover, iArchCover);
                    }
                }

                gmBodywork.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshModMenuForLivery2(ref UIMenu menu, ref UIMenuItem item)
        {
            try
            {
                menu.MenuItems.Clear();
                for (int i = 0; i < veh.Livery2Count(); i++)
                {
                    item = new UIMenuItem(LocalizedT5RoofName(i));
                    if (item.Text == "NULL")
                    {
                        item.Text = Game.GetLocalizedString("CMOD_ARM_0");
                    }

                    if (veh.GetLivery2() == i)
                    {
                        item.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        item.Tag = new ModClass(i, 0);
                    }
                    else
                    {
                        int price = 200 * (i + 1);
                        item.SetRightLabel($"${price}");
                        item.Tag = new ModClass(i, price);
                    }

                    menu.AddItem(item);
                }

                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshModMenuFor(ref UIMenu menu, ref UIMenuItem item, VehicleMod vehmod)
        {
            try
            {
                menu.MenuItems.Clear();
                int count = veh.GetModCount(vehmod);
                for (int i = -1; i < count; i++)
                {
                    item = new UIMenuItem(GetLocalizedModName(i, count, vehmod));
                    if (item.Text == "NULL")
                    {
                        item.Text = Game.GetLocalizedString("CMOD_ARM_0");
                    }

                    if (veh.GetMod(vehmod) == i)
                    {
                        item.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        item.Tag = new ModClass(i, 0);
                    }
                    else
                    {
                        int price = GetIndexedModPrice(i, 200);
                        if (price > 0)
                        {
                            item.SetRightLabel($"${price}");
                        }
                        item.Tag = new ModClass(i, price);
                    }

                    menu.AddItem(item);
                }
                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshModMenuFor(ref UIMenu menu, ref UIMenuItem item, VehicleToggleMod vehmod)
        {
            try
            {
                menu.MenuItems.Clear();

                item = new UIMenuItem(LocalizedModTypeName(vehmod, true));
                if (!veh.IsToggleModOn(vehmod))
                {
                    item.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                    item.Tag = new ModClass(0, 0);
                }
                else
                {
                    item.SetRightLabel("$1000");
                    item.Tag = new ModClass(0, 1000);
                }
                menu.AddItem(item);

                item = new UIMenuItem(LocalizedModTypeName(vehmod));
                if (veh.IsToggleModOn(vehmod))
                {
                    item.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                    item.Tag = new ModClass(1, 0);
                }
                else
                {
                    item.SetRightLabel("$1000");
                    item.Tag = new ModClass(1, 1000);
                }
                menu.AddItem(item);

                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshModMenuForHeadlightsColor(ref UIMenu menu, ref UIMenuItem item)
        {
            try
            {
                menu.MenuItems.Clear();

                bool xenonEnabled = veh.IsToggleModOn(VehicleToggleMod.XenonHeadlights);
                int currentColor = GetXenonHeadlightsColorIndex(veh);

                var stockItem = new UIMenuItem(Game.GetLocalizedString("CMOD_LGT_0"));
                if (!xenonEnabled)
                {
                    stockItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                    stockItem.Tag = new ToggleModClass(false, 255, 0);
                }
                else
                {
                    const int stockPrice = 500;
                    stockItem.SetRightLabel($"${stockPrice}");
                    stockItem.Tag = new ToggleModClass(false, 255, stockPrice);
                }

                menu.AddItem(stockItem);
                item = stockItem;

                for (int i = 0; i <= 12; i++)
                {
                    string label = LocalizedXenonColor(i);
                    var createdItem = new UIMenuItem(label);

                    if (xenonEnabled && currentColor == i)
                    {
                        createdItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        createdItem.Tag = new ToggleModClass(true, i, 0);
                    }
                    else
                    {
                        int price = 500;
                        createdItem.SetRightLabel($"${price}");
                        createdItem.Tag = new ToggleModClass(true, i, price);
                    }

                    menu.AddItem(createdItem);
                    item = createdItem;
                }

                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        private static int GetXenonHeadlightsColorIndex(Vehicle vehicle)
        {
            try
            {
                return vehicle != null
                    ? Function.Call<int>((Hash)0x3DFF319A831E0CDBUL, vehicle.Handle)
                    : 255;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
                return 255;
            }
        }

        private static string LocalizedXenonColor(int colorIndex)
        {
            switch (colorIndex)
            {
                case 0: return "White";
                case 1: return "Blue";
                case 2: return "Electric Blue";
                case 3: return "Mint Green";
                case 4: return "Lime Green";
                case 5: return "Yellow";
                case 6: return "Golden Shower";
                case 7: return "Orange";
                case 8: return "Red";
                case 9: return "Pony Pink";
                case 10: return "Hot Pink";
                case 11: return "Purple";
                case 12: return "Blacklight";
                default: return "Xenon";
            }
        }

        public static void RefreshEngineMenu()
        {
            try
            {
                gmEngine.MenuItems.Clear();

                if (veh.ClassType == VehicleClass.Motorcycles || veh.Model.ToString().Equals("blazer4", StringComparison.OrdinalIgnoreCase))
                {
                    if (veh.GetModCount(VehicleMod.Frame) != 0)
                    {
                        giBEngineBlock = new UIMenuItem(LocalizedModTypeName(VehicleMod.Frame), Game.GetLocalizedString("SMOD_ENGINE_1"));
                        gmEngine.AddItem(giBEngineBlock);
                        gmEngine.BindMenuToItem(mBEngineBlock, giBEngineBlock);
                    }
                    if (veh.GetModCount(VehicleMod.Engine) != 0)
                    {
                        iEngine = new UIMenuItem(LocalizedModTypeName(VehicleMod.Engine), Game.GetLocalizedString("SMOD_ENGINE_4"));
                        gmEngine.AddItem(iEngine);
                        gmEngine.BindMenuToItem(mEngine, iEngine);
                    }
                    if (veh.GetModCount(VehicleMod.SideSkirt) != 0)
                    {
                        giBAirFilter = new UIMenuItem(LocalizedModTypeName(VehicleMod.SideSkirt), Game.GetLocalizedString("CMOD_SMOD_2_D"));
                        gmEngine.AddItem(giBAirFilter);
                        gmEngine.BindMenuToItem(mBAirFilter, giBAirFilter);
                    }
                    if (veh.CanInstallNitroMod())
                    {
                        iNitro = new UIMenuItem(Game.GetLocalizedString("CMM_MOD_NJBOS"), Game.GetLocalizedString("SMOD_ENGINE_2"));
                        gmEngine.AddItem(iNitro);
                        gmEngine.BindMenuToItem(mNitro, iNitro);
                    }
                }
                else
                {
                    if (veh.GetModCount(VehicleMod.Engine) != 0)
                    {
                        iEngine = new UIMenuItem(LocalizedModTypeName(VehicleMod.Engine), Game.GetLocalizedString("SMOD_ENGINE_4"));
                        gmEngine.AddItem(iEngine);
                        gmEngine.BindMenuToItem(mEngine, iEngine);
                    }
                    if (veh.GetModCount(VehicleMod.EngineBlock) != 0)
                    {
                        iEngineBlock = new UIMenuItem(LocalizedModTypeName(VehicleMod.EngineBlock), Game.GetLocalizedString("SMOD_ENGINE_1"));
                        gmEngine.AddItem(iEngineBlock);
                        gmEngine.BindMenuToItem(mEngineBlock, iEngineBlock);
                    }
                    if (veh.CanInstallNitroMod())
                    {
                        iNitro = new UIMenuItem(Game.GetLocalizedString("CMM_MOD_NJBOS"), Game.GetLocalizedString("SMOD_ENGINE_2"));
                        gmEngine.AddItem(iNitro);
                        gmEngine.BindMenuToItem(mNitro, iNitro);
                    }
                    if (!arenavehicle.Contains(veh.Model))
                    {
                        if (veh.GetModCount(VehicleMod.AirFilter) != 0)
                        {
                            giAirfilter = new UIMenuItem(LocalizedModTypeName(VehicleMod.AirFilter), Game.GetLocalizedString("SMOD_ENGINE_2"));
                            gmEngine.AddItem(giAirfilter);
                            gmEngine.BindMenuToItem(mAirFilter, giAirfilter);
                        }
                        if (veh.GetModCount(VehicleMod.Struts) != 0)
                        {
                            giStruts = new UIMenuItem(LocalizedModTypeName(VehicleMod.Struts), Game.GetLocalizedString("SMOD_ENGINE_3b"));
                            gmEngine.AddItem(giStruts);
                            gmEngine.BindMenuToItem(mStruts, giStruts);
                        }
                    }
                }

                gmEngine.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshPerformanceMenuFor(ref UIMenu menu, ref UIMenuItem item, VehicleMod vehmod, string gxt)
        {
            try
            {
                menu.MenuItems.Clear();
                int count = veh.GetModCount(vehmod);
                for (int i = -1; i < count; i++)
                {
                    string label = vehmod == VehicleMod.Engine
                    ? Game.GetLocalizedString(gxt + (i + 2))
                    : Game.GetLocalizedString(gxt + (i + 1));

                    item = new UIMenuItem(label);
                    if (item.Text == "NULL")
                    {
                        item.Text = Game.GetLocalizedString("CMOD_ARM_0");
                    }

                    if (veh.GetMod(vehmod) == i)
                    {
                        item.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        item.Tag = new ModClass(i, 0);
                    }
                    else
                    {
                        int price = GetPerformanceModPrice(vehmod, i);
                        if (price > 0)
                        {
                            item.SetRightLabel($"${price}");
                        }
                        item.Tag = new ModClass(i, price);
                    }

                    menu.AddItem(item);
                }
                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshNitroMenu()
        {
            try
            {
                mNitro.MenuItems.Clear();

                void AddNitroItem(string labelGxt, int modId, int price)
                {
                    var nitroItem = new UIMenuItem(Game.GetLocalizedString(labelGxt));
                    if (nitroItem.Text == "NULL")
                    {
                        nitroItem.Text = Game.GetLocalizedString("CMOD_ARM_0");
                    }

                    if (MenuHelper.GetInt(veh, nitroMod) == modId)
                    {
                        nitroItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        nitroItem.Tag = new ModClass(modId, 0);
                    }
                    else
                    {
                        if (price > 0)
                        {
                            nitroItem.SetRightLabel($"${price}");
                        }
                        nitroItem.Tag = new ModClass(modId, price);
                    }

                    mNitro.AddItem(nitroItem);
                    iNitro = nitroItem;
                }

                AddNitroItem("CMOD_ARM_0", 0, 10000);
                AddNitroItem("CMOD_BOS_1", 1, 10000);
                AddNitroItem("CMOD_BOS_2", 2, 20000);
                AddNitroItem("CMOD_BOS_3", 3, 30000);

                mNitro.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshInteriorMenu()
        {
            try
            {
                gmInterior.MenuItems.Clear();
                if (veh.GetModCount(VehicleMod.ColumnShifterLevers) != 0)
                {
                    iColumnShifterLevers = new UIMenuItem(LocalizedModTypeName(VehicleMod.ColumnShifterLevers), Game.GetLocalizedString("SMOD_IN_KNOB"));
                    gmInterior.AddItem(iColumnShifterLevers);
                    gmInterior.BindMenuToItem(mColumnShifterLevers, iColumnShifterLevers);
                }
                if (veh.GetModCount(VehicleMod.Dashboard) != 0)
                {
                    iDashboard = new UIMenuItem(LocalizedModTypeName(VehicleMod.Dashboard), Game.GetLocalizedString("SMOD_IN_2"));
                    gmInterior.AddItem(iDashboard);
                    gmInterior.BindMenuToItem(mDashboard, iDashboard);
                }
                if (veh.GetModCount(VehicleMod.DialDesign) != 0)
                {
                    iDialDesign = new UIMenuItem(LocalizedModTypeName(VehicleMod.DialDesign), Game.GetLocalizedString("SMOD_IN_4"));
                    gmInterior.AddItem(iDialDesign);
                    gmInterior.BindMenuToItem(mDialDesign, iDialDesign);
                }
                if (!arenavehicle.Contains(veh.Model))
                {
                    if (veh.GetModCount(VehicleMod.Ornaments) != 0)
                    {
                        iOrnaments = new UIMenuItem(LocalizedModTypeName(VehicleMod.Ornaments), Game.GetLocalizedString("CMOD_MOD_64_D"));
                        gmInterior.AddItem(iOrnaments);
                        gmInterior.BindMenuToItem(mOrnaments, iOrnaments);
                    }
                }
                if (veh.GetModCount(VehicleMod.Seats) != 0)
                {
                    iSeats = new UIMenuItem(LocalizedModTypeName(VehicleMod.Seats), Game.GetLocalizedString("SMOD_IN_SEAT"));
                    gmInterior.AddItem(iSeats);
                    gmInterior.BindMenuToItem(mSeats, iSeats);
                }
                if (veh.GetModCount(VehicleMod.SteeringWheels) != 0)
                {
                    iSteeringWheels = new UIMenuItem(LocalizedModTypeName(VehicleMod.SteeringWheels), Game.GetLocalizedString("SMOD_IN_STEER"));
                    gmInterior.AddItem(iSteeringWheels);
                    gmInterior.BindMenuToItem(mSteeringWheels, iSteeringWheels);
                }
                if (veh.GetModCount(VehicleMod.TrimDesign) != 0)
                {
                    iTrimDesign = new UIMenuItem(LocalizedModTypeName(VehicleMod.TrimDesign), Game.GetLocalizedString("SMOD_IN_3"));
                    gmInterior.AddItem(iTrimDesign);
                    gmInterior.BindMenuToItem(mTrimDesign, iTrimDesign);
                }
                if (veh.GetModCount(VehicleMod.DoorSpeakers) != 0)
                {
                    giDoor = new UIMenuItem(LocalizedModTypeName(VehicleMod.DoorSpeakers), Game.GetLocalizedString("SMOD_IN_5b"));
                    gmInterior.AddItem(giDoor);
                    gmInterior.BindMenuToItem(mDoor, giDoor);
                }
                if (veh.GetModCount(VehicleMod.Speakers) != 0)
                {
                    iSpeaker = new UIMenuItem(LocalizedModTypeName(VehicleMod.Speakers), Game.GetLocalizedString("CMOD_MOD_23_D"));
                    gmInterior.AddItem(iSpeaker);
                    gmInterior.BindMenuToItem(mSpeakers, iSpeaker);
                }
                if (bennysvehicle.Contains(veh.Model))
                {
                    iDashboardColor = new UIMenuItem(LocalizedModGroupName(GroupName.LightColor), Game.GetLocalizedString("SMOD_LIGHT_COLb"));
                    gmInterior.AddItem(iDashboardColor);
                    gmInterior.BindMenuToItem(mLightsColor, iDashboardColor);
                    iTrimColor = new UIMenuItem(LocalizedModGroupName(GroupName.TrimColor), Game.GetLocalizedString("CMOD_MOD_6_D"));
                    gmInterior.AddItem(iTrimColor);
                    gmInterior.BindMenuToItem(mTrimColor, iTrimColor);
                }
                gmInterior.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshBumperMenu()
        {
            try
            {
                gmBumper.MenuItems.Clear();
                if (veh.GetModCount(VehicleMod.FrontBumper) != 0)
                {
                    giFBumper = new UIMenuItem(LocalizedModTypeName(VehicleMod.FrontBumper), Game.GetLocalizedString("CMOD_MOD_71_D"));
                    gmBumper.AddItem(giFBumper);
                    gmBumper.BindMenuToItem(mFBumper, giFBumper);
                }
                if (veh.GetModCount(VehicleMod.SideSkirt) != 0)
                {
                    giSSkirt = new UIMenuItem(LocalizedModTypeName(VehicleMod.SideSkirt), Game.GetLocalizedString("CMOD_MOD_21_D"));
                    gmBumper.AddItem(giSSkirt);
                    gmBumper.BindMenuToItem(mSSkirt, giSSkirt);
                }
                if (veh.GetModCount(VehicleMod.RearBumper) != 0)
                {
                    giRBumper = new UIMenuItem(LocalizedModTypeName(VehicleMod.RearBumper), Game.GetLocalizedString("CMOD_MOD_71_D"));
                    gmBumper.AddItem(giRBumper);
                    gmBumper.BindMenuToItem(mRBumper, giRBumper);
                }
                gmBumper.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshEnumModMenuFor(ref UIMenu menu, ref UIMenuItem item, EnumTypes enumType)
        {
            try
            {
                menu.MenuItems.Clear();

                switch (enumType)
                {
                    case EnumTypes.NumberPlateType:
                        foreach (LicensePlateStyle enumItem in Enum.GetValues(typeof(LicensePlateStyle)))
                        {
                            var createdItem = new UIMenuItem(LocalizedLicensePlate((int)enumItem));
                            if (veh.Mods.LicensePlateStyle == enumItem)
                            {
                                createdItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                createdItem.Tag = new ModClass((int)enumItem, 0);
                            }
                            else
                            {
                                createdItem.SetRightLabel($"${200}");
                                createdItem.Tag = new ModClass((int)enumItem, 200);
                            }
                            menu.AddItem(createdItem);
                            item = createdItem;
                        }
                        break;

                    case EnumTypes.VehicleWindowTint:
                        foreach (VehicleWindowTint enumItem in Enum.GetValues(typeof(VehicleWindowTint)))
                        {
                            var createdItem = new UIMenuItem(LocalizedWindowsTint(enumItem));
                            if (veh.Mods.WindowTint == enumItem)
                            {
                                createdItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                createdItem.Tag = new ModClass((int)enumItem, 0);
                            }
                            else
                            {
                                createdItem.SetRightLabel($"${2000}");
                                createdItem.Tag = new ModClass((int)enumItem, 2000);
                            }
                            menu.AddItem(createdItem);
                            item = createdItem;
                        }
                        break;

                    case EnumTypes.VehicleColorPrimary:
                        foreach (VehicleColor enumItem in Enum.GetValues(typeof(VehicleColor)))
                        {
                            var createdItem = new UIMenuItem(GetLocalizedColorName(enumItem));
                            if (veh.Mods.PrimaryColor == enumItem)
                            {
                                createdItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                createdItem.Tag = new ModClass((int)enumItem, 0);
                            }
                            else
                            {
                                createdItem.SetRightLabel($"${2000}");
                                createdItem.Tag = new ModClass((int)enumItem, 2000);
                            }
                            menu.AddItem(createdItem);
                            item = createdItem;
                        }
                        break;

                    case EnumTypes.VehicleColorSecondary:
                        foreach (VehicleColor enumItem in Enum.GetValues(typeof(VehicleColor)))
                        {
                            var createdItem = new UIMenuItem(GetLocalizedColorName(enumItem));
                            if (veh.Mods.SecondaryColor == enumItem)
                            {
                                createdItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                createdItem.Tag = new ModClass((int)enumItem, 0);
                            }
                            else
                            {
                                createdItem.SetRightLabel($"${2000}");
                                createdItem.Tag = new ModClass((int)enumItem, 2000);
                            }
                            menu.AddItem(createdItem);
                            item = createdItem;
                        }
                        break;

                    case EnumTypes.VehicleColorTrim:
                        foreach (VehicleColor enumItem in Enum.GetValues(typeof(VehicleColor)))
                        {
                            var createdItem = new UIMenuItem(GetLocalizedColorName(enumItem));
                            if (veh.Mods.TrimColor == enumItem)
                            {
                                createdItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                createdItem.Tag = new ModClass((int)enumItem, 0);
                            }
                            else
                            {
                                createdItem.SetRightLabel($"${700}");
                                createdItem.Tag = new ModClass((int)enumItem, 700);
                            }
                            menu.AddItem(createdItem);
                            item = createdItem;
                        }
                        break;

                    case EnumTypes.VehicleColorDashboard:
                        foreach (VehicleColor enumItem in Enum.GetValues(typeof(VehicleColor)))
                        {
                            var createdItem = new UIMenuItem(GetLocalizedColorName(enumItem));
                            if (veh.Mods.DashboardColor == enumItem)
                            {
                                createdItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                createdItem.Tag = new ModClass((int)enumItem, 0);
                            }
                            else
                            {
                                createdItem.SetRightLabel($"${700}");
                                createdItem.Tag = new ModClass((int)enumItem, 700);
                            }
                            menu.AddItem(createdItem);
                            item = createdItem;
                        }
                        break;

                    case EnumTypes.VehicleColorRim:
                        foreach (VehicleColor enumItem in Enum.GetValues(typeof(VehicleColor)))
                        {
                            var createdItem = new UIMenuItem(GetLocalizedColorName(enumItem));
                            if (veh.Mods.RimColor == enumItem)
                            {
                                createdItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                createdItem.Tag = new ModClass((int)enumItem, 0);
                            }
                            else
                            {
                                createdItem.SetRightLabel($"${700}");
                                createdItem.Tag = new ModClass((int)enumItem, 700);
                            }
                            menu.AddItem(createdItem);
                            item = createdItem;
                        }
                        break;

                    case EnumTypes.vehicleColorPearlescent:
                        foreach (VehicleColor enumItem in Enum.GetValues(typeof(VehicleColor)))
                        {
                            var createdItem = new UIMenuItem(GetLocalizedColorName(enumItem));
                            if (veh.Mods.PearlescentColor == enumItem)
                            {
                                createdItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                createdItem.Tag = new ModClass((int)enumItem, 0);
                            }
                            else
                            {
                                createdItem.SetRightLabel($"${2000}");
                                createdItem.Tag = new ModClass((int)enumItem, 2000);
                            }
                            menu.AddItem(createdItem);
                            item = createdItem;
                        }
                        break;
                }

                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshWheelsMenu()
        {
            try
            {
                gmWheels.MenuItems.Clear();
                giWheelType = new UIMenuItem(LocalizedModGroupName(GroupName.WheelType), Game.GetLocalizedString("CMOD_MOD_28_D"));
                gmWheels.AddItem(giWheelType);
                gmWheels.BindMenuToItem(gmWheelType, giWheelType);
                iRimColor = new UIMenuItem(LocalizedModGroupName(GroupName.WheelColor), Game.GetLocalizedString("CMOD_MOD_59_D"));
                gmWheels.AddItem(iRimColor);
                gmWheels.BindMenuToItem(mRimColor, iRimColor);
                giTires = new UIMenuItem(LocalizedModGroupName(GroupName.Tires), Game.GetLocalizedString("CMOD_IE_25_D"));
                gmWheels.AddItem(giTires);
                gmWheels.BindMenuToItem(mTires, giTires);
                iTireSmoke = new UIMenuItem(LocalizedModTypeName(VehicleToggleMod.TireSmoke), Game.GetLocalizedString("CMOD_IE_25_D"));
                gmWheels.AddItem(iTireSmoke);
                gmWheels.BindMenuToItem(mTireSmoke, iTireSmoke);
                iBPTires = new UIMenuItem(Game.GetLocalizedString("CMOD_GLD2_1"));
                {
                    var __with1 = iBPTires;
                    if (!veh.CanTiresBurst)
                    {
                        __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                    }
                    else
                    {
                        __with1.SetRightLabel($"${4000}");
                        __with1.Tag = 4000;
                    }
                }
                gmWheels.AddItem(iBPTires);
                gmWheels.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshWheelTypeMenu()
        {
            try
            {
                gmWheelType.MenuItems.Clear();

                switch (veh.ClassType)
                {
                    case VehicleClass.Motorcycles:
                        break;
                    case VehicleClass.Cycles:
                        giBikeWheels = new UIMenuItem(GetLocalizedWheelTypeName(VehicleWheelType.BikeWheels));
                        gmWheelType.AddItem(giBikeWheels);
                        gmWheelType.BindMenuToItem(gmBikeWheels, giBikeWheels);
                        break;
                    default:
                        giHighEndWheels = new UIMenuItem(GetLocalizedWheelTypeName(VehicleWheelType.HighEnd));
                        gmWheelType.AddItem(giHighEndWheels);
                        gmWheelType.BindMenuToItem(gmHighEnd, giHighEndWheels);
                        giLowriderWheels = new UIMenuItem(GetLocalizedWheelTypeName(VehicleWheelType.Lowrider));
                        gmWheelType.AddItem(giLowriderWheels);
                        gmWheelType.BindMenuToItem(gmLowrider, giLowriderWheels);
                        giMuscleWheels = new UIMenuItem(GetLocalizedWheelTypeName(VehicleWheelType.Muscle));
                        gmWheelType.AddItem(giMuscleWheels);
                        gmWheelType.BindMenuToItem(gmMuscle, giMuscleWheels);
                        giOffroadWheels = new UIMenuItem(GetLocalizedWheelTypeName(VehicleWheelType.Offroad));
                        gmWheelType.AddItem(giOffroadWheels);
                        gmWheelType.BindMenuToItem(gmOffroad, giOffroadWheels);
                        giSportWheels = new UIMenuItem(GetLocalizedWheelTypeName(VehicleWheelType.Sport));
                        gmWheelType.AddItem(giSportWheels);
                        gmWheelType.BindMenuToItem(gmSport, giSportWheels);
                        giSUVWheels = new UIMenuItem(GetLocalizedWheelTypeName(VehicleWheelType.SUV));
                        gmWheelType.AddItem(giSUVWheels);
                        gmWheelType.BindMenuToItem(gmSUV, giSUVWheels);
                        giTunerWheels = new UIMenuItem(GetLocalizedWheelTypeName(VehicleWheelType.Tuner));
                        gmWheelType.AddItem(giTunerWheels);
                        gmWheelType.BindMenuToItem(gmTuner, giTunerWheels);
                        giBennysWheels = new UIMenuItem(GetLocalizedWheelTypeName((VehicleWheelType)8));
                        gmWheelType.AddItem(giBennysWheels);
                        gmWheelType.BindMenuToItem(mBennysOriginals, giBennysWheels);
                        giBespokeWheels = new UIMenuItem(GetLocalizedWheelTypeName((VehicleWheelType)9));
                        gmWheelType.AddItem(giBespokeWheels);
                        gmWheelType.BindMenuToItem(mBespoke, giBespokeWheels);

                        giRacingWheels = new UIMenuItem(GetLocalizedWheelTypeName((VehicleWheelType)10));
                        gmWheelType.AddItem(giRacingWheels);
                        gmWheelType.BindMenuToItem(mRacing, giRacingWheels);
                        giStreetWheels = new UIMenuItem(GetLocalizedWheelTypeName((VehicleWheelType)11));
                        gmWheelType.AddItem(giStreetWheels);
                        gmWheelType.BindMenuToItem(mStreet, giStreetWheels);
                        break;
                }

                gmWheelType.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshWheelRimMenu(ref UIMenu menu, ref UIMenu bindStock, ref UIMenu bindChrome, ref UIMenuItem itemStock, ref UIMenuItem itemChrome)
        {
            try
            {
                menu.MenuItems.Clear();
                itemStock = new UIMenuItem(Game.GetLocalizedString("CMOD_WHE4_0"));
                menu.AddItem(itemStock);
                menu.BindMenuToItem(bindStock, itemStock);
                itemChrome = new UIMenuItem(Game.GetLocalizedString("CMOD_WHE4_1"));
                menu.AddItem(itemChrome);
                menu.BindMenuToItem(bindChrome, itemChrome);
                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshStockWheelsModMenuFor(ref UIMenu menu, ref UIMenuItem item, VehicleMod vehmod)
        {
            try
            {
                menu.MenuItems.Clear();
                int count = veh.GetModCount(vehmod);
                int half = count / 2;

                for (int i = -1; i < half; i++)
                {
                    item = new UIMenuItem(GetLocalizedModName(i, count, vehmod));
                    if (item.Text == "NULL")
                    {
                        item.Text = Game.GetLocalizedString("CMOD_ARM_0");
                    }

                    if (veh.GetMod(vehmod) == i)
                    {
                        item.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        item.Tag = new ModClass(i, 0);
                    }
                    else
                    {
                        int price = GetIndexedModPrice(i, 200);
                        item.SetRightLabel($"${price}");
                        item.Tag = new ModClass(i, price);
                    }

                    menu.AddItem(item);
                }

                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshChromeWheelsModMenuFor(ref UIMenu menu, ref UIMenuItem item, VehicleMod vehmod)
        {
            try
            {
                menu.MenuItems.Clear();
                int count = veh.GetModCount(vehmod);
                int half = count / 2;

                for (int i = half; i < count; i++)
                {
                    item = new UIMenuItem(GetLocalizedModName(i, count, vehmod));
                    if (item.Text == "NULL")
                    {
                        item.Text = Game.GetLocalizedString("CMOD_ARM_0");
                    }

                    if (veh.GetMod(vehmod) == i)
                    {
                        item.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        item.Tag = new ModClass(i, 0);
                    }
                    else
                    {
                        int price = 200 * (i + 1);
                        item.SetRightLabel($"${price}");
                        item.Tag = new ModClass(i, price);
                    }

                    menu.AddItem(item);
                }

                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshLowriderDLCWheelsModMenuFor(ref UIMenu menu, ref UIMenuItem item, VehicleMod vehmod)
        {
            try
            {
                menu.MenuItems.Clear();
                int count = veh.GetModCount(vehmod);
                int oneOver6 = count / 7;

                for (int i = -1; i < oneOver6; i++)
                {
                    item = new UIMenuItem(GetLocalizedModName(i, count, vehmod));
                    if (item.Text == "NULL")
                    {
                        item.Text = Game.GetLocalizedString("CMOD_ARM_0");
                    }

                    if (veh.GetMod(vehmod) == i)
                    {
                        item.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        item.Tag = new ModClass(i, 0);
                    }
                    else
                    {
                        int price = GetIndexedModPrice(i, 200);
                        if (price > 0)
                        {
                            item.SetRightLabel($"${price}");
                        }
                        item.Tag = new ModClass(i, price);
                    }

                    menu.AddItem(item);
                }

                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshBikeWheelsModMenuFor(ref UIMenu menu, ref UIMenuItem item, VehicleMod vehmod, bool chromeWheels)
        {
            try
            {
                menu.MenuItems.Clear();
                int count = veh.GetModCount(vehmod);
                List<int> standard = new List<int> { -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48 };
                List<int> chrome = new List<int> { -1, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71 };

                foreach (int i in chromeWheels ? chrome : standard)
                {
                    if (i >= count && i != -1)
                    {
                        continue;
                    }

                    item = new UIMenuItem(GetLocalizedModName(i, count, vehmod));
                    if (item.Text == "NULL")
                    {
                        item.Text = Game.GetLocalizedString("CMOD_ARM_0");
                    }

                    if (veh.GetMod(vehmod) == i)
                    {
                        item.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        item.Tag = new ModClass(i, 0);
                    }
                    else
                    {
                        int price = GetIndexedModPrice(i, 200);
                        if (price > 0)
                        {
                            item.SetRightLabel($"${price}");
                        }
                        item.Tag = new ModClass(i, price);
                    }

                    menu.AddItem(item);
                }

                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshTyresMenu()
        {
            try
            {
                mTires.MenuItems.Clear();

                void addTireItem(string label, int modId, int price, bool installed)
                {
                    iTires = new UIMenuItem(label);
                    if (installed)
                    {
                        iTires.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        iTires.Tag = new ModClass(modId, 0);
                    }
                    else
                    {
                        iTires.SetRightLabel($"${price}");
                        iTires.Tag = new ModClass(modId, price);
                    }
                    mTires.AddItem(iTires);
                }

                if (veh.GetMod(VehicleMod.FrontWheel) == -1)
                {
                    addTireItem(Game.GetLocalizedString("CMOD_TYR_0"), 1, 100, !IsCustomWheels());
                }
                else if (veh.GetWheelType() == (VehicleWheelType)8 || veh.GetWheelType() == (VehicleWheelType)9 || veh.GetWheelType() == (VehicleWheelType)10 || veh.GetWheelType() == (VehicleWheelType)11)
                {
                    int currentWheel = veh.GetMod(VehicleMod.FrontWheel);
                    int baseWheel = GetBennysOriginalRim(currentWheel);
                    int step = Math.Max(1, veh.GetModCount(VehicleMod.FrontWheel) / 7);
                    string[] labels =
                    {
                    Game.GetLocalizedString("CMOD_TYR_0"),
                    Game.GetLocalizedString("collision_v925jg"),
                    Game.GetLocalizedString("collision_v925jh"),
                    Game.GetLocalizedString("collision_v925ji"),
                    Game.GetLocalizedString("collision_v925jj"),
                    Game.GetLocalizedString("collision_v925jk"),
                    Game.GetLocalizedString("CMOD_TYR_1"),
                };

                    for (int variant = 0; variant < labels.Length; variant++)
                    {
                        int modId = baseWheel + (step * variant);
                        int price = 100 * (variant + 1);
                        addTireItem(labels[variant], modId, price, currentWheel == modId);
                    }
                }
                else
                {
                    addTireItem(Game.GetLocalizedString("CMOD_TYR_0"), 1, 100, !IsCustomWheels());
                    addTireItem(Game.GetLocalizedString("CMOD_TYR_1"), 7, 700, IsCustomWheels());
                }

                mTires.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshPlateMenu()
        {
            try
            {
                gmPlate.MenuItems.Clear();
                if (veh.GetModCount(VehicleMod.PlateHolder) != 0)
                {
                    giPlateHolder = new UIMenuItem(LocalizedModTypeName(VehicleMod.PlateHolder), Game.GetLocalizedString("CMOD_MOD_49_D"));
                    gmPlate.AddItem(giPlateHolder);
                    gmPlate.BindMenuToItem(mPlateHolder, giPlateHolder);
                }
                if (!arenavehicle.Contains(veh.Model))
                {
                    if (veh.GetModCount(VehicleMod.VanityPlates) != 0)
                    {
                        giVanityPlate = new UIMenuItem(LocalizedModTypeName(VehicleMod.VanityPlates), Game.GetLocalizedString("CMOD_SMOD_4_D"));
                        gmPlate.AddItem(giVanityPlate);
                        gmPlate.BindMenuToItem(mVanityPlates, giVanityPlate);
                    }
                }
                giNumberPlate = new UIMenuItem(LocalizedModGroupName(GroupName.License), Game.GetLocalizedString("CMOD_MOD_18_D"));
                gmPlate.AddItem(giNumberPlate);
                gmPlate.BindMenuToItem(mNumberPlate, giNumberPlate);
                gmPlate.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshLightsMenu()
        {
            try
            {
                gmLights.MenuItems.Clear();
                iHeadlights = new UIMenuItem(LocalizedModGroupName(GroupName.Headlights), Game.GetLocalizedString("CMOD_MOD_47_D"));
                gmLights.AddItem(iHeadlights);
                gmLights.BindMenuToItem(mHeadlights, iHeadlights);
                if (veh.HasBone("neon_b"))
                {
                    giNeonKits = new UIMenuItem(LocalizedModGroupName(GroupName.NeonKits), Game.GetLocalizedString("CMOD_MOD_6_D"));
                    gmLights.AddItem(giNeonKits);
                    gmLights.BindMenuToItem(gmNeonKits, giNeonKits);
                }
                gmLights.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshNeonKitsMenu()
        {
            try
            {
                gmNeonKits.MenuItems.Clear();
                iNeon = new UIMenuItem(LocalizedModGroupName(GroupName.NeonLayout));
                gmNeonKits.AddItem(iNeon);
                gmNeonKits.BindMenuToItem(mNeon, iNeon);
                if (veh.ClassType != VehicleClass.Motorcycles || veh.Model.ToString().Equals("blazer4", StringComparison.OrdinalIgnoreCase))
                {
                    iNeonColor = new UIMenuItem(LocalizedModGroupName(GroupName.NeonColor), Game.GetLocalizedString("CMOD_MOD_6_D"));
                    gmNeonKits.AddItem(iNeonColor);
                    gmNeonKits.BindMenuToItem(mNeonColor, iNeonColor);
                }
                gmNeonKits.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshNeonMenu()
        {
            try
            {
                mNeon.MenuItems.Clear();

                iNeon = new UIMenuItem(Game.GetLocalizedString("CMOD_NEONLAY_0"));
                {
                    var __with1 = iNeon;
                    if (NeonLayout() == NeonLayouts.None)
                    {
                        __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        __with1.Tag = new ModClass((int)NeonLayouts.None, 0);
                    }
                    else
                    {
                        __with1.Tag = new ModClass((int)NeonLayouts.None, 0);
                    }
                }
                mNeon.AddItem(iNeon);
                iNeon = new UIMenuItem(Game.GetLocalizedString("CMOD_NEONLAY_1"));
                {
                    var __with1 = iNeon;
                    if (NeonLayout() == NeonLayouts.Front)
                    {
                        __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        __with1.Tag = new ModClass((int)NeonLayouts.Front, 0);
                    }
                    else
                    {
                        __with1.SetRightLabel($"${1000}");
                        __with1.Tag = new ModClass((int)NeonLayouts.Front, 1000);
                    }
                }
                mNeon.AddItem(iNeon);
                iNeon = new UIMenuItem(Game.GetLocalizedString("CMOD_NEONLAY_2"));
                {
                    var __with1 = iNeon;
                    if (NeonLayout() == NeonLayouts.Back)
                    {
                        __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        __with1.Tag = new ModClass((int)NeonLayouts.Back, 0);
                    }
                    else
                    {
                        __with1.SetRightLabel($"${1000}");
                        __with1.Tag = new ModClass((int)NeonLayouts.Back, 1000);
                    }
                }
                mNeon.AddItem(iNeon);
                iNeon = new UIMenuItem(Game.GetLocalizedString("CMOD_NEONLAY_3"));
                {
                    var __with1 = iNeon;
                    if (NeonLayout() == NeonLayouts.Sides)
                    {
                        __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        __with1.Tag = new ModClass((int)NeonLayouts.Sides, 0);
                    }
                    else
                    {
                        __with1.SetRightLabel($"${1250}");
                        __with1.Tag = new ModClass((int)NeonLayouts.Sides, 1250);
                    }
                }
                mNeon.AddItem(iNeon);
                iNeon = new UIMenuItem(Game.GetLocalizedString("CMOD_NEONLAY_4"));
                {
                    var __with1 = iNeon;
                    if (NeonLayout() == NeonLayouts.FrontAndBack)
                    {
                        __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        __with1.Tag = new ModClass((int)NeonLayouts.FrontAndBack, 0);
                    }
                    else
                    {
                        __with1.SetRightLabel($"${1800}");
                        __with1.Tag = new ModClass((int)NeonLayouts.FrontAndBack, 1800);
                    }
                }
                mNeon.AddItem(iNeon);
                iNeon = new UIMenuItem(Game.GetLocalizedString("CMOD_NEONLAY_5"));
                {
                    var __with1 = iNeon;
                    if (NeonLayout() == NeonLayouts.FrontAndSides)
                    {
                        __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        __with1.Tag = new ModClass((int)NeonLayouts.FrontAndSides, 0);
                    }
                    else
                    {
                        __with1.SetRightLabel($"${2000}");
                        __with1.Tag = new ModClass((int)NeonLayouts.FrontAndSides, 2000);
                    }
                }
                mNeon.AddItem(iNeon);
                iNeon = new UIMenuItem(Game.GetLocalizedString("CMOD_NEONLAY_6"));
                {
                    var __with1 = iNeon;
                    if (NeonLayout() == NeonLayouts.BackAndSides)
                    {
                        __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        __with1.Tag = new ModClass((int)NeonLayouts.BackAndSides, 0);
                    }
                    else
                    {
                        __with1.SetRightLabel($"${2000}");
                        __with1.Tag = new ModClass((int)NeonLayouts.BackAndSides, 2000);
                    }
                }
                mNeon.AddItem(iNeon);
                iNeon = new UIMenuItem(Game.GetLocalizedString("CMOD_NEONLAY_7"));
                {
                    var __with1 = iNeon;
                    if (NeonLayout() == NeonLayouts.FrontBackAndSides)
                    {
                        __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        __with1.Tag = new ModClass((int)NeonLayouts.FrontBackAndSides, 0);
                    }
                    else
                    {
                        __with1.SetRightLabel($"${3000}");
                        __with1.Tag = new ModClass((int)NeonLayouts.FrontBackAndSides, 3000);
                    }
                }
                mNeon.AddItem(iNeon);
                mNeon.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshResprayMenu()
        {
            try
            {
                gmRespray.MenuItems.Clear();
                giPrimaryCol = new UIMenuItem(LocalizedModGroupName(GroupName.PrimaryColor), Game.GetLocalizedString("CMOD_MOD_6_D"));
                gmRespray.AddItem(giPrimaryCol);
                gmRespray.BindMenuToItem(mPrimaryColor, giPrimaryCol);
                giSecondaryCol = new UIMenuItem(LocalizedModGroupName(GroupName.SecondaryColor), Game.GetLocalizedString("CMOD_MOD_6_D"));
                gmRespray.AddItem(giSecondaryCol);
                gmRespray.BindMenuToItem(mSecondaryColor, giSecondaryCol);
                if (!bennysvehicle.Contains(veh.Model))
                {
                    iDashboardColor = new UIMenuItem(LocalizedModGroupName(GroupName.AccentColor), Game.GetLocalizedString("CMOD_MOD_6_D"));
                    gmRespray.AddItem(iDashboardColor);
                    gmRespray.BindMenuToItem(mLightsColor, iDashboardColor);
                    iTrimColor = new UIMenuItem(LocalizedModGroupName(GroupName.TrimColor), Game.GetLocalizedString("CMOD_MOD_6_D"));
                    gmRespray.AddItem(iTrimColor);
                    gmRespray.BindMenuToItem(mTrimColor, iTrimColor);
                }
                gmRespray.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshPrimaryColorMenu()
        {
            try
            {
                mPrimaryColor.MenuItems.Clear();
                iPrimaryChromeColor = new UIMenuItem(LocalizedColorGroupName(ColorType.Chrome), Game.GetLocalizedString("CMOD_MOD_6_D"));
                mPrimaryColor.AddItem(iPrimaryChromeColor);
                mPrimaryColor.BindMenuToItem(mPrimaryChromeColor, iPrimaryChromeColor);
                iPrimaryClassicColor = new UIMenuItem(LocalizedColorGroupName(ColorType.Classic), Game.GetLocalizedString("CMOD_MOD_6_D"));
                mPrimaryColor.AddItem(iPrimaryClassicColor);
                mPrimaryColor.BindMenuToItem(mPrimaryClassicColor, iPrimaryClassicColor);
                iPrimaryMatteColor = new UIMenuItem(LocalizedColorGroupName(ColorType.Matte), Game.GetLocalizedString("CMOD_MOD_6_D"));
                mPrimaryColor.AddItem(iPrimaryMatteColor);
                mPrimaryColor.BindMenuToItem(mPrimaryMatteColor, iPrimaryMatteColor);
                iPrimaryMetallicColor = new UIMenuItem(LocalizedColorGroupName(ColorType.Metallic), Game.GetLocalizedString("CMOD_MOD_6_D"));
                mPrimaryColor.AddItem(iPrimaryMetallicColor);
                mPrimaryColor.BindMenuToItem(mPrimaryMetallicColor, iPrimaryMetallicColor);
                iPrimaryMetalsColor = new UIMenuItem(LocalizedColorGroupName(ColorType.Metals), Game.GetLocalizedString("CMOD_MOD_6_D"));
                mPrimaryColor.AddItem(iPrimaryMetalsColor);
                mPrimaryColor.BindMenuToItem(mPrimaryMetalsColor, iPrimaryMetalsColor);
                iPrimaryPearlescentColor = new UIMenuItem(LocalizedColorGroupName(ColorType.Pearlescent), Game.GetLocalizedString("CMOD_MOD_6_D"));
                mPrimaryColor.AddItem(iPrimaryPearlescentColor);
                mPrimaryColor.BindMenuToItem(mPrimaryPearlescentColor, iPrimaryPearlescentColor);
                mPrimaryColor.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshSecondaryColorMenu()
        {
            try
            {
                mSecondaryColor.MenuItems.Clear();
                iSecondaryChromeColor = new UIMenuItem(LocalizedColorGroupName(ColorType.Chrome), Game.GetLocalizedString("CMOD_MOD_6_D"));
                mSecondaryColor.AddItem(iSecondaryChromeColor);
                mSecondaryColor.BindMenuToItem(mSecondaryChromeColor, iSecondaryChromeColor);
                iSecondaryClassicColor = new UIMenuItem(LocalizedColorGroupName(ColorType.Classic), Game.GetLocalizedString("CMOD_MOD_6_D"));
                mSecondaryColor.AddItem(iSecondaryClassicColor);
                mSecondaryColor.BindMenuToItem(mSecondaryClassicColor, iSecondaryClassicColor);
                iSecondaryMatteColor = new UIMenuItem(LocalizedColorGroupName(ColorType.Matte), Game.GetLocalizedString("CMOD_MOD_6_D"));
                mSecondaryColor.AddItem(iSecondaryMatteColor);
                mSecondaryColor.BindMenuToItem(mSecondaryMatteColor, iSecondaryMatteColor);
                iSecondaryMetallicColor = new UIMenuItem(LocalizedColorGroupName(ColorType.Metallic), Game.GetLocalizedString("CMOD_MOD_6_D"));
                mSecondaryColor.AddItem(iSecondaryMetallicColor);
                mSecondaryColor.BindMenuToItem(mSecondaryMetallicColor, iSecondaryMetallicColor);
                iSecondaryMetalsColor = new UIMenuItem(LocalizedColorGroupName(ColorType.Metals), Game.GetLocalizedString("CMOD_MOD_6_D"));
                mSecondaryColor.AddItem(iSecondaryMetalsColor);
                mSecondaryColor.BindMenuToItem(mSecondaryMetalsColor, iSecondaryMetalsColor);
                mSecondaryColor.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshColorMenuFor(ref UIMenu menu, ref UIMenuItem item, List<VehicleColor> colorList, string prisecpear)
        {
            try
            {
                menu.MenuItems.Clear();
                foreach (VehicleColor col in colorList)
                {
                    item = new UIMenuItem(GetLocalizedColorName(col));
                    {
                        var __with1 = item;
                        if (prisecpear == "Primary")
                        {
                            if (veh.Mods.PrimaryColor == col)
                            {
                                __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                __with1.Tag = new ModClass((int)col, 0);
                            }
                            else
                            {
                                item.SetRightLabel($"${2000}");
                                __with1.Tag = new ModClass((int)col, 2000);
                            }
                        }
                        else if (prisecpear == "Secondary")
                        {
                            if (veh.Mods.SecondaryColor == col)
                            {
                                __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                __with1.Tag = new ModClass((int)col, 0);
                            }
                            else
                            {
                                item.SetRightLabel($"${2000}");
                                __with1.Tag = new ModClass((int)col, 2000);
                            }
                        }
                        else if (prisecpear == "Pearlescent")
                        {
                            if (veh.Mods.PearlescentColor == col)
                            {
                                __with1.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                __with1.Tag = new ModClass((int)col, 0);
                            }
                            else
                            {
                                item.SetRightLabel($"${2000}");
                                __with1.Tag = new ModClass((int)col, 2000);
                            }
                        }
                    }
                    menu.AddItem(item);
                }
                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void RefreshRGBColorMenuFor(ref UIMenu menu, ref UIMenuItem item, string neonsmoke)
        {
            try
            {
                menu.MenuItems.Clear();
                var removeList = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "R", "G", "B", "A", "IsKnownColor", "IsEmpty", "IsNamedColor", "IsSystemColor", "Name", "Transparent"
            };

                foreach (Reflection.PropertyInfo col in typeof(Color).GetProperties())
                {
                    if (removeList.Contains(col.Name))
                    {
                        continue;
                    }

                    item = new UIMenuItem(RegularExpressions.Regex.Replace(col.Name, "[A-Z]", " $0").Trim());
                    var color = Color.FromName(col.Name);

                    if (string.Equals(neonsmoke, "Neon", StringComparison.OrdinalIgnoreCase))
                    {
                        if (veh.Mods.NeonLightsColor == color)
                        {
                            item.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                            item.Tag = new RGBModClass(color, 0);
                        }
                        else
                        {
                            item.SetRightLabel("$200");
                            item.Tag = new RGBModClass(color, 200);
                        }
                    }
                    else if (string.Equals(neonsmoke, "Smoke", StringComparison.OrdinalIgnoreCase))
                    {
                        if (veh.Mods.TireSmokeColor == color)
                        {
                            item.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                            item.Tag = new RGBModClass(color, 0);
                        }
                        else
                        {
                            item.SetRightLabel("$200");
                            item.Tag = new RGBModClass(color, 200);
                        }
                    }

                    menu.AddItem(item);
                }

                menu.RefreshIndex();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }
        #endregion

        public static void CreateMenus()
        {
            QuitMenu = NewUIMenu(ref QuitMenu, "CMOD_MOD_E", false, false, MainMenuCloseHandler, MainMenuItemSelectHandler, itemName: Game.GetLocalizedString("ITEM_EXIT"), itemDesc: Game.GetLocalizedString("collision_6p1r1v"));
            MainMenu = NewUIMenu(ref MainMenu, "CMOD_MOD_T", false, true, MainMenuCloseHandler, MainMenuItemSelectHandler);
            mUpgradeAW = NewUIMenu(ref mUpgradeAW, "collision_9znude7", false, false, selectHandler: ModsMenuItemSelectHandler, indexChangeHandler: ArenaWarMenuIndexChangedHandler);
            gmBodywork = NewUIMenu(ref gmBodywork, "CMOD_BW_T", false, true, selectHandler: ModsMenuItemSelectHandler);
            gmBodyworkArena = NewUIMenu(ref gmBodyworkArena, "CMOD_BW_T", false, true, selectHandler: ModsMenuItemSelectHandler);
            gmWeapon = NewUIMenu(ref gmWeapon, "PM_SCR_WEA", false, true, selectHandler: ModsMenuItemSelectHandler);
            mAerials = NewUIMenu(ref mAerials, "CMM_MOD_ST18", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mTrim = NewUIMenu(ref mTrim, "CMM_MOD_ST19", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mWindow = NewUIMenu(ref mWindow, "CMM_MOD_ST21", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mArchCover = NewUIMenu(ref mArchCover, "CMM_MOD_ST17", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmEngine = NewUIMenu(ref gmEngine, "CMM_MOD_GT3", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler);
            mEngine = NewUIMenu(ref mEngine, "CMM_MOD_GT3", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mNitro = NewUIMenu(ref mNitro, "CMM_MOD_TBOS", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mEngineBlock = NewUIMenu(ref mEngineBlock, "CMOD_EB_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mAirFilter = NewUIMenu(ref mAirFilter, "CMM_MOD_ST15", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mStruts = NewUIMenu(ref mStruts, "CMM_MOD_ST16", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmInterior = NewUIMenu(ref gmInterior, "CMM_MOD_GT1", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler);
            mColumnShifterLevers = NewUIMenu(ref mColumnShifterLevers, "CMM_MOD_ST9", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mDashboard = NewUIMenu(ref mDashboard, "CMM_MOD_ST4", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mLightsColor = NewUIMenu(ref mLightsColor, "CMM_MOD_ST26", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mDialDesign = NewUIMenu(ref mDialDesign, "CMM_MOD_ST5", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mOrnaments = NewUIMenu(ref mOrnaments, "CMM_MOD_ST3", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSeats = NewUIMenu(ref mSeats, "CMM_MOD_ST7", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSteeringWheels = NewUIMenu(ref mSteeringWheels, "CMM_MOD_ST8", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mTrimDesign = NewUIMenu(ref mTrimDesign, "CMM_MOD_ST2", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mTrimColor = NewUIMenu(ref mTrimColor, "CMOD_MOD_TRIM2", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mDoor = NewUIMenu(ref mDoor, "CMM_MOD_ST6", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmBumper = NewUIMenu(ref gmBumper, "CMOD_BUM_T", false, true, selectHandler: ModsMenuItemSelectHandler);
            mFBumper = NewUIMenu(ref mFBumper, "CMOD_BUMF_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mRBumper = NewUIMenu(ref mRBumper, "CMOD_BUMR_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSSkirt = NewUIMenu(ref mSSkirt, "CMOD_SS_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmWheels = NewUIMenu(ref gmWheels, "CMOD_WHE0_T", false, true, ModsMenuCloseHandler, WheelsMenuItemSelectHandler);
            gmWheelType = NewUIMenu(ref gmWheelType, "CMOD_WHE1_T", false, true, selectHandler: ModsMenuItemSelectHandler);
            gmBikeWheels = NewUIMenu(ref gmBikeWheels, GetLocalizedWheelTypeName(VehicleWheelType.BikeWheels).ToUpper(), true, selectHandler: ModsMenuItemSelectHandler);
            mSBikeWheels = NewUIMenu(ref mSBikeWheels, GetLocalizedWheelTypeName(VehicleWheelType.BikeWheels).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mCBikeWheels = NewUIMenu(ref mCBikeWheels, GetLocalizedWheelTypeName(VehicleWheelType.BikeWheels).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmHighEnd = NewUIMenu(ref gmHighEnd, GetLocalizedWheelTypeName(VehicleWheelType.HighEnd).ToUpper(), true, selectHandler: ModsMenuItemSelectHandler);
            mSHighEnd = NewUIMenu(ref mSHighEnd, GetLocalizedWheelTypeName(VehicleWheelType.HighEnd).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mCHighEnd = NewUIMenu(ref mCHighEnd, GetLocalizedWheelTypeName(VehicleWheelType.HighEnd).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmLowrider = NewUIMenu(ref gmLowrider, GetLocalizedWheelTypeName(VehicleWheelType.Lowrider).ToUpper(), true, selectHandler: ModsMenuItemSelectHandler);
            mSLowrider = NewUIMenu(ref mSLowrider, GetLocalizedWheelTypeName(VehicleWheelType.Lowrider).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mCLowrider = NewUIMenu(ref mCLowrider, GetLocalizedWheelTypeName(VehicleWheelType.Lowrider).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmMuscle = NewUIMenu(ref gmMuscle, GetLocalizedWheelTypeName(VehicleWheelType.Muscle).ToUpper(), true, selectHandler: ModsMenuItemSelectHandler);
            mSMuscle = NewUIMenu(ref mSMuscle, GetLocalizedWheelTypeName(VehicleWheelType.Muscle).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mCMuscle = NewUIMenu(ref mCMuscle, GetLocalizedWheelTypeName(VehicleWheelType.Muscle).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmOffroad = NewUIMenu(ref gmOffroad, GetLocalizedWheelTypeName(VehicleWheelType.Offroad).ToUpper(), true, selectHandler: ModsMenuItemSelectHandler);
            mSOffroad = NewUIMenu(ref mSOffroad, GetLocalizedWheelTypeName(VehicleWheelType.Offroad).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mCOffroad = NewUIMenu(ref mCOffroad, GetLocalizedWheelTypeName(VehicleWheelType.Offroad).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmSport = NewUIMenu(ref gmSport, GetLocalizedWheelTypeName(VehicleWheelType.Sport).ToUpper(), true, selectHandler: ModsMenuItemSelectHandler);
            mSSport = NewUIMenu(ref mSSport, GetLocalizedWheelTypeName(VehicleWheelType.Sport).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mCSport = NewUIMenu(ref mCSport, GetLocalizedWheelTypeName(VehicleWheelType.Sport).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmSUV = NewUIMenu(ref gmSUV, GetLocalizedWheelTypeName(VehicleWheelType.SUV).ToUpper(), true, selectHandler: ModsMenuItemSelectHandler);
            mSSUV = NewUIMenu(ref mSSUV, GetLocalizedWheelTypeName(VehicleWheelType.SUV).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mCSUV = NewUIMenu(ref mCSUV, GetLocalizedWheelTypeName(VehicleWheelType.SUV).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmTuner = NewUIMenu(ref gmTuner, GetLocalizedWheelTypeName(VehicleWheelType.Tuner).ToUpper(), true, selectHandler: ModsMenuItemSelectHandler);
            mSTuner = NewUIMenu(ref mSTuner, GetLocalizedWheelTypeName(VehicleWheelType.Tuner).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mCTuner = NewUIMenu(ref mCTuner, GetLocalizedWheelTypeName(VehicleWheelType.Tuner).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mBennysOriginals = NewUIMenu(ref mBennysOriginals, GetLocalizedWheelTypeName((VehicleWheelType)8).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mBespoke = NewUIMenu(ref mBespoke, GetLocalizedWheelTypeName((VehicleWheelType)9).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mRacing = NewUIMenu(ref mRacing, GetLocalizedWheelTypeName((VehicleWheelType)10).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mStreet = NewUIMenu(ref mStreet, GetLocalizedWheelTypeName((VehicleWheelType)11).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mRimColor = NewUIMenu(ref mRimColor, LocalizedModGroupName(GroupName.WheelColor).ToUpper(), true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mTires = NewUIMenu(ref mTires, "CMOD_TYR_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mTireSmoke = NewUIMenu(ref mTireSmoke, "CMOD_MOD_TYR3", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmPlate = NewUIMenu(ref gmPlate, "CMM_MOD_GT2", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler);
            mPlateHolder = NewUIMenu(ref mPlateHolder, "CMOD_PLH_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mVanityPlates = NewUIMenu(ref mVanityPlates, "CMM_MOD_ST1", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mNumberPlate = NewUIMenu(ref mNumberPlate, "CMOD_MOD_PLA2", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmLights = NewUIMenu(ref gmLights, "CMOD_LGT_T", false, true, ModsMenuCloseHandler);
            mHeadlights = NewUIMenu(ref mHeadlights, "CMOD_HED_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmNeonKits = NewUIMenu(ref gmNeonKits, "CMOD_MOD_LGT_N", true, true);
            mNeon = NewUIMenu(ref mNeon, "CMOD_NEON_0", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mNeonColor = NewUIMenu(ref mNeonColor, "CMOD_NEON_1", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            gmRespray = NewUIMenu(ref gmRespray, "CMOD_COL0_T", false, true);
            mPrimaryColor = NewUIMenu(ref mPrimaryColor, "CMOD_COL1_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mPrimaryClassicColor = NewUIMenu(ref mPrimaryClassicColor, "CMOD_COL0_0", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mPrimaryChromeColor = NewUIMenu(ref mPrimaryChromeColor, "CMOD_COL0_0", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mPrimaryMetallicColor = NewUIMenu(ref mPrimaryMetallicColor, "CMOD_COL0_0", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mPrimaryMetalsColor = NewUIMenu(ref mPrimaryMetalsColor, "CMOD_COL0_0", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mPrimaryMatteColor = NewUIMenu(ref mPrimaryMatteColor, "CMOD_COL0_0", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mPrimaryPearlescentColor = NewUIMenu(ref mPrimaryPearlescentColor, "CMOD_COL0_0", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSecondaryColor = NewUIMenu(ref mSecondaryColor, "CMOD_COL1_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSecondaryClassicColor = NewUIMenu(ref mSecondaryClassicColor, "CMOD_COL0_1", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSecondaryChromeColor = NewUIMenu(ref mSecondaryChromeColor, "CMOD_COL0_1", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSecondaryMetallicColor = NewUIMenu(ref mSecondaryMetallicColor, "CMOD_COL0_1", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSecondaryMetalsColor = NewUIMenu(ref mSecondaryMetalsColor, "CMOD_COL0_1", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSecondaryMatteColor = NewUIMenu(ref mSecondaryMatteColor, "CMOD_COL0_1", true, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mExhaust = NewUIMenu(ref mExhaust, "CMOD_EXH_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mFender = NewUIMenu(ref mFender, "CMOD_WNG_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mRFender = NewUIMenu(ref mRFender, "CMOD_WNG_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mFrame = NewUIMenu(ref mFrame, "CMOD_RC_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mGrille = NewUIMenu(ref mGrille, "CMOD_GRL_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mHood = NewUIMenu(ref mHood, "CMOD_BON_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mHorn = NewUIMenu(ref mHorn, "CMOD_HRN_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mHydraulics = NewUIMenu(ref mHydraulics, "CMM_MOD_ST13", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mLivery = NewUIMenu(ref mLivery, "CMM_MOD_ST23", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mTornadoC = NewUIMenu(ref mTornadoC, "CMOD_ROF_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mPlaques = NewUIMenu(ref mPlaques, "CMM_MOD_ST10", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mRoof = NewUIMenu(ref mRoof, "CMOD_ROF_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSpeakers = NewUIMenu(ref mSpeakers, "CMM_MOD_S11", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSpoilers = NewUIMenu(ref mSpoilers, "CMOD_SPO_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mTank = NewUIMenu(ref mTank, "CMM_MOD_ST20", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mTrunk = NewUIMenu(ref mTrunk, "CMOD_TR_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mTurbo = NewUIMenu(ref mTurbo, "CMOD_TUR_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mSuspension = NewUIMenu(ref mSuspension, "CMOD_SUS_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mArmor = NewUIMenu(ref mArmor, "CMOD_ARM_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mBrakes = NewUIMenu(ref mBrakes, "CMOD_BRA_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mTransmission = NewUIMenu(ref mTransmission, "CMOD_GBX_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mTint = NewUIMenu(ref mTint, "CMOD_WIN_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            // Motorcycles
            mShifter = NewUIMenu(ref mShifter, "CMOD_SHIFTER_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mFMudguard = NewUIMenu(ref mFMudguard, "CMOD_FMUD_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mBSeat = NewUIMenu(ref mBSeat, "CMM_MOD_ST7", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mOilTank = NewUIMenu(ref mOilTank, "CMM_MOD_ST29", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mRMudguard = NewUIMenu(ref mRMudguard, "CMOD_RMUD_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mFuelTank = NewUIMenu(ref mFuelTank, "CMOD_FUL_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mBeltDriveCovers = NewUIMenu(ref mBeltDriveCovers, "CMOD_MOD_BLT", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mBEngineBlock = NewUIMenu(ref mBEngineBlock, "CMOD_EB_T", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mBAirFilter = NewUIMenu(ref mBAirFilter, "CMM_MOD_ST15", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
            mBTank = NewUIMenu(ref mBTank, "CMM_MOD_ST20", false, true, ModsMenuCloseHandler, ModsMenuItemSelectHandler, ModsMenuIndexChangedHandler);
        }



        #region Menu Event Handlers

        public static void WheelsMenuItemSelectHandler(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            if (sender == gmWheels)
            {
                RefreshTyresMenu();
                if (selectedItem == iBPTires)
                {
                    if (iBPTires.RightBadge == UIMenuItem.BadgeStyle.Car)
                    {
                        veh.CanTiresBurst = true;
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Tick);
                        lastVehMemory.BulletProofTires = true;
                    }
                    else
                    {
                        veh.CanTiresBurst = false;
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        lastVehMemory.BulletProofTires = false;
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - Convert.ToInt32(selectedItem.Tag));
                        selectedItem.Tag = 0;
                    }
                }
            }
        }

        public static void ModsMenuItemSelectHandler(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            try
            {
                foreach (UIMenuItem i in sender.MenuItems)
                {
                    if (i.RightBadge == UIMenuItem.BadgeStyle.Car)
                    {
                        i.SetRightBadge(UIMenuItem.BadgeStyle.Tick);
                    }
                }

                // Arena War Upgrade
                if (sender == mUpgradeAW)
                {
                    GTA.UI.Screen.FadeOut(1000);
                    Script.Wait(1000);
                    Vehicle newAWVeh = World.CreateVehicle(((ArenaWarVehicle)(selectedItem.Tag)).Model, veh.Position, veh.Heading);
                    newAWVeh.IsPersistent = false;
                    newAWVeh.Mods.PrimaryColor = lastVehMemory.PrimaryColor;
                    newAWVeh.Mods.SecondaryColor = lastVehMemory.SecondaryColor;
                    newAWVeh.Mods.DashboardColor = lastVehMemory.LightsColor;
                    newAWVeh.Mods.PearlescentColor = lastVehMemory.PearlescentColor;
                    newAWVeh.Mods.TrimColor = lastVehMemory.TrimColor;
                    newAWVeh.Mods.RimColor = lastVehMemory.RimColor;
                    newAWVeh.Mods.NeonLightsColor = lastVehMemory.NeonLightsColor;
                    newAWVeh.Mods.TireSmokeColor = lastVehMemory.TireSmokeColor;
                    newAWVeh.InstallModKit();
                    newAWVeh.SetWheelType(lastVehMemory.WheelType);
                    newAWVeh.SetMod(VehicleMod.Aerials, lastVehMemory.Aerials, false);
                    newAWVeh.SetMod(VehicleMod.AirFilter, lastVehMemory.AirFilter, false);
                    newAWVeh.SetMod(VehicleMod.ArchCover, lastVehMemory.ArchCover, false);
                    newAWVeh.SetMod(VehicleMod.Armor, lastVehMemory.Armor, false);
                    newAWVeh.SetMod(VehicleMod.RearWheel, lastVehMemory.BackWheels, false);
                    newAWVeh.SetMod(VehicleMod.Brakes, lastVehMemory.Brakes, false);
                    newAWVeh.SetMod(VehicleMod.ColumnShifterLevers, lastVehMemory.ColumnShifterLevers, false);
                    newAWVeh.SetMod(VehicleMod.Dashboard, lastVehMemory.Dashboard, false);
                    newAWVeh.SetMod(VehicleMod.DialDesign, lastVehMemory.DialDesign, false);
                    newAWVeh.SetMod(VehicleMod.DoorSpeakers, lastVehMemory.DoorSpeakers, false);
                    newAWVeh.SetMod(VehicleMod.Engine, lastVehMemory.Engine, false);
                    newAWVeh.SetMod(VehicleMod.EngineBlock, lastVehMemory.EngineBlock, false);
                    newAWVeh.SetMod(VehicleMod.Exhaust, lastVehMemory.Exhaust, false);
                    newAWVeh.SetMod(VehicleMod.Fender, lastVehMemory.Fender, false);
                    newAWVeh.SetMod(VehicleMod.Frame, lastVehMemory.Frame, false);
                    newAWVeh.SetMod(VehicleMod.FrontBumper, lastVehMemory.FrontBumper, false);
                    newAWVeh.SetMod(VehicleMod.FrontWheel, lastVehMemory.FrontWheels, false);
                    newAWVeh.SetMod(VehicleMod.Grille, lastVehMemory.Grille, false);
                    newAWVeh.SetMod(VehicleMod.Hood, lastVehMemory.Hood, false);
                    newAWVeh.SetMod(VehicleMod.Horns, lastVehMemory.Horns, false);
                    newAWVeh.SetMod(VehicleMod.Hydraulics, lastVehMemory.Hydraulics, false);
                    newAWVeh.SetMod(VehicleMod.Livery, lastVehMemory.Livery, false);
                    newAWVeh.SetLivery2(lastVehMemory.Livery2);
                    newAWVeh.SetMod(VehicleMod.Ornaments, lastVehMemory.Ornaments, false);
                    newAWVeh.SetMod(VehicleMod.Plaques, lastVehMemory.Plaques, false);
                    newAWVeh.SetMod(VehicleMod.PlateHolder, lastVehMemory.PlateHolder, false);
                    newAWVeh.SetMod(VehicleMod.RearBumper, lastVehMemory.RearBumper, false);
                    newAWVeh.SetMod(VehicleMod.RightFender, lastVehMemory.RightFender, false);
                    newAWVeh.SetMod(VehicleMod.Roof, lastVehMemory.Roof, false);
                    newAWVeh.SetMod(VehicleMod.Seats, lastVehMemory.Seats, false);
                    newAWVeh.SetMod(VehicleMod.SideSkirt, lastVehMemory.SideSkirt, false);
                    newAWVeh.SetMod(VehicleMod.Speakers, lastVehMemory.Speakers, false);
                    newAWVeh.SetMod(VehicleMod.Spoilers, lastVehMemory.Spoilers, false);
                    newAWVeh.SetMod(VehicleMod.SteeringWheels, lastVehMemory.SteeringWheels, false);
                    newAWVeh.SetMod(VehicleMod.Struts, lastVehMemory.Struts, false);
                    newAWVeh.SetMod(VehicleMod.Suspension, lastVehMemory.Suspension, false);
                    newAWVeh.SetMod(VehicleMod.Tank, lastVehMemory.Tank, false);
                    newAWVeh.SetMod(VehicleMod.Transmission, lastVehMemory.Transmission, false);
                    newAWVeh.SetMod(VehicleMod.Trim, lastVehMemory.Trim, false);
                    newAWVeh.SetMod(VehicleMod.TrimDesign, lastVehMemory.TrimDesign, false);
                    newAWVeh.SetMod(VehicleMod.Trunk, lastVehMemory.Trunk, false);
                    newAWVeh.SetMod(VehicleMod.VanityPlates, lastVehMemory.VanityPlates, false);
                    newAWVeh.SetMod(VehicleMod.Windows, lastVehMemory.Windows, false);
                    newAWVeh.ToggleMod(VehicleToggleMod.TireSmoke, true);
                    newAWVeh.ToggleMod(VehicleToggleMod.Turbo, lastVehMemory.Turbo);
                    newAWVeh.ToggleMod(VehicleToggleMod.XenonHeadlights, lastVehMemory.Headlights);
                    newAWVeh.SetXenonHeadlightsColor(lastVehMemory.HeadlightsColor, newAWVeh.IsToggleModOn(VehicleToggleMod.XenonHeadlights));
                    newAWVeh.Mods.LicensePlateStyle = lastVehMemory.NumberPlate;
                    newAWVeh.Mods.LicensePlate = lastVehMemory.PlateNumbers;
                    newAWVeh.CanTiresBurst = lastVehMemory.BulletProofTires;
                    veh.Delete();
                    ply.Task.WarpIntoVehicle(newAWVeh, VehicleSeat.Driver);
                    veh = newAWVeh;
                    newAWVeh.InstallModKit();
                    MainMenu.MenuItems.Remove(iUpgradeAW);
                    isRepairing = true;
                    RefreshMenus();
                    camera.RepositionFor(newAWVeh);
                    Script.Wait(1000);
                    GTA.UI.Screen.FadeIn(1000);
                    Game.Player.Money = (Game.Player.Money - ((ArenaWarVehicle)(selectedItem.Tag)).Price);
                    Function.Call((Hash)0x2206BF9A37B7F724UL, "MP_corona_switch_supermod", 0, true);
                    Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Lowrider_Upgrade", "Lowrider_Super_Mod_Garage_Sounds", 1);
                    PlaySpeech("LR_UPGRADE_SUPERMOD");
                }

                // Performance Mods
                if (sender == mSuspension)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mi = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Suspension, mi.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mi.Price);
                        selectedItem.Tag = new ModClass(mi.ModID, 0);
                        lastVehMemory.Suspension = mi.ModID;
                        PlaySpeech("SHOP_SELL_SUSPENSION");
                    }
                }
                else if (sender == mArmor)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mi = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Armor, mi.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mi.Price);
                        selectedItem.Tag = new ModClass(mi.ModID, 0);
                        lastVehMemory.Armor = mi.ModID;
                        PlaySpeech("SHOP_SELL_ARMOUR");
                    }
                }
                else if (sender == mBrakes)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mi = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Brakes, mi.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mi.Price);
                        selectedItem.Tag = new ModClass(mi.ModID, 0);
                        lastVehMemory.Brakes = mi.ModID;
                        PlaySpeech("SHOP_SELL_BRAKES");
                    }
                }
                else if (sender == mTransmission)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mi = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Transmission, mi.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mi.Price);
                        selectedItem.Tag = new ModClass(mi.ModID, 0);
                        lastVehMemory.Transmission = mi.ModID;
                        PlaySpeech("SHOP_SELL_TRANS_UPGRADE");
                    }
                }
                else if (sender == mEngine)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mi = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Engine, mi.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mi.Price);
                        selectedItem.Tag = new ModClass(mi.ModID, 0);
                        lastVehMemory.Engine = mi.ModID;
                        PlaySpeech("SHOP_SELL_ENGINE_UPGRADE");
                    }
                }
                else if (sender == mNitro)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetInt(nitroMod, mc.ModID);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Nitro = mc.ModID;
                        PlaySpeech("SHOP_SELL_ENGINE_UPGRADE");
                    }
                }

                // Mods
                if (sender == mFBumper)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.FrontBumper, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.FrontBumper = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mRBumper)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.RearBumper, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.RearBumper = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mSSkirt)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.SideSkirt, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.SideSkirt = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mNumberPlate)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.Mods.LicensePlateStyle = (LicensePlateStyle)mc.ModID;
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.NumberPlate = (LicensePlateStyle)mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mHeadlights)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ToggleModClass tmc = (ToggleModClass)selectedItem.Tag;
                        veh.ToggleMod(VehicleToggleMod.XenonHeadlights, tmc.ModToggle);
                        if (selectedItem.Text == Game.GetLocalizedString("CMOD_LGT_0")) { veh.SetXenonHeadlightsColor(255, false); } else { veh.SetXenonHeadlightsColor(tmc.ModID, true); }
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - tmc.Price);
                        selectedItem.Tag = new ToggleModClass(tmc.ModToggle, tmc.ModID, 0);
                        lastVehMemory.Headlights = tmc.ModToggle;
                        lastVehMemory.HeadlightsColor = tmc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mArchCover)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.ArchCover, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.ArchCover = mc.ModID;
                        PlaySpeech("SHOP_SELL_COSMETICS");
                    }
                }
                else if (sender == mExhaust)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Exhaust, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Exhaust = mc.ModID;
                        PlaySpeech("SHOP_SELL_EXHAUST");
                    }
                }
                else if (sender == mFender)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Fender, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Fender = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mRFender)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.RightFender, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.RightFender = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mDoor)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.DoorSpeakers, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.DoorSpeakers = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mFrame)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Frame, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Frame = mc.ModID;
                        PlaySpeech("LR_SELL_EXCHASSIS_MOD");
                    }
                }
                else if (sender == mAerials)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Aerials, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Aerials = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mTrim)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Trim, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Trim = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mEngineBlock)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.EngineBlock, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.EngineBlock = mc.ModID;
                        PlaySpeech("LR_UPGRADE_ENGINE");
                    }
                }
                else if (sender == mAirFilter)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.AirFilter, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.AirFilter = mc.ModID;
                        PlaySpeech("LR_UPGRADE_ENGINE");
                    }
                }
                else if (sender == mStruts)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Struts, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Struts = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mColumnShifterLevers)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.ColumnShifterLevers, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.ColumnShifterLevers = mc.ModID;
                        PlaySpeech("LR_UPGRADE_GEARKNOB");
                    }
                }
                else if (sender == mDashboard)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Dashboard, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Dashboard = mc.ModID;
                        PlaySpeech("LR_SELL_SUPERMOD_INTERIOR");
                    }
                }
                else if (sender == mDialDesign)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.DialDesign, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.DialDesign = mc.ModID;
                        PlaySpeech("LR_SELL_SUPERMOD_INTERIOR");
                    }
                }
                else if (sender == mOrnaments)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Ornaments, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Ornaments = mc.ModID;
                        PlaySpeech("LR_SELL_DOLL");
                    }
                }
                else if (sender == mSeats)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Seats, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Seats = mc.ModID;
                        PlaySpeech("LR_SELL_SUPERMOD_INTERIOR");
                    }
                }
                else if (sender == mSteeringWheels)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.SteeringWheels, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.SteeringWheels = mc.ModID;
                        PlaySpeech("LR_SELL_SUPERMOD_INTERIOR");
                    }
                }
                else if (sender == mTrimDesign)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.TrimDesign, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.TrimDesign = mc.ModID;
                        PlaySpeech("LR_SELL_SUPERMOD_INTERIOR");
                    }
                }
                else if (sender == mPlateHolder)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.PlateHolder, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.PlateHolder = mc.ModID;
                        PlaySpeech("LR_UPGRADE_PLATEHOLDER");
                    }
                }
                else if (sender == mVanityPlates)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.VanityPlates, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.VanityPlates = mc.ModID;
                        PlaySpeech("LR_SELL_VANITYPLATE");
                    }
                }
                else if (sender == mGrille)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Grille, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Grille = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mHood)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Hood, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Hood = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mHorn)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Horns, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Horns = mc.ModID;
                        PlaySpeech("SHOP_SELL_HORN");
                    }
                }
                else if (sender == mHydraulics)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Hydraulics, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Hydraulics = mc.ModID;
                        PlaySpeech("LR_UPGRADE_HYDRAULICS");
                    }
                }
                else if (sender == mLivery)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Livery, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Livery = mc.ModID;
                        PlaySpeech("LR_SELL_LIVERY");
                    }
                }
                else if (sender == mTornadoC)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetLivery2(mc.ModID);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Livery2 = mc.ModID;
                        PlaySpeech("LR_SELL_LIVERY");
                    }
                }
                else if (sender == mPlaques)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Plaques, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Plaques = mc.ModID;
                        PlaySpeech("LR_UPGRADE_PLAQUE");
                    }
                }
                else if (sender == mRoof)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Roof, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Roof = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mSpeakers)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Speakers, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Speakers = mc.ModID;
                        PlaySpeech("LR_UPGRADE_ICE");
                    }
                }
                else if (sender == mSpoilers)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Spoilers, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Spoilers = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mTank)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Tank, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Tank = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mTrunk)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Trunk, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Trunk = mc.ModID;
                        PlaySpeech("LR_UPGRADE_TRUNK");
                    }
                }
                else if (sender == mWindow)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Windows, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Windows = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mTurbo)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.ToggleMod(VehicleToggleMod.Turbo, mc.ModIDBool());
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModIDBool(), 0);
                        lastVehMemory.Turbo = mc.ModIDBool();
                        PlaySpeech("SHOP_SELL_TURBO");
                    }
                }
                else if (sender == mTint)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.Mods.WindowTint = (VehicleWindowTint)mc.ModID;
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Tint = (VehicleWindowTint)mc.ModID;
                        PlaySpeech("");
                    }
                }

                // Bike Mods
                if (sender == mShifter)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Fender, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Fender = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mFMudguard)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.FrontBumper, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.FrontBumper = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mBSeat)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Hood, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Hood = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mOilTank)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Grille, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Grille = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mRMudguard)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.RearBumper, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.RearBumper = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mFuelTank)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Roof, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Roof = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mBeltDriveCovers)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Spoilers, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Spoilers = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mBEngineBlock)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Frame, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Frame = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mBAirFilter)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.SideSkirt, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.SideSkirt = mc.ModID;
                        PlaySpeech("");
                    }
                }
                else if (sender == mBTank)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.Tank, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.Tank = mc.ModID;
                        PlaySpeech("");
                    }
                }

                // Neons Mods
                if (sender == mNeon)
                {
                    ModClass mc = (ModClass)selectedItem.Tag;
                    switch ((NeonLayouts)mc.ModID)
                    {
                        case NeonLayouts.None:
                            if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                            {
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, false);
                                selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                selectedItem.SetRightLabel(null);
                                Game.Player.Money = Game.Player.Money - mc.Price;
                                selectedItem.Tag = new ModClass(mc.ModID, 0);
                                lastVehMemory.FrontNeon = false;
                                lastVehMemory.BackNeon = false;
                                lastVehMemory.LeftNeon = false;
                                lastVehMemory.RightNeon = false;
                            }
                            break;
                        case NeonLayouts.Front:
                            if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                            {
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, false);
                                selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                selectedItem.SetRightLabel(null);
                                Game.Player.Money = Game.Player.Money - mc.Price;
                                selectedItem.Tag = new ModClass(mc.ModID, 0);
                                lastVehMemory.FrontNeon = true;
                                lastVehMemory.BackNeon = false;
                                lastVehMemory.LeftNeon = false;
                                lastVehMemory.RightNeon = false;
                            }
                            break;
                        case NeonLayouts.Back:
                            if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                            {
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, false);
                                selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                selectedItem.SetRightLabel(null);
                                Game.Player.Money = Game.Player.Money - mc.Price;
                                selectedItem.Tag = new ModClass(mc.ModID, 0);
                                lastVehMemory.FrontNeon = false;
                                lastVehMemory.BackNeon = true;
                                lastVehMemory.LeftNeon = false;
                                lastVehMemory.RightNeon = false;
                            }
                            break;
                        case NeonLayouts.Sides:
                            if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                            {
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, true);
                                selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                selectedItem.SetRightLabel(null);
                                Game.Player.Money = Game.Player.Money - mc.Price;
                                selectedItem.Tag = new ModClass(mc.ModID, 0);
                                lastVehMemory.FrontNeon = false;
                                lastVehMemory.BackNeon = false;
                                lastVehMemory.LeftNeon = true;
                                lastVehMemory.RightNeon = true;
                            }
                            break;
                        case NeonLayouts.FrontAndBack:
                            if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                            {
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, false);
                                selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                selectedItem.SetRightLabel(null);
                                Game.Player.Money = Game.Player.Money - mc.Price;
                                selectedItem.Tag = new ModClass(mc.ModID, 0);
                                lastVehMemory.FrontNeon = true;
                                lastVehMemory.BackNeon = true;
                                lastVehMemory.LeftNeon = false;
                                lastVehMemory.RightNeon = false;
                            }
                            break;
                        case NeonLayouts.FrontAndSides:
                            if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                            {
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, true);
                                selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                selectedItem.SetRightLabel(null);
                                Game.Player.Money = Game.Player.Money - mc.Price;
                                selectedItem.Tag = new ModClass(mc.ModID, 0);
                                lastVehMemory.FrontNeon = true;
                                lastVehMemory.BackNeon = false;
                                lastVehMemory.LeftNeon = true;
                                lastVehMemory.RightNeon = true;
                            }
                            break;
                        case NeonLayouts.BackAndSides:
                            if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                            {
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, true);
                                selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                selectedItem.SetRightLabel(null);
                                Game.Player.Money = Game.Player.Money - mc.Price;
                                selectedItem.Tag = new ModClass(mc.ModID, 0);
                                lastVehMemory.FrontNeon = false;
                                lastVehMemory.BackNeon = true;
                                lastVehMemory.LeftNeon = true;
                                lastVehMemory.RightNeon = true;
                            }
                            break;
                        case NeonLayouts.FrontBackAndSides:
                            if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                            {
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, true);
                                selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                                selectedItem.SetRightLabel(null);
                                Game.Player.Money = Game.Player.Money - mc.Price;
                                selectedItem.Tag = new ModClass(mc.ModID, 0);
                                lastVehMemory.FrontNeon = true;
                                lastVehMemory.BackNeon = true;
                                lastVehMemory.LeftNeon = true;
                                lastVehMemory.RightNeon = true;
                            }
                            break;
                    }

                    PlaySpeech("");
                }
                // Wheels Mods
                if ((sender == mSBikeWheels) || (sender == mCBikeWheels))
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.FrontWheel, mc.ModID, false);
                        veh.SetMod(VehicleMod.RearWheel, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, mc.Price);
                        lastVehMemory.WheelType = veh.GetWheelType();
                        lastVehMemory.FrontWheels = mc.ModID;
                        lastVehMemory.BackWheels = mc.ModID;
                        PlaySpeech("LR_UPGRADE_WHEEL");
                    }
                }
                else if ((sender == mSHighEnd) || (sender == mSLowrider) || (sender == mSMuscle) || (sender == mSOffroad) || (sender == mSSport) || (sender == mSSUV) || (sender == mSTuner) || (sender == mCHighEnd) || (sender == mCLowrider) || (sender == mCMuscle) || (sender == mCOffroad) || (sender == mCSport) || (sender == mCSUV) || (sender == mCTuner) || (sender == mBennysOriginals) || (sender == mBespoke) || (sender == mRacing) || (sender == mStreet))
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.SetMod(VehicleMod.FrontWheel, mc.ModID, false);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.WheelType = veh.GetWheelType();
                        lastVehMemory.FrontWheels = mc.ModID;
                        PlaySpeech("LR_UPGRADE_WHEEL");
                    }
                }
                if (sender == mTires)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        bool bennysWheelType = veh.GetWheelType() == (VehicleWheelType)8 || veh.GetWheelType() == (VehicleWheelType)9 || veh.GetWheelType() == (VehicleWheelType)10 || veh.GetWheelType() == (VehicleWheelType)11;

                        if (bennysWheelType)
                        {
                            veh.SetMod(VehicleMod.FrontWheel, mc.ModID, false);
                            lastVehMemory.FrontWheels = mc.ModID;
                        }
                        else if (mc.ModID == 1)
                        {
                            veh.SetMod(VehicleMod.FrontWheel, veh.GetMod(VehicleMod.FrontWheel), false);
                            if (veh.ClassType == VehicleClass.Motorcycles)
                            {
                                veh.SetMod(VehicleMod.RearWheel, veh.GetMod(VehicleMod.RearWheel), false);
                            }
                            lastVehMemory.WheelsVariation = false;
                        }
                        else if (mc.ModID == 7)
                        {
                            veh.SetMod(VehicleMod.FrontWheel, veh.GetMod(VehicleMod.FrontWheel), true);
                            if (veh.ClassType == VehicleClass.Motorcycles)
                            {
                                veh.SetMod(VehicleMod.RearWheel, veh.GetMod(VehicleMod.RearWheel), true);
                            }
                            lastVehMemory.WheelsVariation = true;
                        }

                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = Game.Player.Money - mc.Price;
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        PlaySpeech("LR_UPGRADE_WHEEL");
                    }
                }

                // Wheel Type
                if (sender == gmWheelType)
                {
                    if (selectedItem == giBikeWheels)
                    {
                        veh.SetWheelType(VehicleWheelType.BikeWheels);
                        RefreshBikeWheelsModMenuFor(ref mSBikeWheels, ref iSBikeWheels, VehicleMod.RearWheel, false);
                        RefreshBikeWheelsModMenuFor(ref mCBikeWheels, ref iCBikeWheels, VehicleMod.RearWheel, true);
                    }
                    else if (selectedItem == giHighEndWheels)
                    {
                        veh.SetWheelType(VehicleWheelType.HighEnd);
                        RefreshStockWheelsModMenuFor(ref mSHighEnd, ref iSHighEnd, VehicleMod.FrontWheel);
                        RefreshChromeWheelsModMenuFor(ref mCHighEnd, ref iCHighEnd, VehicleMod.FrontWheel);
                    }
                    else if (selectedItem == giLowriderWheels)
                    {
                        veh.SetWheelType(VehicleWheelType.Lowrider);
                        RefreshStockWheelsModMenuFor(ref mSLowrider, ref iSLowrider, VehicleMod.FrontWheel);
                        RefreshChromeWheelsModMenuFor(ref mCLowrider, ref iCLowrider, VehicleMod.FrontWheel);
                    }
                    else if (selectedItem == giMuscleWheels)
                    {
                        veh.SetWheelType(VehicleWheelType.Muscle);
                        RefreshStockWheelsModMenuFor(ref mSMuscle, ref iSMuscle, VehicleMod.FrontWheel);
                        RefreshChromeWheelsModMenuFor(ref mCMuscle, ref iCMuscle, VehicleMod.FrontWheel);
                    }
                    else if (selectedItem == giOffroadWheels)
                    {
                        veh.SetWheelType(VehicleWheelType.Offroad);
                        RefreshStockWheelsModMenuFor(ref mSOffroad, ref iSOffroad, VehicleMod.FrontWheel);
                        RefreshChromeWheelsModMenuFor(ref mCOffroad, ref iCOffroad, VehicleMod.FrontWheel);
                    }
                    else if (selectedItem == giSportWheels)
                    {
                        veh.SetWheelType(VehicleWheelType.Sport);
                        RefreshStockWheelsModMenuFor(ref mSSport, ref iSSport, VehicleMod.FrontWheel);
                        RefreshChromeWheelsModMenuFor(ref mCSport, ref iCSport, VehicleMod.FrontWheel);
                    }
                    else if (selectedItem == giSUVWheels)
                    {
                        veh.SetWheelType(VehicleWheelType.SUV);
                        RefreshStockWheelsModMenuFor(ref mSSUV, ref iSSUV, VehicleMod.FrontWheel);
                        RefreshChromeWheelsModMenuFor(ref mCSUV, ref iCSUV, VehicleMod.FrontWheel);
                    }
                    else if (selectedItem == giTunerWheels)
                    {
                        veh.SetWheelType(VehicleWheelType.Tuner);
                        RefreshStockWheelsModMenuFor(ref mSTuner, ref iSTuner, VehicleMod.FrontWheel);
                        RefreshChromeWheelsModMenuFor(ref mCTuner, ref iCTuner, VehicleMod.FrontWheel);
                    }
                    else if (selectedItem == giBennysWheels)
                    {
                        veh.SetWheelType((VehicleWheelType)8);
                        RefreshLowriderDLCWheelsModMenuFor(ref mBennysOriginals, ref iBennys, VehicleMod.FrontWheel);
                    }
                    else if (selectedItem == giBespokeWheels)
                    {
                        veh.SetWheelType((VehicleWheelType)9);
                        RefreshLowriderDLCWheelsModMenuFor(ref mBespoke, ref iBespoke, VehicleMod.FrontWheel);
                    }
                    else if (selectedItem == giRacingWheels)
                    {
                        veh.SetWheelType((VehicleWheelType)10);
                        RefreshLowriderDLCWheelsModMenuFor(ref mRacing, ref iRacing, VehicleMod.FrontWheel);
                    }
                    else if (selectedItem == giStreetWheels)
                    {
                        veh.SetWheelType((VehicleWheelType)11);
                        RefreshLowriderDLCWheelsModMenuFor(ref mStreet, ref iStreet, VehicleMod.FrontWheel);
                    }
                }
                if (sender == gmBikeWheels)
                {
                    veh.SetWheelType(VehicleWheelType.BikeWheels);
                }
                else if (sender == gmHighEnd)
                {
                    veh.SetWheelType(VehicleWheelType.HighEnd);
                }
                else if (sender == gmLowrider)
                {
                    veh.SetWheelType(VehicleWheelType.Lowrider);
                }
                else if (sender == gmMuscle)
                {
                    veh.SetWheelType(VehicleWheelType.Muscle);
                }
                else if (sender == gmOffroad)
                {
                    veh.SetWheelType(VehicleWheelType.Offroad);
                }
                else if (sender == gmSport)
                {
                    veh.SetWheelType(VehicleWheelType.Sport);
                }
                else if (sender == gmSUV)
                {
                    veh.SetWheelType(VehicleWheelType.SUV);
                }
                else if (sender == gmTuner)
                {
                    veh.SetWheelType(VehicleWheelType.Tuner);
                }
                else if (sender == mBennysOriginals)
                {
                    veh.SetWheelType((VehicleWheelType)8);
                }
                else if (sender == mBespoke)
                {
                    veh.SetWheelType((VehicleWheelType)9);
                }
                else if (sender == mRacing)
                {
                    veh.SetWheelType((VehicleWheelType)10);
                }
                else if (sender == mStreet)
                {
                    veh.SetWheelType((VehicleWheelType)11);
                }

                // Color()
                if (sender == mLightsColor)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.Mods.DashboardColor = ((VehicleColor)(mc.ModID));
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.LightsColor = ((VehicleColor)(mc.ModID));
                        PlaySpeech("SHOP_SELL_COSMETICS");
                    }
                }
                else if (sender == mTrimColor)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.Mods.TrimColor = ((VehicleColor)(mc.ModID));
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.TrimColor = ((VehicleColor)(mc.ModID));
                        PlaySpeech("SHOP_SELL_COSMETICS");
                    }
                }
                else if (sender == mRimColor)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.Mods.RimColor = ((VehicleColor)(mc.ModID));
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.RimColor = ((VehicleColor)(mc.ModID));
                        PlaySpeech("SHOP_SELL_COSMETICS");
                    }
                }
                else if ((sender == mPrimaryChromeColor) || (sender == mPrimaryClassicColor) || (sender == mPrimaryMatteColor) || (sender == mPrimaryMetalsColor))
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.Mods.PrimaryColor = ((VehicleColor)(mc.ModID));
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.PrimaryColor = ((VehicleColor)(mc.ModID));
                        PlaySpeech("SHOP_SELL_COSMETICS");
                    }
                }
                else if (sender == mPrimaryMetallicColor)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.Mods.PrimaryColor = ((VehicleColor)(mc.ModID));
                        veh.Mods.PearlescentColor = ((VehicleColor)(mc.ModID));
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.PrimaryColor = ((VehicleColor)(mc.ModID));
                        lastVehMemory.PearlescentColor = ((VehicleColor)(mc.ModID));
                        PlaySpeech("SHOP_SELL_COSMETICS");
                    }
                }
                else if (sender == mPrimaryPearlescentColor)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.Mods.PearlescentColor = ((VehicleColor)(mc.ModID));
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.PearlescentColor = ((VehicleColor)(mc.ModID));
                        PlaySpeech("SHOP_SELL_COSMETICS");
                    }
                }
                else if ((sender == mSecondaryChromeColor) || (sender == mSecondaryClassicColor) || (sender == mSecondaryMatteColor) || (sender == mSecondaryMetallicColor) || (sender == mSecondaryMetalsColor))
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        ModClass mc = (ModClass)selectedItem.Tag;
                        veh.Mods.SecondaryColor = ((VehicleColor)(mc.ModID));
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new ModClass(mc.ModID, 0);
                        lastVehMemory.SecondaryColor = ((VehicleColor)(mc.ModID));
                        PlaySpeech("SHOP_SELL_COSMETICS");
                    }
                }
                else if (sender == mNeonColor)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        RGBModClass mc = (RGBModClass)selectedItem.Tag;
                        veh.Mods.NeonLightsColor = mc.Color();
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new RGBModClass(mc.Color(), 0);
                        lastVehMemory.NeonLightsColor = mc.Color();
                        PlaySpeech("");
                    }
                }
                else if (sender == mTireSmoke)
                {
                    if (selectedItem.RightBadge != UIMenuItem.BadgeStyle.Car)
                    {
                        RGBModClass mc = (RGBModClass)selectedItem.Tag;
                        veh.Mods.TireSmokeColor = mc.Color();
                        veh.ToggleMod(VehicleToggleMod.TireSmoke, true);
                        selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                        selectedItem.SetRightLabel(null);
                        Game.Player.Money = (Game.Player.Money - mc.Price);
                        selectedItem.Tag = new RGBModClass(mc.Color(), 0);
                        lastVehMemory.TireSmokeColor = mc.Color();
                        PlaySpeech("");
                    }
                }

                // Camera
                if (sender == gmBumper)
                {
                    if (selectedItem == giFBumper)
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "monster3":
                                break;
                            case "monster4":
                                break;
                            case "monster5":
                                break;
                            case "openwheel1":
                                break;
                            case "openwheel2":
                                break;
                            case "formula":
                                break;
                            case "formula2":
                                camera.MainCameraPosition = CameraPosition.Car;
                                break;
                            default:
                                if (veh.HasBone("neon_f"))
                                {
                                    camera.MainCameraPosition = CameraPosition.FrontBumper;
                                    break;
                                }
                                else
                                {
                                    camera.MainCameraPosition = CameraPosition.Hood;
                                }
                                break;
                        }
                    }
                    else if (selectedItem == giRBumper)
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "monster3":
                                break;
                            case "monster4":
                                break;
                            case "monster5":
                                camera.MainCameraPosition = CameraPosition.Car;
                                break;
                            default:
                                if (veh.HasBone("neon_r"))
                                {
                                    switch (veh.Model.ToString().ToLowerInvariant())
                                    {
                                        case "barrage":
                                            camera.MainCameraPosition = CameraPosition.Car;
                                            break;
                                        default:
                                            camera.MainCameraPosition = CameraPosition.RearBumper;
                                            break;
                                    }
                                    break;
                                }
                                else
                                {
                                    camera.MainCameraPosition = CameraPosition.Trunk;
                                }
                                break;
                        }
                    }
                    else if (selectedItem == giSSkirt)
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "barrage":
                                camera.MainCameraPosition = CameraPosition.Car;
                                break;
                            default:
                                camera.MainCameraPosition = CameraPosition.Wheels;
                                break;
                        }
                    }
                }
                else if (sender == gmPlate)
                {
                    if (selectedItem == giNumberPlate)
                    {
                        if (veh.HasBone("platelight"))
                        {
                            if (veh.ClassType == VehicleClass.Motorcycles || veh.Model.ToString().Equals("blazer4", StringComparison.OrdinalIgnoreCase))
                            {
                                camera.MainCameraPosition = CameraPosition.Car;
                            }
                            else
                            {
                                camera.MainCameraPosition = CameraPosition.BackPlate;
                            }
                        }
                        else if (veh.HasBone("neon_f"))
                        {
                            switch (veh.Model.ToString().ToLowerInvariant())
                            {
                                case "stromberg":
                                    break;
                                case "z190":
                                    break;
                                case "comet4":
                                    break;
                                case "autarch":
                                    camera.MainCameraPosition = CameraPosition.Car;
                                    break;
                                default:
                                    camera.MainCameraPosition = CameraPosition.FrontPlate;
                                    break;
                            }
                        }
                        else
                        {
                            camera.MainCameraPosition = CameraPosition.Car;
                        }
                    }
                    else if (selectedItem == giPlateHolder)
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "slamvan3":
                                break;
                            case "buccaneer2":
                                break;
                            case "chino2":
                                break;
                            case "sabregt2":
                                break;
                            case "voodoo":
                                break;
                            case "primo2":
                                break;
                            case "tornado5":
                                break;
                            case "minivan2":
                                camera.MainCameraPosition = CameraPosition.RearBumper;
                                break;
                            default:
                                camera.MainCameraPosition = CameraPosition.FrontBumper;
                                break;
                        }

                    }
                    else if (selectedItem == giVanityPlate)
                    {
                        camera.MainCameraPosition = CameraPosition.FrontBumper;
                    }
                }
                else if (sender == gmInterior)
                {
                    if (selectedItem == giDoor)
                    {
                        veh.OpenDoor(VehicleDoorIndex.FrontLeftDoor, false, false);
                        veh.OpenDoor(VehicleDoorIndex.FrontRightDoor, false, false);
                    }
                }
                else if (sender == gmBodywork)
                {
                    if (((selectedItem == giShifter) || (selectedItem == giFuelTank) || (selectedItem == giOilTank) || (selectedItem == giBeltDriveCovers) || (selectedItem == giBTank)))
                    {
                        camera.MainCameraPosition = CameraPosition.Wheels;
                    }
                    else if (selectedItem == giFMudguard)
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "blazer4":
                                camera.MainCameraPosition = CameraPosition.Engine;
                                break;
                            default:
                                camera.MainCameraPosition = CameraPosition.FrontMuguard;
                                break;
                        }

                    }
                    else if (selectedItem == giRMudguard)
                    {
                        camera.MainCameraPosition = CameraPosition.RearMuguard;
                    }
                }
                else if (sender == gmEngine)
                {
                    if (selectedItem == giStruts)
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "comet3":
                                veh.OpenDoor(VehicleDoorIndex.Trunk, false, false);
                                camera.MainCameraPosition = CameraPosition.FrontBumper;
                                break;
                        }
                    }
                }
                else if (sender == gmBodyworkArena)
                {
                    if (selectedItem == giOrnaments)
                    {
                        camera.MainCameraPosition = CameraPosition.Interior;
                    }
                }
                else if (sender == gmWeapon)
                {
                    if (selectedItem == giArchCover)
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "monster3":
                                break;
                            case "monster4":
                                break;
                            case "monster5":
                                camera.MainCameraPosition = CameraPosition.Car;
                                break;
                            default:
                                camera.MainCameraPosition = CameraPosition.FrontBumper;
                                break;
                        }
                    }
                    else if (selectedItem == giTank)
                    {
                        HoodCamera(false);
                    }
                    else if (selectedItem == giRoof)
                    {
                        if (veh.HasBone("boot"))
                        {
                            if (veh.GetVehTrunkPos() == EngineLoc.rear)
                            {
                                camera.MainCameraPosition = CameraPosition.Trunk;
                            }
                            else
                            {
                                if (veh.HasBone("windscreen_r"))
                                {
                                    camera.MainCameraPosition = CameraPosition.RearWindscreen;
                                }
                                else
                                {
                                    camera.MainCameraPosition = CameraPosition.RearEngine;
                                }
                            }
                        }
                        else if (veh.HasBone("windscreen_r"))
                        {
                            camera.MainCameraPosition = CameraPosition.RearWindscreen;
                        }
                        else if (veh.GetVehEnginePos() == EngineLoc.rear)
                        {
                            switch (veh.Model.ToString().ToLowerInvariant())
                            {
                                case "barrage":
                                    camera.MainCameraPosition = CameraPosition.Car;
                                    break;
                                default:
                                    camera.MainCameraPosition = CameraPosition.RearEngine;
                                    break;
                            }
                        }
                        else if (veh.HasBone("neon_b"))
                        {
                            camera.MainCameraPosition = CameraPosition.RearBumper;
                        }
                        else
                        {
                            camera.MainCameraPosition = CameraPosition.Car;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void ArenaWarMenuIndexChangedHandler(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            if (selectedItem?.Tag is ArenaWarVehicle arenaVehicle)
            {
                arenaVehImage = arenaVehicle.Image;
            }
        }

        public static void ModsMenuIndexChangedHandler(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            try
            {
                if (sender == mHeadlights)
                {
                    // Headlights color
                    ToggleModClass tmc = (ToggleModClass)selectedItem.Tag;
                    veh.ToggleMod(VehicleToggleMod.XenonHeadlights, tmc.ModToggle);
                    if (index == 0) { veh.SetXenonHeadlightsColor(tmc.ModID, false); } else { veh.SetXenonHeadlightsColor(tmc.ModID, true); }
                }
                else if ((sender == mNeonColor) || (sender == mTireSmoke))
                {
                    // RGB Color()
                    RGBModClass mc = (RGBModClass)selectedItem.Tag;
                    if (sender == mNeonColor)
                    {
                        veh.Mods.NeonLightsColor = mc.Color();
                    }
                    else if (sender == mTireSmoke)
                    {
                        veh.Mods.TireSmokeColor = mc.Color();
                        veh.ToggleMod(VehicleToggleMod.TireSmoke, true);
                    }
                }
                else
                {
                    ModClass mc = (ModClass)selectedItem.Tag;

                    // Performance
                    if (sender == mSuspension)
                    {
                        veh.SetMod(VehicleMod.Suspension, mc.ModID, false);
                    }
                    else if (sender == mArmor)
                    {
                        veh.SetMod(VehicleMod.Armor, mc.ModID, false);
                    }
                    else if (sender == mBrakes)
                    {
                        veh.SetMod(VehicleMod.Brakes, mc.ModID, false);
                    }
                    else if (sender == mTransmission)
                    {
                        veh.SetMod(VehicleMod.Transmission, mc.ModID, false);
                    }
                    else if (sender == mEngine)
                    {
                        veh.SetMod(VehicleMod.Engine, mc.ModID, false);
                    }
                    else if (sender == mNitro)
                    {
                        veh.SetInt(nitroMod, mc.ModID);
                    }

                    // Mod
                    if (sender == mFBumper)
                    {
                        veh.SetMod(VehicleMod.FrontBumper, mc.ModID, false);
                    }
                    else if (sender == mRBumper)
                    {
                        veh.SetMod(VehicleMod.RearBumper, mc.ModID, false);
                    }
                    else if (sender == mSSkirt)
                    {
                        veh.SetMod(VehicleMod.SideSkirt, mc.ModID, false);
                    }
                    else if (sender == mNumberPlate)
                    {
                        veh.Mods.LicensePlateStyle = (LicensePlateStyle)mc.ModID;
                    }
                    else if (sender == mArchCover)
                    {
                        veh.SetMod(VehicleMod.ArchCover, mc.ModID, false);
                    }
                    else if (sender == mExhaust)
                    {
                        veh.SetMod(VehicleMod.Exhaust, mc.ModID, false);
                    }
                    else if (sender == mFender)
                    {
                        veh.SetMod(VehicleMod.Fender, mc.ModID, false);
                    }
                    else if (sender == mRFender)
                    {
                        veh.SetMod(VehicleMod.RightFender, mc.ModID, false);
                    }
                    else if (sender == mDoor)
                    {
                        veh.SetMod(VehicleMod.DoorSpeakers, mc.ModID, false);
                    }
                    else if (sender == mFrame)
                    {
                        veh.SetMod(VehicleMod.Frame, mc.ModID, false);
                    }
                    else if (sender == mAerials)
                    {
                        veh.SetMod(VehicleMod.Aerials, mc.ModID, false);
                    }
                    else if (sender == mTrim)
                    {
                        veh.SetMod(VehicleMod.Trim, mc.ModID, false);
                    }
                    else if (sender == mEngineBlock)
                    {
                        veh.SetMod(VehicleMod.EngineBlock, mc.ModID, false);
                    }
                    else if (sender == mAirFilter)
                    {
                        veh.SetMod(VehicleMod.AirFilter, mc.ModID, false);
                    }
                    else if (sender == mStruts)
                    {
                        veh.SetMod(VehicleMod.Struts, mc.ModID, false);
                    }
                    else if (sender == mColumnShifterLevers)
                    {
                        veh.SetMod(VehicleMod.ColumnShifterLevers, mc.ModID, false);
                    }
                    else if (sender == mDashboard)
                    {
                        veh.SetMod(VehicleMod.Dashboard, mc.ModID, false);
                    }
                    else if (sender == mDialDesign)
                    {
                        veh.SetMod(VehicleMod.DialDesign, mc.ModID, false);
                    }
                    else if (sender == mOrnaments)
                    {
                        veh.SetMod(VehicleMod.Ornaments, mc.ModID, false);
                    }
                    else if (sender == mSeats)
                    {
                        veh.SetMod(VehicleMod.Seats, mc.ModID, false);
                    }
                    else if (sender == mSteeringWheels)
                    {
                        veh.SetMod(VehicleMod.SteeringWheels, mc.ModID, false);
                    }
                    else if (sender == mTrimDesign)
                    {
                        veh.SetMod(VehicleMod.TrimDesign, mc.ModID, false);
                    }
                    else if (sender == mPlateHolder)
                    {
                        veh.SetMod(VehicleMod.PlateHolder, mc.ModID, false);
                    }
                    else if (sender == mVanityPlates)
                    {
                        veh.SetMod(VehicleMod.VanityPlates, mc.ModID, false);
                    }
                    else if (sender == mGrille)
                    {
                        veh.SetMod(VehicleMod.Grille, mc.ModID, false);
                    }
                    else if (sender == mHood)
                    {
                        veh.SetMod(VehicleMod.Hood, mc.ModID, false);
                    }
                    else if (sender == mHorn)
                    {
                        veh.SetMod(VehicleMod.Horns, mc.ModID, false);
                        ply.Task.WarpIntoVehicle(veh, VehicleSeat.Passenger);
                        veh.SoundHorn(3000);
                    }
                    else if (sender == mHydraulics)
                    {
                        veh.SetMod(VehicleMod.Hydraulics, mc.ModID, false);
                    }
                    else if (sender == mLivery)
                    {
                        veh.SetMod(VehicleMod.Livery, mc.ModID, false);
                    }
                    else if (sender == mTornadoC)
                    {
                        veh.SetLivery2(mc.ModID);
                    }
                    else if (sender == mPlaques)
                    {
                        veh.SetMod(VehicleMod.Plaques, mc.ModID, false);
                    }
                    else if (sender == mRoof)
                    {
                        veh.SetMod(VehicleMod.Roof, mc.ModID, false);
                    }
                    else if (sender == mSpeakers)
                    {
                        veh.SetMod(VehicleMod.Speakers, mc.ModID, false);
                    }
                    else if (sender == mSpoilers)
                    {
                        veh.SetMod(VehicleMod.Spoilers, mc.ModID, false);
                    }
                    else if (sender == mTank)
                    {
                        veh.SetMod(VehicleMod.Tank, mc.ModID, false);
                    }
                    else if (sender == mTrunk)
                    {
                        veh.SetMod(VehicleMod.Trunk, mc.ModID, false);
                    }
                    else if (sender == mWindow)
                    {
                        veh.SetMod(VehicleMod.Windows, mc.ModID, false);
                    }
                    else if (sender == mTurbo)
                    {
                        veh.ToggleMod(VehicleToggleMod.Turbo, mc.ModIDBool());
                    }
                    else if (sender == mTint)
                    {
                        veh.Mods.WindowTint = (VehicleWindowTint)mc.ModID;
                    }

                    // Bike Mods
                    if (sender == mShifter)
                    {
                        veh.SetMod(VehicleMod.Fender, mc.ModID, false);
                    }
                    else if (sender == mFMudguard)
                    {
                        veh.SetMod(VehicleMod.FrontBumper, mc.ModID, false);
                    }
                    else if (sender == mBSeat)
                    {
                        veh.SetMod(VehicleMod.Hood, mc.ModID, false);
                    }
                    else if (sender == mOilTank)
                    {
                        veh.SetMod(VehicleMod.Grille, mc.ModID, false);
                    }
                    else if (sender == mRMudguard)
                    {
                        veh.SetMod(VehicleMod.RearBumper, mc.ModID, false);
                    }
                    else if (sender == mFuelTank)
                    {
                        veh.SetMod(VehicleMod.Roof, mc.ModID, false);
                    }
                    else if (sender == mBeltDriveCovers)
                    {
                        veh.SetMod(VehicleMod.Spoilers, mc.ModID, false);
                    }
                    else if (sender == mBEngineBlock)
                    {
                        veh.SetMod(VehicleMod.Frame, mc.ModID, false);
                    }
                    else if (sender == mBAirFilter)
                    {
                        veh.SetMod(VehicleMod.SideSkirt, mc.ModID, false);
                    }
                    else if (sender == mBTank)
                    {
                        veh.SetMod(VehicleMod.Tank, mc.ModID, false);
                    }

                    // Neons Mods
                    if (sender == mNeon)
                    {
                        switch (((NeonLayouts)(mc.ModID)))
                        {
                            case NeonLayouts.None:
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, false);
                                break;
                            case NeonLayouts.Front:
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, false);
                                break;
                            case NeonLayouts.Back:
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, false);
                                break;
                            case NeonLayouts.Sides:
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, true);
                                break;
                            case NeonLayouts.FrontAndBack:
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, false);
                                break;
                            case NeonLayouts.FrontAndSides:
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, true);
                                break;
                            case NeonLayouts.BackAndSides:
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, false);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, true);
                                break;
                            case NeonLayouts.FrontBackAndSides:
                                veh.SetNeonLightsOn(VehicleNeonLight.Back, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Front, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Left, true);
                                veh.SetNeonLightsOn(VehicleNeonLight.Right, true);
                                break;
                        }
                    }

                    // Wheels Mods
                    if ((sender == mSBikeWheels) || (sender == mCBikeWheels))
                    {
                        veh.SetMod(VehicleMod.FrontWheel, mc.ModID, false);
                        veh.SetMod(VehicleMod.RearWheel, mc.ModID, false);
                    }
                    else if ((sender == mSHighEnd) || (sender == mSLowrider) || (sender == mSMuscle) || (sender == mSOffroad) || (sender == mSSport) || (sender == mSSUV) || (sender == mSTuner) || (sender == mCHighEnd) || (sender == mCLowrider) || (sender == mCMuscle) || (sender == mCOffroad) || (sender == mCSport) || (sender == mCSUV) || (sender == mCTuner) || (sender == mBennysOriginals) || (sender == mBespoke) || (sender == mRacing) || (sender == mStreet))
                    {
                        veh.SetMod(VehicleMod.FrontWheel, mc.ModID, false);
                    }
                    if (sender == mTires)
                    {
                        bool bennysWheelType = veh.GetWheelType() == (VehicleWheelType)8 || veh.GetWheelType() == (VehicleWheelType)9 || veh.GetWheelType() == (VehicleWheelType)10 || veh.GetWheelType() == (VehicleWheelType)11;
                        if (bennysWheelType)
                        {
                            veh.SetMod(VehicleMod.FrontWheel, mc.ModID, false);
                        }
                        else if (mc.ModID == 1)
                        {
                            veh.SetMod(VehicleMod.FrontWheel, veh.GetMod(VehicleMod.FrontWheel), false);
                            if (veh.ClassType == VehicleClass.Motorcycles)
                            {
                                veh.SetMod(VehicleMod.RearWheel, veh.GetMod(VehicleMod.RearWheel), false);
                            }
                        }
                        else if (mc.ModID == 7)
                        {
                            veh.SetMod(VehicleMod.FrontWheel, veh.GetMod(VehicleMod.FrontWheel), true);
                            if (veh.ClassType == VehicleClass.Motorcycles)
                            {
                                veh.SetMod(VehicleMod.RearWheel, veh.GetMod(VehicleMod.RearWheel), true);
                            }
                        }
                    }

                    // Color()
                    if (sender == mLightsColor)
                    {
                        veh.Mods.DashboardColor = ((VehicleColor)(mc.ModID));
                    }
                    else if (sender == mTrimColor)
                    {
                        veh.Mods.TrimColor = ((VehicleColor)(mc.ModID));
                    }
                    else if (sender == mRimColor)
                    {
                        veh.Mods.RimColor = ((VehicleColor)(mc.ModID));
                    }
                    else if ((sender == mPrimaryChromeColor) || (sender == mPrimaryClassicColor) || (sender == mPrimaryMatteColor) || (sender == mPrimaryMetalsColor))
                    {
                        veh.Mods.PrimaryColor = ((VehicleColor)(mc.ModID));
                    }
                    else if (sender == mPrimaryMetallicColor)
                    {
                        veh.Mods.PrimaryColor = ((VehicleColor)(mc.ModID));
                        veh.Mods.PearlescentColor = ((VehicleColor)(mc.ModID));
                    }
                    else if (sender == mPrimaryPearlescentColor)
                    {
                        veh.Mods.PearlescentColor = ((VehicleColor)(mc.ModID));
                    }
                    else if ((sender == mSecondaryChromeColor) || (sender == mSecondaryClassicColor) || (sender == mSecondaryMatteColor) || (sender == mSecondaryMetallicColor) || (sender == mSecondaryMetalsColor))
                    {
                        veh.Mods.SecondaryColor = ((VehicleColor)(mc.ModID));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void MainMenuCloseHandler(UIMenu sender)
        {
            try
            {
                if (_suppressMenuRestoreOnClose)
                {
                    return;
                }

                if (sender == QuitMenu)
                {
                    MainMenu.Visible = true;
                }
                else if (sender == MainMenu)
                {
                    QuitMenu.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void MainMenuItemSelectHandler(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            try
            {
                if (sender == MainMenu)
                {
                    if (selectedItem == iRepair)
                    {
                        isRepairing = true;
                        veh.Repair();
                        veh.Wash();
                        Game.Player.Money = (Game.Player.Money - Convert.ToInt32(selectedItem.Tag));
                        RefreshMenus();
                    }
                    else if (selectedItem == iUpgrade || selectedItem == iRemoveUpgrade)
                    {
                        GTA.UI.Screen.FadeOut(1000);
                        Script.Wait(1000);
                        bool isRemovingUpgrade = selectedItem == iRemoveUpgrade;
                        Model replacementModel = isRemovingUpgrade
                            ? (Model)selectedItem.Tag
                            : LowriderUpgrade(veh.Model);
                        Vehicle newVeh = World.CreateVehicle(replacementModel, veh.Position, veh.Heading);
                        newVeh.IsPersistent = false;
                        newVeh.Mods.PrimaryColor = lastVehMemory.PrimaryColor;
                        newVeh.Mods.SecondaryColor = lastVehMemory.SecondaryColor;
                        newVeh.Mods.DashboardColor = lastVehMemory.LightsColor;
                        newVeh.Mods.PearlescentColor = lastVehMemory.PearlescentColor;
                        newVeh.Mods.TrimColor = lastVehMemory.TrimColor;
                        newVeh.Mods.RimColor = lastVehMemory.RimColor;
                        newVeh.Mods.NeonLightsColor = lastVehMemory.NeonLightsColor;
                        newVeh.Mods.TireSmokeColor = lastVehMemory.TireSmokeColor;
                        newVeh.InstallModKit();
                        newVeh.SetWheelType(lastVehMemory.WheelType);
                        newVeh.SetMod(VehicleMod.Aerials, lastVehMemory.Aerials, false);
                        newVeh.SetMod(VehicleMod.AirFilter, lastVehMemory.AirFilter, false);
                        newVeh.SetMod(VehicleMod.ArchCover, lastVehMemory.ArchCover, false);
                        newVeh.SetMod(VehicleMod.Armor, lastVehMemory.Armor, false);
                        newVeh.SetMod(VehicleMod.RearWheel, lastVehMemory.BackWheels, false);
                        newVeh.SetMod(VehicleMod.Brakes, lastVehMemory.Brakes, false);
                        newVeh.SetMod(VehicleMod.ColumnShifterLevers, lastVehMemory.ColumnShifterLevers, false);
                        newVeh.SetMod(VehicleMod.Dashboard, lastVehMemory.Dashboard, false);
                        newVeh.SetMod(VehicleMod.DialDesign, lastVehMemory.DialDesign, false);
                        newVeh.SetMod(VehicleMod.DoorSpeakers, lastVehMemory.DoorSpeakers, false);
                        newVeh.SetMod(VehicleMod.Engine, lastVehMemory.Engine, false);
                        newVeh.SetMod(VehicleMod.EngineBlock, lastVehMemory.EngineBlock, false);
                        newVeh.SetMod(VehicleMod.Exhaust, lastVehMemory.Exhaust, false);
                        newVeh.SetMod(VehicleMod.Fender, lastVehMemory.Fender, false);
                        newVeh.SetMod(VehicleMod.Frame, lastVehMemory.Frame, false);
                        newVeh.SetMod(VehicleMod.FrontBumper, lastVehMemory.FrontBumper, false);
                        newVeh.SetMod(VehicleMod.FrontWheel, lastVehMemory.FrontWheels, false);
                        newVeh.SetMod(VehicleMod.Grille, lastVehMemory.Grille, false);
                        newVeh.SetMod(VehicleMod.Hood, lastVehMemory.Hood, false);
                        newVeh.SetMod(VehicleMod.Horns, lastVehMemory.Horns, false);
                        newVeh.SetMod(VehicleMod.Hydraulics, lastVehMemory.Hydraulics, false);
                        newVeh.SetMod(VehicleMod.Livery, lastVehMemory.Livery, false);
                        newVeh.SetLivery2(lastVehMemory.Livery2);
                        newVeh.SetMod(VehicleMod.Ornaments, lastVehMemory.Ornaments, false);
                        newVeh.SetMod(VehicleMod.Plaques, lastVehMemory.Plaques, false);
                        newVeh.SetMod(VehicleMod.PlateHolder, lastVehMemory.PlateHolder, false);
                        newVeh.SetMod(VehicleMod.RearBumper, lastVehMemory.RearBumper, false);
                        newVeh.SetMod(VehicleMod.RightFender, lastVehMemory.RightFender, false);
                        newVeh.SetMod(VehicleMod.Roof, lastVehMemory.Roof, false);
                        newVeh.SetMod(VehicleMod.Seats, lastVehMemory.Seats, false);
                        newVeh.SetMod(VehicleMod.SideSkirt, lastVehMemory.SideSkirt, false);
                        newVeh.SetMod(VehicleMod.Speakers, lastVehMemory.Speakers, false);
                        newVeh.SetMod(VehicleMod.Spoilers, lastVehMemory.Spoilers, false);
                        newVeh.SetMod(VehicleMod.SteeringWheels, lastVehMemory.SteeringWheels, false);
                        newVeh.SetMod(VehicleMod.Struts, lastVehMemory.Struts, false);
                        newVeh.SetMod(VehicleMod.Suspension, lastVehMemory.Suspension, false);
                        newVeh.SetMod(VehicleMod.Tank, lastVehMemory.Tank, false);
                        newVeh.SetMod(VehicleMod.Transmission, lastVehMemory.Transmission, false);
                        newVeh.SetMod(VehicleMod.Trim, lastVehMemory.Trim, false);
                        newVeh.SetMod(VehicleMod.TrimDesign, lastVehMemory.TrimDesign, false);
                        newVeh.SetMod(VehicleMod.Trunk, lastVehMemory.Trunk, false);
                        newVeh.SetMod(VehicleMod.VanityPlates, lastVehMemory.VanityPlates, false);
                        newVeh.SetMod(VehicleMod.Windows, lastVehMemory.Windows, false);
                        newVeh.ToggleMod(VehicleToggleMod.TireSmoke, true);
                        newVeh.ToggleMod(VehicleToggleMod.Turbo, lastVehMemory.Turbo);
                        newVeh.ToggleMod(VehicleToggleMod.XenonHeadlights, lastVehMemory.Headlights);
                        newVeh.SetXenonHeadlightsColor(lastVehMemory.HeadlightsColor, newVeh.IsToggleModOn(VehicleToggleMod.XenonHeadlights));
                        newVeh.Mods.LicensePlateStyle = lastVehMemory.NumberPlate;
                        newVeh.Mods.LicensePlate = lastVehMemory.PlateNumbers;
                        newVeh.CanTiresBurst = lastVehMemory.BulletProofTires;
                        if (IsNitroModInstalled()) { newVeh.SetInt(nitroMod, lastVehMemory.Nitro); }
                        veh.Delete();
                        ply.Task.WarpIntoVehicle(newVeh, VehicleSeat.Driver);
                        veh = newVeh;
                        newVeh.InstallModKit();
                        MainMenu.MenuItems.Remove(selectedItem);
                        isRepairing = true;
                        RefreshMenus();
                        camera.RepositionFor(newVeh);
                        Script.Wait(1000);
                        GTA.UI.Screen.FadeIn(1000);
                        if (!isRemovingUpgrade)
                        {
                            Game.Player.Money = (Game.Player.Money - Convert.ToInt32(selectedItem.Tag));
                        }
                        Function.Call((Hash)0x2206BF9A37B7F724UL, "MP_corona_switch_supermod", 0, true);
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Lowrider_Upgrade", "Lowrider_Super_Mod_Garage_Sounds", 1);
                        PlaySpeech("LR_UPGRADE_SUPERMOD");
                    }
                    else if (selectedItem == iUpgradeMod)
                    {
                        GTA.UI.Screen.FadeOut(1000);
                        Script.Wait(1000);
                        Tuple<string, int> upgrade2 = veh.DisplayName.GetUpgradeModVehicleInfo();
                        Vehicle newVeh = World.CreateVehicle(upgrade2.Item1, veh.Position, veh.Heading);
                        newVeh.IsPersistent = false;
                        newVeh.Mods.PrimaryColor = lastVehMemory.PrimaryColor;
                        newVeh.Mods.SecondaryColor = lastVehMemory.SecondaryColor;
                        newVeh.Mods.DashboardColor = lastVehMemory.LightsColor;
                        newVeh.Mods.PearlescentColor = lastVehMemory.PearlescentColor;
                        newVeh.Mods.TrimColor = lastVehMemory.TrimColor;
                        newVeh.Mods.RimColor = lastVehMemory.RimColor;
                        newVeh.Mods.NeonLightsColor = lastVehMemory.NeonLightsColor;
                        newVeh.Mods.TireSmokeColor = lastVehMemory.TireSmokeColor;
                        newVeh.InstallModKit();
                        newVeh.SetWheelType(lastVehMemory.WheelType);
                        newVeh.SetMod(VehicleMod.Aerials, lastVehMemory.Aerials, false);
                        newVeh.SetMod(VehicleMod.AirFilter, lastVehMemory.AirFilter, false);
                        newVeh.SetMod(VehicleMod.ArchCover, lastVehMemory.ArchCover, false);
                        newVeh.SetMod(VehicleMod.Armor, lastVehMemory.Armor, false);
                        newVeh.SetMod(VehicleMod.RearWheel, lastVehMemory.BackWheels, false);
                        newVeh.SetMod(VehicleMod.Brakes, lastVehMemory.Brakes, false);
                        newVeh.SetMod(VehicleMod.ColumnShifterLevers, lastVehMemory.ColumnShifterLevers, false);
                        newVeh.SetMod(VehicleMod.Dashboard, lastVehMemory.Dashboard, false);
                        newVeh.SetMod(VehicleMod.DialDesign, lastVehMemory.DialDesign, false);
                        newVeh.SetMod(VehicleMod.DoorSpeakers, lastVehMemory.DoorSpeakers, false);
                        newVeh.SetMod(VehicleMod.Engine, lastVehMemory.Engine, false);
                        newVeh.SetMod(VehicleMod.EngineBlock, lastVehMemory.EngineBlock, false);
                        newVeh.SetMod(VehicleMod.Exhaust, lastVehMemory.Exhaust, false);
                        newVeh.SetMod(VehicleMod.Fender, lastVehMemory.Fender, false);
                        newVeh.SetMod(VehicleMod.Frame, lastVehMemory.Frame, false);
                        newVeh.SetMod(VehicleMod.FrontBumper, lastVehMemory.FrontBumper, false);
                        newVeh.SetMod(VehicleMod.FrontWheel, lastVehMemory.FrontWheels, false);
                        newVeh.SetMod(VehicleMod.Grille, lastVehMemory.Grille, false);
                        newVeh.SetMod(VehicleMod.Hood, lastVehMemory.Hood, false);
                        newVeh.SetMod(VehicleMod.Horns, lastVehMemory.Horns, false);
                        newVeh.SetMod(VehicleMod.Hydraulics, lastVehMemory.Hydraulics, false);
                        newVeh.SetMod(VehicleMod.Livery, lastVehMemory.Livery, false);
                        newVeh.SetLivery2(lastVehMemory.Livery2);
                        newVeh.SetMod(VehicleMod.Ornaments, lastVehMemory.Ornaments, false);
                        newVeh.SetMod(VehicleMod.Plaques, lastVehMemory.Plaques, false);
                        newVeh.SetMod(VehicleMod.PlateHolder, lastVehMemory.PlateHolder, false);
                        newVeh.SetMod(VehicleMod.RearBumper, lastVehMemory.RearBumper, false);
                        newVeh.SetMod(VehicleMod.RightFender, lastVehMemory.RightFender, false);
                        newVeh.SetMod(VehicleMod.Roof, lastVehMemory.Roof, false);
                        newVeh.SetMod(VehicleMod.Seats, lastVehMemory.Seats, false);
                        newVeh.SetMod(VehicleMod.SideSkirt, lastVehMemory.SideSkirt, false);
                        newVeh.SetMod(VehicleMod.Speakers, lastVehMemory.Speakers, false);
                        newVeh.SetMod(VehicleMod.Spoilers, lastVehMemory.Spoilers, false);
                        newVeh.SetMod(VehicleMod.SteeringWheels, lastVehMemory.SteeringWheels, false);
                        newVeh.SetMod(VehicleMod.Struts, lastVehMemory.Struts, false);
                        newVeh.SetMod(VehicleMod.Suspension, lastVehMemory.Suspension, false);
                        newVeh.SetMod(VehicleMod.Tank, lastVehMemory.Tank, false);
                        newVeh.SetMod(VehicleMod.Transmission, lastVehMemory.Transmission, false);
                        newVeh.SetMod(VehicleMod.Trim, lastVehMemory.Trim, false);
                        newVeh.SetMod(VehicleMod.TrimDesign, lastVehMemory.TrimDesign, false);
                        newVeh.SetMod(VehicleMod.Trunk, lastVehMemory.Trunk, false);
                        newVeh.SetMod(VehicleMod.VanityPlates, lastVehMemory.VanityPlates, false);
                        newVeh.SetMod(VehicleMod.Windows, lastVehMemory.Windows, false);
                        newVeh.ToggleMod(VehicleToggleMod.TireSmoke, true);
                        newVeh.ToggleMod(VehicleToggleMod.Turbo, lastVehMemory.Turbo);
                        newVeh.ToggleMod(VehicleToggleMod.XenonHeadlights, lastVehMemory.Headlights);
                        newVeh.SetXenonHeadlightsColor(lastVehMemory.HeadlightsColor, newVeh.IsToggleModOn(VehicleToggleMod.XenonHeadlights));
                        newVeh.Mods.LicensePlateStyle = lastVehMemory.NumberPlate;
                        newVeh.Mods.LicensePlate = lastVehMemory.PlateNumbers;
                        newVeh.CanTiresBurst = lastVehMemory.BulletProofTires;
                        if (IsNitroModInstalled()) { newVeh.SetInt(nitroMod, lastVehMemory.Nitro); }
                        veh.Delete();
                        ply.Task.WarpIntoVehicle(newVeh, VehicleSeat.Driver);
                        veh = newVeh;
                        newVeh.InstallModKit();
                        MainMenu.MenuItems.Remove(selectedItem);
                        isRepairing = true;
                        RefreshMenus();
                        camera.RepositionFor(newVeh);
                        Script.Wait(1000);
                        GTA.UI.Screen.FadeIn(1000);
                        Game.Player.Money = (Game.Player.Money - Convert.ToInt32(selectedItem.Tag));
                        Function.Call((Hash)0x2206BF9A37B7F724UL, "MP_corona_switch_supermod", 0, true);
                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Lowrider_Upgrade", "Lowrider_Super_Mod_Garage_Sounds", 1);
                        PlaySpeech("LR_UPGRADE_SUPERMOD");
                    }
                    else if (selectedItem == iUpgradeAW)
                    {
                        var sitem = mUpgradeAW.MenuItems.First();
                        arenaVehImage = ((ArenaWarVehicle)(sitem.Tag)).Image;
                    }
                    else if (selectedItem == giEngine)
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "alpha":
                                veh.OpenDoor(VehicleDoorIndex.Hood, false, false);
                                camera.MainCameraPosition = CameraPosition.Hood;
                                break;
                            case "openwheel1":
                                break;
                            case "openwheel2":
                                break;
                            case "formula":
                                break;
                            case "formula2":
                                camera.MainCameraPosition = CameraPosition.RearEngine;
                                break;
                            default:
                                if (veh.ClassType != VehicleClass.Motorcycles || veh.Model.ToString().Equals("blazer4", StringComparison.OrdinalIgnoreCase))
                                {
                                    HoodCamera(true);
                                    break;
                                }
                                else
                                {
                                    camera.MainCameraPosition = CameraPosition.Wheels;
                                }
                                break;
                        }
                    }
                    else if (selectedItem == giInterior)
                    {
                        if (veh.ClassType == VehicleClass.Motorcycles || veh.Model.ToString().Equals("blazer4", StringComparison.OrdinalIgnoreCase))
                        {
                            camera.MainCameraPosition = CameraPosition.Car;
                        }
                        else
                        {
                            camera.MainCameraPosition = CameraPosition.Interior;
                        }
                    }
                    else if (selectedItem == giWheels)
                    {
                        camera.MainCameraPosition = CameraPosition.Wheels;
                    }
                    else if (selectedItem == giLights)
                    {
                        veh.SetHighBeamsState(true);
                        veh.SetLightsState(true);
                    }
                    else if (selectedItem == giExhaust)
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "sultanrs":
                                break;
                            case "guardian":
                                break;
                            case "ratloader":
                                break;
                            case "ratloader2":
                                break;
                            case "banshee":
                                break;
                            case "mamba":
                                break;
                            case "feltzer3":
                                break;
                            case "le7b":
                                break;
                            case "barrage":
                                camera.MainCameraPosition = CameraPosition.Wheels;
                                break;
                            case "police3":
                                camera.MainCameraPosition = CameraPosition.Trunk;
                                break;
                            case "tornado6":
                                camera.MainCameraPosition = CameraPosition.Engine;
                                break;
                            default:
                                if (veh.ClassType == VehicleClass.Motorcycles || veh.Model.ToString().Equals("blazer4", StringComparison.OrdinalIgnoreCase))
                                {
                                    camera.MainCameraPosition = CameraPosition.BikeExhaust;
                                    break;
                                }
                                else
                                {
                                    camera.MainCameraPosition = CameraPosition.Exhaust;
                                }
                                break;
                        }
                    }
                    else if (selectedItem == giBrakes)
                    {
                        camera.MainCameraPosition = CameraPosition.Wheels;
                    }
                    else if (selectedItem == giGrille)
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "penetrator":
                                break;
                            case "torero":
                                break;
                            case "viseris":
                                camera.MainCameraPosition = CameraPosition.RearEngine;
                                break;
                            case "banshee2":
                                camera.MainCameraPosition = CameraPosition.Trunk;
                                break;
                            case "zr3802":
                                camera.MainCameraPosition = CameraPosition.Car;
                                break;
                            default:
                                camera.MainCameraPosition = CameraPosition.Grille;
                                break;
                        }
                    }
                    else if (selectedItem == giHood)
                    {
                        HoodCamera(false);
                    }
                    else if (selectedItem == giHydraulics)
                    {
                        veh.OpenDoor(VehicleDoorIndex.Trunk, false, false);
                        camera.MainCameraPosition = CameraPosition.Trunk;
                    }
                    else if (selectedItem == giTrunk)
                    {
                        veh.OpenDoor(VehicleDoorIndex.Trunk, false, false);
                        camera.MainCameraPosition = CameraPosition.Trunk;
                    }
                    else if (selectedItem == giPlaques)
                    {
                        camera.MainCameraPosition = CameraPosition.Plaque;
                    }
                    else if (selectedItem == giSpoilers)
                    {
                        if (veh.HasBone("boot"))
                        {
                            if (veh.GetVehTrunkPos() == EngineLoc.rear)
                            {
                                camera.MainCameraPosition = CameraPosition.Trunk;
                            }
                            else
                            {
                                if (veh.HasBone("windscreen_r"))
                                {
                                    camera.MainCameraPosition = CameraPosition.RearWindscreen;
                                }
                                else
                                {
                                    camera.MainCameraPosition = CameraPosition.RearEngine;
                                }
                            }
                        }
                        else if (veh.HasBone("windscreen_r"))
                        {
                            camera.MainCameraPosition = CameraPosition.RearWindscreen;
                        }
                        else if (veh.GetVehEnginePos() == EngineLoc.rear)
                        {
                            switch (veh.Model.ToString().ToLowerInvariant())
                            {
                                case "barrage":
                                    camera.MainCameraPosition = CameraPosition.Car;
                                    break;
                                default:
                                    camera.MainCameraPosition = CameraPosition.RearEngine;
                                    break;
                            }
                        }
                        else if (veh.HasBone("neon_b"))
                        {
                            camera.MainCameraPosition = CameraPosition.RearBumper;
                        }
                        else
                        {
                            camera.MainCameraPosition = CameraPosition.Car;
                        }
                    }
                    else if (selectedItem == giTank)
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "slamvan3":
                                camera.MainCameraPosition = CameraPosition.Trunk;
                                break;
                            case "elegy":
                                camera.MainCameraPosition = CameraPosition.FrontPlate;
                                break;
                            default:
                                camera.MainCameraPosition = CameraPosition.Tank;
                                break;
                        }
                    }
                    else if ((selectedItem == giAirfilter) || (selectedItem == giStruts))
                    {
                        switch (veh.Model.ToString().ToLowerInvariant())
                        {
                            case "zr380":
                                break;
                            case "zr3802":
                                break;
                            case "zr3803":
                                break;
                            case "issi4":
                                break;
                            case "issi5":
                                break;
                            case "issi6":
                                camera.MainCameraPosition = CameraPosition.Boost;
                                break;
                            case "bruiser":
                                break;
                            case "bruiser2":
                                break;
                            case "bruiser3":
                                break;
                            case "cerberus":
                                break;
                            case "cerberus2":
                                break;
                            case "cerberus3":
                                break;
                            case "deathbike":
                                break;
                            case "deathbike2":
                                break;
                            case "deathbike3":
                                break;
                            case "dominator4":
                                break;
                            case "dominator5":
                                break;
                            case "dominator6":
                                break;
                            case "impaler2":
                                break;
                            case "impaler3":
                                break;
                            case "impaler4":
                                break;
                            case "imperator":
                                break;
                            case "imperator2":
                                break;
                            case "imperator3":
                                break;
                            case "monster3":
                                break;
                            case "monster4":
                                break;
                            case "monster5":
                                break;
                            case "slamvan4":
                                break;
                            case "slamvan5":
                                break;
                            case "slamvan6":
                                break;
                            case "brutus":
                                break;
                            case "brutus2":
                                break;
                            case "brutus3":
                                break;
                            case "scarab":
                                break;
                            case "scarab2":
                                break;
                            case "scarab3":
                                HoodCamera(false);
                                break;
                            default:
                                HoodCamera(true);
                                break;
                        }
                    }
                    else if (selectedItem == giNumberPlate)
                    {
                        if (veh.HasBone("platelight"))
                        {
                            if (veh.ClassType == VehicleClass.Motorcycles || veh.Model.ToString().Equals("blazer4", StringComparison.OrdinalIgnoreCase))
                            {
                                camera.MainCameraPosition = CameraPosition.Car;
                            }
                            else
                            {
                                camera.MainCameraPosition = CameraPosition.BackPlate;
                            }
                        }
                        else if (veh.HasBone("neon_f"))
                        {
                            switch (veh.Model.ToString().ToLowerInvariant())
                            {
                                case "stromberg":
                                    break;
                                case "z190":
                                    break;
                                case "comet4":
                                    break;
                                case "autarch":
                                    camera.MainCameraPosition = CameraPosition.Car;
                                    break;
                                default:
                                    camera.MainCameraPosition = CameraPosition.FrontPlate;
                                    break;
                            }
                        }
                        else
                        {
                            camera.MainCameraPosition = CameraPosition.Car;
                        }
                    }
                }
                else if (sender == QuitMenu)
                {
                    HideAllMenus();
                    PlayExitCutScene();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void ModsMenuCloseHandler(UIMenu sender)
        {
            try
            {
                // Performance Mods
                veh.SetMod(VehicleMod.Suspension, lastVehMemory.Suspension, false);
                veh.SetMod(VehicleMod.Armor, lastVehMemory.Armor, false);
                veh.SetMod(VehicleMod.Brakes, lastVehMemory.Brakes, false);
                veh.SetMod(VehicleMod.Transmission, lastVehMemory.Transmission, false);
                veh.SetMod(VehicleMod.Engine, lastVehMemory.Engine, false);
                if (IsNitroModInstalled()) { veh.SetInt(nitroMod, lastVehMemory.Nitro); }

                // Mods
                veh.SetMod(VehicleMod.FrontBumper, lastVehMemory.FrontBumper, false);
                veh.SetMod(VehicleMod.RearBumper, lastVehMemory.RearBumper, false);
                veh.SetMod(VehicleMod.SideSkirt, lastVehMemory.SideSkirt, false);
                veh.Mods.LicensePlateStyle = lastVehMemory.NumberPlate;
                veh.SetWheelType(lastVehMemory.WheelType);
                veh.SetMod(VehicleMod.FrontWheel, lastVehMemory.FrontWheels, lastVehMemory.WheelsVariation);
                veh.SetMod(VehicleMod.RearWheel, lastVehMemory.BackWheels, lastVehMemory.WheelsVariation);
                veh.ToggleMod(VehicleToggleMod.XenonHeadlights, lastVehMemory.Headlights);
                veh.SetXenonHeadlightsColor(lastVehMemory.HeadlightsColor, veh.IsToggleModOn(VehicleToggleMod.XenonHeadlights));
                veh.SetNeonLightsOn(VehicleNeonLight.Back, lastVehMemory.BackNeon);
                veh.SetNeonLightsOn(VehicleNeonLight.Front, lastVehMemory.FrontNeon);
                veh.SetNeonLightsOn(VehicleNeonLight.Left, lastVehMemory.LeftNeon);
                veh.SetNeonLightsOn(VehicleNeonLight.Right, lastVehMemory.RightNeon);
                veh.SetMod(VehicleMod.ArchCover, lastVehMemory.ArchCover, false);
                veh.SetMod(VehicleMod.Exhaust, lastVehMemory.Exhaust, false);
                veh.SetMod(VehicleMod.Fender, lastVehMemory.Fender, false);
                veh.SetMod(VehicleMod.RightFender, lastVehMemory.RightFender, false);
                veh.SetMod(VehicleMod.DoorSpeakers, lastVehMemory.DoorSpeakers, false);
                veh.SetMod(VehicleMod.Frame, lastVehMemory.Frame, false);
                veh.SetMod(VehicleMod.Grille, lastVehMemory.Grille, false);
                veh.SetMod(VehicleMod.Hood, lastVehMemory.Hood, false);
                veh.SetMod(VehicleMod.Horns, lastVehMemory.Horns, false);
                veh.SetMod(VehicleMod.Hydraulics, lastVehMemory.Hydraulics, false);
                veh.SetMod(VehicleMod.Livery, lastVehMemory.Livery, false);
                veh.SetLivery2(lastVehMemory.Livery2);
                veh.SetMod(VehicleMod.Plaques, lastVehMemory.Plaques, false);
                veh.SetMod(VehicleMod.Roof, lastVehMemory.Roof, false);
                veh.SetMod(VehicleMod.Speakers, lastVehMemory.Speakers, false);
                veh.SetMod(VehicleMod.Spoilers, lastVehMemory.Spoilers, false);
                veh.SetMod(VehicleMod.Aerials, lastVehMemory.Aerials, false);
                veh.SetMod(VehicleMod.Trim, lastVehMemory.Trim, false);
                veh.SetMod(VehicleMod.EngineBlock, lastVehMemory.EngineBlock, false);
                veh.SetMod(VehicleMod.AirFilter, lastVehMemory.AirFilter, false);
                veh.SetMod(VehicleMod.Struts, lastVehMemory.Struts, false);
                veh.SetMod(VehicleMod.ColumnShifterLevers, lastVehMemory.ColumnShifterLevers, false);
                veh.SetMod(VehicleMod.Dashboard, lastVehMemory.Dashboard, false);
                veh.SetMod(VehicleMod.DialDesign, lastVehMemory.DialDesign, false);
                veh.SetMod(VehicleMod.Ornaments, lastVehMemory.Ornaments, false);
                veh.SetMod(VehicleMod.Seats, lastVehMemory.Seats, false);
                veh.SetMod(VehicleMod.SteeringWheels, lastVehMemory.SteeringWheels, false);
                veh.SetMod(VehicleMod.TrimDesign, lastVehMemory.TrimDesign, false);
                veh.SetMod(VehicleMod.PlateHolder, lastVehMemory.PlateHolder, false);
                veh.SetMod(VehicleMod.VanityPlates, lastVehMemory.VanityPlates, false);
                veh.SetMod(VehicleMod.Tank, lastVehMemory.Tank, false);
                veh.SetMod(VehicleMod.Trunk, lastVehMemory.Trunk, false);
                veh.SetMod(VehicleMod.Windows, lastVehMemory.Windows, false);
                veh.ToggleMod(VehicleToggleMod.Turbo, lastVehMemory.Turbo);
                veh.Mods.WindowTint = lastVehMemory.Tint;
                veh.CanTiresBurst = lastVehMemory.BulletProofTires;

                // Color()
                veh.Mods.DashboardColor = lastVehMemory.LightsColor;
                veh.Mods.TrimColor = lastVehMemory.TrimColor;
                veh.Mods.PrimaryColor = lastVehMemory.PrimaryColor;
                veh.Mods.SecondaryColor = lastVehMemory.SecondaryColor;
                veh.Mods.PearlescentColor = lastVehMemory.PearlescentColor;
                veh.Mods.RimColor = lastVehMemory.RimColor;
                veh.Mods.NeonLightsColor = lastVehMemory.NeonLightsColor;
                veh.Mods.TireSmokeColor = lastVehMemory.TireSmokeColor;

                // Close Doors
                if (sender == gmEngine)
                {
                    Function.Call(Hash.SET_VEHICLE_DOORS_SHUT, veh, false);
                }
                if (sender == mStruts)
                {
                    switch (veh.Model.ToString().ToLowerInvariant())
                    {
                        case "comet3":
                            veh.CloseDoor(VehicleDoorIndex.Trunk, false);
                            camera.MainCameraPosition = CameraPosition.RearBumper;
                            break;
                    }
                }
                if (sender == mDoor)
                {
                    Function.Call(Hash.SET_VEHICLE_DOORS_SHUT, veh, false);
                }
                if (sender == mHydraulics) { veh.CloseDoor(VehicleDoorIndex.Trunk, false); }
                if (sender == mTrunk) { veh.CloseDoor(VehicleDoorIndex.Trunk, false); }
                if (sender == gmLights)
                {
                    veh.SetLightsState(true);
                    veh.SetHighBeamsState(false);
                }
                if (sender == mHorn) { ply.Task.WarpIntoVehicle(veh, VehicleSeat.Driver); }

                // Reset Camera Position
                if ((sender == gmInterior) || (sender == gmEngine) || (sender == mFBumper) || (sender == mRBumper) || (sender == mSSkirt) || (sender == mNumberPlate) || (sender == mPlateHolder) || (sender == mSpoilers) || (sender == mVanityPlates) || (sender == gmWheels) || (sender == mExhaust) || (sender == mBrakes) || (sender == mGrille) || (sender == mHood) || (sender == mHydraulics) || (sender == mPlaques) || (sender == mTank) || (sender == mShifter) || (sender == mFMudguard) || (sender == mOilTank) || (sender == mRMudguard) || (sender == mFuelTank) || (sender == mBeltDriveCovers) || (sender == mBTank) || (sender == mTrunk) || (sender == mArchCover) || (sender == mRoof))
                {
                    camera.MainCameraPosition = CameraPosition.Car;
                }
                if (sender.ParentMenu != gmEngine)
                {
                    if ((sender == mStruts) || (sender == mAirFilter))
                    {
                        camera.MainCameraPosition = CameraPosition.Car;
                    }
                }
                if (sender.ParentMenu != gmInterior)
                {
                    if (sender == mOrnaments)
                    {
                        camera.MainCameraPosition = CameraPosition.Car;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        #endregion

        public static UIMenu NewUIMenu(ref UIMenu menu, string gxt, bool gxtUpper, bool showStats, UIMenu.MenuCloseEvent closeHandler = null, UIMenu.ItemSelectEvent selectHandler = null, UIMenu.IndexChangedEvent indexChangeHandler = null, string itemName = "null", string itemDesc = null)
        {
            string title = Game.GetLocalizedString(gxt) ?? string.Empty;
            if (gxtUpper)
            {
                title = title.ToUpper();
            }
            return NewUIMenu(ref menu, title, showStats, closeHandler, selectHandler, indexChangeHandler, itemName, itemDesc);
        }

        public static UIMenu NewUIMenu(ref UIMenu menu, string title, bool showStats, UIMenu.MenuCloseEvent closeHandler = null, UIMenu.ItemSelectEvent selectHandler = null, UIMenu.IndexChangedEvent indexChangeHandler = null, string itemName = "null", string itemDesc = null)
        {
            try
            {
                menu = new UIMenu(string.Empty, title ?? string.Empty, showStats);
                menu.MouseEdgeEnabled = false;

                // NOTE: Mouse button behaviour is handled by the compatibility layer.
                // Removing the explicit assignments below avoids build errors.
                // The menu will use default LemonUI behaviour: left-click selects, right-click goes back.

                if (!string.IsNullOrWhiteSpace(itemName) && !string.Equals(itemName, "null", StringComparison.OrdinalIgnoreCase))
                {
                    menu.AddItem(new UIMenuItem(itemName, itemDesc));
                    menu.RefreshIndex();
                }

                if (_menuPool != null)
                    _menuPool.Add(menu.NativeMenu);
                else
                    Logger.Log($"NewUIMenu: menu pool was null while creating '{title ?? string.Empty}'.");

                if (closeHandler != null) menu.OnMenuClose += closeHandler;
                if (selectHandler != null) menu.OnItemSelect += selectHandler;
                if (indexChangeHandler != null) menu.OnIndexChange += indexChangeHandler;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }

            return menu;
        }

        public static int GetInt(this Vehicle vehicle, string decorName)
        {
            try
            {
                return vehicle != null ? Function.Call<int>((Hash)0xA06C969B02A97298UL, vehicle.Handle, decorName) : 0;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
                return 0;
            }
        }

        public static VehicleWheelType GetWheelType(this Vehicle vehicle)
        {
            try
            {
                return vehicle != null ? Function.Call<VehicleWheelType>(Hash.GET_VEHICLE_WHEEL_TYPE, vehicle.Handle) : VehicleWheelType.HighEnd;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
                return VehicleWheelType.HighEnd;
            }
        }

        public static void SetWheelType(this Vehicle vehicle, VehicleWheelType wheelType)
        {
            try
            {
                if (vehicle != null)
                {
                    Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, vehicle.Handle, (int)wheelType);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void SetNeonLightsOn(this Vehicle vehicle, VehicleNeonLight light, bool enabled)
        {
            try
            {
                if (vehicle != null)
                {
                    Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, vehicle.Handle, (int)light, enabled);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void SetLightsState(this Vehicle vehicle, bool enabled)
        {
            try
            {
                if (vehicle != null)
                {
                    Function.Call(Hash.SET_VEHICLE_LIGHTS, vehicle.Handle, enabled ? 3 : 0);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void SetHighBeamsState(this Vehicle vehicle, bool enabled)
        {
            try
            {
                if (vehicle != null)
                {
                    Function.Call(Hash.SET_VEHICLE_FULLBEAM, vehicle.Handle, enabled);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void SetMod(this Vehicle vehicle, VehicleMod modType, int modIndex, bool variation)
        {
            try
            {
                if (vehicle != null)
                {
                    Function.Call(Hash.SET_VEHICLE_MOD, vehicle, (int)modType, modIndex, variation);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }

        public static void SetInt(this Vehicle vehicle, string decorName, int value)
        {
            try
            {
                if (vehicle != null)
                {
                    Function.Call((Hash)0x0CE3AA5E1CA19E10UL, vehicle.Handle, decorName, value);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
