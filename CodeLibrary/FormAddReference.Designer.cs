
namespace CodeLibrary
{
    partial class FormAddReference
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddReference));
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.dialogButton1 = new CodeLibrary.Controls.DialogButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbName = new System.Windows.Forms.Label();
            this.treeViewLibrary = new System.Windows.Forms.TreeView();
            this.buttonFind = new System.Windows.Forms.Button();
            this.textBoxFind = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
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
            this.lbName.Size = new System.Drawing.Size(128, 16);
            this.lbName.TabIndex = 13;
            this.lbName.Text = "Select Reference";
            // 
            // treeViewLibrary
            // 
            this.treeViewLibrary.AllowDrop = true;
            this.treeViewLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewLibrary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewLibrary.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewLibrary.HideSelection = false;
            this.treeViewLibrary.Indent = 20;
            this.treeViewLibrary.ItemHeight = 24;
            this.treeViewLibrary.LabelEdit = true;
            this.treeViewLibrary.Location = new System.Drawing.Point(71, 55);
            this.treeViewLibrary.Margin = new System.Windows.Forms.Padding(0);
            this.treeViewLibrary.Name = "treeViewLibrary";
            this.treeViewLibrary.Size = new System.Drawing.Size(405, 307);
            this.treeViewLibrary.TabIndex = 14;
            // 
            // buttonFind
            // 
            this.buttonFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFind.Image = global::CodeLibrary.Properties.Resources.find_16x16;
            this.buttonFind.Location = new System.Drawing.Point(439, 29);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(37, 23);
            this.buttonFind.TabIndex = 16;
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // textBoxFind
            // 
            this.textBoxFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFind.Location = new System.Drawing.Point(72, 29);
            this.textBoxFind.Name = "textBoxFind";
            this.textBoxFind.Size = new System.Drawing.Size(361, 22);
            this.textBoxFind.TabIndex = 15;
            // 
            // FormAddReference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 405);
            this.Controls.Add(this.treeViewLibrary);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.textBoxFind);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dialogButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddReference";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Note";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList Icons;
        private Controls.DialogButton dialogButton1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbName;
        internal System.Windows.Forms.TreeView treeViewLibrary;
        internal System.Windows.Forms.Button buttonFind;
        internal System.Windows.Forms.TextBox textBoxFind;
    }
}