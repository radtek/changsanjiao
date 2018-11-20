<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MessageManage.aspx.cs" Inherits="LiveIndex_MessageManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="css/bootstrap-o.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="css/MessageManage.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-1.12.4.js" type="text/javascript"></script>
    <script src="js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/Utility.js" type="text/javascript"></script>
    <script src="js/MessageManage.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
    <div class="head">
        <input type="button" class=" btn-default btn addBtn add" onclick="editUser('添加')" value="添加用户" />
        <input type="button" class="btn-default btn addBtn export" onclick="exportToExcel()" value="导出" />
    </div>

            <div class="tab">
                <table class="display dataTable no-footer" id="table" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style="text-align: center">
                </table>
            </div>
                <!-- 模态框（Modal） -->
        <!-- 模态框（Modal） -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                     <div class="move" id="move"></div>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="myModalTitle">添加用户</h4>
                    </div>
                    <div class="modal-body">
                            <div class="form-group">
                                <label for="userName" class="col-sm-2 control-label" style="padding-right:0;">姓名：</label>
                                <div class="col-sm-9">
                                    <input type="text" class="form-control" id="userName" placeholder="请输入姓名" />
                                </div>
                           </div>
                           <div class="form-group">
                                <label for="phone" class="col-sm-2 control-label" style="padding-right:0;">手机：</label>
                                <div class="col-sm-9">
                                    <input type="text" class="form-control" id="phone" placeholder="请输入手机号" />
                                </div>
                           </div>
                           <div class="form-group">
                                <label for="age" class="col-sm-2 control-label" style="padding-right:0;">年龄：</label>
                                <div class="col-sm-9">
                                    <input type="text" class="form-control" id="age" placeholder="请输入年龄" />
                                </div>
                           </div>
                           <div class="form-group">
                                <label for="gender" class="col-sm-2 control-label" style="padding-right:0;">性别：</label>
                                <div class="col-sm-9">
                                    <select class="form-control" id="gender">
                                        <option value="2">男</option>
                                        <option value="1">女</option>
                                    </select>
                                </div>
                           </div>
                           <div class="form-group">
                                <label for="education" class="col-sm-2 control-label" style="padding-right:0;">学历：</label>
                                <div class="col-sm-9">
                                    <input type="text" class="form-control" id="education" placeholder="请输入学历" />
                                </div>
                           </div>
                           <div class="form-group">
                                <label for="company" class="col-sm-2 control-label" style="padding-right:0;">单位：</label>
                                <div class="col-sm-9">
                                    <input type="text" class="form-control" id="company" placeholder="请输入单位" />
                                </div>
                           </div>
                           <div class="form-group">
                                <label for="occupation" class="col-sm-2 control-label" style="padding-right:0;">职称：</label>
                                <div class="col-sm-9">
                                    <input type="text" class="form-control" id="occupation" placeholder="请输入职称" />
                                </div>
                           </div>
                           <div class="form-group">
                                <label for="postCode" class="col-sm-2 control-label" style="padding-right:0;">邮编：</label>
                                <div class="col-sm-9">
                                    <input type="text" class="form-control" id="postCode" placeholder="请输入邮编" />
                                </div>
                           </div>
                           <div class="form-group">
                                <label for="address" class="col-sm-2 control-label" style="padding-right:0;">地址：</label>
                                <div class="col-sm-9">
                                    <input type="text" class="form-control" id="address" placeholder="请输入地址" />
                                </div>
                           </div>
                           <div class="form-group">
                                <label for="phoneStatus" class="col-sm-2 control-label" style="padding-right:0;">手机号状态：</label>
                                <div class="col-sm-9">
                                    <select class="form-control" id="phoneStatus">
                                        <option value="1">可用</option>
                                        <option value="0">不可用</option>
                                    </select>
                                </div>
                           </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <button type="button" class="btn btn-primary" id="J_submit">提交</button>
                    </div>
                </div><!-- /.modal-content -->
            </div><!-- /.modal -->
        </div>

        <asp:Button ID="btnExport" runat="server" onclick="BtnExport" Text="Button" CssClass="inVisibility" />
    </form>
</body>
</html>
