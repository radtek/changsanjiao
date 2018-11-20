<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmptyWebOffice.aspx.cs" Inherits="AQI_EmptyWebOffice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/AQIArea.css" rel="stylesheet" type="text/css" />
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../AQI/js/main.js" type="text/javascript"></script>
    <SCRIPT language=javascript event=NotifyCtrlReady for=WebOffice1>
/****************************************************
*
*	在装载完Weboffice(执行<object>...</object>)
*	控件后执行 "WebOffice1_NotifyCtrlReady"方法
*
****************************************************/
	WebOffice1_NotifyCtrlReady();

</SCRIPT>

    <SCRIPT language=javascript event=NotifyWordEvent(eventname) for=WebOffice1>
    <!--
//     WebOffice1_NotifyWordEvent(eventname)
 WebOffice1_NotifyWordEvent(okSave)
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
        //var vNoSave = 0;
        var vNoSave = 1;
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
        }
    </SCRIPT>

</head>
<body onunload="window_onunload();">
    <div style="height:700px;width:200px;overflow:">
        <script src="WebOffice/LoadWebOffice.js" type="text/javascript"></script>       
    </div>
    <div onclick=" newSave();" style="height:30px;width:100px;background-color:Black;"></div>
</body>
</html>
