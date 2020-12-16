using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TMRP
{
    public class Player
    {
        private Configuration configuration;

        public LibVLC VLC { get; private set; }
        public MediaPlayer MediaPlayer { get; private set; }
        public int Volume
        {
            get => MediaPlayer.Volume;
            set
            {
                MediaPlayer.Volume = value;
                configuration.Volume = value;
            }
        }

        public Player(Configuration configuration)
        {
            this.configuration = configuration;

            Core.Initialize();
            VLC = new LibVLC("--input-repeat=2");

            MediaPlayer = new MediaPlayer(VLC);
            MediaPlayer.EnableHardwareDecoding = true;
            MediaPlayer.EnableKeyInput = true;
            MediaPlayer.EnableMouseInput = true;
            MediaPlayer.SetVideoTitleDisplay(Position.Bottom, 3000);
            MediaPlayer.EndReached += MediaPlayer_EndReached;

            if (configuration.Volume > 0)
                MediaPlayer.Volume = configuration.Volume;
        }

        public void Play(string filename)
        {
            MediaPlayer.Media = new Media(VLC, filename);
            MediaPlayer.Media.AddOption(":input-repeat");
            MediaPlayer.Play();
        }

        public void Pause()
        {
            MediaPlayer.Pause();
        }

        public void TogglePause()
        {
            if (MediaPlayer.IsPlaying)
                MediaPlayer.Pause();
            else
                MediaPlayer.Play();
        }

        private void MediaPlayer_EndReached(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((state) => this.MediaPlayer.Stop());
            ThreadPool.QueueUserWorkItem((state) => this.MediaPlayer.Play());
        }
    }
}
