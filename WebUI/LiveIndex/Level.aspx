<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Level.aspx.cs" Inherits="LiveIndex_Level" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/bootstrap-o.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="css/Level.css?v=1219" rel="stylesheet" />

    <script src="js/jquery-1.12.4.js"></script>
    <script src="js/jquery.dataTables.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../Ext/ext-lang-zh_CN.js" type="text/javascript"></script>
    <script src="js/Utility.js?v=1219"></script>

    <script src="js/Level.js?v=1219"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="tab-header" style="top:0;background-color:White;height:20px;"></div>
        <div class="tab-header" style="background: linear-gradient(rgb(57, 120, 163),rgba(20, 130, 206, 0.88),rgb(57, 120, 163));">
            <table>
                <tr class="tab-header-tr">
                     <td>主级别</td>
                     <td>级别</td>
                     <td>子级别</td>
                     <td class="special">等级含义</td>
                     <td class="special">等级参数</td>
                     <td class="special">等级说明</td>
                     <td>指数范围</td>
                     <td class="special">指数范围说明</td>
                     <td class="special">提示语</td>
                </tr>
            </table>
        </div>
        <div class="tab">
               <table class="display dataTable no-footer" id="table" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style="text-align: center">
               </table>
        </div>
    </div>
        <!-- 模态框（Modal） -->
    <div class="modal fade form-horizontal" id="myModal"  tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                 <div class="move" id="move"></div>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel" >添加/修改指数级别</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="zs" class="col-sm-3 control-label">指数:</label>
                        <div class="col-sm-7">
                            <input type="text" class=" form-control"readonly ="true" id="zs"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="jb" class="col-sm-3 control-label">级别:</label>
                        <div class="col-sm-7">
                            <select class=" form-control" id="jb"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="mainjb" class="col-sm-3 control-label">主级别:</label>
                        <div class="col-sm-7">
                            <select class="form-control" id="mainjb"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="subjb" class="col-sm-3 control-label">子级别:</label>
                        <div class="col-sm-7">
                            <select class="form-control" id="subjb"></select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="grademean" class="col-sm-3 control-label">等级含义:</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="grademean"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="gradeparam" class="col-sm-3 control-label">等级参数:</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="gradeparam"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="textlevelname" class="col-sm-3 control-label">等级说明:</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="textlevelname"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="scopelower" class="col-sm-3 control-label">指数范围下限:</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="scopelower"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="scopeexplain" class="col-sm-3 control-label">指数范围说明:</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="scopeexplain"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="tip" class="col-sm-3 control-label">提示语:</label>
                        <%--<input type="text" class="w ml" id="tip"/>--%>
                        <div class="col-sm-7">
                            <textarea class="form-control" id="tip" style="resize:none;height:100px;"></textarea>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" id="tijiao">提交</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                </div>
            </div><!-- /.modal-content -->
        </div><!-- /.modal -->
    </div>
    </form>
</body>
</html>
