namespace CodeLibrary
{
    partial class FormSetPassword
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
            this.dialogButton = new Controls.DialogButton();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // dialogButton
            // 
            this.dialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButton.ButtonMode = CodeLibrary.Controls.DialogButton.DialogButtonMode.OkCancel;
            this.dialogButton.Location = new System.Drawing.Point(149, 83);
            this.dialogButton.Name = "dialogButton";
            this.dialogButton.Size = new System.Drawing.Size(156, 23);
            this.dialogButton.TabIndex = 0;
            this.dialogButton.TextCancel = "Cancel";
            this.dialogButton.TextIgnore = "Ignore";
            this.dialogButton.TextNo = "No";
            this.dialogButton.TextOk = "Ok";
            this.dialogButton.TextRetry = "Retry";
            this.dialogButton.TextYes = "Yes";
            this.dialogButton.DialogButtonClick += new Controls.DialogButton.DialogButtonClickEventHandler(this.dialogButton_DialogButtonClick);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPassword.Location = new System.Drawing.Point(12, 41);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(292, 20);
            this.textBoxPassword.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Set Password";
            // 
            // FormSetPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 118);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.dialogButton);
            this.Name = "FormSetPassword";
            this.Text = "Set Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.DialogButton dialogButton;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label1;
    }
}