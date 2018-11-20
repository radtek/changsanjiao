using System;
using System.Collections.Generic;
using System.Text;
using MMShareBLL.DAL;


namespace MMShareBLL.BLL
{
    public class SystemVariable
    {

        private static string _default_systemName = "长三角区域空气质量预报业务平台";
        private static bool _default_enabledCache = true;

        private static string systemName;

        public static bool Inited = false;

        /// <summary>
        /// 系统名称
        /// </summary>
        public static string SystemName
        {
            get { return systemName; }
            set { systemName = value; }
        }

        private static bool enabledCache;

        /// <summary>
        /// 是否开启应用程序级缓存
        /// </summary>
        public static bool EnabledCache
        {
            get { return enabledCache; }
            set { enabledCache = value; }
        }

        static SystemVariable()
        {
            try
            {
                SysetmConfig configDal = new SysetmConfig();
                object[] config = configDal.GetSystemVariable();
                if (config.Length >= 1 && null != config[0])
                    systemName = config[0].ToString();
                else
                    systemName = _default_systemName;

                if (config.Length >= 2 && null != config[1])
                {
                    bool enabled = true;
                    if (Boolean.TryParse(config[1].ToString(), out enabled))
                        enabledCache = enabled;
                    else
                        enabledCache = _default_enabledCache;
                }
                else
                    enabledCache = _default_enabledCache;
                Inited = true;
            }
            catch { }
        }

        public static void SetCacheEnabeld(bool enabled)
        {
            SysetmConfig configDal = new SysetmConfig();
            try
            {
                configDal.UpdateCacheEnabled(enabled);
                enabledCache = enabled;
            }
            catch { throw; }
        }

        public static void Init()
        {
        }
    }
}
