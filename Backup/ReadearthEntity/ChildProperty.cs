using System;

namespace Readearth.Data.Entity
{
    /// <summary>
    /// ChildProperty 的摘要说明
    /// </summary>
    public class ChildProperty
    {
        private string m_SubEnttName;
        private string m_SubEnttHint;
        private string m_RelType;
        private string m_Relation;

        public ChildProperty()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public string Relation
        {
            get
            {
                return m_Relation;
            }
            set
            {
                m_Relation = value;
            }
        }

        public string RelType
        {
            get
            {
                return m_RelType;
            }
            set
            {
                m_RelType = value;
            }
        }

        public string SubEnttHint
        {
            get
            {
                return m_SubEnttHint;
            }
            set
            {
                m_SubEnttHint = value;
            }
        }

        public string SubEnttName
        {
            get
            {
                return m_SubEnttName;
            }
            set
            {
                m_SubEnttName = value;
            }
        }
    }
}
