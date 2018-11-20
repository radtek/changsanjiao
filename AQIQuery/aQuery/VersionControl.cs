using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AQIQuery.aQuery
{
    public class VersionControl
    {
        // <summary>
        /// 根据操作系统名称（OSName）获取全部的更新项目名称和版本，版本号为保留两位小数的实数
        /// 用到了存储过程：[VersionControl_Get_byOSName]
        /// </summary>
        /// 
        /// <returns>返回DataTable</returns>
        /// 日期：2014-4-24日
        public static DataTable GetVersionListbyOSName(string OSName)
        {
            return AQIQuery.DAL.VersionControl.GetVersionListbyOSName(OSName);
        }
    }
}
