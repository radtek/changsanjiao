<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServiceManage.aspx.cs" Inherits="HealthyWeather_ServiceManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
    <link href="../media/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="css/ServiceManage.css?v=2017-609" rel="stylesheet" type="text/css"/>

    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/bootstrap-select.min.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.js"></script>
    <script src="../JS/Utility.js"></script>
    <script src="js/ServiceManage.js?v=091101" type="text/javascript"></script>
    
</head>
<body>
    <form id="form1" runat="server">
        <div class="table">
            <table class="display dataTable no-footer" id="serviceTab" role="grid" aria-describedby="example_info"
                border="0" cellspacing="0" cellpadding="0" style="text-align: center">
         </table>
        </div>
        <div>
            <button class="btn btn-primary create" type="button" id="create"onclick="create_Service(this.innerHTML)"><strong>+ </strong>创建</button>
        </div>
        <div class="modal fade" id="mymodal"tabindex="-1" role="dialog"aria-labelledby="myModalLabel" aria-hidden="true" >
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class=" modal-title" id="title">创建服务接口</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group l-p">
                            <label for="address" class="col-sm-2 control-label r">地址：</label>
                            <input type="text" class="form-control w" id="address" />
                        </div>

                        <div class="form-group l-p">
                            <label for="region" class="col-sm-2 control-label r">区域：</label>
                            <select class="selectpicker show-tick form-control" multiple data-live-search="false" name="region" id="region">
                            </select>
                        </div>
                        
                        <div class="form-group l-p">
                            <label for="key" class="col-sm-2 control-label r" style="padding-right:9px;">密钥：<span style="color:red">*</span></label>
                            <input type="text" class="form-control w" id="key"/>
                        </div>

                        <div class="form-group l-p">
                            <label for="receiver" class="col-sm-3 control-label r" style="margin-left:-44px;">接收单位：</label>
                            <input type="text" class="form-control w" id="receiver"/>
                        </div>

                        <div class="form-group l-p">
                            <label for="product" class="col-sm-2 control-label r">产品：</label>
                            <div  id="product">
                                <label class="checkbox-inline"><input type="checkbox" name="type" value="2" />儿童感冒</label>
                                <label class="checkbox-inline"><input type="checkbox" name="type" value="3" />青年感冒</label>
                                <label class="checkbox-inline"><input type="checkbox" name="type" value="4" />老年感冒</label>
                                <label class="checkbox-inline"><input type="checkbox" name="type" value="5" />COPD感冒</label>
                                <label class="checkbox-inline"><input type="checkbox" name="type" value="6" />儿童哮喘</label>
                                <label class="checkbox-inline"><input type="checkbox" name="type" value="7" />中暑</label>
                                <label class="checkbox-inline"><input type="checkbox" name="type" value="8" />重污染</label>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="btn-default btn"type="button" data-dismiss="modal">关闭</button>
                        <button class="btn-primary btn" type="button" onclick="confirm_create()">确认</button>
                        
                    </div>
                </div>
            </div>
        </div>
        
    </form>
</body>
</html>
