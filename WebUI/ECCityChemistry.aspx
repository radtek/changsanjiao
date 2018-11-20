<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ECCityChemistry.aspx.cs" Inherits="ECCity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <script language="javascript" type="text/javascript">
       var column = "<%=m_column%>";
       var totalCount = "<%=m_totalCount%>";
       var id = "<%=id %>";
       var width = "<%=m_width %>"
       var period = "<%=period%>"
    </script>
    <script language="javascript" type="text/javascript"></script>    
    <link href="css/PublicAspx.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="Ext/ext-all.js"></script>
    <script type="text/javascript" src="Ext/ext-lang-zh_CN.js"></script>
     <script language="javascript" type="text/javascript" src="JS/jquery-1.7.2.min.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery.ua.js"></script>
   <script language="javascript" type="text/javascript" src="JS/ECCity.js"></script>
    <script language="javascript" type="text/javascript" src="JS/Utility.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageFrameECCity.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageViewerOther.js"></script>
        <script language ="javascript" type="text/javascript" src="JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="DatePicker/WdatePicker.js"></script>

</head>
<body id="Body1" runat="server" style="-webkit-overflow-scrolling:touch; overflow: auto;" >
    <div class="contentNone1"  style=" padding-top:10px;">
    <div id="contentImg" runat="server" style=" float:left; width:84%">
     <div  id="moduleTypes" class="tab2" runat="server">
    </div>
    <div  id="imgHtml" style=" margin-top:28px;"></div>
    </div>
    <div id="selectTime" class="panel" style=" float:right; width:15%"   >
    </div>
    <div class="bg" id="bg"  onclick="fadeOut()"></div>
    <div id="showImg" class="hidden">
        <div  id="OnlyOne" class="OnlyOne">
        </div>
        <div class="buttonPatton" id="buttonPatton">
            <div class="button"><input  type="button" id="leftButton" class="leftButton" onclick="ReduceButton()"/></div>
            <div class="date"><input name="" type="text" id="time" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" onchange="changeDate(this)" class="formstyle" runat="server"/></div>
            <div  class="hidden" id="period">
            </div>
            <div  class="hourBut" id="addBut">
            </div>
            <div  id="type" class="hidden"></div>
            <div class="button"><input  type="button" id="rightButton" class="rightButton" onclick="addButton()"/></div>
        </div>
    </div>
    </div>
    <div id="replaceImg" style="display:none">替换图片</div>
</body>
</html>

