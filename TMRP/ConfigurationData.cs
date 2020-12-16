using System;
using System.Drawing;
using Microsoft.Win32;

namespace TMRP
{
    public partial class Configuration
    {
        private RegistryKey registry;

        private string lastFile;
        public string LastFile
        {
            get => lastFile;
            set
            {
                lastFile = value;

                registry.SetValue("LastFile", value);
            }
        }

        private Point location;
        public Point Location
        {
            get => location;
            set
            {
                location = value;

                registry.SetValue("PosX", value.X);
                registry.SetValue("PosY", value.Y);
            }
        }

        private Size size;
        public Size Size
        {
            get => size;
            set
            {
                size = value;

                registry.SetValue("Width", value.Width);
                registry.SetValue("Height", value.Height);
            }
        }

        private bool bordeless;
        public bool Bordeless
        {
            get => bordeless;
            set
            {
                bordeless = value;

                registry.SetValue("Bordeless", value ? 1 : 0);
            }
        }

        private int volume;
        public int Volume
        {
            get => volume;
            set
            {
                volume = value;

                registry.SetValue("Volume", value);
            }
        }
    }
}
