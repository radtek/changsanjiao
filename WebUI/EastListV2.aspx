<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EastListV2.aspx.cs" Inherits="EastListV2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
       <title></title>
    <script language="javascript" type="text/javascript">
        var json = "<%=m_json%>";
        var id = "<%=id %>";
    </script>
    
  <link href="css/PublicAspx.css?v=22" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="Ext/ext-all.js"></script>
    <script type="text/javascript" src="Ext/ext-lang-zh_CN.js"></script>
     <script language="javascript" type="text/javascript" src="JS/jquery-1.7.2.min.js"></script>

    <script language="javascript" type="text/javascript" src="JS/jquery.ua.js"></script>
      <script language="javascript" type="text/javascript" src="JS/ImageFrameOtherV2.js"></script>
     <script language="javascript" type="text/javascript" src="JS/ImageViewerOther.js"></script>
    <script language="javascript" type="text/javascript" src="JS/EastListV2.js"></script>
    <script language="javascript" type="text/javascript" src="JS/Utility.js"></script>
    <script language ="javascript" type="text/javascript" src="JS/highlight-active-input.js"> </script>
     <script src="My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body id="Body1" runat="server" style="-webkit-overflow-scrolling:touch; overflow: auto;" >
    <div class="contentNone1"  style=" padding-top:10px;">
    <div id="contentImg" runat="server" style=" float:left; width:84%">
     <div  id="moduleTypes" class="tab1" runat="server">
    </div>
    <div  id="imgHtml" style=" margin-top:28px;overflow: auto;"></div>
    </div>
    <div id="selectTime" class="panel" style=" float:right; width:15%"   >
    </div>
    <div class="bg" id="bg"  onclick="fadeOut()"></div>
    <div id="showImg" class="hidden">
        <div  id="OnlyOne" class="OnlyOne">
        </div>
        <div class="buttonPattonWideII" id="buttonPatton">
          <div id="btnDiv" class="buttonDiv">
            <div class="button"><input  type="button" id="leftButton" class="leftButton" onclick="ReduceButton()"/></div>
            <div class="date"  style=" display:none"><input name="" type="text" id="time" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" onchange="changeDate(this)" class="formstyle" runat="server"/></div>
            <div  class="hidden" id="period">
            </div>
            <div  class="hourBut" id="addBut">
            </div>
            <div  id="type" class="hidden"></div>
            <div class="button"><input  type="button" id="rightButton" class="rightButton" onclick="addButton()"/></div>
        </div>
        </div>
    </div>
    </div>
</body>
</html>
