namespace CodeLibrary.Controls
{
    partial class TypeList
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TypeList));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listViewTypes = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.treeViewTypes = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.descriptionControl1 = new DescriptionControl();
            this.contextMenuViewMode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.listViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuViewMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listViewTypes);
            this.splitContainer1.Panel1.Controls.Add(this.treeViewTypes);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.descriptionControl1);
            this.splitContainer1.Size = new System.Drawing.Size(284, 524);
            this.splitContainer1.SplitterDistance = 413;
            this.splitContainer1.TabIndex = 0;
            // 
            // listViewTypes
            // 
            this.listViewTypes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewTypes.FullRowSelect = true;
            this.listViewTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewTypes.HideSelection = false;
            this.listViewTypes.LabelWrap = false;
            this.listViewTypes.Location = new System.Drawing.Point(0, 0);
            this.listViewTypes.MultiSelect = false;
            this.listViewTypes.Name = "listViewTypes";
            this.listViewTypes.ShowItemToolTips = true;
            this.listViewTypes.Size = new System.Drawing.Size(284, 413);
            this.listViewTypes.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.listViewTypes.TabIndex = 17;
            this.listViewTypes.UseCompatibleStateImageBehavior = false;
            this.listViewTypes.View = System.Windows.Forms.View.Details;
            this.listViewTypes.Resize += new System.EventHandler(this.ListViewTypes_Resize);
            this.listViewTypes.DoubleClick += new System.EventHandler(this.ListViewTypes_DoubleClick);
            this.listViewTypes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListViewTypes_MouseUp);
            this.listViewTypes.Click += new System.EventHandler(this.ListViewTypes_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Command";
            this.columnHeader1.Width = 175;
            // 
            // treeViewTypes
            // 
            this.treeViewTypes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTypes.ImageIndex = 0;
            this.treeViewTypes.ImageList = this.imageList1;
            this.treeViewTypes.ItemHeight = 18;
            this.treeViewTypes.Location = new System.Drawing.Point(0, 0);
            this.treeViewTypes.Name = "treeViewTypes";
            this.treeViewTypes.SelectedImageIndex = 0;
            this.treeViewTypes.Size = new System.Drawing.Size(284, 413);
            this.treeViewTypes.TabIndex = 16;
            this.treeViewTypes.Visible = false;
            this.treeViewTypes.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewTypes_NodeMouseDoubleClick);
            this.treeViewTypes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TreeViewTypes_MouseUp);
            this.treeViewTypes.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewTypes_NodeMouseClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "FolderClosed");
            this.imageList1.Images.SetKeyName(1, "FolderOpen");
            this.imageList1.Images.SetKeyName(2, "Class");
            // 
            // descriptionControl1
            // 
            this.descriptionControl1.Caption = "...";
            this.descriptionControl1.Description = "";
            this.descriptionControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionControl1.Location = new System.Drawing.Point(0, 0);
            this.descriptionControl1.Margin = new System.Windows.Forms.Padding(0);
            this.descriptionControl1.Name = "descriptionControl1";
            this.descriptionControl1.Size = new System.Drawing.Size(284, 107);
            this.descriptionControl1.TabIndex = 1;
            // 
            // contextMenuViewMode
            // 
            this.contextMenuViewMode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listViewToolStripMenuItem,
            this.treeViewToolStripMenuItem});
            this.contextMenuViewMode.Name = "contextMenuViewMode";
            this.contextMenuViewMode.Size = new System.Drawing.Size(126, 48);
            // 
            // listViewToolStripMenuItem
            // 
            this.listViewToolStripMenuItem.Name = "listViewToolStripMenuItem";
            this.listViewToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.listViewToolStripMenuItem.Text = "List View";
            this.listViewToolStripMenuItem.Click += new System.EventHandler(this.ListViewToolStripMenuItem_Click);
            // 
            // treeViewToolStripMenuItem
            // 
            this.treeViewToolStripMenuItem.Name = "treeViewToolStripMenuItem";
            this.treeViewToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.treeViewToolStripMenuItem.Text = "Tree View";
            this.treeViewToolStripMenuItem.Click += new System.EventHandler(this.TreeViewToolStripMenuItem_Click);
            // 
            // TypeList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TypeList";
            this.Size = new System.Drawing.Size(284, 524);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuViewMode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DescriptionControl descriptionControl1;
        private System.Windows.Forms.TreeView treeViewTypes;
        private System.Windows.Forms.ListView listViewTypes;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuViewMode;
        private System.Windows.Forms.ToolStripMenuItem listViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem treeViewToolStripMenuItem;
    }
}
