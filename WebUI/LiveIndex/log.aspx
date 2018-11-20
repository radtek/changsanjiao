<%@ Page Language="C#" AutoEventWireup="true" CodeFile="log.aspx.cs" Inherits="LiveIndex_log" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/bootstrap-o.css" rel="stylesheet" />
    <link href="css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="css/log.css?v=011601" rel="stylesheet" />

    <script src="js/jquery-1.12.4.js"></script>
    <script src="js/jquery.dataTables.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="../DatePicker/WdatePicker.js"></script>
     <script src="js/Utility.js"></script>
 <%--   <script src="js/WdatePicker.js"></script>--%>
    <script src="js/log.js?v=011601"></script>
</head>
<body>
    <form id="form1" runat="server" class="form-horizontal">
        <div class="page">
            <div class="header">
                <div class="form-group" style="margin-left: 0; margin-right: 0;">
                    <div class="f" style="margin-left:2%;">
                        <label class="f">开始时间：</label>
                        <div class="f">
                            <input type="text" class="form-control" id="startTime" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' });" />
                        </div>
                        <label class="f" style="margin-left:30px;">结束时间：</label>
                        <div class="f">
                            <input type="text" class="form-control" id="endTime" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' });" />
                        </div>
                    </div>
                    <div class="f ml" >
                         <label class="f">人员：</label>
                        <div class="f">
                            <select id="people" class="form-control">
                                <option value="首席检查">首席检查</option>
                          
                            </select>
                        </div>
                    </div>
                    <div class="f ml" >
                        <label class="f">模块：</label>
                        <div class="f">
                            <select id="fun" class="form-control">
                                <option value="全部">全部</option>
                                <option value="首席检查">首席检查</option>
                                   <option value="指数发布">指数发布</option>
                                <option value="指数订正">指数订正</option>
                            </select>
                        </div>
                    </div>
                    <div class="f ml" >
                        <label class="f">状态：</label>
                        <div class="f">
                            <select id="status" class="form-control">
                                <option value="全部">全部</option>
                                <option value="成功">成功</option>
                                <option value="失败">失败</option>
                            </select>
                        </div>
                    </div>
                    <div class="f ml">
                        <input type="button" class="f form-control btn-default btn"  value="查询" id="j_query" onclick="query()" />
                    </div>
                </div>
            </div>
             <!-- 模态框（Modal） -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog" style="width:1200px;">
                <div class="modal-content">
                    <div class="modal-header">
                     <div class="move" id="move"></div>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="myModalTitle">发送内容：</h4>
                    </div>
                    <div class="modal-body" style="">
                        <div class="tab">
                            <ul class="nav nav-tabs">
	                        <%--    <li class="active item1"><a href="#">Home</a></li>
	                            <li class="item2"><a href="#">SVN</a></li>--%>
                            </ul>
                        </div>
                        <textarea id="txt" style="width:1168px;height:550px;resize:none;background-color:white;font-size:13px;"disabled="disabled"></textarea>
                    </div>
                </div><!-- /.modal-content -->
            </div><!-- /.modal -->
        </div>
            <div class="content" style="width:98%;margin:0 auto;">
                <table class="display dataTable no-footer test" id="table" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style="text-align: center">
                </table>
            </div>
        </div>
    </form>
</body>
</html>
