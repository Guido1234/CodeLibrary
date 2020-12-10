namespace CodeLibrary
{
    partial class FormAlarm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.dialogButton = new Controls.DialogButton();
            this.chkAlarm = new System.Windows.Forms.CheckBox();
            this.timeControl = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // datePicker
            // 
            this.datePicker.Location = new System.Drawing.Point(34, 51);
            this.datePicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(245, 22);
            this.datePicker.TabIndex = 0;
            // 
            // dialogButton
            // 
            this.dialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButton.ButtonMode = CodeLibrary.Controls.DialogButton.DialogButtonMode.OkCancel;
            this.dialogButton.Location = new System.Drawing.Point(177, 125);
            this.dialogButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.dialogButton.Name = "dialogButton";
            this.dialogButton.Size = new System.Drawing.Size(206, 28);
            this.dialogButton.TabIndex = 1;
            this.dialogButton.TextCancel = "Cancel";
            this.dialogButton.TextIgnore = "Ignore";
            this.dialogButton.TextNo = "No";
            this.dialogButton.TextOk = "Ok";
            this.dialogButton.TextRetry = "Retry";
            this.dialogButton.TextYes = "Yes";
            this.dialogButton.DialogButtonClick += new Controls.DialogButton.DialogButtonClickEventHandler(this.dialogButton_DialogButtonClick);
            // 
            // chkAlarm
            // 
            this.chkAlarm.AutoSize = true;
            this.chkAlarm.Location = new System.Drawing.Point(11, 23);
            this.chkAlarm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkAlarm.Name = "chkAlarm";
            this.chkAlarm.Size = new System.Drawing.Size(91, 21);
            this.chkAlarm.TabIndex = 2;
            this.chkAlarm.Text = "Set Alarm";
            this.chkAlarm.UseVisualStyleBackColor = true;
            // 
            // timeControl
            // 
            this.timeControl.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timeControl.Location = new System.Drawing.Point(285, 51);
            this.timeControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.timeControl.Name = "timeControl";
            this.timeControl.ShowUpDown = true;
            this.timeControl.Size = new System.Drawing.Size(96, 22);
            this.timeControl.TabIndex = 3;
            // 
            // FormAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 167);
            this.Controls.Add(this.timeControl);
            this.Controls.Add(this.chkAlarm);
            this.Controls.Add(this.dialogButton);
            this.Controls.Add(this.datePicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormAlarm";
            this.Text = "FormAlarm";
            this.Load += new System.EventHandler(this.FormAlarm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker datePicker;
        private Controls.DialogButton dialogButton;
        private System.Windows.Forms.CheckBox chkAlarm;
        private System.Windows.Forms.DateTimePicker timeControl;
    }
}