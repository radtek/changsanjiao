using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
namespace MMShareBLL.DAL
{
    class UpateInteg
    {

        DateTime m_Time; string m_Filename; string m_Aimpath;
         string m_Sourceath;
         public UpateInteg(  string filename, string sourcepath,string aimpath)
         {
             this.m_Sourceath = sourcepath;
             this.m_Aimpath = aimpath;
             m_Filename = filename;
         }
        public string UpateData()
        {
           
            string[] arrar = m_Filename.Split('_');
            m_Time = Convert.ToDateTime(string.Format("{0}-{1}-{2}", arrar[2].Substring(0, 4), arrar[2].Substring(4, 2), arrar[2].Substring(6, 2)));
            string city = arrar[1];
            string sourcename = (string.Format("{0}/{1}12/majorcity/integ.{2}.{3}12.png", m_Sourceath, m_Time.ToString("yyyyMMdd"), city, m_Time.ToString("yyyyMMdd")));
            string aimname = (string.Format("{0}/{1}/{2}/integ_{3}_{4}2000_000.png", m_Aimpath, m_Time.ToString("yyyy"), m_Time.ToString("yyyyMMdd"), city, m_Time.ToString("yyyyMMdd")));
            try
            {
                FileInfo file = new FileInfo(sourcename);
                file.CopyTo(aimname,true);

            }
            catch { }
            return aimname;

        }
    }
}
