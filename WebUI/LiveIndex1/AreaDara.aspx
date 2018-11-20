<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AreaDara.aspx.cs" Inherits="HealthyWeather_orgManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
    <link href="../media/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <%--<link href="css/bootstrap3.css" rel="stylesheet" type="text/css" />--%>
    <link href="css/bootstrap-o.css" rel="stylesheet" />
    <link href="css/AreaDara.css" rel="stylesheet" />

    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script src="js/jquery-1.12.4.js"  type="text/javascript"></script>
    <script src="js/bootstrap.min.js"  type="text/javascript"></script>
    <script src="js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="js/AreaDara.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
   
        <div class="top">
            <input type="button" id="J_insert" class="btn-default btn" style="float:right;margin-right:8%;margin-bottom:10px;" value="添加" />
            <input type="button" id="J_del" class="btn-default btn" style="float:right;margin-right:40px;margin-bottom:10px;" value="删除" />
        </div>
        <div style="width:90%;margin:0 auto;">
            <table class="display dataTable no-footer" id="orgTab" role="grid" aria-describedby="example_info"
                    border="0" cellspacing="0" cellpadding="0" style="text-align: center">
            </table>
        </div>
        <div id="edit">
            <div id="myModal" class="modal fade form-horizontal" tabindex="-1">
                <div class="modal-dialog" id="modal-dialog" role="document"  style="width:450px;">
                    <div class="modal-content">
                        <div class="modal-header">
                             <div class="move" id="move"></div>
                            <button  class="close" type="button" aria-label="Close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h3 class="modal-title" id="myModalTitle">添加/修改区域</h3>
                         </div>
                         <div class="modal-body">
                            <div class="form-group">
                                <label for="idd" class="col-sm-3 control-label">区域编码:</label>
                                <div class="col-sm-8"><input type="text" id="idd" disabled class="form-control"/></div>
                            </div>

                            <div class="form-group">
                                <label for="region" class="col-sm-3 control-label">区域说明:</label>
                                <div class="col-sm-8"><input type="text" class="form-control"  id="region" /></div>
                            </div>
                            
                            <div class="form-group">
                                <label for="orgName" class="col-sm-3 control-label">主要区域:</label>
                                <div class="col-sm-8 ">
                                    <%--<input type="text"  class="form-control" id="orgName"/>--%>
                                    <select class="col-sm-3 form-control" id="orgName">
                                        <option value="是">是</option>
                                        <option value="非">非</option>
                                    </select>
                                </div>
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