using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;

namespace MMShareBLL.DAL
{
     public class FreeQuery
    {
         Database m_Database;
         public FreeQuery()
         {
             m_Database = new Database();
         }
         public FreeQuery(Database db)
        {
            m_Database = db;
        }
         public string CreateLayers(string stationId, string layer)
         {
             if (layer != "")
             {
                 string strSQL = "SELECT distinct Type,D_Monitor_Type.MC FROM T_MonitorAnalysis  LEFT JOIN D_Monitor_Type ON  T_MonitorAnalysis.Type=D_Monitor_Type.DM  WHERE T_MonitorAnalysis.Station='" + stationId + "' AND T_MonitorAnalysis.Layers='" + layer + "'  ORDER BY T_MonitorAnalysis.Type";
                 DataTable dt = m_Database.GetDataTable(strSQL);
                 string types = dt.Rows[0][0].ToString();
                 StringBuilder sb = new StringBuilder();
                 StringBuilder sm = new StringBuilder();
                 sb.Append("<label class='cur-select' id='typeSelect'>" + dt.Rows[0][1] + "</label>");
                 sb.Append("<select id='typeID'>");
                 int k = 0;
                 foreach (DataRow rows in dt.Rows)
                 {
                     if (k == 0)
                     {
                         sb.AppendFormat("<option value ='{0}' selected='true'>{1}</option>", rows[0], rows[1]);
                         k = 1;
                     }
                     else
                         sb.AppendFormat("<option value ='{0}' >{1}</option>", rows[0], rows[1]);
                 }
                 sb.Append("</select>");
                 sm.AppendFormat("'types':\"{0}\",", sb.ToString());
                 strSQL = "SELECT TOP(1)ForecastDate FROM  T_MonitorAnalysis WHERE  Station='" + stationId + "' AND Layers='" + layer + "' AND Type='" + types + "' ORDER BY ForecastDate DESC";
                 string time = m_Database.GetFirstValue(strSQL);
                 sm.Append(CreateTime(time, stationId, layer, types));
                 return sm.ToString();

             }
             else
                 return "";
         }
         public string typeChange(string stationId, string layer, string type)
         {
             if (layer != "" && type != "")
             {
                 StringBuilder sm = new StringBuilder("{");
                 sm.Append(CreateType(stationId, layer, type));
                 sm.Append("}");
                 return sm.ToString();
             }
             else
                 return "";
             
         }
         public string LayersChange(string stationId, string layer)
         {
             StringBuilder sm = new StringBuilder("{");
             sm.Append(CreateLayers(stationId, layer));
             sm.Append("}");
             return sm.ToString();

         }
         public string LayersChangeTime(string stationId, string layer, string city)
         {
             StringBuilder sb = new StringBuilder();
             StringBuilder sm = new StringBuilder("{");
             string tableName = "";
             string strSQL = "";
             if (stationId.IndexOf("D_") >= 0)
             {
                 string[] name = stationId.Split('_');
                 tableName = "T_" + name[1];
                 if(city!="")
                     strSQL = "SELECT TOP(1)ForecastDate FROM  " + tableName + " WHERE  Layers='" + layer + "' AND Type='" + city + "' ORDER BY ForecastDate DESC";
                 else 
                     strSQL = "SELECT TOP(1)ForecastDate FROM  " + tableName + " WHERE  Type='" + layer + "' ORDER BY ForecastDate DESC";
             }
             else
             {
                 tableName = "T_ModelTest";
                 strSQL = "SELECT TOP(1)ForecastDate FROM  " + tableName + " WHERE  Layers='" + layer + "' AND Type='" + stationId + "' ORDER BY ForecastDate DESC";
             }
             string time = m_Database.GetFirstValue(strSQL);
             sm.AppendFormat("'time':\"{0}\",", DateTime.Parse(time).ToString("yyyy-MM-dd"));
             if (stationId.IndexOf("D_") >= 0)
             {
                 if (city != "")
                     strSQL = "SELECT distinct datename(hour,ForecastDate) as hour FROM  " + tableName + " WHERE  Layers='" + layer + "' AND Type='" + city + "'  ORDER BY hour ";
                 else
                     strSQL = "SELECT distinct datename(hour,ForecastDate) as hour FROM  " + tableName + " WHERE  Type='" + layer + "'  ORDER BY hour ";
             }
             else
                 strSQL = "SELECT distinct datename(hour,ForecastDate) as hour FROM  " + tableName + " WHERE  Layers='" + layer + "' AND Type='" + stationId + "'AND ForecastDate BETWEEN '" + DateTime.Parse(time).ToString("yyyy-MM-dd 00:00:00") + "' AND  '" + DateTime.Parse(time).ToString("yyyy-MM-dd 23:59:59") + "' ORDER BY hour DESC";
             DataTable dt = m_Database.GetDataTable(strSQL);
             string period = "";
             foreach (DataRow rows in dt.Rows)
             {
                 period = period + rows[0] + ",";
             }
             sm.AppendFormat("period:\"{0}\"", period.Substring(0, period.Length - 1));
             sm.Append("}");
             return sm.ToString();

         }
         public string trickLayers(string stationId)
         {
             string strSQL = "SELECT distinct Layers,D_Monitor_Layers.MC FROM T_MonitorAnalysis  LEFT JOIN D_Monitor_Layers ON  T_MonitorAnalysis.Layers=D_Monitor_Layers.DM  WHERE T_MonitorAnalysis.Station='" + stationId + "' AND T_MonitorAnalysis.Layers is not null ORDER BY T_MonitorAnalysis.Layers";
             DataTable dt = m_Database.GetDataTable(strSQL);
             StringBuilder sb = new StringBuilder();
             StringBuilder sm= new StringBuilder("{");
             string lays = dt.Rows[0][0].ToString();
             sb.Append("<label class='cur-select' id='layersSelect'>" + dt.Rows[0][1] + "</label>");
             sb.Append("<select id='layersID'>");
             int k = 0;
             foreach (DataRow rows in dt.Rows)
             {
                 if (rows[1].ToString() != "")
                 {
                     if (k == 0)
                     {
                         sb.AppendFormat("<option value ='{0}' selected='true'>{1}</option>", rows[0], rows[1]);
                         k = 1;
                     }
                     else
                         sb.AppendFormat("<option value ='{0}' >{1}</option>", rows[0], rows[1]);
                 }
             }
             sb.Append("</select>");
             sm.AppendFormat("'layers':\"{0}\",",sb.ToString());


             strSQL = "SELECT distinct Type,D_Monitor_Type.MC FROM T_MonitorAnalysis  LEFT JOIN D_Monitor_Type ON  T_MonitorAnalysis.Type=D_Monitor_Type.DM  WHERE T_MonitorAnalysis.Station='" + stationId + "' AND T_MonitorAnalysis.Layers='" + lays + "'  ORDER BY T_MonitorAnalysis.Type";
             dt = m_Database.GetDataTable(strSQL);
             string types = dt.Rows[0][0].ToString();
             sb = new StringBuilder();
             sb.Append("<label class='cur-select' id='typeSelect'>" + dt.Rows[0][1] + "</label>");
             sb.Append("<select id='typeID'>");
             k = 0;
             foreach (DataRow rows in dt.Rows)
             {
                 if (k == 0)
                 {
                     sb.AppendFormat("<option value ='{0}' selected='true'>{1}</option>", rows[0], rows[1]);
                     k = 1;
                 }
                 else
                     sb.AppendFormat("<option value ='{0}' >{1}</option>", rows[0], rows[1]);
             }
             sb.Append("</select>");
             sm.AppendFormat("'types':\"{0}\",", sb.ToString());
             strSQL = "SELECT TOP(1)ForecastDate FROM  T_MonitorAnalysis WHERE  Station='" + stationId + "' AND Layers='" + lays + "' AND Type='" + types + "' ORDER BY ForecastDate DESC";
             string time = m_Database.GetFirstValue(strSQL);
             sm.AppendFormat("'time':\"{0}\",", DateTime.Parse(time).ToString("yyyy-MM-dd"));
             sm.Append(CreateTime(time, stationId, lays, types));
             
             sm.Append("}");
             return sm.ToString();
         }
         public string trickModuleLayers(string stationId)
         {
             string strSQL = "";
             string tableName = "";
             if (stationId.IndexOf("D_") >= 0)
             {
                 strSQL = "SELECT distinct DM,MC FROM " + stationId;
                 string[] name=stationId.Split('_');
                 tableName = "T_" + name[1];
             }
             else
             {
                 strSQL = "SELECT distinct DM,MC FROM D_ModuleF_Layers";
                 tableName = "T_ModelTest";
             }
             DataTable dt = m_Database.GetDataTable(strSQL);
             StringBuilder sb = new StringBuilder();
             StringBuilder sm = new StringBuilder("{");
             string lays = dt.Rows[0][0].ToString();
             sb.Append("<label class='cur-select' id='layersSelect'>" + dt.Rows[0][1] + "</label>");
             sb.Append("<select id='layersID'>");
             int k = 0;
             foreach (DataRow rows in dt.Rows)
             {
                 if (rows[1].ToString() != "")
                 {
                     if (k == 0)
                     {
                         sb.AppendFormat("<option value ='{0}' selected='true'>{1}</option>", rows[0], rows[1]);
                         k = 1;
                     }
                     else
                         sb.AppendFormat("<option value ='{0}' >{1}</option>", rows[0], rows[1]);
                 }
             }
             sb.Append("</select>");
             sm.AppendFormat("'layers':\"{0}\",", sb.ToString());
             if (stationId.IndexOf("D_") >= 0)
                 strSQL = "SELECT TOP(1)ForecastDate FROM  " + tableName + " WHERE  Type='" + lays + "' ORDER BY ForecastDate DESC";
             else 
                 strSQL = "SELECT TOP(1)ForecastDate FROM  " + tableName + " WHERE  Layers='" + lays + "' AND Type='" + stationId + "' ORDER BY ForecastDate DESC";
             string time = m_Database.GetFirstValue(strSQL);
             sm.AppendFormat("'time':\"{0}\",", DateTime.Parse(time).ToString("yyyy-MM-dd"));
             if (stationId.IndexOf("D_") >= 0)
                 strSQL = "SELECT distinct datename(hour,ForecastDate) as hour FROM  " + tableName + " WHERE  Type='" + lays + "'  ORDER BY hour";
             else
                 strSQL = "SELECT distinct datename(hour,ForecastDate) as hour FROM  " + tableName + " WHERE  Layers='" + lays + "' AND Type='" + stationId + "'AND ForecastDate BETWEEN '" + DateTime.Parse(time).ToString("yyyy-MM-dd 00:00:00") + "' AND  '" + DateTime.Parse(time).ToString("yyyy-MM-dd 23:59:59") + "' ORDER BY hour DESC";
             dt = m_Database.GetDataTable(strSQL);
             string period = "";
             foreach (DataRow rows in dt.Rows)
             {
                 period = period + rows[0] + ",";
             }
             sm.AppendFormat("period:\"{0}\"", period.Substring(0,period.Length-1));
             sm.Append("}");
             return sm.ToString();
         }
         public string cityChange(string layerTableName, string city)
         {
             string[] name = layerTableName.Split('_');
             string tableName = "T_" + name[1];
             StringBuilder sb = new StringBuilder();
             StringBuilder sm = new StringBuilder("{");
            
             sb = new StringBuilder();
             string strSQL = "SELECT distinct DM,MC FROM " + layerTableName;
             DataTable dt = m_Database.GetDataTable(strSQL);
             string layer = dt.Rows[0][0].ToString();
             sb.Append("<label class='cur-select' id='layersSelect'>" + dt.Rows[0][1] + "</label>");
             sb.Append("<select id='layersID'>");
             int  k = 0;
             foreach (DataRow rows in dt.Rows)
             {
                 if (rows[1].ToString() != "")
                 {
                     if (k == 0)
                     {
                         sb.AppendFormat("<option value ='{0}' selected='true'>{1}</option>", rows[0], rows[1]);
                         k = 1;
                     }
                     else
                         sb.AppendFormat("<option value ='{0}' >{1}</option>", rows[0], rows[1]);
                 }
             }
             sb.Append("</select>");
             sm.AppendFormat("'layers':\"{0}\",", sb.ToString());
             strSQL = "SELECT TOP(1)ForecastDate FROM  " + tableName + " WHERE  Layers='" + layer + "' AND Type='" + city + "' ORDER BY ForecastDate DESC";

             string time = m_Database.GetFirstValue(strSQL);
             sm.AppendFormat("'time':\"{0}\",", DateTime.Parse(time).ToString("yyyy-MM-dd"));
             strSQL = "SELECT distinct datename(hour,ForecastDate) as hour FROM  " + tableName + " WHERE  Layers='" + layer + "' AND Type='" + city + "'  ORDER BY hour";

             dt = m_Database.GetDataTable(strSQL);
             string period = "";
             foreach (DataRow rows in dt.Rows)
             {
                 period = period + rows[0] + ",";
             }
             sm.AppendFormat("period:\"{0}\"", period.Substring(0, period.Length - 1));
             sm.Append("}");
             return sm.ToString();
         }
         public string trickDiagnosticLayers(string layerTableName)
         {
             string strSQL = "SELECT distinct DM,MC FROM D_ECCity_Type ";
             string[] name = layerTableName.Split('_');
             string tableName = "T_" + name[1];
             DataTable dt = m_Database.GetDataTable(strSQL);
             StringBuilder sb = new StringBuilder();
             StringBuilder sm = new StringBuilder("{");
             string city = dt.Rows[0][0].ToString();
             sb.Append("<label class='cur-select' id='citySelect'>" + dt.Rows[0][1] + "</label>");
             sb.Append("<select id='cityID'>");
             int k = 0;
             foreach (DataRow rows in dt.Rows)
             {
                 if (rows[1].ToString() != "")
                 {
                     if (k == 0)
                     {
                         sb.AppendFormat("<option value ='{0}' selected='true'>{1}</option>", rows[0], rows[1]);
                         k = 1;
                     }
                     else
                         sb.AppendFormat("<option value ='{0}' >{1}</option>", rows[0], rows[1]);
                 }
             }
             sb.Append("</select>");
             sm.AppendFormat("'city':\"{0}\",", sb.ToString());
             sb = new StringBuilder();
             strSQL = "SELECT distinct DM,MC FROM " + layerTableName;
             dt = m_Database.GetDataTable(strSQL);
             string layer = dt.Rows[0][0].ToString();
             sb.Append("<label class='cur-select' id='layersSelect'>" + dt.Rows[0][1] + "</label>");
             sb.Append("<select id='layersID'>");
             k = 0;
             foreach (DataRow rows in dt.Rows)
             {
                 if (rows[1].ToString() != "")
                 {
                     if (k == 0)
                     {
                         sb.AppendFormat("<option value ='{0}' selected='true'>{1}</option>", rows[0], rows[1]);
                         k = 1;
                     }
                     else
                         sb.AppendFormat("<option value ='{0}' >{1}</option>", rows[0], rows[1]);
                 }
             }
             sb.Append("</select>");
             sm.AppendFormat("'layers':\"{0}\",", sb.ToString());
             strSQL = "SELECT TOP(1)ForecastDate FROM  " + tableName + " WHERE  Layers='" + layer + "' AND Type='" + city + "' ORDER BY ForecastDate DESC";
           
             string time = m_Database.GetFirstValue(strSQL);
             sm.AppendFormat("'time':\"{0}\",", DateTime.Parse(time).ToString("yyyy-MM-dd"));
             strSQL = "SELECT distinct datename(hour,ForecastDate) as hour FROM  " + tableName + " WHERE  Layers='" + layer + "' AND Type='" + city + "'  ORDER BY hour";
             
             dt = m_Database.GetDataTable(strSQL);
             string period = "";
             foreach (DataRow rows in dt.Rows)
             {
                 period = period + rows[0] + ",";
             }
             sm.AppendFormat("period:\"{0}\"", period.Substring(0, period.Length - 1));
             sm.Append("}");
             return sm.ToString();
         }
         public DataTable QueryModuleFree(string lastTime, string layer, string type)
         {
             try
             {
                 string strSQL="";
                 string tableName = "";
                 if (type.IndexOf("D_") >= 0)
                 {
                     string[] name = type.Split('_');
                     tableName = "T_" + name[1];
                     strSQL = "SELECT distinct CONVERT(varchar(16),DATEADD(hour, CONVERT(int, Period), ForecastDate), 120) AS MC FROM "+tableName+" Where  Type='" + layer + "' AND ForecastDate='" + DateTime.Parse(lastTime).ToString("yyyy-MM-dd HH:00:00") + "' ORDER BY MC ";
                 }
                 else
                 {
                     tableName = "T_ModelTest";
                     strSQL = "SELECT distinct CONVERT(varchar(16),DATEADD(hour, CONVERT(int, Period), ForecastDate), 120) AS MC FROM  T_ModelTest Where Layers='" + layer + "' AND Type='" + type + "' AND ForecastDate='" + DateTime.Parse(lastTime).ToString("yyyy-MM-dd HH:00:00") + "' ORDER BY MC ";
                 }
                 DataTable dt = m_Database.GetDataTable(strSQL);
                 return dt;
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         public string CreateType(string stationId, string layer, string type)
         {
             StringBuilder sm = new StringBuilder();
             string strSQL = "SELECT TOP(1)ForecastDate FROM  T_MonitorAnalysis WHERE  Station='" + stationId + "' AND Layers='" + layer + "' AND Type='" + type + "' ORDER BY ForecastDate DESC";
             string time = m_Database.GetFirstValue(strSQL);
             sm.AppendFormat("'time':\"{0}\",", DateTime.Parse(time).ToString("yyyy-MM-dd"));
             sm.Append(CreateTime(time, stationId, layer, type));
             return sm.ToString();
         }
         public string CreateTime(string forecastDate, string stationId, string layers, string types)
         {
             string startTime = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 00:00:00");
             string endTime = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd 23:59:59");

             string strSQL = "SELECT datepart(hour,ForecastDate) as hour FROM  T_MonitorAnalysis WHERE  Station='" + stationId + "' AND Layers='" + layers + "' AND Type='" + types + "' AND ForecastDate BETWEEN '" + startTime + "' AND '" + endTime + "'";
             DataTable dt = m_Database.GetDataTable(strSQL);
             StringBuilder sb = new StringBuilder();
             StringBuilder sm = new StringBuilder();
             sb.Append("<label class='hour-select' id='hourSelect'>" + dt.Rows[0][0].ToString() + "</label>");
             sb.Append("<select id='hourID'>");
             int k = 0;
             foreach (DataRow rows in dt.Rows)
             {
                 if (k == 0)
                 {
                     sb.AppendFormat("<option value ='{0}' selected='true'>{0}</option>", rows[0]);
                     k = 1;
                 }
                 else
                     sb.AppendFormat("<option value ='{0}' >{0}</option>", rows[0]);
             }
             sb.Append("</select>");
             sm.AppendFormat("'hour':\"{0}\"", sb.ToString());
             return sm.ToString();
         }
         public string QueryImgList(string forecastDate,string stationId, string layers, string types)
         {
             if (forecastDate != "")
             {
                 StringBuilder sm = new StringBuilder("{");
                 sm.Append(CreateTime(forecastDate, stationId, layers, types));
                 sm.Append("}");
                 return sm.ToString();
             }
             else
                 return "";
         }
         public string QueryListButton(string forecastDate, string stationId, string layers, string types, string hour)
         {
             int hours=int.Parse(hour);
             string time = DateTime.Parse(forecastDate).AddHours(hours).ToString("yyyy-MM-dd HH:00:00");
             string strSQL = "SELECT ('Product/' + folder + '/' + name) AS DM  FROM  T_MonitorAnalysis WHERE  Station='" + stationId + "' AND Layers='" + layers + "' AND Type='" + types + "' AND ForecastDate ='" + time + "'";
             string url = m_Database.GetFirstValue(strSQL);
             return url;
         }
         public string QueryMoudleListButton(string forecastDate, string types, string layers, string time,string city)
         {
             TimeSpan timeSpan = DateTime.Parse(time) - DateTime.Parse(forecastDate);
             double hour = timeSpan.TotalHours;
             string period = hour.ToString("000");
             forecastDate = DateTime.Parse(forecastDate).ToString("yyyy-MM-dd HH:00:00");
             string strSQL = "";
             string tableName = "";
             if (types.IndexOf("D_") >= 0)
             {
                 string[] name = types.Split('_');
                 tableName = "T_" + name[1];
                 if(city!="")
                     strSQL = "SELECT ('Product/' + folder + '/' + name) AS DM  FROM  " + tableName + " WHERE  Layers='" + layers + "' AND Type='" + city + "' AND ForecastDate ='" + forecastDate + "' AND Period='" + period + "'";
                 else 
                     strSQL = "SELECT ('Product/' + folder + '/' + name) AS DM  FROM  " + tableName + " WHERE  Type='" + layers + "' AND ForecastDate ='" + forecastDate + "' AND Period='" + period + "'";
             }
             else
             {
                 tableName = "T_ModelTest";
                 strSQL = "SELECT ('Product/' + folder + '/' + name) AS DM  FROM  T_ModelTest WHERE  Type='" + types + "' AND Layers='" + layers + "'  AND ForecastDate ='" + forecastDate + "' AND Period='" + period + "'";
             }
             string url = m_Database.GetFirstValue(strSQL);
             return url;
         }
    }
}
