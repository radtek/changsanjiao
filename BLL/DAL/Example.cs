using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MMShareBLL.DAL
{
    public class Example
    {

        /// <summary>
        /// DataTable to json
        /// </summary>
        /// <param name="jsonName">返回json的名称</param>
        /// <param name="dt">转换成json的表</param>
        /// <returns></returns>
        public string DataTableToJson(string jsonName, System.Data.DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("[");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("]");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        public string QueryManager(string sDdate, string eDdate, string type, string strWhere)
        {
            Database thDB = new Database();
            string part = " WHERE [DATE] BETWEEN '" + sDdate + " 00:00:00' AND '" + eDdate + " 23:00:00'",
                strSQl = "SELECT CONVERT(VARCHAR(10),[Date],121),PM2_5,[Level],WeatherType,Remark,WindDirect,WindSpeed,Visibility,Weather,Cloudage FROM V_ExampleManager";
            if (type == "day")
            {
                string sql = "SELECT CONVERT(VARCHAR(10),SDATE,121),CONVERT(VARCHAR(10),SDATE+CDAYS-1,121) FROM(SELECT MIN([Date]) AS SDATE,COUNT(1) AS CDAYS FROM(SELECT a.[Date],DATEPART(d,a.[Date])-(SELECT count(1) FROM V_ExampleManager b WHERE  b.[Date]<=a.[Date]) AS DEPART FROM V_ExampleManager a "
                + part + ") t GROUP BY DEPART) T2 WHERE T2.CDAYS=" + strWhere + "  ORDER BY SDATE";
                DataTable dt = thDB.GetDataTable(sql);
                if (dt.Rows.Count == 0) return DataTableToJson("data", dt);
                part = " WHERE(";
                foreach (DataRow dr in dt.Rows)
                {
                    part += "[DATE] BETWEEN '" + dr[0].ToString() + "' AND '" + dr[1].ToString() + "' OR ";
                }
                strSQl += part.Substring(0, part.Length - 3) + ") ORDER BY [Date]";
            }
            else
            {
                string[] where = strWhere.Split('*');
                if (where[0] != "") part += " AND [LEVEL]='" + where[0] + "'";
                if (where[1] != "") part += " AND MONTH([DATE]) IN(" + where[1] + ")";
                if (where[2] != "") part += " AND [TYPE]='" + where[2] + "'";
                strSQl += part + " ORDER BY [Date]";
            }
            DataTable theDt = thDB.GetDataTable(strSQl);
            return DataTableToJson("data", theDt);
        }

        public string QueryDeparture2(string time)
        {
            time = time.Replace("-", "");
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://10.228.177.217/wcf/SimilarService.svc/GetSimilarListByTime?time=" + time);//创建请求
            request.Method = "GET";//设置访问方式
            HttpWebResponse result = request.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(result.GetResponseStream(), Encoding.Default);
            string strResult = sr.ReadToEnd();
            sr.Close();
            Regex theRegex = new Regex("},{");
            string[] sult = theRegex.Split(strResult.Replace("[{", "").Replace("}]", ""));
            string strReturn = "";
            if (strResult != "[]")
            {
                float a, b, c;
                for (int i = 0; i < sult.Length; i++)
                {
                    his theHis = JsonConvert.DeserializeObject<his>("{" + sult[i] + "}");
                    strReturn += "[\"" + (i + 1).ToString() + "\",\"" + theHis.ftime + "\"";

                    if (float.TryParse(theHis.high500Index, out a) && float.TryParse(theHis.high500Index, out b) && float.TryParse(theHis.high500Index, out c))
                    {
                        if (a >= 0 && b >= 0 && c >= 0)
                        {
                            strReturn += ",\"" + ((a + b + c) / 3).ToString() + "\",\"\",\"\",\"\"],";
                            continue;
                        }
                    }
                    strReturn += ",\"\",\"\",\"\",\"\"],";
                }
            }
            return "{\"data\":[" + strReturn.TrimEnd(',') + "]}";
        }

        public string QueryDeparture(string type, string time, string element)
        {
            time = time.Replace("-", "");
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://10.228.177.217/wcf/SimilarService.svc/" + type + "?time=" + time + (element == "" ? "" : "&element=" + element));//创建请求
            request.Method = "GET";//设置访问方式
            HttpWebResponse result = request.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(result.GetResponseStream(), Encoding.Default);
            string strResult = sr.ReadToEnd();
            sr.Close();
            Regex theRegex = new Regex("},{");
            string[] sult = theRegex.Split(strResult.Replace("[", "").Replace("]", "").TrimStart('{').TrimEnd('}'));
            string strReturn = "", strSql = "", strPartSql = ""; Database db = new Database();
            //List<string> returnInfo = new List<string>();
            Dictionary<string, string> returnInfo = new Dictionary<string, string>();
            if (strResult != "[]")
            {
                float a, b, c;
                for (int i = 0; i < sult.Length; i++)
                {
                    his theHis = JsonConvert.DeserializeObject<his>("{" + sult[i] + "}");

                    if (float.TryParse(theHis.high500Index, out a) && float.TryParse(theHis.high500Index, out b) && float.TryParse(theHis.high500Index, out c))
                    {
                        if (a >= 0 && b >= 0 && c >= 0)
                        {
                            strPartSql += " CONVERT(VARCHAR(10),TIME_POINT,121) = '" + theHis.ftime + "' OR";
                            returnInfo.Add(theHis.ftime, "[\"" + (i + 1).ToString() + "\",\"" + theHis.ftime + "\",\"" + ((a + b + c) / 3).ToString() + "\"");
                            continue;
                        }
                    }
                    //returnInfo.Add(theHis.ftime,"[\"" + (i + 1).ToString() + "\",\"" + theHis.ftime + "\",\"\"");
                }
                if (strPartSql != "")
                {
                    strSql = @"SELECT CONVERT(VARCHAR(10),TIME_POINT,121),primary_pollutant+','+CONVERT(VARCHAR(6),AQI)
                    FROM(SELECT *,ROW_NUMBER() OVER ( PARTITION BY CONVERT(VARCHAR(10),TIME_POINT,121)ORDER BY AQI DESC ) rid FROM T_CityAQI
                    WHERE AREA='上海市' AND (" + strPartSql.Substring(0, +strPartSql.Length - 2) + ")) AS t WHERE rid = 1";
                    DataTable dt = db.GetDataTable(strSql);
                    foreach (DataRow dr in dt.Rows)
                    {
                        returnInfo[dr[0].ToString()] += (dr[1].ToString() == "" ? ",\"\",\"\"" : ",\"" + dr[1].ToString().Replace(",", "\",\"") + "\"");
                    }
                    strSql = "SELECT CONVERT(VARCHAR(10),TIME_POINT,121), CONVERT(VARCHAR(6),MAX(PVALUE)) FROM T_CityAQI WHERE("
                        + strPartSql.Substring(0, +strPartSql.Length - 2) + ") and AREA='上海市' GROUP BY CONVERT(VARCHAR(10),TIME_POINT,121)";
                    dt = db.GetDataTable(strSql);
                    foreach (DataRow dr in dt.Rows)
                    {
                        returnInfo[dr[0].ToString()] += (dr[1].ToString() == "" ? ",\"\"" : ",\"" + dr[1].ToString() + "\"");
                    }
                }
                foreach (KeyValuePair<String, String> kv in returnInfo)
                {
                    if (kv.Value.Split(',').Length < 5) strReturn += kv.Value + ",\"\",\"\",\"\"],";
                    else strReturn += kv.Value + "],";
                }
            }

            return "{\"data\":[" + strReturn.TrimEnd(',') + "]}";
        }
    }
    class his
    {
        public string ftime;
        public string high500Index;
        public string pressure999Index;
        public string temper850Index;

    }
}
