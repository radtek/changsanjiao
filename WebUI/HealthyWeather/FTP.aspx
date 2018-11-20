<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FTP.aspx.cs" Inherits="HealthyWeather_FTP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <link href="../media/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap2.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-select.min.css" rel="stylesheet" type="text/css" />
    <link href="css/FTP.css?v=201706161" rel="stylesheet" type="text/css" />


    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="js/bootstrap-select.min.js"></script>
    <script src="js/bootstrap-filestyle.min.js" type="text/javascript"></script>
    <script src="js/ftp.js?v=201706162" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="region" class="list-group col-sm-2" style="text-align:center;margin-left:2px;"> </div>
    <div id="queryResult"  style="margin-left:16%;">
         <table class="display dataTable no-footer" id="ftpTab" role="grid" aria-describedby="example_info"
                border="0" cellspacing="0" cellpadding="0" style="text-align: center">
         </table>
    </div>
    <%--<div class="create"style="margin-right:5%;width:50px;float:right;">--%>
     <button class="btn btn-primary" type="button" id="createFTP"onclick="create(this.innerHTML)">创建FTP</button>
    <%-- </div>--%>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" >
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button  class="close" type="button" aria-label="Close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 class="modal-title" id="myModalTitle"></h3>
                </div>
                <div class="modal-body">
                     <form class="form-horizontal">
                        <div class="form-group col-sm-11">
                            <label for="address" class="col-sm-2 zj" style="text-align:center;">地址：<span style=" color:Red">*</span></label>
                            <input type="text" class="col-sm-4 jl" id="address" />
                            <label for="port" class="col-sm-3" style="text-align:right;">端口号：<span style=" color:Red">*</span></label>
                            <input type="text"onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" class="col-sm-1 jl"id="port" style="width:12%;" />
                        </div>

                        <div class="form-group col-sm-11 ">
                            <label for="accout" class="col-sm-2 zj" style="text-align:center;">账户：<span style=" color:Red">*</span></label>
                            <input type="text" class="col-sm-3 jl" id="accout" />
                            <label for="password" class="col-sm-4" style="text-align:right;margin-left:-0.3%;">密码：<span style=" color:Red">*</span></label>
                            <input type="password" class="col-sm-2 jl" id="password" />
                        </div>

                        <div class="form-group col-sm-11">
                            <!--王斌  2017.5.16-->
                            <label for="contents" class="col-sm-2 zj"id="Label1" style="text-align:center;">目录：<span style=" color:Red">*</span></label>
                            <input type="text" class="col-sm-4 jl" placeholder="/" id="contents" />
                            <label for="regionCre" class="col-sm-2 regionCre"id="regionT" style="text-align:center;">区域：<span style=" color:Red">*</span></label>
                            <select id="regionCre" class="col-sm-3 jl region" style="height:29px;"><option></option></select>
                        </div>

                           <div class="form-group col-sm-11">
                            <!--xuehui  2017.6.15-->
                            <label for="contents" class="col-sm-4 zjII"id="Label2" style="text-align:left;">接收单位：<span style=" color:Red">*</span></label>
                            <input type="text" class="col-sm-4 jlII"  id="reciver" />

                            <div class="form-group">
                                 <label for="pubRegion" class="col-sm-3 control-label">产品类型：<span style=" color:Red">*</span></label>
                                 <div class="col-sm-5" style=" margin-right:-70px; color:Black; float:right; margin-top:-35px"><select name="usertype" style=" color:Black" class="selectpicker show-tick form-control" multiple data-live-search="false" id="selPubRegion"></select></div>
                            </div>
                          </div>
                    </form>
                </div> 
                    
                <div class="modal-footer">
                    <label id="tip" style="float:left;margin:0px;" style="display:none">提示：你目前选择的区域是<span class="color" id="tipR"></span></label>
                    <label id="tipCon" style="float:left"></label>
                    <button class="btn btn-default" type="button" onclick="queren()">确认</button>
                    <button class="btn btn-default" type="button" data-dismiss="modal">关闭</button>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
