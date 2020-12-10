using System;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormAlarm : Form
    {
        public FormAlarm()
        {
            InitializeComponent();
        }

        public bool AlarmActive { get; set; }

        public DateTime? AlarmDate { get; set; }

        private void dialogButton_DialogButtonClick(object sender, Controls.DialogButton.DialogButtonClickEventArgs e)
        {
            AlarmActive = chkAlarm.Checked;
            AlarmDate = datePicker.Value.Date.Add(timeControl.Value.TimeOfDay);

            if (e.Result == DialogResult.OK)
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;

            Close();
        }

        private void FormAlarm_Load(object sender, EventArgs e)
        {
            if (AlarmDate.HasValue)
            {
                datePicker.Value = AlarmDate.Value.Date;
                timeControl.Value = AlarmDate.Value;
            }
            chkAlarm.Checked = AlarmActive;
        }
    }
}