using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.StartScreen;

namespace TMRP.WPF
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        string lockFile = "";

        public App()
        {
            lockFile = Path.Combine(Environment.CurrentDirectory, "lockfile");
        }

        private bool TryGetProcessById(int pid, out Process proc)
        {
            try
            {
                proc = Process.GetProcessById(pid);
                return true;
            }
            catch
            {
                proc = null;
                return false;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var proc = Process.GetCurrentProcess();

            if(File.Exists(lockFile))
            {
                if(int.TryParse(File.ReadAllText(lockFile), out int pid) && TryGetProcessById(pid, out Process process))
                {
                    if(process != null && process.ProcessName == proc.ProcessName)
                    {
                        var args = Environment.GetCommandLineArgs();
                        var cmd = args.Length > 1 ? args[0] : "";

                        switch(cmd)
                        {
                            case "/pp":
                                User32.SendMessage(process.MainWindowHandle, User32.PLAY_PAUSE_COMMAND, IntPtr.Zero, IntPtr.Zero);
                                break;
                            default:
                                if (!string.IsNullOrEmpty(cmd) && File.Exists(cmd))
                                    User32.SendMessage(process.MainWindowHandle, User32.OPEN_FILE, Marshal.StringToBSTR(cmd), IntPtr.Zero);
                                else
                                    MessageBox.Show("O arquivo não foi encontrado!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                        }

                        Shutdown(-1);
                        return;
                    }
                }

                File.Delete(lockFile);
            }

            File.WriteAllText(lockFile, Convert.ToString(proc.Id));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (e.ApplicationExitCode != -1)
                File.Delete(lockFile);
        }
    }
}
