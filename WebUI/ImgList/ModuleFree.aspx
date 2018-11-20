<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModuleFree.aspx.cs" Inherits="ImgList_ModuleFree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script language="javascript" type="text/javascript"></script>    

    <link href="css/FreeList.css" rel="stylesheet" type="text/css" />
      <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
     <script language="javascript" type="text/javascript" src="../JS/jquery-1.7.2.min.js"></script>

    <script language="javascript" type="text/javascript" src="../JS/jquery.ua.js"></script>
        <script language="javascript" type="text/javascript" src="JS/ImageTimeFrame.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ModuleFree.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
</head>
<body id="Body1" runat="server" style="-webkit-overflow-scrolling:touch; overflow: auto;" >
    <div class="contentNone" >
    <div class="ImgContent" id="ImgContent">
        <div style=" margin-bottom:7px;">
        <img  id="H01" width="48%"  onmouseover="ImgMouseOver('H01')"  src=""   style=" margin-right:10px;" />
        <img  id="H02" width="48%" onmouseover="ImgMouseOver('H02')" src=""  />
        </div>
        <div >
        <img  id="H03" width="48%"  style=" margin-right:10px;" src="" onmouseover="ImgMouseOver('H03')" />
        <img  id="H04" width="48%"   onmouseover="ImgMouseOver('H04')" src="" />
        </div>
    </div>
    <div class="deleteImg" id="delete" onclick="ImgDelete()" onmouseover="this.className='deleteImgOver'"  onmouseout="ImgMouseOut()" style=" display:none;" ></div>
    <div class="ListContent" id="ListContent">
        <div class="label" style="margin-bottom: 5px;">筛选条件</div>
        <div  class="dateSelect" id="areaSelect" runat="server">
        </div>
        <div class="label"   style=" clear:both;"></div>
        <div id="time">
        </div>
         <div id="layers" class="dateSelect" >
        </div>
        <div  id="sure" class="buttonDiv" style=" margin-top:0px;"><input  value="确定" class="addBut" type="button" onclick="addImgButton()"/></div>
        </div>
    </div>
</body>
</html>
