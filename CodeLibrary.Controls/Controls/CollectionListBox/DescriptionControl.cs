using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.Controls
{
    public partial class DescriptionControl : UserControl
    {
        public DescriptionControl()
        {
            InitializeComponent();
        }

        public string Caption
        {
            get
            {
                return this.labelName.Text;
            }
            set
            {
                this.labelName.Text = value;
            }
        }

        public string Description
        {
            get
            {
                return this.textBoxDescription.Text;
            }
            set
            {
                this.textBoxDescription.Text = value;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return this.textBoxDescription.ForeColor;
            }
            set
            {
                this.textBoxDescription.ForeColor = value;
            }
        }

        /// <summary>
        /// Sets the Caption and Description by type of an object, The Description Attribute is used for the desription.
        /// </summary>
        /// <param name="pType"></param>
        public void SetByType(Type pType)
        {
            Caption = GetDisplayName(pType);

            Type t = pType;
            StringBuilder sb = new StringBuilder();
            string s = GetDescription(t);
            sb.Append(s);
            t = t.BaseType;
            while (t != null)
            {
                s = GetDescription(t);
                if (!string.IsNullOrEmpty(s))
                {
                    sb.Append("\r\n\r\n\r\n");
                    sb.Append("Derives from:");
                    sb.Append(GetDisplayName(t));
                    sb.Append("\r\n");
                    sb.Append(s);
                }
                t = t.BaseType;
            }
            Description = sb.ToString();
        }

        private void DescriptionControl_Load(object sender, EventArgs e)
        {
        }

        private void DescriptionControl_Resize(object sender, EventArgs e)
        {
            this.textBoxDescription.Width = this.Width - this.textBoxDescription.Left - 3;
            this.textBoxDescription.Height = this.Height - this.textBoxDescription.Top - 3;
        }

        private void TextBoxDescription_MouseUp(object sender, MouseEventArgs e)
        {
        }

        #region Helpers

        private static string GetDescription(Type type) => (type.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description;

        private static string GetDisplayName(Type type) => (type.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute)?.DisplayName;

        #endregion Helpers
    }
}