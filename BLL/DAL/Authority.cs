using System;
using System.Collections.Generic;
using System.Text;

namespace MMShareBLL.DAL
{
   public class Authority
    {
        private string   m_data;
        private string m_function;
        public Authority()
        {

        }
        public Authority(string data, string function)
        {
            m_data = data;
            m_function = function;
        }
        public string data
        {
            get
            {
                return m_data;
            }
            set
            {
                this.m_data = value;
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string function
        {
            get
            {
                return m_function;
            }
            set
            {
                m_function = value;
            }
        }
    }
}
