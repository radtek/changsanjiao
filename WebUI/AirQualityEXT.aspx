<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirQualityEXT.aspx.cs" Inherits="AirQualityEXT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <title></title>
    <script language="javascript" type="text/javascript">
        var json = "<%=m_json%>";
        var id = "<%=id %>";
        var parentTxt = "<%=parentTxt %>";
    </script>
     <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/bootstrap-select.min.css" rel="stylesheet" />
   
    <link href="css/PublicAspx.css" rel="stylesheet" type="text/css" />
    <link href="css/AirQualityEXT.css" rel="stylesheet" />
    <link type="text/css" rel="stylesheet" href="Ext/resources/css/ext-all.css" />
    <script type="text/javascript" src="Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="Ext/ext-all.js"></script>
    <script type="text/javascript" src="Ext/ext-lang-zh_CN.js"></script>
    <%--<script language="javascript" type="text/javascript" src="JS/jquery-1.7.2.min.js"></script>--%>
    <script src="JS/jquery-1.10.2.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery.ua.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageFrameOther.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageViewerOtherX.js?v=2"></script>
    <script src="JS/bootstrap.min.js"></script>
    <script src="JS/bootstrap-select.min.js"></script>

    <script language="javascript" type="text/javascript" src="JS/Utility.js"></script>
        <script language="javascript" type="text/javascript" src="JS/AirQualityEXT.js"></script>
    <script language="javascript" type="text/javascript" src="JS/highlight-active-input.js"> </script>
    <script src="DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body id="Body1" runat="server" >
     
    <div class="contentNone1" id="page" style="padding-top: 0px;padding-bottom:0;">
        <div class="header">
            <font class="show-title"></font>
            <div class="show-intro">
                <img style="width:20px;padding-bottom:2px;" src="images/down.png" title="展开" />
            </div>
        </div>
        <div class="introduce">
        </div>
        <div id="contentImg1" runat="server" style="float: left; width: 99%;padding-top:18px;overflow: initial;" >
            <div style="float: left; display: none" id="moduleTypes" class="tab" runat="server">
            </div>
            
            <div id="selectTime" style="float: left; width: 300px;">
                <div class="button">
                    <input type="button" id="Button1" class="leftButton" onclick="ReduceSelect(-1)" /></div>
                <div class="date" id="date">
                    <input name='H00' type="text" id="H00" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd', readOnly: true })" onchange="changeDates(this)" class="formstyle" runat="server" />
                        <%--      <input id="H00" runat="server" type="text" class="DateFormStyle" onchange="changeDate(this)" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/>--%>
                </div>
                <div class="button">
                    <input type="button" id="Button4" class="rightButton" onclick="ReduceSelect(1)" /></div>
            </div>
            <div  class="monthOrday indis">
                <label style="float:left;margin-top: 6px;">时段：</label>
                <div class="month active" ><span>月</span></div>
                <div class="day"><span>日</span></div>
            </div>
            <div style="float:left;" class="condition special">
                 <label>机构：</label>
               <select class="selectpicker" id="coumtry" multiple>
               </select>
           </div>
            <div class="condition" style="float:left;margin-left:30px">
                 <label>区域：</label>
               <select id="area">
               </select>
           </div>
            <div style="float:left;margin-left:30px" class="condition">
                 <label>时效：</label>
               <select id="_period">
               </select>
           </div>
            <div class="condition" style="float:left;margin-left:30px">
                <input type="button" value="查询" onclick="trickQueryList()" class="btn-default btn" />
           </div>
        </div>
        <div class="line1" ></div>
        <div id="contentNone" class="contentNone1" style="overflow: auto;text-align:left;padding:20px 0 0 0;"></div>
        <div class="bg" id="bg" onclick="fadeOut()"></div>
        <div id="showImg" class="hidden">
            <div id="OnlyOne" class="OnlyOne">
            </div>
            <div class="buttonPatton" id="buttonPatton" style="display: none">
                <div class="button">
                    <input type="button" id="leftButton" class="leftButton" onclick="ReduceButton()" /></div>
                <div class="date">
                    <input name="" type="text" id="time" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' })" onchange="changeDate(this)" class="formstyle" runat="server" /></div>
                <div class="hidden" id="period">
                </div>
                <div class="hourBut" id="addBut">
                </div>
                <div id="type" class="hidden"></div>
                <div class="button">
                    <input type="button" id="rightButton" class="rightButton" onclick="addButton()" /></div>
            </div>
        </div>
    </div>
</body>
</html>
