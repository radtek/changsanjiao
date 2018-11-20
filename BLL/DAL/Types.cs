using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.DAL
{
    public class Types
    {
        private string   m_name;
        private int m_collumn;
        private int m_counts;
        private string m_layers;
        public Types()
        {

        }
        public Types(string name, int collumn, int counts, string layers)
        {
            m_name = name;
            m_collumn =collumn;
            m_counts = counts;
            m_layers = layers;
        }
        /// <summary>
        /// 类型名
        /// </summary>
        public string name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }
        /// <summary>
        /// 列数
        /// </summary>
        public int collumn
        {
            get
            {
                return m_collumn;
            }
            set
            {
                m_collumn = value;
            }
        }
        /// <summary>
        /// 显示个数
        /// </summary>
        public int counts
        {
            get
            {
                return m_counts;
            }
            set
            {
                m_counts = value;
            }
        }
        /// <summary>
        /// 图层列表
        /// </summary>
        public string layers
        {
            get
            {
                return m_layers;
            }
            set
            {
                m_layers = value;
            }
        }
    }
}
