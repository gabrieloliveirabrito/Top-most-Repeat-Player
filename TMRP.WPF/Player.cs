﻿using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TMRP.WPF
{
    public partial class Player : ANotifyObject, IDisposable
    {
        public Player()
        {
            configuration = new Configuration();
            InitializeMethods();

            //configuration.PropertyChanged += (s, e) => NotifyChanged(e.PropertyName);
        }

        ~Player()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (MediaPlayer != null)
            {
                MediaPlayer.Dispose();
                VLC.Dispose();
            }
        }

        public void CreatePlayer()
        {
            MediaPlayer = new MediaPlayer(VLC);
            MediaPlayer.EnableHardwareDecoding = true;
            MediaPlayer.EnableKeyInput = false;
            MediaPlayer.EnableMouseInput = false;
            MediaPlayer.SetVideoTitleDisplay(Position.Bottom, 3000);
            MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
            MediaPlayer.EndReached += MediaPlayer_EndReached;
            MediaPlayer.MediaChanged += MediaPlayer_MediaChanged;

            if (Time > 0 && configuration.LastFile != null)
            {
                MediaPlayer.Media = new Media(VLC, configuration.LastFile, options: ":input-repeat");
                MediaPlayer.Position = Time;
                MediaPlayer.SetPause(Paused);
            }

            NotifyChanged("Volume");
        }

        private void MediaPlayer_MediaChanged(object sender, MediaPlayerMediaChangedEventArgs e)
        {
            if (configuration.Volume > 0)
                MediaPlayer.Volume = configuration.Volume;
        }

        public void Play(string filename)
        {
            configuration.LastFile = filename;
            TogglePlayPauseCommand = new RelayCommand<Player>(_ => TogglePause());

            MediaPlayer.Media = new Media(VLC, configuration.LastFile, options: ":input-repeat");
            MediaPlayer.Play();
            Paused = false;
        }

        public void Play()
        {
            Paused = false;
        }

        public void Pause()
        {
            Paused = true;
        }

        public void TogglePause()
        {
            Paused = !Paused;
        }

        private void MediaPlayer_EndReached(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((state) => this.MediaPlayer.Stop());
            ThreadPool.QueueUserWorkItem((state) => this.MediaPlayer.Play());
        }

        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            Set(ref time, e.Time, "Time");
            Length = MediaPlayer.Length;
        }
    }
}