<%@ Page Language="C#" AutoEventWireup="true" CodeFile="orgManage.aspx.cs" Inherits="HealthyWeather_orgManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <link href="../media/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap3.css" rel="stylesheet" type="text/css" />
    <link href="css/orgManage.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="js/orgManage.js?v=22" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="width:90%;margin:0 auto;">
            <table class="display dataTable no-footer" id="orgTab" role="grid" aria-describedby="example_info"
                    border="0" cellspacing="0" cellpadding="0" style="text-align: center">
            </table>
        </div>
        <div id="edit">
            <div id="myModal" class="modal fade" tabindex="-1">
                <div class="modal-dialog" role="document"  style="width:450px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button  class="close" type="button" aria-label="Close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h3 class="modal-title" id="myModalTitle">编辑用户</h3>
                         </div>
                         <div class="modal-body">
                            <div class="form-group hidden">
                                <label for="idd" class="col-sm-2 control-label">区域编号:</label>
                                <div class="col-sm-8"><input type="text" id="idd" class="form-control" disabled/></div>
                            </div>

                            <div class="form-group">
                                <label for="region" class="col-sm-2 control-label">区域:</label>
                                <div class="col-sm-8"><input type="text" class="form-control"  id="region" disabled /></div>
                            </div>

                            <div class="form-group">
                                <label for="orgName" class="col-sm-2 control-label">机构名称:</label>
                                <div class="col-sm-8 "><input type="text"  class="form-control" id="orgName"/></div>
                            </div>
                         </div>
                         <div class="modal-footer">
                            <button class="btn btn-default" type="button" onclick="queren()">确认</button>
                            <button class="btn btn-default" type="button" data-dismiss="modal">关闭</button>
                         </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>