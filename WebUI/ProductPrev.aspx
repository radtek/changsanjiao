<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductPrev.aspx.cs" Inherits="ProductPrev" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/main_JiangXi.css" rel="stylesheet" type="text/css" />
    <link href="css/icons_JX.css?v=20160510" rel="stylesheet" type="text/css" />
    <link href="css/imageViewer.css" rel="stylesheet" type="text/css" />
    <link href="css/ext-patch.css" rel="stylesheet" type="text/css" />   
    <script language="javascript" type="text/javascript" src="JS/jquery-1.7.2.min.js"></script>
    <link href="Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="Ext/ext-all.js" type="text/javascript"></script>
        <script language="javascript" type="text/javascript" src="JS/Utility.js"></script>
        <script type="text/javascript" language="javascript">
            var FormPanel;
            var titleNamestr = "";
            //        var loginValue = "";
            //        var actionNode = "";
            //        var selNode = "";
            //        var isShow = true;
            //        $(function () {
            //            cover();
            //            $(window).resize(function () { //浏览器窗口变化 
            //                cover();

            //            });
            //        });
            //        function cover() {
            //            var win_width = $(window).width();
            //            var win_height = $(window).height();
            //            $("#loginpanelTop").attr({ width: win_width, height: win_height });
            //        }

            ////        $(window).unload(function () {
            ////            var URLSTR = getCookie("URLSTR");
            ////            if (URLSTR != "exit" && URLSTR != "") {
            ////                hideDOM();
            ////                addCookie("URLSTR", (titleNamestr + ";" + loginValue + ";" + actionNode), 0);
            ////            }
            ////        }
            ////        );

            //        function hideDOM() {
            //            $("#forswitching").hide();
            //            $("#div2").hide();
            //            $("#div3").hide();
            //            $("#div4").hide();
            //            $("#div5").hide();
            //        }

            //        function doLogin() {
            //            // hideDOM();
            //            isShow = true;
            //            var URLSTR = getCookie("URLSTR");
            //            // alert(URLSTR);
            //            if (URLSTR != undefined && URLSTR != "undefined" && URLSTR != "" && URLSTR != null && URLSTR != ";;") {
            //                var strs = URLSTR.split(";");
            //                if (strs.length >= 3) {
            //                    hideDOM();
            //                    Ext.getDom("txtUser").value = strs[1];
            //                    Ext.getDom("txtPWD").value = strs[2];
            //                    titleNamestr = strs[0];
            //                    selNode = strs[3];
            //                    //alert(actionNode);

            //                    isShow = false;
            //                    Login("loginPanel", InitialViewer);
            //                    $("#loginClick").click();
            //                }
            //            }
            //        }

            //        //        $(window).load(function () {
            //        //            Ext.getDom("loginResult").value = "";
            //        //            doLogin();
            //        //        }
            //        //         );

            //        function addCookie(objName, objValue, objHours) {//添加cookie 
            //            if (objHours > 0) {
            //                var exp = new Date();
            //                exp.setTime(exp.getTime() + objHours * 60 * 60 * 1000);
            //                document.cookie = objName + "=" + escape(objValue) + ";path=/;expires=" + exp.toGMTString();
            //            } else {
            //                document.cookie = objName + "=" + escape(objValue);
            //            }
            //        }

            //        function delCookie(name) {
            //            var exp = new Date();
            //            exp.setTime(exp.getTime() - 1);
            //            var cval = escape("exit"); // getCookie(name);
            //            document.cookie = name + "=" + cval;
            //        }

            //        function getCookie(name) {
            //            var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
            //            if (arr != null) return unescape(arr[2]); return null;
            //        }
</script>
     <!-- 登陆界面 --> 
    <script language="javascript" type="text/javascript" src="JS/Login.js"></script>
     <!-- 系统主界面 --> 
    <script language="javascript" type="text/javascript" src="JS/GISToolbarSingleFake.js"></script>
   
    <script language="javascript" type="text/javascript" src="JS/OtherViewer.js"></script>
    <script language="javascript" type="text/javascript" src="JS/fuHe.js"></script>
    
    <script language="javascript" type="text/javascript" src="JS/Password.js"></script>

    <!-- 海洋气象 --> 
    <script language="javascript" type="text/javascript" src="JS/TreeMenu.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageProduct.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ForecastPanel.js"></script>
    <script language="javascript" type="text/javascript" src="JS/FlexGrid.js"></script>
    <script language="javascript" type="text/javascript" src="JS/PatrolCheck.js"></script>
    
    <!-- 图片浏览 --> 
    <script type="text/javascript" src="JS/Chart/jquery.min.js"></script>
    <script type="text/javascript" src="JS/Chart/highstock.js"></script>
    <script type="text/javascript" src="JS/Chart/modules/exporting.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageFrame.js"></script>
    <script language="javascript" type="text/javascript" src="JS/tableQuxian.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageViewer.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery.ua.js"></script>
    <script language="javascript" type="text/javascript" src="My97DatePicker/WdatePicker.js"></script>    
    <script src="JS/ProductPrev.js" type="text/javascript"></script>
    <style> 
        a{ text-decoration:none} 
        a{ color:#0000FF} 
       html,body{ margin:0px; height:100%;} 
    </style> 
    
<script language="javascript" type="text/javascript" src="JS/toMainViewerSingleFunc.js"></script>
</head>
<body>
<form id="form1" runat="server" style="padding: 0px; margin: 0px">
        <input runat="server" type="hidden" id="loginResult" />        
    </form>
</body>
</html>
