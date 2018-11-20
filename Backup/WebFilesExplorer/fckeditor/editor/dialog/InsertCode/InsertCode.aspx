<%@ Page Language="C#" %>

<%@ Register TagPrefix="CH" Namespace="ActiproSoftware.CodeHighlighter" Assembly="ActiproSoftware.CodeHighlighter.Net20" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Highlighter.LanguageKey = ddlLangType.SelectedItem.Text;
        Highlighter.OutliningEnabled = chkOutLining.Checked;
        Highlighter.LineNumberMarginVisible = chkLineNum.Checked;
        Highlighter.Text = txtCode.Text;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CodeHighlighterConfiguration config = ConfigurationManager.GetSection("codeHighlighter") as CodeHighlighterConfiguration;
            string[] keys = new string[config.LanguageConfigs.Keys.Count];
            config.LanguageConfigs.Keys.CopyTo(keys, 0);
            Array.Sort(keys);
            
            foreach (string key in keys)
            {
                ddlLangType.Items.Add(key);
            }
            
            ddlLangType.SelectedIndex = ddlLangType.Items.IndexOf(ddlLangType.Items.FindByText("C#"));
        }
    }

    protected void Highlighter_PostRender(object sender, EventArgs e)
    {
        if (Highlighter.Output != null)
        {
            string output = Highlighter.Output.Replace("  ", "&nbsp;&nbsp;").Replace("\n", "<br />").Trim();

            if (output.StartsWith("<div>", StringComparison.OrdinalIgnoreCase))
            {
                output = "<div style='border:dashed 1px #ff9966;padding:10px;background:#ffffdd; line-height:1.2em;'>" + output.Substring(5);
            }
            else
            {
                output = "<div style='border:dashed 1px #ff9966;padding:10px;background:#ffffdd; line-height:1.2em;'>" + output + "</div>";
            }

            lblCode.Text = Regex.Replace(output, "<!--[\\S\\s]*?-->", string.Empty);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "setOK", "window.parent.SetOkButton( true );", true);
        }
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content="noindex, nofollow" name="robots" />

    <script src="../common/fck_dialog_common.js" type="text/javascript"></script>
    <script type="text/javascript">

        var oEditor = window.parent.InnerDialogLoaded() ;

        function Ok()
        {
            if(GetE('txtCode').value == '')
            {
                alert("代码内容不能为空！"); 
                return false;
            }
            
            oEditor.FCK.InsertHtml( document.getElementById("lblCode").innerHTML ) ;
        	return true ;
        }

    </script>

    <style type="text/css">
        .langType
        {
            padding-bottom: 5px;
        }
        .btnRun
        {
            padding-top: 5px;
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="langType">
            语言类型：<asp:DropDownList ID="ddlLangType" runat="server">
            </asp:DropDownList>
        </div>
        <div class="textCode">
            <asp:TextBox ID="txtCode" runat="server" TextMode="multiline" Width="640px" Height="390px"></asp:TextBox>
        </div>
        <div class="btnRun">
            <asp:CheckBox ID="chkOutLining" Text="折叠代码" runat="server" />
            <asp:CheckBox ID="chkLineNum" Text="允许行号" runat="server" />
            <asp:Button ID="btnSubmit" runat="server" Text="  转  换  " OnClick="btnSubmit_Click" />
            <pre style="display: none;">
<CH:CodeHighlighter runat="server" ID="Highlighter" OnPostRender="Highlighter_PostRender" /></pre>
            <asp:Label ID="lblCode" Style="display: none;" runat="server"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
