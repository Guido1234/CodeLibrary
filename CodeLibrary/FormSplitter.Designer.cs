namespace CodeLibrary
{
    partial class FormSplitter
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
            this.textBoxSplitter = new System.Windows.Forms.TextBox();
            this.dialogButton = new Controls.DialogButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxSplitter
            // 
            this.textBoxSplitter.Location = new System.Drawing.Point(29, 54);
            this.textBoxSplitter.Name = "textBoxSplitter";
            this.textBoxSplitter.Size = new System.Drawing.Size(672, 29);
            this.textBoxSplitter.TabIndex = 0;
            // 
            // dialogButton
            // 
            this.dialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButton.ButtonMode = CodeLibrary.Controls.DialogButton.DialogButtonMode.OkCancel;
            this.dialogButton.Location = new System.Drawing.Point(419, 114);
            this.dialogButton.Margin = new System.Windows.Forms.Padding(11);
            this.dialogButton.Name = "dialogButton";
            this.dialogButton.Size = new System.Drawing.Size(282, 42);
            this.dialogButton.TabIndex = 1;
            this.dialogButton.TextCancel = "Cancel";
            this.dialogButton.TextIgnore = "Ignore";
            this.dialogButton.TextNo = "No";
            this.dialogButton.TextOk = "Ok";
            this.dialogButton.TextRetry = "Retry";
            this.dialogButton.TextYes = "Yes";
            this.dialogButton.DialogButtonClick += new Controls.DialogButton.DialogButtonClickEventHandler(this.dialogButton_DialogButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Split document by:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormSplitter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 195);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dialogButton);
            this.Controls.Add(this.textBoxSplitter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSplitter";
            this.Text = "Split Document";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSplitter;
        private Controls.DialogButton dialogButton;
        private System.Windows.Forms.Label label1;
    }
}