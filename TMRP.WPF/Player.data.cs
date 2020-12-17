using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;

namespace TMRP.WPF
{
    public partial class Player
    {
        private void InitializeData()
        {
            loaded = false;
            time = 0;
            length = 0;
            paused = false;
            controls = false;
            fullscreen = false;
        }

        public LibVLC VLC { get; private set; }

        public double TimeProgress
        {
            get
            {
                if (!Loaded)
                    return 0;
                else
                    return (double)Time / Length;
            }
        }

        public TaskbarItemProgressState TimeProgressState
        {
            get
            {
                if (!Loaded)
                    return TaskbarItemProgressState.None;
                else if (Paused)
                    return TaskbarItemProgressState.Paused;
                else
                    return TaskbarItemProgressState.Normal;
            }
        }

        private bool loaded;
        public bool Loaded
        {
            get => loaded;
            set => Set(ref loaded, value);
        }

        private MediaPlayer player;
        public MediaPlayer MediaPlayer
        {
            get => player;
            set => Set(ref player, value);
        }

        private Configuration configuration;
        public Configuration Configuration
        {
            get => configuration;
            private set => Set(ref configuration, value);
        }

        public int Volume
        {
            get => Loaded ? MediaPlayer.Volume : 0; 
            set
            {
                if (Loaded)
                {
                    MediaPlayer.Volume = value < 0 ? 0 : value > 200 ? 200 : value;
                    configuration.Volume = MediaPlayer.Volume;

                    NotifyChanged();
                }
            }
        }

        private long time;
        public long Time
        {
            get => MediaPlayer == null ? 0 : time;
            set
            {
                Set(ref time, value);
                MediaPlayer.Time = value;

                NotifyChanged("TimeProgress");
                NotifyChanged("TimeProgressChanged");
            }
        }

        private long length;
        public long Length
        {
            get => MediaPlayer == null ? 0 : length;
            set => Set(ref length, value);
        }

        private bool paused;
        public bool Paused
        {
            get => paused;
            set
            {
                Set(ref paused, value);
                if (MediaPlayer != null)
                    MediaPlayer.SetPause(value);

                NotifyChanged("TimeProgress");
                NotifyChanged("TimeProgressChanged");
            }
        }

        private bool controls;
        public bool Controls
        {
            get => controls;
            set => Set(ref controls, value);
        }

        private bool fullscreen;
        public bool FullScreen
        {
            get => fullscreen;
            set => Set(ref fullscreen, value);
        }
    }
}
