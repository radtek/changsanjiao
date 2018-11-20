using System;
using System.Configuration;
using System.Web.UI;
using Readearth.Data;
using System.IO;
using System.Web.Services;

public partial class WarningGroup : System.Web.UI.Page
{
    public static Database m_Database;
    private static string m_direct;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            m_Database = new Database();
            m_direct = ConfigurationManager.AppSettings["warnSaveTxt"];
        }
    }

    [WebMethod]
    public static string Save(string content) {
        string path = m_direct.Split('\\')[0];
        string fileName = "scuem_yjxh_"+ DateTime.Now.ToString("yyyyMMddHHmm")+".txt";
        string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        try
        {
            File.WriteAllText(Path.Combine(dir, fileName), content);
            return "ok";
        }
        catch (Exception e)
        {
            return "error";
        }
    }
    [WebMethod]
    public static string PreView() {
        string filePath= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, m_direct);
        DirectoryInfo di = new DirectoryInfo(filePath);
        FileInfo [] arrFi = di.GetFiles();
        SortAsFileCreationTime(ref arrFi);
        string fileFullName= Path.Combine(filePath, arrFi[0].ToString());
        string txt = "";
        try
        {
            txt = File.ReadAllText(fileFullName)+"#"+ arrFi[0].ToString();
            
        }
        catch(Exception e) {
            return "错误："+e.Message;
        }
        return txt;
    }
    /// <summary>
    　　/// C#按创建时间排序（顺序）
    　　/// </summary>
    　　/// <param name="arrFi">待排序数组</param>
    public static void SortAsFileCreationTime(ref FileInfo[] arrFi)
    {
        Array.Sort(arrFi, delegate (FileInfo x, FileInfo y) { return y.CreationTime.CompareTo(x.CreationTime); });
    }
}
