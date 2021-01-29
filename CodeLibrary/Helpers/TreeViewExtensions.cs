using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeLibrary
{
    public static class TreeViewExtensions
    {
        public static TreeNode FindNodeByPath(this TreeNode parent, string fullpath)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                if (node.FullPath.Equals(fullpath, StringComparison.InvariantCultureIgnoreCase))
                {
                    return node;
                }
                var _newNode = node.FindNodeByPath(fullpath);
                if (_newNode != null)
                {
                    return _newNode;
                }
            }
            return null;
        }

        public static void GetAllChildNames(this TreeNode node, ref List<string> name)
        {
            foreach (TreeNode child in node.Nodes)
            {
                name.Add(child.Name);
                GetAllChildNames(child, ref name);
            }
        }

        public static TreeNode GetRootNode(this TreeNode node)
        {
            TreeNode root = node;
            if (root == null)
                return node;

            while (root.Parent != null)
            {
                if (root.Parent == null)
                    return root;

                root = root.Parent;
            }

            return root;
        }

        public static void MoveDown(this TreeNode node)
        {
            var view = node.TreeView;
            var _parentNodes = ParentNodes(node);

            int index = _parentNodes.IndexOf(node);
            if (index < _parentNodes.Count)
            {
                _parentNodes.RemoveAt(index);
                _parentNodes.Insert(index + 1, node);
            }

            view.SelectedNode = node;
        }

        public static void MoveLeft(this TreeNode node)
        {
            var view = node.TreeView;
            if (node != null)
            {
                view.BeginUpdate();
                if (node.Parent != null)
                {
                    var _parent = node.Parent;
                    if (_parent.Parent == null)
                    {
                        node.Remove();
                        view.Nodes.Insert(_parent.Index + 1, node);
                    }
                    else
                    {
                        node.Remove();
                        _parent.Parent.Nodes.Insert(_parent.Index + 1, node);
                    }
                }
                view.SelectedNode = node;
                view.EndUpdate();
            }
        }

        public static void MoveRight(this TreeNode node)
        {
            var view = node.TreeView;
            if (node != null)
            {
                view.BeginUpdate();
                if (node.PrevNode != null)
                {
                    var _newParent = node.PrevNode;
                    node.Remove();
                    _newParent.Nodes.Add(node);
                }
                view.SelectedNode = node;
                view.EndUpdate();
            }
        }

        public static void MoveToBottom(this TreeNode node)
        {
            var view = node.TreeView;
            var _parentNodes = ParentNodes(node);

            int index = _parentNodes.IndexOf(node);
            if (index > -1)
            {
                _parentNodes.RemoveAt(index);
                _parentNodes.Insert(_parentNodes.Count, node);
            }

            view.SelectedNode = node;
        }

        public static void MoveToTop(this TreeNode node)
        {
            var view = node.TreeView;
            var _parentNodes = ParentNodes(node);

            int index = _parentNodes.IndexOf(node);
            if (index < _parentNodes.Count)
            {
                _parentNodes.RemoveAt(index);
                _parentNodes.Insert(0, node);
            }

            view.SelectedNode = node;
        }

        public static void MoveUp(this TreeNode node)
        {
            var view = node.TreeView;
            var _parentNodes = ParentNodes(node);

            int index = _parentNodes.IndexOf(node);
            if (index > -1)
            {
                _parentNodes.RemoveAt(index);
                _parentNodes.Insert(index - 1, node);
            }

            view.SelectedNode = node;
        }

        public static int ParentCount(this TreeNode node)
        {
            if (node == null)
            {
                return 0;
            }
            return ParentNodes(node).Count;
        }

        public static TreeNodeCollection ParentNodes(this TreeNode node)
        {
            var parent = node.Parent;
            var view = node.TreeView;
            if (parent != null)
            {
                return parent.Nodes;
            }
            return view.Nodes;
        }

        public static List<TreeNode> ParentPath(this TreeNode node)
        {
            List<TreeNode> parents = new List<TreeNode>();
            while (node.Parent != null)
            {
                node = node.Parent;
                if (node != null)
                    parents.Add(node);
            }
            return parents;
        }
    }
}