using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Text.RegularExpressions;
using ChinaAQI;
using System.Web.Script.Serialization;
using System.Collections;
using MMShareBLL.DAL;

public partial class ReportProduce_AQIPeriod72 : System.Web.UI.Page
{
    public static Database m_Database;
    public string period;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            period = "06";
            m_Database = new Database("DBCONFIGII");
        }
    }

    /// <summary>
    /// 点击主观或第一次进入页面的时候获取数据
    /// </summary>
    /// <param name="period">当前页面显示的时效</param>
    /// <param name="type">类型，分两种，1：init表示是第一次进入页面，数据库里没有数据的话不需要读接口数据；
    /// 2：sub表示点击的是主观按钮，此时需要先读数据库数据，如果没有则要读接口数据</param>
    /// <returns></returns>
    [WebMethod]
    public static string GetSubData(string period, string type)
    {
        DateTime dNow = DateTime.Now;
        string hour = " 06:00:00";
        if (period == "17")
        {
            hour = " 00:00:00";
        }
        string forecastDate = dNow.ToString("yyyy-MM-dd") + hour;
        string sql = "select lst,forecastdate,period,durationID,itemid,value,aqi,module,parameter,lst as changedLST from T_ForecastGroupCopy " +
                    "where ForecastDate ='" + forecastDate + "'";
        DataTable dt = m_Database.GetDataTable(sql);
        string txt = ProDataTable(dt);
        if (txt.Length == 0 && type == "sub")
        {    //数据库没有保存数据，需要读取接口数据
            txt = GetInterface(dNow.ToString("yyyy-MM-dd 18:00:00"), "sub");
        }
        return txt;
    }
    //获取接口历史数据，如果昨天的没有获取到就自动获取前天的
    [WebMethod]
    public static string GetHistoryData()
    {
        DateTime dNow = DateTime.Now;
        string date = dNow.AddDays(-1).ToString("yyyy-MM-dd 18:00:00");
        string txt = GetInterface(date, "");
        return txt;
    }

    public static string GetInterface(string date, string isSub)
    {
        bool flag = true;   //读取接口数据如果有的话改为false，否则日期循环减一
        int num = 0;
        string txt = "";
        while (flag)
        {
            string time = DateTime.Parse(date).AddDays(num).ToString("yyyy-MM-dd 18:00:00");
            string url = GetDataUrl(time);
            string content = HttpGetContent(url, "");
            DataTable dt = JsonToDataTable(content);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt = ProInterfaceTab(dt);
                txt = ProDataTable(dt);
            }
            num--;
            if (txt != "")
            {
                flag = false;
            }
            if (num <= -3)
            {
                flag = false;
            }
            if (isSub == "sub" && num<-1)  //早上可能没有数据要获取昨天的数据
            {      //主观数据只读取一次
                flag = false;
            }
        }
        return txt;
    }

    /// <summary>
    /// 处理根据条件获取到的几种模式数据的DataTable，最终返回的是一个json字符串
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ProDataTable(DataTable dt)
    {
        string[] period = { "24", "48", "72" };   //时效
        string[] sd = { "4", "1", "6", "2", "3", "7" };
        string[] items = { "0", "1", "2", "3", "4", "5" };
        StringBuilder sb = new StringBuilder();

        //string[] lst = { "2018-01-09", "2018-01-10", 
        //                   "2018-01-11","2018-01-12" };
        if (dt != null && dt.Rows.Count > 0)
        {
            sb.Append("{");
            //获取dt表中lst 的最小值，用来处理下面的lst数组,
            DateTime minLST = DateTime.Parse(dt.Select("", "changedLST ASC")[0]["changedLST"].ToString());
            string[] lst = { "" + minLST.ToString("yyyy-MM-dd") + "", "" + minLST.AddDays(1).ToString("yyyy-MM-dd") + "",
                           "" + minLST.AddDays(2).ToString("yyyy-MM-dd") + "",""+minLST.AddDays(3).ToString("yyyy-MM-dd")+"" };
            int count = 0;    //记录是多少行
            string filter = "";
            int num = 0;
            int periodAttr = 0, lstAttr = 0;    //定义两个变量判断表格前两列是否要显示
            for (int i = 0; i < period.Length; i++)
            {
                periodAttr = 0;
                for (int k = 0; k < lst.Length; k++)
                {
                    lstAttr = 0;
                    for (int t = 0; t < sd.Length; t++)
                    {
                        if (period[i] != "72" && sd[t] == "7")       //不是72小时的时候出现时段为7的即全天的数据时不需要显示，只有72小时的显示全天的数据
                        {
                            break;
                        }
                        if (lst[k]=="2018-01-29" && sd[t]=="7" && period[i]=="72") {
                            string a = "";
                        }
                        filter = "period='" + period[i] + "' and changedLST >= '" + lst[k] + " 00:00:00' and changedLST <= '" + lst[k] + " 23:00:00' and durationID='" + sd[t] + "'";    //按时效、预报日期、时间段获取值
                        DataRow[] row = dt.Select(filter);
                        if (row.Length > 0)
                        {
                            count++;
                            string pA = "dis", lA = "dis";
                            if (periodAttr != 0)
                            {
                                pA = "indis";
                            }
                            if (lstAttr != 0)
                            {
                                lA = "indis";
                            }
                            sb.Append(ProDataRow(row, count, period[i], lst[k], sd[t], pA, lA, lst));
                            periodAttr++;
                            lstAttr++;
                        }
                    }
                }
            }
            //最后拼接日平均行的json，要显示在最后
            while (num < 2)
            {
                //durationID='" + 0 + "' or durationID='" + 7 + "' 接口返回的是7，和全天冲突，所以在保存的时候处理成0，因此这里如果从接口读的话是7，读保存的数据是0
                filter = "changedLST >= '" + lst[num + 1] + " 00:00:00' and changedLST <= '" + lst[num + 1] + " 23:00:00' and (durationID='" + 7 + "' or durationID='" + 0 + "')";    //按时效、预报日期、时间段获取值
                DataRow[] dr = dt.Select(filter);
                if (dr.Length > 0)
                {
                    count++;
                    if (num == 1)
                    {
                        sb.Append(ProDataRow(dr, count, "0", lst[num + 1], "0", "indis", "dis", lst));    //时效0表示日平均，时段0也表示日平均
                    }
                    else
                    {
                        sb.Append(ProDataRow(dr, count, "0", lst[num + 1], "0", "dis", "dis", lst));    //时效0表示日平均，时段0也表示日平均
                    }
                }
                num++;
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
        }
        string txt = sb.ToString();
        return txt;
    }

    public static string GetPeriodRowSpan(string period)
    {
        string txt = "";
        switch (period)
        {
            case "24小时": txt = "5"; break;
            case "48小时": txt = "5"; break;
            case "72小时": txt = "2"; break;
            case "日平均": txt = "2"; break;
        }
        return txt;
    }

    public static string GetDateRowSpan(string period, string lst, string[] compareLST)
    {
        string txt = "";
        if (lst == compareLST[0])
        {
            txt = "3";
        }
        else if (period == "24小时" && lst == compareLST[1])
        {
            txt = "2";
        }
        else if (lst == compareLST[1] && period == "48小时")
        {
            txt = "3";
        }
        else if (period == "48小时" && lst == compareLST[2])
        {
            txt = "2";
        }
        else
        {
            txt = "1";
        }
        return txt;
    }

    /// <summary>
    /// 对该时段下的行做处理
    /// </summary>
    /// <param name="row">数据行</param>
    /// <param name="t">页面显示的第几行，与DataTable的行不一致</param>
    /// <param name="period">时效</param>
    /// <param name="lst">预报日期</param>
    /// <param name="sd">时段</param>
    /// <returns></returns>
    public static StringBuilder ProDataRow(DataRow[] row, int t, string period, string lst, string sd, string periodAttr, string lstAttr, string[] compareLST)
    {
        StringBuilder sb = new StringBuilder();
        string[] pm25 = { "/", "/", "/" };   //第一个表示的是检测中心的值，第二个表示的是气象局的值，第三个是会商值
        string[] pm10 = { "/", "/", "/" };
        string[] no2 = { "/", "/", "/" };
        string[] O31h = { "/", "/", "/" };
        string[] O38h = { "/", "/", "/" };
        string[] aqi = { "/", "/", "/" };
        for (int n = 0; n < row.Length; n++)
        {
            int num = 0;
            string item = row[n]["itemid"].ToString();    //记录改行的itemID
            string module = row[n]["module"].ToString();     //记录改行的是哪家预报的    ManualSubmit:监测中心  SMCSubmit：气象局   ManualCenter：两家综合
            if (module == "GENERAL" || module == "ManualCenter")
            {    //两家
                num = 2;
            }
            else if (module == "WRF" || module == "SMCSubmit")
            {     //气象局
                num = 1;
            }
            #region   处理显示的值
            string strValue = row[n]["value"].ToString().Trim();
            string strAqi = row[n]["aqi"].ToString().Trim();
            string strPara = row[n]["parameter"].ToString().Trim();
            strValue = strValue == "null" ? "" : strValue;
            string reg = @"^[0-9]+";
            //这个正则有问题，首要污染物O3可能写成03
            //if (Regex.IsMatch(strValue, reg))
            //{
            //    strValue = float.Parse(strValue).ToString("f1");
            //}
            strAqi = strAqi == "null" ? "" : strAqi;
            strPara = strPara == "null" ? "" : strPara;
            #endregion
            string val = strValue + "/" + strAqi;     //在页面上需要显示的值
            if (sd == "7" || item == "0")
            {   //全天或首要污染物
                val = strAqi + "/" + strPara;
            }
            if (item == "0")
            {     //首要污染物
                aqi[num] = val;
            }
            else if (item == "1")
            {    //PM2.5
                pm25[num] = val;
            }
            else if (item == "2")
            {    //PM10
                pm10[num] = val;
            }
            else if (item == "3")
            {    //NO2
                no2[num] = val;
            }
            else if (item == "4")
            {    //O31h
                O31h[num] = val;
            }
            else if (item == "5")
            {     //O38h
                O38h[num] = val;
            }

        }
        string str = GetJson(t, period, lst, sd, pm25, pm10, no2, O31h, O38h, aqi, periodAttr, lstAttr, compareLST) + ",";
        sb.Append(str);
        return sb;
    }

    public static string GetJson(int n, string period, string lst, string sd,
        string[] pm25, string[] pm10, string[] no2, string[] o31h, string[] o38h, string[] aqi, string periodAttr, string lstAttr, string[] compareLST)
    {
        StringBuilder sb = new StringBuilder();
        sd = ProSD(sd);
        if (period == "0")
        {
            period = "日平均";
        }
        else
        {
            period = period + "小时";
        }
        string periodRowSpan = GetPeriodRowSpan(period);
        string lstRowSpan = GetDateRowSpan(period, lst, compareLST);
        sb.Append("\"row" + n + "\":{");
        sb.Append("\"Period\":[{\"val\":\"" + period + "\"},{\"rowspan\":\"" + periodRowSpan + "\"},{\"colspan\":\"1\"},{ \"class\": \"" + periodAttr + "\"}]");
        sb.Append(",\"Date\":[{\"val\":\"" + DateTime.Parse(lst).ToString("MM月dd日") + "\"},{\"rowspan\":\"" + lstRowSpan + "\"},{\"colspan\":\"1\"},{ \"class\": \"" + lstAttr + "\"}]");     //日期列
        sb.Append(",\"Interval\":[{\"val\":\"" + sd + "\"}]");     //时段列
        sb.Append(",\"Ele\":{");
        if (sd != "全天")   //72小时只显示全天的首要污染物
        {
            sb.Append("\"PM25\":[{ \"val\": \"" + pm25[0] + "\" }, { \"val\": \"" + pm25[1] + "\" }, { \"val\": \"" + pm25[2] + "\"}],");    //PM2.5列，包括了三个单元格的值两家和中心预报
            sb.Append("\"PM10\":[{ \"val\": \"" + pm10[0] + "\" }, { \"val\": \"" + pm10[1] + "\" }, { \"val\": \"" + pm10[2] + "\"}],");       //PM10列
            sb.Append("\"NO2\":[{ \"val\": \"" + no2[0] + "\" }, { \"val\": \"" + no2[1] + "\" }, { \"val\": \"" + no2[2] + "\"}],");            //NO2列
            sb.Append("\"O31\":[{ \"val\": \"" + o31h[0] + "\" }, { \"val\": \"" + o31h[1] + "\" }, { \"val\": \"" + o31h[2] + "\"}],");        //O31列
            sb.Append("\"O38\":[{ \"val\": \"" + o38h[0] + "\" }, { \"val\": \"" + o38h[1] + "\" }, { \"val\": \"" + o38h[2] + "\"}],");            //O38列
        }
        if (sd == "全天")
        {
            sb.Append("\"AQI\":[{ \"val\": \"" + aqi[0].Split('/')[0] + "\",\"poll\":\"" + (aqi[0].Split('/')[1].Contains("03")? "O3" : aqi[0].Split('/')[1]) + "\" }, " +
            "{ \"val\": \"" + aqi[1].Split('/')[0] + "\" ,\"poll\":\"" + (aqi[1].Split('/')[1].Contains("03")? "O3" : aqi[1].Split('/')[1]) + "\"}," +
            "{ \"val\": \"" + aqi[2].Split('/')[0] + "\",\"poll\":\"" + (aqi[2].Split('/')[1].Contains("03")? "O3" : aqi[2].Split('/')[1]) + "\"}]");           //AQI列
        }
        else
        {
            sb.Append("\"AQI\":[{ \"val\": \"" + aqi[0] + "\" }, { \"val\": \"" + aqi[1] + "\" }, { \"val\": \"" + aqi[2] + "\"}]");           //AQI列
        }
        sb.Append("}}");
        // sb.Remove(sb.Length - 1, 1);
        string txt = sb.ToString();
        return txt;
    }

    public static string ProSD(string sd)
    {
        string txt = "";//{ "4", "1", "6", "2", "3", "7" };
        switch (sd)
        {
            case "0": txt = "日平均"; break;
            case "1": txt = "下半夜"; break;
            case "2": txt = "上午"; break;
            case "3": txt = "下午"; break;
            case "4": txt = "上半夜"; break;
            case "7": txt = "全天"; break;
            case "6": txt = "夜间"; break;
        }
        return txt;
    }


    //要对接口中的数据做处理，下半夜的时间要加1，例如9月9日下半夜的数据要读9月10日下班夜的数据，其他的时段不变，解决方案是增加一个新的lst列，根据新增的列来读取数据
    public static DataTable ProInterfaceTab(DataTable dt)
    {
        DataTable newTab = dt.Copy();
        newTab.Columns.Add("changedLST");
        foreach (DataRow row in newTab.Rows)
        {
            string durationId = row["durationID"].ToString();
            string lst = row["LST"].ToString();
            string changedLST = lst;
            if (durationId == "1")
            {
                changedLST = DateTime.Parse(lst).AddDays(-1).ToString("yyyy-MM-dd HH:00:00");
            }
            row["changedLST"] = changedLST;
        }
        return newTab;
    }

    public static string GetDataUrl(string date)
    {
        string url = ConfigurationManager.AppSettings["AQIPeriodDataURL"];
        url = url.Replace("%", "&");
        string method = "forecastGroupNew";
        string module = "ManualCenter,SMCSubmit,ManualSubmit";
        url = string.Format(url, method, module, date);
        return url;
    }

    /// <summary>
    /// 获取72时效的接口数据
    /// </summary>
    /// <param name="url">URL</param>
    /// <param name="postDataStr"></param>
    /// <returns></returns>
    public static string HttpGetContent(string url, string postDataStr)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "Get";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    /// <summary>
    /// 将json转换为DataTable
    /// </summary>
    /// <param name="strJson">得到的json</param>
    /// <returns></returns>
    public static DataTable JsonToDataTable(string strJson)
    {
        //转换json格式
        strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
        //取出表名   
        var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
        string strName = rg.Match(strJson).Value;
        DataTable tb = null;
        //去除表名   
        strJson = strJson.Substring(strJson.IndexOf("[") + 1);
        strJson = strJson.Substring(0, strJson.IndexOf("]"));

        //获取数据   
        rg = new Regex(@"(?<={)[^}]+(?=})");
        MatchCollection mc = rg.Matches(strJson);
        for (int i = 0; i < mc.Count; i++)
        {
            string strRow = mc[i].Value;
            string[] strRows = strRow.Split('*');

            //创建表   
            if (tb == null)
            {
                tb = new DataTable();
                tb.TableName = strName;
                foreach (string str in strRows)
                {
                    var dc = new DataColumn();
                    string[] strCell = str.Split('#');

                    if (strCell[0].Substring(0, 1) == "\"")
                    {
                        int a = strCell[0].Length;
                        dc.ColumnName = strCell[0].Substring(1, a - 2);
                    }
                    else
                    {
                        dc.ColumnName = strCell[0];
                    }
                    tb.Columns.Add(dc);
                }
                tb.AcceptChanges();
            }

            //增加内容   
            DataRow dr = tb.NewRow();
            for (int r = 0; r < strRows.Length; r++)
            {
                dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
            }
            tb.Rows.Add(dr);
            tb.AcceptChanges();
        }

        return tb;
    }

    public static string ItemToItemID(string item)
    {
        string txt = "";
        switch (item)
        {
            case "AQI": txt = "0"; break;
            case "PM25": txt = "1"; break;
            case "PM10": txt = "2"; break;
            case "NO2": txt = "3"; break;
            case "O31": txt = "4"; break;
            case "O38": txt = "5"; break;
        }
        return txt;
    }

    //2016年1月21日，修改NO2计算方法,NO2的计算方法是按照小时来计算的。
    [WebMethod]
    public static int ToAQI(string value, string item)
    {
        string itemID = ItemToItemID(item.Trim());
        int AQIValue = 0;
        double inputValue = 0d;
        double.TryParse(value, out inputValue);
        inputValue = inputValue / 1000;
        switch (itemID)
        {
            case "1":
                AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 24, 11, 180);
                break;
            case "2":
                AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 7, 11, 180);
                break;
            case "3":
                AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 22, 10, 0);
                break;
            case "4":
                AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 10, 0);
                break;
            case "5":
                AQIValue = Lucas.AQI2012.ConvertAQI.ConvertToAQI(inputValue, 8, 16, 16);
                break;
        }
        return AQIValue;
    }

    //保存按钮
    /// <summary>
    /// 保存数据入库
    /// </summary>
    /// <param name="json">前台的json数据，需要解析</param>
    /// <param name="UserName">当前登录账户</param>
    /// <param name="period">时效，区分早上和下午的数据</param>
    /// <returns></returns>
    [WebMethod]
    public static string Save(string json, string UserName, string period)
    {
        DateTime dNow = DateTime.Now;
        string interfaceTxt = GetInterface(dNow.ToString("yyyy-MM-dd 18:00:00"), "sub");
        DataTable result = StrJsonToDataTable(json, period);
        string tip = InserTable(result, UserName);
        return tip;
    }

    //将前台json转成的DataTable入库
    public static string InserTable(DataTable dt, string UserName)
    {
        string tip = "ok";
        if (dt != null && dt.Rows.Count > 0)
        {
            string sql = "insert into T_ForecastGroupCopy (LST,ForecastDate,PERIOD,Site,durationID,ITEMID,Value,AQI,Module,Parameter,UserName) values ('{0}','{1}','{2}','58637','{3}','{4}',{5},{6},'{7}','{8}','" + UserName + "')";
            string sqlDel = "delete T_ForecastGroupCopy where ForecastDate ='{0}'";
            try
            {
                string forecastdate = dt.Rows[0]["forecastdate"].ToString().Trim();     //发布日期是相同的
                m_Database.Execute(string.Format(sqlDel, forecastdate));   //先删掉以前的
                foreach (DataRow row in dt.Rows)
                {
                    string lst = row["lst"].ToString().Trim();
                    DateTime lstTime = DateTime.Parse(lst);
                    DateTime forecastTime = DateTime.Parse(row["ForecastDate"].ToString().Trim());
                    string period = row["period"].ToString().Trim();
                    string duration = GetDurationId(row["duration"].ToString().Trim());
                    if (period.IndexOf("小时") > -1)
                    {
                        period = period.Substring(0, 2);
                    }
                    else
                    {    //日平均
                        duration = "0";
                        double day = (lstTime - forecastTime).TotalDays;
                        if (day <= 0)
                        {
                            period = "24";
                        }
                        else if (day <= 1 && day > 0)
                        {
                            period = "48";
                        }
                        else if (day <= 2 && day > 1)
                        {
                            period = "72";
                        }
                    }
                    
                    string itemid = row["itemid"].ToString().Trim();
                    string value = row["value"].ToString().Trim();
                    if (itemid == "0")
                    {   //首要污染物
                        value = "";
                    }
                    value = value == "" ? "null" : value;
                    string aqi = row["aqi"].ToString().Trim();
                    aqi = aqi == "" ? "null" : aqi;
                    string module = row["module"].ToString().Trim();
                    string parameter = row["parameter"].ToString().Trim();
                    string sqlInsert = string.Format(sql, lst, forecastdate, period, duration, itemid, value, aqi, module, parameter);
                    m_Database.Execute(sqlInsert);
                }

            }
            catch
            {
                tip += "error";
            }
        }
        return tip;
    }

    public static string GetDurationId(string duration)
    {
        string txt = "";
        switch (duration)
        {
            case "日平均": txt = "0"; break;
            case "下半夜": txt = "1"; break;
            case "上午": txt = "2"; break;
            case "下午": txt = "3"; break;
            case "上半夜": txt = "4"; break;
            case "全天": txt = "7"; break;
            case "夜间": txt = "6"; break;
        }
        return txt;
    }
    //前台传来的json转成DataTable  直观一点
    public static DataTable StrJsonToDataTable(string json, string period)
    {
        DataTable dataTable = new DataTable();
        string[] col = { "lst", "forecastdate", "period", "duration", "itemid", "value", "aqi", "module", "parameter" };
        foreach (string str in col)
        {
            dataTable.Columns.Add(str);
        }
        try
        {
            json = json.Replace("\"", "\'");
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
            if (arrayList.Count > 0)
            {
                Dictionary<string, object> dictionary = arrayList[0] as Dictionary<string, object>;    //最外层只有一个中括号，不需要遍历
                if (dictionary.Keys.Count<string>() == 0)
                {
                    return dataTable;
                }//Rows

                foreach (string current in dictionary.Keys)       //遍历每一行 row1:{}
                {
                    Dictionary<string, object> dict = dictionary[current] as Dictionary<string, object>;    //获取改行的数据转换成Dictionary
                    List<List<string>> row = new List<List<string>>();    //存储前三列对应污染物的值不变
                    List<List<string>> pollDict = new List<List<string>>();   //污染物
                    string sd = ((Dictionary<string, object>)(dict["Interval"] as ArrayList)[0])["val"].ToString();   //获取时段，时段为全天的要特殊处理
                    foreach (string key in dict.Keys)          //遍历每一行的每一项，如period、时段等 
                    {
                        //通过key值获取该项的数据转换成Dictionary
                        if (key != "Ele")
                        {
                            ArrayList arrList = dict[key] as ArrayList;
                            ForeachDictionary(arrList, key, row, sd);
                        }
                        else
                        {    //处理ele项，ele项是一个对象，要转换成Dictionary

                            Dictionary<string, object> d = dict[key] as Dictionary<string, object>;
                            foreach (string ele in d.Keys)
                            {    //遍历ele里面的各项
                                ArrayList poll = d[ele] as ArrayList;   //将每一项的数据转换成Dictionary
                                                                        // string sd = (dict["Interval"] as ArrayList)[1].ToString();
                                ForeachDictionary(poll, ele, pollDict, sd);
                            }
                        }
                    }
                    CreatDataTable(row, pollDict, dataTable, period);
                }
            }
        }
        catch (Exception e)
        {
            return dataTable;
        }
        return dataTable;
    }

    /// <summary>
    /// 获取lst值，前台传过来的没有年，要确定年月日为最终的lst，主要存在是夸年问题
    /// </summary>
    /// <param name="lst">前台的lst，xx月xx日格式</param>
    /// <param name="mouth">现在的月份</param>
    /// <returns></returns>
    public static string GetLST(string lst, int month, int year)
    {
        int lstMonth = int.Parse(lst.Split("月".ToCharArray())[0]);   //lst中的月份
        string LST = DateTime.Parse(year + "年" + lst).ToString("yyyy-MM-dd");
        if ((lstMonth > month) && lstMonth == 12)
        {   //跨年了，lst的年要减去一年
            LST = DateTime.Parse((year - 1) + "年" + lst).ToString("yyyy-MM-dd");
        }
        return LST;
    }
    /// <summary>
    /// datatable创建行
    /// </summary>
    /// <param name="cols">前三列的值对应后面的污染物不变</param>
    /// <param name="pollDic">污染物</param>
    /// <returns></returns>
    public static DataTable CreatDataTable(List<List<string>> row, List<List<string>> pollDic, DataTable dt, string p)
    {
        DateTime dNow = DateTime.Now;
        string hour = " 00:00:00";   //17点的时候存储为00：00：00,原来评分系统用的是17点，不敢改，早上的不参与，
        if (p == "06")
        {
            hour = " 06:00:00";
        }
        string forecastdate = dNow.ToString("yyyy-MM-dd") + hour;
        string period = row[0][0].ToString();
        string _lst = row[1][0].ToString();
       
        string sd = row[2][0].ToString();
        string lst = GetLST(_lst, dNow.Month, dNow.Year) + hour;
        for (int i = 0; i < pollDic.Count; i++)
        {
            List<string> temp = pollDic[i];
            string module = "ENV";
            string itemId = (i + 1).ToString();
            if (itemId == "6" || sd == "全天")
            {    //首要污染物
                itemId = "0";
            }
            for (int j = 0; j < temp.Count; j++)
            {
                DataRow dr = dt.NewRow();
                string value = temp[j].Split('/')[0];
                string aqi = temp[j].Split('/')[1];
                string parameter = "";
                if (itemId == "0")
                {
                    parameter = aqi;
                    aqi = value;
                    value = null;
                }
                if (j == 1)
                {
                    module = "WRF";  //气象局
                }
                else if (j == 2)
                {
                    module = "GENERAL";    //两家
                }
                dr["period"] = period;
                dr["lst"] = lst;
                dr["duration"] = sd;
                dr["forecastdate"] = forecastdate;
                dr["itemid"] = itemId;
                dr["value"] = value;
                dr["aqi"] = aqi;
                dr["module"] = module;
                dr["parameter"] = parameter;

                dt.Rows.Add(dr);
            }
        }
        return dt;
    }
    public static List<List<string>> ForeachDictionary(ArrayList arrList, string key, List<List<string>> arrResult, string sd)
    {
        string str = "";
        List<string> list = new List<string>();   //记录dictionary中的子项数组，最后转换成dictionary
        foreach (Dictionary<string, object> dic in arrList)
        {
            string flag = "";   //记录dic的子项（val,rowspan.colspan等），用来判断是否等于val，如果等于则需要配合key 值退出foreach
            foreach (string _key in dic.Keys)
            {
                flag = _key;
                if (_key == "val")
                {
                    str = dic["val"].ToString();    //获取值  { "val": "24小时" }
                    if (sd == "全天" && key == "AQI")
                    {
                        str = dic["val"].ToString() + "/" + dic["poll"].ToString();
                    }
                    break;
                }
            }
            list.Add(str);    //按顺序添加1表示检测中心，2表示气象局，3表示两家
            if ((key == "Period" || key == "Date" || key == "Interval") && flag == "val")    //这几项里面有一些其他的属性rowspan、colspan、class等
            {
                break;
            }
        }
        arrResult.Add(list);
        return arrResult;
    }

    [WebMethod]
    public static string TxtSave(string content, string isSubData, string period,string type)
    {
        try
        {
            string localPath = ConfigurationManager.AppSettings["AQIPeriod72TempPath"];
            content = content.Replace("\n", "\r\n");
            //string localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _path);
            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }
            string tempName = "AQIPeriodText48_";
            if (type == "message") {
                tempName = "AQIPeriodMsg48_";
            }
            DateTime dNow = DateTime.Now;
            string fileName = "", time = dNow.ToString("yyyy-MM-dd");
           // string[] tempName = { "AQIPeriodText48_", "AQIPeriodMsg48_" };    //用来拼文件名称，要保存两次，所以定义在数组里面遍历保存
            //if (isSubData != "True")
            //{     //历史数据，日期要减1天
            //    time = dNow.AddDays(-1).ToString("yyyy-MM-dd");
            //}
            fileName = tempName + time + ".txt";
            Write(localPath + "\\" + fileName, content);
        }
        catch (Exception e)
        {
            return "error" + e.Message;
        }
        return "ok";
    }

    [WebMethod]
    public static string Publish(string content, string period,string type,string user)
    {
        DateTime dNow = DateTime.Now;
        string localIP = HttpClientHelper.GetIP();   //写入日志时记录本地机器的ip地址
        content = content.Replace("\n", "\r\n");
        string sqlFTP = "select name,fileName,ip,account,port,password,catalog,type,template from dbo.T_AQIPeriod72_FTP where period='" + period + "' and purpose='test' and type='"+type+"'";
        DataTable dt_Ftp = m_Database.GetDataTable(sqlFTP);
        string localPath = ConfigurationManager.AppSettings["AQIPeriod72TempPath"];
        TxtSave(content, "True", period,type);   //保存到本地
        string MD5FileName = "";    //创建md5码的文件名称
        string fullFileName = "";    //需上传ftp文件的全名称（包括了文件路径及文件名称）
        string tip = "";
        string logFileTempPath="" , startTime="" , endTime="", catalog="", fileName="", url="",state="0", productName="",productType= "AQI预报", ftpIP="";
        foreach (DataRow dr in dt_Ftp.Rows)
        {
            try
            {
                #region  推送文本
                string name = dr["name"].ToString().Trim();
                productName = GetProductName(name);
                startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");     //写入日志开始时间列
                fileName = dr["fileName"].ToString().Trim();
                fileName = fileName.Replace("YYYYMMDD", dNow.ToString("yyyyMMdd"));
                string ip = dr["ip"].ToString().Trim();
                string userName = dr["account"].ToString().Trim();
                string port = dr["port"].ToString().Trim();
                string password = dr["password"].ToString().Trim();
                catalog = dr["catalog"].ToString().Trim();
                port = port != "" ? port : "21";
                if (catalog.Substring(0, 1) == "/" && catalog != "")
                {
                    catalog = catalog.Remove(0, 1);
                }
                url = ip + ":" + port;
                string localFileName = dNow.ToString("yyyyMMddHHmmss") + "_Text_" + fileName;
                logFileTempPath = localPath + localFileName;
                ftpIP = url + "//" + catalog + "//" + fileName;
                if (type == "message")
                {
                    productType = "AQI分时段48小时预报短信模板";
                    productName = "短信";
                    string tempalte = dr["template"].ToString().Trim();
                    LiveIndex index = new LiveIndex();
                    localPath = ConfigurationManager.AppSettings["FtpUploadMessageTempPath"];    //短信本地保存的路径不一样
                    localFileName = dNow.ToString("yyyyMMddHHmmss") + "AQIPeriod_Msg.txt";
                    logFileTempPath = localPath + "//" + localFileName;
                    Write(localPath + "//" + localFileName, content);    //把yyyyMMddHHmmssAQIPeriod_Msg.txt文件保存在本地
                    #region   生成md5码
                    string fileCreateTime = dNow.ToString("yyyyMMddHHmmss");
                    MD5FileName = "SMS" + "-" + "20012" + "-" + fileCreateTime + ".txt";
                    string fName = MD5FileName;
                    Write(localPath+"//"+MD5FileName,content);   //把需要生成md5码的文件保存在本地，用来生成md5码
                    MD5FileName = index.SetFileName(type, fName, localPath, fileCreateTime);    //这里生成md5的算法调用以前生活指数项目里面的算法
                    localFileName = MD5FileName;   //保存在本地文件的名称，这里和上传ftp的文件名称不同
                    fileName = MD5FileName;     //上传ftp文件的名称
                    #endregion  MD5结束
                    content = MessTempProcess(tempalte, content);    //生成发送短信的最终文本格式
                    ftpIP = url + "//" + catalog;
                }
                fullFileName = Path.Combine(localPath, localFileName);
                Write(fullFileName, content);
                Ftp ftp = new Ftp(url, userName, password);
                ftp.Upload(fullFileName, catalog + "/" + fileName);
                endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");       //写入日志结束时间列
                #endregion
                tip += "ok" + ";";
            }
            catch (Exception e)
            {
                endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                state = "1";
                tip += "error" + e.Message + ";";
                File.Delete(logFileTempPath);   //出现异常说明ftp发布失败，要把原先保存在本地的文件删除掉，不然日志预览的时候能看到文本
            }
            ChangeState(tip, startTime, endTime, period);
            ftpIP = url + "//" + catalog + "//" + fileName;   //正式发布要注释
            WriteLog(productType, productName, startTime, endTime, state, url + "//" + catalog + "//" + fileName, user, localIP, endTime, "Type", logFileTempPath);
        }
        return tip;
    }

    public static void ChangeState(string tip, string reTime, string deadLine, string period) {
        string state = "";
        try {
            if (tip.IndexOf("ok") > -1 && tip.IndexOf("error") > -1)
            {
                state = "4";
            }
            else if (tip.IndexOf("ok") > -1)
            {
                state = "3";
            }
            else if (tip.IndexOf("error") > -1) {
                state = "2";
            }
            string strInsertStateSQL = "INSERT INTO T_State(ModuleType,ReTime,DeadLine,State,Type) VALUES('AQIPeriod48_" + period + "','" + reTime + "','" + deadLine + "','" + state + "','2')";
            m_Database.Execute(strInsertStateSQL);
        }
        catch (Exception e) { }
    }
    public static string GetProductName(string name) {
        string productName = "";
        switch (name)
        {
            case "InfoCenterFtp":
                productName = "AQI分时段预报(上传信息中心)";
                break;
            case "SciServiceCenter":
                productName = "AQI分时段预报(上传科技服务中心)";
                break;
            case "AQILocal":
                productName = "分时段AQI预报上传到fserver";
                break;
            case "AQILocal62":
                productName = "分时段AQI152业务气象台传文件";
                break;
            case "62WebSite":
                productName = "AQI分时段预报(上传62网站)";
                break;
            default:
                productName = "分时段文件";
                break;
        }
        return productName;
    }

    public static string MessTempProcess(string template,string content) {
        string result = "", phone = "";
        string queryPhone = "select receiver,number,flag from T_Messgae";
        DataTable dt = m_Database.GetDataTable(queryPhone);
        foreach (DataRow row in dt.Rows) {
            phone += row["number"].ToString().Trim()+Environment.NewLine;
        }
        phone = phone.TrimEnd(Environment.NewLine.ToCharArray());
        phone = "15001711565";
        result = template.Replace("{phone}",phone).Replace("{value}",content);
        return result;
    }
    public static void Write(string fullFileName, string txt)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(fullFileName,false))
            {
                sw.Write(txt);
                sw.Flush();
                sw.Close();
            }
        }
        catch (Exception e)
        {

        }
    }

    public static void WriteLog(string productType, string productName, string startTime,
        string endTime, string state, string ftpIP, string user, string localIP, string deadLine, string type, string fileTempPath)
    {
        try
        {
            // ProductType, ProductName, ReleaseType,StartTime,EndTime,State,Address,[User],IPAddress,Detail,DeadLine,Type,FileTempPath
            string strInsertSQL = "INSERT INTO T_ProductLog (ProductType, ProductName, ReleaseType,StartTime,EndTime,State,Address,[User],IPAddress,Detail,DeadLine,Type,FileTempPath) ";
            string strAQL = String.Format("SELECT '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'",
                productType, productName, "FTP", startTime, endTime, state, ftpIP, user, localIP, "Detail", deadLine, type, fileTempPath);
            strInsertSQL += strAQL;
            m_Database.Execute(strInsertSQL);
        }
        catch(Exception e){
            string tip = e.Message;
        }
    }
    //读取历史文本数据
    [WebMethod]
    public static string ReaderHistoryTxt(string type) {
        DateTime dNow = DateTime.Now;
        string[] tempName = { "AQIPeriodText48_", "AQIPeriodMsg48_" };
        string time = dNow.AddDays(-1).ToString("yyyy-MM-dd");
        string txt = "";
        string localPath = ConfigurationManager.AppSettings["AQIPeriod72TempPath"];
        for (int i = 0; i < tempName.Length; i++) {
            string fileName = tempName[i] + time+".txt";
            txt += Reader(Path.Combine(localPath+fileName)) + "#";
        }
        txt = txt.TrimEnd('#');
        return txt;
    }
    [WebMethod]
    public static string Reader(string fullFileName)
    {
        string txt = "";
        try
        {
            //, Encoding.GetEncoding("GB2312")
            using (StreamReader sr = new StreamReader(fullFileName))
            {
                txt = sr.ReadToEnd();
                sr.Close();
            }
        }
        catch (Exception e)
        {
            txt = "|";
        }
        return txt;
    }
}