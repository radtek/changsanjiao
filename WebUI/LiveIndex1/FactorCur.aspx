<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FactorCur.aspx.cs" Inherits="LiveIndex_FactorCur" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/bootstrap-o.css" rel="stylesheet" />
    <link href="css/IndexCorr.css" rel="stylesheet" />
    <link href="css/FactorCur.css" rel="stylesheet" />
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-1.12.4.js"></script>
    <script src="js/template-native-debug.js"></script>
    <script src="js/Utility.js" type="text/javascript"></script>
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../Ext/ext-lang-zh_CN.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/FactorCur.js"></script>
    <script src="js/IndexCorr.js"></script>
   

</head>
<body>
    <form id="form1" runat="server">
    <div class="top">
        <div class="_top" >
            <div class="form-group">
                <label for="site"class="col-sm-1 control-label">站点：</label>
                <div class="col-sm-2">
                    <select id="site" class="form-control"></select>
                </div>
            </div>
            <div class="form-group">
                <label for="site"class="col-sm-1 control-label">预报时次：</label>
                <div class="col-sm-2">
                    <label class="radio-inline control-label" style="padding-left:5px;">
                        <input type="radio" value="今天" />今天
                    </label>
                    <label class="radio-inline control-label">
                        <input type="radio"  value="明天"/>明天
                    </label>
                    <label class="radio-inline control-label">
                        <input type="radio"  value="后天"/>后天
                    </label>
                </div>
            </div>
            <div>
                <label class="control-label">预报日期：</label>
                <span id="lst"></span>
            </div>
        </div>
    </div>
    <div class="left-content" id="left-content">
        <div id="content" class="content">
            <table id="table">
                    <%--<tr>
                        <td>要素编码</td>
                        <td>要素名称</td>
                        <td>要素值</td>
                        <td>要素描述</td>
                    </tr>--%>
            </table>
        </div>
    </div>
    <div class="right-bar">
        <div class="box">
            <span class="_box" id="_box">要素信息</span>
            <div class="box-display" id="box_display">
                <div class="_display">
                    <span class="display-span">显示</span>
                </div>
                <div class="triangle"></div>
            </div>
            
        </div>
    </div>
    <div class="right-content" id="right-content">
        <div class="feature-tab" id="feature-tab">
            <table style="width:100%;float:right;">
                <tr>
                    <td>编码</td>
                    <td>要素名称</td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
