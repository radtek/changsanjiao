using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using Readearth.Data;
using Readearth.DB1;

namespace Utility1.GradeGuide1
{

  public  class InsertGuide1
    {
        private Database m_Database;
        private DataTable m_Result;
        private DataTable HealthyStandard, HealthyMonths, HealthyGuidelines;//指引字典表
        private string m_Standard, m_Months, m_Guidelines;
        public InsertGuide1(Database database, string tablestandard, string tablemonth, string tableguidelines)
        {
            m_Database = database;
            m_Standard = tablestandard;
            m_Months = tablemonth;
            m_Guidelines = tableguidelines;
            HealthyStandard = DTOperation1.CreateVoidDT(m_Database, tablestandard);
            HealthyMonths = DTOperation1.CreateVoidDT(m_Database, tablemonth);
            HealthyGuidelines = DTOperation1.CreateVoidDT(m_Database, tableguidelines);
        }
        public InsertGuide1(string tablestandard, string tablemonth, string tableguidelines)
        {
            m_Database = new Database();
            m_Standard = tablestandard;
            m_Months = tablemonth;
            m_Guidelines = tableguidelines;
            HealthyStandard = DTOperation1.CreateVoidDT(m_Database, tablestandard);
            HealthyMonths = DTOperation1.CreateVoidDT(m_Database, tablemonth);
            HealthyGuidelines = DTOperation1.CreateVoidDT(m_Database, tableguidelines);

        }
        public List<string> ToDB(DataTable result)
        {
            m_Result = result;
            string item = "";
            string months = "";
            string strf = "";
            string strt = "";
            int dmindex = 0, timeindex = 0;
            string guideLines1 = "";
            List<string> messageList = new List<string>(); 
            //新加入dtDataTable，方便进行月份的判断
            //总共5列：第一列主键为总共的月份个数，第二列为月份，第三列为范围起始值，第四列范围终止值，第五列表示类型
            DataTable dtDataTable = new DataTable();
            for (int i = 0; i < 5; i++)
            {
                string columnName = "第" + i + "列";
                DataColumn dataColumn = new DataColumn();
                dtDataTable.Columns.Add(columnName);
            }
            //////////

            foreach (DataRow dr in m_Result.Rows)
            {
                string strtpf = dr[1].ToString();
                string strtpt = dr[2].ToString();

                string tpitem = dr[0].ToString();
                string tpmonths = dr[3].ToString();
                string tpguideLines1 = dr[4].ToString();
                string type = dr[5].ToString();
                string guideLines2 = dr[6].ToString();

                if (tpitem != "")
                {
                    item = tpitem;

                }
                if (tpguideLines1 != "")
                {
                    guideLines1 = tpguideLines1;
                    if (tpguideLines1 == "无")
                        guideLines1 = "";
                }
                if (guideLines2 == "无")
                    guideLines2 = "";

                if (strtpf + strtpt != "")
                {
                    dmindex++;
                    strf = strtpf == "" ? "-9999" : strtpf;
                    strt = strtpt == "" ? "9999" : strtpt;
                    //if (strtpf.Contains("[") && strtpt != "")
                    //    strtpf = strtpf.Replace("[", "") + "≤";
                    //else if (strtpf.Contains("["))
                    //    strtpf = "≥" + strtpf.Replace("[", "");
                    //else if (strtpf != "" && strtpt != "")
                    //    strtpf = ">" + strtpf;

                    //if (strtpt.Contains("]"))
                    //    strtpt = "≤" + strtpt.Replace("]", "");
                    //else if (strtpt != "")
                    //    strtpt = "<" + strtpt;

                    if (strtpf.Contains("["))
                        strtpf = strtpf.Replace("[", "") + "≤";
                    else if (strtpf != "")
                        strtpf = strtpf + "<";

                    if (strtpt.Contains("]"))
                        strtpt = "≤" + strtpt.Replace("]", "");
                    else if (strtpt != "")
                        strtpt = "<" + strtpt;


                    DataRow newdr = HealthyStandard.NewRow();
                    newdr[0] = dmindex.ToString().PadLeft(2, '0');
                    newdr[1] = strtpf + item + strtpt;
                    newdr[3] = strf;
                    newdr[4] = strt;
                    newdr[5] = item;
                    HealthyStandard.Rows.Add(newdr);
                }
                if (tpmonths != "")
                {
                    timeindex++;
                    months = tpmonths;
                    DataRow newdr = HealthyMonths.NewRow();
                    newdr[0] = timeindex.ToString().PadLeft(2, '0');
                    newdr[1] = ConvertMonths( months);
                    newdr[3] = item;
                    HealthyMonths.Rows.Add(newdr);


                    //////////新加一张表/////用于判断范围与月份是否准确////
                    DataRow newdr1 = dtDataTable.NewRow();
                    newdr1[0] = timeindex.ToString().PadLeft(2, '0');
                    newdr1[1] = ConvertMonths(months);
                    newdr1[2] = strf;
                    newdr1[3] = strt;
                    newdr1[4] = item;
                    dtDataTable.Rows.Add(newdr1);
                    //////////////////////////////////////////////////////////////////
                }
                DataRow row = HealthyGuidelines.NewRow();
                row[0] = item;
                row[1] = dmindex.ToString().PadLeft(2, '0');
                row[2] = timeindex.ToString().PadLeft(2, '0');
                row[3] = guideLines1;
                row[4] = type;
                row[5] = guideLines2;
                HealthyGuidelines.Rows.Add(row);
            }

            //////////////////////将错误写入日志文件
            CheckError1 checkError = new CheckError1();
            //检查数值范围
            //checkError.Range(HealthyStandard);
            string error = checkError.Range1(HealthyStandard);
            if (error!="")
                messageList.Add(error);
            //月份问题检查
            error = checkError.Month(dtDataTable);
            if (error != "")
                messageList.Add(error);
          
            ///////////////////////////////////////////
            if (messageList.Count == 0)
            {
                m_Database.Execute("delete from " + m_Guidelines);
                m_Database.Execute("delete from " + m_Standard);
                m_Database.Execute("delete from " + m_Months);
                DTOperation1.InsertToDB(m_Database, HealthyGuidelines, m_Guidelines);
                DTOperation1.InsertToDB(m_Database, HealthyStandard, m_Standard);
                DTOperation1.InsertToDB(m_Database, HealthyMonths, m_Months);
            }
            return messageList;
        }
        private string ConvertMonths(string month)
        { 
            string months="";
            string [] array=month.Split(',');
            foreach (string var in array)
            {
                months = months + Convert.ToInt32(var).ToString().PadLeft(2, '0') + ",";
            }
            return months.Remove(months.Length -1);
        }

    }
}
