using CodeLibrary.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormProperties : Form
    {
        public FormProperties()
        {
            InitializeComponent();
        }

        public Object SelectedObject
        {
            get
            {
                return propGrid.SelectedObject;
            }
            set
            {
                propGrid.SelectedObject = value;
            }
        }

        private void DarkTheme()
        {
            BackColor = Color.FromArgb(255, 100, 100, 100);
        }

        private void DialogButton_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            DialogResult = e.Result;
            Close();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            if (Config.HighContrastMode)
                HighContrastTheme();
            else if (Config.DarkMode)
                DarkTheme();
            else
                LightTheme();
        }

        private void HighContrastTheme()
        {
            BackColor = Color.FromArgb(255, 80, 80, 80);
        }

        private void LightTheme()
        {
            BackColor = Color.FromArgb(255, 240, 240, 240);
        }
    }
}