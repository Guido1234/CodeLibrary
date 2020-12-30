using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CodeLibrary.Controls.Controls
{
    public partial class ImageViewer : UserControl
    {
        private Point _Offset = new Point(0, 0);

        private bool _Pressed = false;

        public ImageViewer()
        {
            InitializeComponent();
            picture.MouseWheel += Picture_MouseWheel;
            MouseWheel += Picture_MouseWheel;
            picture.MouseMove += Picture_MouseMove;
            Resize += ImageViewer_Resize;
            MouseClick += ImageViewer_MouseClick;
            picture.MouseClick += ImageViewer_MouseClick;
        }

        public event MouseEventHandler ImageMouseClick;

        public Image Image
        {
            get
            {
                return picture.Image;
            }
        }

        public void setImage(byte[] imagedata)
        {
            _Offset = new Point(0, 0);
            Image _image = ConvertByteArrayToImage(imagedata);
            picture.Image = _image;
            picture.Width = _image.Width;
            picture.Height = _image.Height;
            picture.SizeMode = PictureBoxSizeMode.Zoom;
            
            Center();
        }

        private void Center()
        {
            Point _p = new Point((this.Width / 2) - (picture.Width / 2), (this.Height / 2) - (picture.Height / 2));
            picture.Location = _p;
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
            var ms = new MemoryStream(imageToConvert);
            var img = Image.FromStream(ms);
            ms.Close();
            return img;
        }

        private void ImageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            ImageMouseClick(sender, e);
        }

        private void ImageViewer_Resize(object sender, EventArgs e)
        {
            Center();
        }

        private void Picture_MouseMove(object sender, MouseEventArgs e)
        {
            Point _p = PointToClient(Cursor.Position);

            if (e.Button == MouseButtons.Left)
            {
                if (_Pressed == false)
                {
                    _Pressed = true;
                    _Offset = e.Location;
                }
            }
            else
            {
                if (_Pressed == true)
                {
                    _Pressed = false;
                }
            }
            if (e.Button == MouseButtons.Left && _Pressed == true)
            {
                picture.Location = new Point(_p.X - _Offset.X, _p.Y - _Offset.Y);
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
    }
}