﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HeatstrokeWithCity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.forecastTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
            this.serverTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}