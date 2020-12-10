namespace CodeLibrary.PluginPack.Forms
{
    partial class FormTrimLines
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
            this.tbTrimChars = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.chkTrimStart = new System.Windows.Forms.CheckBox();
            this.chkTrimEnd = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbTrimChars
            // 
            this.tbTrimChars.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTrimChars.Location = new System.Drawing.Point(12, 30);
            this.tbTrimChars.Name = "tbTrimChars";
            this.tbTrimChars.Size = new System.Drawing.Size(368, 22);
            this.tbTrimChars.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Trim Characters";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(305, 128);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 35);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(224, 128);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 35);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // chkTrimStart
            // 
            this.chkTrimStart.AutoSize = true;
            this.chkTrimStart.Checked = true;
            this.chkTrimStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrimStart.Location = new System.Drawing.Point(12, 58);
            this.chkTrimStart.Name = "chkTrimStart";
            this.chkTrimStart.Size = new System.Drawing.Size(92, 21);
            this.chkTrimStart.TabIndex = 4;
            this.chkTrimStart.Text = "Trim Start";
            this.chkTrimStart.UseVisualStyleBackColor = true;
            // 
            // chkTrimEnd
            // 
            this.chkTrimEnd.AutoSize = true;
            this.chkTrimEnd.Checked = true;
            this.chkTrimEnd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrimEnd.Location = new System.Drawing.Point(12, 85);
            this.chkTrimEnd.Name = "chkTrimEnd";
            this.chkTrimEnd.Size = new System.Drawing.Size(87, 21);
            this.chkTrimEnd.TabIndex = 5;
            this.chkTrimEnd.Text = "Trim End";
            this.chkTrimEnd.UseVisualStyleBackColor = true;
            // 
            // FormTrimLines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 175);
            this.Controls.Add(this.chkTrimEnd);
            this.Controls.Add(this.chkTrimStart);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTrimChars);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormTrimLines";
            this.Text = "TrimLines";
            this.Load += new System.EventHandler(this.FormTrimLines_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbTrimChars;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.CheckBox chkTrimStart;
        private System.Windows.Forms.CheckBox chkTrimEnd;
    }
}