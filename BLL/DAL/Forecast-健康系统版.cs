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
        public DataTable  GetLeftPanel(string node)
        {
            string strSQL = "SELECT T_ImageProductJK.ENTITYNAME,ALIGN,PERIOD,AliasName,T_ENTITYJK.HINT,CLASS,cssName,MenuName,flag  FROM T_ImageProductJK LEFT OUTER JOIN T_ENTITYJK ON T_ENTITYJK.ENTITYNAME = T_ImageProductJK.ENTITYNAME WHERE MODULENAME = '" + node + "' ORDER BY ORDERID,CLASS,MenuName";
            return m_Database.GetDataTable(strSQL);
        }
        public string GetImageProduct(string node)
        {
            //string nodeAuthority = GetAuthority();
            string nodeAuthority ="";

            IList<TreeNode> tree = new List<TreeNode>();

            //bool blnLeaf = true;
            string strSQL;
            if(nodeAuthority!="")
                strSQL = "SELECT T_ImageProductJK.ENTITYNAME,ALIGN,PERIOD,AliasName,T_ENTITYJK.HINT,CLASS FROM T_ImageProductJK LEFT OUTER JOIN T_ENTITYJK ON T_ENTITYJK.ENTITYNAME = T_ImageProductJK.ENTITYNAME WHERE MODULENAME = '" + node + "' AND  T_ImageProductJK.ENTITYNAME not in (" + nodeAuthority + ") ORDER BY CLASS,ORDERID";
            else
                strSQL = "SELECT T_ImageProductJK.ENTITYNAME,ALIGN,PERIOD,AliasName,T_ENTITYJK.HINT,CLASS  FROM T_ImageProductJK LEFT OUTER JOIN T_ENTITYJK ON T_ENTITYJK.ENTITYNAME = T_ImageProductJK.ENTITYNAME WHERE MODULENAME = '" + node + "' ORDER BY CLASS,ORDERID";
            //子节点的时候是通过“|”来表示的
            //if (node.Contains("|"))
            //{
            //    string[] strElements = node.Split('|');
            //    if(nodeAuthority!="")
            //        strSQL = "SELECT T_ImageProduct_test.ENTITYNAME,ALIGN,PERIOD,AliasName,T_ENTITYJK.HINT,CLASS = NULL  FROM T_ImageProduct_test LEFT OUTER JOIN T_ENTITYJK ON T_ENTITYJK.ENTITYNAME = T_ImageProduct_test.ENTITYNAME WHERE MODULENAME = '" + strElements[0] + "' AND CLASS = '" + strElements[1] + "' AND  T_ImageProduct_test.ENTITYNAME not in (" + nodeAuthority + ") ORDER BY ORDERID";
            //    else
            //        strSQL = "SELECT T_ImageProduct_test.ENTITYNAME,ALIGN,PERIOD,AliasName,T_ENTITYJK.HINT,CLASS= NULL  FROM T_ImageProduct_test LEFT OUTER JOIN T_ENTITYJK ON T_ENTITYJK.ENTITYNAME = T_ImageProduct_test.ENTITYNAME WHERE MODULENAME = '" + strElements[0] + "' AND CLASS = '" + strElements[1] + "' ORDER BY ORDERID";
            //}
            //DataTable dt = m_Database.GetDataTable(strSQL);
            SqlDataReader drProduct = m_Database.GetDataReader(strSQL);
            string nodeText = "";
            string jsonData="";
            StringBuilder sb = new StringBuilder("[");
            StringBuilder sm=new StringBuilder();
            if (drProduct.HasRows)
            {
                while (drProduct.Read())
                {
                    TreeNode treeNode = new TreeNode();
                    if (drProduct.IsDBNull(5))
                    {
                        //treeNode.id = drProduct.GetString(0) + "|" + drProduct.GetString(1);
                        //treeNode.text = drProduct.IsDBNull(4) ? "" : drProduct.GetString(4);
                        //treeNode.tag = drProduct.IsDBNull(2) ? "" : drProduct.GetString(2);
                        //treeNode.leaf = blnLeaf;
                        //treeNode.aliasName = drProduct.IsDBNull(3) ? "" : drProduct.GetString(3);
                    }
                    else
                    {

                        if (nodeText == "")
                        {
                            sm.Append("{");
                            nodeText = drProduct.GetString(5);
                            sm.AppendFormat("\"text\":\"{0}\",\"icon\":\"{1}\",", nodeText, "");
                            treeNode.id = drProduct.GetString(0);
                            treeNode.text = drProduct.IsDBNull(4) ? "" : drProduct.GetString(4);
                            treeNode.tag = drProduct.IsDBNull(2) ? "" : drProduct.GetString(2);
                            treeNode.aliasName = drProduct.IsDBNull(3) ? "" : drProduct.GetString(3);
                            tree.Add(treeNode);
                        }
                        else
                        {
                            string nodeTextNew = drProduct.GetString(5);
                            if (nodeText != nodeTextNew)
                            {
                                nodeText = nodeTextNew;
                                jsonData = JsonConvert.SerializeObject(tree);
                                sm.AppendFormat("\"children\":{0}", jsonData);
                                tree.Clear();
                                sm.Append("},{");
                                sm.AppendFormat("\"text\":\"{0}\",\"icon\":\"{1}\",", nodeTextNew, "");
                                treeNode.id = drProduct.GetString(0);
                                treeNode.text = drProduct.IsDBNull(4) ? "" : drProduct.GetString(4);
                                treeNode.tag = drProduct.IsDBNull(2) ? "" : drProduct.GetString(2);
                                treeNode.aliasName = drProduct.IsDBNull(3) ? "" : drProduct.GetString(3);
                                tree.Add(treeNode);
                            }
                            else
                            {
                                treeNode.id = drProduct.GetString(0);
                                treeNode.text = drProduct.IsDBNull(4) ? "" : drProduct.GetString(4);
                                treeNode.tag = drProduct.IsDBNull(2) ? "" : drProduct.GetString(2);
                                treeNode.aliasName = drProduct.IsDBNull(3) ? "" : drProduct.GetString(3);
                                tree.Add(treeNode);

                            }
                        }

                    }
                }
            }
            jsonData = JsonConvert.SerializeObject(tree);
            sm.AppendFormat("\"children\":{0}", jsonData);
            drProduct.Close();
            if (sm.Length > 1)
                sm.Append("}");
            sb.Append(sm.ToString());
            if (sb.Length > 1)
            {
                sb.Append("]");
            }
            else
                sb.Length = 0;

            return sb.ToString();
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
        public IList<PropertyJsOV> GetEntityType(string entityName, string type)
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
                    fieldJsOV.ShowValue = GetLastestDatetime(entity,type);
                fieldJsOV.UpdatedValue = fieldValue.UpdatedValue;
                fieldJsOV.Value = fieldValue.Value;
                fieldJsOV.YField = fieldValue.YField;

                properties.Add(fieldJsOV);
            }
            return properties;
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
        private string GetLastestDatetimeCopy(Entity entity)
        {
            string strWhere = "SELECT COUNT(*) FROM T_ImageProductJK WHERE ENTITYNAME = '" + entity.Name + "' AND PERIOD IS NULL";
            string strExisit = m_Database.GetFirstValue(strWhere);
            string strGetLastestDatetime = "";
            IList fieldsProperty = entity.GetProperties(EntityStateContants.esQuery, QueryTypeContants.qtIndexQuery);
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

        private string GetLastestDatetime(Entity entity)
        {
            string strWhere = "SELECT COUNT(*) FROM T_ImageProductJK WHERE ENTITYNAME = '" + entity.Name + "' AND PERIOD IS NULL";
            string strExisit = m_Database.GetFirstValue(strWhere);
            string strGetLastestDatetime = "";
            IList fieldsProperty = entity.GetProperties(EntityStateContants.esQuery, QueryTypeContants.qtIndexQuery);
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
                        else
                        {//在前台只是显示到时，因此实况数据中存在时分的情况的最新数据就不会被查到，因此增加一小时
                            if (entity.Name != "AQIAM" && entity.Name != "AQIPM" && entity.Name != "AQINM" && entity.Name != "PM25WHF" && entity.Name != "YJ")
                            {
                                strGetLastestDatetime = dt.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {                                
                                strGetLastestDatetime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
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

        private string GetLastestDatetime(Entity entity,string type)
        {
            string strWhere = "SELECT COUNT(*) FROM T_ImageProductJK WHERE ENTITYNAME = '" + entity.Name + "' AND PERIOD IS NULL";
            string strExisit = m_Database.GetFirstValue(strWhere);
            string strGetLastestDatetime = "";
            IList fieldsProperty = entity.GetProperties(EntityStateContants.esQuery, QueryTypeContants.qtIndexQuery);
            strWhere = "SELECT MAX(FORECASTDATE) FROM " + entity.TableName;
            if (entity.Condition != "")
            {
                strWhere = strWhere + " WHERE " + entity.Condition;
            }
            if (type != "")
            {
                if(type.IndexOf(",")>0)
                    strWhere = strWhere + " AND  Type in" + returnSQLStr(type); 
                else
                    strWhere = strWhere + " AND  Type='" + type+"'"; 

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
        //切换时间查询
        public DataTable QueryTextList(string entityName, string entityObj)
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

            try
            {
                string strSQL = "SELECT distinct CONVERT(varchar(16),DATEADD(hour, CONVERT(int, Period), ForecastDate), 120) AS MC FROM  " + entity.TableName + strWhere + "ORDER BY MC ";
                DataTable dt = m_Database.GetDataTable(strSQL);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string trickText(string Datetime, string entityName)
        {
            IList<PropertyJsOV> properties = new List<PropertyJsOV>();
            properties = GetEntity(entityName);
            Entity entity = new Entity(m_Database, entityName);
            string strWhere = " WHERE  ForecastDate='"+Datetime+"'";
            string entityObj = ToJSON(properties);
            string strSQL = "SELECT  (folder + '/' + name) AS DM, ForecastDate  AS MC FROM  " + entity.TableName + strWhere;
            string url = m_Database.GetFirstValue(strSQL);
            url = ConfigurationManager.AppSettings["testFileLoad"] + url.Replace("/", "\\");
            int index = url.IndexOf('?');
            url = url.Substring(0, index);
            StreamReader sr = File.OpenText(url);
            string nextLine = sr.ReadLine();
            nextLine = sr.ReadLine();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}*", nextLine);
            nextLine = sr.ReadLine();
            sb.AppendFormat("{0}*", nextLine);
            while ((nextLine = sr.ReadLine()) != null)
            {
                sb.AppendFormat("<p>{0}</p>", nextLine);
            } 
            sr.Close();
            return sb.ToString();
        }
        public string trickQueryEastList(string Datetime, string entityName, string json, string period)
        {
            if (Datetime != "")
            {
                string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
                //DataTable dTable = QueryDataTable(Datetime, entityName, json,period);
                DataTable dTable = QueryDataTableForMM(Datetime, entityName, json, period);
                try
                {
                    string imgUrl = QueryImgUrlEast(dTable, entityName, info);
                    return imgUrl;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
                return "";
        }
        public string trickQueryList(string Datetime, string entityName, string json)
        {
            if (Datetime != "")
            {
                string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
                DataTable dTable= QueryDataTable(Datetime,entityName,json,"");
                try
                {
                    string imgUrl = QueryImgUrl(dTable, entityName, info);
                    return imgUrl;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else 
                return "";
        }

        public string trickQueryListII(string Datetime, string entityName, string json)
        {
            if (Datetime != "")
            {
                string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
                DataTable dTable = QueryDataTableII(Datetime, entityName, json, "");
                try
                {
                    string imgUrl = QueryImgUrl(dTable, entityName, info);
                    return imgUrl;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
                return "";
        }

        /// <summary>
        /// 薛辉 2016-10-13
        /// </summary>
        /// <param name="Datetime"></param>
        /// <param name="entityName"></param>
        /// <param name="json"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public string trickQueryListCSJ(string Datetime, string entityName, string json, string period)
        {
            if (Datetime != "")
            {

                if (period == "p08")
                {
                      Datetime=DateTime.Parse(Datetime).AddDays(-1).ToString("yyyy-MM-dd 20:00:00");
                }
                else {
                      Datetime = DateTime.Parse(Datetime).AddDays(0).ToString("yyyy-MM-dd 08:00:00");
                }

                string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
                DataTable dTable = QueryDataTableCSJ(Datetime, entityName, json, "");
                try
                {
                    string imgUrl = QueryImgUrl(dTable, entityName, info);
                    return imgUrl;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
                return "";
        }
        public DataTable QueryDataTableCSJ(string Datetime, string entityName, string json, string period)
        {
            DataTable dTable = new DataTable();
            if (Datetime != "")
            {
                string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
                IList<PropertyJsOV> properties = new List<PropertyJsOV>();
                properties = GetEntity(entityName);
                StringBuilder sb = new StringBuilder();
                Entity entity = new Entity(m_Database, entityName);
                string entityObj = ToJSON(properties);
                string strWhere = "";

                if (info[7] != "" && info[1] != "全国" && info[1] != "华东")
                {
                    if (info[1] != "")
                    {
                        int periods = int.Parse(period);
                        string forecastDate = DateTime.Parse(Datetime).AddHours(-periods).ToString("yyyy-MM-dd HH:00:00");
                        strWhere = " WHERE ForecastDate='" + forecastDate + "' AND Period='" + period + "'";
                    }
                    else
                        strWhere = " WHERE ForecastDate='" + Datetime + "'";
                    strWhere = strWhere + "  AND Type in " + returnSQLStr(info[7]);
                }
                else
                    strWhere = " WHERE ForecastDate='" + Datetime + "'";
                if (entity.Condition != "")
                    strWhere = strWhere + " AND " + entity.Condition;
                if (info[1] == "全国" || info[1] == "华东")
                    strWhere = strWhere + " AND  station='" + info[7] + "' ";

                int counts = int.Parse(info[3]);
                int allCounts = int.Parse(info[5]);
                double width = 100.0 / counts - 1.0;
                int TypeLength = info[7].Split(',').Length;


                string strSQL = "SELECT TOP(" + allCounts + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + "ORDER BY Period,Type asc";
                //string strSQL = "SELECT TOP(" + allCounts + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + strTypeSQL;

                dTable = m_Database.GetDataTable(strSQL);
                if (dTable != null && dTable.Rows.Count > 0) {
                    foreach (DataRow row in dTable.Rows) {
                        if (row["DM"].ToString().Trim().IndexOf("Cold_O")>=0)
                        {
                            row["DM"] = row["DM"].ToString().Replace("Cold_O", "Cold_T");
                        }
                        else if (row["DM"].ToString().Trim().IndexOf("Cold_T") >= 0)
                        {
                            row["DM"] = row["DM"].ToString().Replace("Cold_T", "Cold_O");
                        }
                    }
                }
                return dTable;
            }
            else
                return dTable;

        }

        //用于模式分析部分
        public string trickQueryListMM(string Datetime, string entityName, string json)
        {
            if (Datetime != "")
            {
                string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
                DataTable dTable = QueryDataTableForMM(Datetime, entityName, json, "");
                try
                {
                    string imgUrl = QueryImgUrl(dTable, entityName, info);
                    return imgUrl;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
                return "";
        }
        public DataTable QueryDataTable(string Datetime, string entityName, string json,string period)
        {
            DataTable dTable=new DataTable();
            if (Datetime != "")
            {
                string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
                IList<PropertyJsOV> properties = new List<PropertyJsOV>();
                properties = GetEntity(entityName);
                StringBuilder sb = new StringBuilder();
                Entity entity = new Entity(m_Database, entityName);
                string entityObj = ToJSON(properties);
                string strWhere = "";
               
                if (info[7] != "" && info[1] != "全国" && info[1] != "华东")
                {                    
                    if (info[1] != "")
                    {
                        int periods = int.Parse(period);
                        string forecastDate = DateTime.Parse(Datetime).AddHours(-periods).ToString("yyyy-MM-dd HH:00:00");
                        strWhere = " WHERE ForecastDate='" + forecastDate + "' AND Period='" + period + "'";
                    }
                    else
                        strWhere = " WHERE ForecastDate='" + Datetime + "'";
                    strWhere = strWhere + "  AND Type in " + returnSQLStr(info[7]);
                }
                else
                    strWhere = " WHERE ForecastDate<='" + Datetime + "'";
                if (entity.Condition != "")
                    strWhere = strWhere + " AND " + entity.Condition;
                if(info[1] == "全国" || info[1] == "华东")
                    strWhere = strWhere + " AND  station='" + info[7] +"' ";

                int counts = int.Parse(info[3]);
                int allCounts = int.Parse(info[5]);
                double width = 100.0 / counts - 1.0;
                int TypeLength = info[7].Split(',').Length;               

                
                 
                       

                string strSQL = "SELECT TOP(" + allCounts + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + "ORDER BY MC DESC";
                //string strSQL = "SELECT TOP(" + allCounts + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + strTypeSQL;

                dTable = m_Database.GetDataTable(strSQL);
                return dTable;
            }
            else
                return dTable;
                
        }

        public DataTable QueryDataTableII(string Datetime, string entityName, string json, string period)
        {
            DataTable dTable = new DataTable();
            if (Datetime != "")
            {
                string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
                IList<PropertyJsOV> properties = new List<PropertyJsOV>();
                properties = GetEntity(entityName);
                StringBuilder sb = new StringBuilder();
                Entity entity = new Entity(m_Database, entityName);
                string entityObj = ToJSON(properties);
                string strWhere = "";

                if (info[7] != "" && info[1] != "全国" && info[1] != "华东")
                {
                    if (info[1] != "")
                    {
                        int periods = int.Parse(period);
                        string forecastDate = DateTime.Parse(Datetime).AddHours(-periods).ToString("yyyy-MM-dd HH:00:00");
                        strWhere = " WHERE ForecastDate='" + forecastDate + "' AND Period='" + period + "'";
                    }
                    else
                        strWhere = " WHERE ForecastDate='" + Datetime + "'";
                    strWhere = strWhere + "  AND Type in " + returnSQLStr(info[7]);
                }
                else
                    strWhere = " WHERE ForecastDate='" + Datetime + " 20:00:00'";
                if (entity.Condition != "")
                    strWhere = strWhere + " AND " + entity.Condition;
                if (info[1] == "全国" || info[1] == "华东")
                    strWhere = strWhere + " AND  station='" + info[7] + "' ";

                int counts = int.Parse(info[3]);
                int allCounts = int.Parse(info[5]);
                double width = 100.0 / counts - 1.0;
                int TypeLength = info[7].Split(',').Length;





                string strSQL = "SELECT TOP(" + allCounts + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + "ORDER BY MC DESC";
                //string strSQL = "SELECT TOP(" + allCounts + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + strTypeSQL;

                dTable = m_Database.GetDataTable(strSQL);
                return dTable;
            }
            else
                return dTable;

        }

        //模式分析的方法
        public DataTable QueryDataTableForMM(string Datetime, string entityName, string json, string period)
        {
            DataTable dTable = new DataTable();
            if (Datetime != "")
            {
                string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
                IList<PropertyJsOV> properties = new List<PropertyJsOV>();
                properties = GetEntity(entityName);
                StringBuilder sb = new StringBuilder();
                Entity entity = new Entity(m_Database, entityName);
                string entityObj = ToJSON(properties);
                string strWhere = "";
                //数据库中连接字符串内Type后数字的个数
                string strTypeSQL = "";
                if (info[7] != "" && info[1] != "全国" && info[1] != "华东")
                {
                    strTypeSQL = "ORDER BY  CHARINDEX(Type," + "'" + info[7] + "'" + ")";
                    if (info[1] != "")
                    {
                        int periods = int.Parse(period);
                        string forecastDate = DateTime.Parse(Datetime).AddHours(-periods).ToString("yyyy-MM-dd HH:00:00");
                        strWhere = " WHERE ForecastDate='" + forecastDate + "' AND Period='" + period + "'";
                    }
                    else
                        strWhere = " WHERE ForecastDate='" + Datetime + "'";
                    strWhere = strWhere + "  AND Type in " + returnSQLStr(info[7]);
                }
                else
                    strWhere = " WHERE ForecastDate<='" + Datetime + "'";
                if (entity.Condition != "")
                    strWhere = strWhere + " AND " + entity.Condition;
                if (info[1] == "全国" || info[1] == "华东")
                    strWhere = strWhere + " AND  station='" + info[7] + "' ";

                int counts = int.Parse(info[3]);
                int allCounts = int.Parse(info[5]);
                double width = 100.0 / counts - 1.0;
                int TypeLength = info[7].Split(',').Length;





                //string strSQL = "SELECT TOP(" + allCounts + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + "ORDER BY MC DESC";
                string strSQL = "SELECT TOP(" + allCounts + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + strTypeSQL;

                dTable = m_Database.GetDataTable(strSQL);
                return dTable;
            }
            else
                return dTable;

        }
        public string QueryImgUrlEast(DataTable dTable, string entityName, string[] info)
        {
            StringBuilder sb = new StringBuilder();
            int counts = int.Parse(info[3]);
            int allCounts = int.Parse(info[5]);
            double width = 0.0;
            int imgSpan = 0;
            if (info[9] != "")
            {
                //imgSpan = 3;
                imgSpan = 1;
                if (dTable.Rows.Count <= counts & dTable.Rows.Count > 2)
                    width = (100.0 + imgSpan) / dTable.Rows.Count - imgSpan;
                else
                {
                    if (info[1] == "1")
                    {
                        if (info.Length > 10)
                            width = int.Parse(info[11]);
                        else 
                            width = 65;
                    }
                    else if (info.Length > 10)
                        width = int.Parse(info[11]);
                    else
                        width = 40;
                }
            }
            else
            {
                if (info[1] == "1")
                {
                    if (dTable.Rows.Count <= counts & dTable.Rows.Count > 2)
                    {
                        imgSpan = 3;
                        width = (100.0 + imgSpan) / dTable.Rows.Count - imgSpan;
                    }
                    else
                    {
                        if (info.Length > 10)
                            width = int.Parse(info[11]);
                        else
                            width = 65;
                    }
                }
                else
                {
                    imgSpan = 10;
                    width = (100.0 + imgSpan) / counts - imgSpan;
                }

            }

            if (dTable.Rows.Count < 4)
            {
                imgSpan = 0;
            }

            for (int i = 0; i <Math.Ceiling(double.Parse(dTable.Rows.Count.ToString()) / counts); i++)
            {
                if (entityName == "HysplitBackward")
                    sb.AppendFormat("<div style='margin-top:80px;text-align:center;margin-right:auto;margin-left:auto'>", imgSpan.ToString());
                else
                {
                    if (info[9] != "")
                    {
                        sb.AppendFormat("<div style='margin-top:7px;text-align:center;margin-right:auto;margin-left:auto'>", imgSpan.ToString());
                    }
                    else
                    {
                        if (info[1] == "1")
                            sb.AppendFormat("<div style='margin-top:7px;text-align:center;margin-right:auto;margin-left:auto'>", imgSpan.ToString());
                        else 
                            sb.AppendFormat("<div style='margin-top:7px;margin-right:{0}%;margin-left:{0}%'>", imgSpan.ToString());
                    }
                }
                for (int j = 0; j < counts; j++)
                {
                    if (i * counts + j + 1 <= allCounts)
                    {
                        if ((i * counts + j) >= dTable.Rows.Count)
                        {
                            if (info[9] == "")
                                sb.AppendFormat("<img src='{0}' width='{1}' onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" style='margin-right:{7}%'/>", "", width.ToString() + "%", entityName, "", "", "", "", imgSpan.ToString());                                
                            continue;
                        }
                        if (j != counts - 1)                            
                            sb.AppendFormat("<img src='{0}' width='{1}' onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" style='margin-right:{7}%;'/>", dTable.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dTable.Rows[i * counts + j][1], dTable.Rows[i * counts + j][2], dTable.Rows[i * counts + j][3], imgSpan.ToString());
                        else
                            sb.AppendFormat("<img src='{0}' width='{1}'  onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" />", dTable.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dTable.Rows[i * counts + j][1], dTable.Rows[i * counts + j][2], dTable.Rows[i * counts + j][3]);
                        
                    }
                }
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        public string QueryImgUrl(DataTable dTable,string entityName,string[]info)
        {
            StringBuilder sb = new StringBuilder();
            int counts = int.Parse(info[3]);
            int allCounts = int.Parse(info[5]);
            double width = 102.0 / counts -3;
            for (int i = 0; i <= allCounts; i++)
            {
                sb.Append("<div style='margin-bottom:7px'>");
                for (int j = 0; j < counts; j++)
                {
                    if (i * counts + j + 1 <= allCounts)
                    {
                        if ((i * counts + j) >= dTable.Rows.Count)
                        {
                            sb.AppendFormat("<img src='{0}' width='{1}' onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" style='margin-right:3%'/>", "", width.ToString() + "%", entityName, "", "", "","");
                            continue;
                        }
                        if (j != counts - 1)
                            sb.AppendFormat("<img src='{0}' width='{1}' onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" style='margin-right:3%'/>", dTable.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dTable.Rows[i * counts + j][1], dTable.Rows[i * counts + j][2], dTable.Rows[i * counts + j][3]);
                        else
                            sb.AppendFormat("<img src='{0}' width='{1}'  onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" />", dTable.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dTable.Rows[i * counts + j][1], dTable.Rows[i * counts + j][2], dTable.Rows[i * counts + j][3]);
                    }
                }
                sb.Append("</div>");
            }
            return sb.ToString();
        }
        public string returnSQLStr(string type)
        {
            string module="(";
            string[] types = type.Split(',');
            for (int i = 0; i < types.Length; i++)
            {
                module = module + "'" + types[i] + "',";
            }
            module = module.Substring(0, module.Length - 1) + ")";
            return module;
        }
        public string ECCity(string entityName, string type, string column, string totalCount, string period)
        {
            IList<PropertyJsOV> properties = new List<PropertyJsOV>();
            properties = GetEntity(entityName);
            Entity entity = new Entity(m_Database, entityName);
            string forecastDate = GetLastestDatetime(entity);
            int counts = int.Parse(column);
            int allCounts = int.Parse(totalCount);
            double width = 100.0 / counts - 1.0;
            StringBuilder sb = new StringBuilder("\"");
            string entityObj = ToJSON(properties);
            string strWhere = " WHERE ForecastDate<='" + forecastDate + "' AND Type='"+type+"'";
            if (entity.Condition != "")
                strWhere = strWhere + " AND " + entity.Condition;
            string strSQL = "SELECT TOP(" + allCounts + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + "ORDER BY MC DESC";
            DataTable dt2 = m_Database.GetDataTable(strSQL);
            string jsonStr = "";
            for (int i = 0; i <= allCounts; i++)
            {
                sb.Append("<div style='margin-bottom:7px'>");
                for (int j = 0; j < counts; j++)
                {
                    if (i * counts + j + 1 <= allCounts)
                    {
                        if ((i * counts + j) >= dt2.Rows.Count)
                        {
                            sb.AppendFormat("<img src='{0}' width='{1}' onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" style='margin-right:10px'/>", "", width.ToString() + "%", entityName, "", "", "", "");
                            continue;
                        }
                        if (j == counts - 1)
                            sb.AppendFormat("<img src='{0}' width='{1}' onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" style='margin-right:10px'/>", dt2.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                        else
                            sb.AppendFormat("<img src='{0}' width='{1}'  onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" />", dt2.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                    }
                }
                sb.Append("</div>");
            }
            jsonStr = jsonStr + "{\"imgHtml\":" + sb.Append("\"}");
            return jsonStr;
        }
        public string AirQualityQueryList(string entityName, string json)
        {
            string[] entityNameArray = entityName.Split(',');
            string strSQL = "";
            Entity entity;
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
            string strWhere = " ";
            int counts = int.Parse(info[3]);
            int imgCount = int.Parse(info[5]);
            for (int i = 0; i < entityNameArray.Length; i++)
            {
                entity = new Entity(m_Database, entityNameArray[i]);
                strWhere = " WHERE ";
              
                if (entity.Condition != "")
                    strWhere = strWhere  + entity.Condition;
                if(info[7]!="")
                    strWhere = strWhere + " AND  Station='" + info[7] + "'";
                if (entityNameArray.Length == 1)
                    strSQL = strSQL + " SELECT TOP(" + imgCount + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period,'" + entityNameArray[i] + "' AS entityName FROM  " + entity.TableName + strWhere + " ORDER BY ForecastDate DESC  union ";
                else
                    strSQL = strSQL + " SELECT * FROM ( SELECT TOP(1)('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period,'" + entityNameArray[i] + "' AS entityName FROM  " + entity.TableName + strWhere + " ORDER BY  ForecastDate DESC) as  a"+i+" union all";         
            }
            int index = strSQL.LastIndexOf("union");
            if (entityNameArray.Length!= 1)
                strSQL = strSQL.Substring(0, index) + " ORDER BY  type";
            else
                strSQL = strSQL.Substring(0, index);
            DataTable dt2 = m_Database.GetDataTable(strSQL);


            int allCounts = dt2.Rows.Count;
            double width = 100.0 / counts - 1.0;
            StringBuilder sb = new StringBuilder();
            if (dt2.Rows.Count == 1)
            {
                if (info[7]=="01")
                    width = 60;
                else
                    width = 40;
                sb.Append("<div style='text-align:center;'>");
                sb.AppendFormat("<img src='{0}' width='{1}'  onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" />", dt2.Rows[0][0], width.ToString() + "%", entityName, "", dt2.Rows[0][1], dt2.Rows[0][2], dt2.Rows[0][3]);
                sb.Append("</div>");

            }
            else
            {
                for (int i = 0; i <= allCounts; i++)
                {
                    sb.Append("<div style='margin-bottom:7px'>");
                    for (int j = 0; j < counts; j++)
                    {
                        if (i * counts + j + 1 <= allCounts)
                        {
                            if ((i * counts + j) >= dt2.Rows.Count)
                            {
                                sb.AppendFormat("<img src='{0}' width='{1}' onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" style='margin-right:10px'/>", "", width.ToString() + "%", dt2.Rows[i * counts + j][4], "", "", "", "");
                                continue;
                            }
                            if (j == counts - 1)
                                sb.AppendFormat("<img src='{0}' width='{1}' onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" style='margin-right:10px'/>", dt2.Rows[i * counts + j][0], width.ToString() + "%", dt2.Rows[i * counts + j][4], "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                            else
                                sb.AppendFormat("<img src='{0}' width='{1}'  onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" />", dt2.Rows[i * counts + j][0], width.ToString() + "%", dt2.Rows[i * counts + j][4], "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                        }
                    }
                    sb.Append("</div>");
                }
            }
            string jsonStr = sb.ToString();
            return jsonStr;           
        }

        public string AirQualityQueryListX(string entityName, string json)
        {
            string[] entityNameArray = entityName.Split(',');
            string strSQL = "";
            Entity entity;
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
            string strWhere = " ";
            int counts = int.Parse(info[3]);
            int imgCount = int.Parse(info[5]);
            for (int i = 0; i < entityNameArray.Length; i++)
            {
                entity = new Entity(m_Database, entityNameArray[i]);
                strWhere = " WHERE ";

                if (entity.Condition != "")
                    strWhere = strWhere + entity.Condition;
                if (info[7] != "")
                    strWhere = strWhere + " AND  Station='" + info[7] + "'";
                if (entityNameArray.Length == 1)
                    strSQL = strSQL + " SELECT TOP(" + imgCount + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period,'" + entityNameArray[i] + "' AS entityName FROM  " + entity.TableName + strWhere + " ORDER BY ForecastDate DESC  union ";
                else
                    strSQL = strSQL + " SELECT * FROM ( SELECT TOP(1)('Product/' + folder + '/' + name) AS DM, ForecastDate  AS MC,TYPE,Period,'" + entityNameArray[i] + "' AS entityName FROM  " + entity.TableName + strWhere + " ORDER BY  ForecastDate DESC) as  a" + i + " union all";
            }
            int index = strSQL.LastIndexOf("union");
            if (entityNameArray.Length != 1)
                strSQL = strSQL.Substring(0, index) + " ORDER BY  type";
            else
                strSQL = strSQL.Substring(0, index);
            DataTable dt2 = m_Database.GetDataTable(strSQL);


            int allCounts = dt2.Rows.Count;
            double width = 100.0 / counts - 1.0;
            StringBuilder sb = new StringBuilder();
            if (dt2.Rows.Count == 1)
            {
                if (info[7] == "01")
                    width = 60;
                else
                    width = 40;
                sb.Append("<div style='text-align:center;'>");
                sb.AppendFormat("<img src='{0}' width='{1}'  onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" />", dt2.Rows[0][0], width.ToString() + "%", entityName, "", dt2.Rows[0][1], dt2.Rows[0][2], dt2.Rows[0][3]);
                sb.Append("</div>");

            }
            else
            {
                for (int i = 0; i <= allCounts; i++)
                {
                    sb.Append("<div style='margin-bottom:7px'>");
                    for (int j = 0; j < counts; j++)
                    {
                        if (i * counts + j + 1 <= allCounts)
                        {
                            if ((i * counts + j) >= dt2.Rows.Count)
                            {
                                sb.AppendFormat("<img src='{0}' width='{1}' onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" style='margin-right:10px'/>", "", width.ToString() + "%", dt2.Rows[i * counts + j][4], "", "", "", "");
                                continue;
                            }
                            if (j == counts - 1)
                                sb.AppendFormat("<img src='{0}' width='{1}' onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" style='margin-right:10px'/>", dt2.Rows[i * counts + j][0], width.ToString() + "%", dt2.Rows[i * counts + j][4], "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                            else
                                sb.AppendFormat("<img src='{0}' width='{1}'  onclick=\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\" />", dt2.Rows[i * counts + j][0], width.ToString() + "%", dt2.Rows[i * counts + j][4], "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                        }
                    }
                    sb.Append("</div>");
                }
            }
            string jsonStr = sb.ToString();
            return jsonStr;
        }

        public string PublicQueryListPast(string entityName,string json)
        {
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';',':' });
            IList<PropertyJsOV> properties = new List<PropertyJsOV>();
            properties = GetEntity(entityName);
            string forecastDate = properties[0].ShowValue;
            string startTime = DateTime.Parse(forecastDate).AddDays(-10).ToString("yyyy-MM-dd HH:00:00");
            StringBuilder sb = new StringBuilder();
            string entityObj = ToJSON(properties);
            
            DataTable timeList = new DataTable();//时间列表
            DataTable dt2 = new DataTable();//数据
            if (info[7] != ""&& info[1] != "全国" && info[1] != "华东")
            {
                if (info[1] != "")
                   dt2= QueryModuleList(entityName,json);
                else 
                    dt2 = QueryList(entityName, entityObj);
                properties[0].ShowValue = startTime + "||" + forecastDate;
                entityObj = ToJSON(properties);
                timeList = QueryTimeList(entityName, entityObj,json);
                DataView dv = timeList.DefaultView;
                dv.Sort = "MC DESC";
                timeList = dv.ToTable();
                sb.Append("<label class='cur-select' id='labelSelect'>" + timeList.Rows[0][0] + "</label>");
                sb.Append("<select id='selectID'>");
                int k = 0;
                foreach (DataRow rows in timeList.Rows)
                {
                    if (k == 0)
                    {
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", rows[0]);
                        k = 1;
                    }
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", rows[0]);
                }
            }
            else
            {
                properties[0].ShowValue = startTime + "||" + forecastDate;
                entityObj = ToJSON(properties);
                if (info[1] != "")
                    timeList = QueryListAirQuality(entityName, entityObj,info[7]);
                else 
                    timeList = QueryList(entityName, entityObj);
                DataView dv = timeList.DefaultView;
                dv.Sort = "MC DESC";
                dt2 = dv.ToTable();
                //sb.Append("<label class='cur-select' id='labelSelect'>" + dt2.Rows[0][1] + "</label>");
                //sb.Append("<select id='selectID'>");


                sb.Append("<select id='selectID'>" + dt2.Rows[0][1]);
                int k = 0;
                foreach (DataRow rows in dt2.Rows)
                {
                    if (k == 0)
                    {
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", rows[1]);
                        k = 1;
                    }
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", rows[1]);
                }
            }           
            sb.Append("</select>");
            string jsonStr = "{\"date\":\"" + sb.ToString() + "\",";
            int counts = int.Parse(info[3]);
            int allCounts = int.Parse(info[5]);
            double width = 100.0 / counts - 1.0;
            sb = new StringBuilder("\"");
            for (int i = 0; i <= allCounts; i++)
            {
                sb.Append("<div style='margin-bottom:7px'>");
                for (int j = 0; j < counts; j++)
                {
                    if (i * counts + j + 1 <= allCounts)
                    {
                        if ((i * counts + j) >= dt2.Rows.Count)
                        {
                            sb.AppendFormat("<img src='{0}' width='{1}' onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" style='margin-right:10px'/>", "", width.ToString() + "%", entityName, "", "", "","");
                            continue;
                        }
                        if (j == counts - 1)
                            sb.AppendFormat("<img src='{0}' width='{1}' onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" style='margin-right:10px'/>", dt2.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                        else
                            sb.AppendFormat("<img src='{0}' width='{1}'  onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" />", dt2.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                    }
                }
                sb.Append("</div>");
            }
            jsonStr = jsonStr + "\"contentNone\":" + sb.Append("\"}");
            return jsonStr;

        }

        //将select下拉菜单改写为ul li形式
        public string PublicQueryList(string entityName, string json)
        {
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
            IList<PropertyJsOV> properties = new List<PropertyJsOV>();
            properties = GetEntity(entityName);
            string forecastDate = properties[0].ShowValue;
            string startTime = DateTime.Parse(forecastDate).AddDays(-10).ToString("yyyy-MM-dd HH:00:00");
            StringBuilder sb = new StringBuilder();
            string entityObj = ToJSON(properties);

            DataTable timeList = new DataTable();//时间列表
            DataTable dt2 = new DataTable();//数据
            if (info[7] != "" && info[1] != "全国" && info[1] != "华东" && info[1] != "江西")
            {
                if (info[1] != "")
                    dt2 = QueryModuleList(entityName, json);
                else
                    dt2 = QueryList(entityName, entityObj);
                properties[0].ShowValue = startTime + "||" + forecastDate;
                entityObj = ToJSON(properties);
                timeList = QueryTimeList(entityName, entityObj, json);
                DataView dv = timeList.DefaultView;
                dv.Sort = "MC DESC";
                timeList = dv.ToTable();
                sb.Append("<label class='cur-select' id='labelSelect'>" + timeList.Rows[0][0] + "</label>");
                sb.Append("<select id='selectID'>");
                int k = 0;
                foreach (DataRow rows in timeList.Rows)
                {
                    if (k == 0)
                    {
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", rows[0]);
                        k = 1;
                    }
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", rows[0]);
                }
            }
            else
            {
                properties[0].ShowValue = startTime + "||" + forecastDate;
                entityObj = ToJSON(properties);
                if (info[1] != "")
                    timeList = QueryListAirQuality(entityName, entityObj, info[7]);
                else
                    timeList = QueryList(entityName, entityObj);
                DataView dv = timeList.DefaultView;
                dv.Sort = "MC DESC";
                dt2 = dv.ToTable();
                //sb.Append("<label class='cur-select' id='labelSelect'>" + dt2.Rows[0][1] + "</label>");
                //sb.Append("<select id='selectID'>");


                sb.Append("<div id='selectID' class='dateDiv'>" +"<div id='dateTxt'>"+ dt2.Rows[0][1] +"</div>"+ "<div id='selIcon' class='selIcon'></div></div>");
                int k = 0;
                sb.Append("<ul id='dateUl' class='dateUl'>");
                foreach (DataRow rows in dt2.Rows)
                {
                    if (k == 0)
                    {
                        sb.AppendFormat("<li>{0}</li>", rows[1]);
                        k = 1;
                    }
                    else
                        sb.AppendFormat("<li>{0}</li>", rows[1]);
                }
            }
            sb.Append("</ul>");
            string jsonStr = "{\"date\":\"" + sb.ToString() + "\",";
            int counts = int.Parse(info[3]);
            int allCounts = int.Parse(info[5]);
            double width = 100.0 / counts - 1.0;
            sb = new StringBuilder("\"");
            for (int i = 0; i <= allCounts; i++)
            {
                sb.Append("<div style='margin-bottom:7px'>");
                for (int j = 0; j < counts; j++)
                {
                    if (i * counts + j + 1 <= allCounts)
                    {
                        if ((i * counts + j) >= dt2.Rows.Count)
                        {
                            sb.AppendFormat("<img src='{0}' width='{1}' onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" style='margin-right:10px'/>", "", width.ToString() + "%", entityName, "", "", "", "");
                            continue;
                        }
                        if (j == counts - 1)
                            sb.AppendFormat("<img src='{0}' width='{1}' onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" style='margin-right:10px'/>", dt2.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                        else
                            sb.AppendFormat("<img src='{0}' width='{1}'  onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" />", dt2.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                    }
                }
                sb.Append("</div>");
            }
            jsonStr = jsonStr + "\"contentNone\":" + sb.Append("\"}");
            return jsonStr;

        }


        public string PublicQueryListX(string entityName, string json)
        {
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
            IList<PropertyJsOV> properties = new List<PropertyJsOV>();
            properties = GetEntity(entityName);
            string forecastDate = properties[0].ShowValue;
            string startTime = DateTime.Parse(forecastDate).AddDays(-10).ToString("yyyy-MM-dd HH:00:00");
            StringBuilder sb = new StringBuilder();
            string entityObj = ToJSON(properties);

            DataTable timeList = new DataTable();//时间列表
            DataTable dt2 = new DataTable();//数据
            if (info[7] != "" && info[1] != "全国" && info[1] != "华东" && info[1] != "江西")
            {
                if (info[1] != "")
                    dt2 = QueryModuleList(entityName, json);
                else
                    dt2 = QueryList(entityName, entityObj);
                properties[0].ShowValue = startTime + "||" + forecastDate;
                entityObj = ToJSON(properties);
                timeList = QueryTimeList(entityName, entityObj, json);
                DataView dv = timeList.DefaultView;
                dv.Sort = "MC DESC";
                timeList = dv.ToTable();
                sb.Append("<label class='cur-select' id='labelSelect'>" + timeList.Rows[0][0] + "</label>");
                sb.Append("<select id='selectID' style='display:none;'>");
                int k = 0;
                foreach (DataRow rows in timeList.Rows)
                {
                    if (k == 0)
                    {
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", rows[0]);
                        k = 1;
                    }
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", rows[0]);
                }
            }
            else
            {
                properties[0].ShowValue = startTime + "||" + forecastDate;
                entityObj = ToJSON(properties);
                if (info[1] != "")
                    timeList = QueryListAirQuality(entityName, entityObj, info[7]);
                else
                    timeList = QueryList(entityName, entityObj);
                DataView dv = timeList.DefaultView;
                dv.Sort = "MC DESC";
                dt2 = dv.ToTable();
                //sb.Append("<label class='cur-select' id='labelSelect'>" + dt2.Rows[0][1] + "</label>");
                //sb.Append("<select id='selectID'>");


                sb.Append("<div id='selectID' class='dateDiv'>" + "<div id='dateTxt'>" + dt2.Rows[0][1] + "</div>" + "<div name='time' id='selIcon' class='selIcon' onclick='WdatePicker({dateFmt:'yyyy-MM-dd'})' onchange='changeDate(this)' /></div>");
                int k = 0;
                sb.Append("<ul id='dateUl' class='dateUl' style='display:none'>");
                foreach (DataRow rows in dt2.Rows)
                {
                    if (k == 0)
                    {
                       // sb.AppendFormat("<li>{0}</li>", rows[1]);
                        k = 1;
                    }
                   // else
                      //  sb.AppendFormat("<li>{0}</li>", rows[1]);
                }
            }
           // sb.Append("</ul>");
            string jsonStr = "{";
            int counts = int.Parse(info[3]);
            int allCounts = int.Parse(info[5]);
            double width = 100.0 / counts - 1.0;
            sb = new StringBuilder("\"");
            for (int i = 0; i <= allCounts; i++)
            {
                sb.Append("<div style='margin-bottom:7px'>");
                for (int j = 0; j < counts; j++)
                {
                    if (i * counts + j + 1 <= allCounts)
                    {
                        if ((i * counts + j) >= dt2.Rows.Count)
                        {
                            sb.AppendFormat("<img src='{0}' width='{1}' onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" style='margin-right:10px'/>", "", width.ToString() + "%", entityName, "", "", "", "");
                            continue;
                        }
                        if (j == counts - 1)
                            sb.AppendFormat("<img src='{0}' width='{1}' onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" style='margin-right:10px'/>", dt2.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                        else
                            sb.AppendFormat("<img src='{0}' width='{1}'  onclick=\\\"showOne('{2}','{3}','{0}','{4}','{5}','{6}')\\\" />", dt2.Rows[i * counts + j][0], width.ToString() + "%", entityName, "", dt2.Rows[i * counts + j][1], dt2.Rows[i * counts + j][2], dt2.Rows[i * counts + j][3]);
                    }
                }
                sb.Append("</div>");
            }
            jsonStr = jsonStr + "\"contentNone\":" + sb.Append("\"}");
            return jsonStr;

        }
        //下拉时间列表数据获取
        public DataTable QueryTimeList(string entityName, string entityObj,string json)
        {
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });

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
            if (info[1] != "全国" && info[1] != "华东")
            {
                if (info[7] != "")
                {
                    if (info[7].IndexOf(",") <= 0)
                        strWhere = strWhere + "  AND Type = '" + info[7] + "'";
                    else
                        strWhere = strWhere + "  AND Type in " + returnSQLStr(info[7]);
                }
            }
            try
            {
                string strSQL = "SELECT distinct CONVERT(varchar(16),DATEADD(hour, CONVERT(int, Period), ForecastDate), 120) AS MC,Period AS DM FROM  " + entity.TableName + strWhere + "ORDER BY MC ";
                DataTable dt = m_Database.GetDataTable(strSQL);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //模式数据查询列表
        public DataTable QueryModuleList(string entityName,string json)
        {
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
            IList<PropertyJsOV> properties = new List<PropertyJsOV>();
            properties = GetEntity(entityName);
            DataTable dt = new DataTable();
            string forecastDate = properties[0].ShowValue;
            Entity entity = new Entity(m_Database, entityName);
            string strWhere =" WHERE   ForecastDate='" + forecastDate + "'";
            if (entity.Condition != "")
                strWhere = strWhere +" AND " +entity.Condition;//+ " AND " 
            strWhere = strWhere + "  AND Type in " + returnSQLStr(info[7]);
            string strSQL = "SELECT TOP(" + info[5] + ") ('Product/' + folder + '/' + name) AS DM, ForecastDate AS MC,Type,Period FROM   " + entity.TableName + strWhere + "ORDER BY ForecastDate,Period DESC";
            dt = m_Database.GetDataTable(strSQL);
            return dt;
        }
        public string AirQualityBottomSelect(string Datetime, string entityName, string json, string type)
        {
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
            StringBuilder sb = new StringBuilder("{");
            DataTable dt = new DataTable();
            int hour = DateTime.Parse(Datetime).Hour;
            string startTime = DateTime.Parse(Datetime).ToString("yyyy-MM-dd 00:00:00");
            string endTime = DateTime.Parse(Datetime).ToString("yyyy-MM-dd 23:59:59");
            Entity entity = new Entity(m_Database, entityName);
            string strWhere = "  WHERE ";
            if (entity.Condition != "")
                strWhere = strWhere + entity.Condition + " AND ";//+ " AND " 
            if (info[7] != "")
                strWhere = strWhere + " Station='" + info[7] + "'  AND ";
            if (info[9] == "")
            {
                strWhere = strWhere + "   ForecastDate BETWEEN'" + startTime + "' AND '" + endTime + "'";
                string strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, datepart(hour,ForecastDate) as hour FROM   " + entity.TableName + strWhere + "ORDER BY ForecastDate,Period DESC";
                dt = m_Database.GetDataTable(strSQL);
                sb.Append("'addBut':\"");
                sb.Append("<select id='selectHour' onchange='selectChange()'>");
                foreach (DataRow rows in dt.Rows)
                {
                    if (rows[1].ToString() == hour.ToString())
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", rows[1]);
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", rows[1]);
                }
                sb.Append("</select >");
                sb.Append("\"");
            }
            sb.Append("}");
            string jsonStr = sb.ToString();
            return jsonStr;
        }

        public string CreateBottomSelect(string Datetime, string entityName, string json, string type, string Period)
        {
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
            StringBuilder sb = new StringBuilder("{");
            DataTable dt = new DataTable();
            int hour = DateTime.Parse(Datetime).Hour;
            string startTime = DateTime.Parse(Datetime).ToString("yyyy-MM-dd 00:00:00");
            string endTime = DateTime.Parse(Datetime).ToString("yyyy-MM-dd 23:59:59");
            Entity entity = new Entity(m_Database, entityName);
            string strWhere = "  WHERE ";
            if (entity.Condition != "")
                strWhere = strWhere + entity.Condition + " AND ";//+ " AND " 
            if (info[7] != "")
                strWhere = strWhere + " Type='" + type + "'  AND ";
            if (info[9] == "")
            {
                strWhere = strWhere + "   ForecastDate BETWEEN'" + startTime + "' AND '" + endTime + "'";
                string strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, datepart(hour,ForecastDate) as hour FROM   " + entity.TableName + strWhere + "ORDER BY ForecastDate,Period DESC";
                dt = m_Database.GetDataTable(strSQL);
                sb.Append("'addBut':\"");
                sb.Append("<select id='selectHour' onchange='selectChange()'>");
                foreach (DataRow rows in dt.Rows)
                {
                    if (rows[1].ToString() == hour.ToString())
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", rows[1]);
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", rows[1]);
                }
                sb.Append("</select >");
                sb.Append("\"");
            }
            else
            {
                strWhere = strWhere + "   ForecastDate ='" + DateTime.Parse(Datetime).ToString("yyyy-MM-dd HH:00:00") + "'";
                string strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, Period as hour FROM   " + entity.TableName + strWhere + "ORDER BY Period ";
                dt = m_Database.GetDataTable(strSQL);
                sb.Append("'addBut':\"");
                sb.Append("<select id='selectHour' onchange='selectChange()'>");
                foreach (DataRow rows in dt.Rows)
                {
                    if (rows[1].ToString() == Period)
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", rows[1]);
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", rows[1]);
                }
                sb.Append("</select >");
                sb.Append("\",");
                sb.Append("'period':\"");
                sb.Append("<select id='selectperiod' >");
                string[] periodArray = info[9].Split(',');
                for (int i = 0; i < periodArray.Length; i++)
                {
                    if (periodArray[i] == DateTime.Parse(Datetime).Hour.ToString())
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", periodArray[i]);
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", periodArray[i]);
                }
                sb.Append("</select >");
                sb.Append("\"");
            }
            sb.Append("}");
            string jsonStr = sb.ToString();
            return jsonStr;
        }
        public string ReduceButton(string entityName, string dateTime, string hour, string json, string type, string period)
        {
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
            StringBuilder sb = new StringBuilder("{");
            DataTable dt = new DataTable();
            int hours = 0;
            string forecastDate = "";
            string strWhere = " WHERE ";
            forecastDate = DateTime.Parse(dateTime).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
            Entity entity = new Entity(m_Database, entityName);
            string strSQL = "";

            if (entity.Condition != "")
                strWhere = strWhere + entity.Condition + "  AND ";//+ " AND "      
            if (info[7] != "")
            {
                if (period == "airQuality")
                    strWhere = strWhere + "  Station='" + info[7] + "'  AND";
                else
                    strWhere = strWhere + "  Type='" + type + "'  AND";
            }
            if (hour != "-1" && hour != "-2")//-1标示时间往前查询，-2标示时间往后查询
            {
                if (info[9] == "")
                {
                    hours = int.Parse(hour);
                    forecastDate = DateTime.Parse(dateTime).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
                    strWhere = strWhere + "   ForecastDate= '" + forecastDate + "'";
                }
                else
                {
                    hours = int.Parse(period);
                    forecastDate = DateTime.Parse(dateTime).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
                    strWhere = strWhere + "   ForecastDate= '" + forecastDate + "' AND Period='" + hour + "'";
                }
                strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, datepart(hour,ForecastDate) as hour FROM   " + entity.TableName + strWhere + "ORDER BY ForecastDate,Period DESC";

            }
            else
            {

                string str = "";
                if (info[9] == "")
                {
                    if (hour == "-1")
                    {
                        str = "   ForecastDate <'" + forecastDate + "'";
                        strSQL = "SELECT  TOP(1) ForecastDate  FROM   " + entity.TableName + strWhere + str + " ORDER BY ForecastDate DESC";
                    }
                    else
                    {
                        forecastDate = DateTime.Parse(dateTime).AddDays(1).ToString("yyyy-MM-dd HH:00:00");
                        str = "   ForecastDate >='" + forecastDate + "'";
                        strSQL = "SELECT  TOP(1) ForecastDate  FROM   " + entity.TableName + strWhere + str + " ORDER BY ForecastDate ";
                    }
                    forecastDate = m_Database.GetFirstValue(strSQL);
                    if (forecastDate == "")
                        return "";
                    string startTime = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 00:00:00");
                    string endTime = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 23:59:59");
                    strWhere = strWhere + "    ForecastDate BETWEEN'" + startTime + "' AND '" + endTime + "'";
                    strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, datepart(hour,ForecastDate) as hour FROM   " + entity.TableName + strWhere + "ORDER BY ForecastDate,Period DESC";
                }
                else
                {
                    hours = int.Parse(period);
                    if (hour == "-1")
                        forecastDate = DateTime.Parse(dateTime).AddDays(-1).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
                    else
                        forecastDate = DateTime.Parse(dateTime).AddDays(1).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
                    strWhere = strWhere + "    ForecastDate ='" + forecastDate + "' ";
                    strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, Period FROM   " + entity.TableName + strWhere + " ORDER BY Period ";
                }
            }

            dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count <= 0)
                return "";
            string src = dt.Rows[0][0].ToString();
            if (hour == "-1" || hour == "-2")
            {
                sb.Append("'addBut':\"");
                sb.Append("<select id='selectHour' onchange='selectChange()'>");
                foreach (DataRow rows in dt.Rows)
                {
                    string selectValue = "";
                    if (info[9] == "")
                        selectValue = DateTime.Parse(forecastDate).Hour.ToString();
                    else
                    {
                        if (hour == "-1")
                            selectValue = dt.Rows[dt.Rows.Count - 1][1].ToString();
                        else
                            selectValue = dt.Rows[0][1].ToString();
                    }
                    if (rows[1].ToString() == selectValue)
                    {
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", rows[1]);
                        src = rows[0].ToString();
                    }
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", rows[1]);
                }
                sb.Append("</select >");
                sb.Append("\",");
                sb.AppendFormat("'time':'{0}',", DateTime.Parse(forecastDate).ToString("yyyy-MM-dd"));

            }
            sb.AppendFormat("'src':'{0}'", src);
            sb.Append("}");
            string jsonStr = sb.ToString();
            return jsonStr;
        }

        //public string CreateBottomSelect(string Datetime, string entityName, string json, string type, string Period)
        //{
        //    string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
        //    StringBuilder sb = new StringBuilder("{");
        //    DataTable dt = new DataTable();
        //    int hour = DateTime.Parse(Datetime).Hour;
        //    string startTime = DateTime.Parse(Datetime).ToString("yyyy-MM-dd 00:00:00");
        //    string endTime = DateTime.Parse(Datetime).ToString("yyyy-MM-dd 23:59:59");
        //    Entity entity = new Entity(m_Database, entityName);
        //    string strWhere = "  WHERE ";
        //    if (entity.Condition != "")
        //        strWhere = strWhere + entity.Condition + " AND ";//+ " AND " 
        //    if (info[7] != "")
        //        strWhere = strWhere + " Type='" + type + "'  AND ";
        //    if (info[9] == "")
        //    {
        //        strWhere = strWhere + "   ForecastDate BETWEEN'" + startTime + "' AND '" + endTime + "'";
        //        string strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, datepart(hour,ForecastDate) as hour FROM   " + entity.TableName + strWhere + "ORDER BY ForecastDate,Period DESC";
        //        dt = m_Database.GetDataTable(strSQL);
        //        sb.Append("'addBut':\"");
        //        sb.Append("<select id='selectHour' onchange='selectChange()'>");
        //        foreach (DataRow rows in dt.Rows)
        //        {

        //            string value = DateTime.Parse(Datetime).ToString("yyyy-MM-dd HH:mm:ss");//.AddHours(int.Parse(rows[1].ToString())).ToString("yyyy-MM-dd HH:mm:ss");// xuehui 2016-08-15

        //            if (rows[1].ToString() == hour.ToString())
        //                sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", value);
        //            else
        //                sb.AppendFormat("<option value ='{0}' >{0}</option>", value);
        //        }
        //        sb.Append("</select >");
        //        sb.Append("\"");
        //    }
        //    else
        //    {
        //        strWhere = strWhere + "   ForecastDate ='" + DateTime.Parse(Datetime).ToString("yyyy-MM-dd HH:00:00") + "'";
        //        string strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, Period as hour FROM   " + entity.TableName + strWhere + "ORDER BY Period ";
        //        dt = m_Database.GetDataTable(strSQL);
        //        sb.Append("'addBut':\"");
        //        sb.Append("<select id='selectHour' onchange='selectChange()'>");

        //        // string sx = (json.Split(new string[] { "P:" }, StringSplitOptions.RemoveEmptyEntries)[1]).
        //        // Substring(0, (json.Split(new string[] { "P:" }, StringSplitOptions.RemoveEmptyEntries)[1]).IndexOf(';'));

        //        foreach (DataRow rows in dt.Rows)
        //        {
        //            string value = DateTime.Parse(Datetime).AddHours(int.Parse(rows[1].ToString())).ToString("yyyy-MM-dd HH:mm:ss");// xuehui 2016-08-15

        //            if (rows[1].ToString() == Period)
        //                sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", value);
        //            else
        //                sb.AppendFormat("<option value ='{0}' >{0}</option>", value);
        //        }
        //        sb.Append("</select >");
        //        sb.Append("\",");
        //        sb.Append("'period':\"");
        //        sb.Append("<select id='selectperiod' style='display:none'>");
        //        string[] periodArray = info[9].Split(',');
        //        for (int i = 0; i < periodArray.Length; i++)
        //        {
        //            if (periodArray[i] == DateTime.Parse(Datetime).Hour.ToString())
        //                sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", periodArray[i]);
        //            else
        //                sb.AppendFormat("<option value ='{0}' >{0}</option>", periodArray[i]);
        //        }
        //        sb.Append("</select >");
        //        sb.Append("\"");
        //    }
        //    sb.Append("}");
        //    string jsonStr = sb.ToString();
        //    return jsonStr;
        //}
        //public string ReduceButton(string entityName, string dateTime, string hour, string json, string type, string period)
        //{
        //    string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
        //    StringBuilder sb = new StringBuilder("{");
        //    DataTable dt = new DataTable();
        //    int hours = 0;
        //    string hour2 = "X";
        //    string forecastDate = "";
        //    string strWhere = " WHERE ";
        //    forecastDate = DateTime.Parse(dateTime).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
        //    Entity entity = new Entity(m_Database, entityName);
        //    string strSQL = "";

        //    if (entity.Condition != "")
        //        strWhere = strWhere + entity.Condition + "  AND ";//+ " AND "      
        //    if (info[7] != "")
        //    {
        //        if (period == "airQuality")
        //            strWhere = strWhere + "  Station='" + info[7] + "'  AND";
        //        else
        //            strWhere = strWhere + "  Type='" + type + "'  AND";
        //    }

        //    //XUEHUI 2016-08-15
        //    try
        //    {
        //        DateTime DT1 = DateTime.Parse(dateTime).AddHours(int.Parse(period));
        //        DateTime DT2 = DateTime.Parse(hour);
        //        TimeSpan TS = DT2 - DT1;
        //        hour = TS.TotalHours.ToString().PadLeft(4, '0');//重新计算
        //        hour2 = TS.TotalHours.ToString().PadLeft(3, '0');//重新计算;
        //    }
        //    catch
        //    {

        //    }

        //    if (hour != "-1" && hour != "-2")//-1标示时间往前查询，-2标示时间往后查询
        //    {
        //        if (info[9] == "")
        //        {
        //            hours = int.Parse(hour);
        //            forecastDate = DateTime.Parse(dateTime).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
        //            strWhere = strWhere + "   ForecastDate= '" + forecastDate + "'";
        //        }
        //        else
        //        {
        //            hours = int.Parse(period);
        //            forecastDate = DateTime.Parse(dateTime).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
        //            strWhere = strWhere + "   ForecastDate= '" + forecastDate + "' AND (Period='" + hour + "' or Period='" + hour2 + "') ";
        //        }
        //        strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, datepart(hour,ForecastDate) as hour FROM   " + entity.TableName + strWhere + "ORDER BY ForecastDate,Period DESC";

        //    }
        //    else
        //    {

        //        string str = "";
        //        if (info[9] == "")
        //        {
        //            if (hour == "-1")
        //            {
        //                str = "   ForecastDate <'" + forecastDate + "'";
        //                strSQL = "SELECT  TOP(1) ForecastDate  FROM   " + entity.TableName + strWhere + str + " ORDER BY ForecastDate DESC";
        //            }
        //            else
        //            {
        //                forecastDate = DateTime.Parse(dateTime).AddDays(1).ToString("yyyy-MM-dd HH:00:00");
        //                str = "   ForecastDate >='" + forecastDate + "'";
        //                strSQL = "SELECT  TOP(1) ForecastDate  FROM   " + entity.TableName + strWhere + str + " ORDER BY ForecastDate ";
        //            }
        //            forecastDate = m_Database.GetFirstValue(strSQL);
        //            if (forecastDate == "")
        //                return "";
        //            string startTime = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 00:00:00");
        //            string endTime = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 23:59:59");
        //            strWhere = strWhere + "    ForecastDate BETWEEN'" + startTime + "' AND '" + endTime + "'";
        //            strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, datepart(hour,ForecastDate) as hour FROM   " + entity.TableName + strWhere + "ORDER BY ForecastDate,Period DESC";
        //        }
        //        else
        //        {
        //            hours = int.Parse(period);
        //            if (hour == "-1")
        //                forecastDate = DateTime.Parse(dateTime).AddDays(-1).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
        //            else
        //                forecastDate = DateTime.Parse(dateTime).AddDays(1).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
        //            strWhere = strWhere + "    ForecastDate ='" + forecastDate + "' ";
        //            strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, Period FROM   " + entity.TableName + strWhere + " ORDER BY Period ";
        //        }
        //    }

        //    dt = m_Database.GetDataTable(strSQL);
        //    if (dt.Rows.Count <= 0)
        //        return "";
        //    string src = dt.Rows[0][0].ToString();
        //    //if (hour == "-1" || hour == "-2")//xuehui 2016-08-15
        //    //{
        //    //    sb.Append("'addBut':\"");
        //    //    sb.Append("<select id='selectHour' onchange='selectChange()'>");
        //    //    foreach (DataRow rows in dt.Rows)
        //    //    {
        //    //        string selectValue = "";
        //    //        if (info[9] == "")
        //    //            selectValue = DateTime.Parse(forecastDate).Hour.ToString();
        //    //        else
        //    //        {
        //    //            if (hour == "-1")
        //    //                selectValue = dt.Rows[dt.Rows.Count - 1][1].ToString();
        //    //            else
        //    //                selectValue = dt.Rows[0][1].ToString();
        //    //        }
        //    //        if (rows[1].ToString() == selectValue)
        //    //        {
        //    //            sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", rows[1]);
        //    //            src = rows[0].ToString();
        //    //        }
        //    //        else
        //    //            sb.AppendFormat("<option value ='{0}' >{0}</option>", rows[1]);
        //    //    }
        //    //    sb.Append("</select >");
        //    //    sb.Append("\",");
        //    //    sb.AppendFormat("'time':'{0}',", DateTime.Parse(forecastDate).ToString("yyyy-MM-dd"));

        //    //}

        //    if (hour == "-1" || hour == "-2")//xuehui 2016-08-15
        //    {
        //        return "";

        //    }
        //    sb.AppendFormat("'src':'{0}'", src);
        //    sb.Append("}");
        //    string jsonStr = sb.ToString();
        //    return jsonStr;
        //}

        public string CreateBottomSelectII(string Datetime, string entityName, string json, string type, string Period, string src)
        {
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
            StringBuilder sb = new StringBuilder("{");
            DataTable dt = new DataTable();
            int hour = DateTime.Parse(Datetime).Hour;
            string startTime = DateTime.Parse(Datetime).ToString("yyyy-MM-dd 00:00:00");
            string endTime = DateTime.Parse(Datetime).ToString("yyyy-MM-dd 23:59:59");
            Entity entity = new Entity(m_Database, entityName);
            string strWhere = "  WHERE ";


            if (entityName == "PMZH")
            {
                if (src.IndexOf("PM25") >= 0)
                {
                    strWhere = strWhere + " Layers='01' " + "  AND ";//+ " AND " 
                }
                else if (src.IndexOf("PM10") >= 0)
                {
                    strWhere = strWhere + " Layers='02' " + "  AND ";//+ " AND " 
                }
                else if (src.IndexOf("O3") >= 0)
                {
                    strWhere = strWhere + " Layers='03'  " + "  AND ";//+ " AND " 
                }
                else if (src.IndexOf("NO2") >= 0)
                {
                    strWhere = strWhere + " Layers='09' " + "  AND ";//+ " AND " 
                }
            }
            else if (entityName == "HysplitBackward") //09-30
            {
                if (src.IndexOf("Backward100") >= 0)
                {
                    strWhere = strWhere + " Layers='01' " + "  AND ";//+ " AND " 
                }
                else if (src.IndexOf("Backward1500") >= 0)
                {
                    strWhere = strWhere + " Layers='03' " + "  AND ";//+ " AND " 
                }
                else if (src.IndexOf("Backward500") >= 0)
                {
                    strWhere = strWhere + " Layers='02'  " + "  AND ";//+ " AND " 
                }
            }
            else
            {
                if (entity.Condition != "")
                    strWhere = strWhere + entity.Condition + "  AND ";//+ " AND " 
            }


            if (info[7] != "")
                strWhere = strWhere + " Type='" + type + "'  AND ";




            if (info[9] == "")
            {
                strWhere = strWhere + "   ForecastDate BETWEEN'" + startTime + "' AND '" + endTime + "'";
                string strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, datepart(hour,ForecastDate) as hour FROM   " + entity.TableName + strWhere + "ORDER BY ForecastDate,Period DESC";
                dt = m_Database.GetDataTable(strSQL);
                sb.Append("'addBut':\"");
                sb.Append("<select id='selectHour' onchange='selectChange()'>");
                foreach (DataRow rows in dt.Rows)
                {

                    string value = DateTime.Parse(Datetime).ToString("yyyy-MM-dd HH:mm:ss");//.AddHours(int.Parse(rows[1].ToString())).ToString("yyyy-MM-dd HH:mm:ss");// xuehui 2016-08-15

                    if (rows[1].ToString() == hour.ToString())
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", value);
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", value);
                }
                sb.Append("</select >");
                sb.Append("\"");
            }
            else
            {
                strWhere = strWhere + "   ForecastDate ='" + DateTime.Parse(Datetime).ToString("yyyy-MM-dd HH:00:00") + "'";
                string strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, Period as hour FROM   " + entity.TableName + strWhere + "ORDER BY Period ";
                dt = m_Database.GetDataTable(strSQL);
                sb.Append("'addBut':\"");
                sb.Append("<select id='selectHour' onchange='selectChange()'>");

                // string sx = (json.Split(new string[] { "P:" }, StringSplitOptions.RemoveEmptyEntries)[1]).
                // Substring(0, (json.Split(new string[] { "P:" }, StringSplitOptions.RemoveEmptyEntries)[1]).IndexOf(';'));

                foreach (DataRow rows in dt.Rows)
                {
                    string value = DateTime.Parse(Datetime).AddHours(int.Parse(rows[1].ToString())).ToString("yyyy-MM-dd HH:mm:ss");// xuehui 2016-08-15

                    if (rows[1].ToString() == Period)
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", value);
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", value);
                }
                sb.Append("</select >");
                sb.Append("\",");
                sb.Append("'period':\"");
                sb.Append("<select id='selectperiod' style='display:none'>");
                string[] periodArray = info[9].Split(',');
                for (int i = 0; i < periodArray.Length; i++)
                {
                    if (periodArray[i] == DateTime.Parse(Datetime).Hour.ToString())
                        sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", periodArray[i]);
                    else
                        sb.AppendFormat("<option value ='{0}' >{0}</option>", periodArray[i]);
                }
                sb.Append("</select >");
                sb.Append("\"");
            }
            sb.Append("}");
            string jsonStr = sb.ToString();
            return jsonStr;
        }

        public string ReduceButtonII(string entityName, string dateTime, string hour, string json, string type, string period, string src)
        {
            string[] info = json.Substring(1, json.Length - 2).Split(new char[] { ';', ':' });
            StringBuilder sb = new StringBuilder("{");
            DataTable dt = new DataTable();
            int hours = 0;
            string hour2 = "X";
            string forecastDate = "";
            string strWhere = " WHERE ";
            forecastDate = DateTime.Parse(dateTime).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
            Entity entity = new Entity(m_Database, entityName);
            string strSQL = "";


            if (entityName == "PMZH")
            {
                if (src.IndexOf("PM25") >= 0)
                {
                    strWhere = strWhere + " Layers='01' " + "  AND ";//+ " AND " 
                }
                else if (src.IndexOf("PM10") >= 0)
                {
                    strWhere = strWhere + " Layers='02' " + "  AND ";//+ " AND " 
                }
                else if (src.IndexOf("O3") >= 0)
                {
                    strWhere = strWhere + " Layers='03'  " + "  AND ";//+ " AND " 
                }
                else if (src.IndexOf("NO2") >= 0)
                {
                    strWhere = strWhere + " Layers='09' " + "  AND ";//+ " AND " 
                }
            }
            else if (entityName == "HysplitBackward") //09-30
            {
                if (src.IndexOf("Backward100") >= 0)
                {
                    strWhere = strWhere + " Layers='01' " + "  AND ";//+ " AND " 
                }
                else if (src.IndexOf("Backward1500") >= 0)
                {
                    strWhere = strWhere + " Layers='03' " + "  AND ";//+ " AND " 
                }
                else if (src.IndexOf("Backward500") >= 0)
                {
                    strWhere = strWhere + " Layers='02'  " + "  AND ";//+ " AND " 
                }
            }
            else
            {
                if (entity.Condition != "")
                    strWhere = strWhere + entity.Condition + "  AND ";//+ " AND " 
            }

            if (info[7] != "")
            {
                if (period == "airQuality")
                    strWhere = strWhere + "  Station='" + info[7] + "'  AND";
                else
                    strWhere = strWhere + "  Type='" + type + "'  AND";
            }

            //XUEHUI 2016-08-15
            try
            {
                DateTime DT1 = DateTime.Parse(dateTime).AddHours(int.Parse(period));
                DateTime DT2 = DateTime.Parse(hour);
                TimeSpan TS = DT2 - DT1;
                hour = TS.TotalHours.ToString().PadLeft(4, '0');//重新计算
                hour2 = TS.TotalHours.ToString().PadLeft(3, '0');//重新计算;
            }
            catch
            {

            }

            if (hour != "-1" && hour != "-2")//-1标示时间往前查询，-2标示时间往后查询
            {
                if (info[9] == "")
                {
                    hours = int.Parse(hour);
                    forecastDate = DateTime.Parse(dateTime).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
                    strWhere = strWhere + "   ForecastDate= '" + forecastDate + "'";
                }
                else
                {
                    hours = int.Parse(period);
                    forecastDate = DateTime.Parse(dateTime).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
                    strWhere = strWhere + "   ForecastDate= '" + forecastDate + "' AND (Period='" + hour + "' or Period='" + hour2 + "') ";
                }
                strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, datepart(hour,ForecastDate) as hour FROM   " + entity.TableName + strWhere + "ORDER BY ForecastDate,Period DESC";

            }
            else
            {

                string str = "";
                if (info[9] == "")
                {
                    if (hour == "-1")
                    {
                        str = "   ForecastDate <'" + forecastDate + "'";
                        strSQL = "SELECT  TOP(1) ForecastDate  FROM   " + entity.TableName + strWhere + str + " ORDER BY ForecastDate DESC";
                    }
                    else
                    {
                        forecastDate = DateTime.Parse(dateTime).AddDays(1).ToString("yyyy-MM-dd HH:00:00");
                        str = "   ForecastDate >='" + forecastDate + "'";
                        strSQL = "SELECT  TOP(1) ForecastDate  FROM   " + entity.TableName + strWhere + str + " ORDER BY ForecastDate ";
                    }
                    forecastDate = m_Database.GetFirstValue(strSQL);
                    if (forecastDate == "")
                        return "";
                    string startTime = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 00:00:00");
                    string endTime = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 23:59:59");
                    strWhere = strWhere + "    ForecastDate BETWEEN'" + startTime + "' AND '" + endTime + "'";
                    strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, datepart(hour,ForecastDate) as hour FROM   " + entity.TableName + strWhere + "ORDER BY ForecastDate,Period DESC";
                }
                else
                {
                    hours = int.Parse(period);
                    if (hour == "-1")
                        forecastDate = DateTime.Parse(dateTime).AddDays(-1).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
                    else
                        forecastDate = DateTime.Parse(dateTime).AddDays(1).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
                    strWhere = strWhere + "    ForecastDate ='" + forecastDate + "' ";
                    strSQL = "SELECT  ('Product/' + folder + '/' + name) AS DM, Period FROM   " + entity.TableName + strWhere + " ORDER BY Period ";
                }
            }

            dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count <= 0)
                return "";
            src = dt.Rows[0][0].ToString();
            //if (hour == "-1" || hour == "-2")//xuehui 2016-08-15
            //{
            //    sb.Append("'addBut':\"");
            //    sb.Append("<select id='selectHour' onchange='selectChange()'>");
            //    foreach (DataRow rows in dt.Rows)
            //    {
            //        string selectValue = "";
            //        if (info[9] == "")
            //            selectValue = DateTime.Parse(forecastDate).Hour.ToString();
            //        else
            //        {
            //            if (hour == "-1")
            //                selectValue = dt.Rows[dt.Rows.Count - 1][1].ToString();
            //            else
            //                selectValue = dt.Rows[0][1].ToString();
            //        }
            //        if (rows[1].ToString() == selectValue)
            //        {
            //            sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", rows[1]);
            //            src = rows[0].ToString();
            //        }
            //        else
            //            sb.AppendFormat("<option value ='{0}' >{0}</option>", rows[1]);
            //    }
            //    sb.Append("</select >");
            //    sb.Append("\",");
            //    sb.AppendFormat("'time':'{0}',", DateTime.Parse(forecastDate).ToString("yyyy-MM-dd"));

            //}

            if (hour == "-1" || hour == "-2")//xuehui 2016-08-15
            {
                return "";

            }
            sb.AppendFormat("'src':'{0}'", src);
            sb.Append("}");
            string jsonStr = sb.ToString();
            return jsonStr;
        }

        public DataTable QueryListAirQuality(string entityName, string entityObj,string station)
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
            if(station!="")
                strWhere = strWhere + " AND  station='" + station+"'";
            string strSQL = "SELECT ('Product/' + folder + '/' + name) AS DM, (CASE WHEN Period IS NULL THEN CONVERT(varchar(16),ForecastDate, 120) ELSE SUBSTRING(CONVERT(varchar(16),ForecastDate, 120), 0, 5)+'年'+SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 6, 2) + '月' + SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 9, 2) + '日' + SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 12, 2) + '时 F' + Period + 'H' END) AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + "ORDER BY NAME";

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
            string strSQL = "SELECT ('Product/' + folder + '/' + name) AS DM, (CASE WHEN Period IS NULL THEN CONVERT(varchar(16),ForecastDate, 120) ELSE SUBSTRING(CONVERT(varchar(16),ForecastDate, 120), 0, 5)+'年'+SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 6, 2) + '月' + SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 9, 2) + '日' + SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 12, 2) + '时 F' + Period + 'H' END) AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + "ORDER BY NAME";
            if (entityName == "HuadongMeto" || entityName == "HuadongForecast" || entityName == "WeekForecast" || entityName == "ChangForecast" || entityName == "ShanghaiAna")
            {
                strSQL = "SELECT ('Product/' + folder + '/' + name) AS DM, (CASE WHEN Period IS NULL THEN CONVERT(varchar(16),ForecastDate, 120) ELSE SUBSTRING(CONVERT(varchar(16),ForecastDate, 120), 0, 5)+'年'+SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 6, 2) + '月' + SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 9, 2) + '日' + SUBSTRING(CONVERT(varchar(16), ForecastDate, 120), 12, 2) + '时 F' + Period + 'H' END) AS MC,TYPE,Period FROM  " + entity.TableName + strWhere + "ORDER BY ForecastDate";
            }
            try
            {
                DataSet dt = m_Database.GetDataset(strSQL);
                if (dt.Tables.Count > 0)
                {
                    DataTable dTable = dt.Tables[0];
                    if (dTable.Rows.Count > 0)
                    {
                        if (dTable.Rows[0]["MC"].ToString().IndexOf('F') > 0)
                        {
                            foreach (DataRow dr in dTable.Rows)
                            {
                                string oldStr = dr[1].ToString().Substring(0, 14);
                                int hourAdd = int.Parse(dr[1].ToString().Substring(16, 3));
                                //string newStr = DateTime.Parse(oldStr).AddHours(hourAdd).ToString("yyyy-MM-dd HH:00");
                                string newStr = null; ;
                                if (entityName == "HuadongMeto" || entityName == "HuadongForecast" || entityName == "WeekForecast" || entityName == "ChangForecast" || entityName == "ShanghaiAna")
                                {
                                    newStr = DateTime.Parse(oldStr).AddHours(hourAdd).ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    newStr = DateTime.Parse(oldStr).AddHours(hourAdd).ToString("yyyy-MM-dd HH:00");
                                }
                                dr[1] = newStr;
                            }
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
            //string strSQL = "SELECT PERIOD FROM T_ImageProduct WHERE ENTITYNAME = '" + entityName + "'";
            string strSQL = "SELECT PERIOD FROM T_ImageProductJK WHERE ENTITYNAME = '" + entityName + "'";
            return m_Database.GetFirstValue(strSQL);
        }
        /// <summary>
        /// 将对象转化为JSON数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ToJSON(object obj)
        {
            string josnData = string.Empty; 
            if (obj == null)
            {
                return "";
            }
            //如果为DataTable对象
            if (obj.GetType() == typeof(System.Data.DataTable))
            {
                DataTable dTable = (DataTable)obj;
                josnData = JsonConvert.SerializeObject(obj, new DataTableConverter());
                //JsonConvert.DeserializeObject(josnData,new DataTableConverter());
                //需要返回总的记录数的话
                if (dTable.TableName.Contains("data:{0}"))
                {
                    josnData = "{" + string.Format(dTable.TableName, josnData) + "}";
                }
                return josnData;
            }
            else if (obj.GetType() == typeof(string))
            {
                return obj.ToString();
            }
            else if (obj.GetType() == typeof(DataSet))
            {
                josnData = JsonConvert.SerializeObject(obj, new DataTableConverter());
                return josnData;
            }
            return JsonConvert.SerializeObject(obj);
        }


        //2015年11月9日，闫海涛

        /// <summary>
        /// 查询用于填充AQI分时段预报的数据表
        /// </summary>
        /// <param name="tableName">查询表名称</param>
        /// <param name="curDate">当前日期</param>
        /// <param name="siteID">所属站点（徐家汇（58367）表示上海）</param>
        /// <returns></returns>
        public string GetAQIForecast(string tableName,string curDate,string siteID)
        {
           //string strSQL= "select * from dbo.T_ForecastSite where  ForecastDate=(select max(ForecastDate) from dbo.T_ForecastSite)"
            string strSQL = "select * from T_ForecastSite where  ForecastDate=(select max(ForecastDate) from T_ForecastSite)";
            DataSet dt = m_Database.GetDataset(strSQL);
                if (dt.Tables.Count > 0)
                {
                    DataTable dTable = dt.Tables[0];
                    
                }
            return "query Test";
        }

        public string IndexData(string entityList)
        {
            string strImgJson = "";
            if (entityList != "")
            {
                string[] entitys = entityList.Split('&');
                if (entitys.Length > 0)
                {
                    for (int i = 0; i < entitys.Length; i++)
                    {
                        string strJson= JsonConvert.SerializeObject(GetEntity(entitys[i]));
                        DataTable dt = QueryList(entitys[i], strJson);
                        strImgJson += "\""+entitys[i]+"\":\"";
                        if (dt.Rows.Count > 0)
                        {
                            strImgJson += dt.Rows[0]["DM"] + "\",";
                        }
                        else
                        {
                            strImgJson += "\",";
                        }
                    }
                }
            }
            return "{" + strImgJson.Trim(',') + "}";
        }
    }

}
