<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IndexCorr.aspx.cs" Inherits="LiveIndex_IndexCorr" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/jquery.page.css" rel="stylesheet" />
    <link href="css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="css/bootstrap1.css" rel="stylesheet" />
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <link href="css/IndexCorr.css" rel="stylesheet" />
      

    <script src="js/jquery-1.10.2.js"></script>
    <script src="js/jquery.page.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/bootstrap-select.min.js"></script>
   <%-- <script src="js/jquery.dataTables.js"></script>--%>
    <script src="js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="js/Utility.js"></script>
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-lang-zh_CN.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="js/WinPosition.js" type="text/javascript"></script>
    <script src="js/IndexCorr.js"></script>
</head>
<body>
    <form id="form1" runat="server">
     <div id="_page" style="width:100%;background-color:white;height: 57px;position:fixed;z-index:9999;top:0;">
        <div class="top">
            <div style="float:left;margin-top:10px;">
                <label class="fl">站点：</label>
                <select id="site" class="fl" style="margin-top: -4px;"></select>
            </div>
            <div class="fl mt top-time ">
                <span class="fl">预报时次：</span>
                <div class="ri fl" style="margin-top:2px;">
                    <label class="radio fl"><input type="radio"value="今天" />今天</label>
                    <label class="radio fl"><input type="radio"value="明天"/>明天</label>
                    <label class="radio fl"><input type="radio"value="后天" />后天</label>
                </div>
            </div>
            <div class="fl mt">
                <span>预报日期：</span>
                <span id="fore-time"></span>
            </div>
            <div>
                <input type="button" class="btn-default btn fl mt hidden" style="margin-top:5px" onclick="saveAll()" value="保存" />
            </div>
            <div>
                <input type="button" class="btn-default btn fl mt" onclick="messPreview()" style="margin-top:5px" value="生成短信" />
            </div>
            <div>
                <input type="button" class="btn-default btn fl mt" onclick="centerPre()" style="margin-top:5px" value="中心预报" />
            </div>
            <div>
                <input type="button" class="btn-default btn fl mt" style="margin-top:5px" onclick="exportToExcel()" value="导出到Excel" />
            </div>
        </div>
    </div>
       <%-- <input value="点击" id="btn" />--%>
    <div class="left-content" id="left-content">
        <div class="tab-header">
            <table style="width:100%">
                <tr class="tab-header-tr">
                     <td class="mid">指数名称</td>
                     <td class="short">指数值</td>
                     <td class="short">指数级别</td>
                     <td class="mid">含义</td>
                     <td class="long">简短提示</td>
                     <td class="long">详细提示</td>
                </tr>
            </table>
        </div>
        <div class="publiced">
            <div class="tr open" id="pub" onclick="details(id,'publiced')">已发布</div>
               <table class="display dataTable no-footer test" id="publiced" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style="text-align: center">
               </table>
            <div id="J_pub_total" class="y-total">123</div>
        </div>
        <div class="publiced" style="padding-bottom:45px;">
            <div class="tr open" id="unpub"onclick="details(id,'unpublic')">未发布</div>
               <table class="display dataTable no-footer" id="unpublic" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style="text-align: center">
               </table>
            <div id="J_unpub_total" class="y-total"></div>
        </div>
        <div id="page_navigation" class="page_navigation"></div>
        <div class="y-detail" id="J_detail">
            <div class="firstrow">
                <div class="form-group">
                        <label class="col-sm-1 control-label">级别:</label>
                        <div class="col-sm-6">
                            <div class="input-group">
                                <input type="text" class="form-control" id="J_lvalueDropDown"/>
                                <div class="input-group-btn">
                                    <button type="button"id="dropDownMenu" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
							            级别
							            <span class="caret"></span>
						            </button>
                                </div>
                            </div>
                        </div>
                    </div>
                <div class="form-group">
                    <label class="col-sm-1 control-label">原因：</label>
                    <div class="col-sm-2">
                        <select class="selectpicker show-tick form-control" multiple data-live-search="false" id="J_reason">
                           
                        </select>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-1 control-label">指数值：</label>
                    <div class="col-sm-1">
                        <input class="form-control"id="J_IndexValue"/>
                    </div>
                </div>
            </div>
            <div class="secondrow">
                <label  class="col-sm-1 control-label d-s-margintop">提示语:</label>
                <textarea class="form-control fl d-s-textarea d-s-margintop" id="J_tip" rows="3"></textarea>
            </div>
            <div>
                
                <button type="button" class="btn btn-default d-s-margintop"style="float:right;margin-right:2.5%;" id="J_cancel">取消</button>
                <button type="button" class="btn btn-primary d-s-margintop" style="float:right;margin-right:2%;" id="J_save">保存</button>
            </div>
        </div>
       <%-- 下拉菜单出来的表格--%>
    <div class="drop-down" id="J_drop-down">
        <table class="drop-table" style="width:100%;margin-bottom: 20px;display:block;">
            <tr>
                <td class="drop-table-td">含义</td>
                <td class="drop-table-td">简短提示</td>
                <td class="drop-table-td">指数值范围</td>
                <td class="drop-table-td"></td>
            </tr>
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
                    <td>要素值</td>
                </tr>
            </table>
        </div>
    </div>

     <asp:HiddenField id="siteid" runat="server" />
     <asp:HiddenField id="lst" runat="server" />
    <asp:Button ID="btnExport" runat="server" onclick="Button1_Click" Text="Button" CssClass="inVisibility" />
    </form>
</body>
</html>
<script>
    getSite();
</script>