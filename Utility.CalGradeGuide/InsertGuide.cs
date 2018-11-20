using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using Readearth.Data;
using Readearth.DB;

namespace Utility.GradeGuide
{

  public  class InsertGuide
    {
        private Database m_Database;
        private DataTable m_Result;
        private DataTable HealthyStandard, HealthyMonths, HealthyGuidelines;//指引字典表
        private string m_Standard, m_Months, m_Guidelines;
        public InsertGuide(Database database, string tablestandard, string tablemonth, string tableguidelines)
        {
            m_Database = database;
            m_Standard = tablestandard;
            m_Months = tablemonth;
            m_Guidelines = tableguidelines;
            HealthyStandard = DTOperation.CreateVoidDT(m_Database, tablestandard);
            HealthyMonths = DTOperation.CreateVoidDT(m_Database, tablemonth);
            HealthyGuidelines = DTOperation.CreateVoidDT(m_Database, tableguidelines);
        }
        public InsertGuide(string tablestandard, string tablemonth, string tableguidelines)
        {
            m_Database = new Database();
            m_Standard = tablestandard;
            m_Months = tablemonth;
            m_Guidelines = tableguidelines;
            HealthyStandard = DTOperation.CreateVoidDT(m_Database, tablestandard);
            HealthyMonths = DTOperation.CreateVoidDT(m_Database, tablemonth);
            HealthyGuidelines = DTOperation.CreateVoidDT(m_Database, tableguidelines);

        }
        public void ToDB(DataTable result)
        {
            m_Result = result;
            string item = "";
            string months = "";
            string strf = "";
            string strt = "";
            int dmindex = 0, timeindex = 0;
            string guideLines1 = "";
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
            m_Database.Execute("delete from " + m_Guidelines);
            m_Database.Execute("delete from " + m_Standard);
            m_Database.Execute("delete from " + m_Months);
            DTOperation.InsertToDB(m_Database, HealthyGuidelines, m_Guidelines);
            DTOperation.InsertToDB(m_Database, HealthyStandard, m_Standard);
            DTOperation.InsertToDB(m_Database, HealthyMonths, m_Months);
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
