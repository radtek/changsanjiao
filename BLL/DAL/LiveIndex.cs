using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;
using Utility.GradeGuide;
using System.IO;
using System.Reflection;
using MMShareBLL.Properties;
using System.Web.UI;
using OpenFTP;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Diagnostics;
using System.Threading;
//using WeatherForecastData;

namespace MMShareBLL.DAL
{
    public class LiveIndex
    {
        Database m_database;
        private string DataTableToJson(string jsonName, System.Data.DataTable dt)
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
        //获取站点数据
        public DataTable GetSite() {
            m_database = new Database("DBCONFIG");
            string sql = "SELECT name,stationCo FROM Weather_Station";
            return m_database.GetDataTable(sql);
        }
        public string GetData()
        {
            m_database = new Database("DBCONFIG");
            string sql_query = "select code,name from Weather_Index";
            DataTable dt= m_database.GetDataTable(sql_query);
            return DataTableToJson("data",dt);
        }
        public string GetDataDetails(string code) {
             m_database = new Database("DBCONFIG");
            string html = "<table class='subtab'>";
            string query = "select mainLevel,levelValue,subLevel,meanName,tparams,levelName,rvalue,rtip,tipInfo from dbo.Weather_Level where indexCode='" + code + "' ";
            DataTable dTable = m_database.GetDataTable(query);
            if(dTable!=null&&dTable.Rows.Count>0){
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    string mainLevel = dTable.Rows[i][0].ToString().Trim();  //主级别
                    string levelValue = dTable.Rows[i][1].ToString().Trim();
                    string subLevel = dTable.Rows[i][2].ToString().Trim();
                    string meanName = dTable.Rows[i][3].ToString().Trim();
                    string param = dTable.Rows[i][4].ToString().Trim();
                    string levelName = dTable.Rows[i][5].ToString().Trim();
                    string rvalue = dTable.Rows[i][6].ToString().Trim();
                    string rtip = dTable.Rows[i][7].ToString().Trim();
                    string tipInfo = dTable.Rows[i][8].ToString().Trim();
                    html += "<tr ondblclick='showModel($(this))'><td class='td5'>" + mainLevel + "</td><td class='td5'>" + levelValue + "</td><td class='td5'>" + subLevel + "</td><td class='td10'>" + meanName + "</td><td class='td10'>" + param + "</td><td class='td10'>" + levelName + "</td><td class='td5'>" + rvalue + "</td><td class='td10'>" + rtip + "</td><td class='td10'>" + tipInfo + "</td></tr>";
                }
                html += "</table>";
            }
            return html;
        }


        //指数订正提交按钮
        public string Submit(string code,string jibie,string mainjibie,string subjibie,string mean,string param,string explain,string scopelimit,string scopexplain,string tip) {
            m_database = new Database("DBCONFIG");
            string sql_update = "UPDATE Weather_Level SET mainLevel='"+mainjibie+"',levelValue='"+jibie+"',subLevel='"+subjibie+"',meanName='"+mean+"',tparams='"+param+"',"+
            "levelName='"+explain+"',rvalue='"+scopelimit+"',rtip='"+scopexplain+"',tipInfo='"+tip+"' where indexCode='"+code+"' and levelValue='"+jibie+"'";
            try {
                m_database.Execute(sql_update);
                return "ok";
            }
            catch(Exception e){
                return "error";
            }
        }

        //获取指数订正表格     status为发布状态，即发布或未发布,site站点id，lst预报日期
        //这个方法与GetTable一样，只不过返回类型不相同，主要是用于导出Excel获取DataTable的方法接口
        public DataTable GetTableII(string status, string site, string lst)
        {
            m_database = new Database("DBCONFIG");
            status = HttpUtility.UrlDecode(status);
            site = HttpUtility.UrlDecode(site);
            lst = HttpUtility.UrlDecode(lst);
            DateTime dNow = DateTime.Now;
            string foreTime = dNow.ToString("yyyy-MM-dd");   //发布日期
            string LST = DateTime.Parse(lst).ToString("yyyy-MM-dd");
            string timeWhere = "lst between '" + LST + " 00:00:00' and '" + LST + " 23:59:59'";
            DataTable result = new DataTable();
            string sql = "select name,code,name as mc from Weather_Index where gname2='" + status + "' order by indexSort asc";
            if (status == "")
            {   //如果状态为空则说明是点击首席检查所需要获取的文本内容
                sql = "select name,code,name as mc from Weather_Index order by indexSort asc";
            }
            DataTable dt = m_database.GetDataTable(sql);     //基础数据表中的数据
            result = DataProcess(dt, site, LST);
            return result;
        }

         //获取指数订正表格     status为发布状态，即发布或未发布,site站点id，lst预报日期
        public string GetTable(string status, string site, string lst)
        {
            m_database = new Database("DBCONFIG");
            status = HttpUtility.UrlDecode(status);
            site = HttpUtility.UrlDecode(site);
            lst = HttpUtility.UrlDecode(lst);
            DateTime dNow = DateTime.Now;
            string foreTime = dNow.ToString("yyyy-MM-dd");   //发布日期
            string LST = DateTime.Parse(lst).ToString("yyyy-MM-dd");
            string timeWhere = "lst between '" + LST + " 00:00:00' and '" + LST + " 23:59:59'";
            DataTable result = new DataTable();
            string sql = "select name,code,name as mc from Weather_Index where gname2='" + status + "' order by indexSort asc";
            if (status == "")
            {   //如果状态为空则说明是点击首席检查所需要获取的文本内容
                sql = "select name,code,name as mc from Weather_Index order by indexSort asc";
            }
            DataTable dt = m_database.GetDataTable(sql);     //基础数据表中的数据
            result = DataProcess(dt,site, LST);
            return DataTableToJson("data", result);
        }

        public DataTable DataProcess(DataTable basic, string site, string lst)
        {
            DataTable result = new DataTable();
            if (basic != null && basic.Rows.Count > 0)
            {
                string sql_fore = "", sql_save = "";
                //循环遍历指数获取相对应的值（每一个指数的最大时间可能不一样）
                for (int i = 0; i < basic.Rows.Count; i++)
                {
                    //获取预报数据
                    string code = basic.Rows[i][1].ToString();
                    string name = basic.Rows[i][0].ToString();
                    string fore_where = " where a.station='" + site + "' and a.lst between '" + lst + " 00:00:00' and '" + lst + " 23:59:59' and a.code='" + code + "'";
                    if (code.IndexOf("rtssd") < 0 && code.IndexOf("gmzs") < 0)
                    {
                        sql_fore += "select c.name as name1,a.code, c.name,a.Value,a.Grade,B.meanName,b.levelName,b.tipInfo,b.tparams,a.ForecastDate from Weather_IndexResult as A" +
                             " left join Weather_Level AS B on A.code =B.indexCode left join Weather_Index as C on a.code=c.code " + fore_where + " and a.mainlevel=B.mainlevel" +
                             " and a.Grade=B.levelValue and A.ForecastDate=(select MAX(ForecastDate) from Weather_IndexResult as a " + fore_where + ") union all ";
                    }
                    else{
                        //人体指数 /感冒
                        sql_fore += "select c.name as name1,a.code, c.name,a.Value,a.Grade,d.mainTip,d.shortTip,d.TipInfo,c.name,a.ForecastDate from Weather_IndexResult as A "+
                            "left join Weather_Index as C on a.code=c.code left join Weather_IndexTip as d "+
                            " on d.code=a.code  " + fore_where + " and d.ForecastDate=" +
                            "(select MAX(ForecastDate) from Weather_IndexResult as a " + fore_where + ") and  A.ForecastDate=d.ForecastDate and a.station=d.station and a.lst=d.lst union all ";
                    };
                   
                    //获取保存的数据
                    string where = "where station='" + site + "' and indexCode='" + code + "' and lst between '" + lst + " 00:00:00' and '" + lst + " 23:59:59'";
                    string field = "INDEXCODE,NAME,RVALUE,LEVELVALUE,MEANNAME,BRIEFTIP,TIPINFO,params,realtime";
                    sql_save += "select NAME1 as NAME," + field+" FROM (SELECT top 1 NAME as NAME1,"+field+" FROM T_Weather_Save " + where + " and forecastdate=(select MAX(ForecastDate) from T_Weather_Save " + where + ")  order by realtime desc) as b union all ";
                }
                sql_fore = sql_fore.TrimEnd("union all".ToCharArray());
                sql_save = sql_save.TrimEnd("union all".ToCharArray());
                DataTable dt_fore = m_database.GetDataTable(sql_fore);
                DataTable dt_save = m_database.GetDataTable(sql_save);
                //预报数据和保存数据整合
                result = MerTable(basic,dt_save, dt_fore);
            }
            return result;
        }

        //将基础数据与指数值整合
        public DataTable MerTable(DataTable basic, DataTable save, DataTable fore)
        {
            DataTable result = fore.Clone();
            if(basic!=null&&basic.Rows.Count>0){
                for (int i = 0; i < basic.Rows.Count;i++ )
                {
                    bool compareTime = false;
                    string name = basic.Rows[i]["name"].ToString();
                    string code = basic.Rows[i]["code"].ToString();
                    DataRow[] dr_save = save.Select("INDEXCODE='" + code + "'");
                    DataRow[] dr_fore = fore.Select("code='" + code + "'");
                    if (dr_save.Length > 0 && dr_fore.Length > 0)
                    {
                        compareTime = DateTime.Parse(dr_save[0][9].ToString()) > DateTime.Parse(dr_fore[0][9].ToString());
                    }
                    else if (dr_save.Length > 0 && dr_fore.Length == 0)
                    {
                        compareTime = true;
                    }
                    else if (dr_save.Length == 0 && dr_fore.Length > 0)
                    {
                        compareTime = false;
                    }
                    //取保存表中的数据
                    if (compareTime)
                    {
                        result.Rows.Add(dr_save[0].ItemArray);
                    }
                    else
                    {
                        //取预报
                        if (dr_fore.Length > 0)
                        {
                            result.Rows.Add(dr_fore[0].ItemArray);
                        }
                        else
                        {
                            result.Rows.Add(name, code, name, 999.9, -1);
                        }
                    }
                }
            }
            return result;
        }
        //以前写的，速度慢
        public DataTable DataProcess2(DataTable basic,string site,string lst) {
            DataTable result = new DataTable();
            if (basic != null && basic.Rows.Count > 0)
            {
                //循环遍历指数获取相对应的值（每一个指数的最大时间可能不一样）
                for (int i = 0; i < basic.Rows.Count; i++)
                {
                    //获取预报数据
                    string code = basic.Rows[i][1].ToString();
                    string name = basic.Rows[i][0].ToString();
                    string fore_where1 = " where a.station='" + site + "' and a.lst between '" + lst + " 00:00:00' and '" + lst + " 23:59:59' and a.code='" + code + "'";
                    DataTable dt_fore = GetIndexForeTab(fore_where1, code);
                    //获取保存的数据
                    string where = "where station='" + site + "' and indexCode='" + code + "' and lst between '" + lst + " 00:00:00' and '" + lst + " 23:59:59'";
                    DataTable dt_save = SaveTable(basic, where);
                    //预报数据和保存数据整合
                    DataTable temp = MerTable2(name, code, name, dt_save, dt_fore);
                    if (i == 0)
                    {
                        result = temp.Clone();
                    }
                    result.Rows.Add(temp.Rows[0].ItemArray);
                }
            }
            return result;
        }

        public DataTable SaveTable(DataTable basic,string where ) {
            m_database = new Database("DBCONFIG");
            string sql = "SELECT NAME,INDEXCODE,NAME,RVALUE,LEVELVALUE,MEANNAME,BRIEFTIP,TIPINFO,params,realtime FROM T_Weather_Save " + where + " and forecastdate=(select MAX(ForecastDate) from T_Weather_Save "+where+")";
            DataTable dt = m_database.GetDataTable(sql);
            return dt;
        }
        //将基础数据与指数值整合
        //以前写的，速度慢
        public DataTable MerTable2(string name,string code,string name2,DataTable save, DataTable fore) {
            //保存表的数据优先
            DataTable result = fore.Clone();
            //取时间大的显示
            bool compareTime = false;
            if (save.Rows.Count > 0 && fore.Rows.Count > 0) {
                compareTime = DateTime.Parse(save.Rows[0][9].ToString()) > DateTime.Parse(fore.Rows[0][9].ToString());
            }
            else if (save.Rows.Count > 0 && fore.Rows.Count == 0) {
                compareTime = true;
            }
            else if (save.Rows.Count == 0 && fore.Rows.Count > 0) {
                compareTime = false;
            }
            //取保存表中的数据
            if (compareTime)
            {
                result.Rows.Add(save.Rows[0].ItemArray);
            }
            else { 
                //取预报
                if (fore.Rows.Count > 0)
                {
                    result.Rows.Add(fore.Rows[0].ItemArray);
                }
                else {
                    result.Rows.Add(name, code,name2, 999.9, -1);
                }
            }
            return result;

        }

        //获取要素信息
        //target=index则说明是点击要素所需要的指数，否则则是点击指数所需要的要素名称
        public string GetFeatureInfo(string code,string target,string site,string lst) {
            m_database = new Database("DBCONFIG");
            IndexCalculate indexCal = new IndexCalculate(m_database);
            DateTime LST = DateTime.Parse(lst);
            string html = "";
            DataTable dt = new DataTable();
            if (target == "index")
            {
                dt = indexCal.Ele_SelectIndex(code);
            }
            else {
                dt = indexCal.Index_SelectEle(LST, site, code);
            }
            if (dt != null && dt.Rows.Count > 0) {
                for (int i = 0; i < dt.Rows.Count; i++) {
                    if (target == "index")
                    {
                        html += "<tr><td class=cl>" + (i + 1) + "</td><td class=cl>" + dt.Rows[i][0] + "</td><td class=cl>" + dt.Rows[i][1] + "</td></tr>";
                    }
                    else
                    {
                        html += "<tr><td class=cl>" + (i + 1) + "</td><td class=cl>" + dt.Rows[i][0] + "</td><td class=cl>" + dt.Rows[i][1] + "</td><td class=cl>" + dt.Rows[i][2] + "</td></tr>";
                    }
                }
            }
            return html;
        }

        //点击“+”号或双击行获取展开的html
        public string GetForeHtml(string code) {
            m_database = new Database("DBCONFIG");
            string html = "";
            string sql = "SELECT LEVELNAME,TPARAMS,RVALUE,TIPINFO FROM Weather_Level WHERE INDEXCODE='"+code+"'";
            DataTable dt = m_database.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0) {
                for (int i = 0; i < dt.Rows.Count; i++) { 
                    
                }
            }
            return html;
        }

        //获取指数订正页面行展开后下拉菜单的等级
        public string GetTabGradeData(string code)
        {
            string mainLevel = "1";
            string[] arr_code = { "tgzs", "xjktzs" };     //其他指数的主级别没有按月份的，只有这两个指数的主级别根据月份显示
            m_database = new Database("DBCONFIG");
            DateTime dNow = DateTime.Now;
            int month = dNow.Month;//分主级别，分季节显示
            if (code.IndexOf("tgzs") > -1)
            {
                if ((month >= 3 && month <= 5) || (month >= 9 && month <= 11))
                {   //春、秋
                    mainLevel = "3";
                }
                else if (month >= 6 && month <= 8)
                {   //夏季
                    mainLevel = "2";
                }
                else if (month >= 12 && month <= 2)
                {  //冬季
                    mainLevel = "1";
                }
            }
            else if (code.IndexOf("xjktzs") > -1)
            {
                if (month >= 3 && month <= 8)
                {
                    mainLevel = "1";    //春夏
                }
                else if (month >= 9 && month <= 2)
                {
                    mainLevel = "2";    //秋冬
                }
            }
            string sql = "SELECT levelValue FROM Weather_Level WHERE INDEXCODE='" + code + "' and MAINLEVEL='" + mainLevel + "'";
            DataTable dt = m_database.GetDataTable(sql);
            string html = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html += "<tr style='border-bottom:2px solid #99bbe8;padding-top:5px'><td id='" + dt.Rows[i][0] + "' class='y-bgPlus open'></td><td class='y-dj' month='" + mainLevel + "'>级别：" + dt.Rows[i][0] + "</td></tr>";
                    html += getTabGData(code, dt.Rows[i][0].ToString(), mainLevel);
                }
            }
            return html;
        }

        //获取指数订正页面行展开后下拉菜单等级的详细信息
        public string getTabGData(string code,string grade,string mainLevel) {
            m_database = new Database("DBCONFIG");
            string html = "<tr class='tab_content' style='border-bottom:1px solid #ddd;border-right:1px solid #ddd;'><td style='width:100%;'><div class='dj' onclick='changeColor(this,3)' style='padding-top:8px'>";    //最终生成的文本用tr和td标签包裹，要不然在前台嵌入到table中不起作用（后来修改的）
            string sql = "SELECT MEANNAME,LEVELNAME,RTIP,TIPINFO,RVALUE,TPARAMS FROM Weather_Level WHERE INDEXCODE='" + code + "' AND LEVELVALUE='" + grade + "' AND MAINLEVEL='"+mainLevel+"'";
            DataTable DT = m_database.GetDataTable(sql);
            if (DT != null && DT.Rows.Count > 0) {
                for (int i = 0; i < DT.Rows.Count; i++) {
                    string mean = DT.Rows[i][0].ToString().Trim();  //含义
                    string briTip = DT.Rows[i][1].ToString().Trim();   //简短提示
                    string valueScope = DT.Rows[i][2].ToString().Trim();   //指数范围
                    string tip = DT.Rows[i][3].ToString().Trim();   //详细提示;
                    string value = DT.Rows[i][4].ToString().Trim();   //指数值，页面不显示，要隐藏
                    string reason = DT.Rows[i][5].ToString().Trim();   //原因，先放在标签中，用的时候分割，页面不显示，要隐藏
                    html += "<div class='change'style='height:25px;padding-left:16px;'><div class='fl drop-table-td oh mean'>" + mean + "</div>";
                    html += "<div class='fl drop-table-td oh britip'>" + briTip + "</div>";
                    html += "<div class='fl drop-table-td oh valueScope'>" + valueScope + "</div>";
                    html += "<div class='fl rad '><img src='images/selected.png' onmouseenter='changeColor(this,1)' onmouseleave='changeColor(this,2)' onclick='getMValue(this)'/></div>";
                    html += "<span style='display:none' class='value'>" + value + "</span>";    //指数值，页面不显示，要隐藏,点击按钮添加到对应的框中
                    html += "<span style='display:none' class='reason'>" + reason + "</span></div>";    //指数值，页面不显示，要隐藏,点击按钮添加到对应的框中
                    html += "<div class='change' style='padding-left:10px;'><label style='padding-left:17px;font-weight:600;'>详细提示：</label>" + tip + "</div>";
                }
            }
            html += "</div></td></tr>";
            return html;
        }

        public string Save(string briefTip, string value,string tip,string LST,string code,string station,string forecaster,string levelValue,string meanName,string name,string status,string param)
        {
            m_database = new Database("DBCONFIG");
            briefTip = briefTip.Trim();    //防止有空格，去掉前后空格
            tip = tip.Trim();
            param = param.Trim();
            value = value.Trim();
            levelValue = levelValue.Trim();
            DateTime dNow = DateTime.Now;
            string hour = "";
            int h = dNow.Hour;
            if (h < 12)
            {
                hour = " 06:00:00";
            }
            else {
                hour = " 16:00:00";
            }
            LST = DateTime.Parse(LST).ToString("yyyy-MM-dd") + hour;
            string foreTime = dNow.ToString("yyyy-MM-dd") + hour;    //发布时间   LST预报时间
            //先删掉在插入
            string where = " where indexcode='"+code+"' and forecastDate='"+foreTime+"' and LST='"+LST+"' and forecaster='"+forecaster+"' and station='"+station+"'";
            string values = " ('" + forecaster + "','" + code + "','" + name + "','" + station + "','" + foreTime + "','" + LST + "','" + levelValue + "','" + meanName + "','" + briefTip + "','" + tip + "','" + value + "','" + param + "','" + status + "','" + dNow.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            string sql_del = "delete from T_Weather_Save"+where;
            string sql_insert = "insert into T_Weather_Save (forecaster,indexcode,name,station,forecastdate,lst,levelvalue,meanname,brieftip,tipinfo,rvalue,params,status,realtime) values" + values + "";
            try
            {
                m_database.Execute(sql_del);
                m_database.Execute(sql_insert);
                string lst = DateTime.Parse(LST).ToString("yyyy-MM-dd");
                string foreDate = DateTime.Parse(foreTime).ToString("yyyy-MM-dd");
                //string indexTip = GetWeather_IndexTip(lst,foreTime,code,station);
                return "ok";
            }
            catch (Exception e) {
                return "error";
            }
        }

        public string SaveAll(string forecaster, string code,string name,string indexVal,string levelVal,string indexMean,string shortTip,string longTip,string site,string LST) {
            m_database = new Database("DBCONFIG");
            string [] arr_code = code.TrimEnd('#').Split('#');
            string[] arr_name = name.TrimEnd('#').Split('#');
            string [] arr_indexVal=indexVal.TrimEnd('#').Split('#');
            string[] arr_levelVal = levelVal.TrimEnd('#').Split('#');
            string[] arr_indexMean = indexMean.TrimEnd('#').Split('#');
            string[] arr_shortTip = shortTip.TrimEnd('#').Split('#');
            string[] arr_longTip = longTip.TrimEnd('#').Split('#');
            DateTime dNow = DateTime.Now;
            string hour = "";
            int h = dNow.Hour;
            if (h < 12)
            {
                hour = " 06:00:00";
            }
            else
            {
                hour = " 16:00:00";
            }
            LST = DateTime.Parse(LST).ToString("yyyy-MM-dd")+hour;
            string foreTime = dNow.ToString("yyyy-MM-dd") + hour;
            string where = " where forecastDate='" + foreTime + "' and LST='" + LST + "' and forecaster='" + forecaster + "' and station='" + site + "'";
            string sql_insert = "insert into T_Weather_Save (forecaster,indexcode,name,station,forecastdate,lst,levelvalue,meanname,brieftip,tipinfo,rvalue,realtime) values";
            string sql_del = "delete from T_Weather_Save" + where;
            m_database.Execute(sql_del);
            string tip = "";
            for (int i = 0; i < arr_code.Length; i++)
            {
                try
                {
                    string value = " ('" + forecaster + "','" + arr_code[i] + "','" + arr_name[i] + "','" + site + "','" + foreTime + "','" + LST + "','" + arr_levelVal[i].Split('级')[0] + "','" + arr_indexMean[i] + "','" + arr_shortTip[i] + "','" + arr_longTip[i] + "','" + arr_indexVal[i] + "','" + dNow.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    m_database.Execute(sql_insert+value);
                }catch (Exception e){
                    tip = "error";
                }
            }
            return tip;
        }

        public string GetFactorTable(string site, string LST)
        {   //sql语句需要修改，加时间
            m_database = new Database("DBCONFIG");
            DateTime dNow = DateTime.Now;
            DataTable result = new DataTable();
            string forecastDate = dNow.ToString("yyyy-MM-dd");
            LST = DateTime.Parse(LST).ToString("yyyy-MM-dd");
            string html = " <tr class='tab-h'><td>要素编码</td><td>要素名称</td><td>要素值</td><td class='desc'>要素描述</td><td></td></tr>";
            string sql_basic = "select code,name,defvalue,descinfo,isedit,gname,gid from Weather_Ele order by gid,id asc";     //取基础表中的数据
            DataTable dt_basic = m_database.GetDataTable(sql_basic);
            string sql_fore = "";
            for (int i = 0; i < dt_basic.Rows.Count; i++)
            {
                string code = dt_basic.Rows[i]["code"].ToString();
                string foreWhere = " where a.ITEMID='" + code + "' and a.Site='" + site + "' and a.LST between '" + LST + " 00:00:00' and '" + LST + " 23:59:59'";
                sql_fore += "select a.itemid as code,b.name,a.value as defvalue,b.descinfo,b.isedit,b.gname,b.gid from Weather_EleData as a left join Weather_Ele as b on a.ITEMID=b.code" +
                    "" + foreWhere + " and ForecastDate = (select MAX(ForecastDate) from Weather_EleData as a " + foreWhere + ") union all ";   //取预报表中的数据
                //sql_fore += "select value from Weather_EleData as a " + foreWhere + " and ForecastDate=(select MAX(ForecastDate) from Weather_EleData as a" + foreWhere + " ) union all ";
            }
            sql_fore = sql_fore.TrimEnd("union all".ToCharArray());
            DataTable dt_fore = m_database.GetDataTable(sql_fore);
            html = FactorMer(dt_basic, dt_fore, html);
            return html;
            //return DataTableToJson("data",dt_fore);
        }

        //要素订正基础表和数据表的合并
        public string FactorMer(DataTable basic, DataTable info,string html)
        {
            DataTable result = info.Clone();
            int count = 0;
            if (basic != null && basic.Rows.Count > 0)
            {
                for (int i = 0; i < basic.Rows.Count; i++)
                {
                    string gname = basic.Rows[i]["gname"].ToString();
                    string code = basic.Rows[i][0].ToString();
                    string name = basic.Rows[i]["name"].ToString();
                    string gid = basic.Rows[i]["gid"].ToString();
                    string desc = basic.Rows[i]["descinfo"].ToString();
                    string isedit = basic.Rows[i]["isedit"].ToString();
                    string val = basic.Rows[i]["defvalue"].ToString();
                    val = val == "999.99"? "999.9" : val;
                    DataRow[] row = info.Select("code='" + code + "'");
                    if (row.Length > 0)
                    {
                        val = row[0]["defvalue"].ToString();
                        val = val == "" ? "999.9" : val;
                    }
                    if (i == 0)
                    {
                        html += "<tr class=parent><td class='wea open'colspan='5'><span class='disp'></span>" + gname + "</td></tr><tr class='cont'><td colspan='5'><table>";
                    }
                    if (i > 0 && (basic.Rows[i - 1]["gname"].ToString() != gname))
                    {
                        count = 0;
                        html += "<tr class=parent><td class='wea open'colspan='5'><span class='disp'></span>" + gname + "</td></tr><tr class='cont'><td colspan='5'><table>";
                    }
                    if (isedit == "0")
                    {
                        html += "<tr class='" + gid + " cur'><td class='info'>" + code + "</td><td class='info'>" + name + "</td><td class='info'><input type='text' class='input-txt'/><span class='span-txt'>" + val + "</span></td><td class='info desc'>" + desc + "</td><td class='info'></td></tr>";
                    }
                    else
                    {
                        html += "<tr class='" + gid + " cur'><td class='info'>" + code + "</td><td class='info'>" + name + "</td><td class='info'><input type='text' class='input-txt'/><span class='span-txt'>" + val + "</span></td><td class='info desc'>" + desc + "</td><td class='info'><img src='images\\saveIcon.png' title='保存'></td></tr>";
                    }
                    //本行与下一行比较，最后一行后面也要加
                    count++;
                    if ((i < basic.Rows.Count - 1 && (gname != basic.Rows[i + 1]["gname"].ToString())) || i == basic.Rows.Count - 1)
                    {
                        html += "</table></td></tr><tr><td class='total'colspan='5'>（" + count + "个要素）</td></tr>";
                    }
                }
            }

            return html;
        }
       
        //要素订正保存
        public void OnlySave(string txt,string LST,string fCode,string site) {
            m_database = new Database("DBCONFIG");
            IndexCalculate indexCal = new IndexCalculate(m_database);
            DateTime dNow = DateTime.Now;
            LST = DateTime.Parse(LST).ToString("yyyy-MM-dd");
            string foreTime = dNow.ToString("yyyy-MM-dd");
            string period = "", interval = "";
            TimeSpan day = DateTime.Parse(LST) - DateTime.Parse(foreTime);
            if (day.TotalDays == 0) {
                period = "24";
                interval = "0";
            }
            else if (day.TotalDays == 1) {
                period = "48";
                interval = "24";
            }
            else if (day.TotalDays == 0) {
                period = "72";
                interval = "48";
            }
            string where = " where PERIOD='"+period+"' and Site='"+site+"' and ITEMID='"+fCode+"' and lst between '"+LST+" 00:00:00' and '"+LST+" 23:00:00'";
            //如果Weather_EleData表中没有这个条件下的itemID，则需要插入
            string sql_query = "select * from Weather_EleData " + where + " and ForecastDate=(select MAX(ForecastDate) from Weather_EleData "+where+")";
            DataTable dt_query = m_database.GetDataTable(sql_query);
            if (dt_query.Rows.Count > 0)
            {
                string sql_update = "update Weather_EleData set Module='manual',ForecastDate='" + foreTime + " " + dNow.Hour + ":00:00',Value='" + txt + "'" + where + " and ForecastDate=(select max(ForecastDate) from Weather_EleData " + where + ")";
                m_database.Execute(sql_update);
            }
            else {
                string sql_insert = "insert into Weather_EleData (LST,ForecastDate,Interval,PERIOD,Site,ITEMID,Value,Module ) "+
                    "values ('" + LST + " 00:00:00','" + foreTime + " " + dNow.Hour + ":00:00','" + interval + "','" + period + "','" + site + "','" + fCode + "','" + txt + "','manual')";
                m_database.Execute(sql_insert);
            }
            string [] station={site};
            DateTime L=DateTime.Parse(LST+" 00:00:00");
            DateTime foreD = DateTime.Parse(foreTime + " " + dNow.Hour.ToString() + ":00:00");
            DataTable dtWeather_IndexResult = indexCal.Index_Calcu(foreD, L, station);
            //删除t_weather_save表中相对应得数据
            string [] arr_code={fCode};
            DelWeaSaveData(arr_code, site, LST, foreTime);
        }
         
        //删除订正后保存的数据
        public void DelWeaSaveData(string [] code,string station,string lst,string forecastDate) {
            m_database = new Database("DBCONFIG");
            IndexCalculate indexCal = new IndexCalculate(m_database);
            string indexCode = "";
            DateTime dNow = DateTime.Now;
            string hour = " 06:00:00";
            if (dNow.Hour >= 12)
            {
                hour = " 16:00:00";
            }
            foreach(string fcode in code){
                //调用接口获取与该要素有关的指数,然后从保存的表中删除这个要素（最大发布日期），要不然没效果
                DataTable dt = indexCal.Ele_SelectIndex(fcode);
                if (dt != null && dt.Rows.Count > 0) { 
                    foreach(DataRow row in dt.Rows){
                        indexCode = row[0].ToString();
                        string del_Wea_Save = "delete from T_Weather_Save where lst = '" + lst + " "+hour+"' and forecastdate = '" + forecastDate + " "+hour+"'" +
                                " and station='" + station + "'";
                        //string timeWhere = "lst between '" + lst + " 00:00:00' and '" + lst + " 23:59:59'";
                        //string sub_where = " where indexcode='" + indexCode + "'and station='" + station + "' and " + timeWhere + "";
                        //where = "" + sub_where + " and forecastDate=(select max(ForecastDate) from T_Weather_Save "+sub_where+")";
                        m_database.Execute(del_Wea_Save);
                    }
                }
            }
        }
        //首席检查
        public string Check(string code, string userName, string name, string operater)
        {
            //获取推送ftp的账户
            DataTable dt_ftp = GetFtpOption("","check",name);
            string ftpAddress = dt_ftp.Rows[0]["address"].ToString().Trim();
            //获取数据
            int num = 1;
            bool flag=true;
            int hour = DateTime.Now.Hour;
            string txt = "", str = "";
            if (hour < 15)
            {   //15点之前的要推送今明两天的数据
                while (flag)
                {
                    txt = GetUploadTxt(userName, ftpAddress, "check", num.ToString(),name);
                    //上传操作，调用UploadFtp（）
                    str += UploadFtp(ftpAddress, txt, "check", num.ToString(), name, operater);
                    num++;
                    if (num > 2)
                    {
                        flag = false;
                    }
                }
            }
            else   //16点只推送今天一天的
            {
                txt = GetUploadTxt(userName, ftpAddress, "check", num.ToString(),name);
                //上传操作，调用UploadFtp（）
                str = UploadFtp(ftpAddress, txt, "check", num.ToString(), name, operater);
            }
            return str;
        }

        //区域数据获取
        public string AreaData() {
            m_database = new Database("DBCONFIG");
            string sql = "SELECT stationCo,name,ISMAIN FROM Weather_Station ORDER BY name DESC";
            DataTable dt = m_database.GetDataTable(sql);
            return DataTableToJson("data",dt);
        }

        public void _Submit(string idd,string region,string orgName,string flag) {
            m_database = new Database("DBCONFIG");
            bool ismain;
            if (orgName == "是")
            {
                ismain = true;
            }
            else {
                ismain = false;
            }
            string sql = "UPDATE Weather_Station SET stationCo='" + idd + "',name='" + region + "' ,ISMAIN='" + ismain + "' WHERE stationCo='" + idd + "'";
            if (flag == "1") {
                sql = "INSERT INTO Weather_Station (stationCo,name,ISMAIN) VALUES ('" + idd + "','" + region + "','" + ismain + "')";
            }
            m_database.Execute(sql);
        }

        public void Del(string idd) {
            m_database = new Database("DBCONFIG");
            string del = "delete from Weather_Station where stationCo='" + idd + "'";
            m_database.Execute(del);
        }

        //获取站点ID
        public string GetSiteId(string accout) {
            m_database = new Database("DBCONFIG");
            string sql_user = "select POSTIONAREA from t_user where userName='" + accout + "'";
            DataTable dt_user = m_database.GetDataTable(sql_user);
            string site = dt_user.Rows[0][0].ToString().Substring(0,2);
            if (site == "中心") {
                site = "徐家汇";
            }
            string sql_siteId = "select stationCo,name from Weather_Station where name like '%" + site + "%'";
            DataTable dt_siteId = m_database.GetDataTable(sql_siteId);
            string siteId=dt_siteId.Rows[0][0].ToString();
            site = dt_siteId.Rows[0][1].ToString();
            //其他地方需要站点，因此需要拼接一下
            return siteId+"#"+site;
        }

        //计算字符串的MD5值
        public string GetMd5Hash(string input)
        {
            if (input == null)
            {
                return null;
            }

            MD5 md5Hash = MD5.Create();

            // 将输入字符串转换为字节数组并计算哈希数据  
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 创建一个 Stringbuilder 来收集字节并创建字符串  
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回十六进制字符串  
            return sBuilder.ToString();
        }

        public string SetFileName(string type,string fileName,string direct,string fileCreateTime) {
            DateTime dNow = DateTime.Now;
            //计算文件的MD5的值
            string filePath = direct+"\\"+fileName;
            FileStream file=new FileStream(filePath,FileMode.Open);
            System.Security.Cryptography.MD5 md5=new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte [] retVal=md5.ComputeHash(file);
            file.Close();
            StringBuilder sb=new StringBuilder();
            for(int i=0;i<retVal.Length;i++){
                sb.Append(retVal[i].ToString("x2"));
            }
            string strSysID = ConfigurationManager.AppSettings["MsgSysID"];
            //中间生成的文件名，用语计算最终的MD5码
            string strTempFileName = "SMS-" + "20012-" + fileCreateTime +"-" + strSysID + "-" + sb.ToString().ToUpper();
            string strFinalMD5 = GetMd5Hash(strTempFileName);
            string strFinalFileName = "SMS-" + "20012-" + fileCreateTime + "-" + strFinalMD5.ToUpper()+".txt";
            string oldFilePath = direct +"\\"+ fileName;
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.FileInfo file1 = new System.IO.FileInfo(oldFilePath);
                string finalFileName = direct +"\\"+ strFinalFileName;
                file1.MoveTo(finalFileName);
            }
            return strFinalFileName;
        }

        public string UploadFtp(string address, string txt, string type, string num, string name, string operater)
        {
            DateTime dNow = DateTime.Now;
            string ftpUploadTxt = ConfigurationManager.AppSettings["ftpUploadTxt"];
            if (type == "forecaster") {
                ftpUploadTxt = ConfigurationManager.AppSettings["FtpUploadTxtForecaster"];
            }
            string tip = "", fileCreateTime = "", fileName = "",explain="",module="指数上传",logDate="",status="成功";
            string date = dNow.ToString("yyyyMMdd");
            int h = dNow.Hour;
            try
            {
               
                //获取上传FTP的相关参数
                DataTable dt_ftp = GetFtpOption(address, type,name);
                string direct = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ftpUploadTxt);   //创建在本地项目上的文件夹
                if (dt_ftp != null && dt_ftp.Rows.Count > 0)
                {
                    explain = dt_ftp.Rows[0]["name"].ToString();
                    logDate = dNow.ToString("yyyy-MM-dd HH:mm:ss");
                    if (explain == "首席") {
                        module = "首席检查";
                        explain = "首席上传";
                    }
                    else if (explain == "短信") {
                        explain = "短信上传";
                        module = "指数订正";
                    }
                    if (type == "forecaster")
                    {
                        txt = txt.Replace("\n", System.Environment.NewLine);   //预报员发布的内容是从前台传过来的，换行符发生了变化，其他的是数据库读的
                        fileName = dt_ftp.Rows[0]["name"].ToString().Trim().Split('：')[1];    //上传到FTP上对应的文件名称
                        if (fileName.IndexOf("HX") >= 0) {   //火险指数下午发送时名称日期要变成第二天的，其他指数的用时效区分了
                            if (h >= 12) {
                                date = dNow.AddDays(1).ToString("yyyyMMdd");
                            }
                        }
                        fileName = fileName.Replace("yyyymmdd", date);
                        
                    }
                    if (type == "check")
                    {
                        List<string> list = GetLST_ForeTime(num);
                        string period = list[2].ToString();
                        string p = "";
                        if (period == "0") {
                            p = "24";
                        }
                        else if (period == "24") {
                            p = "48";
                        }
                        if (h >= 15)
                        {
                            period = "24";
                            p = "24";
                        }
                        fileName = "index_" + DateTime.Parse(list[0]).ToString("yyyyMMdd") + list[3] + p + ".txt";
                    }
                    else if(type=="message"){
                        fileCreateTime = dNow.ToString("yyyyMMddHHmmss");
                        fileName = "SMS" + "-" + "20012" + "-" + fileCreateTime + ".txt";
                    }
                    string userName = dt_ftp.Rows[0]["account"].ToString().Trim();  //用户名账户
                    string port = dt_ftp.Rows[0]["port"].ToString().Trim();   //端口
                    string password = dt_ftp.Rows[0]["password"].ToString().Trim();
                    string catalog = dt_ftp.Rows[0]["catalog"].ToString().Trim();   //上传文本在服务器上的目录
                    if (catalog.Substring(0, 1) == "/" && catalog!="")
                    {
                        catalog = catalog.Remove(0, 1);
                    }
                    string url = address + ":" + port;
                    string urlPath = url + "/" + catalog;
                    //先把文本保存到本地项目路径下
                    if (!Directory.Exists(direct))
                    {
                        Directory.CreateDirectory(direct);
                    }
                    string path = Path.Combine(direct, fileName);
                    StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("GB2312"));
                    sw.Write(txt);
                    sw.Flush();
                    sw.Close();
                    if (type == "message") {
                       // string fName = fileName.Split('.')[0].ToString();
                        string fName = fileName;
                        fileName = SetFileName(type, fName, direct, fileCreateTime);
                        path = Path.Combine(direct, fileName);
                    }
                    Ftp ftp = new Ftp(url,userName,password);
                    ftp.Upload( path, catalog + "/" + fileName);
                        //将本地保存的文本推到FTP上
                    //ftp.Connect(address, int.Parse(port), userName, password);
                    ////ftp.CreateDir("/" + catalog, "ftp://" + url, userName, password);
                    //ftp.Files.Upload("/" + catalog + "/" + fileName, path);
                    //while (!ftp.Files.UploadComplete)
                    //{ }
                    //ftp.Disconnect();
                    ftp = null;
                    tip = "success";
                }
            }catch(Exception e){
                status = "失败";
                tip = e.Message;
            }
            if (num != "2")
            {
                WriteLog(status, operater, logDate, module, explain);
            } return tip;
        }

        //写入日志
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="status">是否成功</param>
        /// <param name="operater">操作人</param>
        /// <param name="date">日期</param>
        /// <param name="module">模块</param>
        /// <param name="explain">说明</param>
        public void WriteLog(string status,string operater,string date,string module,string explain) {
            string sql = "insert into T_weather_LiveLog (Operater,OptDate,Module,Explain,Status) values ('"+operater+"','"+date+"','"+module+"','"+explain+"','"+status+"')";
            m_database.Execute(sql);
        }
        //num表示第二次发送   
        //首席检查获取相应的时间、时效
        public List<string> GetLST_ForeTime(string num) {
            List<string> list = new List<string>();
            DateTime dNow = DateTime.Now;
            string hour = "";
            int h = dNow.Hour;
            string periods = "0";
            string forecastDate = dNow.ToString("yyyy-MM-dd");
            string LST = dNow.ToString("yyyy-MM-dd");
            if (h >= 15) {
                LST = dNow.AddDays(1).ToString("yyyy-MM-dd");
            }
            if (num == "2") {
                LST = DateTime.Parse(LST).AddDays(1).ToString("yyyy-MM-dd");
            }
            TimeSpan day = DateTime.Parse(LST) - DateTime.Parse(forecastDate);
            if (day.TotalDays == 1)
            {
                periods = "24";
            }
            else if (day.TotalDays == 2) {
                periods = "48";
            }
            if (h < 10) {
                hour = "07";
            }
            else if (h < 15) {
                hour = "11";
            }
            else if (h >= 15) {
                hour = "16";
            }
            list.Add(forecastDate);
            list.Add(LST);
            list.Add(periods);
            list.Add(hour);
            return list;
        }

        //获取指数上传到服务器所需的文本
        public string GetUploadTxt(string userAccount,string address,string type,string num,string name_ftp)
        {
            m_database = new Database("DBCONFIG");
            DateTime dNow = DateTime.Now;
            int h;
            string period = "0", _status="",LST="" ;
            if (type.Split('#').Length > 1) {   //大于1说明是指数上传页面，第二个值在合并DataTable是使用，如果等于upload DataTable为空则返回空，其他的要与基础要素连在一起
                string [] _type = type.Split('#');
                _status = _type[1].ToString();
                type = _type[0].ToString();
            }
            //根据当前用户登录获取该用户相对应的站点及编号
            string [] str = GetSiteId(userAccount).Split('#');
            string siteId = str[0].ToString().Trim();
            string site = str[1].ToString().Trim();
            /////////////////////////////  预报员相关时间处理 //////////////////
            h = dNow.Hour;
            string forecastData = dNow.ToString("yyyy-MM-dd");
            LST = dNow.ToString("yyyy-MM-dd");
            if (h >= 12)
            {
                LST = dNow.AddDays(1).ToString("yyyy-MM-dd");
            }
            ///////////////////////////////////////// 预报员结束
            #region   首席
            if (type == "check") {    //首席和预报员不同
                List<string> list = GetLST_ForeTime(num);
                forecastData = list[0].ToString();
                LST = list[1].ToString();
                period = list[2].ToString();
            }
            #endregion
            #region    短信文本的获取，一天就一次
            if (type=="message"){
                forecastData = dNow.ToString("yyyy-MM-dd");
                LST = dNow.ToString("yyyy-MM-dd");
            }
            #endregion
            TimeSpan day = DateTime.Parse(LST) - DateTime.Parse(forecastData);
            if (day.TotalDays == 1)
            {
                period = "24";
            }
            else if (day.TotalDays == 2)
            {
                period = "48";
            }
            //基础数据获取
            string sql = "select name,code,name as mc from Weather_Index order by mc asc";
            DataTable dt_basic = m_database.GetDataTable(sql);
            DataTable result = DataProcess(dt_basic, siteId, LST);
            if (result != null && result.Rows.Count > 0)
            {
                //获取上传文本模板
                string temp = GetFtpOption(address, type, name_ftp).Rows[0]["sample"].ToString();
                if (type == "message")
                {
                    //type=“message”在数据库中的模板包括了短信文本和短信发送接口文本两部分，用@分割的
                    temp = temp.Split('@')[0].ToString();
                    if (dNow.Month < 6 || dNow.Month > 8)
                    {
                        temp = temp.Replace("中暑（上午mean:{zszs_sw}，下午mean:{zszs_xw}）", "干燥（mean:{gzzs}）");
                    }
                }
                temp = ProTemp(temp, LST, site, result,type);
                return temp;
            }
            else {
                return "";
            }
        }

        //模板处理
        public string ProTemp(string temp,string LST,string site,DataTable result,string type) {
            string code, name, value, shortTip, longTip, grade, meanName;
            //模板处理
            temp = temp.Replace("{title}", "气象生活指数预报");
            temp = temp.Replace("{yyyy}", DateTime.Parse(LST).ToString("yyyy"));
            temp = temp.Replace("{MM}", DateTime.Parse(LST).ToString("MM"));
            temp = temp.Replace("{dd}", DateTime.Parse(LST).ToString("dd"));
            temp = temp.Replace("{hh}","05");
            temp = temp.Replace("{site}", site);
            int num = 0;
            string[] str = { "人体舒适度", "锻炼", "洗晒", "日照", "干燥"};
            foreach (DataRow row in result.Rows)
            {
                code = row["code"].ToString().Trim();
                name = row["name"].ToString().Trim();
                value = row["Value"].ToString().Trim();
                grade = row["Grade"].ToString().Trim() + "级";
                shortTip = row["levelName"].ToString().Trim() == "" ? "--" : row["levelName"].ToString().Trim();
                longTip = row["tipInfo"].ToString().Trim() == "" ? "--" : row["tipInfo"].ToString().Trim(); ;
                meanName = row["meanName"].ToString().Trim() == "" ? "--" : row["meanName"].ToString().Trim();
                temp = temp.Replace("mc:{" + code + "}", name);
                temp = temp.Replace("jb:{" + code + "}", grade);
                temp = temp.Replace("stip:{" + code + "}", shortTip);
                temp = temp.Replace("tip:{" + code + "}", longTip);
                temp = temp.Replace("val:{" + code + "}", value);
                if (type == "message") {
                    meanName = meanName.Replace("洗晒","");
                }
                temp = temp.Replace("mean:{" + code + "}", meanName);
            }
            return temp;
        }
        //获取指数上传相关参数
        public DataTable GetFtpOption(string address,string type,string name) {
            m_database = new Database("DBCONFIG");
            string sql = "select address,account,port,password,catalog,sample,name from T_Weather_Ftp where address='" + address + "' and type='"+type+"' and name='"+name+"'";
            if (address == "") {
                sql = "select address,account,port,password,catalog,sample,name from T_Weather_Ftp where type='" + type + "' and name='" + name + "'";
            }
            return m_database.GetDataTable(sql);
        }
        //获取指数预报数据
        public DataTable GetIndexForeTab(string fore_where,string code)
        {
            m_database = new Database("DBCONFIG");
            string sql_fore = "select c.name as name1,a.code, c.name,a.Value,a.Grade,B.meanName,b.levelName,b.tipInfo,b.tparams,a.ForecastDate from Weather_IndexResult as A"+
                " left join Weather_Level AS B on A.code =B.indexCode left join Weather_Index as C on a.code=c.code " + fore_where + " and a.mainlevel=B.mainlevel"+
                " and a.Grade=B.levelValue and A.ForecastDate=(select MAX(ForecastDate) from Weather_IndexResult as a " + fore_where + ")";
            if (code.IndexOf("rtssd") >= 0 || code.IndexOf("gmzs") >= 0)
            {
                //人体指数、感冒
                sql_fore = "select c.name as name1,a.code, c.name,a.Value,a.Grade,d.mainTip,d.shortTip,d.TipInfo,c.name,a.ForecastDate from Weather_IndexResult as A " +
                    "left join Weather_Index as C on a.code=c.code left join Weather_IndexTip as d "+
                    " on d.code=a.code  " + fore_where + " and d.ForecastDate=" +
                    "(select MAX(ForecastDate) from Weather_IndexResult as a " + fore_where + ") and  A.ForecastDate=d.ForecastDate and a.station=d.station and a.lst=d.lst";
            }
            DataTable dt = m_database.GetDataTable(sql_fore);
            return dt;
        }
        //下拉选择ftp的上传文本的名称以及地址
        public string GetSelFtp(string type) {
            m_database = new Database("DBCONFIG");
            string sql = "select name,address from t_weather_ftp where type='"+type+"'";
            DataTable dt = m_database.GetDataTable(sql);
            return DataTableToJson("data",dt); ;
        }

        //短信管理界面获取短信用户信息
        public string GetMessUser()
        {//["userName", "phone", "age", "gender", "education", "company", "occupation", "postCode", "address", "phoneStatus"];
            m_database = new Database("DBCONFIG");
            string sql = "select id,userName,iphone,age,gender,edu,company,occupation,postcode,address,IsUse from dbo.T_Weather_Message order by Time desc";
            DataTable dt = m_database.GetDataTable(sql);
            return DataTableToJson("data",dt);
        }

        //获取短信预报文本
        public string GetMessTxt(string type,string userName) {
            DataTable dt_ftp = GetFtpOption("",type,"短信");
            string ftpAddress=dt_ftp.Rows[0][0].ToString();
            string txt = GetUploadTxt(userName, ftpAddress, type, "1","短信");
            return txt;
        }


        //点击发送把短信文本推送到ftp上
        public string MessTxtUpload(string type, string txt, string num, string name, string operater)
        {
            m_database = new Database("DBCONFIG");
            //获取用户号码
            string phone = "";
            string sql = "select iphone from T_Weather_Message where IsUse='1' order by iphone desc";
            DataTable dt_mess = m_database.GetDataTable(sql);
            DataTable dt_ftp = GetFtpOption("", type,name);
            string ftpAddress = dt_ftp.Rows[0][0].ToString();
            string uploadTmp = dt_ftp.Rows[0][5].ToString().Split('@')[1];   //上传文本模板
            string uploadTxt = uploadTmp.Replace("{value}", txt);
            foreach(DataRow row in dt_mess.Rows){
                string str = row[0].ToString();
                phone += str + System.Environment.NewLine;
            }
            phone = phone.TrimEnd((char[])System.Environment.NewLine.ToCharArray());
            uploadTxt = uploadTxt.Replace("{phone}", phone);
            string tip = UploadFtp(ftpAddress, uploadTxt, type, "1", name, operater);
            return tip;
        }

        ////获取ftp接口数据
        public string GetPreviewData()
        {
            try
            {
                string myConn = ConfigurationManager.AppSettings["CenterPreviewDataFtp"].ToString();
                string ftpUploadTxt = ConfigurationManager.AppSettings["ftpUploadTxt"];
                string[] arr = myConn.Split(';');
                //string url = arr[0].ToString().Split('/')[0].ToString();
                string ftpPathURL = arr[0].ToString();
                string userName = arr[1].ToString();
                string password = arr[2].ToString();
                string fileName1 = "gbg.txt";
                string fileName2 = "tenday.txt";
                string localDirect = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ftpUploadTxt);
                FtpLib.FtpWeb ftp = new FtpLib.FtpWeb(ftpPathURL, userName, password);
                ftp.Download(localDirect, fileName1);
                ftp.Download(localDirect, fileName2);
                //CallWithTimeout(localDirect, fileName1,fileName2, ftp.Download, 8000);
                ftp = null;
                string gbgTxt = ReadTxtFile(Path.Combine(localDirect, fileName1));
                string tenDayTxt = ReadTxtFile(Path.Combine(localDirect, fileName2));
                string resultTxt = (gbgTxt + "#" + tenDayTxt).Replace("\0", "");
                return resultTxt;
            }
            catch (Exception e) {
                return "error";
            }
        }

        public string ReadTxtFile(string path) {
            //path = "D:\\业务\\SMS-20012-20171128165229-D34909433F61A213CF4CF8AEF1ECC686.txt";
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("GB2312")))
            {
                try
                {
                    string txt = sr.ReadToEnd();
                    sr.Close();
                    return txt;
                }
                catch (IOException e) {
                    return e.Message;
                }
            }
        }

        public delegate void FtpDownload(string localDirect, string fileName);
        public void CallWithTimeout(string localDirect, string fileName1, string fileName2, FtpDownload action, int timeoutMilliseconds)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                action(localDirect, fileName1);
                action(localDirect, fileName2);
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                throw new TimeoutException();
            }
        }

        //单个指数的重新计算
        public string One_Cal(string lst,string siteId,string indexName,string indexCode) {
            try
            {
                m_database = new Database("DBCONFIG");
                IndexCalculate indexCal = new IndexCalculate(m_database);
                DateTime dNow = DateTime.Now;
                string publishTime = dNow.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime pubDate = DateTime.Parse(publishTime);
                DateTime LST = DateTime.Parse(lst);
                string[] siteCode = { siteId };
                //删除该指数保存的数据
                string hour = " 06:00:00";
                if (dNow.Hour > 12) {
                    hour = " 16:00:00";
                }
                string delWhere = " where indexCode='" + indexCode + "' and Station='" + siteId + "' and LST = '"+LST.ToString("yyyy-MM-dd")+hour+"'";
                string del = "delete from T_Weather_Save" + delWhere + "and ForecastDate='"+dNow.ToString("yyyy-MM-dd")+hour+"'";
                m_database.Execute(del);
                indexCal.Index_CalOne(pubDate, LST, siteCode, indexName);
                //string tip = GetAfterOneCal(LST.ToString("yyyy-MM-dd"), publishTime, siteId, indexCode);
                //取数据
                string foreWhere = " where a.station='" + siteId + "' and a.lst between '" + LST.ToString("yyyy-MM-dd") + " 00:00:00' and '" + LST.ToString("yyyy-MM-dd") + " 23:59:59' and a.code='" + indexCode + "'";
                DataTable dt_fore = GetIndexForeTab(foreWhere, indexCode);
                string tip = DataTableToJson("data", dt_fore);
                return "ok#"+tip;
            }catch(Exception e){
                return e.Message;
            }
        }

        //获取日志表格
        public string GetLogTable(string startTime, string endTime, string people, string fun, string status)
        {
            m_database = new Database("DBCONFIG");
            status = status == "全部" ? "1=1" : "status='" + status + "'";
            people = people == "全部" ? "1=1" : "Operater='"+people+"'";
            fun = fun == "全部" ? "1=1" : "Module='"+fun+"'";
            string sql = "select Operater,OptDate,Module,Explain,Status from T_weather_LiveLog where " + people + " and " + fun + " and "+
                "OptDate between '" + startTime + "' and '" + endTime + "' and "+status+" order by OptDate desc";
            DataTable dt = m_database.GetDataTable(sql);
            return DataTableToJson("data",dt);
        }
    }
}
