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
            this.buttonClose = new System.Windows.Forms.Button();
            this.pluginListBox = new CollectionListBox();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(812, 627);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(113, 46);
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
            this.pluginListBox.DefaultCategoryName = "No Category";
            this.pluginListBox.ImageProperty = null;
            this.pluginListBox.Location = new System.Drawing.Point(13, 14);
            this.pluginListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pluginListBox.MultiSelect = false;
            this.pluginListBox.Name = "pluginListBox";
            this.pluginListBox.NameProperty = null;
            this.pluginListBox.ShowAdd = false;
            this.pluginListBox.ShowCopy = false;
            this.pluginListBox.ShowDelete = false;
            this.pluginListBox.ShowRefresh = false;
            this.pluginListBox.ShowSearch = false;
            this.pluginListBox.ShowToolstrip = false;
            this.pluginListBox.Size = new System.Drawing.Size(351, 573);
            this.pluginListBox.TabIndex = 1;
            this.pluginListBox.ItemSelected += new System.EventHandler<CollectionListBox.CollectionListBoxEventArgs>(this.PluginListBox_ItemSelected);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(371, 12);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(554, 575);
            this.propertyGrid.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 610);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(265, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Changes will take place after restart.";
            // 
            // PluginConfigurator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 685);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.pluginListBox);
            this.Controls.Add(this.buttonClose);
            this.Name = "PluginConfigurator";
            this.Text = "PluginConfigurator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private CodeLibrary.Controls.CollectionListBox pluginListBox;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Label label1;
    }
}