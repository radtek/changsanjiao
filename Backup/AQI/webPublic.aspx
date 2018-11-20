<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webPublic.aspx.cs" Inherits="AQI_webPublic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>网站发布情况</title>
  <script language="javascript" type="text/javascript"></script> 
  <link href="images/css/WebPublic.css" rel="stylesheet" type="text/css" />
 <link href="images/css/css.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 
 <script language="javascript" type="text/javascript" src="js/AQIUtility.js"></script>
 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="js/webPublic.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
</head>
<body>
    <div id="contentIframe" class="contentIframe" style="overflow: hidden">
         <iframe id="SHSSFB" src="http://www.semc.gov.cn/aqi/home/Index.aspx" scrolling="no" class="ssfbContent" frameborder="0" style="position: relative"></iframe>
         <div class ="ContentNoneDiv">
         <div  class="splitDiv">
            <div class="labelHuanjing">上海环境</div>
             <iframe id="Iframe1" src="http://www.sepb.gov.cn/fa/cms/shhj/aqi_city_login.jsp" scrolling="no" class="huanJingOne"  frameborder="0"></iframe>
             <iframe id="Iframe2" src="http://www.sepb.gov.cn/fa/cms/shhj/aqi_site_login.jsp" scrolling="no"  class="huanJingTwo" frameborder="0"></iframe>
        </div>
        <div class="huanjingReDiv">
            <div class="labelHuanjing">上海热线</div>
             <iframe id="Iframe3" src="http://www.envir.gov.cn/aqi.asp"  class="huanJingReX"   frameborder="0"></iframe>
        </div>
        </div>
        <div class="ContentDiv">
         <div class="labelHuanjing">微博</div>
            <table style="width: 1046px">
            <tr>
            <td class="wBButtonTitle" ><a onclick="window.open('http://weibo.com/p/1001061787537264/manage');" href="#">新浪微博</a></td>
            <td class="wBButtonTitle" ><a onclick="window.open('http://t.qq.com/shhjbh');" href="#">腾讯微博</a></td>
            <td class="wBButtonTitle" ><a onclick="window.open('http://weibo.com');" href="#">新浪微博</a></td>
            <td class="wBButtonTitle" ><a onclick="window.open('http://weibo.com');" href="#">新浪微博</a></td>
            </tr>
            <tr>
            <td class="wBButton">用户名：vlutao@163.com</td>
            <td class="wBButton">用户名:2629600220</td>
            <td class="wBButton">用户名</td>
            <td class="wBButton">用户名</td>
            </tr>
            <tr>
            <td class="wBButton">密码：sepb$23111111$</td>
            <td class="wBButton">密码:sepb$23111111$</td>
            <td class="wBButton">密码</td>
            <td class="wBButton">密码</td>
            </tr>
            </table>
        </div>
    </div>
</body>
</html>
