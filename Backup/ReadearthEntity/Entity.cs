using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Readearth.Data.Entity
{
    /// <summary>
    /// 实体类，用于描述一个实体，或者对实体进行相关的增、删、改、查等操作
    /// 作者：张伟锋              日期：2007年4月17日             最后修改日期：2007年4月17日
    /// </summary>
    public class Entity
    {
        private string m_EntityName;//实体名称
        private string m_Hint;//实体中文名称
        private string m_TableName;//实体所对应的表名
        private string m_Condition;//实体的基层条件
        private bool m_SaveHistory;//表明实体在编辑过程中是否保留历史轨迹
        private bool m_Returned;//表明在编辑过程中是否返回编辑过的记录


        private EntityStateContants m_EntityState;//实体状态,0－查询；1－插入；2－变更
        private IList m_Properties;//实体的属性
        private Database m_Database;//实体所在的数据库
        private string m_JoinSQL;//字典关联语句
        private IList m_SubEntities;//子实体集合

        private PropertyOV m_RelateFrom;
        private PropertyOV m_RelateTo;
        private string m_RelType;
        private string m_Relation;

        private Entity m_History;//历史实体
        private string m_HistoryEntity;//历史实体名称
        private OperatorTypeContants m_OperatorType;//实体操作类型

        private User m_User;//操作此实体的用户
        private string[] m_OrderFields;

        private string m_SQL;//SQL操作语句
        private int m_PrimaryIndex;//关键字的列号

        private bool m_Initialized = false;//标识初始化是否成功
        public Entity(Database db, string entityName)
        {
            m_EntityName = entityName;
            m_EntityState = EntityStateContants.esQuery;

            m_Database = db;

            //初始化实体概要信息
            InitEntityProperty();

            //实体存在则进行另外的信息的初始化
            if (m_Initialized)
            {
                //初始化实体字段信息
                InitEntityFields();
                //初始化子实体信息
                InitSubEntity();
                //初始化历史实体
                if (m_HistoryEntity != "" && m_HistoryEntity != null)
                    m_History = new Entity(m_Database, m_HistoryEntity);
            }
        }

        
        #region 属性
        public string SQL
        {
            get
            {
                return m_SQL;
            }
        }

        public int PrimaryIndex
        {
            get
            {
                if (m_PrimaryIndex == 0)
                    GetPrimaryField();
                return m_PrimaryIndex;
            }
        }
        public string JoinSQL
        {
            get
            {
                return m_JoinSQL;
            }
            set
            {
                m_JoinSQL = value;
            }
        }

        public string TableName
        {
            get
            {
                return m_TableName;
            }
            set
            {
                m_TableName = value;
            }
        }

        public bool SaveHistory
        {
            get
            {
                return m_SaveHistory;
            }
            set
            {
                m_SaveHistory = value;
            }
        }

        public bool Returned
        {
            get
            {
                return m_Returned;
            }
            set
            {
                m_Returned = value;
            }
        }

        public string Name
        {
            get
            {
                return m_EntityName;
            }
            //set
            //{
            //    m_EntityName = value;
            //}
        }
        public OperatorTypeContants OperatorType
        {
            get
            {
                return m_OperatorType;
            }
            //set
            //{
            //    m_EntityName = value;
            //}
        }
        
        public string Hint
        {
            get
            {
                return m_Hint;
            }
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

        public PropertyOV RelateTo
        {
            get
            {
                return m_RelateTo;
            }
            set
            {
                m_RelateTo = value;
            }
        }

        public PropertyOV RelateFrom
        {
            get
            {
                return m_RelateFrom;
            }
            set
            {
                m_RelateFrom = value;
            }
        }

        public EntityStateContants EntityState
        {
            get
            {
                return m_EntityState;
            }
            set
            {
                m_EntityState = value;
            }
        }

        public Database Database
        {
            get
            {
                return m_Database;
            }
            set
            {
                m_Database = value;
            }
        }


        public User User
        {
            get
            {
                return m_User ;
            }
            set
            {
                m_User = value;
            }
        }

        public Entity History
        {
            get
            {
                return m_History;
            }
        }

        public bool Initialized
        {
            get
            {
                return m_Initialized;
            }
        }

        public string Condition
        {
            get
            {
                return m_Condition;
            }
        }
        #endregion

        #region 公有函数

       

        public DataSet Submit(FilterOV filterOV)
        {
            int recordsAffected = 0;
            return Submit(filterOV, ref recordsAffected);
        }

       
        /// <summary>
        /// 提交操作SQL，返回记录集，并通过recordsAffected可以得知影响的行数
        /// </summary>
        /// <param name="filterOV">被提交的过虑对象</param>
        /// <param name="recordsAffected">受影响的行数</param>
        /// <returns>符合条件的记录集</returns>
        public DataSet Submit(FilterOV filterOV, ref int recordsAffected)
        {
            string strFilterMessage ="";
            DataSet dataSet = null;
            //返回操作语句
            if (filterOV != null)
            {
                strFilterMessage = Parse(filterOV);
                strFilterMessage = strFilterMessage.Trim();
            }

            //根据状态，进行相应操作
            switch (m_EntityState)
            {
                case EntityStateContants.esQuery://查询
                    {
                        dataSet = Query(strFilterMessage, "");
                        break;
                    }
                case EntityStateContants.esInsert://插入
                    {
                        dataSet = Insert(strFilterMessage, ref recordsAffected);
                        break;
                    }
                case EntityStateContants.esUpdate://更新
                    {
                        dataSet = Update(strFilterMessage, ref recordsAffected);
                        break;
                    }
                case EntityStateContants.esDelete://删除
                    {
                        recordsAffected = Delete(strFilterMessage);
                        break;
                    }
            }
            return dataSet;
        }


        /// <summary>
        /// 返回主键字段属性结构,主键可能有两个或者以上
        /// </summary>
        /// <returns>一个PropertyOV列表</returns>
        public IList GetPrimaryField()
        {
            IList primaryFields = new ArrayList();
            m_PrimaryIndex = 0;
            foreach (PropertyOV fieldValue in m_Properties)
            {
                if (fieldValue.IsPK)
                {
                    m_PrimaryIndex += 1;
                    primaryFields.Add(fieldValue);
                }
            }

            return primaryFields;
        }

        public DataSet Query(string whereCause)
        {
            return Query(whereCause, "");
        }

        public DataSet Query(string whereCause, string joinField)
        {
            //string strSQL = "";
            //string strDisplayFields = "";
            //int orderCount;
            //string strCondition = "";
            //string strWhereCause = whereCause;

            ////初始化排序字段
            //strSQL = "SELECT COUNT(*) FROM T_FIELDINFO WHERE ORDERINDEX >0 AND ENTITYNAME = '" + m_EntityName + "'";
            //orderCount = int.Parse(m_Database.GetFirstValue(strSQL));
            //if (orderCount > 0)
            //    m_OrderFields = new string[orderCount];

            //strDisplayFields = GetDisplayFields(true);

            
            ////增加数据权限控制，通过实体的Condition和当前登陆用户的数据权限
            //if (m_User != null)
            //{
            //    if (m_Condition != "" && m_User.Class != "")
            //        strCondition = m_Condition.Replace("@X", "'" + m_User.Class + "'");
            //    if (whereCause != "")
            //        strWhereCause = whereCause + " AND " + strCondition;
            //    else
            //        strWhereCause = strCondition;
            //}

            //if (strWhereCause != "")
            //    strSQL = "SELECT " + strDisplayFields + " FROM " + m_TableName + " " + m_JoinSQL + " " + joinField + " WHERE " + strWhereCause;
            //else
            //    strSQL = "SELECT " + strDisplayFields + " FROM " + m_TableName + " " + m_JoinSQL + " " + joinField;

            ////生成排序SQL语句
            //if (orderCount > 0)
            //{
            //    strSQL = strSQL + " ORDER BY ";
            //    for (int i = 0; i < orderCount; i++)
            //        strSQL = strSQL + m_OrderFields[i] + ", ";
            //    strSQL = strSQL.Substring(0, strSQL.Length - 2);
            //}
            m_SQL = BuildQuerySQL(whereCause,joinField);
            return m_Database.GetDataset(m_SQL);
        }

        /// <summary>
        /// 清空字段中的显示值
        /// </summary>
        public void ClearFields()
        {
            if (m_Properties != null)
            {
                foreach (PropertyOV propertyOV in m_Properties)
                    propertyOV.ShowValue = "";
            }
        }

        /// <summary>
        /// 根据输入的字段的名称和别名，返回字段属性结构
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public PropertyOV GetPropertyOV(string fieldName)
        {
            if (m_Properties != null)
            {
                foreach (PropertyOV propertyOV in m_Properties)
                    if (propertyOV.Name.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase) || propertyOV.Alias.Equals(fieldName))
                        return propertyOV;
            }
            return null;
        }

        /// <summary>
        /// 获取字段属性信息
        /// </summary>
        /// <returns></returns>
        public IList GetProperties()
        {
            return m_Properties;
        }

        /// <summary>
        /// 返回符合条件的字段集合
        /// </summary>
        /// <param name="entityState"></param>
        /// <param name="queryType"></param>
        /// <returns></returns>
        public IList GetProperties(EntityStateContants entityState, QueryTypeContants queryType)
        {
            IList validFields = new ArrayList();
            foreach (PropertyOV fieldValue in m_Properties)
            {
                
                if (CheckProperty(fieldValue,entityState,queryType))
                {
                    validFields.Add(fieldValue);
                }
            }
            return validFields;
        }

        /// <summary>
        /// 返回实体中显示的字段
        /// </summary>
        /// <param name="showType"></param>
        /// <returns></returns>
        public IList GetDisplayFields(ShowTypeContants showType)
        {
            IList validFields = new ArrayList();
            foreach (PropertyOV fieldValue in m_Properties)
            {

                if (fieldValue.ShowType == showType)
                {
                    validFields.Add(fieldValue);
                }
            }
            return validFields;
        }
        /// <summary>
        /// 获取事件字段列表
        /// </summary>
        /// <returns></returns>
        public IList GetEventFields()
        {
            IList eventFields = new ArrayList();
            foreach (PropertyOV fieldValue in m_Properties)
            {
                if (fieldValue.IsEvent)
                {
                    eventFields.Add(fieldValue);
                }
            }

            return eventFields;
        }

    
        /// <summary>
        /// 为每个属性分配值
        /// </summary>
        public bool AssignProperties()
        {
            string strSQL = "";
            string strFields = "";
            bool assignSuccess = false;
            //获取主键，生成条件
            IList primaryFields = GetPrimaryField();
            foreach (PropertyOV propertyOV in primaryFields)
            {
                BuildRealValue(propertyOV);
                strFields = strFields + propertyOV.Name + ",";
                strSQL = strSQL + propertyOV.Name + " = '" + propertyOV.Value + "' AND ";
            }
            strFields = strFields.Substring(0, strFields.Length - 1);
            strSQL = strSQL.Substring(0, strSQL.Length - 4);

            //查询需要修改的记录
            strSQL = "SELECT * FROM " + m_TableName + " WHERE " + strSQL;
            SqlDataReader sdrUpdated = m_Database.GetDataReader(strSQL);
            if (sdrUpdated.Read())
            {
                foreach (PropertyOV propertyOV in m_Properties)
                {
                    propertyOV.Value = sdrUpdated[propertyOV.Name].ToString();
                    BuildShowValue(propertyOV);
                }
                assignSuccess = true;
            }
            sdrUpdated.Close();
            return assignSuccess;
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 根据当前的状态，判断当前字段是否符合条件
        /// </summary>
        /// <param name="propertyOV">待判定的字段</param>
        /// <param name="entityState">当前实体的状态</param>
        /// <param name="queryType">查询类型</param>
        /// <returns></returns>
        private bool CheckProperty(PropertyOV propertyOV, EntityStateContants entityState, QueryTypeContants queryType)
        {
            bool validCheck = false;
            if (entityState == EntityStateContants.esQuery)
            {
                validCheck = (propertyOV.QueryType == queryType);
            }
            else if (entityState == EntityStateContants.esInsert || entityState == EntityStateContants.esUpdate)
            {
                validCheck = propertyOV.IsEditable;
            }

            return validCheck;
        }
        /// <summary>
        /// 获取实体的概要信息，包括实体所对应的表、实体中文名称、实体条件等
        /// </summary>
        private void InitEntityProperty()
        {
            string strSQL = "SELECT * FROM T_ENTITY WHERE ENTITYNAME = '" + m_EntityName + "'";
            SqlDataReader dataReader = m_Database.GetDataReader(strSQL);

            if (dataReader.HasRows)
            {
                if (dataReader.Read())
                {
                    m_Hint = dataReader.GetString(1);
                    m_TableName = dataReader.GetString(2);
                    m_Condition = dataReader.IsDBNull(3) ? "" : dataReader.GetString(3);
                    m_HistoryEntity = dataReader.IsDBNull(4) ? "" : dataReader.GetString(4);
                    m_OperatorType = dataReader.IsDBNull(5) ? OperatorTypeContants.otNone : (OperatorTypeContants)dataReader.GetInt32(5);
                    m_Initialized = true;
                }
            }
            dataReader.Close();
        }

        /// <summary>
        /// 初始化实体的字段属性
        /// </summary>
        private void InitEntityFields()
        {
            string strSQL = "SELECT * FROM T_FIELDINFO WHERE ENTITYNAME = '" + m_EntityName + "' ORDER BY ID";
            SqlDataReader dataReader = m_Database.GetDataReader(strSQL);
            PropertyOV propertyOV;

            if (dataReader.HasRows)
            {
                m_Properties = new ArrayList();
                while (dataReader.Read())
                {
                    propertyOV = new PropertyOV();
                    propertyOV.EntityName = dataReader.GetString(1);
                    propertyOV.Name = dataReader.GetString(2);
                    propertyOV.Alias = dataReader.GetString(3);
                    propertyOV.QueryType = (QueryTypeContants)Int32.Parse(dataReader.GetString(4));
                    propertyOV.IsEditable = dataReader.GetBoolean(5);
                    propertyOV.ShowType = (ShowTypeContants)Int32.Parse(dataReader.GetString(6));
                    propertyOV.IsPK = dataReader.GetBoolean(7);
                    propertyOV.DictName = dataReader.IsDBNull(8) ? "" :dataReader.GetString(8);
                    propertyOV.Length = dataReader.GetInt32(9);
                    propertyOV.DefaultValue = dataReader.IsDBNull(10) ? "" : dataReader.GetString(10);
                    propertyOV.IsNullable = dataReader.GetBoolean(11);
                    propertyOV.FieldType = (FieldTypeContants)Int32.Parse(dataReader.GetString(12));
                    propertyOV.OrderIndex = dataReader.IsDBNull(13) ? 0 : dataReader.GetInt32(13);
                    propertyOV.IsEvent = dataReader.IsDBNull(16) ? false : dataReader.GetBoolean(16);
                    propertyOV.YField = dataReader.IsDBNull(15) ? "" : dataReader.GetString(15);
                    m_Properties.Add(propertyOV);
                }
            }
            dataReader.Close();
        }

        /// <summary>
        /// 初始化子实体
        /// </summary>
        private void InitSubEntity()
        {
            string strSQL = "SELECT * FROM T_SubEntities WHERE ENTITYNAME = '" + m_EntityName + "'";
            SqlDataReader dataReader = m_Database.GetDataReader(strSQL);
            ChildProperty childProperty;
            if (dataReader.HasRows)
            {
                m_SubEntities = new ArrayList();
                while (dataReader.Read())
                {
                    childProperty = new ChildProperty();

                    childProperty.SubEnttName = dataReader.GetString(1);
                    childProperty.SubEnttHint = dataReader.GetString(2);
                    childProperty.RelType = dataReader.GetString(3);
                    childProperty.Relation = dataReader.GetString(4);

                    m_SubEntities.Add(childProperty);
                }
            }
            dataReader.Close();
        }

        /// <summary>
        /// 根据实体的状态，解析字段值SQL语句
        /// </summary>
        /// <param name="filterOV">被提交的过虑对象</param>
        /// <returns>标准SQL语句</returns>
        private string Parse(FilterOV filterOV)
        {
            string strSQL = "";
            //根据状态，进行相应操作
            switch (m_EntityState)
            {
                case EntityStateContants.esQuery://查询
                    {
                        strSQL = BuildQuerySQL(filterOV);
                        break;
                    }
                case EntityStateContants.esInsert://插入
                    {
                        strSQL = BuildInsertSQL(filterOV);
                        break;
                    }
                case EntityStateContants.esUpdate://更新
                    {
                        strSQL = BuildUpdateSQL(filterOV);
                        break;
                    }
                case EntityStateContants.esDelete://删除
                    {
                        strSQL = BuildDeleteSQL();
                        break;
                    }
            } 
            return strSQL;
        }

        /// <summary>
        /// 生成查询sql语句
        /// </summary>
        /// <param name="filterOV">被提交的过虑对象</param>
        /// <returns>标准SQL语句</returns>
        public string BuildQuerySQL(FilterOV filterOV)
        {
            string strSQL = "";
            string strDicWhere;
            string[] strDates;
            string strValue;
            string[] stringSeparators = new string[] { "||" };
            
            PropertyOV fieldValue;
            for(int i=0;i<filterOV.Count;i++)
            {
                fieldValue = filterOV[i];
                BuildRealValue(fieldValue);//获取真实值
                 
                if (fieldValue.Value != null && fieldValue.Value != "")
                {
                    //日期
                    if (fieldValue.FieldType == FieldTypeContants.ET_DATETIME || fieldValue.FieldType == FieldTypeContants.ET_DATE || fieldValue.FieldType == FieldTypeContants.ET_TIME)
                    {                       
                        if (fieldValue.Value != "||")
                        {
                            strValue = fieldValue.Value;
                            strDates = strValue.Split(stringSeparators, StringSplitOptions.None);
                            if (strDates.Length == 2)
                            {
                                if (strDates[0] != "" && strDates[1] == "")
                                    strSQL = strSQL + fieldValue.Name + " > ='" + strDates[0] + "'";
                                else if (strDates[0] == "" && strDates[1] != "")
                                    strSQL = strSQL + fieldValue.Name + " <='" + strDates[1] + "'";
                                else
                                    strSQL = strSQL + fieldValue.Name + " BETWEEN '" + strDates[0] + "' AND '" + strDates[1] + "'";
                            }else
                                strSQL = strSQL + fieldValue.Name + " ='" + strDates[0] + "'";
                        }
                    }
                    else if (fieldValue.IsDictionary)//字典
                    {
                        strDicWhere = fieldValue.Name + " ='" + fieldValue.Value + "'";
                        
                        int j = i;
                        for (j = i + 1; j < filterOV.Count; j++)
                        {
                            fieldValue = filterOV[j];
                            BuildRealValue(fieldValue);//获取真实值
                            if (filterOV[j].IsDictionary && fieldValue.Name == filterOV[i].Name)
                                strDicWhere = strDicWhere + " OR " + fieldValue.Name + " ='" + fieldValue.Value + "'";
                            else
                                strDicWhere = strDicWhere + " AND " + fieldValue.Name + " ='" + fieldValue.Value + "'";

                        }
                        i = j;
                         
                        strSQL = strSQL + "(" + strDicWhere + ")";

                    }
                    else
                    {
                        strValue = fieldValue.Value;
                        if (strValue.IndexOf("*") != -1 || strValue.IndexOf("?") != -1)
                        {
                            strValue = strValue.Replace("?", "_");
                            strValue = strValue.Replace("*", "%");
                            strSQL = strSQL + fieldValue.Name + " LIKE '" + strValue + "'";
                        }
                        else
                            strSQL = strSQL + fieldValue.Name + " = '" + fieldValue.Value + "'";
                    }
                    if (strSQL != "")
                    {
                        strSQL = strSQL + " AND ";
                    }
                }
            }

            //生成最终SQL语句
            if (strSQL != "")
            {
                strSQL = strSQL.Substring(0, strSQL.Length - 4);
            }
            return strSQL;
        }
        /// <summary>
        /// 根据传入的where语句，生成查询SQL
        /// </summary>
        /// <param name="whereCause"></param>
        /// <returns></returns>
        public string BuildQuerySQL(string whereCause,string joinField)
        {
            string strSQL = "";
            string strDisplayFields = "";
            int orderCount;
            string strCondition = "";
            string strWhereCause = whereCause;

            //初始化排序字段
            strSQL = "SELECT COUNT(*) FROM T_FIELDINFO WHERE ORDERINDEX >0 AND ENTITYNAME = '" + m_EntityName + "'";
            orderCount = int.Parse(m_Database.GetFirstValue(strSQL));
            if (orderCount > 0)
                m_OrderFields = new string[orderCount];

            strDisplayFields = GetDisplayFields(true);


            //增加数据权限控制，通过实体的Condition和当前登陆用户的数据权限
            if (m_User != null)
            {
                if (m_Condition != "" && m_User.Class != "")
                    strCondition = m_Condition.Replace("@X", "'" + m_User.Class + "'");
                if (whereCause != "")
                    strWhereCause = whereCause + " AND " + strCondition;
                else
                    strWhereCause = strCondition;
            }

            if (strWhereCause != "")
                strSQL = "SELECT " + strDisplayFields + " FROM " + m_TableName + " " + m_JoinSQL + " " + joinField + " WHERE " + strWhereCause + "AND Flag='Former'";
            else
                strSQL = "SELECT " + strDisplayFields + " FROM " + m_TableName + " " + m_JoinSQL + " " + joinField+ " WHERE AND Flag='Former'";

            //生成排序SQL语句
            if (orderCount > 0)
            {
                strSQL = strSQL + " ORDER BY ";
                for (int i = 0; i < orderCount; i++)
                    strSQL = strSQL + m_OrderFields[i] + ", ";
                strSQL = strSQL.Substring(0, strSQL.Length - 2);
            }

            return strSQL;
        }

        public string BuildSQL(FilterOV filterOV)
        {
            string strFilterMessage = "";
            //返回操作语句
            if (filterOV != null)
            {
                strFilterMessage = Parse(filterOV);
                strFilterMessage = strFilterMessage.Trim();
            }

            //根据状态，进行相应操作
            switch (m_EntityState)
            {
                case EntityStateContants.esQuery://查询
                    {
                        strFilterMessage = BuildQuerySQL(strFilterMessage, "");
                        break;
                    }
                case EntityStateContants.esInsert://插入
                    {
                        strFilterMessage = "INSERT INTO " + m_TableName + strFilterMessage;
                        break;
                    }
                case EntityStateContants.esUpdate://更新
                    {
                        strFilterMessage = "UPDATE " + m_TableName + " SET " + strFilterMessage;
                        break;
                    }
                case EntityStateContants.esDelete://删除
                    {
                        strFilterMessage = "DELETE " + m_TableName + " WHERE " + strFilterMessage;
                        break;
                    }
            }
            return strFilterMessage;
        }
        /// <summary>
        /// 生成插入SQL语句
        /// </summary>
        /// <param name="filterOV">被提交的过虑对象</param>
        /// <returns>标准SQL语句</returns>
        private string BuildInsertSQL(FilterOV filterOV)
        {
            string strFields = "";
            string strValues = "";
            string strSQL = "";
            foreach (PropertyOV fieldValue in filterOV)
            {
                BuildRealValue(fieldValue);//获取真实值
                if (fieldValue.Value != "")
                {
                    strFields = strFields + fieldValue.Name + ",";
                    strValues = strValues + "'" + fieldValue.Value + "',";
                }
            }

            //如果保留历史轨迹，那么就增加日期信息
            if (m_SaveHistory)
            {
                strFields = strFields + " UpdateDate";
                strValues = strValues + "GETDATE()";
            }
            else
            {
                strFields = strFields.Substring(0, strFields.Length - 1);
                strValues = strValues.Substring(0, strValues.Length - 1);
            }

            //生成最终SQL语句
            if (strFields != "")
            {
                strSQL = "(" + strFields + ") VALUES(" + strValues + ")";
            }

            return strSQL;

        }

        /// <summary>
        /// 生成更新SQL语句
        /// </summary>
        /// <param name="filterOV">被提交的过虑对象</param>
        /// <returns>标准SQL语句</returns>
        private string BuildUpdateSQL(FilterOV filterOV)
        {
            string strSQL = "";

            foreach (PropertyOV fieldValue in filterOV)
            {
                BuildRealValue(fieldValue);
                if (fieldValue.Value != "")
                {
                    strSQL = strSQL + fieldValue.Name + " ='" + fieldValue.Value + "',";
                }
                else
                {
                    strSQL = strSQL + fieldValue.Name + " =NULL,"; //如果为空，则置为NULL
                }
            }
            //如果保留历史轨迹，那么就增加日期信息
            if (m_SaveHistory)
                strSQL = strSQL + " UpdateDate = GETDATE()";
            else
                strSQL = strSQL.Substring(0, strSQL.Length - 1);

            return strSQL;

        }

        /// <summary>
        /// 生成删除SQL语句
        /// </summary>
        /// <returns>标准SQL语句</returns>
        private string BuildDeleteSQL()
        {
            IList primaryFields = null;
            string strSQL = "";

            //获取实体关键字
            primaryFields = GetPrimaryField();
            foreach (PropertyOV fieldValue in primaryFields)
            {
                BuildRealValue(fieldValue);//获取真实值
                strSQL = strSQL + fieldValue.Name + " ='" + fieldValue.Value + "' AND ";
            }

            //生成最终SQL语句
            if (strSQL != "")
                strSQL = strSQL.Substring(0, strSQL.Length - 4);

            return strSQL;
        }

        /// <summary>
        /// 根据字段的特性，返回字段在数据库中存储的真实值
        /// </summary>
        /// <param name="fieldValue">需要获取真实值的PropertyOV对象</param>
        private void BuildRealValue(PropertyOV fieldValue)
        {
            string strSQL;
            SqlDataReader dataReader;
            string strRealValue;
            if (fieldValue.IsDictionary)
            {
                if (fieldValue.Value==null || fieldValue.Value == "")//如果字典字段中存在值，那么就不从字典表中获取,即值优先原则
                {
                    //if (fieldValue.DictName == "D_EquipType")
                    //    strSQL = "SELECT MC FROM " + fieldValue.DictName + " WHERE MC = '" + fieldValue.ShowValue + "'";
                    //else
                    strSQL = "SELECT DM FROM " + fieldValue.DictName + " WHERE MC = '" + fieldValue.ShowValue + "'";
                    dataReader = m_Database.GetDataReader(strSQL);
                    if (dataReader.Read())
                    {
                        strRealValue = dataReader.GetString(0);
                        fieldValue.Value = strRealValue.Trim();
                    }
                    else
                        fieldValue.Value = "";
                    dataReader.Close();
                }
            }
            else//针对日期型字段，需要进行判断
            {
                if (fieldValue.ShowValue != null)
                {
                    strRealValue = fieldValue.ShowValue.Trim();
                    fieldValue.Value = (strRealValue == "||") ? "" : strRealValue;
                }
            }

        }

        /// <summary>
        /// 根据字段的特性，把数据库中存储的真实值翻译成界面显示值
        /// </summary>
        /// <param name="fieldValue">进行操作的字段</param>
        private void BuildShowValue(PropertyOV fieldValue)
        {
            string strSQL = "";
            SqlDataReader dataReader = null;
            if (fieldValue.IsDictionary)
            {
                //if (fieldValue.DictName == "D_EquipType")
                //    strSQL = "SELECT MC FROM " + fieldValue.DictName + " WHERE MC = '" + fieldValue.ShowValue + "'";
                //else
                strSQL = "SELECT MC FROM " + fieldValue.DictName + " WHERE DM = '" + fieldValue.Value + "'";
                dataReader = m_Database.GetDataReader(strSQL);
                if (dataReader.Read())
                    fieldValue.ShowValue = dataReader.GetString(0);
                dataReader.Close();
            }
            else
            {
                if (fieldValue.FieldType == FieldTypeContants.ET_DATE)
                {
                    if (fieldValue.Value != "")
                    {
                        DateTime dt = Convert.ToDateTime(fieldValue.Value);
                        fieldValue.ShowValue = string.Format("{0:yyyy-MM-dd}", dt);
                    }
                }
                else
                    fieldValue.ShowValue = fieldValue.Value;

            }

            //一旦获取显示值以后，清空真实值
            fieldValue.Value = "";
        }

        /// <summary>
        /// 获取查询返回的字段，有简要信息显示和详细信息显示两种，同时生成连接SQL
        /// </summary>
        /// <param name="isSimply">表明返回简要信息还是详细信息</param>
        /// <returns>查询结果中显示的字段</returns>
        private string GetDisplayFields(bool isSimply)
        {
            ShowTypeContants showType;
            string strDicAliasTable = "";
            string strDisplayField = "";
            string strDisplayFields = "";
            int i = 0;

            //判断查询的类型
            if (isSimply)
                showType = ShowTypeContants.stSimpleShow;
            else
                showType = ShowTypeContants.stDetailShow;

            //清空连接字符串
            m_JoinSQL = "";

            //获取查询结果的显示字段
            foreach (PropertyOV propertyOV in m_Properties)
            {
                //需要是符合查询类型的字段
                if (propertyOV.ShowType == ShowTypeContants.stSimpleShow || propertyOV.ShowType == ShowTypeContants.stIDShow || propertyOV.ShowType == showType)
                {
                    if (propertyOV.IsDictionary)
                    {
                        strDicAliasTable = propertyOV.DictName + i.ToString();
                        strDisplayField = strDicAliasTable + ".MC";
                        m_JoinSQL = m_JoinSQL + " LEFT OUTER JOIN " + propertyOV.DictName + " " + strDicAliasTable + " ON " + strDicAliasTable + ".DM = " + m_TableName + "." + propertyOV.Name;
                    }
                    else
                    {   
                        if(propertyOV.FieldType==FieldTypeContants.ET_DATE)//如果是日期型，即包含时间，那么进行转换，格式“yyyy-MM-dd”
                            strDisplayField = "CONVERT(VARCHAR(10)," + m_TableName + "." + propertyOV.Name + ",120)" ;
                        else
                            strDisplayField = m_TableName + "." + propertyOV.Name;
                    }
                    strDisplayFields = strDisplayFields + strDisplayField + " AS [" + propertyOV.Alias + "],";
                }
                //获取排序字段
                if (propertyOV.OrderIndex > 0)
                    m_OrderFields[propertyOV.OrderIndex - 1] = m_TableName + "." + propertyOV.Name;
                i = i + 1;
            }
            //去掉最后一个逗号
            if (strDisplayFields != "")
                strDisplayFields = strDisplayFields.Substring(0, strDisplayFields.Length - 1);

            return strDisplayFields;
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="values">新增记录的值</param>
        /// <param name="recordsAffected">受影响的行数</param>
        /// <returns>新增的记录</returns>
        private DataSet Insert(string values,ref int recordsAffected)
        {
            IList primaryFields = null;
            string strFields = "";
            DataSet sdrInsert = null;

            string strSQL = "INSERT INTO " + m_TableName + values;
            recordsAffected = m_Database.Execute(strSQL);

            //如果插入成功，则返回当前插入的记录
            if (recordsAffected > 0 && m_Returned)
            {
                strSQL = "";
                primaryFields = GetPrimaryField();
                foreach (PropertyOV propertyOV in primaryFields)
                {
                    BuildRealValue(propertyOV);
                    strFields = strFields + propertyOV.Name + ",";
                    strSQL = strSQL + propertyOV.Name + " = '" + propertyOV.Value + "' AND ";
                }
                strFields = strFields.Substring(0, strFields.Length - 1);
                strSQL = strSQL.Substring(0, strSQL.Length - 4);

                strSQL = "SELECT " + strFields + " FROM " + m_TableName + " WHERE " + strSQL;
                sdrInsert = m_Database.GetDataset(strSQL);
            }
            return sdrInsert;
        }

        /// <summary>
        /// 更新当前记录
        /// </summary>
        /// <param name="values">更新记录的值</param>
        /// <param name="recordsAffected">受影响的行数</param>
        /// <returns>更新的记录</returns>
        private DataSet Update(string values, ref int recordsAffected)
        {
            string strSQL = "";
            string strFields = "";
            DataSet sdrUpdate = null;
            //获取关键字
            IList primaryFields = GetPrimaryField();

            foreach (PropertyOV propertyOV in primaryFields)
            {
                strSQL = strSQL + propertyOV.Name + " = '" + propertyOV.UpdatedValue + "' AND ";
            }
            strSQL = strSQL.Substring(0, strSQL.Length - 4);


            //保存历史信息
            if (SaveHistoryInfo(strSQL) == false)
                return null;
            

            strSQL = "UPDATE " + m_TableName + " SET " + values + " WHERE " + strSQL;

            
            recordsAffected = m_Database.Execute(strSQL);
            if (recordsAffected > 0 && m_Returned)//如果更新成功，则返回当前更新记录
            {
                strSQL = "";
                foreach (PropertyOV propertyOV in primaryFields)
                {
                    strFields = strFields + propertyOV.Name + ",";
                    strSQL = strSQL + propertyOV.Name + " = '" + propertyOV.Value + "' AND ";
                }
                strFields = strFields.Substring(0, strFields.Length - 1);
                strSQL = strSQL.Substring(0, strSQL.Length - 4);

                strSQL = "SELECT " + strFields + " FROM " + m_TableName + " WHERE " + strSQL;
                sdrUpdate = m_Database.GetDataset(strSQL);
            }
            return sdrUpdate;
        }

        /// <summary>
        /// 删除记录，把当前表中需要删除的记录放入变动表，然后从当前表中删除
        /// </summary>
        /// <param name="whereCause"></param>
        /// <returns></returns>
        private int Delete(string whereCause)
        {
            string strSQL = "";
            //bool canDeleted;
            int recordsAffected = 0;


            ////如果保留历史轨迹，那么就增加日期信息
            //if (m_SaveHistory)
            //{
            //    strSQL = "UPDATE " + m_TableName + " SET UPDATEDATE = GETDATE() WHERE " + whereCause;
            //    m_Database.Execute(strSQL);
            //    strSQL = "INSERT INTO " + m_TableName + "_BDB SELECT * FROM " + m_TableName + " WHERE " + whereCause;
            //    canDeleted = (m_Database.Execute(strSQL) > 0);
            //}
            //else
            //    canDeleted = true;

            if (SaveHistoryInfo(whereCause))
            {
                strSQL = "DELETE " + m_TableName + " WHERE " + whereCause;
                recordsAffected = m_Database.Execute(strSQL);
            }

            return recordsAffected;
        }

        /// <summary>
        /// 存储历史信息
        /// </summary>
        private bool SaveHistoryInfo(string whereCause)
        {
            string strSQL = "";
            int recordsAffected;
            IList eventFields = null;
            string strValues = "";
            bool saveHistoryInfo = false;

            if (m_SaveHistory)
            {
                //更新当前实体的时间，用于表示历史的演变情况
                strSQL = "UPDATE " + m_TableName + " SET UPDATEDATE = GETDATE() WHERE " + whereCause;
                recordsAffected = m_Database.Execute(strSQL);
                if (recordsAffected > 0)
                {
                    eventFields = m_History.GetEventFields();
                    foreach (PropertyOV fieldValue in eventFields)
                    {
                        strValues = strValues + ",'" + fieldValue.Value + "'";
                    }

                    strSQL = "INSERT INTO " + m_History.TableName + " SELECT *" + strValues + " FROM " + m_TableName + " WHERE " + whereCause;
                    saveHistoryInfo = (m_Database.Execute(strSQL) > 0);
                }
            }
            else
                saveHistoryInfo = true;
            return saveHistoryInfo;
        }

        
        #endregion

    }

    //实体状态
    public enum EntityStateContants
    {
        esQuery,//查询
        esInsert,//插入
        esUpdate,//更新
        esDelete //删除
    }

    //实体操作类型
    public enum OperatorTypeContants
    {
        otNone = 0, //无操作类型
        otQuery = 1, //查询
        otStatistic = 2, //统计
        otAll = 3 //可查可统
    }
    
}

