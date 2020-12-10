using CodeLibrary.Controls;

namespace CodeLibrary
{
    partial class FormDefaults
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDefaults));
            this.dialogButton = new DialogButton();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbCode = new FastColoredTextBoxNS.FastColoredTextBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.checkBoxCodeType = new System.Windows.Forms.CheckBox();
            this.labelHelp = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbCode)).BeginInit();
            this.SuspendLayout();
            // 
            // dialogButton
            // 
            this.dialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButton.ButtonMode = DialogButton.DialogButtonMode.OkCancel;
            this.dialogButton.Location = new System.Drawing.Point(655, 423);
            this.dialogButton.Name = "dialogButton";
            this.dialogButton.Size = new System.Drawing.Size(156, 23);
            this.dialogButton.TabIndex = 0;
            this.dialogButton.TextCancel = "Cancel";
            this.dialogButton.TextIgnore = "Ignore";
            this.dialogButton.TextNo = "No";
            this.dialogButton.TextOk = "Ok";
            this.dialogButton.TextRetry = "Retry";
            this.dialogButton.TextYes = "Yes";
            this.dialogButton.DialogButtonClick += new DialogButton.DialogButtonClickEventHandler(this.DialogButton_DialogButtonClick);
            // 
            // tbName
            // 
            this.tbName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbName.Location = new System.Drawing.Point(12, 12);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(567, 22);
            this.tbName.TabIndex = 2;
            // 
            // tbCode
            // 
            this.tbCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCode.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.tbCode.AutoScrollMinSize = new System.Drawing.Size(179, 14);
            this.tbCode.BackBrush = null;
            this.tbCode.CharHeight = 14;
            this.tbCode.CharWidth = 8;
            this.tbCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbCode.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.tbCode.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.tbCode.IsReplaceMode = false;
            this.tbCode.Location = new System.Drawing.Point(12, 40);
            this.tbCode.Name = "tbCode";
            this.tbCode.Paddings = new System.Windows.Forms.Padding(0);
            this.tbCode.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.tbCode.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("tbCode.ServiceColors")));
            this.tbCode.Size = new System.Drawing.Size(799, 370);
            this.tbCode.TabIndex = 3;
            this.tbCode.Text = "fastColoredTextBox1";
            this.tbCode.Zoom = 100;
            // 
            // comboBoxType
            // 
            this.comboBoxType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(604, 12);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(207, 21);
            this.comboBoxType.TabIndex = 4;
            // 
            // checkBoxCodeType
            // 
            this.checkBoxCodeType.AutoSize = true;
            this.checkBoxCodeType.Location = new System.Drawing.Point(585, 16);
            this.checkBoxCodeType.Name = "checkBoxCodeType";
            this.checkBoxCodeType.Size = new System.Drawing.Size(15, 14);
            this.checkBoxCodeType.TabIndex = 5;
            this.checkBoxCodeType.UseVisualStyleBackColor = true;
            // 
            // labelHelp
            // 
            this.labelHelp.AutoSize = true;
            this.labelHelp.Location = new System.Drawing.Point(9, 423);
            this.labelHelp.Name = "labelHelp";
            this.labelHelp.Size = new System.Drawing.Size(144, 13);
            this.labelHelp.TabIndex = 7;
            this.labelHelp.Text = "{0} DateTime {1} NodeCount";
            // 
            // FormDefaults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(823, 458);
            this.Controls.Add(this.labelHelp);
            this.Controls.Add(this.checkBoxCodeType);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.tbCode);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.dialogButton);
            this.Name = "FormDefaults";
            this.Text = "FormDefaults";
            this.Load += new System.EventHandler(this.Defaults_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DialogButton dialogButton;
        private System.Windows.Forms.TextBox tbName;
        private FastColoredTextBoxNS.FastColoredTextBox tbCode;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.CheckBox checkBoxCodeType;
        private System.Windows.Forms.Label labelHelp;
    }
}