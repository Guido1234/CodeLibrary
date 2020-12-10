using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace CodeLibrary.Controls
{
    [DefaultEvent("DialogButtonClick")]
    public partial class DialogButton : UserControl
    {
        private DialogButtonMode buttonMode = DialogButtonMode.OkCancel;

        private int spacing = 6;

        public DialogButton()
        {
            InitializeComponent();
            this.TextOk = "Ok";
            this.TextCancel = "Cancel";
            this.TextYes = "Yes";
            this.TextNo = "No";
            this.TextIgnore = "Ignore";
            this.TextRetry = "Retry";
        }

        public delegate void DialogButtonClickEventHandler(object sender, DialogButtonClickEventArgs e);

        [Category("DialogButton")]
        [Description("Event triggered when dialogbutton is clicked")]
        public event DialogButtonClickEventHandler DialogButtonClick;

        public enum DialogButtonMode
        {
            Ok = 0,
            OkCancel = 1,
            YesNo = 2,
            YesNoCancel = 3,
            OkCancelRetry = 4,
            Ignore = 5,
            OkCancelIgnore = 6,
            YesNoIgnore = 7,
            OkIgnore = 8
        }

        [Category("DialogButton")]
        [Description("Determines which buttons to show")]
        public DialogButtonMode ButtonMode
        {
            get
            {
                return buttonMode;
            }
            set
            {
                buttonMode = value;
                setButtons();
            }
        }

        [Category("DialogButton")]
        [Description("Text for the Cancel button")]
        public string TextCancel { get; set; }

        [Category("DialogButton")]
        [Description("Text for the Ignore button")]
        public string TextIgnore { get; set; }

        [Category("DialogButton")]
        [Description("Text for the No button")]
        public string TextNo { get; set; }

        [Category("DialogButton")]
        [Description("Text for the OK button")]
        public string TextOk { get; set; }

        [Category("DialogButton")]
        [Description("Text for the Retry button")]
        public string TextRetry { get; set; }

        [Category("DialogButton")]
        [Description("Text for the Yes button")]
        public string TextYes { get; set; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            raiseEventDialogButtonClick(DialogResult.Cancel);
        }

        private void buttonIgnore_Click(object sender, EventArgs e)
        {
            raiseEventDialogButtonClick(DialogResult.Ignore);
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            raiseEventDialogButtonClick(DialogResult.No);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            raiseEventDialogButtonClick(DialogResult.OK);
        }

        private void buttonRetry_Click(object sender, EventArgs e)
        {
            raiseEventDialogButtonClick(DialogResult.Retry);
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            raiseEventDialogButtonClick(DialogResult.Yes);
        }

        private void DialogButton_Load(object sender, EventArgs e)
        {
            setButtons();
        }

        private void DialogButton_Resize(object sender, EventArgs e)
        {
            setButtons();
        }

        private void raiseEventDialogButtonClick(DialogResult result)
        {
            DialogButtonClickEventHandler handler = DialogButtonClick;
            if (handler != null)
            {
                DialogButtonClickEventArgs ea = new DialogButtonClickEventArgs();
                ea.Result = result;
                handler(this, ea);
            }
        }

        private void setButtons()
        {
            this.buttonOk.Text = this.TextOk;
            this.buttonCancel.Text = this.TextCancel;
            this.buttonYes.Text = this.TextYes;
            this.buttonNo.Text = this.TextNo;
            this.buttonIgnore.Text = this.TextIgnore;
            this.buttonRetry.Text = this.TextRetry;

            switch (buttonMode)
            {
                case DialogButtonMode.Ignore:
                    this.buttonCancel.Visible = false;
                    this.buttonIgnore.Visible = true;
                    this.buttonNo.Visible = false;
                    this.buttonOk.Visible = false;
                    this.buttonRetry.Visible = false;
                    this.buttonYes.Visible = false;
                    break;

                case DialogButtonMode.Ok:
                    this.buttonCancel.Visible = false;
                    this.buttonIgnore.Visible = false;
                    this.buttonNo.Visible = false;
                    this.buttonOk.Visible = true;
                    this.buttonRetry.Visible = false;
                    this.buttonYes.Visible = false;
                    break;

                case DialogButtonMode.OkCancel:
                    this.buttonCancel.Visible = true;
                    this.buttonIgnore.Visible = false;
                    this.buttonNo.Visible = false;
                    this.buttonOk.Visible = true;
                    this.buttonRetry.Visible = false;
                    this.buttonYes.Visible = false;
                    break;

                case DialogButtonMode.OkCancelIgnore:
                    this.buttonCancel.Visible = true;
                    this.buttonIgnore.Visible = true;
                    this.buttonNo.Visible = false;
                    this.buttonOk.Visible = true;
                    this.buttonRetry.Visible = false;
                    this.buttonYes.Visible = false;
                    break;

                case DialogButtonMode.OkCancelRetry:
                    this.buttonCancel.Visible = true;
                    this.buttonIgnore.Visible = false;
                    this.buttonNo.Visible = false;
                    this.buttonOk.Visible = true;
                    this.buttonRetry.Visible = true;
                    this.buttonYes.Visible = false;
                    break;

                case DialogButtonMode.OkIgnore:
                    this.buttonCancel.Visible = false;
                    this.buttonIgnore.Visible = true;
                    this.buttonNo.Visible = false;
                    this.buttonOk.Visible = true;
                    this.buttonRetry.Visible = false;
                    this.buttonYes.Visible = false;
                    break;

                case DialogButtonMode.YesNo:
                    this.buttonCancel.Visible = false;
                    this.buttonIgnore.Visible = false;
                    this.buttonNo.Visible = true;
                    this.buttonOk.Visible = false;
                    this.buttonRetry.Visible = false;
                    this.buttonYes.Visible = true;
                    break;

                case DialogButtonMode.YesNoCancel:
                    this.buttonCancel.Visible = true;
                    this.buttonIgnore.Visible = false;
                    this.buttonNo.Visible = true;
                    this.buttonOk.Visible = false;
                    this.buttonRetry.Visible = false;
                    this.buttonYes.Visible = true;
                    break;

                case DialogButtonMode.YesNoIgnore:
                    this.buttonCancel.Visible = false;
                    this.buttonIgnore.Visible = true;
                    this.buttonNo.Visible = true;
                    this.buttonOk.Visible = false;
                    this.buttonRetry.Visible = false;
                    this.buttonYes.Visible = true;
                    break;
            }

            int left = 0;
            int width = 0;
            foreach (Control c in this.Controls)
            {
                if (c.Visible)
                {
                    c.Left = left;
                    left = c.Left + c.Width + spacing;
                    width = c.Left + c.Width;
                }
            }
            this.Width = width;
            this.Height = this.buttonOk.Height;
        }

        public class DialogButtonClickEventArgs : EventArgs
        {
            public DialogButtonClickEventArgs()
            {
                // Constructor logic here
            }

            public DialogResult Result { get; set; }
        }
    }
}