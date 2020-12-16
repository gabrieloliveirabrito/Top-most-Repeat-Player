using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.ComponentModel;
using Microsoft.Win32;
using System.Windows.Shell;

namespace TMRP.WPF
{
    public partial class Configuration : ANotifyObject
    {
        private RegistryKey registry;

        public Configuration()
        {
            PropertyChanged += Configuration_PropertyChanged;
        }

        private void Configuration_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Loaded && registry != null)
            {
                var prop = GetType().GetProperty(e.PropertyName).GetValue(this);
                if (prop.GetType() == typeof(bool))
                    prop = (bool)prop ? 1 : 0;

                registry.SetValue(e.PropertyName, prop);
            }
        }

        private string lastFile;
        public string LastFile
        {
            get => lastFile;
            set => Set(ref lastFile, value);
        }

        private int posX;
        public int PosX
        {
            get => posX;
            set => Set(ref posX, value);
        }

        private int posY;
        public int PosY
        {
            get => posY;
            set => Set(ref posY, value);
        }

        private int width = 800;
        public int Width
        {
            get => width;
            set => Set(ref width, value);
        }

        private int height = 400;
        public int Height
        {
            get => height;
            set => Set(ref height, value);
        }

        private int volume;
        public int Volume
        {
            get => volume;
            set => Set(ref volume, value);
        }

        private bool topMost;
        public bool TopMost
        {
            get => topMost;
            set => Set(ref topMost, value);
        }

        private bool borderless;
        public bool Borderless
        {
            get => borderless;
            set
            {
                Set(ref borderless, value);
            }
        }
    }
}