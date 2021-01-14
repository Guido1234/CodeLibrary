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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProperties));
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbCode = new FastColoredTextBoxNS.FastColoredTextBox();
            this.cbDefaultType = new System.Windows.Forms.ComboBox();
            this.checkBoxCodeType = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbImportant = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbAlarm = new System.Windows.Forms.CheckBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.comboBoxShortCutKeys = new System.Windows.Forms.ComboBox();
            this.cbControl = new System.Windows.Forms.CheckBox();
            this.cbShift = new System.Windows.Forms.CheckBox();
            this.cbAlt = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timeControl = new System.Windows.Forms.DateTimePicker();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbWordWrap = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.picture = new System.Windows.Forms.PictureBox();
            this.cbHtmlPreview = new System.Windows.Forms.CheckBox();
            this.lbName = new System.Windows.Forms.Label();
            this.cbExpand = new System.Windows.Forms.CheckBox();
            this.rtf = new CodeLibrary.Controls.Controls.RtfControl();
            this.dialogButton = new CodeLibrary.Controls.DialogButton();
            ((System.ComponentModel.ISupportInitialize)(this.tbCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbName.Location = new System.Drawing.Point(415, 142);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(318, 22);
            this.tbName.TabIndex = 2;
            // 
            // tbCode
            // 
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
            this.tbCode.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.tbCode.BackBrush = null;
            this.tbCode.CharHeight = 14;
            this.tbCode.CharWidth = 8;
            this.tbCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbCode.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.tbCode.Hotkeys = resources.GetString("tbCode.Hotkeys");
            this.tbCode.IsReplaceMode = false;
            this.tbCode.Location = new System.Drawing.Point(415, 195);
            this.tbCode.Name = "tbCode";
            this.tbCode.Paddings = new System.Windows.Forms.Padding(0);
            this.tbCode.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.tbCode.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("tbCode.ServiceColors")));
            this.tbCode.Size = new System.Drawing.Size(318, 181);
            this.tbCode.TabIndex = 3;
            this.tbCode.Zoom = 100;
            // 
            // cbDefaultType
            // 
            this.cbDefaultType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultType.FormattingEnabled = true;
            this.cbDefaultType.Location = new System.Drawing.Point(433, 90);
            this.cbDefaultType.Name = "cbDefaultType";
            this.cbDefaultType.Size = new System.Drawing.Size(175, 21);
            this.cbDefaultType.TabIndex = 4;
            // 
            // checkBoxCodeType
            // 
            this.checkBoxCodeType.AutoSize = true;
            this.checkBoxCodeType.Location = new System.Drawing.Point(415, 67);
            this.checkBoxCodeType.Name = "checkBoxCodeType";
            this.checkBoxCodeType.Size = new System.Drawing.Size(192, 17);
            this.checkBoxCodeType.TabIndex = 5;
            this.checkBoxCodeType.Text = "Use following default type for childs";
            this.checkBoxCodeType.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(415, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(248, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Default name: ( use: {0} DateTime {1} NodeCount )";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(415, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(239, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Default text: ( use: {0} DateTime {1} NodeCount )";
            // 
            // cbImportant
            // 
            this.cbImportant.AutoSize = true;
            this.cbImportant.Location = new System.Drawing.Point(90, 113);
            this.cbImportant.Name = "cbImportant";
            this.cbImportant.Size = new System.Drawing.Size(70, 17);
            this.cbImportant.TabIndex = 11;
            this.cbImportant.Text = "Important";
            this.cbImportant.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CodeLibrary.Properties.Resources.error_16x16;
            this.pictureBox1.Location = new System.Drawing.Point(64, 113);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 17);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // cbAlarm
            // 
            this.cbAlarm.AutoSize = true;
            this.cbAlarm.Location = new System.Drawing.Point(90, 140);
            this.cbAlarm.Name = "cbAlarm";
            this.cbAlarm.Size = new System.Drawing.Size(71, 17);
            this.cbAlarm.TabIndex = 13;
            this.cbAlarm.Text = "Reminder";
            this.cbAlarm.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::CodeLibrary.Properties.Resources.clock_16x16;
            this.pictureBox2.Location = new System.Drawing.Point(64, 140);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(22, 17);
            this.pictureBox2.TabIndex = 14;
            this.pictureBox2.TabStop = false;
            // 
            // datePicker
            // 
            this.datePicker.Location = new System.Drawing.Point(90, 163);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(200, 20);
            this.datePicker.TabIndex = 15;
            // 
            // comboBoxShortCutKeys
            // 
            this.comboBoxShortCutKeys.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxShortCutKeys.FormattingEnabled = true;
            this.comboBoxShortCutKeys.Location = new System.Drawing.Point(64, 316);
            this.comboBoxShortCutKeys.Name = "comboBoxShortCutKeys";
            this.comboBoxShortCutKeys.Size = new System.Drawing.Size(165, 21);
            this.comboBoxShortCutKeys.TabIndex = 16;
            // 
            // cbControl
            // 
            this.cbControl.AutoSize = true;
            this.cbControl.Location = new System.Drawing.Point(63, 293);
            this.cbControl.Name = "cbControl";
            this.cbControl.Size = new System.Drawing.Size(59, 17);
            this.cbControl.TabIndex = 17;
            this.cbControl.Text = "Control";
            this.cbControl.UseVisualStyleBackColor = true;
            // 
            // cbShift
            // 
            this.cbShift.AutoSize = true;
            this.cbShift.Location = new System.Drawing.Point(128, 293);
            this.cbShift.Name = "cbShift";
            this.cbShift.Size = new System.Drawing.Size(47, 17);
            this.cbShift.TabIndex = 18;
            this.cbShift.Text = "Shift";
            this.cbShift.UseVisualStyleBackColor = true;
            // 
            // cbAlt
            // 
            this.cbAlt.AutoSize = true;
            this.cbAlt.Location = new System.Drawing.Point(181, 293);
            this.cbAlt.Name = "cbAlt";
            this.cbAlt.Size = new System.Drawing.Size(38, 17);
            this.cbAlt.TabIndex = 19;
            this.cbAlt.Text = "Alt";
            this.cbAlt.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(60, 273);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Template Shortcut Keys";
            // 
            // timeControl
            // 
            this.timeControl.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timeControl.Location = new System.Drawing.Point(295, 163);
            this.timeControl.Margin = new System.Windows.Forms.Padding(2);
            this.timeControl.Name = "timeControl";
            this.timeControl.ShowUpDown = true;
            this.timeControl.Size = new System.Drawing.Size(73, 20);
            this.timeControl.TabIndex = 21;
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(64, 83);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(175, 21);
            this.cbType.TabIndex = 22;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(60, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Properties:";
            // 
            // cbWordWrap
            // 
            this.cbWordWrap.AutoSize = true;
            this.cbWordWrap.Location = new System.Drawing.Point(64, 191);
            this.cbWordWrap.Name = "cbWordWrap";
            this.cbWordWrap.Size = new System.Drawing.Size(78, 17);
            this.cbWordWrap.TabIndex = 24;
            this.cbWordWrap.Text = "Word wrap";
            this.cbWordWrap.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(412, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Defaults:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(61, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Type:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(61, 340);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(355, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Use a note as a Template to merge with selection by using a shortcut key.";
            // 
            // Icons
            // 
            this.Icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Icons.ImageStream")));
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            this.Icons.Images.SetKeyName(0, "csharp");
            this.Icons.Images.SetKeyName(1, "html");
            this.Icons.Images.SetKeyName(2, "js");
            this.Icons.Images.SetKeyName(3, "lua");
            this.Icons.Images.SetKeyName(4, "php");
            this.Icons.Images.SetKeyName(5, "rtf");
            this.Icons.Images.SetKeyName(6, "sql");
            this.Icons.Images.SetKeyName(7, "none");
            this.Icons.Images.SetKeyName(8, "vb");
            this.Icons.Images.SetKeyName(9, "xml");
            this.Icons.Images.SetKeyName(10, "template");
            this.Icons.Images.SetKeyName(11, "folder");
            // 
            // picture
            // 
            this.picture.Image = global::CodeLibrary.Properties.Resources.Txt_Black;
            this.picture.Location = new System.Drawing.Point(12, 18);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(32, 32);
            this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picture.TabIndex = 28;
            this.picture.TabStop = false;
            // 
            // cbHtmlPreview
            // 
            this.cbHtmlPreview.AutoSize = true;
            this.cbHtmlPreview.Location = new System.Drawing.Point(64, 214);
            this.cbHtmlPreview.Name = "cbHtmlPreview";
            this.cbHtmlPreview.Size = new System.Drawing.Size(88, 17);
            this.cbHtmlPreview.TabIndex = 29;
            this.cbHtmlPreview.Text = "Html Preview";
            this.cbHtmlPreview.UseVisualStyleBackColor = true;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(60, 18);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(51, 16);
            this.lbName.TabIndex = 31;
            this.lbName.Text = "NAME";
            // 
            // cbExpand
            // 
            this.cbExpand.AutoSize = true;
            this.cbExpand.Location = new System.Drawing.Point(64, 237);
            this.cbExpand.Name = "cbExpand";
            this.cbExpand.Size = new System.Drawing.Size(91, 17);
            this.cbExpand.TabIndex = 32;
            this.cbExpand.Text = "Expand Node";
            this.cbExpand.UseVisualStyleBackColor = true;
            // 
            // rtf
            // 
            this.rtf.BackColor = System.Drawing.SystemColors.Window;
            this.rtf.Location = new System.Drawing.Point(351, 191);
            this.rtf.Name = "rtf";
            this.rtf.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 " +
    "Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.19041}\\viewkind4\\uc1 \r\n\\par" +
    "d\\f0\\fs17\\par\r\n}\r\n";
            this.rtf.SelectedRtf = "";
            this.rtf.SelectedText = "";
            this.rtf.Size = new System.Drawing.Size(132, 163);
            this.rtf.TabIndex = 30;
            this.rtf.Visible = false;
            this.rtf.Zoom = 100;
            // 
            // dialogButton
            // 
            this.dialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButton.ButtonMode = CodeLibrary.Controls.DialogButton.DialogButtonMode.OkCancel;
            this.dialogButton.Location = new System.Drawing.Point(571, 386);
            this.dialogButton.Name = "dialogButton";
            this.dialogButton.Size = new System.Drawing.Size(156, 23);
            this.dialogButton.TabIndex = 0;
            this.dialogButton.TextCancel = "Cancel";
            this.dialogButton.TextIgnore = "Ignore";
            this.dialogButton.TextNo = "No";
            this.dialogButton.TextOk = "Ok";
            this.dialogButton.TextRetry = "Retry";
            this.dialogButton.TextYes = "Yes";
            this.dialogButton.DialogButtonClick += new CodeLibrary.Controls.DialogButton.DialogButtonClickEventHandler(this.DialogButton_DialogButtonClick);
            // 
            // FormProperties2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(760, 421);
            this.Controls.Add(this.cbExpand);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.rtf);
            this.Controls.Add(this.cbHtmlPreview);
            this.Controls.Add(this.picture);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbWordWrap);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.timeControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbAlt);
            this.Controls.Add(this.cbShift);
            this.Controls.Add(this.cbControl);
            this.Controls.Add(this.comboBoxShortCutKeys);
            this.Controls.Add(this.datePicker);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.cbAlarm);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cbImportant);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxCodeType);
            this.Controls.Add(this.cbDefaultType);
            this.Controls.Add(this.tbCode);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.dialogButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProperties2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.Load += new System.EventHandler(this.Defaults_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DialogButton dialogButton;
        private System.Windows.Forms.TextBox tbName;
        private FastColoredTextBoxNS.FastColoredTextBox tbCode;
        private System.Windows.Forms.ComboBox cbDefaultType;
        private System.Windows.Forms.CheckBox checkBoxCodeType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbImportant;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox cbAlarm;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.ComboBox comboBoxShortCutKeys;
        private System.Windows.Forms.CheckBox cbControl;
        private System.Windows.Forms.CheckBox cbShift;
        private System.Windows.Forms.CheckBox cbAlt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker timeControl;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbWordWrap;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ImageList Icons;
        private System.Windows.Forms.PictureBox picture;
        private System.Windows.Forms.CheckBox cbHtmlPreview;
        private Controls.Controls.RtfControl rtf;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.CheckBox cbExpand;
    }
}