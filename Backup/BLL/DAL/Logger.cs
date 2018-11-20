using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
namespace MMShareBLL.DAL
{
    class Logger
    {
        private int m_Count = 0;
        /// <summary>
        /// 将错误信息写入日志文件。
        /// </summary>
        /// <param name="message">错误信息</param>
        public void WriteToLog(string message, string errorType)
        {
            string folder = string.Empty;
            string log = string.Empty;
            if (errorType == "error")
            {
                folder = System.Web.HttpContext.Current.Server.MapPath("") + "\\RecordLog";
                log = folder + "\\Error" + DateTime.Today.ToString("yyyyMMdd") + ".log"; 
            }
            else
            {
                folder = System.Web.HttpContext.Current.Server.MapPath("") + "\\RecordLog";
                log = folder + "\\Record" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            }

            //检查并创建文件夹
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            //检查并创建错误日志
            if (!File.Exists(log))
            {
                File.Create(log).Close();
            }

            try
            {

                //将错误信息写入日志
                using (StreamWriter sw = File.AppendText(log))
                {
                    sw.WriteLine("Time: {0}", DateTime.Now.ToString());
                    sw.WriteLine(message);
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                }
            }
            catch
            {
                m_Count++;
                while (m_Count > 3) return;
                //如果存在文件共享冲突，重新执行此函数。
                WriteToLog(message, errorType);
            }
        }
    }
}
