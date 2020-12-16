using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HarmonyLib;
using System.Reflection;

namespace TMRP
{
    public partial class frmMain : Form
    {
        VideoView video;
        Configuration configuration;
        Player player;

        public frmMain()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            configuration = new Configuration();

            video = new VideoView();
            video.Parent = this;
            video.Dock = DockStyle.Fill;
            video.Margin = new Padding(5);
            video.Name = "vlcView";
            video.KeyDown += Video_KeyUp;
            video.DragEnter += Video_DragEnter;
            video.DragDrop += Video_DragDrop;
            video.MouseWheel += Video_MouseWheel;
            video.MouseDown += Video_MouseDown;
            video.MouseMove += Video_MouseMove;
            video.MouseUp += Video_MouseUp;


            AllowDrop = video.AllowDrop = true;
            MouseWheel += Video_MouseWheel;
            DragEnter += Video_DragEnter;
            DragDrop += Video_DragDrop;
            ResizeEnd += FrmMain_ResizeEnd;
            MouseEnter += FrmMain_MouseEnter;

            Controls.Add(video);
        }

        private bool mouseDown = false;
        private void Video_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseDown = true;
        }

        private void Video_MouseMove(object sender, MouseEventArgs e)
        {
            if(mouseDown)
            {
                User32.ReleaseCapture();
                User32.SendMessage(Handle, User32.WM_NCLBUTTONDOWN, User32.HT_CAPTION, 0);
            }
        }

        private void Video_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        public void UpdateTitle() => Text = $"TMRP - {player.MediaPlayer.Media.Meta(MetadataType.Title)} - {player.Volume}%";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Show();
            Focus();

            if (!configuration.Load())
            {
                MessageBox.Show(this, "Erro ao carregar a configuração do registro!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            else
            {
                if (configuration.Size != null)
                    Size = configuration.Size;

                if (configuration.Location != null)
                    Location = configuration.Location;

                FormBorderStyle = configuration.Bordeless ? FormBorderStyle.None : FormBorderStyle.SizableToolWindow;
                TopMost = configuration.Bordeless;

                if (TopMost)
                    TopLevel = true;

                player = new Player(configuration);
                video.MediaPlayer = player.MediaPlayer;

                if (configuration.LastFile != null)
                    OpenFile(configuration.LastFile, false);
            }
        }

        private void FrmMain_MouseEnter(object sender, EventArgs e)
        {
            video.Focus();
        }

        private void Video_MouseWheel(object sender, MouseEventArgs e)
        {
            const int SCROLL_VOLUME_CHANGE = 5;
            if(e.Delta > 0)
            {
                player.Volume += SCROLL_VOLUME_CHANGE;
                UpdateTitle();
            }
            else if(video.MediaPlayer.Volume > 0)
            {
                player.Volume -= SCROLL_VOLUME_CHANGE;
                UpdateTitle();
            }
        }

        private void FrmMain_ResizeEnd(object sender, EventArgs e)
        {
            configuration.Location = Location;
            configuration.Size = Size;
        }

        void OpenFile(string filename, bool writeRegistry = true)
        {
            player.Play(filename);
            UpdateTitle();

            if (writeRegistry)
                configuration.LastFile = filename;
        }

        private void Video_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.H)
            {
                FormBorderStyle = FormBorderStyle == FormBorderStyle.None ? FormBorderStyle.SizableToolWindow : FormBorderStyle.None;
                configuration.Bordeless = FormBorderStyle == FormBorderStyle.None;
            }
            else if(e.Control && e.KeyCode == Keys.T)
            {
                TopMost = !TopMost;
                if (TopMost)
                {
                    TopLevel = true;
                    Show();
                }
            }
            else if(e.KeyCode == Keys.Space)
            {
                player.TogglePause();
            }
            else if(e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if(e.Control && (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus))
            {
                player.Volume += 5;
                UpdateTitle();
            }
            else if(e.Control && (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus) && player.Volume > 0)
            {
                player.Volume -= 5; 
                UpdateTitle();
            }
        }

        private void Video_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void Video_DragDrop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if(files.Length > 0)
                {
                    OpenFile(files[0]);
                }
            }
        }
    }
}
