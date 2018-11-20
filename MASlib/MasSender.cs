using System;
using System.Collections.Generic;
using System.Text;

using ImApiDotNet; 
using System.Configuration;

namespace MASlib
{
    public class MasSender
    {        
        //用于记录系统错误日志
        protected static readonly log4net.ILog m_Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ImApiDotNet.APIClient m_APIClient;
        int m_Con;
        public MasSender()
        {
            try
            {

                //获取连接数据库的参数
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["MAS"];
                m_APIClient = new APIClient();
                string constring = settings.ConnectionString;
                string[] parts = constring.Split(new char[] { ';', '=' }, StringSplitOptions.None);


                m_Con = m_APIClient.init(parts[1], parts[7], parts[9], parts[11], parts[3]);
                if (m_Con != 0)
                {
                    m_APIClient.release();
                    m_APIClient = null;
                }
            }
            catch (Exception ex)
            {
                m_Log.Error("MasSender", ex);

            }
        }

        public int SendSM(string[] mobiles, string content)
        {
            if (m_APIClient != null)
                return m_APIClient.sendSM(mobiles, content, "", 10, 10);
            else
                return m_Con;
        }


        public void Relese()
        {
            if (m_APIClient != null)
            {
                m_APIClient.release();
                m_APIClient = null;
            }
        }

    }
}
