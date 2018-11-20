<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OzoneWarning.aspx.cs" Inherits="ReportProduce_OzoneWarning" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../AQI/js/OzoneWarning.js" type="text/javascript"></script>
    <link href="../css/OzoneWarning.css" rel="stylesheet" type="text/css" />
    <script src="../AQI/js/main.js" type="text/javascript"></script>
    <SCRIPT language=javascript event=NotifyCtrlReady for=WebOffice1>
/****************************************************
*
*	在装载完Weboffice(执行<object>...</object>)
*	控件后执行 "WebOffice1_NotifyCtrlReady"方法
*
****************************************************/
	WebOffice1_NotifyCtrlReady()
</SCRIPT>

    <SCRIPT language=javascript event=NotifyWordEvent(eventname) for=WebOffice1>
    <!--
     WebOffice1_NotifyWordEvent(eventname)
    //-->
    </SCRIPT>

    <SCRIPT language=javascript>
        /****************************************************
        *
        *		控件初始化WebOffice方法
        *
        ****************************************************/
        function WebOffice1_NotifyCtrlReady() {
            document.all.WebOffice1.OptionFlag |= 128;
            // 新建文档
            document.all.WebOffice1.LoadOriginalFile("", "doc");

        }
        var flag = false;
        function menuOnClick(id) {
            var id = document.getElementById(id);
            var dis = id.style.display;
            if (dis != "none") {
                id.style.display = "none";

            } else {
                id.style.display = "block";
            }
        }
        /****************************************************
        *
        *		接收office事件处理方法
        *
        ****************************************************/
        var vNoCopy = 0;
        var vNoPrint = 0;
        var vNoSave = 0;
        var vClose = 0;
        function no_copy() {
            vNoCopy = 1;
        }
        function yes_copy() {
            vNoCopy = 0;
        }


        function no_print() {
            vNoPrint = 1;
        }
        function yes_print() {
            vNoPrint = 0;
        }


        function no_save() {
            vNoSave = 1;
        }
        function yes_save() {
            vNoSave = 0;
        }
        function EnableClose(flag) {
            vClose = flag;
        }
        function CloseWord() {

            document.all.WebOffice1.CloseDoc(0);
        }

        function WebOffice1_NotifyWordEvent(eventname) {
            if (eventname == "DocumentBeforeSave") {
                if (vNoSave) {
                    document.all.WebOffice1.lContinue = 0;
                    alert("此文档已经禁止保存");
                } else {
                    document.all.WebOffice1.lContinue = 1;
                }
            } else if (eventname == "DocumentBeforePrint") {
                if (vNoPrint) {
                    document.all.WebOffice1.lContinue = 0;
                    alert("此文档已经禁止打印");
                } else {
                    document.all.WebOffice1.lContinue = 1;
                }
            } else if (eventname == "WindowSelectionChange") {
                if (vNoCopy) {
                    document.all.WebOffice1.lContinue = 0;
                    //alert("此文档已经禁止复制");
                } else {
                    document.all.WebOffice1.lContinue = 1;
                }
            } else if (eventname == "DocumentBeforeClose") {
                if (vClose == 0) {
                    document.all.WebOffice1.lContinue = 0;
                } else {
                    //alert("word");
                    document.all.WebOffice1.lContinue = 1;
                }
            }
            //alert(eventname); 
        }
    </SCRIPT>
</head>
<body onunload="window_onunload();">
<div id="content">
    <div class="tableTop">
          <div id="topInfo" class="titleContent">
            <table>
                <tr><th class="thTab"></th><th class="attrName">预报员：</th><td class="attrValue" id="forecaster"></td><th class="attrName">预报时间：</th><td id="forecastTime" class="attrValue"></td><th class="attrName">预报时次：</th><td id="forecastTimeLevel" class="attrValue"></td><td></td><th class="thButton"></th></tr>
            </table>
        </div>
    </div>
    <table>
       <tr>
          <th></th>
          <th>预警信号：</th>
          <th>
         
           <div id="radArea" class="radWarnType">
           <input class="radioTypeItem" name="radWarn" type="radio" id="warnNew" value="1"/><label for="warnNew">新发</label>
           <input class="radioTypeItem" name="radWarn" type="radio" id="warnUpdate" value="2"/><label for="warnUpdate">更新</label>
           <input class="radioTypeItem" name="radWarn" type="radio" id="warnCancel" value="3"/><label for="warnCancel">解除</label>
        </div>
        
   
          </th>
          <th>预警等级：</th>
          <th>
          <div id="warningLevelArea" class="radWarnLevel">
           <input class="radioColorItem" name="radWarnLevel" type="radio" id="warnYellow" value="Yellow"/><label for="warnYellow">黄色预警</label>
           <input class="radioColorItem" name="radWarnLevel" type="radio" id="warnOrange" value="Orange"/><label for="warnYellow">橙色预警</label>
           <input class="radioColorItem" name="radWarnLevel" type="radio" id="warnRed" value="Red"/><label for="warnYellow">红色预警</label>
        </div>
          </th>
          <th></th>
          <th></th>
          <th></th>
          <th></th>
       </tr>
    </table>    
    <div id="productContent" class="warnTable"></div>
</div>

<table class="reportArea">
    <tr><td class="reportTitle"<span>预报用语：</span></td><td class="reportContent"><textarea id="reportText" ></textarea></td></tr>
</table>
  



<div class="wordContent">  
    <script src="../AQI/js/LoadWebOffice.js" type="text/javascript"></script>
</div>
    <br/>
    <div class="submit">
        <input id="btnSave" class="submitbutton" type="button" value="保存" />
        <input id="btnPublish" class="submitbutton" type="button" value="发布" />        
    </div>
    <input name="txtHideHostName" type="hidden" id="txtHideHostName" value="http://localhost:21765/WebUI/"/>
</body>
</html>
