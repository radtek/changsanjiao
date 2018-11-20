using System;

namespace Readearth.Data.Entity
{
    /// <summary>
    /// 此类用于描述字段信息
    /// </summary>
    public class PropertyOV
    {
        private string m_Name;
        private string m_Alias;
        private bool m_IsEditable;
        private QueryTypeContants m_QueryType;
        private string m_DictName;
        private ShowTypeContants m_ShowType;
        private bool m_IsPK;
        private bool m_IsNullable;
        private int m_Length;
        private string m_DefaultValue;
        private string m_ShowValue;
        private string m_Value;
        private string m_EntityName;
        private FieldTypeContants m_FieldType;
        private string m_UpdatedValue;
        private string m_Link;
        private int m_OrderIndex;
        private bool m_IsEvent;
        private string m_YField;

        public string YField
        {
            get
            {
                return m_YField;
            }
            set
            {
                this.m_YField = value;
            }
        }
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }

        public string Alias
        {
            get
            {
                return m_Alias;
            }
            set
            {
                this.m_Alias = value;
            }
        }

        public string DefaultValue
        {
            get
            {
                return m_DefaultValue;
            }
            set
            {
                this.m_DefaultValue = value;
            }
        }

        public string DictName
        {
            get
            {
                return m_DictName;
            }
            set
            {
                this.m_DictName = value;
            }
        }

        public FieldTypeContants FieldType
        {
            get
            {
                return m_FieldType;
            }
            set
            {
                this.m_FieldType = value;
            }
        }

        public bool IsEditable
        {
            get
            {
                return m_IsEditable;
            }
            set
            {
                this.m_IsEditable = value;
            }
        }

        public bool IsNullable
        {
            get
            {
                return m_IsNullable;
            }
            set
            {
                this.m_IsNullable = value;
            }
        }

        public bool IsPK
        {
            get
            {
                return m_IsPK;
            }
            set
            {
                this.m_IsPK = value;
            }
        }

        public int Length
        {
            get
            {
                return m_Length;
            }
            set
            {
                this.m_Length = value;
            }
        }

        public string Link
        {
            get
            {
                return m_Link;
            }
            set
            {
                this.m_Link = value;
            }
        }

        public int OrderIndex
        {
            get
            {
                return m_OrderIndex;
            }
            set
            {
                this.m_OrderIndex = value;
            }
        }

        public QueryTypeContants QueryType
        {
            get
            {
                return m_QueryType;
            }
            set
            {
                this.m_QueryType = value;
            }
        }

        public ShowTypeContants ShowType
        {
            get
            {
                return m_ShowType;
            }
            set
            {
                this.m_ShowType = value;
            }
        }

        public string UpdatedValue
        {
            get
            {
                return m_UpdatedValue;
            }
            set
            {
                this.m_UpdatedValue = value;
            }
        }

        public string ShowValue
        {
            get
            {
                return m_ShowValue;
            }
            set
            {
                this.m_ShowValue = value;
            }
        }

        public string Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                this.m_Value = value;
            }
        }

        public string EntityName
        {
            get
            {
                return m_EntityName;
            }
            set
            {
                this.m_EntityName = value;
            }
        }

        public bool IsDictionary
        {
            get
            {
                return (m_DictName != "" || m_FieldType == FieldTypeContants.ET_DICT);
            }
           
        }

        //表明字段是否是事件字段，主要用于变动实体
        public bool IsEvent
        {
            get
            {
                return m_IsEvent;
            }
            set
            {
                this.m_IsEvent = value;
            }
        }


        public PropertyOV()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public PropertyOV Clone()
        {
            PropertyOV propertyOV = new PropertyOV();
            propertyOV.Alias = this.Alias;
            propertyOV.DefaultValue = this.DefaultValue;
            propertyOV.DictName = this.DictName;
            propertyOV.EntityName = this.EntityName;
            propertyOV.FieldType = this.FieldType;
            propertyOV.IsEditable = this.IsEditable;
            propertyOV.IsEvent = this.IsEvent;
            propertyOV.IsNullable = this.IsNullable;
            propertyOV.IsPK = this.IsPK;
            propertyOV.Length = this.Length;
            propertyOV.Link = this.Link;
            propertyOV.Name = this.Name;
            propertyOV.OrderIndex =this.OrderIndex;
            propertyOV.QueryType = this.QueryType;
            propertyOV.ShowType = this.ShowType;
            propertyOV.ShowValue = this.ShowValue;
            propertyOV.UpdatedValue = this.UpdatedValue;
            propertyOV.Value = this.Value;
            propertyOV.YField = this.YField;

            return propertyOV;

        }
    }
    //查询类型
    public enum QueryTypeContants
    {
        qtNotQuery, //不用于查询
        qtIndexQuery, //用于索引查询
        qtIntegrate //用于综合查询
    }

    //显示类型
    public enum ShowTypeContants
    {
        stNotShow,    //不显示
        stSimpleShow, //简要显示
        stDetailShow, //详细显示
        stIDShow      //标识显示
    }

    public enum FieldTypeContants
    {
        ET_IMAGE,
        ET_TEXT,
        ET_MEMO,
        ET_BOOL,
        ET_DICT,
        ET_STRINGDICT,
        ET_DATE,
        ET_TIME,
        ET_DATETIME,
        ET_FILE,
        ET_CLOB,
        ET_SFZHM,
        ET_INT,
        ET_FLOAT
    }
}