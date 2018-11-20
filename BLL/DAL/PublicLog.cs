using System;
using System.Collections.Generic;
using System.Text;
using Readearth.Data;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using MMShareBLL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using ChinaAQI;
namespace MMShareBLL.DAL
{

    public class PublicLog
    {
        private Database m_Database;
        private Database m_DatabaseS;
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ComForecast comForecast = new ComForecast();
        public PublicLog()
        {
            m_Database = new Database();
            m_DatabaseS = new Database("SEMCDMC");
        }
        public string PublicLogQuery(string fromDate, string toDate, string publicStyle, string selectSource)
        {
            DateTime dtFrom = DateTime.Parse(fromDate);
            DateTime dtTo = DateTime.Parse(toDate).AddDays(1);
            string strSQL = "";
            if (publicStyle == "所有" && selectSource != "所有")
                strSQL = string.Format("SELECT DateTime,dataResource,PublicResource,PublicStyle,contentMes,LoginName,Message,Recount FROM T_SendLog WHERE DateTime BETWEEN '{0}' AND '{1}' AND PublicResource='{2}' ORDER BY DateTime DESC", dtFrom, dtTo, selectSource);
            else if (publicStyle != "所有" && selectSource == "所有")
                strSQL = string.Format("SELECT DateTime,dataResource,PublicResource,PublicStyle,contentMes,LoginName,Message,Recount FROM T_SendLog WHERE DateTime BETWEEN '{0}' AND '{1}' AND publicStyle='{2}' ORDER BY DateTime DESC", dtFrom, dtTo, publicStyle);
            else if (publicStyle != "所有" && selectSource != "所有")
                strSQL = string.Format("SELECT DateTime,dataResource,PublicResource,PublicStyle,contentMes,LoginName,Message,Recount FROM T_SendLog WHERE DateTime BETWEEN '{0}' AND '{1}' AND publicStyle='{2}' AND PublicResource='{3}' ORDER BY DateTime DESC", dtFrom, dtTo, publicStyle, selectSource);
            else if (publicStyle == "所有" && selectSource == "所有")
                strSQL = string.Format("SELECT DateTime,dataResource,PublicResource,PublicStyle,contentMes,LoginName,Message,Recount FROM T_SendLog WHERE DateTime BETWEEN '{0}' AND '{1}' ORDER BY DateTime DESC", dtFrom, dtTo);
            DataTable dt = m_Database.GetDataTable(strSQL);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='LogDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0' style='table-layout: fixed'>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='tabletitle' style='width: 10%'>发布时间</td>");
            sb.AppendLine("<td class='tabletitle'>数据日期</td>");
            sb.AppendLine("<td class='tabletitle'>数据源</td>");
            sb.AppendLine("<td class='tabletitle'>发布方式</td>");
            sb.AppendLine("<td class='tabletitle'>发布内容</td>");
            sb.AppendLine("<td class='tabletitle'>用户名</td>");
            sb.AppendLine("<td class='tabletitle'>消息</td>");
            sb.AppendLine("<td class='tabletitle' style='width:5%'>发布次数</td>");
            sb.AppendLine("<td class='tabletitle' style='width: 5%'>重新发送</td>");
            sb.AppendLine("</tr>");
            int rowIndex = 0;
            foreach (DataRow dr in dt.Rows)
            {
                rowIndex++;
                sb.AppendLine(string.Format("<tr  id='{0}'>", rowIndex));
                for (int i = 0; i < dt.Columns.Count - 1; i++)
                {
                    if (i == 4)

                        sb.AppendLine(string.Format("<td class='Logtablerow' id='{1}' onmouseover=\"mouseOver(this,'{2}',event)\" onmouseout=\"mouseOut(this,'{2}')\" >{0}</td>", dr[i].ToString(), rowIndex.ToString() + "4", dr[4].ToString()));
                    else
                        sb.AppendLine(string.Format("<td class='Logtablerow' onmouseover=\"mouseOverS(this,'{1}',event)\" onmouseout=\"mouseOutS(this,'{1}')\">{0}</td>", dr[i].ToString(), ""));
                }
                sb.AppendLine(string.Format("<td class='Logtablerow' style='width:5%'>{0}</td>", dr[dt.Columns.Count - 1].ToString()));
                sb.AppendLine(string.Format("<td class='Logtablerow' style='width: 5%'><a href=\"javascript:reSentMes('{0}','{1}');\"><img src='images/send.gif'/></a></td>", dr[0].ToString(), dr[3].ToString()));
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");

            return sb.ToString();

        }

        public string PublicLogExistOrNot(string forecastDate)
        {
            DateTime fromTime = DateTime.Parse(forecastDate);
            DateTime toTime = DateTime.Parse(forecastDate).AddDays(1);
            string strSQL = "";
            strSQL = "SELECT DateTime,PublicStyle FROM T_SendLog WHERE DateTime BETWEEN '" + fromTime + "' AND '" + toTime + "' AND Message='发送成功'";
            DataTable dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count > 0)
                return "1";
            else
                return "2";

        }

        public string reSend(string sendLogTime, string sendLogStyle)
        {
            string strSQL = "";
            string returnStr = "";
            strSQL = "SELECT DateTime,PublicStyle,PublicResource,Message,contentMes,Address,Recount FROM T_SendLog WHERE DateTime='" + sendLogTime + "' AND  PublicStyle='" + sendLogStyle + "'";
            DataTable dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                string style = dt.Rows[0][1].ToString();
                try
                {
                    switch (style)
                    {
                        case "移动短信":
                            returnStr = comForecast.SendSM(dt.Rows[0][4].ToString(), sendLogTime, dt.Rows[0][5].ToString(), "", (int.Parse(dt.Rows[0][6].ToString()) + 1).ToString(), dt.Rows[0][2].ToString());
                            break;
                        case "电信联通":
                            returnStr = comForecast.SendSMDX(dt.Rows[0][4].ToString(), sendLogTime, dt.Rows[0][5].ToString(), "", (int.Parse(dt.Rows[0][6].ToString()) + 1).ToString(), dt.Rows[0][2].ToString());
                            break;
                        case "宣教中心":
                            returnStr = comForecast.XJZX(sendLogTime, "", (int.Parse(dt.Rows[0][6].ToString()) + 1).ToString(), dt.Rows[0][4].ToString(), dt.Rows[0][2].ToString());
                            break;
                        case "市环保局":
                            returnStr = comForecast.SHBJ(sendLogTime, "", (int.Parse(dt.Rows[0][6].ToString()) + 1).ToString(), dt.Rows[0][4].ToString(), dt.Rows[0][2].ToString());
                            break;
                        case "新浪微博":
                            returnStr = comForecast.weiBo(dt.Rows[0][4].ToString(), sendLogTime, "", (int.Parse(dt.Rows[0][6].ToString()) + 1).ToString(), dt.Rows[0][2].ToString());
                            break;
                        case "腾讯微博":
                            returnStr = comForecast.TencentWeiBo(dt.Rows[0][4].ToString(), sendLogTime, "", (int.Parse(dt.Rows[0][6].ToString()) + 1).ToString(), dt.Rows[0][2].ToString());
                            break;
                        case "实时发布系统":
                            returnStr = comForecast.InsertDataBase(dt.Rows[0][4].ToString(), sendLogTime, "", (int.Parse(dt.Rows[0][6].ToString()) + 1).ToString(), dt.Rows[0][2].ToString());
                            break;
                    }
                }
                catch (Exception ex)
                {
                    returnStr = ex.Message;
                }
            }
            else
            {
                returnStr = "没有相应的记录";
            }
            return returnStr;

        }
        public string commentCotent()
        {
            string strSQL = "";
            DateTime startTime = DateTime.Now.Date;
            DateTime endTime = startTime.AddDays(1);
            strSQL = "SELECT CommentTime,Name,CommentContent,Folder,ImgName,Tag,ImgTime FROM T_DayComment WHERE CommentTime BETWEEN '" + startTime + "' AND '" + endTime + "'  ORDER BY CommentTime";
            DataTable dt = m_Database.GetDataTable(strSQL);
            int count = dt.Rows.Count;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class='titleName' id='titleName'>每日关注&nbsp&nbsp&nbsp<b style='font-family:宋体;font-size:14px;'>" + DateTime.Now.ToString("yyyy年MM月dd日") + "</b><input type='button'  onclick='closeComment()' class='close' onmouseover=\"this.className = 'closeHover';\" onmouseout =\"this.className ='close';\"   id='commentClose'></div>");
            if (dt.Rows.Count > 0)
            {

                sb.AppendLine("<div class='OutDiv' id='OutDiv' >");

                int index = 1;
                foreach (DataRow rows in dt.Rows)
                {
                    if (index != dt.Rows.Count)
                        sb.AppendLine("<div class='perData'>");
                    else
                        sb.AppendLine("<div class='perDataSec'>");
                    index++;
                    sb.AppendLine("<div class='commentContent'>");
                    sb.AppendLine("<div class='Image' ><img src='css/images/person.png' width='41px' height='41px' /></div>");
                    sb.AppendLine("<div class='commentText' >" + rows["CommentContent"] + "</div>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("<div class='ImagePicContent'><label>" + rows["ImgName"] + "</label><label>&nbsp&nbsp&nbsp" + rows["ImgTime"] + "</label></div>");
                    sb.AppendLine("<div class='imageContent'>");
                    sb.AppendLine("<div class='ImagePic'><img src='" + rows["Folder"] + "'' width='170px' height='150px' /></div>");
                    sb.AppendFormat("<div class='car'><input type='button' id='btnSave' class='ImgCar' onmouseover=\"this.className='ImgCar car_over'\" onmouseout=\"this.className='ImgCar car_normal'\" onmousedown=\"this.className='ImgCar car_Down'\" onmouseup =\"this.className='ImgCar car_normal'\"  onclick=\"addCart('{0}','{1}','{2}')\"/></div>", rows["CommentTime"], rows["Name"], rows["CommentContent"]);
                    sb.AppendFormat("<div class='car'><input type='button' id='btnSave' class='ImgCollect' onmouseover=\"this.className='ImgCollect ImgCollect_over'\" onmouseout=\"this.className='ImgCollect ImgCollect_normal'\" onmousedown=\"this.className='ImgCollect ImgCollect_Down'\" onmouseup =\"this.className='ImgCollect ImgCollect_normal'\"  onclick=\"addCollect('{0}','{1}')\"/></div>", rows["CommentTime"], rows["Name"]);
                    sb.AppendLine("</div>");
                    sb.AppendLine("<div class='commentTime'>" + DateTime.Parse(rows["CommentTime"].ToString()).ToString("yyyy-MM-dd HH:mm") + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp;来自<b style='color:#3399FF'>" + rows["Name"] + "</b></div>");
                    sb.AppendLine("</div>");
                }
                sb.AppendLine("</div>");

            }
            sb.AppendLine("<div class='fixComment'>");
            sb.AppendLine("<div class='Image' ><img src='css/images/hPerson.png' width='41px' height='41px' /></div>");
            sb.AppendLine("<div class='ImageInput'><div id='textContent' class='inputText' contenteditable='true' autocomplete='off' style='border: 1px solid #badbff;'></div><input type='button' class='commentBut'  onmouseover=\"this.className='commentBut commentButOver'\" onmouseout=\"this.className='commentBut'\" onmousedown=\"this.className='commentBut commentButDown'\" onmouseup =\"this.className='commentBut'\"  id='buttonID' onclick='PublicComment()' id='commentPublicBut'></div>");
            sb.AppendLine("</div>");
            return sb.ToString();
        }
        public string SaveCommentCotent(string textValue, string commentPeople, string entityName, string folder, string Imgtime, string imgName)
        {
            string strSQL = "";
            strSQL = "SELECT Alias FROM T_User WHERE UserName='" + commentPeople + "'";
            string alias = m_Database.GetFirstValue(strSQL);
            string className = ClassValue(entityName);
            string moduleName = ModuleName(entityName);
            int flag = 0;
            int tag = 0;
            DateTime imgDateTime = DateTime.Parse(Imgtime);
            try
            {
                strSQL = "INSERT INTO T_DayComment(CommentTime,Name,CommentContent,Folder,ImgName,Tag,Type,ImgTime,Flag,ModuleName) VALUES('" + DateTime.Now + "','" + alias + "','" + textValue + "','" + folder + "','" + imgName + "','" + flag + "','" + className + "','" + imgDateTime + "','" + tag + "','" + moduleName + "')";
                m_Database.Execute(strSQL);
                return "成功";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string ModuleName(string entityName)
        {
            string strSQL = "SELECT ModuleName FROM T_ImageProduct_test WHERE EntityName='" + entityName + "'";
            string ClassName = m_Database.GetFirstValue(strSQL);
            string ModuleName = "";
            switch (ClassName)
            {
                case "airQuality":
                    ModuleName = "实时监测";
                    break;
                case "xsforcast":
                    ModuleName = "数值预报";
                    break;
                case "jgRadar":
                    ModuleName = "超级站";
                    break;
                case "innp":
                    ModuleName = "气象观测";
                    break;
            }
            return ModuleName;

        }
        public string ClassValue(string entityName)
        {
            string Value = "";
            string strSQL = "SELECT ModuleName FROM T_ImageProduct_test WHERE EntityName='" + entityName + "'";
            string ClassName = m_Database.GetFirstValue(strSQL);
            if (ClassName == "airQuality" || ClassName == "xsforcast" || ClassName == "jgRadar")
                Value = "污染类";
            if (ClassName == "innp")
                Value = "气象类";
            return Value;

        }
        public string AddcommentCart(string dateTime, string name, string content)
        {
            string strSQL = "";
            try
            {
                strSQL = "SELECT CommentTime,Name,CommentContent,Folder,ImgName,Tag,ImgTime FROM T_DayComment WHERE CommentTime='" + dateTime + "' AND Name='" + name + "' AND Tag=1  ORDER BY CommentTime DESC";
                SqlDataReader sqlRead = m_Database.GetDataReader(strSQL);
                if (sqlRead.HasRows)
                    return "该信息已经加入会商资料库！";
                else
                {
                    strSQL = @"UPDATE T_DayComment SET Tag=1 WHERE CommentTime='" + dateTime + "' AND Name='" + name + "'";
                    m_Database.Execute(strSQL);
                    return "成功";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string AddcommentCollect(string dateTime, string name)
        {
            string strSQL = "";
            try
            {
                strSQL = "SELECT CommentTime,Name,CommentContent,Folder,ImgName,Tag,ImgTime FROM T_DayComment WHERE CommentTime='" + dateTime + "' AND Name='" + name + "' AND Flag=1  ORDER BY CommentTime DESC";
                SqlDataReader sqlRead = m_Database.GetDataReader(strSQL);
                if (sqlRead.HasRows)
                    return "该信息已经加入收藏夹！";
                else
                {
                    strSQL = @"UPDATE T_DayComment SET Flag=1 WHERE CommentTime='" + dateTime + "' AND Name='" + name + "'";
                    m_Database.Execute(strSQL);
                    return "成功";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string QueryDayCollect(string dateTime)
        {
            string strSQL = "";
            DateTime startTime = DateTime.Parse(dateTime);
            DateTime endTime = startTime.AddDays(1);
            strSQL = "SELECT CommentTime,Name,CommentContent,Folder,ImgName,ImgTime,ModuleName FROM T_DayComment WHERE Tag=1  AND CommentTime BETWEEN '" + startTime + "' AND '" + endTime + "'  ORDER BY CommentTime";
            DataTable dt = m_Database.GetDataTable(strSQL);

            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                sb.AppendLine("<div class='WIframe'  id='WIframe'  runat='server'>");
                int row = 0;
                foreach (DataRow rows in dt.Rows)
                {
                    row++;
                    sb.AppendFormat(string.Format("<div class='commentText' id='{0}'>" + rows["CommentContent"] + "</div>", "text" + row.ToString()));
                    sb.AppendFormat(string.Format("<div class='commentName' id='{0}'>" + rows["ImgName"] + "&nbsp&nbsp&nbsp&nbsp" + rows["ImgTime"] + "</div>", "commentName" + row.ToString()));
                    sb.AppendFormat(string.Format("<div class='ImageFolder'><img id='{0}' src='" + rows["Folder"] + "'' width='80%' width='80%' class='Folder'/></div>", "Image" + row.ToString()));
                    sb.AppendFormat(string.Format("<div class='OtherText' id='{0}'>" + rows["CommentTime"].ToString() + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp;来自<b style='color:#3399FF'>" + rows["ModuleName"] + "</b></div>", "OtherText" + row.ToString()));
                }
                sb.AppendLine("</div>");
            }
            return sb.ToString();
        }
        public string QueryDayComment(string dateTime, string user)
        {
            string strSQL = "";
            string text = "";
            DateTime startTime = DateTime.Parse(dateTime);
            DateTime endTime = startTime.AddDays(1);
            strSQL = "SELECT CommentTime,Name,CommentContent,Folder,ImgName,ImgTime FROM T_DayComment WHERE Flag=1 AND Name='" + user + "'  AND CommentTime BETWEEN '" + startTime + "' AND '" + endTime + "'  ORDER BY CommentTime";
            DataTable dt = m_Database.GetDataTable(strSQL);

            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                sb.AppendLine("<div class='WIframe' >");
                int row = 0;
                foreach (DataRow rows in dt.Rows)
                {
                    sb.AppendLine("<div class='personImg'>");
                    sb.AppendLine("<div class='person' ><img src='css/images/person.png' width='41px' height='41px' /></div>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("<div class='imageDetail'>");
                    sb.AppendLine("<div class='ImageFolder'><img src='" + rows["Folder"] + "'' width='80%' width='80%' class='Folder'/>");
                    sb.AppendLine("<div class='ImgValue'>");
                    sb.AppendLine("<div class='imgText'>" + rows["ImgName"] + "</div>");
                    sb.AppendLine("<div class='imgTimetext'>" + rows["ImgTime"] + "</div>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("</div>");
                    string value = rows["CommentContent"].ToString();
                    if (value.Length > 57)
                    {
                        text = value.Substring(0, 57);
                        sb.AppendLine("<div class='commentText' id='" + row.ToString() + "'>" + text + "<a href=\"javascript:ExpandCommentContent('" + text + "','" + value + "','" + row.ToString() + "')\"><img src='css/images/gd_d.png' width='15px' height='15px'/></a></div>");
                    }
                    else
                    {
                        text = value;
                        sb.AppendLine("<div class='commentText' id='" + row.ToString() + "'>" + text + "</div>");
                    }
                    sb.AppendLine("<div class='OtherText'>" + rows["CommentTime"].ToString() + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp;来自<b style='color:#3399FF'>" + rows["Name"] + "</b></div>");
                    sb.AppendLine("</div>");
                    if (dt.Rows.Count - 1 != row)
                        sb.AppendLine("<div class='spltLine'></div>");
                    row++;
                }
                sb.AppendLine("</div>");

            }
            return sb.ToString();
        }
        public string commentLabelValue()
        {
            string strSQL = "";
            string count;
            DateTime startTime = DateTime.Now.Date;
            DateTime endTime = startTime.AddDays(1);
            strSQL = "SELECT COUNT(*) FROM T_DayComment WHERE Flag=1 AND CommentTime BETWEEN  '" + startTime + "' AND  '" + endTime + "'";
            count = m_Database.GetFirstValue(strSQL);
            return count;
        }
        public string divEvery(string value, string tag)
        {
            StringBuilder sb = new StringBuilder();
            if (tag == "0")
            {
                string[] webValue = value.Split('|');

                sb.AppendLine("<table id='changeDatable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
                sb.AppendLine("<tr>");

                //创建抬头
                sb.AppendLine("<td class='tableEditorNew'>时段</td>");
                sb.AppendLine("<td class='tableEditorNew'>空气质量</td>");
                sb.AppendLine("<td class='tableEditorNew'>首要污染物</td>");
                sb.AppendLine("<td class='tableEditorRightNew'>AQI</td>");
                sb.AppendLine("</tr>");
                for (int i = 0; i < 3; i++)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine(string.Format("<td class='tableRowEditorNew' >{0}</td>", webValue[i * 4 + 1]));
                    sb.AppendLine(string.Format("<td class='tableRowEditorNew' >{0}</td>", webValue[i * 4 + 3]));
                    sb.AppendLine(string.Format("<td class='tableRowEditorNew' >{0}</td>", webValue[i * 4 + 4]));
                    sb.AppendLine(string.Format("<td class='tableRowEditorRightNew' >{0}</td>", webValue[i * 4 + 2]));
                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</table>");
                sb.AppendLine(string.Format("<label id='lableEditor' class='lableStyleNew'>发布描述：</label><div class='tableSign' id='Detail'>{0}</div>", webValue[13]));
                sb.AppendLine(string.Format("<div class='tableSign' id='Sign'>{0}</div>", webValue[14]));
                sb.AppendLine(string.Format("<div class='tableSign' id='publishTimeChange'>{0}</div>", webValue[15]));



            }
            else
                sb.AppendLine(string.Format("<div>{0}</div>", value));
            return sb.ToString();
        }
        public string DateChinaTable(string dateTime)
        {
            string year = dateTime.Substring(0, 4);
            string month = dateTime.Substring(4, 2);
            string day = dateTime.Substring(6, 2);
            string imageDateTime = year + "-" + month + "-" + day;
            imageDateTime = DateTime.Parse(imageDateTime).ToString("yyyy/M/d 0:00:00");
            string strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_DATEAVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  China_RT_city_station.id ", imageDateTime);

            DataTable dt = m_DatabaseS.GetDataTable(strSQL);
            string returnStr = htmlJoson(dt, imageDateTime, "date");
            return returnStr;

        }


        public string chinaTable(string dateTime)
        {
            string year = dateTime.Substring(0, 4);
            string month = dateTime.Substring(4, 2);
            string day = dateTime.Substring(6, 2);
            string hour = dateTime.Substring(8, 2);
            string imageDateTime = year + "-" + month + "-" + day + " " + hour + ":00";
            string strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value DESC", imageDateTime);

            DataTable dt = m_DatabaseS.GetDataTable(strSQL);
            string returnStr = htmlJoson(dt, imageDateTime, "hour");
            return returnStr;
        }

        /// <summary>
        /// 返回上海站点浓度和AQI
        /// 张伟锋    2015-02-15
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string ShanghaiTable(string dateTime)
        {
            string year = dateTime.Substring(0, 4);
            string month = dateTime.Substring(4, 2);
            string day = dateTime.Substring(6, 2);
            string hour = dateTime.Substring(8, 2);
            string imageDateTime = year + "-" + month + "-" + day + " " + hour + ":00";
            //string strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value DESC", imageDateTime);
            string strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='PM2.5' and LST_AQI='{0}' order by AQI DESC", imageDateTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            string returnStr = htmlJoson(dt, imageDateTime, "SH");
            return returnStr;
        }

        public string ShanghaiRTO3Table(string dateTime)
        {
            string year = dateTime.Substring(0, 4);
            string month = dateTime.Substring(4, 2);
            string day = dateTime.Substring(6, 2);
            string hour = dateTime.Substring(8, 2);
            string imageDateTime = year + "-" + month + "-" + day + " " + hour + ":00";
            //string strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value DESC", imageDateTime);
            string strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='O3' and LST_AQI='{0}' order by AQI DESC", imageDateTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            string returnStr = htmlJoson(dt, imageDateTime, "SHO3");
            return returnStr;
        }

        public string ShanghaiRTPM10Table(string dateTime)
        {
            string year = dateTime.Substring(0, 4);
            string month = dateTime.Substring(4, 2);
            string day = dateTime.Substring(6, 2);
            string hour = dateTime.Substring(8, 2);
            string imageDateTime = year + "-" + month + "-" + day + " " + hour + ":00";
            //string strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value DESC", imageDateTime);
            string strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='PM10' and LST_AQI='{0}' order by AQI DESC", imageDateTime);
            DataTable dt = m_Database.GetDataTable(strSQL);
            string returnStr = htmlJoson(dt, imageDateTime, "SHPM10");
            return returnStr;
        }

        /// <summary>
        /// 创建华东table Pm2.5
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public string chinaTableHD(string dateTime)
        {
            string year = dateTime.Substring(0, 4);
            string month = dateTime.Substring(4, 2);
            string day = dateTime.Substring(6, 2);
            string hour = dateTime.Substring(8, 2);
            string imageDateTime = year + "-" + month + "-" + day + " " + hour + ":00";
            string strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value DESC", imageDateTime);

            DataTable dt = m_DatabaseS.GetDataTable(strSQL);
            string returnStr = htmlJoson(dt, imageDateTime, "hour");
            return returnStr;
        }


        public string PM10ChinaTable(string dateTime)
        {
            string year = dateTime.Substring(0, 4);
            string month = dateTime.Substring(4, 2);
            string day = dateTime.Substring(6, 2);
            string hour = dateTime.Substring(8, 2);
            string imageDateTime = year + "-" + month + "-" + day + " " + hour + ":00";
            //imageDateTime = "2014/5/27 15:00:00";
            string strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 2) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value DESC", imageDateTime);

            DataTable dt = m_DatabaseS.GetDataTable(strSQL);
            string returnStr = htmlJoson(dt, imageDateTime, "PM10Air");
            return returnStr;
        }
        private string color(string color)
        {
            switch (color)
            {
                case "green":
                    return "#00e400";
                case "yellow":
                    return "#FFFF00";
                case "orange":
                    return "#ffa500";
                case "red":
                    return "#ff0000";
                case "purple":
                    return "#800080";
                case "grayred":
                    return "#7e0023";
                default: return "";
            }
        }
        private string Fontcolor(string color)
        {
            if (color == "purple" || color == "grayred")
            {
                return "color: #FFFFFF;";
            }
            else
                return "";
        }
        /// <summary>
        /// 返回PM2.5曲线图
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public string chinaQuxian(string startDateTime, string endDateTime, string initStation)
        {
            //startDateTime = "2013/12/24 16:00:00";
            //endDateTime = "2013/12/25 16:00:00";
            string strSQL = "";
            string str = "";
            string stationID = initStation;
            string strWhere = " AND itemid=1  AND timepoint BETWEEN '" + startDateTime + "' AND  '" + endDateTime + "'";
            string[] chinaStationID = stationID.Split(',');
            for (int i = 0; i < chinaStationID.Length; i++)
            {
                //这里处理下  有市没有市一起查询
                string station = chinaStationID[i].ToString();
                if (station.IndexOf("市") < 0)
                    station = station + "市";

                strSQL = string.Format("SELECT  DATEDIFF(S,'1970-01-01 00:00:00',timepoint) AS timepoint,value from CHINA_RT_CITY_AVERAGE where (Area='{0}' or Area='{2}') {1} ORDER BY timepoint", chinaStationID[i], strWhere, station);
                str = str + strSQL + ";";

            }
            DataSet ds = m_DatabaseS.GetDataset(str);
            return CreatStringQuxian(ds);

        }
        public string chinaPM10Quxian(string startDateTime, string endDateTime, string initStation)
        {
            //startDateTime = "2014/5/21 04:00:00";
            //endDateTime = "2014/5/27 15:00:00";
            string strSQL = "";
            string str = "";
            string stationID = initStation;
            string strWhere = "  AND itemid=2 AND  timepoint BETWEEN '" + startDateTime + "' AND  '" + endDateTime + "'";
            string[] chinaStationID = stationID.Split(',');
            for (int i = 0; i < chinaStationID.Length; i++)
            {
                //这里处理下  有市没有市一起查询
                string station = chinaStationID[i].ToString();
                if (station.IndexOf("市") < 0)
                    station = station + "市";

                strSQL = string.Format("SELECT  DATEDIFF(S,'1970-01-01 00:00:00',timepoint) AS timepoint,value from CHINA_RT_CITY_AVERAGE where (Area='{0}' or Area='{2}') {1} ORDER BY timepoint", chinaStationID[i], strWhere, station);
                str = str + strSQL + ";";

            }
            DataSet ds = m_DatabaseS.GetDataset(str);
            return CreatStringQuxian(ds);
        }
        public string CreatStringQuxian(DataSet ds)
        {
            string x = "";
            string y = "";
            double minX = 1000000000000;
            string strReturn = "";
            for (int index = 0; index < ds.Tables.Count; index++)
            {
                DataTable dtElement = ds.Tables[index];
                x = ""; y = "";
                foreach (DataRow dr in dtElement.Rows)
                {
                    x = x + "|" + dr[0].ToString();
                    if (double.Parse(dr[0].ToString()) < minX)
                        minX = double.Parse(dr[0].ToString());
                    y = y + "|" + dr[1].ToString();
                }
                strReturn = strReturn + ",'" + index.ToString() + "':'" + x.TrimStart('|') + "*" + y.TrimStart('|') + "'";
            }
            if (strReturn != ",")
                strReturn = "{" + strReturn.TrimStart(',') + ",minX:" + minX.ToString() + "}";
            return strReturn;

        }
        /// <summary>
        /// 返回PM2.5曲线日报图
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public string DatechinaQuxian(string startDateTime, string endDateTime, string initStation)
        {
            startDateTime = DateTime.Parse(startDateTime).ToString("yyyy/M/d 0:00:00");
            endDateTime = DateTime.Parse(endDateTime).ToString("yyyy/M/d 0:00:00");
            string strSQL = "";
            string str = "";
            string stationID = initStation;
            string strWhere = " AND  timepoint BETWEEN '" + startDateTime + "' AND  '" + endDateTime + "'";
            string[] chinaStationID = stationID.Split(',');
            for (int i = 0; i < chinaStationID.Length; i++)
            {
                string station = chinaStationID[i].ToString();
                if (station.IndexOf("市") < 0)
                    station = station + "市";

                strSQL = string.Format("SELECT  DATEDIFF(S,'1970-01-01 00:00:00',timepoint) AS timepoint,value from CHINA_RT_CITY_DATEAVERAGE where (Area='{0}' or Area='{2}') {1} ORDER BY timepoint", chinaStationID[i], strWhere, station);
                str = str + strSQL + ";";

            }
            DataSet ds = m_DatabaseS.GetDataset(str);
            return CreatStringQuxian(ds);

        }
        public string chinaStationName(string startDateTime, string endDateTime, string checkStation, string entityName)
        {
            string strSQL = "";
            DataTable dt;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id='stationNameTable' border='0'>");
            strSQL = "SELECT Area,flag from CHINA_RT_CITY_STATION ORDER BY ID";
            dt = m_DatabaseS.GetDataTable(strSQL);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0 || i % 4 == 0)
                    sb.AppendLine("<tr>");
                if (checkStation.IndexOf(dt.Rows[i][0].ToString()) >= 0)
                    sb.AppendFormat("<td class='checkViewer'><input type='checkbox' name='CheckStation' checked='checked'  id='{1}' value ={0}><label for='{1}'>{0}</label></td>", dt.Rows[i][0], dt.Rows[i][1] + i.ToString());
                else
                    sb.AppendFormat("<td class='checkViewer'><input type='checkbox' name='CheckStation'  id='{1}'  value ={0}><label for='{1}'>{0}</label></td>", dt.Rows[i][0], dt.Rows[i][1] + i.ToString());
                if ((i + 1) % 4 == 0)
                    sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
        private string htmlJoson(DataTable dt, string dateTime, string flag)
        {
            StringBuilder sb = new StringBuilder();
            string dataTitle = "";
            sb.AppendLine("<input type='button'  onclick='closeTable()' class='close' onmouseover=\"this.className = 'closeHover';\" onmouseout =\"this.className ='close';\"   id='tableClose'>");
            if (flag == "hour" || flag == "SH")
            {
                sb.AppendLine(string.Format("<div id='title' class='tableTitleImage'>{0}</div>", "PM2.5污染物小时报"));
                dataTitle = dateTime;
                sb.AppendLine(string.Format("<div><input type='button'  onclick=\"ascTable('{0}','{1}')\" class='asc' title = '升序'><input type='button'  onclick=\"normalTable('{0}','{1}')\" class='normal' title = '恢复原序'><input type='button'  onclick=\"descTable('{0}','{1}')\" class='desc' title = '降序'><div id='title' class='tableTitletime'>{2}</div></div>", dateTime, flag, dataTitle));
            }
            else if (flag == "PM10Air" || flag == "SHPM10")//实时监测PM10
            {
                sb.AppendLine(string.Format("<div id='title' class='tableTitleImage'>{0}</div>", "PM10污染物小时报"));
                dataTitle = dateTime;
                sb.AppendLine(string.Format("<div><input type='button'  onclick=\"ascTable('{0}','{1}')\" class='asc' title = '升序'><input type='button'  onclick=\"normalTable('{0}','{1}')\" class='normal' title = '恢复原序'><input type='button'  onclick=\"descTable('{0}','{1}')\" class='desc' title = '降序'><div id='title' class='tableTitletime'>{2}</div></div>", dateTime, flag, dataTitle));
            }
            else if (flag == "SHO3")//实时监测O3小时
            {
                sb.AppendLine(string.Format("<div id='title' class='tableTitleImage'>{0}</div>", "O3污染物小时报"));
                dataTitle = dateTime;
                sb.AppendLine(string.Format("<div><input type='button'  onclick=\"ascTable('{0}','{1}')\" class='asc' title = '升序'><input type='button'  onclick=\"normalTable('{0}','{1}')\" class='normal' title = '恢复原序'><input type='button'  onclick=\"descTable('{0}','{1}')\" class='desc' title = '降序'><div id='title' class='tableTitletime'>{2}</div></div>", dateTime, flag, dataTitle));
            }
            else
            {
                sb.AppendLine(string.Format("<div id='title' class='tableTitleImage'>{0}</div>", "PM2.5污染物日报"));
                dateTime = DateTime.Parse(dateTime).ToString("yyyy-M-d 0:00:00");
                dataTitle = "&nbsp;&nbsp;&nbsp;" + DateTime.Parse(dateTime).ToString("yyyy-M-d");
                sb.AppendLine(string.Format("<div><input type='button'  onclick=\"ascTable('{0}','{1}')\" class='asc' title = '升序'><input type='button'  onclick=\"normalTable('{0}','{1}')\" class='normal' title = '恢复原序'><input type='button'  onclick=\"descTable('{0}','{1}')\" class='desc' title = '降序'><div id='title' class='tableTitletime'>{2}</div></div>", dateTime, flag, dataTitle));
            }

            sb.AppendLine("<table id='imageTableKuang'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<td class='imagetableRowtitle'>站点</td>");
            sb.AppendLine("<td class='imagetableRowtitle'>浓度</td>");
            sb.AppendLine("<td class='imagetableRowRightTitle'>AQI</td>");

            sb.AppendLine("<td class='imagetableRowtitle'>站点</td>");
            sb.AppendLine("<td class='imagetableRowtitle'>浓度</td>");
            sb.AppendLine("<td class='imagetableRowRightTitle'>AQI</td>");

            sb.AppendLine("<td class='imagetableRowtitle'>站点</td>");
            sb.AppendLine("<td class='imagetableRowtitle'>浓度</td>");
            sb.AppendLine("<td class='imagetableRowRightTitle'>AQI</td>");

            sb.AppendLine("<td class='imagetableRowtitle'>站点</td>");
            sb.AppendLine("<td class='imagetableRowtitle'>浓度</td>");
            sb.AppendLine("<td class='imagetableRowtitle'>AQI</td>");
            sb.AppendLine("</tr>");
            int length = 0;
            AQIExtention aqiExt;
            int AQIValue;
            for (int i = 0; i < dt.Rows.Count / 4; i++)
            {
                sb.AppendLine("<tr>");
                AQIValue = int.Parse(dt.Rows[i * 4][2].ToString());
                aqiExt = new AQIExtention(AQIValue);
                sb.AppendLine(string.Format("<td class='imagetableRow' style='background-color:{1};{2}'>{0}</td>", dt.Rows[i * 4][0], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                sb.AppendLine(string.Format("<td class='imagetableRow' style='background-color:{1};{2}'>{0}</td>", dt.Rows[i * 4][1], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                sb.AppendLine(string.Format("<td class='imagetableRowRight' style='background-color:{1};{2}'>{0}</td>", dt.Rows[i * 4][2], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                AQIValue = int.Parse(dt.Rows[i * 4 + 1][2].ToString());
                aqiExt = new AQIExtention(AQIValue);

                sb.AppendLine(string.Format("<td class='imagetableRow' style='background-color:{1};{2}'>{0}</td>", dt.Rows[i * 4 + 1][0], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                sb.AppendLine(string.Format("<td class='imagetableRow' style='background-color:{1};{2}'>{0}</td>", dt.Rows[i * 4 + 1][1], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                sb.AppendLine(string.Format("<td class='imagetableRowRight' style='background-color:{1};{2}'>{0}</td>", dt.Rows[i * 4 + 1][2], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                AQIValue = int.Parse(dt.Rows[i * 4 + 2][2].ToString());
                aqiExt = new AQIExtention(AQIValue);

                sb.AppendLine(string.Format("<td class='imagetableRow' style='background-color:{1};{2}'>{0}</td>", dt.Rows[i * 4 + 2][0], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                sb.AppendLine(string.Format("<td class='imagetableRow' style='background-color:{1};{2}'>{0}</td>", dt.Rows[i * 4 + 2][1], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                sb.AppendLine(string.Format("<td class='imagetableRowRight' style='background-color:{1};{2}'>{0}</td>", dt.Rows[i * 4 + 2][2], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                AQIValue = int.Parse(dt.Rows[i * 4 + 3][2].ToString());
                aqiExt = new AQIExtention(AQIValue);

                sb.AppendLine(string.Format("<td class='imagetableRow' style='background-color:{1};{2}'>{0}</td>", dt.Rows[i * 4 + 3][0], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                sb.AppendLine(string.Format("<td class='imagetableRow' style='background-color:{1};{2}'>{0}</td>", dt.Rows[i * 4 + 3][1], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                sb.AppendLine(string.Format("<td class='imagetableRow' style='background-color:{1};	font-style: italic;;{2}'>{0}</td>", dt.Rows[i * 4 + 3][2], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                sb.AppendLine("</tr>");
                length = i;
            }
            if (dt.Rows.Count % 4 != 0)
            {
                sb.AppendLine("<tr>");
                for (int j = 0; j < dt.Rows.Count % 4; j++)
                {
                    if (j == 3)
                    {
                        AQIValue = int.Parse(dt.Rows[(dt.Rows.Count / 4) * 4 + j][2].ToString());
                        aqiExt = new AQIExtention(AQIValue);
                        sb.AppendLine(string.Format("<td class='imagetableRowBorder' style='background-color:{1};{2}'>{0}</td>", dt.Rows[(dt.Rows.Count / 4) * 4 + j][0], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                        sb.AppendLine(string.Format("<td class='imagetableRowBorder' style='background-color:{1};{2}'>{0}</td>", dt.Rows[(dt.Rows.Count / 4) * 4 + j][1], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                        sb.AppendLine(string.Format("<td class='imagetableRowBorder' style='background-color:{1};font-style: italic;;{2}'	>{0}</td>", dt.Rows[(dt.Rows.Count / 4) * 4 + j][2], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                    }
                    else
                    {
                        AQIValue = int.Parse(dt.Rows[(dt.Rows.Count / 4) * 4 + j][2].ToString());
                        aqiExt = new AQIExtention(AQIValue);
                        sb.AppendLine(string.Format("<td class='imagetableRowBorder' style='background-color:{1};{2}'>{0}</td>", dt.Rows[(dt.Rows.Count / 4) * 4 + j][0], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                        sb.AppendLine(string.Format("<td class='imagetableRowBorder' style='background-color:{1};{2}'>{0}</td>", dt.Rows[(dt.Rows.Count / 4) * 4 + j][1], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                        sb.AppendLine(string.Format("<td class='imagetableRowRightBorder' style='background-color:{1};	font-style: italic;;{2} ' >{0}</td>", dt.Rows[(dt.Rows.Count / 4) * 4 + j][2], color(aqiExt.Color), Fontcolor(aqiExt.Color)));
                    }

                }
                sb.AppendLine("</tr>");

            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
        public string ascTable(string dateTime, string flag)
        {
            //dateTime = "2013-12-24 16:00:00";
            string strSQL;
            DataTable dt;
            if (flag == "hour")
            {
                strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value ASC", dateTime);
                dt = m_DatabaseS.GetDataTable(strSQL);
            }
            else if (flag == "PM10Air")//实时监测PM10
            {
                strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 2) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value ASC", dateTime);
                dt = m_DatabaseS.GetDataTable(strSQL);
            }
            else if (flag == "SH")//上海
            {
                strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='PM2.5' and LST_AQI='{0}' order by AQI ASC", dateTime);
                dt = m_Database.GetDataTable(strSQL);
            }
            else if (flag == "SHO3")//上海
            {
                strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='O3' and LST_AQI='{0}' order by AQI ASC", dateTime);
                dt = m_Database.GetDataTable(strSQL);
            }
            else if (flag == "SHPM10")//上海
            {
                strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='PM10' and LST_AQI='{0}' order by AQI ASC", dateTime);
                dt = m_Database.GetDataTable(strSQL);
            }
            else
            {
                strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_DATEAVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value ASC", dateTime);
                dt = m_DatabaseS.GetDataTable(strSQL);
            }
            string returnStr = htmlJoson(dt, dateTime, flag);
            return returnStr;
        }
        public string descTable(string dateTime, string flag)
        {
            //dateTime = "2013-12-24 16:00:00";
            string strSQL;
            DataTable dt;
            if (flag == "hour")
            {
                strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value DESC", dateTime);
                dt = m_DatabaseS.GetDataTable(strSQL);
            }
            else if (flag == "PM10Air")//实时监测PM10
            {
                strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 2) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value DESC", dateTime);
                dt = m_DatabaseS.GetDataTable(strSQL);
            }
            else if (flag == "SHO3")//上海
            {
                strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='O3' and LST_AQI='{0}' order by AQI DESC", dateTime);
                dt = m_Database.GetDataTable(strSQL);
            }
            else if (flag == "SHPM10")//上海
            {
                strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='PM10' and LST_AQI='{0}' order by AQI DESC", dateTime);
                dt = m_Database.GetDataTable(strSQL);
            }
            else if (flag == "SH")//上海
            {
                strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='PM2.5' and LST_AQI='{0}' order by AQI DESC", dateTime);
                dt = m_Database.GetDataTable(strSQL);
            }
            else
            {
                strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_DATEAVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  a.value DESC", dateTime);
                dt = m_DatabaseS.GetDataTable(strSQL);
            }
            string returnStr = htmlJoson(dt, dateTime, flag);
            return returnStr;
        }
        public string normalTable(string dateTime, string flag)
        {
            //dateTime = "2013-12-24 16:00:00";//测试用的，数据库数据不多
            string strSQL;
            DataTable dt;
            if (flag == "hour")//实时监测PM2.5
            {
                strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  isNull(China_RT_city_station.id,10000000) ", dateTime);
                dt = m_DatabaseS.GetDataTable(strSQL);
            }
            else if (flag == "PM10Air")//实时监测PM10
            {
                strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_AVERAGE WHERE (itemid = 2) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  isNull(China_RT_city_station.id ,10000000)", dateTime);
                dt = m_DatabaseS.GetDataTable(strSQL);
            }
            else if (flag == "SHO3")//上海
            {
                strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='O3' and LST_AQI='{0}' order by NAME", dateTime);
                dt = m_Database.GetDataTable(strSQL);
            }
            else if (flag == "SHPM10")//上海
            {
                strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='PM10' and LST_AQI='{0}' order by NAME", dateTime);
                dt = m_Database.GetDataTable(strSQL);
            }
            else if (flag == "SH")//上海
            {
                strSQL = string.Format("SELECT Name as area,LEFT(Value*1000, CHARINDEX('.', Value*1000) + 1) as value,aqi   FROM [SEMC_AQI].[dbo].[V_ForDiting_Mapping] Where Parameter ='PM2.5' and LST_AQI='{0}' order by NAME", dateTime);
                dt = m_Database.GetDataTable(strSQL);
            }
            else
            {//实时监测PM2.5重污染区
                strSQL = string.Format("SELECT A.area, value, aqi FROM (select * from  CHINA_RT_CITY_DATEAVERAGE WHERE (itemid = 1) AND (timepoint ='{0}'))as A left join China_RT_city_station on a.area=China_RT_city_station.area order by  isNull(China_RT_city_station.id,10000000) ", dateTime);
                dt = m_DatabaseS.GetDataTable(strSQL);
            }
            string returnStr = htmlJoson(dt, dateTime, flag);
            return returnStr;
        }
        public string WeekcommentCotent(string entityName)
        {
            StringBuilder sb = new StringBuilder();
            string strSQL = "SELECT ModuleName FROM T_ImageProduct_test WHERE EntityName='" + entityName + "'";
            string moduleName = m_Database.GetFirstValue(strSQL);
            DateTime startTime = DateTime.Now.Date.AddDays(-1);
            DateTime endTime = startTime.AddDays(2);
            strSQL = "SELECT * From T_PerWeekRevise WHERE CommentTime BETWEEN '" + startTime + "' AND '" + endTime + "' ORDER BY CommentTime";
            DataTable dt = m_Database.GetDataTable(strSQL);//明天和后天的数据
            string flag = "";
            sb.AppendLine("<input type='button'  onclick='closeWeek()' class='close' onmouseover=\"this.className = 'closeHover';\" onmouseout =\"this.className ='close';\"   id='commentClose'>");
            sb.AppendLine("<div class='titleNameOther'>预报会商</div>");
            sb.AppendLine("<table id='WeekDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='tabletitleL'>日期</td>");
            sb.AppendLine("<td class='tabletitle'>今天(<b style='font-family:宋体;font-size:12px;'>" + DateTime.Now.ToString("MM月dd日") + "</b>)</td>");
            sb.AppendLine("<td class='tabletitle'>明天(<b style='font-family:宋体;font-size:12px;'>" + DateTime.Now.AddDays(1).ToString("MM月dd日") + "</b>)</td>");
            sb.AppendLine("<td class='tabletitle'>后天(<b style='font-family:宋体;font-size:12px;'>" + DateTime.Now.AddDays(2).ToString("MM月dd日") + "</b>)</td>");
            sb.AppendLine("</tr>");
            if (moduleName == "innp")//innp
            {
                flag = "0";
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='tabletitleOtherLeft'>气象分析</td>");
                if (dt.Rows.Count == 1)
                {
                    if (DateTime.Parse(dt.Rows[0]["CommentTime"].ToString()).Date == DateTime.Now.Date)
                    {
                        sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W000' cols='35' rows='3'></textarea></td>");
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W100' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["qixLiveAnalyse24"].ToString()));
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W200' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["qixLiveAnalyse48"].ToString()));
                        sb.AppendLine("</tr>");
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td class='tabletitleOtherLeft'>气象预报</td>");
                        sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text'class='inputClss' id='W011'></div><div class='labelCss'>江苏沿江<input class='inputClss' type='text' id='W012'></div><div class='labelCss'>上海<input class='inputClss' type='text' id='W013'></div><div class='labelCss'>浙北<input type='text' class='inputClss' id='W014'></div><div class='labelCss'>浙东南<input type='text' class='inputClss' id='W015'></div><div class='labelCss'>浙西南<input type='text' class='inputClss' id='W016'></div></td>");
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input class='inputClss' type='text' id='W111' value='{0}'></div><div class='labelCss'>江苏沿江<input class='inputClss'  type='text' id='W112' value='{1}'></div><div class='labelCss'>上海<input class='inputClss' type='text' id='W113' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W114' class='inputClss' value='{3}'></div><div class='labelCss'>浙东南<input type='text' class='inputClss' id='W115' value='{4}'></div><div class='labelCss'>浙西南<input type='text' class='inputClss' id='W116' value='{5}'></div></td>", dt.Rows[0]["QSuBei24"].ToString(), dt.Rows[0]["QjiangsuYj24"].ToString(), dt.Rows[0]["Qshanghai24"].ToString(), dt.Rows[0]["Qzhebei24"].ToString(), dt.Rows[0]["Qzhedongnan24"].ToString(), dt.Rows[0]["Qzhexinan24"].ToString()));
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input class='inputClss' type='text' id='W211' value='{0}'></div><div class='labelCss'>江苏沿江<input class='inputClss' type='text' id='W212' value='{1}'></div><div class='labelCss'>上海<input class='inputClss' type='text' id='W213' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W214' class='inputClss' value='{3}'></div><div class='labelCss'>浙东南<input type='text' class='inputClss' id='W215' value='{4}'></div><div class='labelCss'>浙西南<input type='text' class='inputClss' id='W216' value='{5}'></div></td>", dt.Rows[0]["QSuBei48"].ToString(), dt.Rows[0]["QjiangsuYj48"].ToString(), dt.Rows[0]["Qshanghai48"].ToString(), dt.Rows[0]["Qzhebei48"].ToString(), dt.Rows[0]["Qzhedongnan48"].ToString(), dt.Rows[0]["Qzhexinan48"].ToString()));
                        sb.AppendLine("</tr>");
                    }
                    else
                    {
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea id='W000' cols='35' class='textarea' rows='3'>{0}</textarea></td>", dt.Rows[0]["qixLiveAnalyse24"].ToString()));
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea  id='W100' cols='35' class='textarea' c rows='3'></textarea></td>"));
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea  id='W200' cols='35' class='textarea'  rows='3'></textarea></td>"));
                        sb.AppendLine("</tr>");
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td class='tabletitleOtherLeft'>气象预报</td>");
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' class='inputClss' id='W011' value='{0}'></div><div class='labelCss'>江苏沿江<input  class='inputClss' type='text' id='W012' value='{1}'></div><div class='labelCss'>上海<input class='inputClss'  type='text' id='W013' value='{2}'></div><div class='labelCss'>浙北<input type='text' class='inputClss' id='W014' value='{3}'></div><div class='labelCss'>浙东南<input type='text' class='inputClss'  id='W015' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W016' class='inputClss' value='{5}'></div></td>", dt.Rows[0]["QSuBei24"].ToString(), dt.Rows[0]["QjiangsuYj24"].ToString(), dt.Rows[0]["Qshanghai24"].ToString(), dt.Rows[0]["Qzhebei24"].ToString(), dt.Rows[0]["Qzhedongnan24"].ToString(), dt.Rows[0]["Qzhexinan24"].ToString()));
                        sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' class='inputClss' id='W111'></div><div class='labelCss'>江苏沿江<input class='inputClss' type='text' id='W112'></div><div class='labelCss'>上海<input type='text' class='inputClss' id='W113'></div><div class='labelCss'>浙北<input type='text' class='inputClss' id='W114'></div><div class='labelCss'>浙东南<input type='text' class='inputClss' id='W115'></div><div class='labelCss'>浙西南<input class='inputClss'  type='text' id='W116'></div></td>");
                        sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' class='inputClss' id='W211'></div><div class='labelCss'>江苏沿江<input class='inputClss' type='text' id='W212'></div><div class='labelCss'>上海<input type='text' class='inputClss' id='W213'></div><div class='labelCss'>浙北<input type='text' class='inputClss' id='W214'></div><div class='labelCss'>浙东南<input type='text' class='inputClss' id='W215'></div><div class='labelCss'>浙西南<input class='inputClss' type='text' id='W216'></div></td>");
                        sb.AppendLine("</tr>");
                    }

                }
                if (dt.Rows.Count == 0)
                {
                    sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W000' cols='35' rows='3'></textarea></td>");
                    sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea'  id='W100' cols='35' rows='3'></textarea></td>");
                    sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea'  id='W200' cols='35' rows='3'></textarea></td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>气象预报</td>");
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W011' /></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W012'/></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W013'/></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W014'/></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W015'/></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W016'/></div></td>");
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W111'/></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W112'/></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W113'/></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W114'/></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W115'/></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W116'/></div></td>");
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W211'/></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W212'/></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W213'/></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W214'/></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W215'/></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W216'/></div></td>");
                    sb.AppendLine("</tr>");
                }
                if (dt.Rows.Count == 2)
                {
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  cols='35' id='W000'  rows='3'>{0}</textarea></td>", dt.Rows[0]["qixLiveAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  cols='35'id='W100'  rows='3'>{0}</textarea></td>", dt.Rows[1]["qixLiveAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  cols='35' id='W200' rows='3'>{0}</textarea></td>", dt.Rows[1]["qixLiveAnalyse48"].ToString()));
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>气象预报</td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss'  id='W011' value='{0}'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W012' value='{1}'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W013' value='{2}'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W014' value='{3}'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W015' value='{4}'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W016' value='{5}'></div></td>", dt.Rows[0]["QSuBei24"].ToString(), dt.Rows[0]["QjiangsuYj24"].ToString(), dt.Rows[0]["Qshanghai24"].ToString(), dt.Rows[0]["Qzhebei24"].ToString(), dt.Rows[0]["Qzhedongnan24"].ToString(), dt.Rows[0]["Qzhexinan24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss'  id='W111' value='{0}'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W112' value='{1}'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W113' value='{2}'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W114' value='{3}'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W115' value='{4}'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W116' value='{5}'></div></td>", dt.Rows[1]["QSuBei24"].ToString(), dt.Rows[1]["QjiangsuYj24"].ToString(), dt.Rows[1]["Qshanghai24"].ToString(), dt.Rows[1]["Qzhebei24"].ToString(), dt.Rows[1]["Qzhedongnan24"].ToString(), dt.Rows[1]["Qzhexinan24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss'  id='W211' value='{0}'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W212' value='{1}'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W213' value='{2}'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W214' value='{3}'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W215' value='{4}'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W216' value='{5}'></div></td>", dt.Rows[1]["QSuBei48"].ToString(), dt.Rows[1]["QjiangsuYj48"].ToString(), dt.Rows[1]["Qshanghai48"].ToString(), dt.Rows[1]["Qzhebei48"].ToString(), dt.Rows[1]["Qzhedongnan48"].ToString(), dt.Rows[1]["Qzhexinan48"].ToString()));
                    sb.AppendLine("</tr>");

                }

            }
            if (moduleName == "airQuality" || moduleName == "jgRadar" || moduleName == "xsforcast")//moduleName == "airQuality" || moduleName == "jgRadar" || moduleName == "xsforcast"
            {
                flag = "1";
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='tabletitleOtherLeft'>实况分析</td>");
                if (dt.Rows.Count == 1)
                {
                    if (DateTime.Parse(dt.Rows[0]["CommentTime"].ToString()).Date == DateTime.Now.Date)
                    {
                        sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W020' cols='35' rows='3'></textarea></td>");
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W120' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteLiveAnalyse24"].ToString()));
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W220' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteLiveAnalyse48"].ToString()));
                        sb.AppendLine("</tr>");
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td class='tabletitleOtherLeft'>预报依据</td>");
                        sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea'  cols='33' id='W030'  rows='3'></textarea></td>");
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W130'  cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteCauseAnalyse24"].ToString()));
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W230'  cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteCauseAnalyse48"].ToString()));
                        sb.AppendLine("</tr>");
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td class='tabletitleOtherLeft'>污染预报</td>");
                        sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W041'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W042'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W043'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W014'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W045'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W046'></div></td>");
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W141' value='{0}'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W142' value='{1}'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W143' value='{2}'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W144' value='{3}'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W145' value='{4}'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W146' value='{5}'></div></td>", dt.Rows[0]["PSuBei24"].ToString(), dt.Rows[0]["PjiangsuYj24"].ToString(), dt.Rows[0]["Pshanghai24"].ToString(), dt.Rows[0]["Pzhebei24"].ToString(), dt.Rows[0]["Pzhedongnan24"].ToString(), dt.Rows[0]["Pzhexinan24"].ToString()));
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W241' value='{0}'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W242' value='{1}'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W243' value='{2}'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W244' value='{3}'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W245' value='{4}'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W246' value='{5}'></div></td>", dt.Rows[0]["PSuBei48"].ToString(), dt.Rows[0]["PjiangsuYj48"].ToString(), dt.Rows[0]["Pshanghai48"].ToString(), dt.Rows[0]["Pzhebei48"].ToString(), dt.Rows[0]["Pzhedongnan48"].ToString(), dt.Rows[0]["Pzhexinan48"].ToString()));
                        sb.AppendLine("</tr>");
                    }
                    else
                    {
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea id='W020' class='textarea' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteLiveAnalyse24"].ToString()));
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W120' cols='35' rows='3'></textarea></td>"));
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W220' cols='35' rows='3'></textarea></td>"));
                        sb.AppendLine("</tr>");
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td class='tabletitleOtherLeft'>预报依据</td>");
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W030'  cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteCauseAnalyse24"].ToString()));
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W130'  cols='35' rows='3'></textarea></td>"));
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W230'  cols='35' rows='3'></textarea></td>"));
                        sb.AppendLine("</tr>");
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td class='tabletitleOtherLeft'>污染预报</td>");
                        sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W041' value='{0}'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W042' value='{1}'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W043' value='{2}'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W044' value='{3}'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W045' value='{4}'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W046' value='{5}'></div></td>", dt.Rows[0]["PSuBei24"].ToString(), dt.Rows[0]["PjiangsuYj24"].ToString(), dt.Rows[0]["Pshanghai24"].ToString(), dt.Rows[0]["Pzhebei24"].ToString(), dt.Rows[0]["Pzhedongnan24"].ToString(), dt.Rows[0]["Pzhexinan24"].ToString()));
                        sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W141'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W142'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W143'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W144'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W145'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W146'></div></td>");
                        sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W241'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W242'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W243'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W244'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W245'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W246'></div></td>");
                        sb.AppendLine("</tr>");
                    }

                }
                if (dt.Rows.Count == 0)
                {
                    sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W020' cols='35' rows='3'></textarea></td>");
                    sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W120' cols='35' rows='3'></textarea></td>");
                    sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W220' cols='35' rows='3'></textarea></td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>预报依据</td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W030'  cols='35' rows='3'></textarea></td>"));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W130'  cols='35' rows='3'></textarea></td>"));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W230'  cols='35' rows='3'></textarea></td>"));
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>污染预报</td>");
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W041'/></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W042'/></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W043'/></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W014'/></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W045'/></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W046'/></div></td>");
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W141'/></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W142'/></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W143'/></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W144'/></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W145'/></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W146'/></div></td>");
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W241'/></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W242'/></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W243'/></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W244'/></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W245'/></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W246'/></div></td>");
                    sb.AppendLine("</tr>");
                }
                if (dt.Rows.Count == 2)
                {
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W020' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteLiveAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W120'  cols='35' rows='3'>{0}</textarea></td>", dt.Rows[1]["polluteLiveAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'   id='W220' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[1]["polluteLiveAnalyse48"].ToString()));
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>预报依据</td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W030' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteCauseAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W130' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[1]["polluteCauseAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W230'  cols='35' rows='3'>{0}</textarea></td>", dt.Rows[1]["polluteCauseAnalyse48"].ToString()));
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>污染预报</td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W041' value='{0}'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W042' value='{1}'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W043' value='{2}'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W044' value='{3}'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W045' value='{4}'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W046' value='{5}'></div></td>", dt.Rows[0]["PSuBei24"].ToString(), dt.Rows[0]["PjiangsuYj24"].ToString(), dt.Rows[0]["Pshanghai24"].ToString(), dt.Rows[0]["Pzhebei24"].ToString(), dt.Rows[0]["Pzhedongnan24"].ToString(), dt.Rows[0]["Pzhexinan24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W141' value='{0}'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W142' value='{1}'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W143' value='{2}'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W144' value='{3}'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W145' value='{4}'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W146' value='{5}'></div></td>", dt.Rows[1]["PSuBei24"].ToString(), dt.Rows[1]["PjiangsuYj24"].ToString(), dt.Rows[1]["Pshanghai24"].ToString(), dt.Rows[1]["Pzhebei24"].ToString(), dt.Rows[1]["Pzhedongnan24"].ToString(), dt.Rows[1]["Pzhexinan24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市 <input type='text'  class='inputClss' id='W241' value='{0}'></div><div class='labelCss'>江苏沿江 <input type='text'  class='inputClss' id='W242' value='{1}'></div><div class='labelCss'>上海 <input type='text'  class='inputClss' id='W243' value='{2}'></div><div class='labelCss'>浙北 <input type='text'  class='inputClss' id='W244' value='{3}'></div><div class='labelCss'>浙东南 <input type='text'  class='inputClss' id='W245' value='{4}'></div><div class='labelCss'>浙西南 <input type='text'  class='inputClss' id='W246' value='{5}'></div></td>", dt.Rows[1]["PSuBei48"].ToString(), dt.Rows[1]["PjiangsuYj48"].ToString(), dt.Rows[1]["Pshanghai48"].ToString(), dt.Rows[1]["Pzhebei48"].ToString(), dt.Rows[1]["Pzhedongnan48"].ToString(), dt.Rows[1]["Pzhexinan48"].ToString()));
                    sb.AppendLine("</tr>");

                }

            }
            sb.AppendLine("</table>");
            sb.AppendLine("<div class='divSave'>");
            sb.AppendLine("<input type='button'  onclick=\"SaveRevise('" + flag + "')\"  class='normal-btn  input-btn' onmouseover='this.className=\"normal-btn-h input-btn\"' onmouseout='this.className=\"normal-btn input-btn\"' onmousedown='this.className=\"normal-btn-d input-btn\"' onmouseup ='this.className='normal-btn input-btn\"' value='保存'/>");
            sb.AppendLine("</div>");
            return sb.ToString();
        }
        public string SaveRevise(string content, string flag, string name)
        {
            string[] parts = content.Split(',');
            string newValue = "";
            string oldValue = "";
            string tomoValue = "";
            string[] keyValue;
            string strSQL = "";
            for (int i = 0; i < parts.Length; i++)
            {
                keyValue = parts[i].Split(':');
                if (flag == "0")
                {
                    if (keyValue[0].Substring(1, 1) == "0")
                    {
                        oldValue = oldValue + keyValue[1] + ",";
                    }
                    else if (keyValue[0].Substring(1, 1) == "1")
                    {
                        newValue = newValue + keyValue[1] + ",";
                    }
                    else if (keyValue[0].Substring(1, 1) == "2")
                    {
                        tomoValue = tomoValue + keyValue[1] + ",";
                    }

                }
                else
                {
                    if (keyValue[0].Substring(1, 1) == "0")
                    {
                        oldValue = oldValue + keyValue[1] + ",";
                    }
                    else if (keyValue[0].Substring(1, 1) == "1")
                    {
                        newValue = newValue + keyValue[1] + ",";
                    }
                    else if (keyValue[0].Substring(1, 1) == "2")
                    {
                        tomoValue = tomoValue + keyValue[1] + ",";
                    }
                }
            }
            string Value = newValue + tomoValue;
            Value = Value.Substring(0, Value.Length - 1);
            oldValue = oldValue.Substring(0, oldValue.Length - 1);
            string[] qixiang = oldValue.Split(',');
            string[] qixiangNew = Value.Split(',');
            string existsSQL = "";
            string updateSQL = "";
            DateTime startTime = DateTime.Now.Date;
            DateTime endTime = startTime.AddDays(1);
            DateTime oldTime = startTime.AddDays(-1);
            try
            {
                if (flag == "0")
                {
                    existsSQL = "SELECT CommentTime FROM T_PerWeekRevise WHERE CommentTime BETWEEN  '" + oldTime + "' AND  '" + startTime + "'";
                    updateSQL = "UPDATE T_PerWeekRevise SET  CommentTime='" + DateTime.Now.AddDays(-1) + "',qixLiveAnalyse24='" + qixiang[0] + "',QSuBei24='" + qixiang[1] + "',QjiangsuYj24='" + qixiang[2] + "',Qshanghai24='" + qixiang[3] + "',Qzhebei24='" + qixiang[4] + "',Qzhedongnan24='" + qixiang[5] + "',Qzhexinan24='" + qixiang[6] + "' WHERE CommentTime BETWEEN  '" + oldTime + "' AND  '" + startTime + "'";
                    strSQL = "INSERT INTO T_PerWeekRevise(CommentTime,Name,qixLiveAnalyse24,QSuBei24,QjiangsuYj24,Qshanghai24,Qzhebei24,Qzhedongnan24,Qzhexinan24) VALUES('" + DateTime.Now.AddDays(-1) + "','" + name + "','" + qixiang[0] + "','" + qixiang[1] + "','" + qixiang[2] + "','" + qixiang[3] + "','" + qixiang[4] + "','" + qixiang[5] + "','" + qixiang[6] + "')";
                    m_Database.Execute(existsSQL, updateSQL, strSQL);

                    existsSQL = "SELECT CommentTime FROM T_PerWeekRevise WHERE CommentTime BETWEEN  '" + startTime + "' AND  '" + endTime + "'";
                    updateSQL = "UPDATE T_PerWeekRevise SET  CommentTime='" + DateTime.Now + "',qixLiveAnalyse24='" + qixiangNew[0] + "',QSuBei24='" + qixiangNew[1] + "',QjiangsuYj24='" + qixiangNew[2] + "',Qshanghai24='" + qixiangNew[3] + "',Qzhebei24='" + qixiangNew[4] + "',Qzhedongnan24='" + qixiangNew[5] + "',Qzhexinan24='" + qixiangNew[6] + "',qixLiveAnalyse48='" + qixiangNew[7] + "',QSuBei48='" + qixiangNew[8] + "',QjiangsuYj48='" + qixiangNew[9] + "',Qshanghai48='" + qixiangNew[10] + "',Qzhebei48='" + qixiangNew[11] + "',Qzhedongnan48='" + qixiangNew[12] + "',Qzhexinan48='" + qixiangNew[13] + "'  WHERE CommentTime BETWEEN  '" + startTime + "' AND  '" + endTime + "'";
                    strSQL = "INSERT INTO T_PerWeekRevise(CommentTime,Name,qixLiveAnalyse24,QSuBei24,QjiangsuYj24,Qshanghai24,Qzhebei24,Qzhedongnan24,Qzhexinan24,qixLiveAnalyse48,QSuBei48,QjiangsuYj48,Qshanghai48,Qzhebei48,Qzhedongnan48,Qzhexinan48) VALUES('" + DateTime.Now + "','" + name + "','" + qixiangNew[0] + "','" + qixiangNew[1] + "','" + qixiangNew[2] + "','" + qixiangNew[3] + "','" + qixiangNew[4] + "','" + qixiangNew[5] + "','" + qixiangNew[6] + "','" + qixiangNew[7] + "','" + qixiangNew[8] + "','" + qixiangNew[9] + "','" + qixiangNew[10] + "','" + qixiangNew[11] + "','" + qixiangNew[12] + "','" + qixiangNew[13] + "')";
                    m_Database.Execute(existsSQL, updateSQL, strSQL);
                }
                else
                {
                    existsSQL = "SELECT CommentTime FROM T_PerWeekRevise WHERE CommentTime BETWEEN  '" + oldTime + "' AND  '" + startTime + "'";
                    updateSQL = "UPDATE T_PerWeekRevise SET  CommentTime='" + DateTime.Now.AddDays(-1) + "',polluteLiveAnalyse24='" + qixiang[0] + "',polluteCauseAnalyse24='" + qixiang[1] + "',PSuBei24='" + qixiang[2] + "',PjiangsuYj24='" + qixiang[3] + "',Pshanghai24='" + qixiang[4] + "',Pzhebei24='" + qixiang[5] + "',Pzhedongnan24='" + qixiang[6] + "',Pzhexinan24='" + qixiang[7] + "' WHERE CommentTime BETWEEN  '" + oldTime + "' AND  '" + startTime + "'";
                    strSQL = "INSERT INTO T_PerWeekRevise(CommentTime,Name,polluteLiveAnalyse24,polluteCauseAnalyse24,PSuBei24,PjiangsuYj24,Pshanghai24,Pzhebei24,Pzhedongnan24,Pzhexinan24) VALUES('" + DateTime.Now.AddDays(-1) + "','" + name + "','" + qixiang[0] + "','" + qixiang[1] + "','" + qixiang[2] + "','" + qixiang[3] + "','" + qixiang[4] + "','" + qixiang[5] + "','" + qixiang[6] + "','" + qixiang[7] + "')";
                    m_Database.Execute(existsSQL, updateSQL, strSQL);


                    existsSQL = "SELECT CommentTime FROM T_PerWeekRevise WHERE CommentTime BETWEEN  '" + startTime + "' AND  '" + endTime + "'";
                    updateSQL = "UPDATE T_PerWeekRevise SET  CommentTime='" + DateTime.Now + "',polluteLiveAnalyse24='" + qixiangNew[0] + "',polluteCauseAnalyse24='" + qixiangNew[1] + "',PSuBei24='" + qixiangNew[2] + "',PjiangsuYj24='" + qixiangNew[3] + "',Pshanghai24='" + qixiangNew[4] + "',Pzhebei24='" + qixiangNew[5] + "',Pzhedongnan24='" + qixiangNew[6] + "',Pzhexinan24='" + qixiangNew[7] + "',polluteLiveAnalyse48='" + qixiangNew[8] + "',polluteCauseAnalyse48='" + qixiangNew[9] + "',PSuBei48='" + qixiangNew[10] + "',PjiangsuYj48='" + qixiangNew[11] + "',Pshanghai48='" + qixiangNew[12] + "',Pzhebei48='" + qixiangNew[13] + "',Pzhedongnan48='" + qixiangNew[14] + "',Pzhexinan48='" + qixiangNew[15] + "'  WHERE CommentTime BETWEEN  '" + startTime + "' AND  '" + endTime + "'";
                    strSQL = "INSERT INTO T_PerWeekRevise(CommentTime,Name,polluteLiveAnalyse24,polluteCauseAnalyse24,PSuBei24,PjiangsuYj24,Pshanghai24,Pzhebei24,Pzhedongnan24,Pzhexinan24,polluteLiveAnalyse48,polluteCauseAnalyse48,PSuBei48,PjiangsuYj48,Pshanghai48,Pzhebei48,Pzhedongnan48,Pzhexinan48) VALUES('" + DateTime.Now + "','" + name + "','" + qixiangNew[0] + "','" + qixiangNew[1] + "','" + qixiangNew[2] + "','" + qixiangNew[3] + "','" + qixiangNew[4] + "','" + qixiangNew[5] + "','" + qixiangNew[6] + "','" + qixiangNew[7] + "','" + qixiangNew[8] + "','" + qixiangNew[9] + "','" + qixiangNew[10] + "','" + qixiangNew[11] + "','" + qixiangNew[12] + "','" + qixiangNew[13] + "','" + qixiangNew[14] + "','" + qixiangNew[15] + "')";
                    m_Database.Execute(existsSQL, updateSQL, strSQL);
                }
                return "成功";
            }
            catch (Exception ex)
            {
                m_Log.Error("SaveRevise", ex);
                return ex.ToString();
            }
        }
        public string QueryWeekRevise(string dateTime)
        {
            StringBuilder sb = new StringBuilder();
            DateTime startTime = DateTime.Parse(dateTime).Date;
            DateTime oldTime = startTime.AddDays(-1);
            DateTime endTime = startTime.AddDays(1);
            string strSQL = "SELECT * From T_PerWeekRevise WHERE CommentTime BETWEEN '" + oldTime + "' AND '" + endTime + "' ORDER BY CommentTime";
            DataTable dt = m_Database.GetDataTable(strSQL);//明天和后天的数据

            sb.AppendLine("<table id='WeekDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sb.AppendLine("<tr>");

            //创建抬头
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='tabletitleL'>日期</td>");
            sb.AppendLine("<td class='tabletitle'>今天</td>");
            sb.AppendLine("<td class='tabletitle'>明天</td>");
            sb.AppendLine("<td class='tabletitle'>后天</td>");
            sb.AppendLine("</tr>");
            if (dt.Rows.Count == 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='tabletitleOtherLeft'>气象分析</td>");
                sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W000' cols='35' rows='3'></textarea></td>");
                sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea'  id='W100' cols='35' rows='3'></textarea></td>");
                sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea'  id='W200' cols='35' rows='3'></textarea></td>");
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='tabletitleOtherLeft'>气象预报</td>");
                sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W011' /></div><div class='labelCss'>江苏沿江<input type='text' id='W012'/></div><div class='labelCss'>上海<input type='text' id='W013'/></div><div class='labelCss'>浙北<input type='text' id='W014'/></div><div class='labelCss'>浙东南<input type='text' id='W015'/></div><div class='labelCss'>浙西南<input type='text' id='W016'/></div></td>");
                sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W111'/></div><div class='labelCss'>江苏沿江<input type='text' id='W112'/></div><div class='labelCss'>上海<input type='text' id='W113'/></div><div class='labelCss'>浙北<input type='text' id='W114'/></div><div class='labelCss'>浙东南<input type='text' id='W115'/></div><div class='labelCss'>浙西南<input type='text' id='W116'/></div></td>");
                sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W211'/></div><div class='labelCss'>江苏沿江<input type='text' id='W212'/></div><div class='labelCss'>上海<input type='text' id='W213'/></div><div class='labelCss'>浙北<input type='text' id='W214'/></div><div class='labelCss'>浙东南<input type='text' id='W215'/></div><div class='labelCss'>浙西南<input type='text' id='W216'/></div></td>");
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='tabletitleOtherLeft'>实况分析</td>");
                sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W020' cols='35' rows='3'></textarea></td>");
                sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W120' cols='35' rows='3'></textarea></td>");
                sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W220' cols='35' rows='3'></textarea></td>");
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='tabletitleOtherLeft'>预报依据</td>");
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W030'  cols='35' rows='3'></textarea></td>"));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W130'  cols='35' rows='3'></textarea></td>"));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W230'  cols='35' rows='3'></textarea></td>"));
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='tabletitleOtherLeft'>污染预报</td>");
                sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W041'/></div><div class='labelCss'>江苏沿江<input type='text' id='W042'/></div><div class='labelCss'>上海<input type='text' id='W043'/></div><div class='labelCss'>浙北<input type='text' id='W014'/></div><div class='labelCss'>浙东南<input type='text' id='W045'/></div><div class='labelCss'>浙西南<input type='text' id='W046'/></div></td>");
                sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W141'/></div><div class='labelCss'>江苏沿江<input type='text' id='W142'/></div><div class='labelCss'>上海<input type='text' id='W143'/></div><div class='labelCss'>浙北<input type='text' id='W144'/></div><div class='labelCss'>浙东南<input type='text' id='W145'/></div><div class='labelCss'>浙西南<input type='text' id='W146'/></div></td>");
                sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W241'/></div><div class='labelCss'>江苏沿江<input type='text' id='W242'/></div><div class='labelCss'>上海<input type='text' id='W243'/></div><div class='labelCss'>浙北<input type='text' id='W244'/></div><div class='labelCss'>浙东南<input type='text' id='W245'/></div><div class='labelCss'>浙西南<input type='text' id='W246'/></div></td>");
                sb.AppendLine("</tr>");
            }
            else if (dt.Rows.Count == 2)
            {
                sb.AppendLine("<td class='tabletitleOtherLeft'>气象分析</td>");
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  cols='35' id='W000'  rows='3'>{0}</textarea></td>", dt.Rows[0]["qixLiveAnalyse24"].ToString()));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  cols='35'id='W100'  rows='3'>{0}</textarea></td>", dt.Rows[1]["qixLiveAnalyse24"].ToString()));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  cols='35' id='W200' rows='3'>{0}</textarea></td>", dt.Rows[1]["qixLiveAnalyse48"].ToString()));
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='tabletitleOtherLeft'>气象预报</td>");
                sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W011' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W012' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W013' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W014' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W015' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W016' value='{5}'></div></td>", dt.Rows[0]["QSuBei24"].ToString(), dt.Rows[0]["QjiangsuYj24"].ToString(), dt.Rows[0]["Qshanghai24"].ToString(), dt.Rows[0]["Qzhebei24"].ToString(), dt.Rows[0]["Qzhedongnan24"].ToString(), dt.Rows[0]["Qzhexinan24"].ToString()));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W111' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W112' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W113' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W114' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W115' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W116' value='{5}'></div></td>", dt.Rows[1]["QSuBei24"].ToString(), dt.Rows[1]["QjiangsuYj24"].ToString(), dt.Rows[1]["Qshanghai24"].ToString(), dt.Rows[1]["Qzhebei24"].ToString(), dt.Rows[1]["Qzhedongnan24"].ToString(), dt.Rows[1]["Qzhexinan24"].ToString()));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W211' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W212' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W213' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W214' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W215' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W216' value='{5}'></div></td>", dt.Rows[1]["QSuBei48"].ToString(), dt.Rows[1]["QjiangsuYj48"].ToString(), dt.Rows[1]["Qshanghai48"].ToString(), dt.Rows[1]["Qzhebei48"].ToString(), dt.Rows[1]["Qzhedongnan48"].ToString(), dt.Rows[1]["Qzhexinan48"].ToString()));
                sb.AppendLine("</tr>");
                sb.AppendLine("<td class='tabletitleOtherLeft'>实况分析</td>");
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W020' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteLiveAnalyse24"].ToString()));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W120'  cols='35' rows='3'>{0}</textarea></td>", dt.Rows[1]["polluteLiveAnalyse24"].ToString()));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'   id='W220' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[1]["polluteLiveAnalyse48"].ToString()));
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='tabletitleOtherLeft'>预报依据</td>");
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W030' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteCauseAnalyse24"].ToString()));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W130' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[1]["polluteCauseAnalyse24"].ToString()));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W230'  cols='35' rows='3'>{0}</textarea></td>", dt.Rows[1]["polluteCauseAnalyse48"].ToString()));
                sb.AppendLine("</tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='tabletitleOtherLeft'>污染预报</td>");
                sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W041' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W042' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W043' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W044' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W045' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W046' value='{5}'></div></td>", dt.Rows[0]["PSuBei24"].ToString(), dt.Rows[0]["PjiangsuYj24"].ToString(), dt.Rows[0]["Pshanghai24"].ToString(), dt.Rows[0]["Pzhebei24"].ToString(), dt.Rows[0]["Pzhedongnan24"].ToString(), dt.Rows[0]["Pzhexinan24"].ToString()));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W141' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W142' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W143' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W144' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W145' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W146' value='{5}'></div></td>", dt.Rows[1]["PSuBei24"].ToString(), dt.Rows[1]["PjiangsuYj24"].ToString(), dt.Rows[1]["Pshanghai24"].ToString(), dt.Rows[1]["Pzhebei24"].ToString(), dt.Rows[1]["Pzhedongnan24"].ToString(), dt.Rows[1]["Pzhexinan24"].ToString()));
                sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W241' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W242' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W243' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W244' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W245' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W246' value='{5}'></div></td>", dt.Rows[1]["PSuBei48"].ToString(), dt.Rows[1]["PjiangsuYj48"].ToString(), dt.Rows[1]["Pshanghai48"].ToString(), dt.Rows[1]["Pzhebei48"].ToString(), dt.Rows[1]["Pzhedongnan48"].ToString(), dt.Rows[1]["Pzhexinan48"].ToString()));
                sb.AppendLine("</tr>");
            }
            else if (dt.Rows.Count == 1)
            {
                if (DateTime.Parse(dt.Rows[0]["CommentTime"].ToString()).Date == DateTime.Parse(dateTime).Date)
                {
                    sb.AppendLine("<td class='tabletitleOtherLeft'>气象分析</td>");
                    sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W000' cols='35' rows='3'></textarea></td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W100' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["qixLiveAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W200' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["qixLiveAnalyse48"].ToString()));
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>气象预报</td>");
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W011'></div><div class='labelCss'>江苏沿江<input type='text' id='W012'></div><div class='labelCss'>上海<input type='text' id='W013'></div><div class='labelCss'>浙北<input type='text' id='W014'></div><div class='labelCss'>浙东南<input type='text' id='W015'></div><div class='labelCss'>浙西南<input type='text' id='W016'></div></td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W111' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W112' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W113' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W114' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W115' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W116' value='{5}'></div></td>", dt.Rows[0]["QSuBei24"].ToString(), dt.Rows[0]["QjiangsuYj24"].ToString(), dt.Rows[0]["Qshanghai24"].ToString(), dt.Rows[0]["Qzhebei24"].ToString(), dt.Rows[0]["Qzhedongnan24"].ToString(), dt.Rows[0]["Qzhexinan24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W211' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W212' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W213' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W214' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W215' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W216' value='{5}'></div></td>", dt.Rows[0]["QSuBei48"].ToString(), dt.Rows[0]["QjiangsuYj48"].ToString(), dt.Rows[0]["Qshanghai48"].ToString(), dt.Rows[0]["Qzhebei48"].ToString(), dt.Rows[0]["Qzhedongnan48"].ToString(), dt.Rows[0]["Qzhexinan48"].ToString()));
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>实况分析</td>");
                    sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea' id='W020' cols='35' rows='3'></textarea></td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W120' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteLiveAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W220' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteLiveAnalyse48"].ToString()));
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>预报依据</td>");
                    sb.AppendLine("<td class='tabletitleOther'><textarea class='textarea'  cols='33' id='W030'  rows='3'></textarea></td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W130'  cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteCauseAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W230'  cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteCauseAnalyse48"].ToString()));
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>污染预报</td>");
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W041'></div><div class='labelCss'>江苏沿江<input type='text' id='W042'></div><div class='labelCss'>上海<input type='text' id='W043'></div><div class='labelCss'>浙北<input type='text' id='W014'></div><div class='labelCss'>浙东南<input type='text' id='W045'></div><div class='labelCss'>浙西南<input type='text' id='W046'></div></td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W141' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W142' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W143' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W144' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W145' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W146' value='{5}'></div></td>", dt.Rows[0]["PSuBei24"].ToString(), dt.Rows[0]["PjiangsuYj24"].ToString(), dt.Rows[0]["Pshanghai24"].ToString(), dt.Rows[0]["Pzhebei24"].ToString(), dt.Rows[0]["Pzhedongnan24"].ToString(), dt.Rows[0]["Pzhexinan24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W241' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W242' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W243' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W244' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W245' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W246' value='{5}'></div></td>", dt.Rows[0]["PSuBei48"].ToString(), dt.Rows[0]["PjiangsuYj48"].ToString(), dt.Rows[0]["Pshanghai48"].ToString(), dt.Rows[0]["Pzhebei48"].ToString(), dt.Rows[0]["Pzhedongnan48"].ToString(), dt.Rows[0]["Pzhexinan48"].ToString()));
                    sb.AppendLine("</tr>");
                }
                else
                {
                    sb.AppendLine("<td class='tabletitleOtherLeft'>气象分析</td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea id='W000' cols='35' class='textarea' rows='3'>{0}</textarea></td>", dt.Rows[0]["qixLiveAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea  id='W100' cols='35' class='textarea' c rows='3'></textarea></td>"));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea  id='W200' cols='35' class='textarea'  rows='3'></textarea></td>"));
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>气象预报</td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W011' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W012' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W013' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W014' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W015' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W016' value='{5}'></div></td>", dt.Rows[0]["QSuBei24"].ToString(), dt.Rows[0]["QjiangsuYj24"].ToString(), dt.Rows[0]["Qshanghai24"].ToString(), dt.Rows[0]["Qzhebei24"].ToString(), dt.Rows[0]["Qzhedongnan24"].ToString(), dt.Rows[0]["Qzhexinan24"].ToString()));
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W111'></div><div class='labelCss'>江苏沿江<input type='text' id='W112'></div><div class='labelCss'>上海<input type='text' id='W113'></div><div class='labelCss'>浙北<input type='text' id='W114'></div><div class='labelCss'>浙东南<input type='text' id='W115'></div><div class='labelCss'>浙西南<input type='text' id='W116'></div></td>");
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W211'></div><div class='labelCss'>江苏沿江<input type='text' id='W212'></div><div class='labelCss'>上海<input type='text' id='W213'></div><div class='labelCss'>浙北<input type='text' id='W214'></div><div class='labelCss'>浙东南<input type='text' id='W215'></div><div class='labelCss'>浙西南<input type='text' id='W216'></div></td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>实况分析</td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea id='W020' class='textarea' cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteLiveAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W120' cols='35' rows='3'></textarea></td>"));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W220' cols='35' rows='3'></textarea></td>"));
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>预报依据</td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W030'  cols='35' rows='3'>{0}</textarea></td>", dt.Rows[0]["polluteCauseAnalyse24"].ToString()));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea' id='W130'  cols='35' rows='3'></textarea></td>"));
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><textarea class='textarea'  id='W230'  cols='35' rows='3'></textarea></td>"));
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td class='tabletitleOtherLeft'>污染预报</td>");
                    sb.AppendLine(string.Format("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W041' value='{0}'></div><div class='labelCss'>江苏沿江<input type='text' id='W042' value='{1}'></div><div class='labelCss'>上海<input type='text' id='W043' value='{2}'></div><div class='labelCss'>浙北<input type='text' id='W044' value='{3}'></div><div class='labelCss'>浙东南<input type='text' id='W045' value='{4}'></div><div class='labelCss'>浙西南<input type='text' id='W046' value='{5}'></div></td>", dt.Rows[0]["PSuBei24"].ToString(), dt.Rows[0]["PjiangsuYj24"].ToString(), dt.Rows[0]["Pshanghai24"].ToString(), dt.Rows[0]["Pzhebei24"].ToString(), dt.Rows[0]["Pzhedongnan24"].ToString(), dt.Rows[0]["Pzhexinan24"].ToString()));
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W141'></div><div class='labelCss'>江苏沿江<input type='text' id='W142'></div><div class='labelCss'>上海<input type='text' id='W143'></div><div class='labelCss'>浙北<input type='text' id='W144'></div><div class='labelCss'>浙东南<input type='text' id='W145'></div><div class='labelCss'>浙西南<input type='text' id='W146'></div></td>");
                    sb.AppendLine("<td class='tabletitleOther'><div class='labelCss'>苏北5市<input type='text' id='W241'></div><div class='labelCss'>江苏沿江<input type='text' id='W242'></div><div class='labelCss'>上海<input type='text' id='W243'></div><div class='labelCss'>浙北<input type='text' id='W244'></div><div class='labelCss'>浙东南<input type='text' id='W245'></div><div class='labelCss'>浙西南<input type='text' id='W246'></div></td>");
                    sb.AppendLine("</tr>");
                }

            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
    }
}
