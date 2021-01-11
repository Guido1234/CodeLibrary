
namespace CodeLibrary
{
    partial class FormBackupRestore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBackupRestore));
            this.btCancel = new System.Windows.Forms.Button();
            this.btRestore = new System.Windows.Forms.Button();
            this.lbName = new System.Windows.Forms.Label();
            this.lbBackups = new CodeLibrary.Controls.CollectionListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.Location = new System.Drawing.Point(617, 391);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btRestore
            // 
            this.btRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btRestore.Enabled = false;
            this.btRestore.Location = new System.Drawing.Point(536, 391);
            this.btRestore.Name = "btRestore";
            this.btRestore.Size = new System.Drawing.Size(75, 23);
            this.btRestore.TabIndex = 2;
            this.btRestore.Text = "Restore";
            this.btRestore.UseVisualStyleBackColor = true;
            this.btRestore.Click += new System.EventHandler(this.btRestore_Click);
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(68, 9);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(51, 16);
            this.lbName.TabIndex = 3;
            this.lbName.Text = "NAME";
            // 
            // lbBackups
            // 
            this.lbBackups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBackups.AskBeforeDelete = false;
            this.lbBackups.CategoryProperty = null;
            this.lbBackups.ColorProperty = null;
            this.lbBackups.ColumnWidth = 383;
            this.lbBackups.DefaultCategoryName = "No Category";
            this.lbBackups.ImageProperty = null;
            this.lbBackups.Location = new System.Drawing.Point(71, 34);
            this.lbBackups.MultiSelect = false;
            this.lbBackups.Name = "lbBackups";
            this.lbBackups.NameProperty = null;
            this.lbBackups.ShowAdd = false;
            this.lbBackups.ShowCopy = false;
            this.lbBackups.ShowDelete = false;
            this.lbBackups.ShowRefresh = false;
            this.lbBackups.ShowSearch = false;
            this.lbBackups.ShowToolstrip = false;
            this.lbBackups.Size = new System.Drawing.Size(621, 333);
            this.lbBackups.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.lbBackups.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(71, 388);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(445, 41);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "Backup restore does not change the current file, it only loads the backup into me" +
    "mory.if you want to revert to original then exit without saving.\r\n";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CodeLibrary.Properties.Resources.backup_manager_32x32;
            this.pictureBox1.InitialImage = global::CodeLibrary.Properties.Resources.error_32x32;
            this.pictureBox1.Location = new System.Drawing.Point(8, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // FormBackupRestore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 426);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.btRestore);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.lbBackups);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormBackupRestore";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Backup Restore";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.CollectionListBox lbBackups;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btRestore;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}