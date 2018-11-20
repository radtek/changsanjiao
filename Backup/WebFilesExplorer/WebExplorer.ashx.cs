using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using SharpZipLib;


namespace WebFilesExplorer
{
    /// <summary>
    /// WebExplorer 的摘要说明
    /// </summary>
    public class WebExplorer : IHttpHandler
    {

        public void ProcessRequest(HttpContext  Context)
        {
             Context.Response.Buffer = true;//互不影响
             Context.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(0);
             Context.Response.Expires = 0;
             Context.Response.AddHeader("Pragma","No-Cache");
            string action =  Context.Request["action"];//获取操作类型

                switch (action)
                {
                    case "LIST": ResponseList( Context); break;//获取文件列表
                    case "DOWNLOAD": DownFile( Context); break;//下载文件
                    case "GETEDITFILE": GetEditFileContent( Context); break;//从服务器读取文件内容
                    case "SAVEEDITFILE": SaveFile( Context, false); break;//保存已经编辑的文件 
                    case "NEWDIR": CreateDirectory( Context); break;//新建目录
                    case "NEWFILE": SaveFile( Context, true); break;//新建文件
                    case "DELETE": Delete( Context); break;//删除操作
                    case "COPY": CutCopy( Context,"copy"); break;//复制操作
                    case "CUT": CutCopy( Context, "cut"); break;//剪贴操作
                    case "UPLOAD": UpLoad( Context); break;//上传操作
                    case "RENAME": ReName( Context); break;//重名
                    case "ZIP": Zip( Context); break;//压缩文件
                    case "UNZIP": UnZip( Context); break;//解压缩
                    case "DOWNLOADS": DownLoads( Context); break;//下载多个文件

                }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #region 具体的操作实现过程
        /// <summary>
        /// 获取文件及文件夹路径列表
        /// </summary>
        /// <param name=" Context"></param>
        private void ResponseList(HttpContext  Context)
        {
            string value1 =  Context.Request["value1"];//获取参数    {key:value,}
            StringBuilder json = new StringBuilder("var GetList={\"Directory\":[",500);
            string path =  Context.Server.MapPath(value1);//获取要列举的物理路径

            string[] dir = Directory.GetDirectories(path);//获取指定路径下面所有文件夹 
            string[] files = Directory.GetFiles(path);//....
            foreach (string d in dir)
            {
                DirectoryInfo info = new DirectoryInfo(d);
                //{"Name":"Program","LastModify":"2012-08-08 12:12:49"}
                json.Append("{\"Name\":\""+info.Name+"\",\"LastModify\":\""+info.LastWriteTime+"\"},");
            }
            string tem = json.ToString();
            if (tem.EndsWith(","))//去掉最后一个尾巴 
            {
                tem = tem.Substring(0,tem.Length-1);
            }
            json = new StringBuilder(tem);
            json.Append("],\"File\":[");//接着拼接文件 

            foreach (string f in files)
            {
                FileInfo info = new FileInfo(f);
                string size = null;//换算单位 
                if (info.Length > 1024 * 1024)//M
                    size = ((double)info.Length / 1024 / 1024).ToString("F2") + "MB";
                else if (info.Length > 1024)
                    size = ((double)info.Length / 1024).ToString("F2") + "KB";
                else
                    size = info.Length.ToString() + "B";

                json.Append("{\"Name\":\"" + info.Name + "\",\"LastModify\":\""+info.LastWriteTime+"\"},");
            }
            tem = json.ToString();
            if (tem.EndsWith(","))//去掉最后一个尾巴 
            {
                tem = tem.Substring(0, tem.Length - 1);
            }
            json = new StringBuilder(tem);
            json.Append("]}");
            //输出JSON
             Context.Response.Write(json.ToString());
        }
        /// <summary>
        /// 下载文件操作
        /// </summary>
        /// <param name=" Context"></param>
        private void DownFile(HttpContext  Context)
        {
            string value1 =  Context.Request["value1"];
            string[] files = value1.Split('|');
            foreach (string item in files)
            {
                string path =  Context.Server.MapPath(item);//获取绝对路径
                if (File.Exists(path))
                    DownloadFile.ResponseFile(path,  Context,false);
            }
        }
        /// <summary>
        /// 从服务器读取文件内容
        /// </summary>
        /// <param name=" Context"></param>
        private void GetEditFileContent(HttpContext  Context)
        {
            string path =  Context.Server.MapPath( Context.Request["value1"]);
             Context.Response.Write(File.ReadAllText(path,Encoding.UTF8));
        }
        /// <summary>
        /// 保存已经编辑的文件 
        /// </summary>
        /// <param name=" Context"></param>
        
        private void SaveFile(HttpContext  Context,bool isNew)
        {//isNew-true:表示是新文件    false表示是修改操作
            string path =  Context.Server.MapPath( Context.Request["value1"]);
            if (isNew&File.Exists(path))
                return;
            string content =  Context.Request["content"];
            StreamWriter sw = File.CreateText(path);
            sw.Write(content);
            sw.Close();
             Context.Response.Write("OK");
        }
        /// <summary>
        /// 新建目录
        /// </summary>
        /// <param name=" Context"></param>
        private void CreateDirectory(HttpContext Context)
        {
            string path = Context.Request["value1"];
            Directory.CreateDirectory(Context.Server.MapPath(path));
            Context.Response.Write("OK");
        }
        /// <summary>
        /// 删除文件/目录操作
        /// </summary>
        /// <param name="Context"></param>
        private void Delete(HttpContext Context)
        {
            string[] files = Context.Request["value1"].Split('|');// 分割
            foreach (string item in files)
            {
                string path = Context.Server.MapPath(item);
                if (File.Exists(path))
                    File.Delete(path);
                else if (Directory.Exists(path))
                    Directory.Delete(path, false);
            }
            Context.Response.Write("OK");
        }
        /// <summary>
        /// 执行剪贴 复制 操作
        /// </summary>
        /// <param name="Context"></param>
        private void CutCopy(HttpContext Context,string flag)
        {
            string path = Context.Server.MapPath(Context.Request["value1"]);//请求的路径
            string[] files = Context.Request["value2"].Split('|');
            foreach (string item in files)
            {
                string p = Context.Server.MapPath(item);
                string fileName = Path.GetFileName(p);//获取文件名
                if (File.Exists(p))//如果是文件 
                    if (flag == "cut")
                        File.Move(p, path + fileName);
                    else
                        File.Copy(p, path + fileName);
                else if (Directory.Exists(p))
                    if (flag == "cut")
                        Directory.Move(p, path + fileName);
                    else
                        Direactory(p, path + fileName);
            }
            Context.Response.Write("OK");
        }
        /// <summary>
        /// 复制目录操作
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private void Direactory(string source, string target)
        {
            if (!target.StartsWith(source, StringComparison.CurrentCultureIgnoreCase))
            {
                DirectoryInfo s = new DirectoryInfo(source);//源
                DirectoryInfo t = new DirectoryInfo(target);//目的地
                t.Create();// 目录创建完毕 
                //连同文件也复制过去
                FileInfo[] sFiles = s.GetFiles();
                for (int i = 0; i < sFiles.Length; i++)
                {
                    File.Copy(sFiles[i].FullName, t.FullName + "\\" + sFiles[i].Name,true);
                }
                //连同子目录也复制过去
                DirectoryInfo[] ds = t.GetDirectories();
                for (int i = 0; i < ds.Length; i++)
                {
                    Direactory(ds[i].FullName, t.FullName + "\\" + ds[i].Name);
                }
            }

        }
        /// <summary>
        /// 压缩文件 
        /// </summary>
        /// <param name="Context"></param>
        private void Zip(HttpContext Context)
        {
            string zipFile = Context.Server.MapPath(Context.Request["value1"]);
            string[] fd = Context.Request["value2"].Split('|');
            List<string> files = new List<string>();
            List<string> dirs = new List<string>();
            //将要压缩的文件或者文件夹全部存储到集合中
            foreach (string item in fd)
            {
                string p = Context.Server.MapPath(item);
                if (File.Exists(p))
                    files.Add(p);
                else if (Directory.Exists(p))
                    dirs.Add(p);
            }
            ZipClass.Zip(Path.GetDirectoryName(zipFile) + "\\", zipFile, "", true, files.ToArray(), dirs.ToArray());
            Context.Response.Write("OK");
        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="Context"></param>
        private void UnZip(HttpContext Context)
        {
            string unZipDir = Context.Server.MapPath(Context.Request["value1"]);
            string[] zipFiles = Context.Request["value2"].Split('|');
            foreach (string item in zipFiles)
            {
                ZipClass.UnZip(Context.Server.MapPath(item), unZipDir, "");
            }
            Context.Response.Write("OK");
        }
        /// <summary>
        /// 上传 
        /// </summary>
        /// <param name="Context"></param>
        private void UpLoad(HttpContext Context)
        {
            string path = Context.Server.MapPath(Context.Request["value1"]);
            HttpFileCollection files = Context.Request.Files;
            long allSize = 0;
            for (int i = 0; i < files.Count; i++)
            {
                allSize += files[i].ContentLength;
            }
            if (allSize > 20 * 1024 * 1024)
                Context.Response.Write("文件大小超过限制");
            for (int i = 0; i < files.Count; i++)
            {
                files[i].SaveAs(path + Path.GetFileName(files[i].FileName));
            }
            Context.Response.Write("OK");
        }
        /// <summary>
        /// 下载多个文件
        /// </summary>
        /// <param name="Context"></param>
        private void DownLoads(HttpContext Context)
        {
            string zipFile = Context.Server.MapPath("#download.zip");
            string[] fd = Context.Request["value1"].Split('|');
            List<string> files = new List<string>();
            List<string> dirs = new List<string>();
            foreach (string item in fd)
            {
                string p = Context.Server.MapPath(item);
                if (File.Exists(p))
                    files.Add(p);
                else if (Directory.Exists(p))
                    dirs.Add(p);
            }
            ZipClass.Zip(Path.GetDirectoryName(zipFile)+"\\",zipFile,"",true,files.ToArray(),dirs.ToArray());
            DownloadFile.ResponseFile(zipFile,Context,true);
        }
        /// <summary>
        /// 重命名操作
        /// </summary>
        /// <param name="Context"></param>
        private void ReName(HttpContext Context)
        {
            string oldName = Context.Server.MapPath(Context.Request["value1"]);
            string newName = Context.Server.MapPath(Context.Request["value2"]);
            File.Move(oldName, newName);
            Context.Response.Write("OK");
        }
        #endregion
    }
}