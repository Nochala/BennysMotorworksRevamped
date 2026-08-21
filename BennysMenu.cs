using GTA;
using GTA.Native;
using LemonUI;
using LemonUI.Scaleform;
using LemonUI.Tools;
using System;
using System.Drawing;
using System.Reflection;
using static BennysMotorworksRevamped.Helper;
using static BennysMotorworksRevamped.MenuHelper;

namespace BennysMotorworksRevamped
{
    public class BennysMenu : Script
    {
        private const float LemonUiToShvdnScaledRatio = 720f / 1080f;
        private const float VehicleStatsPanelWidth = 288f;
        private const float VehicleStatsPanelHeight = 100f;
        private const float VehicleStatsPanelPaddingLeft = 14f;
        private const float VehicleStatsPanelPaddingTop = 10f;
        private const float VehicleStatsRowSpacing = 21f;
        private const float VehicleStatsBarOffsetX = 118f;
        private const float VehicleStatsBarYOffset = 8f;
        private const float VehicleStatsBarWidth = 150f;
        private const float VehicleStatsBarHeight = 7f;
        private const int VehicleStatsBarSegments = 5;
        private const float VehicleStatsBarSegmentGap = 4f;
        private const float VehicleStatsBarMaxValue = 200f;
        private const float MenuAreaFallbackWidth = 431f;
        private const float MenuAreaFallbackHeight = 550f;
        private const float MenuAreaEstimatedBaseHeight = 170f;
        private const float MenuAreaEstimatedRowHeight = 38f;
        private const int MenuAreaEstimatedMaxVisibleRows = 10;
        private const float MenuAreaEstimatedFooterBaseHeight = 46f;
        private const float MenuAreaEstimatedFooterLineHeight = 18f;
        private const int MenuAreaEstimatedFooterWrapCharacters = 38;

        public bool IsScriptLoaded { get; private set; }

        public BennysMenu()
        {
            Tick += OnTick;
            Aborted += OnAborted;

            MenuHelper._menuPool = new ObjectPool();
            Helper._menuPool = MenuHelper._menuPool;
            Logger.Initialize();
            Logger.Log("BennysMenu initialized.");
            camera = new WorkshopCamera();
            BtnZoom = new LemonUI.Scaleform.InstructionalButton(Game.GetLocalizedString("INPUT_CREATOR_ZOOM_IN_DISPLAYONLY"), zinKey);
            BtnZoomOut = new LemonUI.Scaleform.InstructionalButton(Game.GetLocalizedString("INPUT_CREATOR_ZOOM_OUT_DISPLAYONLY"), zoutKey);

            Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "VEHICLE_SHOP_HUD_1", false, -1);
            Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "VEHICLE_SHOP_HUD_2", false, -1);
        }

        private static object GetReflectedMemberValue(object instance, string memberName)
        {
            if (instance == null || string.IsNullOrWhiteSpace(memberName))
            {
                return null;
            }

            Type type = instance.GetType();

            try
            {
                PropertyInfo property = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (property != null && property.GetIndexParameters().Length == 0)
                {
                    return property.GetValue(instance, null);
                }
            }
            catch
            {
            }

            try
            {
                FieldInfo field = type.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    return field.GetValue(instance);
                }
            }
            catch
            {
            }

            return null;
        }

        private static bool TryConvertToFloat(object value, out float result)
        {
            switch (value)
            {
                case float floatValue:
                    result = floatValue;
                    return true;
                case double doubleValue:
                    result = (float)doubleValue;
                    return true;
                case decimal decimalValue:
                    result = (float)decimalValue;
                    return true;
                case int intValue:
                    result = intValue;
                    return true;
                case long longValue:
                    result = longValue;
                    return true;
                case short shortValue:
                    result = shortValue;
                    return true;
                case byte byteValue:
                    result = byteValue;
                    return true;
                default:
                    result = 0f;
                    return false;
            }
        }

        private static bool TryExtractPointF(object value, out PointF point)
        {
            if (value is PointF pointF)
            {
                point = pointF;
                return true;
            }

            if (value is Point pointInt)
            {
                point = new PointF(pointInt.X, pointInt.Y);
                return true;
            }

            if (TryConvertToFloat(GetReflectedMemberValue(value, "X"), out float x)
                && TryConvertToFloat(GetReflectedMemberValue(value, "Y"), out float y))
            {
                point = new PointF(x, y);
                return true;
            }

            point = PointF.Empty;
            return false;
        }

        private static bool TryExtractSizeF(object value, out SizeF size)
        {
            if (value is SizeF sizeF)
            {
                size = sizeF;
                return true;
            }

            if (value is Size sizeInt)
            {
                size = new SizeF(sizeInt.Width, sizeInt.Height);
                return true;
            }

            if (value is RectangleF rectangleF)
            {
                size = rectangleF.Size;
                return true;
            }

            if (value is Rectangle rectangle)
            {
                size = rectangle.Size;
                return true;
            }

            if (TryConvertToFloat(GetReflectedMemberValue(value, "Width"), out float width)
                && TryConvertToFloat(GetReflectedMemberValue(value, "Height"), out float height))
            {
                size = new SizeF(width, height);
                return true;
            }

            size = SizeF.Empty;
            return false;
        }

        private static PointF ConvertLemonUiPointToShvdnScaled(PointF value)
        {
            return new PointF(
                value.X * LemonUiToShvdnScaledRatio,
                value.Y * LemonUiToShvdnScaledRatio
            );
        }

        private static SizeF ConvertLemonUiSizeToShvdnScaled(SizeF value)
        {
            return new SizeF(
                value.Width * LemonUiToShvdnScaledRatio,
                value.Height * LemonUiToShvdnScaledRatio
            );
        }

        private static float ConvertScaledDrawXToNormalized(float scaledX)
        {
            float scaledWidth = (float)GTA.UI.Screen.ScaledWidth;
            if (scaledWidth <= 0f)
            {
                return 0f;
            }

            return scaledX / scaledWidth;
        }

        private static float ConvertScaledDrawYToNormalized(float scaledY)
        {
            return scaledY / 720f;
        }

        private static PointF GetVisibleMenuPositionLemonUi(BennysMotorworksRevamped.Compat.UIMenu visibleMenu)
        {
            object visibleNativeMenu = visibleMenu?.NativeMenu;
            if (TryExtractPointF(GetReflectedMemberValue(visibleNativeMenu, "Position"), out PointF menuPosition))
            {
                return menuPosition;
            }

            object banner = GetReflectedMemberValue(visibleNativeMenu, "Banner");
            if (TryExtractPointF(GetReflectedMemberValue(banner, "Position"), out PointF bannerPosition))
            {
                return bannerPosition;
            }

            return SafeZone.GetSafePosition(new PointF(0f, 0f));
        }

        private static float EstimateVisibleMenuFooterHeightLemonUi(BennysMotorworksRevamped.Compat.UIMenu visibleMenu)
        {
            object visibleNativeMenu = visibleMenu?.NativeMenu;
            string[] footerHeightMembers = { "DescriptionHeight", "FooterHeight", "HelpHeight", "InfoHeight" };

            foreach (string memberName in footerHeightMembers)
            {
                if (TryConvertToFloat(GetReflectedMemberValue(visibleNativeMenu, memberName), out float footerHeight) && footerHeight > 0f)
                {
                    return footerHeight;
                }
            }

            if (visibleMenu == null)
            {
                return 0f;
            }

            int selectedIndex = visibleMenu.NativeMenu.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= visibleMenu.MenuItems.Count)
            {
                return 0f;
            }

            string description = visibleMenu.MenuItems[selectedIndex].Description;
            if (string.IsNullOrWhiteSpace(description))
            {
                return 0f;
            }

            string[] explicitLines = description.Replace("\r", string.Empty).Trim().Split(new[] { '\n' }, StringSplitOptions.None);
            int wrappedLineCount = 0;
            foreach (string explicitLine in explicitLines)
            {
                string line = explicitLine?.Trim() ?? string.Empty;
                wrappedLineCount += Math.Max(1, (int)Math.Ceiling(line.Length / (double)MenuAreaEstimatedFooterWrapCharacters));
            }

            wrappedLineCount = Math.Max(1, wrappedLineCount);
            return MenuAreaEstimatedFooterBaseHeight + ((wrappedLineCount - 1) * MenuAreaEstimatedFooterLineHeight);
        }

        private static SizeF EstimateVisibleMenuSizeLemonUi(BennysMotorworksRevamped.Compat.UIMenu visibleMenu)
        {
            object visibleNativeMenu = visibleMenu?.NativeMenu;
            string[] sizeMembers = { "Size", "MenuSize", "VisibleSize", "DrawSize", "Bounds", "Rectangle" };

            foreach (string memberName in sizeMembers)
            {
                if (TryExtractSizeF(GetReflectedMemberValue(visibleNativeMenu, memberName), out SizeF reflectedSize)
                    && reflectedSize.Width > 0f
                    && reflectedSize.Height > 0f)
                {
                    return reflectedSize;
                }
            }

            bool hasWidth = TryConvertToFloat(GetReflectedMemberValue(visibleNativeMenu, "Width"), out float reflectedWidth) && reflectedWidth > 0f;
            bool hasHeight = TryConvertToFloat(GetReflectedMemberValue(visibleNativeMenu, "Height"), out float reflectedHeight) && reflectedHeight > 0f;
            if (hasWidth && hasHeight)
            {
                return new SizeF(reflectedWidth, reflectedHeight);
            }

            int visibleRowCount = visibleMenu == null ? 1 : visibleMenu.MenuItems.Count;
            visibleRowCount = Math.Max(1, Math.Min(visibleRowCount, MenuAreaEstimatedMaxVisibleRows));

            float width = hasWidth ? reflectedWidth : MenuAreaFallbackWidth;
            float height = hasHeight ? reflectedHeight : MenuAreaEstimatedBaseHeight + (visibleRowCount * MenuAreaEstimatedRowHeight);
            height += EstimateVisibleMenuFooterHeightLemonUi(visibleMenu);
            return new SizeF(width, height);
        }

        private static void GetMenuAreaDrawBounds(out PointF position, out SizeF size)
        {
            BennysMotorworksRevamped.Compat.UIMenu visibleMenu = BennysMotorworksRevamped.Compat.UIMenu.GetVisibleMenu();
            PointF menuPosition = GetVisibleMenuPositionLemonUi(visibleMenu);
            SizeF menuSize = EstimateVisibleMenuSizeLemonUi(visibleMenu);

            position = ConvertLemonUiPointToShvdnScaled(menuPosition);
            size = menuSize.Width > 0f && menuSize.Height > 0f
                ? ConvertLemonUiSizeToShvdnScaled(menuSize)
                : ConvertLemonUiSizeToShvdnScaled(new SizeF(MenuAreaFallbackWidth, MenuAreaFallbackHeight));
        }

        private static bool ShouldDrawVehicleStatsPanel()
        {
            BennysMotorworksRevamped.Compat.UIMenu visibleMenu = BennysMotorworksRevamped.Compat.UIMenu.GetVisibleMenu();
            return visibleMenu != null && visibleMenu.ShowStats && veh != null && veh.Exists();
        }

        private static float ConvertScaledWidthToNormalized(float scaledWidth)
        {
            float scaledScreenWidth = (float)GTA.UI.Screen.ScaledWidth;
            if (scaledScreenWidth <= 0f)
            {
                return 0f;
            }

            return scaledWidth / scaledScreenWidth;
        }

        private static float ConvertScaledHeightToNormalized(float scaledHeight)
        {
            return scaledHeight / 720f;
        }

        private static float Clamp01(float value)
        {
            if (value <= 0f)
            {
                return 0f;
            }

            if (value >= 1f)
            {
                return 1f;
            }

            return value;
        }

        private static void DrawRectNormalized(float centerX, float centerY, float width, float height, Color color)
        {
            if (width <= 0f || height <= 0f)
            {
                return;
            }

            Function.Call(Hash.DRAW_RECT, centerX, centerY, width, height, color.R, color.G, color.B, color.A, false);
        }

        private static void DrawTextNormalized(string value, float x, float y, float scale, GTA.UI.Font font, Color color)
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
            Function.Call(Hash.SET_TEXT_JUSTIFICATION, 1);
            Function.Call(Hash.SET_TEXT_WRAP, x, 1.0f);
            Function.Call(Hash.SET_TEXT_RIGHT_JUSTIFY, false);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, value);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0);
        }

        private static void DrawVehicleStatBarNativeStyle(float value, float barLeftScaled, float barTopScaled)
        {
            float clampedRatio = Clamp01(value / VehicleStatsBarMaxValue);
            float segmentWidthScaled = (VehicleStatsBarWidth - (VehicleStatsBarSegmentGap * (VehicleStatsBarSegments - 1))) / VehicleStatsBarSegments;
            float activeSegments = clampedRatio * VehicleStatsBarSegments;
            float barCenterY = ConvertScaledDrawYToNormalized(barTopScaled + (VehicleStatsBarHeight * 0.5f));
            float segmentHeightNormalized = ConvertScaledHeightToNormalized(VehicleStatsBarHeight);

            for (int i = 0; i < VehicleStatsBarSegments; i++)
            {
                float segmentLeftScaled = barLeftScaled + (i * (segmentWidthScaled + VehicleStatsBarSegmentGap));
                float segmentCenterX = ConvertScaledDrawXToNormalized(segmentLeftScaled + (segmentWidthScaled * 0.5f));
                float segmentWidthNormalized = ConvertScaledWidthToNormalized(segmentWidthScaled);
                Color baseColor = Color.FromArgb(115, 55, 55, 55);
                Color fillColor = Color.FromArgb(235, 255, 255, 255);

                DrawRectNormalized(segmentCenterX, barCenterY, segmentWidthNormalized, segmentHeightNormalized, baseColor);

                float segmentFill = Clamp01(activeSegments - i);
                if (segmentFill > 0f)
                {
                    float fillWidthScaled = segmentWidthScaled * segmentFill;
                    float fillCenterX = ConvertScaledDrawXToNormalized(segmentLeftScaled + (fillWidthScaled * 0.5f));
                    float fillWidthNormalized = ConvertScaledWidthToNormalized(fillWidthScaled);
                    DrawRectNormalized(fillCenterX, barCenterY, fillWidthNormalized, segmentHeightNormalized, fillColor);
                }
            }
        }

        private static void DrawVehicleStatRow(string label, float value, float rowTopScaled, float panelLeftScaled)
        {
            float labelX = ConvertScaledDrawXToNormalized(panelLeftScaled + VehicleStatsPanelPaddingLeft);
            float textY = ConvertScaledDrawYToNormalized(rowTopScaled);
            float barLeftScaled = panelLeftScaled + VehicleStatsBarOffsetX;
            float barTopScaled = rowTopScaled + VehicleStatsBarYOffset;

            DrawTextNormalized(label, labelX, textY, 0.285f, GTA.UI.Font.ChaletLondon, Color.WhiteSmoke);
            DrawVehicleStatBarNativeStyle(value, barLeftScaled, barTopScaled);
        }

        private static void DrawVehicleStatsPanel()
        {
            if (!ShouldDrawVehicleStatsPanel())
            {
                return;
            }

            GetMenuAreaDrawBounds(out PointF menuPosition, out SizeF menuSize);

            float panelLeft = menuPosition.X + vehicleStatsOffsetX;
            float panelTop = menuPosition.Y + menuSize.Height + vehicleStatsOffsetY;
            float panelWidth = VehicleStatsPanelWidth;
            float panelHeight = VehicleStatsPanelHeight;

            float panelCenterX = ConvertScaledDrawXToNormalized(panelLeft + (panelWidth * 0.5f));
            float panelCenterY = ConvertScaledDrawYToNormalized(panelTop + (panelHeight * 0.5f));
            DrawRectNormalized(
                panelCenterX,
                panelCenterY,
                ConvertScaledWidthToNormalized(panelWidth),
                ConvertScaledHeightToNormalized(panelHeight),
                Color.FromArgb(120, 0, 0, 0));

            float firstRowTop = panelTop + VehicleStatsPanelPaddingTop;
            DrawVehicleStatRow("Top Speed", vehStats.TopSpeed, firstRowTop, panelLeft);
            DrawVehicleStatRow("Acceleration", vehStats.Acceleration, firstRowTop + VehicleStatsRowSpacing, panelLeft);
            DrawVehicleStatRow("Braking", vehStats.Braking, firstRowTop + (VehicleStatsRowSpacing * 2f), panelLeft);
            DrawVehicleStatRow("Traction", vehStats.Traction, firstRowTop + (VehicleStatsRowSpacing * 3f), panelLeft);
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
                if (optEnableMouse && MenuHelper._menuPool != null && MenuHelper._menuPool.AreAnyVisible)
                {
                    EnableWorkshopMenuMouseControls();
                }
                MenuHelper.RefreshMenuMouseBehavior();
                MenuHelper._menuPool?.Process();
                BennysMotorworksRevamped.Compat.UIMenu.EnsureSingleVisibleMenu();

                bool isMenuVisible = MenuHelper._menuPool != null && MenuHelper._menuPool.AreAnyVisible;
                SetWorkshopPlayerControlSuppressed(isMenuVisible && !optEnableMouse);
                if (isMenuVisible && optEnableMouse)
                {
                    EnableWorkshopMenuMouseControls();
                }

                if (veh != null)
                {
                    vehStats = GetVehicleStats(veh);
                }

                if (isMenuVisible)
                {
                    Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                    DrawVehicleStatsPanel();
                }

                if (isCutscene && !isMenuVisible && veh != null)
                {
                    Helper.DisplayVehicleInfoBottomRight(
                        Helper.GetVehicleMakeAndModelDisplayName(veh),
                        GetClassDisplayName(veh.ClassType));
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

        private void OnAborted(object sender, EventArgs e)
        {
            SetWorkshopPlayerControlSuppressed(false);
        }
    }
}
