<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IndexUpload.aspx.cs" Inherits="LiveIndex_IndexUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/bootstrap-o.css" rel="stylesheet" />
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <link href="css/IndexUpload.css" rel="stylesheet" />

    <script src="js/jquery-1.12.4.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/Utility.js"></script>
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../Ext/ext-lang-zh_CN.js" type="text/javascript"></script>
    <script src="js/IndexUpload.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="todayFore">
        <div class="title"><span>今日预报</span></div>
        <div class="foreContent form-horizontal">
            <div class="form-group col-sm-7">
	            <label class="control-label" style="width:18%;float:left;">选择上传服务器：</label>
	            <div class="col-sm-9" style="padding-left:0;">
			        <select class="form-control" id="sName">
                   <%-- 
                        <option>上传62服务器(需11点后), 范例：scuem_zsyb_yyyymmdd1600.txt</option>
                        <option>加分人群感冒并上传(需11点后), 范例：index2_yyyymmdd1624.txt</option>
                        <option>加人体舒适度并上传, 范例：index_yyyymmdd0724.txt</option>
                        <option>上传微信服务器(需11点后), 范例：index2_yyyymmdd1624.txt</option>
                        <option>上传火险指数, 范例：HX_yyyymmdd_05.txt</option>
                        --%>
			        </select>
		        </div>
	        </div>
            <div class="col-sm-2">
                <input type="button" class="btn-default btn" id="exportIndex" value="1、导出气象指数"/>
            </div>
            <div class="col-sm-2">
                <input type="button" class="btn-default btn" id="upload" disabled="true" value="2、确定并上传到服务器"/>
            </div>
        </div>
    </div>

    <div class="sevenFore">
        <div class="title"><span>七日预报</span></div>
        <%--<textarea id="txt" readonly="readonly"></textarea>--%>
    </div>
    <div class="tipSuccess">
        <div class="frame">
            <div class="title2">成功！</div>
            <div class="outer">
                <div class="inner">
                    <div class="div-txt">
                        <span>已成功获取气象指数数据</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
