using System;
using System.Collections.Generic;
using System.Text;

namespace AQIQuery.aQuery
{
    public class DataUser
    {
        /// <summary>
        /// 区县用户使用，根据用户名和密码获取可以访问的GroupID权限。
        /// 用到了存储过程：[DataUser_Get_byUserandPassword]
        /// </summary>
        /// <returns>返回GroupID,如果返回0，则失败</returns>
        /// 日期：2014-4-24日
        public static int GetAuthorizedGroupID(string UserName, string Password)
        {
            return AQIQuery.DAL.DataUser.GetAuthorizedGroupID(UserName, Password);
        }
 
    }
}
