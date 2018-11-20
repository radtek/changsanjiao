using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MMShareBLL.DAL
{
    public class TreeNode
    {
        public string id;
        public string text;
        public bool leaf;
        public string tag;
        public string aliasName;
        //public bool @checked;

        private IList<TreeNode> m_children;
        public TreeNode()
        {
            m_children = new List<TreeNode>();
        }
        public TreeNode(string id, string text, bool leaf, bool isChecked, string aliasName)
        {
            this.id = id;
            this.text = text;
            this.leaf = leaf;
            this.aliasName = aliasName;
            //this.@checked = isChecked;
            m_children = new List<TreeNode>();
        }
        public void appendChild(TreeNode node)
        {
            m_children.Add(node);
        }
    }
}
