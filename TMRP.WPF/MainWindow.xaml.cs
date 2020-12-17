using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace TMRP.WPF
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        Player player;
        public MainWindow()
        {
            InitializeComponent();
            
            player = DataContext as Player;
            DropCanvas.Focus();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch(msg)
            {
                case User32.PLAY_PAUSE_COMMAND:
                    player.TogglePause();
                    break;
                case User32.OPEN_FILE:
                    var filename = Marshal.PtrToStringBSTR(wParam);
                    player.Play(filename);
                    break;
            }

            return IntPtr.Zero;
        }

        private void EventCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Link;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = e.Effects == DragDropEffects.None;
        }

        private void EventCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files.Length > 0)
                {
                    player.Play(files[0]);
                }
            }
        }
    }
}
