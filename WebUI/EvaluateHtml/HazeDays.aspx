<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HazeDays.aspx.cs" Inherits="EvaluateHtml_HazeDays" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>霾日评分</title>
 <link href="css/Evaluate.css" rel="stylesheet" type="text/css" />
  <link rel="stylesheet" href="../themes/base/jquery.ui.all.css"/>
   <link href="css/WorkGroup.css?v=4" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
  <script language="javascript" type="text/javascript" src="JS/jquery-1.9.1.js"></script>
  <script type="text/javascript" language="javascript" src="../media/js/jquery.js"></script>
<script language="javascript" type="text/javascript" src="JS/ajaxfileupload.js"></script>
 <script language="javascript" type="text/javascript" src="JS/HazeDays.js"></script>
   <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
 <script src="JS/jquery-ui.js"></script>
</head>
<body>
  <div style=" width:80%; margin-left:auto; margin-right:auto; ">
    <div class="divTop" >
    <div>
        <div class="checkStyle">
            <div class="checkLable" style=" margin-top:4px;">评分时间</div>
          <input id="H00" type="text" class="selectDateFormStyle" runat="server" onchange="InitTable()" onclick="WdatePicker({dateFmt:'yyyy年MM月'})" />
                    <input type="button" style="float: left; margin-left: 20px;" id="ScanBack" class="button" value="查询" onclick="InitTable()" />
                    <input type="button" style="float: left; margin-left: 20px;" class="button" value="导入" onclick="InsertData()" />
                    <input type="button" style="float: left; margin-left: 20px;" id="Button3" class="button" value="评分" onclick="Evaluate()" />
                    <input type="button" style="float: left; margin-left: 20px;" class="button" value="导出" onclick="OutTable()" />
                    <input type="button" style="float: left; margin-left: 20px;" class="button" value="下载模板" onclick="DownloadData()" />
        </div>
    </div>
    </div>
    <div style=" clear:both;"></div>
    <div id="leftTable" class="score">
    <div id="coutTable0" ></div>
    </div>
    </div>
<iframe id="uploadFrm" frameborder="no" border="0" scrolling="no" onload="iframeOnload()" name="uploadFrm" style="border: 2px solid #0066FF; width:0px; height:0px; display:none;"></iframe>
<div id="dialog-form" style=" display:none;  height:100%;">
  <form  name="actionForm" id="actionForm" action="WebExplorerss.ashx" method="post" target="uploadFrm"  enctype="multipart/form-data">
    <fieldset  style=" border:0 none white;">
         <div id="editorHidden" class="show editorContent" style="position: absolute; margin-top:10px;"/>
         <div id="Tb1">
          <div>
          <span >文件：</span><input  type="file" id="fileAccept"  style="width:305px;" runat="server" name="fileAccept" />
          </div>
          <div style="padding-top:10px; padding-left:50px;">
          <input type="button" class="Upload-btn upload-btnQuery" id="Button4" onclick="add()"onmouseover="this.className='Upload-btn-h upload-btnQuery'" onmouseout="this.className='Upload-btn upload-btnQuery'" onmousedown="this.className='Upload-btn-d upload-btnQuery'" onmouseup ="this.className='Upload-btn upload-btnQuery'" value="上传" />
          <input type="button"  class="UploadRelese-btn upload-relese" id="Button5" onclick="closeDialog()"onmouseover="this.className='UploadRelese-btn-h upload-relese'" onmouseout="this.className='UploadRelese-btn upload-relese'" onmousedown="this.className='UploadRelese-btn-d upload-relese'" onmouseup ="this.className='UploadRelese-btn upload-relese'" value="取消"/>
          </div>
         </div>
    </fieldset>
  </form>
   <div id='uploadStatus' style='display:none; margin-top:20px; margin-left:35px;'><img src='css/image/process.gif' /><div style='color:#ccc;' id="dstext">正在导入数据，如果长时间不响应，可能是上传文件太大导致出错！</div></div>
</div>
     <form id="form1" runat="server">
     <asp:HiddenField id="Element" runat="server" />
     <asp:Button ID="btnExport" runat="server" onclick="Button1_Click" Text="Button" CssClass="inVisibility"/>
    </form>
</body>
</html>
