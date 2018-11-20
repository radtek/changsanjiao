using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportProduce_AirPollutionForecast : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        #region　ExtJS

        ExtHelper.Add(this.Header, this);

        #endregion

        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}