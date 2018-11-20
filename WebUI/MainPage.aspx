<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPage.aspx.cs" Inherits="MainPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <title>长三角环境气象业务平台</title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<%--<%--    <link type="text/css" rel="stylesheet" href="css/plugin/bootstrap.css">--%>
     <link type="text/css" rel="stylesheet" href="Ext/resources/css/ext-all.css"/>
	 <link rel="stylesheet" href="css/icon.css" />
	 <link rel="stylesheet" href="css/treePanel.css" />
    <link rel="stylesheet" href="css/imageViewer.css" />
    <link rel="stylesheet" href="css/MainPage.css" />
    <link rel="stylesheet" href="css/main.css" />
    <link rel="stylesheet" href="css/utility.css">
    	<link rel="stylesheet" type="text/css" href="css/plugin/easyui.css">
	<link rel="stylesheet" type="text/css" href="css/plugin/icon.css">
	<script type="text/javascript" src="js/plugin/jquery.min.js"></script>
	<script type="text/javascript" src="js/plugin/jquery.easyui.min.js"></script>
 
</head>
<body>
    <header>
        <img src="images/logo.png" style=" margin-top:16px;" />
            <nav><ul></ul></nav>
            <div class="buttons_user" >
            <div class="user_img" id="userName" runat="server">测试</div>
            <div  id="JB" runat="server" style=" display:none;"></div>
             <div><div class="icon_img_left" onmouseover="this.className='icon_img_left_d'" onmouseout="this.className='icon_img_left'"></div><div class="icon_img_right" onmouseover="this.className='icon_img_right_d'" onmouseout="this.className='icon_img_right'" onclick="exit()"></div></div>
            </div>
    </header>
    	<div class="easyui-panel"  style="width:100%;height:100%; position:fixed" id="otherHtml" >
		<div class="easyui-layout" data-options="fit:true">
			<div data-options="region:'west'" style="width:72px;" id="addMunu">
           
                 <main><ul></ul></main>
                 
  
			</div>
			<div data-options="region:'center'" id="mainPanel">
             <div class="easyui-layout" data-options="fit:true" id="collapsed">
              <div data-options="region:'west'"  id="leftMenu" style="width:200px;" >
            </div>
            <div class="collapseBtn bit" title="收缩"><a class="ishow"></a></div>
            <div data-options="region:'center'"   id="mainContent">
            </div>
             </div>

			</div>
            <div data-options="region:'center'" id="qyyj" style=" display:none;">

			</div>

		</div>
	</div>
    <div id="GisHtml" style=" width:100%; height:100%;">
    </div>
<%--    <div class="arrow_collapsed up_collapsed"  onclick="collapsePanel()"></div>--%>
     <script src="js/plugin/jquery-1.11.3.min.js"></script>
    <script src="js/plugin/bootstrap.min.js"></script>
    	 <script type="text/javascript" src="Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="Ext/ext-all.js"></script>
    <script type="text/javascript" src="Ext/ext-lang-zh_CN.js"></script>
    <script src="js/utility.js"></script>

        <script language="javascript" type="text/javascript" src="DatePicker/WdatePicker.js"></script>
     <script language="javascript" type="text/javascript" src="js/ImageViewer.js"></script>
    <script language="javascript" type="text/javascript" src="js/ImageFrame.js"></script>
      <script language="javascript" type="text/javascript" src="js/jquery.nicescroll.min.js"></script>
    <script src="js/MainPage.js"></script>
</body>
</html>
