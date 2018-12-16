using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace weixin_api.HeathWearth
{
    public partial class LifeIndex_LifeIndex : System.Web.UI.Page
    {
        public static string comPath;


        protected void Page_Load(object sender, EventArgs e)
        {
            comPath = ConfigurationManager.AppSettings["LiveIndex"];
        }

       [WebMethod]
        public static string GetLiveIndexData()
        {
            string res = "";
            try
            {
                //获取当前系统时间
                DateTime nowTime = DateTime.Now;
                if (Directory.Exists(comPath))
                {
                    //获取文件夹路径
                    DirectoryInfo dir = new DirectoryInfo(comPath);
                    //获取文件夹下的文件，并根据条件进行筛选出最新的文件
                    string files = dir.GetFiles("index_weixin_*.txt").OrderByDescending(x => x.Name).ToList()[0].ToString();
                    //拼接路径
                    string pathName = Path.Combine(comPath, files);
                    string name = files.Split('_')[2].Replace(".txt", "")+"00";
                    nowTime = DateTime.ParseExact(name, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    //解析文件
                    string[] strArr = { "7", "8", "9", "15", "16", "17", "4", "5", "6", "1", "2", "3", "10", "11", "12", "13", "14" };

                    for (int i = 0; i < strArr.Length; i++)
                    {
                        string arr = strArr[i];
                        //读取文件中数据相应的行
                        StreamReader sr = new StreamReader(pathName, Encoding.GetEncoding("gb2312"));
                        String line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            //分割字符串输出字符数组
                            string[] lineArr = Regex.Split(line, "  ", RegexOptions.IgnoreCase);
                            Regex reg = new Regex(arr);
                            if (lineArr[0] == arr)
                            {
                                res = res + line + ";";
                            }
                        }

                    }
                }
                else {
                    return comPath;
                }
                string fontShowTime = nowTime.ToString("yyyy") + "年" + nowTime.ToString("MM") + "月" + nowTime.ToString("dd") + "日 "+nowTime.ToString("HH")+"时发布";
                res = res + fontShowTime;
                res = res.Replace("晨练指数", "学生户外活动指数(早晨)").Replace("户外晚间锻炼指数", "学生户外活动指数(晚上)").Replace("学生户外活动指数", "户外活动指数");
                return res;
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(e.Message);
                throw e;
            }
        }
    }
}