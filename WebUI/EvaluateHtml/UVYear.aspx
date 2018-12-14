<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UVYear.aspx.cs" Inherits="EvaluateHtml_UVYear" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>紫外线年评分</title>
 <link href="css/Evaluate.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 <script language="javascript" type="text/javascript" src="JS/UVYear.js"></script>
   <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
</head>
<body>
  <div style=" width:80%; margin-left:auto; margin-right:auto; ">
    <div class="divTop" >
    <div>
        <div class="checkStyle">
            <div class="checkLable">评分时间：</div>
            <input id="H00" type="text" class="selectDateFormStyle" runat="server" onchange="InitTable()" onclick="WdatePicker({dateFmt:'yyyy年'})"/>
             <input type="button" style=" float:left;  margin-left:20px;" id="ScanBack" class="normal-btn input-btn" value="查询" onclick="InitTable()" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'" onmousedown="this.className='normal-btn-d input-btn'"  onmouseup ="this.className='normal-btn input-btn'"/>
             <input type="button" style=" float:left;  margin-left:20px;" id="Button1" class="normal-btn input-btn" value="导出" onclick="OutTable()" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'" onmousedown="this.className='normal-btn-d input-btn'"  onmouseup ="this.className='normal-btn input-btn'"/>
        </div>
    </div>
    </div>
    <div style=" clear:both;"></div>
    <div id="leftTable" class="score">
    <div id="coutTable0" ></div>
    </div>
    </div>
     <form id="form1" runat="server">
     <asp:HiddenField id="Element" runat="server" />
   
     <asp:Button ID="btnExport" runat="server" onclick="Button1_Click" Text="Button" CssClass="inVisibility"/>
    </form>
</body>
</html>

