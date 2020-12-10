using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CodeLibrary.Controls
{
    /// <summary>
    /// Control to show a list op Types and ability to create instances of types.
    /// </summary>
    public partial class TypeList : UserControl
    {
        public TypeList()
        {
            InitializeComponent();
        }

        public delegate void TypeListDoubleClickEventHandler(object sender, TypeListEventArgs ea);

        public event TypeListDoubleClickEventHandler OnTypeClick;

        public event TypeListDoubleClickEventHandler OnTypeDoubleClick;

        public enum EViewMode
        {
            ListView = 0,
            TreeView = 1
        }

        public bool AutoExpand { get; set; }
        public List<Type> Types { get; private set; }

        [Description("ListView or TreeView")]
        public EViewMode ViewMode { get; set; }

        public void Clear()
        {
            Types.Clear();
            listViewTypes.Items.Clear();
            listViewTypes.Groups.Clear();
            treeViewTypes.Nodes.Clear();
        }

        /// <summary>
        /// Fill CollectionTypes (AvailableTypes) based on a base type.
        /// </summary>
        public void FillCollectionTypes(Type pBaseType)
        {
            Types = GetObjectsWithBaseType(pBaseType);
            Refresh();
        }

        /// <summary>
        /// Fill CollectionTypes (AvailableTypes) based on multiple base types.
        /// </summary>
        public void FillCollectionTypes(params Type[] pBaseTypes)
        {
            Types = GetObjectsWithBaseType(pBaseTypes);
            Refresh();
        }

        public override void Refresh()
        {
            base.Refresh();
            if (ViewMode == EViewMode.ListView)
            {
                listViewTypes.Visible = true;
                treeViewTypes.Visible = false;
            }
            else
            {
                listViewTypes.Visible = true;
                treeViewTypes.Visible = false;
            }
            BuildAvailableTypeList();
        }

        #region Helpers

        public static bool DerivesFromBaseType(Type type, params Type[] basetypes)
        {
            if (basetypes == null)
                return false;

            foreach (Type t in basetypes)
            {
                if (DerivesFromBaseType(type, t))
                    return true;
            }

            return false;
        }

        public static bool DerivesFromBaseType(Type type, Type basetype)
        {
            if (type == basetype)
                return true;

            if (type == null)
                return false;

            if (type.BaseType != null)
            {
                if (type.BaseType == basetype)
                    return true;

                return DerivesFromBaseType(type.BaseType, basetype);
            }
            return false;
        }

        private static string GetCategory(Type type) => (type.GetCustomAttribute(typeof(CategoryAttribute)) as CategoryAttribute)?.Category;

        private static string GetDisplayName(Type type) => (type.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute)?.DisplayName;

        private static List<Type> GetObjectsWithBaseType(params Type[] basetypes)
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (DerivesFromBaseType(t, basetypes))
                        types.Add(t);
                }
            }
            return types;
        }

        #endregion Helpers

        private void BuildAvailableTypeList()
        {
            listViewTypes.Items.Clear();
            listViewTypes.Groups.Clear();
            treeViewTypes.Nodes.Clear();

            IEnumerable<Type> sortedTypes = from tp in Types
                                            orderby GetCategory(tp), GetDisplayName(tp) descending
                                            select tp;

            if (ViewMode == EViewMode.ListView)
            {
                foreach (Type t in sortedTypes)
                {
                    string cat = GetCategory(t);
                    if (string.IsNullOrEmpty(cat))
                    {
                        cat = "Misc";
                    }

                    string name = GetDisplayName(t);

                    ListViewGroup lvg = GetListViewGroup(cat, listViewTypes);
                    ListViewItem lvi = new ListViewItem
                    {
                        Name = name,
                        Text = name,
                        Tag = t,
                        Group = lvg
                    };
                    listViewTypes.Items.Add(lvi);
                }

                listViewTypes.BringToFront();
                listViewTypes.Visible = true;
            }
            else
            {
                foreach (Type t in Types)
                {
                    string cat = GetCategory(t);
                    if (string.IsNullOrEmpty(cat))
                        cat = "Misc";

                    string name = GetDisplayName(t);
                    TreeNode catNode = GetTreeViewCat(cat);
                    TreeNode typeNode = catNode.Nodes.Add(name, name, "Class", "Class");
                    if (AutoExpand)
                        catNode.ExpandAll();

                    typeNode.Tag = t;
                }
                this.treeViewTypes.BringToFront();
                this.treeViewTypes.Visible = true;
            }
        }

        private ListViewGroup GetListViewGroup(string pCategory, ListView pListView)
        {
            foreach (ListViewGroup lvg in pListView.Groups)
            {
                if (lvg.Name.Equals(pCategory))
                {
                    return lvg;
                }
            }

            ListViewGroup lvgNew = new ListViewGroup(pCategory, pCategory);
            pListView.Groups.Add(lvgNew);
            return lvgNew;
        }

        private TreeNode GetTreeViewCat(string pCatName)
        {
            foreach (TreeNode node in this.treeViewTypes.Nodes)
            {
                if (node.Name.Equals(pCatName, StringComparison.OrdinalIgnoreCase))
                    return node;
            }
            return treeViewTypes.Nodes.Add(pCatName, pCatName, "FolderClosed", "FolderOpen");
        }

        private void ListViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewMode = EViewMode.ListView;
            Refresh();
        }

        private void ListViewTypes_Click(object sender, EventArgs e)
        {
            if (listViewTypes.SelectedItems.Count == 1)
            {
                Type t = (Type)listViewTypes.SelectedItems[0].Tag;
                descriptionControl1.SetByType(t);
                if (OnTypeClick != null)
                {
                    TypeListEventArgs ea = new TypeListEventArgs(Activator.CreateInstance(t), t);
                    OnTypeClick(this, ea);
                }
            }
        }

        private void ListViewTypes_DoubleClick(object sender, EventArgs e)
        {
            if (listViewTypes.SelectedItems.Count == 1)
            {
                Type t = (Type)listViewTypes.SelectedItems[0].Tag;
                TypeListEventArgs ea = new TypeListEventArgs(Activator.CreateInstance(t), t);
                OnTypeDoubleClick?.Invoke(this, ea);
            }
        }

        private void ListViewTypes_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuViewMode.Show(Cursor.Position.X, Cursor.Position.Y);
        }

        private void ListViewTypes_Resize(object sender, EventArgs e)
        {
            listViewTypes.Columns[0].Width = listViewTypes.Width - 20;
        }

        private void TreeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewMode = EViewMode.TreeView;
            Refresh();
        }

        private void TreeViewTypes_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuViewMode.Show(Cursor.Position.X, Cursor.Position.Y);
        }

        private void TreeViewTypes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                Type t = (Type)e.Node.Tag;
                descriptionControl1.SetByType(t);
                if (OnTypeClick != null)
                {
                    TypeListEventArgs ea = new TypeListEventArgs(Activator.CreateInstance(t), t);
                    OnTypeClick(this, ea);
                }
            }
        }

        private void TreeViewTypes_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                Type t = (Type)e.Node.Tag;
                if (OnTypeDoubleClick != null)
                {
                    TypeListEventArgs ea = new TypeListEventArgs(Activator.CreateInstance(t), t);
                    OnTypeDoubleClick(this, ea);
                }
            }
        }

        public class TypeListEventArgs
        {
            public TypeListEventArgs(object pInstance, Type type)
            {
                Instance = pInstance;
                Type = type;
            }

            public object Instance { get; set; }
            public Type Type { get; set; }
        }
    }
}