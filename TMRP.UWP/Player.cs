using LibVLCSharp.Platforms.UWP;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TMRP.UWP
{
    public class Player : ANotifyObject, IDisposable
    {
        LibVLC vlc;

        private MediaPlayer player;
        public MediaPlayer MediaPlayer
        {
            get => player;
            set => Set(nameof(MediaPlayer), ref player, value);
        }

        public ICommand InitializedCommand { get; }

        public Player()
        { 
            InitializedCommand = new RelayCommand<InitializedEventArgs>(Initialize);
        }

        ~Player()
        {
            Dispose();
        }

        public void Initialize(InitializedEventArgs e)
        {

        }

        public void Dispose()
        {
            player?.Dispose();
            vlc?.Dispose();
        }

        public void Play(string filename)
        {
            player.Play(new Media(vlc, filename));
        }
    }
}
