using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Readearth.Data;
public partial class Logs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Entity entity = new Entity(m_Database, "Disaster");
        //DataTable dataTable = null;
        //if (entity.Initialized)
        //{
        //    DataSet dataSet = entity.Query(sb.ToString(), "");
        //    dataTable = dataSet.Tables[0];
        //    //PrimaryValue.Value = dataTable.Rows[0][0].ToString();
        //    //imgPhoto.ImageUrl = dataTable.Rows[0][dataTable.Columns.Count - 1].ToString();
        //}
        Database db = new Database();
        string strSQL = "SELECT * FROM V_LoginCount";
        DataSet dataSet = db.GetDataset(strSQL);
        DataTable dataTable = dataSet.Tables[0];
        gdvQueryAttribute.DataSource = dataTable;
        gdvQueryAttribute.DataBind();

    }

    protected void gdvQueryAttribute_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow grdViewRow = e.Row;

        if (grdViewRow.RowType == DataControlRowType.DataRow)
        {
            grdViewRow.Attributes.Add("onmouseover", "MoveOver(this)");
            grdViewRow.Attributes.Add("onmouseout", "MoveOut(this)");
            grdViewRow.Attributes.Add("onclick", "SelectThisRow(this)");
            grdViewRow.Style.Add("cursor", "hand");

            foreach (TableCell tblCell in grdViewRow.Cells)
            {
                tblCell.CssClass = "listtd";
                tblCell.Wrap = false;
            }

        }
        else
        {
            foreach (TableCell tblCell in grdViewRow.Cells)
            {
                tblCell.CssClass = "listtop-td";
                tblCell.Wrap = false;
            }

        }
    }

   
}
