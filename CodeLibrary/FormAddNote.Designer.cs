
namespace CodeLibrary
{
    partial class FormAddNote
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddNote));
            this.listViewTypes = new System.Windows.Forms.ListView();
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.cbRoot = new System.Windows.Forms.CheckBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.cbDefaultParent = new System.Windows.Forms.CheckBox();
            this.nuRepeat = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.dialogButton1 = new CodeLibrary.Controls.DialogButton();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nuRepeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewTypes
            // 
            this.listViewTypes.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.listViewTypes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewTypes.HideSelection = false;
            this.listViewTypes.LargeImageList = this.Icons;
            this.listViewTypes.Location = new System.Drawing.Point(68, 33);
            this.listViewTypes.MultiSelect = false;
            this.listViewTypes.Name = "listViewTypes";
            this.listViewTypes.Size = new System.Drawing.Size(408, 226);
            this.listViewTypes.TabIndex = 0;
            this.listViewTypes.UseCompatibleStateImageBehavior = false;
            // 
            // Icons
            // 
            this.Icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Icons.ImageStream")));
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            this.Icons.Images.SetKeyName(0, "c#");
            this.Icons.Images.SetKeyName(1, "html");
            this.Icons.Images.SetKeyName(2, "js");
            this.Icons.Images.SetKeyName(3, "lua");
            this.Icons.Images.SetKeyName(4, "php");
            this.Icons.Images.SetKeyName(5, "rtf");
            this.Icons.Images.SetKeyName(6, "sql");
            this.Icons.Images.SetKeyName(7, "txt");
            this.Icons.Images.SetKeyName(8, "vb");
            this.Icons.Images.SetKeyName(9, "xml");
            this.Icons.Images.SetKeyName(10, "template");
            this.Icons.Images.SetKeyName(11, "folder");
            // 
            // cbRoot
            // 
            this.cbRoot.AutoSize = true;
            this.cbRoot.Location = new System.Drawing.Point(68, 318);
            this.cbRoot.Name = "cbRoot";
            this.cbRoot.Size = new System.Drawing.Size(71, 17);
            this.cbRoot.TabIndex = 4;
            this.cbRoot.Text = "Add Root";
            this.cbRoot.UseVisualStyleBackColor = true;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(68, 265);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(233, 20);
            this.tbName.TabIndex = 0;
            this.tbName.Text = "New Note";
            // 
            // cbDefaultParent
            // 
            this.cbDefaultParent.AutoSize = true;
            this.cbDefaultParent.Location = new System.Drawing.Point(68, 341);
            this.cbDefaultParent.Name = "cbDefaultParent";
            this.cbDefaultParent.Size = new System.Drawing.Size(213, 17);
            this.cbDefaultParent.TabIndex = 6;
            this.cbDefaultParent.Text = "Set type as default child for parent Note";
            this.cbDefaultParent.UseVisualStyleBackColor = true;
            // 
            // nuRepeat
            // 
            this.nuRepeat.Location = new System.Drawing.Point(424, 266);
            this.nuRepeat.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuRepeat.Name = "nuRepeat";
            this.nuRepeat.Size = new System.Drawing.Size(52, 20);
            this.nuRepeat.TabIndex = 7;
            this.nuRepeat.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(374, 268);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Repeat:";
            // 
            // dialogButton1
            // 
            this.dialogButton1.ButtonMode = CodeLibrary.Controls.DialogButton.DialogButtonMode.OkCancel;
            this.dialogButton1.Location = new System.Drawing.Point(320, 365);
            this.dialogButton1.Name = "dialogButton1";
            this.dialogButton1.Size = new System.Drawing.Size(156, 23);
            this.dialogButton1.TabIndex = 3;
            this.dialogButton1.TextCancel = "Cancel";
            this.dialogButton1.TextIgnore = "Ignore";
            this.dialogButton1.TextNo = "No";
            this.dialogButton1.TextOk = "Ok";
            this.dialogButton1.TextRetry = "Retry";
            this.dialogButton1.TextYes = "Yes";
            this.dialogButton1.DialogButtonClick += new CodeLibrary.Controls.DialogButton.DialogButtonClickEventHandler(this.dialogButton1_DialogButtonClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 288);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "( use: {0} DateTime {1} NodeCount )";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CodeLibrary.Properties.Resources.page_add_32x32;
            this.pictureBox1.InitialImage = global::CodeLibrary.Properties.Resources.error_32x32;
            this.pictureBox1.Location = new System.Drawing.Point(11, 9);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(68, 9);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(75, 16);
            this.lbName.TabIndex = 13;
            this.lbName.Text = "New Note";
            // 
            // FormAddNote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 405);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nuRepeat);
            this.Controls.Add(this.cbDefaultParent);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.cbRoot);
            this.Controls.Add(this.dialogButton1);
            this.Controls.Add(this.listViewTypes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddNote";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Note";
            ((System.ComponentModel.ISupportInitialize)(this.nuRepeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewTypes;
        private System.Windows.Forms.ImageList Icons;
        private Controls.DialogButton dialogButton1;
        private System.Windows.Forms.CheckBox cbRoot;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.CheckBox cbDefaultParent;
        private System.Windows.Forms.NumericUpDown nuRepeat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbName;
    }
}