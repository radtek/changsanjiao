using System;
using System.Collections.Generic;
using System.Text;
using AQIQuery.aQuery;
using System.Data;
using Newtonsoft.Json;
using Readearth.Data;

namespace AQIQueryAPI
{
   public class AQIQueryAPI
    {

      private Database m_DatabaseS;
      public AQIQueryAPI()
      {
          m_DatabaseS = new Database("DBCONFIG");
      }


       /// <summary>
       /// 查询日平均AQI的数据
       /// </summary>
       /// <param name="LST1"></param>
       /// <param name="LST2"></param>
       /// <param name="GroupIDs"></param>
       /// <param name="AQIItemIDs"></param>
       /// <returns></returns>
       public string GroupDailyAQI(string LST1, string LST2, string GroupIDs, string AQIItemIDs) {
           string jsonString = "";
           try
           {
               DateTime lst1=DateTime.Parse(LST1).AddHours(+1);
               DateTime lst2=DateTime.Parse(LST2);
               DataTable dt = Data.GroupHourlyAQI(lst1, lst2, GroupIDs, AQIItemIDs);
               jsonString = DataTableToJson("AQIDATA", dt);
               //jsonString = lst1.ToString()+" "+lst2.ToString();
           }
           catch { }
           return jsonString;
       }

       /// <summary>
       /// DataTable to json
       /// </summary>
       /// <param name="jsonName">返回json的名称</param>
       /// <param name="dt">转换成json的表</param>
       /// <returns></returns>
       public static string DataTableToJson(string jsonName, System.Data.DataTable dt)
       {
           StringBuilder Json = new StringBuilder();
           Json.Append("{\"" + jsonName + "\":[");
           if (dt.Rows.Count > 0)
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   Json.Append("{");
                   for (int j = 0; j < dt.Columns.Count; j++)
                   {
                       Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                       if (j < dt.Columns.Count - 1)
                       {
                           Json.Append(",");
                       }
                   }
                   Json.Append("}");
                   if (i < dt.Rows.Count - 1)
                   {
                       Json.Append(",");
                   }
               }
           }
           Json.Append("]}");
           return Json.ToString();
       }


       /// <summary>
       /// 查询重金属数据
       /// </summary>
       /// <param name="LST1"></param>
       /// <param name="LST2"></param>
       /// <param name="GroupIDs">Sites</param>
       /// <param name="AQIItemIDs"></param>
       /// <returns></returns>
       public string XRFData(string LST1, string LST2, string SiteIDs)
       {
           string jsonString = "";
           try
           {
               DateTime lst1 = DateTime.Parse(LST1);
               DateTime lst2 = DateTime.Parse(LST2);
               string sqlstr = " select * from dbo.T_HeavyMetal where SiteID='{0}' and "+
                               " (createTime between '{1}' and '{2}')";
               DataTable dt = m_DatabaseS.GetDataTable(string.Format(sqlstr, SiteIDs,lst1, lst2 ));
               jsonString = DataTableToJson("XRFDATA", dt);
           }
           catch(Exception ex) {
               jsonString = ex.Message;
           }
           return jsonString;
       }



    }
}
