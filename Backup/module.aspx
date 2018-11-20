<%@ Page Language="C#" AutoEventWireup="true" CodeFile="module.aspx.cs" Inherits="module" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>模式数据</title>
    <script language="javascript" type="text/javascript">
            var module="<%=m_module %>"
            var polluteStyle="<%=m_polluteStyle %>"
    </script>
 <link href="AQI/images/css/module.css" rel="stylesheet" type="text/css" />
 <link href="AQI/images/css/css.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="Ext/ext-all.js"></script>
 <script type="text/javascript" src="Ext/ext-lang-zh_CN.js"></script>
 
 <script language="javascript" type="text/javascript" src="AQI/js/AQIUtility.js"></script>
 <script language="javascript" type="text/javascript" src="JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="AQI/js/module.js"></script>
 <script language="javascript" type="text/javascript" src="DatePicker/WdatePicker.js"></script>
</head>
<body>
    <div id="contentNone" style="margin-top: 10px">
    <div style="font-size: 15px; text-align: center; line-height: 15px; font-family: 微软雅黑; font-weight: bold;" class="tool">污染物浓度与气象场空间分布图</div>
        <div class="checkStyleH">
             <div class="checkLable">预报日期：</div>
             <input id="H00" type="text" class="selectDateFormStyle"  value="<%= m_FromDate%>" onchange="changeDate(this)" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/>
        </div>
       <div id="zone" class="tool">
           <label >区域：</label>
           <label class="forecast"><input type="radio" name="zone" checked ="checked"  id="china"  value ="D1"  onclick="changeClick()"/>全国</label>
           <label class="forecast"><input type="radio" name="zone"  id="east" value ="D2"  onclick="changeClick()"/>华东</label>
           <label class="forecast"><input type="radio" name="zone" id="csj"   value ="D3" onclick="changeClick()" />长三角</label>
           <label class="forecast"><input type="radio" name="zone" id="shanghai"  value ="D4"  onclick="changeClick()"/>上海</label>
       </div>   
       <iframe  id="iframePage" width="100%"  height="650px" frameborder="0" style="margin-top: 10px"></iframe>
   </div>
</body>
</html>
