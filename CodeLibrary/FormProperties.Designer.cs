using CodeLibrary.Controls;

namespace CodeLibrary
{
    partial class FormProperties
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
            this.dialogButton = new DialogButton();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // dialogButton
            // 
            this.dialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButton.ButtonMode = DialogButton.DialogButtonMode.Ok;
            this.dialogButton.Location = new System.Drawing.Point(381, 425);
            this.dialogButton.Name = "dialogButton";
            this.dialogButton.Size = new System.Drawing.Size(75, 23);
            this.dialogButton.TabIndex = 0;
            this.dialogButton.TextCancel = "Cancel";
            this.dialogButton.TextIgnore = "Ignore";
            this.dialogButton.TextNo = "No";
            this.dialogButton.TextOk = "Ok";
            this.dialogButton.TextRetry = "Retry";
            this.dialogButton.TextYes = "Yes";
            this.dialogButton.DialogButtonClick += new DialogButton.DialogButtonClickEventHandler(this.DialogButton_DialogButtonClick);
            // 
            // propGrid
            // 
            this.propGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propGrid.Location = new System.Drawing.Point(12, 12);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(444, 398);
            this.propGrid.TabIndex = 1;
            // 
            // FormProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(468, 460);
            this.Controls.Add(this.propGrid);
            this.Controls.Add(this.dialogButton);
            this.Name = "FormProperties";
            this.Text = "FormDefaults";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DialogButton dialogButton;
        private System.Windows.Forms.PropertyGrid propGrid;
    }
}