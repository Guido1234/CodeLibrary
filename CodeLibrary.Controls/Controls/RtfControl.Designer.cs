
namespace CodeLibrary.Controls.Controls
{
    partial class RtfControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RtfControl));
            this.rtf = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cmbFont = new System.Windows.Forms.ToolStripComboBox();
            this.cmbFontSize = new System.Windows.Forms.ToolStripComboBox();
            this.btFont = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btForeColor = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btItalic = new System.Windows.Forms.ToolStripButton();
            this.btUnderline = new System.Windows.Forms.ToolStripButton();
            this.btBold = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btAlignLeft = new System.Windows.Forms.ToolStripButton();
            this.btAlignCenter = new System.Windows.Forms.ToolStripButton();
            this.btAlignRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btBullets = new System.Windows.Forms.ToolStripButton();
            this.btIndentMin = new System.Windows.Forms.ToolStripButton();
            this.btIndentPlus = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cbStyles = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.updateStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.addStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.removeStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.btSwitchTheme = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtf
            // 
            this.rtf.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtf.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtf.HideSelection = false;
            this.rtf.Location = new System.Drawing.Point(0, 28);
            this.rtf.Name = "rtf";
            this.rtf.ShowSelectionMargin = true;
            this.rtf.Size = new System.Drawing.Size(816, 436);
            this.rtf.TabIndex = 0;
            this.rtf.Text = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Silver;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmbFont,
            this.cmbFontSize,
            this.btFont,
            this.toolStripSeparator1,
            this.btForeColor,
            this.toolStripSeparator5,
            this.btItalic,
            this.btUnderline,
            this.btBold,
            this.toolStripSeparator2,
            this.btAlignLeft,
            this.btAlignCenter,
            this.btAlignRight,
            this.toolStripSeparator3,
            this.btBullets,
            this.btIndentMin,
            this.btIndentPlus,
            this.toolStripSeparator4,
            this.cbStyles,
            this.toolStripDropDownButton1,
            this.btSwitchTheme});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(816, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cmbFont
            // 
            this.cmbFont.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmbFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFont.Name = "cmbFont";
            this.cmbFont.Size = new System.Drawing.Size(200, 23);
            // 
            // cmbFontSize
            // 
            this.cmbFontSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmbFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFontSize.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFontSize.Name = "cmbFontSize";
            this.cmbFontSize.Size = new System.Drawing.Size(75, 25);
            // 
            // btFont
            // 
            this.btFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btFont.Image = global::CodeLibrary.Controls.Properties.Resources.Fon1;
            this.btFont.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btFont.Name = "btFont";
            this.btFont.Size = new System.Drawing.Size(23, 20);
            this.btFont.Text = "Font";
            this.btFont.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btFont.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // btForeColor
            // 
            this.btForeColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btForeColor.Image = global::CodeLibrary.Controls.Properties.Resources.Color;
            this.btForeColor.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btForeColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btForeColor.Name = "btForeColor";
            this.btForeColor.Size = new System.Drawing.Size(23, 20);
            this.btForeColor.Text = "Color";
            this.btForeColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btForeColor.ToolTipText = "forecolor";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 23);
            // 
            // btItalic
            // 
            this.btItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btItalic.Image = global::CodeLibrary.Controls.Properties.Resources.Italic;
            this.btItalic.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btItalic.Name = "btItalic";
            this.btItalic.Size = new System.Drawing.Size(23, 20);
            this.btItalic.Text = "Italic";
            this.btItalic.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btUnderline
            // 
            this.btUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btUnderline.Image = global::CodeLibrary.Controls.Properties.Resources.Underline;
            this.btUnderline.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btUnderline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btUnderline.Name = "btUnderline";
            this.btUnderline.Size = new System.Drawing.Size(23, 20);
            this.btUnderline.Text = "Underline";
            this.btUnderline.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btUnderline.ToolTipText = "Underline";
            // 
            // btBold
            // 
            this.btBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btBold.Image = global::CodeLibrary.Controls.Properties.Resources.Bold;
            this.btBold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btBold.Name = "btBold";
            this.btBold.Size = new System.Drawing.Size(23, 20);
            this.btBold.Text = "bold";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // btAlignLeft
            // 
            this.btAlignLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAlignLeft.Image = global::CodeLibrary.Controls.Properties.Resources.AlignLeft;
            this.btAlignLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAlignLeft.Name = "btAlignLeft";
            this.btAlignLeft.Size = new System.Drawing.Size(23, 20);
            this.btAlignLeft.Text = "Align left";
            // 
            // btAlignCenter
            // 
            this.btAlignCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAlignCenter.Image = global::CodeLibrary.Controls.Properties.Resources.AlignCenter;
            this.btAlignCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAlignCenter.Name = "btAlignCenter";
            this.btAlignCenter.Size = new System.Drawing.Size(23, 20);
            this.btAlignCenter.Text = "Align center";
            // 
            // btAlignRight
            // 
            this.btAlignRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btAlignRight.Image = global::CodeLibrary.Controls.Properties.Resources.AlignRight;
            this.btAlignRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btAlignRight.Name = "btAlignRight";
            this.btAlignRight.Size = new System.Drawing.Size(23, 20);
            this.btAlignRight.Text = "Align right";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // btBullets
            // 
            this.btBullets.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btBullets.Image = global::CodeLibrary.Controls.Properties.Resources.bullets;
            this.btBullets.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btBullets.Name = "btBullets";
            this.btBullets.Size = new System.Drawing.Size(23, 20);
            this.btBullets.Text = "Bullets";
            // 
            // btIndentMin
            // 
            this.btIndentMin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btIndentMin.Image = global::CodeLibrary.Controls.Properties.Resources.DecreaseIndent;
            this.btIndentMin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btIndentMin.Name = "btIndentMin";
            this.btIndentMin.Size = new System.Drawing.Size(23, 20);
            this.btIndentMin.Text = "decrease indent";
            // 
            // btIndentPlus
            // 
            this.btIndentPlus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btIndentPlus.Image = global::CodeLibrary.Controls.Properties.Resources.IncreaseIndent;
            this.btIndentPlus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btIndentPlus.Name = "btIndentPlus";
            this.btIndentPlus.Size = new System.Drawing.Size(23, 20);
            this.btIndentPlus.Text = "increase indent";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 23);
            // 
            // cbStyles
            // 
            this.cbStyles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbStyles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStyles.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbStyles.Name = "cbStyles";
            this.cbStyles.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateStyle,
            this.addStyle,
            this.removeStyle});
            this.toolStripDropDownButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(50, 19);
            this.toolStripDropDownButton1.Text = "Styles";
            // 
            // updateStyle
            // 
            this.updateStyle.Image = global::CodeLibrary.Controls.Properties.Resources.update_16x16;
            this.updateStyle.Name = "updateStyle";
            this.updateStyle.Size = new System.Drawing.Size(145, 22);
            this.updateStyle.Text = "Update Style";
            // 
            // addStyle
            // 
            this.addStyle.Image = global::CodeLibrary.Controls.Properties.Resources.Add;
            this.addStyle.Name = "addStyle";
            this.addStyle.Size = new System.Drawing.Size(145, 22);
            this.addStyle.Text = "Add Style";
            // 
            // removeStyle
            // 
            this.removeStyle.Image = global::CodeLibrary.Controls.Properties.Resources.Erase;
            this.removeStyle.Name = "removeStyle";
            this.removeStyle.Size = new System.Drawing.Size(145, 22);
            this.removeStyle.Text = "Remove Style";
            // 
            // btSwitchTheme
            // 
            this.btSwitchTheme.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btSwitchTheme.Image = global::CodeLibrary.Controls.Properties.Resources._switch;
            this.btSwitchTheme.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btSwitchTheme.Name = "btSwitchTheme";
            this.btSwitchTheme.Size = new System.Drawing.Size(23, 20);
            this.btSwitchTheme.Text = "Switch Theme";
            // 
            // RtfControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.rtf);
            this.Name = "RtfControl";
            this.Size = new System.Drawing.Size(816, 464);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtf;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btItalic;
        private System.Windows.Forms.ToolStripButton btUnderline;
        private System.Windows.Forms.ToolStripButton btBold;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btAlignLeft;
        private System.Windows.Forms.ToolStripButton btAlignRight;
        private System.Windows.Forms.ToolStripButton btAlignCenter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btIndentPlus;
        private System.Windows.Forms.ToolStripButton btIndentMin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btForeColor;
        private System.Windows.Forms.ToolStripButton btFont;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripComboBox cbStyles;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem addStyle;
        private System.Windows.Forms.ToolStripMenuItem updateStyle;
        private System.Windows.Forms.ToolStripComboBox cmbFont;
        private System.Windows.Forms.ToolStripComboBox cmbFontSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem removeStyle;
        private System.Windows.Forms.ToolStripButton btBullets;
        private System.Windows.Forms.ToolStripButton btSwitchTheme;
    }
}
