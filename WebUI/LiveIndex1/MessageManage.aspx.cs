using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using System.Data;
using System.Web.Services;
using Aspose.Cells;
using System.Text;
using MMShareBLL.DAL;

public partial class LiveIndex_MessageManage : System.Web.UI.Page
{
    public static Database m_database;
    public static LiveIndex index = new LiveIndex();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            m_database = new Database("DBCONFIG");
        }
    }

    //导出到Excel  这个功能不做了
    public void BtnExport(object sender, EventArgs e)
    {
        string[] title = { "姓名", "手机", "年龄", "性别", "学历", "单位", "职称", "邮编", "地址", "是否注销"};
        string sql_query = "select userName,iphone,age,gender,edu,occupation,company,postcode,address,IsUse from dbo.T_Weather_Message order by userName desc";
        DataTable dt = m_database.GetDataTable(sql_query);
        Workbook wk = new Workbook();   //创建工作表
        Worksheet sheet = wk.Worksheets[0]; 
        Cells cell = sheet.Cells;    //创建单元格
        Aspose.Cells.Style styleTitle = wk.Styles[wk.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        //标题
        for (int i = 0; i < title.Length;i++ ) {
            cell[0, i].PutValue(title[i]);
        }

        string FileName = "用户导出" +DateTime.Now.ToString("yyyyMMdd") + ".xls";
        string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
        if (UserAgent.IndexOf("firefox") == -1)
            FileName = HttpUtility.UrlEncode(FileName, Encoding.UTF8);

        Response.ContentType = "application/ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.BinaryWrite(wk.SaveToStream().ToArray());
        Response.Flush();
        Response.End();
    }

    //删除用户
    [WebMethod]
    public static string DelUser(string id) {
        string sql_del = "delete from t_weather_message where id='"+id+"'";
        m_database.GetDataTable(sql_del);
        return "ok";
    }
    //编辑用户
    [WebMethod]
    public static string EditUser(string value, string userId,string title)
    {//["userName", "phone", "age", "gender", "education", "company", "occupation", "postCode", "address", "phoneStatus"];
        string tip = "";
        string userName, phone, age, gender, education, company, occupation, postCode, address, phoneStatus;
        string[] val = value.Split(',');
        userName = val[0];
        phone = val[1];
        age = val[2];
        gender = val[3];
        education = val[4];
        company = val[5];
        occupation=val[6];
        postCode = val[7];
        address = val[8];
        phoneStatus = val[9] == "" ? "NULL" : val[9];
        string sql = "update T_Weather_Message set userName='"+userName+"',iphone='"+phone+"',age='"+age+"',gender='"+gender+"',edu='"+education+"',"+
            "company='"+company+"',occupation='"+occupation+"',PostCode='"+postCode+"',address='"+address+"',IsUse='"+phoneStatus+"' where id='"+userId+"'";
        if (title == "添加用户") {
            string values = "'"+userName+"','"+phone+"','"+age+"','"+gender+"','"+education+"','"+company+"','"+occupation+"','"+postCode+"','"+address+"','"+phoneStatus+"'";
            sql = "insert into T_Weather_Message (userName,iphone,age,gender,edu,company,occupation,PostCode,address,IsUse) values ("+values+")";
        }
        try
        {
            m_database.Execute(sql);
            tip = "success";
        }
        catch (Exception e) 
        {
            tip = "error";
        }
        return tip;
    }

    //上传到服务器
    //[WebMethod]
    //public static void FtpUpload() {
    //    //获取短信推送ftp的相关参数
    //    string name = "";
    //    DataTable dt_Ftp = index.GetFtpOption("", "message");
    //    string address = dt_Ftp.Rows[0][0].ToString().Trim();
    //    string sample = dt_Ftp.Rows[0][5].ToString();
    //    //获取数据
    //    string sql_query = "select iphone from dbo.T_Weather_Message order by id desc";
    //    DataTable dt = m_database.GetDataTable(sql_query);
    //    //模板格式处理
    //    string txt = "";
    //    //推送ftp方法
    //    index.UploadFtp(address,txt,"message","1");
    //}
    
}