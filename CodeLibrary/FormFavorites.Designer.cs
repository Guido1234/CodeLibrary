
namespace CodeLibrary
{
    partial class FormFavorites
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
            this.lbName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lsbItems = new CodeLibrary.Controls.CollectionListBox();
            this.dlgButton = new CodeLibrary.Controls.DialogButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.Location = new System.Drawing.Point(76, 15);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(73, 16);
            this.lbName.TabIndex = 8;
            this.lbName.Text = "Favorites";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CodeLibrary.Properties.Resources.star_32x32;
            this.pictureBox1.InitialImage = global::CodeLibrary.Properties.Resources.error_32x32;
            this.pictureBox1.Location = new System.Drawing.Point(11, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // lsbItems
            // 
            this.lsbItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsbItems.AskBeforeDelete = false;
            this.lsbItems.CategoryProperty = null;
            this.lsbItems.ColorProperty = null;
            this.lsbItems.ColumnWidth = 300;
            this.lsbItems.DefaultCategoryName = "No Category";
            this.lsbItems.ImageProperty = null;
            this.lsbItems.Location = new System.Drawing.Point(79, 38);
            this.lsbItems.MultiSelect = false;
            this.lsbItems.Name = "lsbItems";
            this.lsbItems.NameProperty = null;
            this.lsbItems.ShowAdd = true;
            this.lsbItems.ShowCopy = false;
            this.lsbItems.ShowDelete = true;
            this.lsbItems.ShowRefresh = false;
            this.lsbItems.ShowSearch = false;
            this.lsbItems.ShowToolstrip = true;
            this.lsbItems.Size = new System.Drawing.Size(579, 245);
            this.lsbItems.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.lsbItems.TabIndex = 0;
            // 
            // dlgButton
            // 
            this.dlgButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dlgButton.ButtonMode = CodeLibrary.Controls.DialogButton.DialogButtonMode.OkCancel;
            this.dlgButton.Location = new System.Drawing.Point(502, 307);
            this.dlgButton.Name = "dlgButton";
            this.dlgButton.Size = new System.Drawing.Size(156, 23);
            this.dlgButton.TabIndex = 9;
            this.dlgButton.TextCancel = "Cancel";
            this.dlgButton.TextIgnore = "Ignore";
            this.dlgButton.TextNo = "No";
            this.dlgButton.TextOk = "Ok";
            this.dlgButton.TextRetry = "Retry";
            this.dlgButton.TextYes = "Yes";
            this.dlgButton.DialogButtonClick += new CodeLibrary.Controls.DialogButton.DialogButtonClickEventHandler(this.dlgButton_DialogButtonClick);
            // 
            // FormFavorites
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 342);
            this.Controls.Add(this.dlgButton);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lsbItems);
            this.Name = "FormFavorites";
            this.Text = "Favorites";
            this.Load += new System.EventHandler(this.FormFavorites_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.CollectionListBox lsbItems;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Controls.DialogButton dlgButton;
    }
}