using CodeLibrary.Core;
using System;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormUsbKeyDrive : Form
    {
        private Timer _Timer = new Timer();

        public FormUsbKeyDrive()
        {
            InitializeComponent();
            _Timer.Interval = 2500;
            _Timer.Tick += _Timer_Tick;
            _Timer.Enabled = true;
            CreateUsbKeyDrive = false;
        }

        public bool CreateUsbKeyDrive { get; set; } = false;

        private void _Timer_Tick(object sender, EventArgs e)
        {
            UsbKey _usb = new UsbKey();
            if (_usb.UsbKeyDrivePresent())
            {
                CreateUsbKeyDrive = false;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _Timer.Enabled = false;
            CreateUsbKeyDrive = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _Timer.Enabled = false;
            CreateUsbKeyDrive = false;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _Timer.Enabled = false;
            CreateUsbKeyDrive = false;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}