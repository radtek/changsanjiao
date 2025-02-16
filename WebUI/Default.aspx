﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server">
    <title>长三角区域环境气象预报分析平台（1.0版）</title>     
    <link href="css/main.css" rel="stylesheet" type="text/css" />
    <link href="css/icons.css" rel="stylesheet" type="text/css" />
    <link href="css/imageViewer.css" rel="stylesheet" type="text/css" />
    <link href="css/ext-patch.css" rel="stylesheet" type="text/css" />   
<%--     <script language="javascript" type="text/javascript" src="../JS/jquery-1.9.1.js"></script>--%>
    <script language="javascript" type="text/javascript" src="JS/jquery-1.7.2.min.js"></script>

    <script language="javascript" type="text/javascript" src="JS/jquery.ua.js"></script>
     <script language="javascript" type="text/javascript" src="My97DatePicker/WdatePicker.js"></script>
    <style> 
        a{ text-decoration:none} 
        a{ color:#0000FF} 
    </style> 
    <script type="text/javascript" language="javascript">
        var FormPanel;
        var titleNamestr = "";
        var loginValue = "";
        var actionNode = "";
        var selNode = "";
        var isShow = true;
        $(function () {
            cover();
            $(window).resize(function () { //浏览器窗口变化 
                cover();
         
            });
        });
        function cover() {
            var win_width = $(window).width();
            var win_height = $(window).height();
            $("#loginpanelTop").attr({ width: win_width, height: win_height });
        }

        $(window).unload(function () {
            var URLSTR = getCookie("URLSTR");
            if (URLSTR != "exit" && URLSTR != "") {
               // hideDOM();
                addCookie("URLSTR", (titleNamestr + ";" + loginValue + ";" + actionNode), 0);
            }
        }
        );

        function hideDOM() {  

            $("#txtUser").hide();
            $("#txtPWD").hide();
            $("#loginClick").hide();
            $("#loginCancel").hide();
            $("#loginPanel").hide();
            $("#loginpanelTop").hide();
            $("#Div2").hide();       
        }

        function doLogin() {
           // hideDOM();
            isShow = true;
             var URLSTR = getCookie("URLSTR");
            // alert(URLSTR);
             if (URLSTR != undefined && URLSTR != "undefined" && URLSTR != "" && URLSTR != null && URLSTR != ";;") {
                 var strs = URLSTR.split(";");
                 if (strs.length >= 3) {
                     //hideDOM();
                     Ext.getDom("txtUser").value = strs[1];
                     Ext.getDom("txtPWD").value = strs[2];
                     titleNamestr = strs[0];
                     selNode = strs[3];
                     //alert(actionNode);
                
                     isShow = false;
                    // Login("loginPanel", InitialViewer);
                     //$("#loginClick").click();
                 }
             }
           }

        $(window).load(function () {
              Ext.getDom("loginResult").value = "";
              //doLogin();
            }
         );
      
        function addCookie(objName, objValue, objHours) {//添加cookie 
            if (objHours > 0) {
                 var exp = new Date();
                 exp.setTime(exp.getTime() + objHours * 60 * 60 * 1000);
                 document.cookie = objName + "=" + escape(objValue) + ";path=/;expires=" + exp.toGMTString();
                } else {
                 document.cookie = objName + "=" + escape(objValue);
            }
        }

        function delCookie(name) {
            var exp = new Date();
            exp.setTime(exp.getTime() - 1);
            var cval = escape("exit");// getCookie(name);
            document.cookie = name + "=" + cval;
        }

        function getCookie(name) {
            var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
            if (arr != null) return unescape(arr[2]); return null;
        }
</script>
   <style>
        .fram
        {
            position: absolute;
            left:65%;
            top:250px;
            width:22%;  
            background-color:rgba(187, 187, 104, 0.3);
            font-weight:bold;
            border-radius:25px;
            border:1px solid #49444e;
        }
        .to
        {
            text-align: center;
            margin-top: 10px;
            font-size: 22px;     
        }
        .conten
        {
            text-indent: 2em;
            line-height: 25px;
            font-size: 16px;
            margin-top: 15px;
            font-weight:normal;
            margin-bottom:30px;
            padding:0 10px;
        }
    </style>
</head>
<body >
    
     <!-- 全局脚本 --> 
    <script language="javascript" type="text/javascript" src="JS/Utility.js"></script>
     <!-- 登陆界面 --> 
    <script language="javascript" type="text/javascript" src="JS/Login.js"></script>
     <!-- 系统主界面 --> 
    <script language="javascript" type="text/javascript" src="JS/GISToolbar.js"></script>
   
    <script language="javascript" type="text/javascript" src="JS/OtherViewer.js"></script>
    <script language="javascript" type="text/javascript" src="JS/fuHe.js"></script>
    <script language="javascript" type="text/javascript" src="JS/MainViewer.js"></script>
    <script language="javascript" type="text/javascript" src="JS/Password.js"></script>
<%--    <script language="javascript" type="text/javascript" src="JS/jquery.cookie.js"></script>--%>
   <%-- <script language="javascript" type="text/javascript" src="JS/jquery.base64.js"></script>--%>  
    <div id="loginPanel" class="index-body">
        <div class="loginpanelTop" id="loginpanelTop">
            <div class="logo"> <div id="AQI" class="AQIposition" style="cursor:pointer" runat="server"></div></div>
            <div class="login-inuput" id="Div2">
              <div class="login-group">
                  <div class="login-name"></div>
                     <div class="loginform"><input name="" id="txtUser" type="text" class="loginformstyle" /></div>
                  <div class="login-password" style="margin-top: 10px"></div>
                      <div class="loginform" style="margin-top: 10px"><input name="" id="txtPWD" type="password" class="loginformstyle" /></div>
              </div>
 
              <div class="login-btn-area">
                  <input  type="button" class=" normal-btn-login-up " id="loginClick"   onmouseover="this.className='normal-btn-login-over'"  onmouseout="this.className='normal-btn-login-up'" />

                  <input type="button" class=" normal-btn-retry-up" id="loginCancel"  onmouseover="this.className='normal-btn-retry-over'"  onclick="" onmouseout="this.className='normal-btn-retry-up'" />
              </div>
            </div>

        </div>
            <div class="fram">
        <div class="to">【公告】</div>
        <div class="conten">近期系统安全维护访问受到限制，如有问题敬请谅解！</div>
    </div>
        <div class="index-footer">
            <ul>
              <li>版本：Version 1.0</li>
              <li>版权所有：长三角环境气象预报预警中心 </li>
              <li>技术支持：上海地听信息科技有限公司</li>
         </ul>
        </div>
    </div>
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
    
     <form id="form1" runat="server" style="padding: 0px; margin: 0px">
        <asp:HiddenField ID="loginResult" runat="server" />
        <asp:HiddenField ID="ID" runat="server" />
    </form>

</body>

</html>

