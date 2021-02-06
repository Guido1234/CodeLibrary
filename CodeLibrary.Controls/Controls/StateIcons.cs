using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary.Controls.Controls
{
    public partial class StateIcons : UserControl
    {
        private Dictionary<string, ImageHolder> _Icons = new Dictionary<string, ImageHolder>();
        private Timer _Timer = new Timer();
        private bool _Blink = false;

        public StateIcons()
        {
            InitializeComponent();
            ResizeRedraw = true;
            DoubleBuffered = true;
            _Timer.Interval = 750;            
            _Timer.Tick += _Timer_Tick;
            Resize += StateIcons_Resize;
            Paint += StateIcons_Paint;
            Load += StateIcons_Load;
        }

        private void StateIcons_Load(object sender, EventArgs e)
        {
            _Timer.Enabled = true;
        }

        private void _Timer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        public bool this[string key]
        {
            get
            {
                return _Icons[key].Visible;
            }
            set
            {
                _Icons[key].Visible = value;
                Refresh();
            }
        }

        public void AddIcon(Image image, string key, bool blink = false)
        {
            _Icons.Add(key, new ImageHolder() { Image = image, Key = key, Visible = false, Blink = blink });
        }

        private void StateIcons_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.Clear(BackColor);
            int x = Width - 16;
            foreach (ImageHolder image in _Icons.Values.Where(p => p.Visible == true))
            {
                if (image.Blink == false || (image.Blink == true && _Blink))
                {
                    e.Graphics.DrawImage(image.Image, new Point(x, 0));
                }
                x -= 16;
            }
            _Blink = !_Blink;
        }

        private void StateIcons_Resize(object sender, EventArgs e)
        {
            Height = 16;
        }

        private class ImageHolder
        {
            public Image Image { get; set; }
            public string Key { get; set; }
            public bool Visible { get; set; }

            public bool Blink { get; set; }
        }
    }
}