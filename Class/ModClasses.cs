using System;
using System.Drawing;

namespace BennysMotorworksRevamped
{
    public sealed class ArenaWarVehicle
    {
        public ArenaWarVehicle(string model, string image, int price)
        {
            Model = model;
            Image = image;
            Price = price;
        }

        public string Model { get; }
        public string Image { get; }
        public int Price { get; }
    }

    public sealed class ModClass
    {
        public ModClass(int modId, int price)
        {
            ModID = modId;
            Price = price;
        }

        public ModClass(bool modEnabled, int price)
        {
            ModID = Convert.ToInt32(modEnabled);
            Price = price;
        }

        public int ModID { get; set; }
        public int Price { get; set; }

        public bool ModIDBool() => Convert.ToBoolean(ModID);
    }

    public sealed class ToggleModClass
    {
        public ToggleModClass(bool modToggle, int modId, int price)
        {
            ModToggle = modToggle;
            ModID = modId;
            Price = price;
        }

        public bool ModToggle { get; set; }
        public int ModID { get; set; }
        public int Price { get; set; }
    }

    public sealed class RGBModClass
    {
        public RGBModClass(Color color, int price)
        {
            Alpha = color.A;
            Red = color.R;
            Green = color.G;
            Blue = color.B;
            Price = price;
        }

        public RGBModClass(int alpha, int red, int green, int blue, int price)
        {
            Alpha = alpha;
            Red = red;
            Green = green;
            Blue = blue;
            Price = price;
        }

        public RGBModClass(int alpha, Color color, int price)
        {
            Alpha = alpha;
            Red = color.R;
            Green = color.G;
            Blue = color.B;
            Price = price;
        }

        public RGBModClass(int red, int green, int blue, int price)
        {
            Alpha = 255;
            Red = red;
            Green = green;
            Blue = blue;
            Price = price;
        }

        public int Alpha { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public int Price { get; set; }

        public Color Color()
        {
            return System.Drawing.Color.FromArgb(Alpha, Red, Green, Blue);
        }
    }
}
