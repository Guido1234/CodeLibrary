using CodeLibrary.Controls;

namespace EditorPlugins.Engine
{
    partial class PluginConfigurator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginConfigurator));
            this.buttonClose = new System.Windows.Forms.Button();
            this.pluginListBox = new CodeLibrary.Controls.CollectionListBox();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(541, 408);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(2);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 30);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // pluginListBox
            // 
            this.pluginListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pluginListBox.AskBeforeDelete = false;
            this.pluginListBox.CategoryProperty = null;
            this.pluginListBox.ColorProperty = null;
            this.pluginListBox.ColumnWidth = 300;
            this.pluginListBox.DefaultCategoryName = "No Category";
            this.pluginListBox.ImageProperty = null;
            this.pluginListBox.Location = new System.Drawing.Point(70, 8);
            this.pluginListBox.MultiSelect = false;
            this.pluginListBox.Name = "pluginListBox";
            this.pluginListBox.NameProperty = null;
            this.pluginListBox.ShowAdd = false;
            this.pluginListBox.ShowCopy = false;
            this.pluginListBox.ShowDelete = false;
            this.pluginListBox.ShowRefresh = false;
            this.pluginListBox.ShowSearch = false;
            this.pluginListBox.ShowToolstrip = false;
            this.pluginListBox.Size = new System.Drawing.Size(242, 372);
            this.pluginListBox.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.pluginListBox.TabIndex = 1;
            this.pluginListBox.ItemSelected += new System.EventHandler<CodeLibrary.Controls.CollectionListBox.CollectionListBoxEventArgs>(this.PluginListBox_ItemSelected);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(317, 8);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(2);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(299, 374);
            this.propertyGrid.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 395);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Changes will take place after restart.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::EditorPlugins.Engine.Properties.Resources.plugin_32x32;
            this.pictureBox1.Location = new System.Drawing.Point(11, 8);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 42);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // PluginConfigurator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 445);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.pluginListBox);
            this.Controls.Add(this.buttonClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PluginConfigurator";
            this.Text = "PluginConfigurator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private CodeLibrary.Controls.CollectionListBox pluginListBox;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}