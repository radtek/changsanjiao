<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JiangXiHomePage.aspx.cs" Inherits="JiangXiHomePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"  runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <title>长三角区域环境气象预报分析平台（1.0版）江西用户</title>    
    <link href="css/main_JiangXi.css" rel="stylesheet" type="text/css" />
    <link href="css/icons.css" rel="stylesheet" type="text/css" />
    <link href="css/imageViewer.css" rel="stylesheet" type="text/css" />
    <link href="css/ext-patch.css" rel="stylesheet" type="text/css" />   
    <script language="javascript" type="text/javascript" src="JS/jquery-1.7.2.min.js"></script>
    <script src="Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="Ext/ext-all.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery.ua.js"></script>
     <script language="javascript" type="text/javascript" src="My97DatePicker/WdatePicker.js"></script>
    <%--<script src="AQI/js/JiangXiHomePage.js" type="text/javascript"></script>--%>
    <style> 
        a{ text-decoration:none} 
        a{ color:#0000FF} 
       html,body{ margin:0px; height:100%;} 
    </style> 
    <script type="text/javascript" language="javascript">
        var FormPanel;
        var titleNamestr = "";
        var loginValue = "";
        var actionNode = "";
        var selNode = "";
        var isShow = true;
        $(function () {
            var loginParams = "<% =logInfo %>";
             $("#outPdPreview").click(function () {
                    //            window.location.href = "http://222.66.83.21:8282/PEMFCShare/ProductPrev.aspx?User=" + logResult["Alias"] + "&JB=" + logResult["JB"];
                   //window.location.href = "http://222.66.83.21:8282/PEMFCShare/ProductPrev.aspx?logPara=" + loginParams;
                 //                      window.location.href = "ProductPrev.aspx?logPara=" + loginParams;
                 window.location.href = "ProductPrev.aspx";
                });
                $("#InteractiveOUT").click(function () {
                    window.location.href = "InteractAna.aspx?V=1";
                });
                $("#productMake").click(function () {
                    window.location.href = "MakeAndPub.aspx?V=1";
                });
                $("#dataService").click(function () {
                    window.location.href = "DataService.aspx?V=1";
                });
//            if (JB == "666") {
//                $("#outPdPreview").click(function () {
//                    //            window.location.href = "http://222.66.83.21:8282/PEMFCShare/ProductPrev.aspx?User=" + logResult["Alias"] + "&JB=" + logResult["JB"];
//                    window.location.href = "http://222.66.83.21:8282/PEMFCShare/ProductPrev.aspx?logPara=" + loginParams;

//                });
//                $("#InteractiveOUT").click(function () {
//                    window.location.href = "InteractAna.aspx?V=1";
//                });
//                $("#productMake").click(function () {
//                    window.location.href = "MakeAndPub.aspx?V=1";
//                });
//                $("#dataService").click(function () {
//                    window.location.href = "DataService.aspx?V=1";
//                });
//            }
//            else {
//                window.location.href = "Default.aspx";
//            }

            cover();
            $(window).resize(function () { //浏览器窗口变化 
                cover();

            });
        });
        function cover() {
            var win_width = $(window).width();
            var win_height = $(window).height();
            $("#loginpanelTop").attr({ width: win_width, height: win_height });
        }

        $(window).unload(function () {
            var URLSTR = getCookie("URLSTR");
            if (URLSTR != "exit" && URLSTR != "") {
                hideDOM();
                addCookie("URLSTR", (titleNamestr + ";" + loginValue + ";" + actionNode), 0);
            }
        }
        );

        function hideDOM() {
            $("#forswitching").hide();
            $("#div2").hide();
            $("#div3").hide();
            $("#div4").hide();
            $("#div5").hide();
        }

        function doLogin() {
            // hideDOM();
            isShow = true;
            var URLSTR = getCookie("URLSTR");
            // alert(URLSTR);
            if (URLSTR != undefined && URLSTR != "undefined" && URLSTR != "" && URLSTR != null && URLSTR != ";;") {
                var strs = URLSTR.split(";");
                if (strs.length >= 3) {
                    hideDOM();
                    Ext.getDom("txtUser").value = strs[1];
                    Ext.getDom("txtPWD").value = strs[2];
                    titleNamestr = strs[0];
                    selNode = strs[3];
                    //alert(actionNode);

                    isShow = false;
                    Login("loginPanel", InitialViewer);
                    $("#loginClick").click();
                }
            }
        }

        //        $(window).load(function () {
        //            Ext.getDom("loginResult").value = "";
        //            doLogin();
        //        }
        //         );

        function addCookie(objName, objValue, objHours) {//添加cookie 
            if (objHours > 0) {
                var exp = new Date();
                exp.setTime(exp.getTime() + objHours * 60 * 60 * 1000);
                document.cookie = objName + "=" + escape(objValue) + ";path=/;expires=" + exp.toGMTString();
            } else {
                document.cookie = objName + "=" + escape(objValue);
            }
        }

        function delCookie(name) {
            var exp = new Date();
            exp.setTime(exp.getTime() - 1);
            var cval = escape("exit"); // getCookie(name);
            document.cookie = name + "=" + cval;
        }

        function getCookie(name) {
            var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
            if (arr != null) return unescape(arr[2]); return null;
        }
</script>
</head>
<body>
    
     <!-- 全局脚本 --> 
    <script language="javascript" type="text/javascript" src="JS/Utility.js"></script>
     <!-- 登陆界面 --> 
    <script language="javascript" type="text/javascript" src="JS/Login.js"></script>
     <!-- 系统主界面 --> 
    <script language="javascript" type="text/javascript" src="JS/GISToolbar.js"></script>
   
    <script language="javascript" type="text/javascript" src="JS/OtherViewer.js"></script>
    <script language="javascript" type="text/javascript" src="JS/fuHe.js"></script>
    <script language="javascript" type="text/javascript" src="JS/toMainViewer.js"></script>
    <script language="javascript" type="text/javascript" src="JS/Password.js"></script>

      <div id="forswitching" class="Pageswitching">
       <div id="switchpaneltop" class="switchpaneltop" >
       <div class="switchlogo"><div id="free" class="freeposition" style="cursor:pointer" runat="server"></div></div>
       <div class="all" >
        <div id="productPrev" class="divtwo">
        <p id="outPdPreview" class="outPdPreview" onclick="" onmouseover="this.className='PdPreview'" onmouseout="this.className='outPdPreview'"></p>
         </div>
         <div id="interactAna" class="divthree">
         <p id="InteractiveOUT" class="InteractiveOUT" onclick="" onmouseover="this.className='InteractiveIN'" onmouseout="this.className='InteractiveOUT'"></p>
          </div>
          <div id="makeAndPub" class="divfour" > 
          <p  id="productMake" class="MakeOUT"  onmouseover="this.className='MakeIN  '" onclick="" onmouseout="this.className=' MakeOUT '"></p>
          </div>
           <div id="dataSer"  class="divfive">
           <p id="dataService" class="Dataservice_Disabled" ></p>
           </div>
            </div>
           </div>
           </div>
     
    <!-- 海洋气象 --> 
    <script language="javascript" type="text/javascript" src="JS/TreeMenu.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageProduct.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ForecastPanel.js"></script>
    <script language="javascript" type="text/javascript" src="JS/FlexGrid.js"></script>
    <script language="javascript" type="text/javascript" src="JS/PatrolCheck.js"></script>
    
    <!-- 图片浏览 --> 
    <script type="text/javascript" src="JS/Chart/jquery.min.js"></script>
    <script type="text/javascript" src="JS/Chart/highstock.js"></script>
    <script type="text/javascript" src="JS/Chart/modules/exporting.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageFrame.js"></script>
    <script language="javascript" type="text/javascript" src="JS/tableQuxian.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ImageViewer.js"></script>
    
     <form id="form1" runat="server" style="padding: 0px; margin: 0px">
    <asp:HiddenField ID="loginParams" runat="server" />

    </form>
</body>
</html>
