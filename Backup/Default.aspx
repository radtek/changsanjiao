<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server">
    <title>长三角区域空气质量预报业务平台（1.0版）</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link rel="shortcut icon" href="images/Readearth.ico" />
    <link rel="icon" href="images/Readearth.ico" />
    <link href="css/main.css" rel="stylesheet" type="text/css" />
    <link href="css/icons.css" rel="stylesheet" type="text/css" />
    <link href="css/imageViewer.css" rel="stylesheet" type="text/css" />
    <link href="css/ext-patch.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="JS/jquery-1.7.2.min.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery.ua.js"></script>
    <style> 
        a{ text-decoration:none} 
        a{ color:#0000FF} 
    </style> 
    <script type="text/javascript" language="javascript">
        
        $(function() {
            cover();
            $(window).resize(function() { //浏览器窗口变化 
                cover();
            });
        });
        function cover() {
            var win_width = $(window).width();
            var win_height = $(window).height();
//            alert(win_width); alert(win_height);
            $("#loginpanelTop").attr({ width: win_width, height: win_height });
        }
</script>
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
    <div id="loginPanel" class="index-body">
        <div class="loginpanelTop" id="loginpanelTop">
            <div class="logo"></div>
            <div class="login-inuput" id="Div2">
              <div class="login-group">
                  <div class="login-name"></div>
                     <div class="loginform"><input name="" id="txtUser" type="text" class="loginformstyle" /></div>
                  <div class="login-password" style="margin-top: 10px"></div>
                      <div class="loginform" style="margin-top: 10px"><input name="" id="txtPWD" type="password" class="loginformstyle" /></div>
              </div>
 
              <div class="login-btn-area">
                  <input type="button" class="normal-btn login-btn leftColor" id="loginClick" value="登录"  onmouseover="this.className='normal-btn-h login-btn leftColor'"  onmouseout="this.className='normal-btn login-btn leftColor'" onmousedown="this.className='normal-btn-d login-btn leftColor'" onmouseup ="this.className='normal-btn login-btn leftColor'"/>

                  <input type="button" class="normal-btn login-btn rightColor" id="loginCancel" value="重置" onmouseover="this.className='normal-btn-h login-btn rightColor'"  onclick="" onmouseout="this.className='normal-btn login-btn rightColor'" onmousedown="this.className='normal-btn-d login-btn rightColor'" onmouseup ="this.className='normal-btn login-btn rightColor'"/>
              </div>
            </div>

        </div>
        <div class="index-footer">
            <ul>
              <li>版本：Version 1.0</li>
              <li>版权所有：上海市环境监测中心</li>
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

