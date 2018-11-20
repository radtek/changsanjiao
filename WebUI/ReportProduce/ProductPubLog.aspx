<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductPubLog.aspx.cs" Inherits="ReportProduce_ProductPubLog" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <link href="../css/ProductPubLog.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/ux/RowExpander.js" type="text/javascript"></script>
    <script src="../AQI/js/SetExpander.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../AQI/js/ProductPubLog.js" type="text/javascript"></script> 
   <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>  
    <script src="../JS/jquery.nicescroll.min.js" type="text/javascript"></script> 
    <script src="../AQI/js/PDFPreview/chili-1.7.pack.js" type="text/javascript"></script>
    <script src="../AQI/js/PDFPreview/jquery.media.js" type="text/javascript"></script>
    <script src="../AQI/js/PDFPreview/jquery.metadata.js" type="text/javascript"></script>
<link href="../css/Title.css" rel="stylesheet" type="text/css" />  
</head>
<body>
<div class="search">
<div class="tableTop">
      <div id="topInfo" class="titleContent">    
        <table width="100%" class="log_table" style="width: 100%;" border="0">
  <tbody><tr>
   <td class="attrName">产品名称</td>
    <td class="attrValue tdName">
        <div class="proNameSelect" id="Div5">
            <div id="Div6" class="dateDiv">
                <div class="proNameText" id="proName">全部</div>
                <div id="Div8" class="selIcon"></div>
            </div>
                <ul id="Ul2" class="firstPolUl hide">
                    <li><div>全部</div></li>
                    <li><div>AQI预报</div></li>
                    <li><div>AQI分区预报</div></li>
                    <li><div>空气污染气象条件</div></li>
                    <li><div>霾预报</div></li>
                    <li><div>紫外线预报</div></li>
                    <li><div>臭氧预报产品</div></li>
                    <li><div>环境专报</div></li>
                    <li><div>专报公报</div></li>
                    <li><div>重点城市预报</div></li>                    
                </ul>
            </div>
      </td>
    <td class="attrName">发布方式</td>
    <td class="attrValue tdPubMethod">
        <div class="pubMethodSelect" id="58367_FirstPol">
            <div id="selectID" class="dateDiv">
                <div class="pubMethodText" id="pubMethod">全部</div>
                <div id="selIcon" class="selIcon"></div>
            </div>
                <ul id="firstPolUl" class="firstPolUl hide">
                    <li><div>全部</div></li>
                    <li><div>FTP</div></li>
                    <li><div>传真</div></li>
                    <li><div>短信</div></li>
                </ul>
            </div>
      </td>
    <td class="attrName">发布时间</td>
    <td class="attrValue_date"><input name="txtPublishStartDate" class="s_k1_left" id="txtPublishStartDate_Search" onclick=" WdatePicker();" type="text" readonly="readonly" value=""/></td>
    <td class="shortline">-</td>
    <td class="attrValue_date_right"><input name="txtPublishEndDate" class="s_k1_right" id="txtPublishEndDate_Search" onclick=" WdatePicker();" type="text" readonly="readonly" value=""/></td>
    <td class="attrName">发布状态</td>
    <td class="attrValue tdPubCon">
        <div class="pubConSelect" id="Div1">
            <div id="Div2" class="dateDiv">
                <div class="pubConText" id="pubCon">全部</div>
                <div id="Div4" class="selIcon"></div>
            </div>
                <ul id="Ul1" class="firstPolUl hide">
                    <li><div>全部</div></li>
                    <li><div>发布成功</div></li>
                    <li><div>发布失败</div></li>                   
                </ul>
            </div>
    </td>
    <td class="attrName">发布人</td>
    <td class="attrValue"><input name="txtPublishClerk_Search" class="s_k3" id="txtPublishClerk_Search" type="text"/></td>
    <td class="attrValue"><div class="button" id="btnSearch">查询</div></td>
  </tr>
</tbody></table>
      </div>
   </div>

</div>
<div class="tableOutLine" id="tableOutLine">
<div class="mapTitle">
           <div class="titlePoint"></div>
           <span>产品发布信息列表</span>
           </div>
<div id="logTable" class="logTable">
</div>
</div>
</body>

</html>
