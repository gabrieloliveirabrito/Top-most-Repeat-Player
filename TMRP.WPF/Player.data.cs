using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using System;
using System.Windows;
using System.Windows.Input;

namespace TMRP.WPF
{
    public partial class Player
    {
        public LibVLC VLC { get; private set; }

        private bool loaded = false;
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

        private long time = 0;
        public long Time
        {
            get => MediaPlayer == null ? 0 : time;
            set
            {
                Set(ref time, value);
                MediaPlayer.Time = value;
            }
        }

        private long length = 0;
        public long Length
        {
            get => MediaPlayer == null ? 0 : length;
            set => Set(ref length, value);
        }

        private bool paused = false;
        public bool Paused
        {
            get => paused;
            set
            {
                Set(ref paused, value);
                if (MediaPlayer != null)
                    MediaPlayer.SetPause(value);
            }
        }

        private bool controls = false;
        public bool Controls
        {
            get => controls;
            set => Set(ref controls, value);
        }

        private bool fullscreen = false;
        public bool FullScreen
        {
            get => fullscreen;
            set => Set(ref fullscreen, value);
        }
    }
}
