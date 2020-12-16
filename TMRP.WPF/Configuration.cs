using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TMRP.WPF
{
    public partial class Configuration
    {
        public bool Loaded { get; set; }

        public bool Load()
        {
            try
            {
                registry = Registry.CurrentUser.OpenSubKey("SOFTWARE\\TMRP", true);
                if (registry == null)
                    registry = Registry.CurrentUser.CreateSubKey("SOFTWARE\\TMRP", RegistryKeyPermissionCheck.ReadWriteSubTree);

                lastFile = registry.GetValue("LastFile") as string;

                var posX = registry.GetValue("PosX");
                var posY = registry.GetValue("PosY");
                var width = registry.GetValue("Width");
                var height = registry.GetValue("Height");
                var borderless = registry.GetValue("Borderless");
                var topMost = registry.GetValue("TopMost");
                var volume = registry.GetValue("Volume");

                PosX = posX == null ? 0 : Convert.ToInt32(posX);
                PosY = posY == null ? 0 : Convert.ToInt32(posY);
                Width = width == null ? 0 : Convert.ToInt32(width);
                Height = height == null ? 0 : Convert.ToInt32(height);
                TopMost = topMost == null ? false : Convert.ToInt32(topMost) == 1;
                Volume = volume == null ? 100 : Convert.ToInt32(volume);
                Borderless = borderless == null ? false : Convert.ToInt32(borderless) == 1;

                Loaded = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ToggleTopMost()
        {
            TopMost = !TopMost;
        }
    }
}
