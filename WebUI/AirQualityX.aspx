<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirQualityX.aspx.cs" Inherits="AirQualityX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta http-equiv="X-UA-Compatible" content="IE=9" />
     <title></title>
    <script language="javascript" type="text/javascript">
        var json = "<%=m_json%>";
        var id = "<%=id %>";
    </script>
    <link href="css/PublicAspx.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="Ext/ext-all.js"></script>
    <script type="text/javascript" src="Ext/ext-lang-zh_CN.js"></script>
     <script language="javascript" type="text/javascript" src="JS/jquery-1.7.2.min.js"></script>

    <script language="javascript" type="text/javascript" src="JS/jquery.ua.js"></script>
      <script language="javascript" type="text/javascript" src="JS/ImageFrameOther.js"></script>
     <script language="javascript" type="text/javascript" src="JS/ImageViewerOtherX.js?v=2"></script>
    <script language="javascript" type="text/javascript" src="JS/AirQualityX.js"></script>
    <script language="javascript" type="text/javascript" src="JS/Utility.js"></script>
    <script language ="javascript" type="text/javascript" src="JS/highlight-active-input.js"> </script>
    <script src="DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body id="Body1" runat="server" style="-webkit-overflow-scrolling:touch; overflow: auto;" >
    <div class="contentNone1"  style=" padding-top:10px;">
    <div id="contentImg" runat="server" style=" float:left; width:99%">
      <div style=" float:left;display:none" id="moduleTypes" class="tab" runat="server" >
      </div>
    <div id="selectTime" style=" float:left; width:400px;">
     <div class="button" ><input  type="button" id="Button1" class="leftButton" onclick="ReduceSelect(-1)"/></div>
     <div  class="date" id="date">
     <input name='H00'  type="text" id="H00" onclick="WdatePicker({dateFmt:'yyyy-MM-dd',readOnly:true})"   onchange="changeDates(this)" class="formstyle" runat="server"/>

<%--      <input id="H00" runat="server" type="text" class="DateFormStyle" onchange="changeDate(this)" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/>--%>
     </div>
       <div class="button" ><input  type="button" id="Button4" class="rightButton" onclick="ReduceSelect(1)"/></div>
    </div>

    </div>
    <div  id="contentNone" class="contentNone1"  style=" margin-top:28px;overflow: auto;"></div>
    <div class="bg" id="bg"  onclick="fadeOut()"></div>
    <div id="showImg" class="hidden">
        <div  id="OnlyOne" class="OnlyOne">
        </div>
        <div class="buttonPatton" id="buttonPatton" style="display:none">
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
</body>
</html>
