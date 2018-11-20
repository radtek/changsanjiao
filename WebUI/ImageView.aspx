<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImageView.aspx.cs" Inherits="ImageView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link rel="stylesheet" href="css/plugin/bootstrap.css">
     <link rel="stylesheet" href="css/icon.css">
     <link rel="stylesheet" href="css/imageViewer.css">
    <link rel="stylesheet" href="css/default.css">
    <link rel="stylesheet" href="css/utility.css">
        <script language="javascript" type="text/javascript">
            var id = "<%=m_id %>"
    </script>
    <!--[if lte IE 9]>
    <script src="js/plugin/respond.min.js"></script>
    <script src="js/plugin/html5.js"></script>
    <![endif]-->
</head>
<body style=" padding:0 0 0 0" >
 <div runat="server" id="result" style=" display:none;"  ></div>
     <div  runat="server" id="id" style=" display:none;"></div>
    <div id="imgBody" style=" width:100%;">
        <div id='ImageView' class='ImageView'></div>
        <div id='TimeView' class='TimeView'>
        <div id='topHtml'></div>
        <div id='bottomHtml'></div>
        <div style=" margin-top:10px;">
            <div class="speedButton">
            <select id="speed" style=" cursor:pointer;">
            <option value="1">1</option>
            <option value="2">2</option>
            <option value="3">3</option>
            <option value="5">5</option>
            <option value="10">10</option>
            </select>
            </div>
            <div class="speedClas" >
            <div class=" SpeennormalSlow  " onmouseover="this.className='SpeennormalSlowOver'" onmouseout="this.className='SpeennormalSlow'" onclick="playSpeed(this,100)"></div>
            <div class="Speennormal_D"   onclick="playToggle(this)" id="move"></div>
            <div class=" SpeennormalQuick " onmouseover="this.className='SpeennormalQuickOver'" onmouseout="this.className='SpeennormalQuick'" onclick="playSpeed(this,-100)"></div>
            </div>
        </div>
        </div>
    </div>
    <script src="js/plugin/jquery-1.11.3.min.js"></script>
    <script src="js/plugin/bootstrap.min.js"></script>
    <script src="js/utility.js"></script>
     <script language="javascript" type="text/javascript" src="DatePicker/WdatePicker.js"></script>
   <script type="text/javascript" src="Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="Ext/ext-all.js"></script>
    <script type="text/javascript" src="Ext/ext-lang-zh_CN.js"></script>
     <script language="javascript" type="text/javascript" src="js/ImageViewer.js"></script>
    <script language="javascript" type="text/javascript" src="js/ImageFrame.js"></script>
          <script language="javascript" type="text/javascript" src="js/jquery.nicescroll.min.js"></script>
        <script language="javascript" type="text/javascript" src="js/ImageViewBody.js"></script>
</body>
</html>
