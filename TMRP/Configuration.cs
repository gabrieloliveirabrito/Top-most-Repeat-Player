using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMRP
{
    public partial class Configuration
    {
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

                if (posX != null && posY != null)
                    location = new Point((int)posX, (int)posY);

                var sizeW = registry.GetValue("Width");
                var sizeH = registry.GetValue("Height");

                if (sizeW != null && sizeH != null)
                    size = new Size((int)sizeW, (int)sizeH);

                var bordeless = registry.GetValue("Bordeless");
                if (bordeless != null)
                    this.bordeless = (int)bordeless == 1;

                var volume = registry.GetValue("Volume");
                if (volume != null)
                    Volume = (int)volume;
                else
                    Volume = 100;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
