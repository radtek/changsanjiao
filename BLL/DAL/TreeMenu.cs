using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

using Readearth.Data;

namespace MMShareBLL.DAL
{
    class TreeMenu
    {
        Database m_Database;
       
        public TreeMenu()
        {
            m_Database = new Database();
        }

        public TreeMenu(Database db)
        {
            m_Database = db;
        }

        /// <summary>
        /// 根据节点名称获取子菜单
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IList<TreeNode> GetList(string node)
        {
            bool blnLeaf;
            IList<TreeNode> tree = new List<TreeNode>();

            blnLeaf = true;
            string strSQL = "SELECT NAME,URL,CLASS FROM T_TREEMENU WHERE MODULENAME = '" + node + "' ORDER BY CLASS";
            if (node.Contains("|"))
            {
                string[] strElements = node.Split('|');
                strSQL = "SELECT NAME,URL,CLASS = NULL FROM T_TREEMENU WHERE MODULENAME = '" + strElements[0] + "' AND CLASS = '" + strElements[1] + "'";
            }
            SqlDataReader drProduct = m_Database.GetDataReader(strSQL);
            if (drProduct.HasRows)
            {
                while (drProduct.Read())
                {
                    TreeNode treeNode = new TreeNode();
                    if (drProduct.IsDBNull(2))
                    {
                        treeNode.id = drProduct.GetString(1);
                        treeNode.text = drProduct.IsDBNull(0) ? "" : drProduct.GetString(0);
                        treeNode.leaf = blnLeaf;
                    }
                    else
                    {
                        treeNode.id = node + "|" + drProduct.GetString(2);
                        treeNode.text = drProduct.GetString(2);
                        while (drProduct.GetString(2) == treeNode.text)
                        {
                            if (!drProduct.Read())
                                break;
                        }

                    }
                    tree.Add(treeNode);

                }
            }
            drProduct.Close();
            return tree;
        }
    }
}
