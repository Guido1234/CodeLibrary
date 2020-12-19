using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeLibrary.Controls.Controls
{
    public partial class ImageViewer : UserControl
    {
        private int _OffsetX = 0;
        private int _OffsetY = 0;
        private int _CurrentX = 0;
        private int _CurrentY = 0;

        public event MouseEventHandler ImageMouseClick;

        public ImageViewer()
        {
            InitializeComponent();
            picture.MouseWheel += Picture_MouseWheel;
            MouseWheel += Picture_MouseWheel;
            picture.MouseMove += Picture_MouseMove;
            picture.MouseHover += Picture_MouseHover;
            Resize += ImageViewer_Resize;
            MouseClick += ImageViewer_MouseClick;
            picture.MouseClick += ImageViewer_MouseClick;
        }

        private void ImageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            ImageMouseClick(sender, e);
        }

        private void ImageViewer_Resize(object sender, EventArgs e)
        {
            Center();
        }

        private void Picture_MouseHover(object sender, EventArgs e)
        {

        }

        private void Picture_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _OffsetX = _CurrentY - e.X;
                _OffsetY = _CurrentY - e.Y;

            }
            else
            {
                _CurrentX = e.X;
                _CurrentY = e.Y;
            }
        }

        private void Picture_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                // Zoom in
                picture.Width -= 50;
                picture.Height -= 50;
                Center();
            }
            else
            {
                // Zoom out
                picture.Width += 50;
                picture.Height += 50;
                Center();
            }
        }

        private void Center()
        {
            Point _p = new Point((this.Width / 2) - (picture.Width / 2), (this.Height / 2) - (picture.Height / 2)  );
            picture.Location = _p;
        }

        private void ImageViewer_Load(object sender, EventArgs e)
        {
        }

        private Image ConvertByteArrayToImage(byte[] imageToConvert)
        {
            if (imageToConvert == null)
            {
                return null;
            }
            if (imageToConvert.Length == 0)
            {
                return null;
            }
            MemoryStream ms = new MemoryStream(imageToConvert);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            ms.Close();
            return img;
        }

        public void setImage(byte[] imagedata)
        {
            _OffsetX = 0;
            _OffsetY = 0;
            Image _image = ConvertByteArrayToImage(imagedata);
            picture.Image = _image;
            picture.SizeMode = PictureBoxSizeMode.Zoom;
            Center();
        }

        public Image Image
        {
            get
            {
                return picture.Image;
            }
        }

    }
}
