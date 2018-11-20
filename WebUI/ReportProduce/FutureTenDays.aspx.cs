using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportProduce_FutureTenDays : System.Web.UI.Page
{
    public string url = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        url += "<%=PageOfficeLink.OpenWindow(";
        url +="'../AQI/PageOfficePreview/FutureTenDaysPreview.aspx?filePath=FutureTenDays.doc&ProductName=FutureTenDays'";
        url +=",'width=1200px;height=700px;')%>";            
    }
}