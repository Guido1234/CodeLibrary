using DevToys;
using System.Windows.Forms;

namespace CodeLibrary.Core
{
    public class TreeNodeDictionaryList : DictionaryList<TreeNode, string>
    {
        public TreeNodeDictionaryList() : base(p => p.Name)
        {
        }

        public override void Add(TreeNode item)
        {
            if (ContainsKey(item.Name))
            {
                return;
            }
            base.Add(item);
        }

        public void Add(TreeView treeview)
        {
            Clear();
            foreach (TreeNode node in treeview.Nodes)
            {
                base.Add(node);
                Add(node.Nodes);
            }
        }

        private void Add(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                base.Add(node);
                Add(node.Nodes);
            }
        }
    }
}