namespace CodeLibrary.Controls
{
    partial class FormTypeSelector
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
            this.typeList1 = new TypeList();
            this.dialogButton1 = new DialogButton();
            this.SuspendLayout();
            // 
            // typeList1
            // 
            this.typeList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.typeList1.AutoExpand = true;
            this.typeList1.Location = new System.Drawing.Point(12, 12);
            this.typeList1.Name = "typeList1";
            this.typeList1.Size = new System.Drawing.Size(309, 441);
            this.typeList1.TabIndex = 0;
            this.typeList1.ViewMode = TypeList.EViewMode.ListView;
            // 
            // dialogButton1
            // 
            this.dialogButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButton1.ButtonMode = CodeLibrary.Controls.DialogButton.DialogButtonMode.OkCancel;
            this.dialogButton1.Location = new System.Drawing.Point(165, 476);
            this.dialogButton1.Name = "dialogButton1";
            this.dialogButton1.Size = new System.Drawing.Size(156, 23);
            this.dialogButton1.TabIndex = 1;
            this.dialogButton1.TextCancel = "Cancel";
            this.dialogButton1.TextIgnore = "Ignore";
            this.dialogButton1.TextNo = "No";
            this.dialogButton1.TextOk = "Ok";
            this.dialogButton1.TextRetry = "Retry";
            this.dialogButton1.TextYes = "Yes";
            this.dialogButton1.DialogButtonClick += new Controls.DialogButton.DialogButtonClickEventHandler(this.DialogButton1_DialogButtonClick);
            // 
            // FormTypeSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 511);
            this.Controls.Add(this.dialogButton1);
            this.Controls.Add(this.typeList1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTypeSelector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormTypeSelector";
            this.Load += new System.EventHandler(this.FormTypeSelector_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private TypeList typeList1;
        private DialogButton dialogButton1;
    }
}