<%@ WebHandler Language="C#" Class="WebExplorers" %>

using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.IO;
using SharpZipLib;

public class WebExplorers : IHttpHandler {

    private Readearth.Data.Database m_Database;
    public void ProcessRequest(HttpContext Context)
    {
        m_Database = new Readearth.Data.Database();
        Context.Response.Buffer = true;//互不影响
        Context.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(0);
        Context.Response.Expires = 0;
        Context.Response.AddHeader("Pragma", "No-Cache");
        string action = Context.Request["action"];//获取操作类型

        switch (action)
        {
            case "DOWNLOAD": DownFile(Context); break;//下载文件
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

    private System.Data.DataSet GetXML(string path)
    {
        try
        {
            path = path.Replace("ChangjiangData", "CJDATA");
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.ReadXml(path);
            return ds;
        }
        catch { }
        return null;
    }
    public  long GetDirectoryLength(string dirPath)
    {
        //判断给定的路径是否存在,如果不存在则退出
        if (!Directory.Exists(dirPath))
            return 0;
        long len = 0;

        //定义一个DirectoryInfo对象
        DirectoryInfo di = new DirectoryInfo(dirPath);

        //通过GetFiles方法,获取di目录中的所有文件的大小
        foreach (FileInfo fi in di.GetFiles())
        {
            len += fi.Length;
        }

        //获取di中所有的文件夹,并存到一个新的对象数组中,以进行递归
        DirectoryInfo[] dis = di.GetDirectories();
        if (dis.Length > 0)
        {
            for (int i = 0; i < dis.Length; i++)
            {
                len += GetDirectoryLength(dis[i].FullName);
            }
        }
        return len;
    }


    
    /// <summary>
    /// 获取文件及文件夹路径列表
    /// </summary>
    /// <param name=" Context"></param>
    private void ResponseList(HttpContext Context)
    {
        string value1 = Context.Request["value1"];//获取参数    {key:value,}
        StringBuilder json = new StringBuilder("var GetList={\"Directory\":[", 500);
        string path = Context.Server.MapPath(value1);//获取要列举的物理路径
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        System.Data.DataSet ds = GetXML(Context.Server.MapPath("cfg.xml"));
        string[] dir = Directory.GetDirectories(path);//获取指定路径下面所有文件夹 
        string[] files = Directory.GetFiles(path);//....
        foreach (string d in dir)
        {
            DirectoryInfo info = new DirectoryInfo(d);
            long len=GetDirectoryLength(d);
            string lengths = "0";
            if (len > 1024 * 1024)//M
                lengths = ((double)len / 1024 / 1024).ToString("F2") + "MB";
            else if (len > 1024)
                lengths = ((double)len / 1024).ToString("F2") + "KB";
            else
                lengths = len.ToString() + "B";
            //{"Name":"Program","LastModify":"2012-08-08 12:12:49"}
            json.Append("{\"Name\":\"" + info.Name + "\",\"LastModify\":\"" + info.LastWriteTime + "\",\"FileSize\":\"" + lengths + "\"},");
        }
        string tem = json.ToString();
        if (tem.EndsWith(","))//去掉最后一个尾巴 
        {
            tem = tem.Substring(0, tem.Length - 1);
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

            string createTime = "未知";
            string creaters = "未知";
            string filePath = info.FullName;
            if (ds != null && ds.Tables.Count > 0)
            {
                System.Data.DataRow[] rows = ds.Tables[0].Select("filePath='" + filePath + "'");
                if (rows != null && rows.Length > 0) {
                    creaters = rows[0]["creater"].ToString();
                    createTime = rows[0]["createTime"].ToString();
                }
            }
            json.Append("{\"Name\":\"" + info.Name + "\",\"LastModify\":\"" + info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"Size\":\"" + size + "\",\"createTime\":\"" + createTime + "\",\"creaters\":\"" + creaters + "\"},");
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


    private void ResponseListII(HttpContext Context)
    {
        string value1 = "~/CJDATA/预报简报";// Context.Request["value1"];//获取参数    {key:value,}
        StringBuilder json = new StringBuilder("var GetList={\"Directory\":[", 500);
        string path = Context.Server.MapPath(value1);//获取要列举的物理路径
        string[] strs = { "安徽", "江苏", "上海", "浙江" };
        string[] files = Directory.GetFiles(path);//....
        string tem = json.ToString();
        if (tem.EndsWith(","))//去掉最后一个尾巴 
        {
            tem = tem.Substring(0, tem.Length - 1);
        }
        System.Data.DataSet ds = GetXML(Context.Server.MapPath("cfg.xml"));
        
        json = new StringBuilder(tem);
        json.Append("],\"File\":[");//接着拼接文件 
        foreach (string str in strs)
        {
            string  paths = path + @"\" + str + @"\" + DateTime.Now.ToString("yyyy-MM-dd");
            if (!Directory.Exists(paths))
                Directory.CreateDirectory(paths);

            files = Directory.GetFiles(paths);
            
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

                string createTime = "未知";
                string creater = "未知";
                string filePath = info.FullName;
                if (ds != null && ds.Tables.Count > 0)
                {
                    System.Data.DataRow[] rows = ds.Tables[0].Select("filePath='" + filePath + "'");
                    if (rows != null && rows.Length > 0)
                    {
                        creater = rows[0]["creater"].ToString();
                        createTime = rows[0]["createTime"].ToString();
                    }
                }
                json.Append("{\"Name\":\"" + info.Name + "\",\"LastModify\":\"" + info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"Size\":\"" + size + "\",\"createTime\":\"" + createTime + "\",\"creaters\":\"" + creater + "\"},");
            }
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
    private void DownFile(HttpContext Context)
    {
        string value1 = Context.Request["value1"];
        string path = value1;// Context.Server.MapPath(value1);//获取绝对路径
            if (File.Exists(path))
              ResponseFile(path, Context, false);
        
    }


    public  void ResponseFile(string path, HttpContext context, bool hasfileName)
    {
        string fileName;
        context = HttpContext.Current;
        Stream stream = null;
        byte[] buffer = new byte[0x2710];
        if (!hasfileName)
        {
            fileName = Path.GetFileName(path);
        }
        else
        {
            fileName = "down_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip";
        }
        try
        {
            stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            long length = stream.Length;
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
            while (length > 0L)
            {
                if (context.Response.IsClientConnected)
                {
                    int count = stream.Read(buffer, 0, 0x2710);
                    context.Response.OutputStream.Write(buffer, 0, count);
                    context.Response.Flush();
                    buffer = new byte[0x2710];
                    length -= count;
                }
                else
                {
                    length = -1L;
                }
            }
        }
        catch (Exception exception)
        {
            context.Response.Write(exception.Message);
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
    }



    private string GETUSER(HttpContext Context)
    {
        Readearth.Data.Database m_Database = new Readearth.Data.Database();
        try
        {
            string userName = Context.Request.Cookies["User"]["name"].ToString();
            string strSQL = "SELECT UserName,Alias FROM WEMCShare.dbo.T_User WHERE UserName='" + userName + "'";
            System.Data.DataTable dt = m_Database.GetDataTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                string LoginName = dt.Rows[0][1].ToString();
                return LoginName;
            }
        }
        catch { }
        return "未知";
    }
    
    private string  GetLen(long len) {
        string size = null;//换算单位 
        if (len > 1024 * 1024)//M
            size = ((double)len / 1024 / 1024).ToString("F2") + "MB";
        else if (len > 1024)
            size = ((double)len / 1024).ToString("F2") + "KB";
        else
            size = len.ToString() + "B";

        return size;
    }

    private void DownFileII(HttpContext Context)
    {
        string value1 = Context.Request["value1"];
        string[] files = value1.Split('|');
        foreach (string item in files)
        {
            string[] areas = { "安徽", "江苏", "上海", "浙江" };
            foreach (string area in areas)
            {
                string path = Context.Server.MapPath(item);//获取绝对路径
                FileInfo info=new FileInfo(path.Split('#')[0]);
                string fileSize = path.Split('#')[1];
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string name = "预报简报/" + area + "/" + date+"/"+info.Name ;
                string filePath = Context.Server.MapPath(name).Replace("ChangjiangData", "CJDATA");
                FileInfo finfo = new FileInfo(filePath);
                try
                {
                    if (GetLen(finfo.Length) == fileSize)
                    {
                        //if (File.Exists(filePath))
                        //{
                        DownloadFile.ResponseFile(filePath, Context, false);
                            return;
                       // }
                    }
                }
                catch { }
            }
        }
    }


    private string urlconvertor(HttpContext Context,string imagesurl1)
    {
        FileInfo info = new FileInfo(imagesurl1);
        string tmpRootDir = Context.Server.MapPath(info.Name);//获取程序根目录
        string imagesurl2 = imagesurl1.Replace(tmpRootDir, ""); //转换成相对路径
        imagesurl2 = imagesurl2.Replace(@"\", @"/");
        int index=imagesurl2.IndexOf("CJDATA");
        //imagesurl2=imagesurl2.Substring(index, imagesurl2.Length - index);
        //imagesurl2 = imagesurl2.Replace(@"Aspx_Uc/", @"");
        imagesurl2 = imagesurl2.Replace(@"/", @"\");
        //~/CJDATA/
        return ("~/"+imagesurl2);
    }
    
    /// <summary>
    /// 从服务器读取文件内容
    /// </summary>
    /// <param name=" Context"></param>
    private void GetEditFileContent(HttpContext Context)
    {
        //string path = Context.Server.MapPath(Context.Request["value1"]);
        //Context.Response.Write(File.ReadAllText(path, Encoding.UTF8));
        //DownloadFile.ResponseFile(path, Context, false);
        DownFile(Context);
    }
    /// <summary>
    /// 保存已经编辑的文件 
    /// </summary>
    /// <param name=" Context"></param>

    private void SaveFile(HttpContext Context, bool isNew)
    {//isNew-true:表示是新文件    false表示是修改操作
        string path = Context.Server.MapPath(Context.Request["value1"]);
        if (isNew & File.Exists(path))
            return;
        string content = Context.Request["content"];
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
        string sql = "delete from  T_ChangjiangData  where ID='" + Context.Request["value2"] + "'";
        m_Database.Execute(sql);
        Context.Response.Write("OK");
    }
    /// <summary>
    /// 执行剪贴 复制 操作
    /// </summary>
    /// <param name="Context"></param>
    private void CutCopy(HttpContext Context, string flag)
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
                File.Copy(sFiles[i].FullName, t.FullName + "\\" + sFiles[i].Name, true);
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
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        
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
        //插入数据库//I:\new\news\SEMCShares\WebUI\CJDATA\会商PPT\上海\2015-01-22\
        string type = "";// path.Split('\\')[6].ToString();
        string area = "";// path.Split('\\')[7].ToString();
        string fileName = Path.GetFileName(files[0].FileName);
        string fileSize = GetLen(allSize);
        string uploadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string uploadUser = GETUSER(Context);
        type = "会商PPT";
        if(path.IndexOf("预报简报")>=0)
            type = "预报简报";
        if (path.IndexOf("超级站") >= 0)
            type = "超级站";
        if (path.IndexOf("一周回顾") >= 0)
            type = "一周回顾";

        area = "安徽";
        if (path.IndexOf("上海") >= 0)
            area = "上海";
        if (path.IndexOf("浙江") >= 0)
            area = "浙江";
        if (path.IndexOf("江苏") >= 0)
            area = "江苏";

        string filePath = "~/CJDATA/" + type + "/"+area+"/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + Path.GetFileName(files[0].FileName);
        string sql = "insert into T_ChangjiangData(type,area,fileName,fileSize,uploadTime,uploadUser,filepath)"+
                    " values('" + type + "','" + area + "','" + fileName + "','" + fileSize + "','" + uploadTime + "','" + uploadUser + "','" + filePath + "')";
         m_Database.Execute(sql);
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
        ZipClass.Zip(Path.GetDirectoryName(zipFile) + "\\", zipFile, "", true, files.ToArray(), dirs.ToArray());
        DownloadFile.ResponseFile(zipFile, Context, true);
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

        string sql = "update T_ChangjiangData set fileName='" + Path.GetFileName(newName) + "',filepath='" + Context.Request["value2"] + "' where filePath='" + Context.Request["value1"] + "'";
        m_Database.Execute(sql);
        
        Context.Response.Write("OK");
    }
    #endregion

}