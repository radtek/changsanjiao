using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Readearth.Data;
using MMShareBLL.DAL;
using System.Data;
using Aspose.Cells;
using System.Text;

public partial class LiveIndex_IndexCorr : System.Web.UI.Page
{
    public static Database m_Database;
    public static string userName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            m_Database = new Database("DBCONFIG");
            if (Request.Cookies["User"] != null)
            {
                userName = Request.Cookies["User"]["name"];
            }
        }
    }
    public void Button1_Click(object sender,EventArgs e){
        LiveIndex index = new LiveIndex();
        DateTime dNow = DateTime.Now;
        string str =index.GetSiteId(userName);
        string siteId = str.Split('#')[0];
        string site = str.Split('#')[1];
        string LST = lst.Value;
        DataTable result = index.GetTableII("已发布指数", siteId, LST);
        string date = "预报日期："+LST+" 站点："+site;
        result.Columns.Remove("name1");
        result.Columns.Remove("code");
        result.Columns.Remove("meanName");
        result.Columns.Remove("tipInfo");
        Workbook wk = new Workbook();
        Worksheet sheet = wk.Worksheets[0];
        sheet.Name = "indexData";    //sheet的名称
        Cells cells = sheet.Cells;   //单元格
        Aspose.Cells.Style style = wk.Styles[wk.Styles.Add()];   //设置样式1
        Aspose.Cells.Style style2 = wk.CreateStyle();
        Aspose.Cells.Style style3 = wk.CreateStyle();
        #region  设置样式
        style3.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;    //日期右对齐
        style2.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Left;
        style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin; //应用边界线 右边界线  
        style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;//应用边界线 上边界线  
        style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;//应用边界线 下边界线
        style.HorizontalAlignment = TextAlignmentType.Center;   //标题文字居中
        #endregion
        string[] title = { "序号", "指数名称", "指数值", "级别", "提示语" };
        cells.Merge(0, 0, 2, 5);
        cells[0, 0].PutValue("气象生活指数预报");
        cells.Merge(2, 0, 1, 5);
        cells[2, 0].PutValue(date);
        #region  设置标题样式
        style.Font.Size = 20;
        style.Font.IsItalic = true;
        style.Font.Color = System.Drawing.Color.FromArgb(0, 0, 139);
        cells[0, 0].SetStyle(style,true);
        cells[2, 0].SetStyle(style3,true);
        #endregion
        for (int i = 0; i < title.Length; i++)
        {   //表头赋值以及样式
            cells[3, i].PutValue(title[i]);   //0,0,139
            style.ForegroundColor = System.Drawing.Color.FromArgb(192, 192, 192);
            style.Pattern = Aspose.Cells.BackgroundType.Solid;
            style.Font.Color = System.Drawing.Color.FromArgb(0, 0, 139);   //字体颜色
            style.Font.Size = 12;
            style.Font.IsItalic = false;    //不是斜体
            sheet.Cells[3, i].SetStyle(style,true);   //为单元格设置样式

        }
        for (int j = 0; j < result.Rows.Count; j++)
        {
            for (int k = 0; k < title.Length; k++)
            {
                if (k == 0)
                {
                    cells[(j + 4), k].PutValue((j + 1));

                }
                else
                {
                    if (k == 2 || k == 3)    //这两列是数字，因此需要把数字转换成float型，要不然导出的Excel左上角有个绿色小三角
                    {
                        float num = float.Parse(result.Rows[j][(k - 1)].ToString());
                        cells[(j + 4), k].PutValue((float)(((int)(num*100))*1.0)/100);   //直接转会发生误差，需要做处理，不四舍五入
                    }
                    else
                    {
                        cells[(j + 4), k].PutValue(result.Rows[j][(k - 1)]);
                    }
                }
                   //正文内容靠做
                cells[(j + 4), k].SetStyle(style2, true);
            }
        }
        sheet.AutoFitColumns();   //列适宜自宽
        string FileName = "indexData.xls";
        string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
        if (UserAgent.IndexOf("firefox") == -1)
            FileName = HttpUtility.UrlEncode(FileName, Encoding.UTF8);
        Response.ContentType = "application/ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.BinaryWrite(wk.SaveToStream().ToArray());
        Response.Flush();
        Response.End();
    }
}