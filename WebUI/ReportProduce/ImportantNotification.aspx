<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportantNotification.aspx.cs" Inherits="ReportProduce_ImportantNotification" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <link href="../css/ProductPubLog.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/ux/RowExpander.js" type="text/javascript"></script>
    <script src="../AQI/js/SetExpander.js" type="text/javascript"></script>
    <script src="../AQI/js/ImportantNotification.js" type="text/javascript"></script> 
    <script src="../AQI/js/InportantNoticeEdit.js" type="text/javascript"></script>
        <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../JS/jquery.nicescroll.min.js" type="text/javascript"></script>
    <script src="../AQI/js/PDFPreview/chili-1.7.pack.js" type="text/javascript"></script>
    <script src="../AQI/js/PDFPreview/jquery.media.js" type="text/javascript"></script>
    <script src="../AQI/js/PDFPreview/jquery.metadata.js" type="text/javascript"></script>   
</head>
<body>
<div class="search">
<div class="tableTop">
      <div id="topInfo" class="titleContent">    
        <table width="100%" class="log_table" style="width: 80%;" border="0">
  <tbody><tr>
   <td class="attrName">通知类型</td>
    <td class="attrValue tdName">
        <div class="proNameSelect" id="Div5">
            <div id="Div6" class="dateDiv">
                <div class="proNameText" id="proName">全部</div>
                <div id="Div8" class="selIcon"></div>
            </div>
                <ul id="Ul2" class="firstPolUl hide">
                    <li><div>全部</div></li>
                    <li><div>全系统通知</div></li>
                    <li><div>全市通知</div></li>
                    <li><div>城环中心通知</div></li>
                    <li><div>会商通知</div></li>                          
                </ul>
            </div>
      </td>    
    <td class="attrName">通知时间</td>
    <td class="attrValue"><input name="txtPublishStartDate" class="s_k1" id="txtPublishStartDate_Search" onclick=" WdatePicker();" type="text" readonly="readonly" value=""/></td>
    <td>-</td>
    <td><input name="txtPublishEndDate" class="s_k1" id="txtPublishEndDate_Search" onclick=" WdatePicker();" type="text" readonly="readonly" value=""/></td>
    <td class="attrName">是否停用</td>
    <td class="attrValue tdPubCon">
        <div class="pubConSelect" id="Div1">
            <div id="Div2" class="dateDiv">
                <div class="pubConText" id="pubCon">全部</div>
                <div id="Div4" class="selIcon"></div>
            </div>
                <ul id="Ul1" class="firstPolUl hide">
                    <li><div>全部</div></li>
                    <li><div>停用</div></li>
                    <li><div>未停用</div></li>                   
                </ul>
            </div>
    </td>
   
    <td class="attrValue"><div class="button" id="btnSearch">查询</div></td>
  </tr>
</tbody></table>
      </div>
   </div>

</div>
<div class="tableOutLine" id="tableOutLine">
<div class="mapTitle">
           <div class="titlePoint"></div>
           <span>重要通知</span>       
           </div>
<div class="editBtnArea">
   <div id="addNew" class="noticeEditBtn">新增</div>
   <div id="delete" class="noticeEditBtn">删除</div>
</div>
<div id="logTable" class="logTable">
</div>
</div>
</body>
</html>
