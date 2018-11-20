<%@ WebHandler Language="C#" Class="WebExplorerss" %>

using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Collections.Specialized;
using SharpZipLib;
using Aspose.Cells;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility.GradeGuide;
using System.Collections;
using System.Text.RegularExpressions;

public class WebExplorerss : IHttpHandler {

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
            case "LIST": ResponseList(Context); break;//获取文件列表
            case "LISTII": ResponseListII(Context); break;//获取文件列表
            case "DOWNLOAD": DownFile(Context); break;//下载文件
            case "DOWNLOADII": DownFileII(Context); break;//下载文件
            case "GETEDITFILE": GetEditFileContent(Context); break;//从服务器读取文件内容
            case "SAVEEDITFILE": SaveFile(Context, false); break;//保存已经编辑的文件 
            case "NEWDIR": CreateDirectory(Context); break;//新建目录
            case "NEWFILE": SaveFile(Context, true); break;//新建文件
            case "DELETE": Delete(Context); break;//删除操作
            case "COPY": CutCopy(Context, "copy"); break;//复制操作
            case "CUT": CutCopy(Context, "cut"); break;//剪贴操作
            case "UPLOAD": UpLoad(Context); break;//上传操作
            case "UpLoadII": UpLoadII(Context); break;//上传操作
            case "RENAME": ReName(Context); break;//重名
            case "ZIP": Zip(Context); break;//压缩文件
            case "UNZIP": UnZip(Context); break;//解压缩
            case "DOWNLOADS": DownLoads(Context); break;//下载多个文件
            case "EXPORT": Export(Context); break;
            case "DataExport": DataExport(Context); break;  //王斌2017.5.12
            case "DataExportGuidelines": DataExportGuidelines(Context); break;  //导出防御指引
            case "Cal": Cal(Context); break;   //王斌2017.5.12

        }
    }
    
    private void Export(HttpContext Context)
    {
       // Context.Response.Write("0k");
        //Context.Response.Flush();
        
        string Content = Context.Request["value1"];
        string[] contents = Content.Split(',');
        string value2 = Context.Request["value2"];
        string title = "江苏13城市("+value2+")综合指数排名";
        
        string templateFile = Context.Server.MapPath("../x年x月.xlsx");
        Aspose.Cells.WorkbookDesigner designer = new Aspose.Cells.WorkbookDesigner();
        designer.Open(templateFile);

        Aspose.Cells.Worksheet worksheet = designer.Workbook.Worksheets[0];
        for (int i = 1; i < 40; i++)
        {
            int j = i + 2;
            designer.SetDataSource("V" + i, contents[j]);
        }

        string UserAgent = Context.Request.ServerVariables["http_user_agent"].ToLower();
        string FileName = title + ".xls";
        if (UserAgent.IndexOf("firefox") == -1)
            FileName = HttpUtility.UrlEncode(FileName, Encoding.UTF8);

        designer.Process();
        designer.Save(FileName, Aspose.Cells.SaveType.OpenInExcel, Aspose.Cells.FileFormatType.Excel97To2003, Context.Response);

        Context.Response.End();
        
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
        string[] files = value1.Split('|');
        foreach (string item in files)
        {
            string path = Context.Server.MapPath(item);//获取绝对路径
            if (File.Exists(path))
                DownloadFile.ResponseFile(path, Context, false);
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
        try
        {
            Context.Response.ContentType = "text/plain";
            Context.Response.Charset = "UTF-8";
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
            {
                Context.Response.Write("error");
                return;
            }
            
            
            for (int i = 0; i < files.Count; i++)
            {
                files[i].SaveAs(path + Path.GetFileName(files[i].FileName));
            }
            string saveName = path + Path.GetFileName(files[0].FileName);
            StringCollection sheetNames = ExcelSheetName(saveName);
            DataTable excelTable = ExecleDs(saveName, sheetNames[0]);

            Utility1.GradeGuide1.InsertGuide1 insertGuide1 = new Utility1.GradeGuide1.InsertGuide1(m_Database,
                "D_HealthyStandard", "D_HealthyMonths", "T_HealthyGuidelines");
            List<string> msg= insertGuide1.ToDB(excelTable);
            StringBuilder msgStr = new StringBuilder();
            if (msg.Count > 0) {
                foreach (string str in msg) {
                    msgStr.AppendLine(str);
                }
            }
            
            //string HtmlInfo = GetHtmlInfo(excelTable);
            string msgs = msgStr.ToString().Trim();
            //Context.Response.Write(Path.GetFileNameWithoutExtension(saveName) + "$" + HtmlInfo);
          
            if(string.IsNullOrEmpty(msgs))
               Context.Response.Write("ok");
            else
                Context.Response.Write("err" + msgs);
            //插入到数据
        }
        catch(Exception ex) {
            Context.Response.Write("error"+ex.Message);
        }
    }

    public string GetHtmlInfo(DataTable excelTable)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<table id='PerHourDataTable'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
        sb.AppendLine("<tr>");

        //创建抬头
        sb.AppendLine("<td class='tabletitleD'style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9'>城市名称</td>");
        sb.AppendLine("<td class='tabletitleD' >综合指数</td>");
        sb.AppendLine("<td class='tabletitleD' >排名</td>");
        sb.AppendLine("</tr>");

        for (int m = 0; m < excelTable.Rows.Count; m++)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine(string.Format("<td class='tablerowD' style='border-left-style: solid; border-left-width: 1px; border-left-color: #C9C9C9'>{0}</td>", excelTable.Rows[m][0].ToString()));
            sb.AppendLine(string.Format("<td class='tablerowD'><div id='x" + m + "' class = 'divInputType_ZHZS' onclick='showInput(event,this)'>{0}</div></td>", excelTable.Rows[m][1].ToString()));
            sb.AppendLine(string.Format("<td class='tablerowD'><div id='y" + m + "' class = 'divInputType_ZHZS' onclick='showInputInt(event,this)'>{0}</div></td>", excelTable.Rows[m][2].ToString()));
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");
        return sb.ToString(); 
    }

    /// <summary>
    /// 查询EXCEL电子表格添加到DATASET
    /// </summary>
    /// <param name="filenameurl">服务器路径</param>
    /// <param name="table">表名</param>
    /// <param name="SheetName">Sheet表名</param>
    /// <returns>读取的DataSet </returns>
    public DataTable ExecleDs(string filenameurl, string SheetName)
    {
        string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filenameurl + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
        OleDbConnection conn = new OleDbConnection(strConn);
        conn.Open();
        DataTable dt = new DataTable();
        OleDbDataAdapter odda = new OleDbDataAdapter("select * from [" + SheetName + "]", conn);
        odda.Fill(dt);
        odda.Dispose();
        conn.Close();
        return dt;
    }
    public StringCollection ExcelSheetName(string filepath)
    {
        StringCollection names = new StringCollection();
        string strConn;
        strConn = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=2'";
        OleDbConnection conn = new OleDbConnection(strConn);
        conn.Open();
        DataTable sheetNames = conn.GetOleDbSchemaTable
        (System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        conn.Close();
        foreach (DataRow dr in sheetNames.Rows)
        {
            names.Add(dr[2].ToString());
        }
        return names;
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


    
     
    //王斌  2017.4.12    2017.5.17
    public void UpLoadII(HttpContext Context)
    {
        try
        {
            string path = Context.Server.MapPath(Context.Request["value2"]);
            HttpFileCollection files = Context.Request.Files;
            long allSize = 0;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            for (int i = 0; i < files.Count; i++)
            {
                allSize += files[i].ContentLength;
            }
            if (allSize > 20 * 1024 * 1024)
            {
                Context.Response.Write("error");
                return;
            }


            for (int i = 0; i < files.Count; i++)
            {
                files[i].SaveAs(path + Path.GetFileName(files[i].FileName));
            }

            string saveName = path + Path.GetFileName(files[0].FileName);
            StringCollection sheetNames = ExcelSheetNameII(saveName);

            DataTable excelTable_PubGroup = ExecleDs(saveName, sheetNames[1]);
            DataTable excelTable_PubUser = ExecleDs(saveName, sheetNames[0]);
            string group = Insert_PubGroup(excelTable_PubGroup);
            string user = Insert_Pubuser(excelTable_PubUser, excelTable_PubGroup);
            Context.Response.Write(user+Environment.NewLine+group);
        }
        catch (Exception ex)
        {
            Context.Response.Write("error" + ex.Message);
        }
    }
    public StringCollection ExcelSheetNameII(string filepath)
    {
        StringCollection names = new StringCollection();
        try
        {

            string strConn;
            strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable sheetNames = conn.GetOleDbSchemaTable
            (System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            conn.Close();
            sheetNames.DefaultView.Sort = "Table_Name asc";
            foreach (DataRow dr in sheetNames.Rows)
            {
                if (dr["Table_Name"].ToString() == "'A(用户)$'" ||
                    dr["Table_Name"].ToString() == "'B(分组)$'")
                {
                    names.Add(dr[2].ToString());
                }
            }
        }
        catch { 
        }
        return names;
    }

    /// <summary>
    /// 从System.Data.DataTable导入数据到T_Pubuser表中
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    ///  //王斌  2017.5.17
    public string Insert_Pubuser(DataTable dt_user, DataTable dt_group)
    {
        int successNum = 0, errorNum = 0, repeatNum = 0;   //记录导入多少条数据
        string results = "", error = "";   //记录错误信息
        string name = "", phone = "", email = "", group = "", healthyType = "", canMessage = "", Message_PubLvl = "", Message_PubTime = "", canEmail = "", Email_PubLvl = "", Email_PubTime = "", remark = "";
        //判断表中是否有空行，如果有则移除
        for (int i = 0; i < dt_user.Rows.Count; i++)
        {
            DataRow dr = dt_user.Rows[i];
            string str = "";
            for (int j = 0; j < dt_user.Columns.Count; j++)
            {
                str += dr[j].ToString();
            }
            if (str == "")
            {
                dt_user.Rows.Remove(dr);
            }
        }
        //行遍历
        foreach (DataRow dr in dt_user.Rows)
        {
            try
            {
                name = dr[0].ToString().Trim();
                phone = dr[1].ToString().Trim();
                email = dr[2].ToString().Trim();
                group = dr[5].ToString().Trim();
                #region  处理疾病类型
                string[] arrHealthy;
                if (dr[7].ToString().Trim() == "")
                {
                    DataRow[] temp = dt_group.Select("分组='" + group + "'");
                    arrHealthy = temp[0][2].ToString().Trim().Split(',');
                }
                else
                {
                    arrHealthy = dr[7].ToString().Trim().Split(',');
                }
                string v = "";
                foreach (String str in arrHealthy)
                {
                    switch (str)
                    {
                        case "儿童感冒": v += "2_"; break;
                        case "青年感冒": v += "3_"; break;
                        case "老年感冒": v += "4_"; break;
                        case "COPD": v += "5_"; break;
                        case "儿童哮喘": v += "6_"; break;
                        case "中暑": v += "7_"; break;
                        case "重污染": v += "8_"; break;
                    }
                }
                healthyType = v.TrimEnd('_');
                #endregion   处理疾病类型结束

                #region  短信处理
                if (dr[3].ToString().Trim() == "是")
                {
                    canMessage = "True";
                    #region  短信等级处理
                    string strMessPubLv = "";
                    if (dr[8].ToString().Trim() == "")
                    {
                        DataRow[] temp = dt_group.Select("分组='" + group + "'");
                        strMessPubLv = temp[0][3].ToString().Trim();
                    }
                    else
                    {
                        strMessPubLv = dr[8].ToString().Trim();
                    }
                    switch (strMessPubLv)
                    {
                        case "1级": Message_PubLvl = "01"; break;
                        case "2级": Message_PubLvl = "02"; break;
                        case "3级": Message_PubLvl = "03"; break;
                        case "4级": Message_PubLvl = "04"; break;
                        case "5级": Message_PubLvl = "05"; break;
                    }
                    #endregion  短信等级处理结束
                    #region  短信时效处理
                    string[] arrMessTime;
                    Message_PubTime = "";
                    if (dr[9].ToString().Trim() == "")
                    {
                        DataRow[] temp = dt_group.Select("分组='" + group + "'");
                        arrMessTime = temp[0][4].ToString().Trim().Split(',');
                    }
                    else
                    {
                        arrMessTime = dr[9].ToString().Trim().Split(',');
                    }
                    foreach (string str in arrMessTime)
                    {
                        switch (str)
                        {
                            case "上午今天": Message_PubTime += "01_"; break;
                            case "上午明天": Message_PubTime += "02_"; break;
                            case "下午明天": Message_PubTime += "03_"; break;
                            case "下午后天": Message_PubTime += "04_"; break;
                        }
                    }
                    Message_PubTime = Message_PubTime.TrimEnd('_');

                    #endregion  短信时效处理结束
                }
                else
                {
                    canMessage = "False";
                    Message_PubTime = "";
                    Message_PubLvl = "";
                }
                #endregion 短信处理结束

                #region  邮件处理
                if (dr[4].ToString().Trim() == "是")
                {
                    canEmail = "True";

                    #region 邮件等级处理
                    string strEmailPubLv = "";
                    if (dr[4].ToString().Trim() == "")
                    {
                        DataRow[] temp = dt_group.Select("分组='" + group + "'");
                        strEmailPubLv = temp[0][5].ToString().Trim();
                    }
                    else
                    {
                        strEmailPubLv = dr[10].ToString().Trim();
                    }
                    switch (strEmailPubLv)
                    {
                        case "1级": Email_PubLvl = "01"; break;
                        case "2级": Email_PubLvl = "02"; break;
                        case "3级": Email_PubLvl = "03"; break;
                        case "4级": Email_PubLvl = "04"; break;
                        case "5级": Email_PubLvl = "05"; break;
                    }
                    #endregion  邮件等级处理结束
                    #region  邮件时效处理
                    string[] arrEmailTime;
                    Email_PubTime = "";
                    if (dr[11].ToString().Trim() == "")
                    {
                        DataRow[] temp = dt_group.Select("分组='" + group + "'");
                        arrEmailTime = temp[0][6].ToString().Trim().Split(',');
                    }
                    else
                    {
                        arrEmailTime = dr[11].ToString().Trim().Split(',');
                    }
                    foreach (string str in arrEmailTime)
                    {
                        switch (str)
                        {
                            case "上午今天": Email_PubTime += "01_"; break;
                            case "上午明天": Email_PubTime += "02_"; break;
                            case "下午明天": Email_PubTime += "03_"; break;
                            case "下午后天": Email_PubTime += "04_"; break;
                        }
                    }
                    Email_PubTime = Email_PubTime.TrimEnd('_');

                    #endregion  邮件时效结束
                }
                else
                {
                    canEmail = "False";
                    Email_PubLvl = "";
                    Email_PubTime = "";
                }
                #endregion  邮件处理结束
                remark = dr[12].ToString().Trim();
 //测试T_PubUser表中是否有重复的信息，如果有就提醒用户
                string sqlSel = "SELECT * FROM T_PubUser WHERE Name='"+name+"' AND GroupName='"+group+"' AND Phone='"+phone+"' AND Email='"+email+"' AND HealthyType='"+healthyType+"' AND "+
                "CanMessage='"+canMessage+"' AND Message_PubLvl='"+Message_PubLvl+"' AND Message_PubTime='"+Message_PubTime+"' AND CanEmail='"+canEmail+"' AND Email_PubLvl='"+Email_PubLvl+"' AND Email_PubTime='"+Email_PubTime+"' AND Remark='"+remark+"'";
                string sqlInsert = @"INSERT INTO T_PubUser (Name,GroupName,Phone,Email,HealthyType,CanMessage,Message_PubLvl,Message_PubTime,CanEmail,Email_PubLvl,Email_PubTime,Remark) VALUES
                        ('" + name + "','" + group + "','" + phone + "','" + email + "','" + healthyType + "','" + canMessage + "','" + Message_PubLvl + "','" + Message_PubTime + "','" + canEmail + "','" + Email_PubLvl + "','" + Email_PubTime + "','" + remark + "')";
  DataTable dt = m_Database.GetDataTable(sqlSel);
                if (dt!=null && dt.Rows.Count > 0)
                {
                    repeatNum++;
                }
                else
                {
                    m_Database.Execute(sqlInsert);
                    successNum++;
                }
            }
            catch (Exception e)
            {
                error=e.Message;
                errorNum++;
            }
        }
        if (errorNum > 0)
        {
 results = "用户表:成功导入" + successNum + "条数据,其中重复数据有"+repeatNum+"条。失败" + errorNum + "条数据,原因：" + error;
 }
        else if (repeatNum > 0)
        {
            results = "用户表:成功导入" + successNum + "条数据,其中重复数据有" + repeatNum + "条。";
        }
        else {
            results = "用户表:成功导入" + successNum + "条数据。";
        }
        return results;
        //行遍历结束
    }

    //数据插入到T_PubGroup表
    ///  //王斌  2017.5.17
    public string Insert_PubGroup(DataTable dt)
    {
        int successNum = 0, errorNum = 0, repeatNum = 0;  //记录导入多少条数据
        string error = "";   //记录错误信息
        string groupName = "", region = "", healthyType = "", message_PubLvl = "", message_PubTime = "", Email_PubLvl = "", Email_PubTime = "";
        //判断表中是否有空行，如果有则移除
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = dt.Rows[i];
            string str = "";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                str += dr[j].ToString();
            }
            if (str == "")
            {
                dt.Rows.Remove(dr);
            }
        }
        //行遍历
        string messageErr = "";
        string results = "";
        foreach (DataRow dr in dt.Rows)
        {
            try
            {
                groupName = dr[0].ToString().Trim();
                region = dr[1].ToString().Trim().Replace(",", "_");
                #region  处理疾病类型
                string[] healthy = dr[2].ToString().Trim().Split(',');
                string v = "";
                foreach (String str in healthy)
                {
                    switch (str)
                    {
                        case "儿童感冒": v += "2_"; break;
                        case "青年感冒": v += "3_"; break;
                        case "老年感冒": v += "4_"; break;
                        case "COPD": v += "5_"; break;
                        case "儿童哮喘": v += "6_"; break;
                        case "中暑": v += "7_"; break;
                        case "重污染": v += "8_"; break;
                    }
                }
                healthyType = v.TrimEnd('_');
                #endregion   处理疾病类型结束

                #region  短信等级处理
                message_PubLvl = "";
                switch (dr[3].ToString().Trim())
                {
                    case "1级": message_PubLvl = "01"; break;
                    case "2级": message_PubLvl = "02"; break;
                    case "3级": message_PubLvl = "03"; break;
                    case "4级": message_PubLvl = "04"; break;
                    case "5级": message_PubLvl = "05"; break;
                }
                #endregion  短信等级处理结束

                #region  短信时效处理
                string[] messageTime = dr[4].ToString().Trim().Split(',');
                message_PubTime = "";
                foreach (string str in messageTime)
                {
                    switch (str)
                    {
                        case "上午今天": message_PubTime += "01_"; break;
                        case "上午明天": message_PubTime += "02_"; break;
                        case "下午明天": message_PubTime += "03_"; break;
                        case "下午后天": message_PubTime += "04_"; break;
                    }
                }
                message_PubTime = message_PubTime.TrimEnd('_');
                #endregion  短信时效处理结束

                #region  邮件等级处理
                Email_PubLvl = "";
                switch (dr[5].ToString().Trim())
                {
                    case "1级": Email_PubLvl = "01"; break;
                    case "2级": Email_PubLvl = "02"; break;
                    case "3级": Email_PubLvl = "03"; break;
                    case "4级": Email_PubLvl = "04"; break;
                    case "5级": Email_PubLvl = "05"; break;
                }
                #endregion  邮件等级处理结束

                #region  邮件时效处理
                string[] emailTime = dr[6].ToString().Trim().Split(',');
                Email_PubTime = "";
                foreach (string str in emailTime)
                {
                    switch (str)
                    {
                        case "上午今天": Email_PubTime += "01_"; break;
                        case "上午明天": Email_PubTime += "02_"; break;
                        case "下午明天": Email_PubTime += "03_"; break;
                        case "下午后天": Email_PubTime += "04_"; break;
                    }
                }
                Email_PubTime = Email_PubTime.TrimEnd('_');
                #endregion  邮件时效处理结束
                string sqlInsert = "INSERT INTO T_PubGroup VALUES ('" + groupName + "','" + region + "','" + healthyType + "','" + message_PubLvl + "','" + message_PubTime + "','" + Email_PubLvl + "','" + Email_PubTime + "')";
                m_Database.Execute(sqlInsert);
                successNum++;
            }
            catch (Exception e)
            {
                messageErr = e.Message;
                errorNum++;
                if (messageErr.IndexOf("PRIMARY KEY") > -1)
                {
                    repeatNum++;
                    error = messageErr;
                }
            }
        }
        if (repeatNum > 0) {
            results = "分组表:成功导入" + successNum + "条数据," + "失败" + errorNum + "条数据，其中重复数据有" + repeatNum + "条。原因：" + error;
        }
        else if (errorNum > 0 && repeatNum == 0) {
            results = "分组表:成功导入" + successNum + "条数据," + "失败" + errorNum + "条数据。原因："+messageErr;
        }
        else if (errorNum == 0) {
            results = "分组表:成功导入" + successNum + "条数据。";
        }
        return results;
        //行遍历结束
    }

    public void DataExportGuidelines(HttpContext Context)
    {
        #region   创建工作表
        Workbook wk = new Workbook();
        Worksheet sheet = wk.Worksheets[0];
        Cells cell = sheet.Cells;
        int num = 1;   //第一行是标题所以从1开始
        Aspose.Cells.Style style = wk.Styles[wk.Styles.Add()];   //设置样式
        style.HorizontalAlignment = TextAlignmentType.Center;   //标题文字居中
        style.Font.Size = 11;
        style.Font.Name = "宋体";
        #endregion
        //查询的sql xuehui 08-18
        string sql_query = " select t1.Item,t2.MC AS LMC,t3.MC,t1.GuideLines1,t1.Type,t1.GuideLines2 from T_HealthyGuidelines t1 left join D_HealthyStandard t2 "+
		             " on t1.Standard=t2.DM left join D_HealthyMonths t3 on t1.Months=t3.DM "+
                     " where t1.Item=t2.Code and t1.Item=t3.Code order by Item";
        DataTable d_Guidelines = m_Database.GetDataTable(sql_query);
        //======================插入处理
        string[] title = { "要素", "上限", "下限", "月份判断", "指引1", "疾病", "指引2" };
        string query_item = " select Item from T_HealthyGuidelines group by Item order by Item desc";
        DataTable item = m_Database.GetDataTable(query_item);
        //遍历添加标题数据
        for (int k = 0; k < title.Length; k++) {
            cell[0, k].PutValue(title[k].ToString());
            cell[0, k].SetStyle(style);
        }
        //添加内容数据
        foreach (DataRow row in item.Rows)
        {
            string str = row[0].ToString();
            DataRow[] dr = d_Guidelines.Select("Item='" + str + "'");
            for (int i = 0; i < dr.Length; i++)
            {
                string limit = dr[i]["LMC"].ToString();
                string month = dr[i]["MC"].ToString();
                string Guide = dr[i]["GuideLines1"].ToString();
                string str_item = dr[i]["item"].ToString();
                //先给每一个单元格赋值，然后把与上一行值相同的赋值为空
                ArrayList list = SplitStr(limit, str_item);
                for (int j = 0; j < dr[i].ItemArray.Length; j++)
                {
                    if (j < 2)
                    {
                        if (j == 0)
                        {
                            cell[num, j].PutValue(dr[i][j]);
                        }
                        else {
                            cell[num, 1].PutValue(list[0]);
                            cell[num, 2].PutValue(list[1]);
                        }
                    }
                    else
                    {
                        cell[num, (j + 1)].PutValue(dr[i][j]);
                    }   
                       
                        
                    if (i > 0) {    //这几列数据如果与上一行相同，则不需要添加数据
                        if (str_item == dr[(i - 1)]["item"].ToString()) cell[num, 0].PutValue("");
                        if (limit == dr[(i - 1)]["LMC"].ToString()) { cell[num, 1].PutValue(""); cell[num, 2].PutValue(""); }
                        if (month == dr[(i - 1)]["MC"].ToString()) cell[num, 3].PutValue("");
                        if (Guide == dr[(i - 1)]["GuideLines1"].ToString()) cell[num, 4].PutValue("");
                    }
                }
                num++;
            }

        }
        sheet.AutoFitColumns();   //列适宜自宽预防建议表格20170504导入
        string FileName = "预防建议表格" + DateTime.Now.ToString("yyyyMMdd")+"导出" + ".xls";
        string UserAgent = Context.Request.ServerVariables["http_user_agent"].ToLower();
        Context.Response.ContentType = "application/ms-excel";
        Context.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        Context.Response.BinaryWrite(wk.SaveToStream().ToArray());
        Context.Response.Flush();
        Context.Response.End();
    }

    public ArrayList SplitStr(string str, string term)
    {
        ArrayList list = new ArrayList();
        string[] arr = Regex.Split(str, term, RegexOptions.IgnoreCase);
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].ToString() != "")
            {
                if (arr[i].IndexOf("<") > -1)
                {
                    list.Add(arr[i].Split('<')[i]);
                }
                else if (arr[i].IndexOf("≤") > -1)
                {
                    string join = "[";
                    if (i > 0)
                    {
                        join = "]";
                        list.Add(arr[i].Split('≤')[i] + join);
                    }
                    else {
                        list.Add(join + arr[i].Split('≤')[i]);
                    }
                }
                else if (arr[i].IndexOf(">") > -1)
                {
                    list.Insert(0, arr[i].Split('>')[i]);
                }
                else if (arr[i].IndexOf("≥") > -1)
                {
                    //"40>温度>30"类似这种情况，则需要把“>”后面的值位置互换
                    string join = "]";
                    if (i > 0)
                    {
                        join = "[";
                        list.Insert(0, arr[i].Split('≥')[i] + join);
                    }
                    else {
                        list.Insert(0, join + arr[i].Split('≥')[i] );
                    }
                }
            }
            else {
                list.Add("");
            }
        }
        return list; 
    }
    
    //王斌  2017.5.11   2017.5.15
    public void DataExport(HttpContext Context)
    {

        string[] u_header = { "姓名", "手机号码", "邮件地址", "是否发送短信", "是否发送邮件", "分组名称", "所属区域","疾病类型", "短信发送等级", "短信时效", "邮件发送等级", "邮件时效", "备注" };
        string[] g_header = { "分组", "所属区域", "疾病类型", "短信发送等级", "短信时效", "邮件发送等级", "邮件时效" };
        DataTable d_PubUser = new DataTable();
        DataTable d_PubGroup = new DataTable();
        string sql_User = " SELECT NAME,PHONE,EMAIL,CASE CANMESSAGE WHEN '1' THEN '是' ELSE '否' END,CASE CanEmail WHEN '1' THEN '是' ELSE '否' END,GROUPNAME,GroupName,HealthyType,Message_PubLvl,Message_PubTime,Email_PubLvl,Email_PubTime,Remark FROM T_PubUser";
        string sql_Group = "SELECT GroupName,Region,HealthyType,Message_PubLvl,Message_PubTime,Email_PubLvl,Email_PubTime FROM T_PubGroup ";
        d_PubUser = m_Database.GetDataTable(sql_User);
        d_PubGroup = m_Database.GetDataTable(sql_Group);
        Workbook workbook = new Workbook();
        Aspose.Cells.Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        workbook.Worksheets.Clear();
        workbook.Worksheets.Add("A(用户)").AutoFitColumns();
        workbook.Worksheets.Add("B(分组)").AutoFitColumns();
        Cells u_cell = workbook.Worksheets["A(用户)"].Cells;
        Cells g_cell = workbook.Worksheets["B(分组)"].Cells;
        //打印表头
        for (int i = 0; i < u_header.Length; i++)
        {
           u_cell[0, i].PutValue(u_header[i]);
        }
        for (int i = 0; i < g_header.Length; i++)
        {
            g_cell[0, i].PutValue(g_header[i]);
        }
        //用户表处理
        for (int i = 0; i < d_PubUser.Rows.Count; i++)
        {
            DataRow dr = d_PubUser.Rows[i];
            for (int j = 0; j < u_header.Length; j++)
            {
                if (j == 8|| j==10) {
                    u_cell[(i + 1), j].PutValue(PubLvl(dr[j].ToString()));
                }
                else if (j == 9 || j == 11)
                {
                    u_cell[(i + 1), j].PutValue(PubTime(dr[j].ToString()));
                }
                else if (j == 7) {
                    u_cell[(i + 1), j].PutValue(HealthyType(dr[j].ToString()));
                }
                else if (j == 6)
                {
                    u_cell[(i + 1), j].PutValue(GetRegion(dr[j].ToString(), d_PubGroup));
                }
                else
                {
                    u_cell[(i + 1), j].PutValue(dr[j].ToString());
                }
            }
        }
        //分组表处理
        for (int i = 0; i < d_PubGroup.Rows.Count; i++)
        {
            DataRow dr = d_PubGroup.Rows[i];
            for (int j = 0; j < g_header.Length; j++)
            {
                if (j == 3 || j == 5)
                {
                    g_cell[(i + 1), j].PutValue(PubLvl(dr[j].ToString()));
                }
                else if (j == 4 || j == 6)
                {
                    g_cell[(i + 1), j].PutValue(PubTime(dr[j].ToString()));
                }
                else if(j==2){
                    g_cell[(i + 1), j].PutValue(HealthyType(dr[j].ToString()));
                }
                else if (j == 1){
                    g_cell[(i + 1), j].PutValue(Region(dr[j].ToString()));
                }
                else {
                    g_cell[(i + 1), j].PutValue(dr[j].ToString());
                }
            }
        }
        string FileName = "用户导出" + DateTime.Now.ToString("yyyy年MM月dd日") + ".xls";
        string UserAgent = Context.Request.ServerVariables["http_user_agent"].ToLower();
        Context.Response.ContentType = "application/ms-excel";
        Context.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        Context.Response.BinaryWrite(workbook.SaveToStream().ToArray());
        Context.Response.Flush();
        Context.Response.End();
    }

      public string PubLvl(string grade) { 
        switch(grade){
            case "01": grade = "1级"; break;
            case "02": grade = "2级"; break;
            case "03": grade = "3级"; break;
            case "04": grade = "4级"; break;
            case "05": grade = "5级"; break;
        }
        return grade;
    }
    public string PubTime(string time) {
    string [] str = time.Split('_');
        time = "";
        foreach (string v in str) {
            switch (v)
            {
                case "01": time += "上午今天,"; break;
                case "02": time += "上午明天,"; break;
                case "03": time += "下午明天,"; break;
                case "04": time += "下午后天,"; break;
            }
        }
        return time.TrimEnd(',');
    }
    public string HealthyType(string type) { 
        string [] str = type.Trim().Split('_');
        type = "";
        foreach(string v in str){
            switch(v){
                case "2": type += "儿童感冒,"; break;
                case "3": type += "青年感冒,"; break;
                case "4": type += "老年感冒,"; break;
                case "5": type += "COPD,"; break;
                case "6": type += "儿童哮喘,"; break;
                case "7": type += "中暑,"; break;
                case "8": type += "重污染,"; break;
            }
        }
        return type.TrimEnd(',');
    }
    public string GetRegion(string region,DataTable dt) {
        region = region.Trim();
        DataRow[] dr = dt.Select("GroupName='"+region+"'");
        region = dr[0][1].ToString();
        string[] str = region.Split('_');
        region = "";
        for (int i = 0; i < str.Length; i++) {
            region += str[i] + ",";
        }
            return region.TrimEnd(',');
    }
    public string Region(string region) {
        string[] str = region.Trim().Split('_');
        region = "";
        foreach (string temp in str) {
            region += temp+",";
        }
        return region.TrimEnd(',');
    }
    
    //王斌  2017.5.12   2017.5.15
    public void Cal(HttpContext Context)
    {
        //当天日期,或者你要订正的日期
        DateTime dtnow = DateTime.Now.AddDays(0);
        //数据库连接
        //m_Database = new Database(ConnectionString);
        DateTime dttime;
        string typeGrade = "", typeGuide = "", sqlGrade = "", sqlGuide = ""; ;
        float time = dtnow.Hour + ((float)dtnow.Minute / 60);
        HealthyGrade hg = new HealthyGrade(m_Database);
        HealthyGuide hgd = new HealthyGuide(m_Database);
        string dzTime =dtnow.ToString("yyyy-MM-dd HH:mm:ss");
        try {
            if (time <= 16 + 55 / 60)
            {
                typeGrade = "Grade10";
                typeGuide = "Guide10";
                //处理20预报等级数据即上午预报
                dttime = Convert.ToDateTime(dtnow.AddDays(-1).ToString("yyyy-MM-dd 20:00:00"));
                hg.InsertTData(dttime, "CITYF");
                //修改10时的等级预报时间
                sqlGrade = "UPDATE T_HealthyTime SET TSCTime='" + dzTime + "'WHERE TYPE='" + typeGrade + "'";
                m_Database.Execute(sqlGrade);
                //处理08订正指引数据即下午订正
                //修改10时的指引时间
                hgd.InsertTData(dttime, "CITYF");
                sqlGuide = "UPDATE T_HealthyTime SET TSCTime='" + dzTime + "'WHERE TYPE='" + typeGuide + "'";
                m_Database.Execute(sqlGuide);
                //处理20时实况等级数据
                dttime = Convert.ToDateTime(dtnow.ToString("yyyy-MM-dd 20:00:00"));
                hg.InsertAaData(dttime);
                
            }
            else {
                typeGrade = "Grade17";
                typeGuide = "Guide17";
                //处理08订正等级数据即下午订正
                dttime = Convert.ToDateTime(dtnow.ToString("yyyy-MM-dd 08:00:00"));
                hg.InsertTData(dttime, "CITYF");
                //修改17时的等级预报时间
                sqlGrade = "UPDATE T_HealthyTime SET TSCTime='" + dzTime + "'WHERE TYPE='" + typeGrade + "'";
                m_Database.Execute(sqlGrade);
                //处理08订正指引数据即下午订正
                hgd.InsertTData(dttime, "CITYF");
                //修改17时的指引时间
                sqlGuide = "UPDATE T_HealthyTime SET TSCTime='" + dzTime + "'WHERE TYPE='" + typeGuide + "'";
                m_Database.Execute(sqlGuide);
            }
            Context.Response.Write("成功");
        }
        catch (Exception e)
        {
            Context.Response.Write("失败");
        }
        
    }
}