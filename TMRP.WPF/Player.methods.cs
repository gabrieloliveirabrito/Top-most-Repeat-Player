using LibVLCSharp.Shared;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TMRP.WPF
{
    public partial class Player
    {
        public RelayCommand<Player> TogglePlayPauseCommand { get; set; }
        public RelayCommand<RoutedEventArgs> OpenFileCommand { get; set; }
        public RelayCommand<RoutedEventArgs> ExitCommand { get; set; }
        public RelayCommand<RoutedEventArgs> InitializeCommand { get; set; }
        public RelayCommand<MouseEventArgs> ShowControlsCommand { get; set; }
        public RelayCommand<MouseEventArgs> HideControlsCommand { get; set; }
        public RelayCommand<KeyEventArgs> KeyDownCommand { get; set; }
        public RelayCommand<MouseEventArgs> DragWindowCommand { get; set; }
        public RelayCommand<MouseWheelEventArgs> VolumeWheelCommand { get; set; }
        public RelayCommand<RoutedEventArgs> IncreaseVolumeCommand { get; set; }
        public RelayCommand<RoutedEventArgs> DecreaseVolumeCommand { get; set; }
        public RelayCommand<RoutedEventArgs> MuteVolumeCommand { get; set; }
        public RelayCommand<RoutedEventArgs> ToggleTopMostCommand { get; set; }
        public RelayCommand<RoutedEventArgs> ToggleFullScreenCommand { get; set; }

        void InitializeMethods()
        {
            TogglePlayPauseCommand = new RelayCommand<Player>(_ => TogglePause(), _ => MediaPlayer != null);
            OpenFileCommand = new RelayCommand<RoutedEventArgs>(OpenFile);
            InitializeCommand = new RelayCommand<RoutedEventArgs>(Initialize);
            ExitCommand = new RelayCommand<RoutedEventArgs>(Exit);
            ShowControlsCommand = new RelayCommand<MouseEventArgs>(_ => Controls = true);
            HideControlsCommand = new RelayCommand<MouseEventArgs>(_ => Controls = false);
            KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyShortcuts);
            DragWindowCommand = new RelayCommand<MouseEventArgs>(DragWindow);
            VolumeWheelCommand = new RelayCommand<MouseWheelEventArgs>(VolumeWheel);
            IncreaseVolumeCommand = new RelayCommand<RoutedEventArgs>(IncreaseVolume);
            DecreaseVolumeCommand = new RelayCommand<RoutedEventArgs>(DecreaseVolume);
            MuteVolumeCommand = new RelayCommand<RoutedEventArgs>(MuteVolume);
            ToggleTopMostCommand = new RelayCommand<RoutedEventArgs>(ToggleTopMost);
            ToggleFullScreenCommand = new RelayCommand<RoutedEventArgs>(ToggleFullScreen);
        }

        private void OpenFile(RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Video Files|*.mp4;*.mkv;*.mpg;*.avi";

            if (dialog.ShowDialog() == true)
                Play(dialog.FileName);
        }

        private void ToggleFullScreen(RoutedEventArgs e)
        {
            FullScreen = !FullScreen;
        }

        private void ToggleTopMost(RoutedEventArgs e)
        {
            configuration.ToggleTopMost();
        }

        private void MuteVolume(RoutedEventArgs e)
        {
            player.ToggleMute();
        }

        private void DecreaseVolume(RoutedEventArgs e)
        {
            Volume -= 5;
        }

        private void IncreaseVolume(RoutedEventArgs e)
        {
            Volume += 5;
        }

        private void VolumeWheel(MouseWheelEventArgs e)
        {
            Volume += 5 * (e.Delta > 0 ? 1 : -1);
        }

        private void DragWindow(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                Application.Current.MainWindow.DragMove();
        }

        private void KeyShortcuts(KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                switch (e.Key)
                {
                    case Key.T:
                        Configuration.ToggleTopMost();
                        break;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Space:
                        TogglePause();
                        break;
                    case Key.Escape:
                        Exit(e);
                        break;
                }
            }
        }

        private void Exit(RoutedEventArgs e)
        {
            Dispose();
            Application.Current.Shutdown();
        }

        async void Initialize(RoutedEventArgs e)
        {
            if (Loaded)
            {
                return;
            }
            else if (!configuration.Load())
            {
                MessageBox.Show("Erro ao carregar o arquivo de configurações! ", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Core.Initialize();

                VLC = new LibVLC("--input-repeat=2", $"--volume={configuration.Volume}");
                CreatePlayer();

                var args = Environment.GetCommandLineArgs();

                if (args.Length > 1 && File.Exists(args[1]))
                    Play(args[1]);
                else if (configuration.LastFile != null)
                    Play(configuration.LastFile);

                Loaded = true;
            }
        }
    }
}
