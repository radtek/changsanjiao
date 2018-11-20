using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using ChinaAirUtility;
using Readearth.Data;

namespace DealHealthyGuide
{
    class Program
    {
        private static Database m_Database;
        static void Main(string[] argsS)
        {
            string[] args = { " {} ", "{\"DataBasePath\":\"F:/EMFCDatabase/\",\"UpdateTime\":\"2015/1/10 19:00:00\",\"ConnectionString\":\"Data Source=10.228.177.62;Initial Catalog=EMFCShare;Persist Security Info=True;User ID=sa;Password=Diting2015\"}" };
            ResolvePar resolvePar = new ResolvePar(args);
            DateTime dtnow = DateTime.Now.AddDays(0);
            DateTime dttime = Convert.ToDateTime(dtnow.AddDays(-1).ToString("yyyy-MM-dd 20:00:00"));
            string ConnectionString = resolvePar.TryGetValue("ConnectionString");
            m_Database = new Database(ConnectionString);

            //处理08预报等级数据
            SceneryGrade SceneryGrade = new SceneryGrade(m_Database);
            dttime = Convert.ToDateTime(dtnow.ToString("yyyy-MM-dd 08:00:00"));
            SceneryGrade.InsertData(dttime);

            //处理20预报等级数据
            HealthyGrade HealthyGrade = new HealthyGrade(m_Database);
            HealthyGrade.InsertData(dttime, "WRF");
            //HealthyGrade.InsertAscData(dttime, "WRF");
            //处理20预报指引数据
            HealthyGuide HealthyGuide = new HealthyGuide(m_Database);
            HealthyGuide.InsertData(dttime, "WRF");

            //处理实况等级数据
            dttime = Convert.ToDateTime(dtnow.ToString("yyyy-MM-dd 20:00:00"));
            HealthyGrade.InsertAaData(dttime);

            //处理08预报等级数据
            dttime = Convert.ToDateTime(dtnow.ToString("yyyy-MM-dd 08:00:00"));
            HealthyGrade.InsertData(dttime, "WRFS");
            //处理08预报指引数据
            HealthyGuide.InsertData(dttime, "WRFS");
            //HealthyGrade.InsertAscData(dttime, "WRFS");

            //处理08订正等级数据
            HealthyGrade.InsertTData(dttime, "CITYF");
            //处理08订正指引数据
            HealthyGuide.InsertTData(dttime, "CITYF");

        }
    }
}
