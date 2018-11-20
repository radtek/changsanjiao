using System;
using System.Collections.Generic;
using System.Text;

using Readearth.Data;
namespace MMShareBLL.DAL
{
    class Utility
    {
        public static void InsertLog(User user,string ip, string operatorContext)
        {
            LogInfo logInfo = new LogInfo();
            logInfo.OperatorContext = operatorContext;
            logInfo.UserName = user.ID;
            logInfo.IP = ip;

            Database db=new Database();
            Log log = new Log(db);
            log.AddLog(logInfo);

        }
    }
}
