using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Readearth.Data;
using System.IO;
using Aspose.Slides;
using Aspose.Slides.Export;
using System.Drawing;

public partial class ConsultationCar : System.Web.UI.Page
{
    public string m_ForecastDate;
    private Database m_Database;
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime dtNow = DateTime.Now;
        m_Database = new Database();
        H00.Value = dtNow.ToString("yyyy年MM月dd日");
        m_ForecastDate = dtNow.ToString("yyyy年MM月dd日");

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string Content = Element.Value;
        DateTime startTime = DateTime.Parse(Content);
        DateTime endTime = startTime.AddDays(1);
        string  strSQL = "SELECT CommentTime,Name,CommentContent,Folder,ImgName,ImgTime,ModuleName FROM T_DayComment WHERE Tag=1  AND CommentTime BETWEEN '" + startTime + "' AND '" + endTime + "'  ORDER BY CommentTime";
        DataTable dt = m_Database.GetDataTable(strSQL);
        if (dt.Rows.Count > 0)
        {
            string filePath = "D:\\SEMCShareTest\\4.ppt";
            string dataDir = Path.GetFullPath("../../../Data/");
            bool IsExists = System.IO.Directory.Exists(dataDir);
            if (!IsExists)
                System.IO.Directory.CreateDirectory(dataDir);
            string strPic = @"E:\SMMCDatabase\";
            Presentation pres = new Presentation(filePath);
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Slide slide = pres.Slides[0];
                Slide slideTemp;
                if (i == 0)
                    slideTemp = slide;
                else
                    slideTemp = pres.CloneSlide(slide, i+1);
                string content = "";
                if (dt.Rows[i]["CommentContent"].ToString() == "")
                    content = "  ";
                else
                    content = dt.Rows[i]["CommentContent"].ToString();
                slideTemp.Shapes[0].TextFrame.Text = content;
                slideTemp.Shapes[1].TextFrame.Text = dt.Rows[i]["ImgName"].ToString() + "  " + dt.Rows[i]["ImgTime"].ToString();

                slideTemp.Shapes[2].FillFormat.Type = FillType.Picture;
                string startPaht = dt.Rows[i]["Folder"].ToString().Replace("Product/", "");
                string path = "";
                int index = startPaht.IndexOf("?");
                if (index < 0)
                    path = startPaht;
                else
                    path = startPaht.Substring(0, index);
                Aspose.Slides.Picture pic = new Picture(pres, strPic + path);
                int picID = pres.Pictures.Add(pic);
                slideTemp.Shapes[2].FillFormat.PictureId = picID;

            }
            string fileName = "ConsulationCar" + DateTime.Now.ToString("MMddHHmmss") + ".ppt";
            pres.Save(dataDir + fileName, SaveFormat.Ppt);

            Response.ContentType = "application/vnd.ms-powerpoint";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.Flush();
            System.IO.Stream st = this.Response.OutputStream;
            pres.Save(st, Aspose.Slides.Export.SaveFormat.Ppt);
            Response.End();
        }

    }
}
