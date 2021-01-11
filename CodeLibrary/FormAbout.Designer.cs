using CodeLibrary.Controls;

namespace CodeLibrary
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.tbCode = new FastColoredTextBoxNS.FastColoredTextBox();
            this.lbTitle = new System.Windows.Forms.Label();
            this.dialogButton = new CodeLibrary.Controls.DialogButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
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
            this.tbCode.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;]+);\r\n^\\s*(case|default)\\s*[^:]" +
    "*(?<range>:)\\s*(?<range>[^;]+);\r\n";
            this.tbCode.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.tbCode.BackBrush = null;
            this.tbCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCode.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.tbCode.CharHeight = 14;
            this.tbCode.CharWidth = 8;
            this.tbCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbCode.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.tbCode.Hotkeys = resources.GetString("tbCode.Hotkeys");
            this.tbCode.IsReplaceMode = false;
            this.tbCode.Language = FastColoredTextBoxNS.Language.CSharp;
            this.tbCode.LeftBracket = '(';
            this.tbCode.LeftBracket2 = '{';
            this.tbCode.Location = new System.Drawing.Point(3, 51);
            this.tbCode.Name = "tbCode";
            this.tbCode.Paddings = new System.Windows.Forms.Padding(0);
            this.tbCode.RightBracket = ')';
            this.tbCode.RightBracket2 = '}';
            this.tbCode.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.tbCode.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("tbCode.ServiceColors")));
            this.tbCode.Size = new System.Drawing.Size(1006, 570);
            this.tbCode.TabIndex = 6;
            this.tbCode.Zoom = 100;
            this.tbCode.Load += new System.EventHandler(this.TbCode_Load);
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(40, 9);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(39, 33);
            this.lbTitle.TabIndex = 9;
            this.lbTitle.Text = "...";
            // 
            // dialogButton
            // 
            this.dialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButton.ButtonMode = CodeLibrary.Controls.DialogButton.DialogButtonMode.Ok;
            this.dialogButton.ForeColor = System.Drawing.Color.Black;
            this.dialogButton.Location = new System.Drawing.Point(923, 630);
            this.dialogButton.Name = "dialogButton";
            this.dialogButton.Size = new System.Drawing.Size(75, 23);
            this.dialogButton.TabIndex = 7;
            this.dialogButton.TextCancel = "Cancel";
            this.dialogButton.TextIgnore = "Ignore";
            this.dialogButton.TextNo = "No";
            this.dialogButton.TextOk = "Ok";
            this.dialogButton.TextRetry = "Retry";
            this.dialogButton.TextYes = "Yes";
            this.dialogButton.DialogButtonClick += new CodeLibrary.Controls.DialogButton.DialogButtonClickEventHandler(this.DialogButton_DialogButtonClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CodeLibrary.Properties.Resources.Note;
            this.pictureBox1.Location = new System.Drawing.Point(5, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(34, 39);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1010, 665);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.dialogButton);
            this.Controls.Add(this.tbCode);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormAbout";
            ((System.ComponentModel.ISupportInitialize)(this.tbCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox tbCode;
        private DialogButton dialogButton;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}