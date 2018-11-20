using System;
using System.Data;
using System.Configuration; 
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public interface IPagerMessager
{
    int GetRecordCount();
}

public delegate void PageIndexChange(int newPageIndex);
