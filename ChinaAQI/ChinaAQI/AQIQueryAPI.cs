using System;
using System.Collections.Generic;
using System.Text;
using AQIQuery.aQuery;
using System.Data;
using Newtonsoft.Json;

namespace ChinaAQI
{
   public class AQIQueryAPI
    {
       /// <summary>
       /// 查询日平均AQI的数据
       /// </summary>
       /// <param name="LST1"></param>
       /// <param name="LST2"></param>
       /// <param name="GroupIDs"></param>
       /// <param name="AQIItemIDs"></param>
       /// <returns></returns>
       public string GroupDailyAQI(DateTime LST1, DateTime LST2, string GroupIDs, string AQIItemIDs) {
           DataTable dt=Data.GroupDailyAQI(LST1, LST2, GroupIDs, AQIItemIDs); 
           string jsonString = "";
           foreach (DataRow row in dt.Rows) {
               jsonString += JsonConvert.SerializeObject(row) + ",";
           }
           if (jsonString != ""){
               jsonString = "[" + jsonString.Substring(0, jsonString.Length - 1) + "]";
           }
           return jsonString;
       }

    }
}
