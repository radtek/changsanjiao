<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UVForecast.aspx.cs" Inherits="ReportProduce_UVForecast" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <link href="../css/UVForecast.css" rel="stylesheet" type="text/css" />
    <script src="../AQI/js/UVForecast.js" type="text/javascript"></script>
<link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="top">
    <div class="top_left"></div>
    <div class="top_mid">
    <img width="16" height="16" src="image/tubiao.png" alt="tubiao" />
    紫外线预报</div>
    <div class="top_right"></div>
    </div>

    <div class="all">
    <div class="lable1">
    <table class="table" width="100%" cellspacing="0" cellpadding="0" border="0">
    <tbody>
    <tr>
    <th class="one" scope="col">
    <div class="photo">基本信息</div>
    </th>
    <th scope="col"></th>
    <th class="style1" scope="col">预报员：</th>
    <th scope="col"><input id="txtForcastClerk" class="inputgetdata" type="text" value="" readonly="readonly" name="txtForcastClerk" /></input></th>
    <th class="style1" scope="col">预报时间：</th>
    <th scope="col"><input id="txtForcastDate" class="inputgetdata" type="text" value="" readonly="readonly" name="txtForcastDate" /></input>
    </th>
    <th scope="col">制作频次：10时制作</th>
    <th scope="col">
    <input id="txtUVType" class="inputgetdata" type="text" value="" readonly="readonly" name="txtUVType" /></input></th>
    <th class="thbutton" scope="col"></th>
    </tr>
    <tr>
    <td class="tdtable" rowspan="2" colspan="8">
    <table class="report" cellspacing="5" cellpadding="0" border="0">
    <tr>
    <th class="style2" scope="row">实况值：</th>
    <td class="textbox" ime-mode: disabled;">
    <input id="txtAvgUVAB" type="text" value="" maxlength="4" name="inputID" /></input>w/m <sup>2</sup></td>
    <input type="hidden" value="" /></input>
    <th scope="row"></th>
    <td class="tdtext"></td>
    <th scope="row"> 预报指数：</th>
    <td class="textbox"><input id="txtUVIndex" type="text" maxlength="3" value="" name="txtUVIndex" /></input></td>
    <th scope="row">(紫外线等级：<span class="lblUVLevelDesc"></span>）</th>
    <td class="tdtext"></td>
    </tr>
    </table>
    </td>
    </tr>
    <tr><td>
    <input id="btnAutoCreate" class="button" type="button" value="获取实况" /></input>
    <input id="btnLastOne" class="button" type="button" value="历史记录" /></input>
    </td></tr></tbody>
    </table>
    </div><br /><br />
    
    <div class="lable1">
    <table class="table" width="100%" cellspacing="0" cellpadding="0" border="0">
    <tbody><tr>
    <th class="one" scope="col">
    <div class="photo">防护措施</div></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th class="thbutton" scope="col"></th>
    </tr>
    <tr>
    <td colspan="9">
    <textarea id="txtUVDefense" style="width:99%;height:45px;" rows="2" cols="20" name="txtarea1"></textarea>
    </td>
    </tr>
    </tbody>
    </table>
    <br /><br />

    <table class="table" width="100%" cellspacing="0" cellpadding="0" border="0">
    <tbody><tr>
    <th class="one" scope="col">
    <div class="photo">产品结果</div></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th scope="col"></th>
    <th class="thbutton" scope="col"></th>
    </tr>
    <tr>
    <td colspan="9">
    <textarea id="txtResult" style="width:99%;height:45px;" rows="2" cols="20" name="txtarea2"></textarea>
    </td>
    </tr>
    <tr>
    <td colspan="9">
    <textarea id="txtResult_Tomorrow10" style="width:99%;height:45px;" rows="2" cols="20" name="txtarea3"></textarea>
    </td>
    </tr>
    </tbody>
    </table>
    </div><br /><br />
    </div>

    <div id="lastbutton" class="submit" disabled="disabled">
    <input name="txtHideUVType" type="hidden" id="txtHideUVType" value=<%=ForecastType %>>
    <input id="savebtutton" class="submitbutton" type="button" value="保存" /></input>
    <input id="authbutton" class="submitbutton" type="button" style="display:none" value="审核" /></input>
    <input id="pulishbutton" class="submitbutton" type="button" style="" value="发布"  /></input>
    </div>
    <textarea name="txtHideTempleteContent" id="txtHideTempleteContent" cols="20" rows="2" style="display:none;">58362 1{UVLevel}000 </textarea>
    <textarea name="txtHideTempleteContent_Tomorrow10" id="txtHideTempleteContent_Tomorrow10" cols="20" rows="2" style="display:none;">58362 1{UVLevel}000 {10}</textarea>
</body>
</html>

