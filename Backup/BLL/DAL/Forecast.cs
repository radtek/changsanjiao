using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;
using System.Configuration;
using System.Net;

using System.Data;
using System.Data.SqlClient;
using Readearth.Data;
using Readearth.Data.Entity;
using MMShareBLL.Model;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MMShareBLL.DAL
{

    public class Forecast
    {
        Database m_Database;
        string m_ID;

        public Forecast()
        {
            m_Database = new Database();
        }

        public Forecast(Database db)
        {
            m_Database = db;
        }

        public IList<TreeNode> GetImageProduct(string node)
        {
            bool blnLeaf;
            string nodeAuthority = GetAuthority();
            IList<TreeNode> tree = new List<TreeNode>();

            blnLeaf = true;
            string strSQL;
            if(nodeAuthority!="")
                strSQL = "SELECT T_ImageProduct_test.ENTITYNAME,ALIGN,PERIOD,AliasName,T_ENTITY.HINT,CLASS FROM T_ImageProduct_test LEFT OUTER JOIN T_ENTITY ON T_ENTITY.ENTITYNAME = T_ImageProduct_test.ENTITYNAME WHERE MODULENAME = '" + node + "' AND  T_ImageProduct_test.ENTITYNAME not in (" + nodeAuthority + ") ORDER BY ORDERID";
            else
                strSQL = "SELECT T_ImageProduct_test.ENTITYNAME,ALIGN,PERIOD,AliasName,T_ENTITY.HINT,CLASS  FROM T_ImageProduct_test LEFT OUTER JOIN T_ENTITY ON T_ENTITY.ENTITYNAME = T_ImageProduct_test.ENTITYNAME WHERE MODULENAME = '" + node + "' ORDER BY ORDERID";
            //子节点的时候是通过“|”来表示的
            if (node.Contains("|"))
            {
                string[] strElements = node.Split('|');
                if(nodeAuthority!="")
                    strSQL = "SELECT T_ImageProduct_test.ENTITYNAME,ALIGN,PERIOD,AliasName,T_ENTITY.HINT,CLASS = NULL  FROM T_ImageProduct_test LEFT OUTER JOIN T_ENTITY ON T_ENTITY.ENTITYNAME = T_ImageProduct_test.ENTITYNAME WHERE MODULENAME = '" + strElements[0] + "' AND CLASS = '" + strElements[1] + "' AND  T_ImageProduct_test.ENTITYNAME not in (" + nodeAuthority + ") ORDER BY ORDERID";
                else
                    strSQL = "SELECT T_ImageProduct_test.ENTITYNAME,ALIGN,PERIOD,AliasName,T_ENTITY.HINT,CLASS= NULL  FROM T_ImageProduct_test LEFT OUTER JOIN T_ENTITY ON T_ENTITY.ENTITYNAME = T_ImageProduct_test.ENTITYNAME WHERE MODULENAME = '" + strElements[0] + "' AND CLASS = '" + strElements[1] + "' ORDER BY ORDERID";
            }
            SqlDataReader drProduct = m_Database.GetDataReader(strSQL);
            string nodeText = "";
            if (drProduct.HasRows)
            {
                while (drProduct.Read())
                {
                    bool newNode=false ;
                    TreeNode treeNode = new TreeNode();
                    if (drProduct.IsDBNull(5))
                    {
                        newNode = true;
                        treeNode.id = drProduct.GetString(0) + "|" + drProduct.GetString(1);
                        treeNode.text = drProduct.IsDBNull(4) ? "" : drProduct.GetString(4);
                        treeNode.tag = drProduct.IsDBNull(2) ? "" : drProduct.GetString(2);
                        treeNode.leaf = blnLeaf;
                        treeNode.aliasName = drProduct.IsDBNull(3) ? "" : drProduct.GetString(3);
                    }
                    else
                    {
                        if (nodeText == "")
                        {
                            
                            nodeText = drProduct.GetString(5);
                            newNode = true;
                            if (nodeText != "")
                            {
                                treeNode.id = node + "|" + nodeText;
                                treeNode.text = nodeText;
                            }
                            else
                            {
                                treeNode.id = drProduct.GetString(0) + "|" + drProduct.GetString(1);
                                treeNode.text = drProduct.IsDBNull(4) ? "" : drProduct.GetString(4);
                                treeNode.tag = drProduct.IsDBNull(2) ? "" : drProduct.GetString(2);
                                treeNode.leaf = blnLeaf;
                                treeNode.aliasName = drProduct.IsDBNull(3) ? "" : drProduct.GetString(3);
                            }
                        }
                        else
                        {
                            string nodeTextNew= drProduct.GetString(5);
                            if (nodeText != nodeTextNew)
                            {
                                newNode = true;
                                treeNode.id = node + "|" + nodeTextNew;
                                treeNode.text = nodeTextNew;
                                nodeText = nodeTextNew;
                                treeNode.aliasName = drProduct.IsDBNull(3) ? "" : drProduct.GetString(3);
                            }
                        }
                        
                    }
                    if (newNode)
                        tree.Add(treeNode);

                }
            }
            drProduct.Close();

            return tree;
        }
        public string  GetAuthority()
        {
            string id = m_ID;
            string strSQL = "SELECT Authority From T_Classes WHERE ID="+id;
            DataTable dt = m_Database.GetDataTable(strSQL);
            string dataAuthority;
            if (dt.Rows[0][0].ToString() !="")
            {
                Authority m = (Authority)JsonConvert.DeserializeObject(dt.Rows[0][0].ToString(), typeof(Authority));//((Newtonsoft.Json.Linq.JContainer)(m)).First
                dataAuthority = m.data;
            }
            else
                dataAuthority = "";
            return dataAuthority;

        }
        public void setUserID(string ID)
        {
            m_ID = ID;

        }
        public IList<PropertyJsOV> GetEntity(string entityName)
        {
            Entity entity = new Entity(m_Database, entityName);
            IList fieldsProperty = entity.GetProperties(EntityStateContants.esQuery, QueryTypeContants.qtIndexQuery);
            PropertyOV fieldValue = null;
            IList<PropertyJsOV> properties = new List<PropertyJsOV>();
            for (int i = 0; i < fieldsProperty.Count; i++)
            {
                fieldValue = (PropertyOV)fieldsProperty[i];
                PropertyJsOV fieldJsOV = new PropertyJsOV();
                fieldJsOV.Alias = fieldValue.Alias;
                fieldJsOV.DefaultValue = fieldValue.DefaultValue;
                fieldJsOV.DictName = fieldValue.DictName;
                fieldJsOV.EntityName = fieldValue.EntityName;
                fieldJsOV.FieldType = (int)fieldValue.FieldType;
                fieldJsOV.IsEditable = fieldValue.IsEditable;
                fieldJsOV.IsEvent = fieldValue.IsEvent;
                fieldJsOV.IsNullable = fieldValue.IsNullable;
                fieldJsOV.IsPK = fieldValue.IsPK;
                fieldJsOV.Length = fieldValue.Length;
                fieldJsOV.Link = fieldValue.Link;
                fieldJsOV.Name = fieldValue.Name;
                fieldJsOV.OrderIndex = fieldValue.OrderIndex;
                fieldJsOV.QueryType = (int)fieldValue.QueryType;
                fieldJsOV.ShowType = (int)fieldValue.ShowType;
                if (fieldValue.FieldType == FieldTypeContants.ET_DATETIME)
                    fieldJsOV.ShowValue = GetLastestDatetime(entity);
                fieldJsOV.UpdatedValue = fieldValue.UpdatedValue;
                fieldJsOV.Value = fieldValue.Value;
                fieldJsOV.YField = fieldValue.YField;

                properties.Add(fieldJsOV);
            }
            return properties;
        }

        /// <summary>
        /// 获取实体当前
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string GetLastestDatetime(Entity entity)
        {
            string strWhere = "SELECT COUNT(*) FROM T_ImageProduct_test WHERE ENTITYNAME = '" + entity.Name + "' AND PERIOD IS NULL";
            string strExisit = m_Database.GetFirstValue(strWhere);
            string strGetLastestDatetime = "";

            strWhere = "SELECT MAX(FORECASTDATE) FROM " + entity.TableName;
            if (entity.Condition != "")
            {
                strWhere = strWhere + " WHERE " + entity.Condition;
            }
            SqlDataReader dr = m_Database.GetDataReader(strWhere);
            if (dr.Read())
            {
                if (dr.IsDBNull(0) == false)
                {
                    DateTime dt = dr.GetDateTime(0);
                    if (strExisit == "1")
                        if (entity.OperatorType == OperatorTypeContants.otStatistic)//对于Pm10，Pm2.5的整点数据不加一小时
                            strGetLastestDatetime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                        else//在前台只是显示到时，因此实况数据中存在时分的情况的最新数据就不会被查到，因此增加一小时
                            strGetLastestDatetime = dt.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        strGetLastestDatetime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            dr.Close();

            if (strGetLastestDatetime == "")
            {
                string strPeriod = GetPeriod(entity.Name);
                if (strPeriod != "")
                {
                    string[] periods = strPeriod.Split(',');
                    strGetLastestDatetime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd " + periods[periods.Length - 1] + ":00:00");
                }
                else
                    strGetLastestDatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            }

            return strGetLastestDatetime;
        }

        public DataTable QueryList(string entityName, string entityObj)
        {
            Entity entity = new Entity(m_Database, entityName);
            string strWhere = " WHERE ";
            if (entityObj != "")
                strWhere = strWhere + GetUserWhere(entity, entityObj);
            else
                strWhere = strWhere + GetDefaultWhere(entity);

            if (entity.Condition != "")
            {
                strWhere = strWhere + " AND " + entity.Condition;
            }
            //string strSQL = "SELECT (folder + '/' + name) AS DM,(CASE WHEN Period IS NULL THEN REPLACE(REPLACE(REPLACE(CONVERT(varchar(16),ForecastDate, 120), '-', ''), ':', ''), ' ', '') ELSE REPLACE(REPLACE(REPLACE(CONVERT(varchar(16), ForecastDate, 120), '-', ''), ':', ''), ' ', '') + '.' + Period END) AS MC FROM  " + entity.TableName + strWhere;
            string strSQL = "SELECT ('Product/' + folder + '/' + name) AS DM, (CASE WHEN Period IS NULL THEN CONVERT(varchar(16),ForecastDate, 120) ELSE SUBSTRING(CONVERT(varchar(16),ForecastDate, 120), 0, 5)+'年'+SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 6, 2) + '月' + SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 9, 2) + '日' + SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 12, 2) + '时 F' + Period + 'H' END) AS MC FROM  " + entity.TableName + strWhere + "ORDER BY NAME";

            try
            {
                DataSet dt = m_Database.GetDataset(strSQL);
                if (dt.Tables.Count > 0)
                {
                    DataTable dTable = dt.Tables[0];
                    if (dTable.Rows[0]["MC"].ToString().IndexOf('F') > 0)
                    {
                        foreach (DataRow dr in dTable.Rows)
                        {
                            string oldStr = dr[1].ToString().Substring(0, 14);
                            int hourAdd = int.Parse(dr[1].ToString().Substring(16, 3));
                            string newStr = DateTime.Parse(oldStr).AddHours(hourAdd).ToString("yyyy-MM-dd HH:00");
                            dr[1] = newStr;
                        }
                    }
                    return dTable;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 获取缺省的Where语句
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string GetDefaultWhere(Entity entity)
        {
            IList fieldsProperty = entity.GetProperties(EntityStateContants.esQuery, QueryTypeContants.qtIndexQuery);
            PropertyOV fieldValue = null;
            FilterOV filterOV = new FilterOV();
            string strPeriod = GetPeriod(entity.Name);

            string strSQL = "";
            for (int i = 0; i < fieldsProperty.Count; i++)
            {
                fieldValue = (PropertyOV)fieldsProperty[i];
                if (fieldValue.IsDictionary)
                {
                    strSQL = "";
                    if (fieldValue.YField != "")
                    {
                        PropertyOV dpField = entity.GetPropertyOV(fieldValue.YField);
                        strSQL = "SELECT TOP 1 DM FROM " + dpField.DictName;
                        strSQL = " WHERE DP LIKE '%" + m_Database.GetFirstValue(strSQL) + "%'";

                    }
                    strSQL = "SELECT TOP 1 MC FROM " + fieldValue.DictName + strSQL;
                    fieldValue.ShowValue = m_Database.GetFirstValue(strSQL);
                }
                else if (fieldValue.FieldType == FieldTypeContants.ET_DATETIME)
                {
                    string strLastestDatetime = GetLastestDatetime(entity);
                    if (strLastestDatetime != "")
                    {
                        DateTime dtNow = DateTime.Parse(strLastestDatetime);
                        if (strPeriod == "")
                            fieldValue.ShowValue = dtNow.ToString("yyyy-MM-dd 00:00:00") + "||" + dtNow.ToString("yyyy-MM-dd 23:59:59");
                        else
                        {
                            //预报数据
                            //string[] periods = strPeriod.Split(',');
                            fieldValue.ShowValue = strLastestDatetime;
                        }
                    }
                }
                filterOV.Add(fieldValue);
            }
            return entity.BuildQuerySQL(filterOV);
        }

        /// <summary>
        /// 获取用户输入的条件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string GetUserWhere(Entity entity, string entityObj)
        {
            IList<PropertyJsOV> properties;
            PropertyJsOV fieldJsOV;
            PropertyOV fieldValue;
            FilterOV filterOV = new FilterOV();
            string strSQL = "";
            string strPeriodSQL = "";
            string strForecastSQL = "";

            properties = JsonConvert.DeserializeObject<IList<PropertyJsOV>>(entityObj);
            for (int i = 0; i < properties.Count; i++)
            {
                fieldJsOV = (PropertyJsOV)properties[i];
                fieldValue = entity.GetPropertyOV(fieldJsOV.Name);
                if (fieldValue.IsDictionary && fieldValue.IsEvent)
                {
                    string[] showValues = fieldJsOV.ShowValue.Split('+');//此处特殊处理   环境监测中心  张伟锋   2013-05-06
                    fieldValue.ShowValue = showValues[0];
                    strPeriodSQL = " AND PERIOD = '000'";
                    if (showValues.Length > 1)
                    {
                        strForecastSQL = "PERIOD <= '" + showValues[1] + "' AND " + showValues[2];
                    }
                }
                else
                {
                    fieldValue.ShowValue = fieldJsOV.ShowValue;
                }
                filterOV.Add(fieldValue);
            }

            strSQL = entity.BuildQuerySQL(filterOV);

            //对环境监测的特殊处理
            string strSplit = " AND ";
            int andIndex = strSQL.LastIndexOf(strSplit);
            if (andIndex > 0)
            {
                strSplit = strSQL.Substring(andIndex);
                if (strForecastSQL != "")
                    strSQL = "(" + strSQL + strPeriodSQL + ") OR (" + strForecastSQL + strSplit + ")";
                else
                    strSQL = strSQL + strPeriodSQL;
            }


            return strSQL;
        }



        /// <summary>
        /// 获取产品的预报时效，判定是预报数据还是实时数据
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        private string GetPeriod(string entityName)
        {
            string strSQL = "SELECT PERIOD FROM T_ImageProduct_test WHERE ENTITYNAME = '" + entityName + "'";
            return m_Database.GetFirstValue(strSQL);
        }



    }

}
