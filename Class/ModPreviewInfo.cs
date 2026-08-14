using System.Collections.Generic;
using GTA;

namespace BennysMotorworksRevamped
{
    public sealed class ModPreviewInfo
    {
        public ModPreviewInfo(string name)
            : this(name, new List<VehicleDoor>(), CameraPosition.Car)
        {
        }

        public ModPreviewInfo(string name, CameraPosition pos)
            : this(name, new List<VehicleDoor>(), pos)
        {
        }

        public ModPreviewInfo(string name, List<VehicleDoor> doors)
            : this(name, doors, CameraPosition.Car)
        {
        }

        public ModPreviewInfo(string name, List<VehicleDoor> doors, CameraPosition pos)
        {
            Name = name;
            OpenParts = doors;
            CameraPosition = pos;
        }

        public CameraPosition CameraPosition { get; set; }
        public int Cost { get; set; }
        public string Name { get; set; }
        public List<VehicleDoor> OpenParts { get; set; }
    }
}
