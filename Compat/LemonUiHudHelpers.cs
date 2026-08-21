using System.Drawing;
using GTA;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Tools;
using Font = GTA.UI.Font;

namespace BennysOriginalMotorworks
{
    internal static class LemonUiHudHelpers
    {
        public static void DrawVehicleTitle(string vehicleName, string vehicleClass, Language language)
        {
            PointF titlePos = SafeZone.GetPositionAt(new PointF(0.95f, 0.82f), Alignment.Right, GFXAlignment.Right);
            PointF classPos = SafeZone.GetPositionAt(new PointF(0.95f, 0.87f), Alignment.Right, GFXAlignment.Right);

            Font titleFont =
                language == Language.Chinese ||
                language == Language.Japanese ||
                language == Language.Korean ||
                language == Language.ChineseSimplified
                    ? Font.ChaletLondon
                    : Font.ChaletComprimeCologne;

            var title = new ScaledText(titlePos, vehicleName, 0.85f, titleFont)
            {
                Alignment = Alignment.Right,
                Color = Color.White,
                Shadow = true
            };

            var subtitle = new ScaledText(classPos, vehicleClass, 0.85f, Font.HouseScript)
            {
                Alignment = Alignment.Right,
                Color = Color.DodgerBlue,
                Shadow = true
            };

            title.Draw();
            subtitle.Draw();
        }
    }
}
