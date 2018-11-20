<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarningGroup.aspx.cs" Inherits="WarningGroup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<%--    <script language="javascript" type="text/javascript">
        var lastTab = "<%=m_FirstTab %>";
    </script>--%>
    <link rel="stylesheet" type="text/css" href="../media/css/jquery.dataTables.css" />
    <link rel="stylesheet" href="../themes/base/jquery.ui.all.css" />
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css" />
    <link href="css/WarningGroup.css" rel="stylesheet" />

    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/jquery-1.9.1.js"></script>
    <script type="text/javascript" language="javascript" src="../media/js/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../media/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="../ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../ui/jquery.ui.accordion.js"></script>
    <script language="javascript" type="text/javascript" src="js/WarningGroup.js?v=0821"></script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <link rel="stylesheet" href="images/jquery-ui.css" />
    <script src="js/jquery-ui.js"></script>

</head>
<body style="-webkit-overflow-scrolling: touch; overflow: auto; font-size: 14px;">
    <div class="wrapborder">
        <div class="border1">
            <div class="wrap">
                <div class="outer_content">
                    <div class="inner_content">
                        <div id="tabbtn" style="display: none;">
                            <ul id="tabItem" runat="server">
                            </ul>
                        </div>
                        <div id="content" style="width: 100%; border: 0px solid #0000ff">
                            <div id="Tb0" style="width: 100%;">
                                <div style="float: left; margin-top: 10px; margin-bottom: 10px;">
                                    <div style="float: left;" id="workGroup">
                                    </div>

                                </div>
                                <div style="float: left; margin-bottom: 10px; margin-top: 10px; margin-left: 35px">
                                    <div style="float: left;">
                                        预警等级：
                                    </div>
                                    <select name="TypeList" onchange="queryData();" id="TypeList" class="grouplist"
                                        style="width: 150px;">
                                        <option selected="selected" value="全部">全部</option>
                                        <option value="蓝色">蓝色</option>
                                        <option value="黄色">黄色</option>
                                        <option value="橙色">橙色</option>
                                    </select>
                                </div>
                                <div style="float: left; margin-bottom: 10px; margin-top: 10px; margin-left: 35px">
                                    <div style="float: left;">
                                        发布状态：
                                    </div>
                                    <select onchange="queryData();" id="status"
                                        style="width: 150px;">
                                        <option selected="selected" value="全部">全部</option>
                                        <option value="成功">成功</option>
                                        <option value="失败">失败</option>
                                    </select>
                                </div>
                                <br />
                                <div>
                                    <button hidden type="button" style="margin-top: -10px; margin-right: 10px; margin-bottom: 13px; font-size: 14px; float: right;"
                                        class="normal-btn input-btnQuery" id="Button2"
                                        onclick="ftpsendModule()" onmouseover="this.className='normal-btn-h input-btnQuery'"
                                        onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'"
                                        onmouseup="this.className='normal-btn input-btnQuery'">
                                        <span class="select-now"></span><span class="select-text">发布FTP</span>
                                    </button>

                                    <button type="button" style="margin-top: -10px; margin-right: 10px; margin-bottom: 13px; font-size: 14px; float: right;"
                                        class="normal-btn input-btnQuery" id="Button1"
                                         onclick="showAddDialogs();" onmouseover="this.className='normal-btn-h input-btnQuery'"
                                        onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'"
                                        onmouseup="this.className='normal-btn input-btnQuery'">
                                        <span class="select-add"></span><span class="select-text">新增预警</span>

                                    </button>
                                    <button type="button" style="margin-top: -10px; margin-right: 10px; margin-bottom: 13px; font-size: 14px; float: right;"
                                        class="normal-btn input-btnQuery" id="Button1"
                                         onclick="queryData()" onmouseover="this.className='normal-btn-h input-btnQuery'"
                                        onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'"
                                        onmouseup="this.className='normal-btn input-btnQuery'">
                                        <span class="select-add search"></span><span class="select-text">查询</span>

                                    </button>
                                </div>
                                <table class="display dataTable no-footer" id="example_us" role="grid" aria-describedby="example_info"
                                    border="0" cellspacing="0" cellpadding="0" style="text-align: center">
                                </table>
                            </div>
                            <div id="Tb2" class="hidden">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="dialog-confirm" title="提示">
        <p>
            <span class="ui-icon-alert" style="float: left; margin: 0 7px 20px 0; overflow: hidden;"></span>
            <label id="toolstr">
            </label>
        </p>
    </div>
    <div id="dialog-Ftp" title="提示">
        <p>
            <span class="ui-icon-alert" style="float: left; margin: 0 7px 20px 0; overflow: hidden;"></span>
            <label id="toolstr1">
            </label>
        </p>
    </div>
     <%--<div id="dialog-form-preView1" title="文件" style="display: none; height: 100%; font-size: 18px; line-height: 40px;">
                <div id="preView">
                </div>
    </div>--%>
    <div id="dialog-preView" title="预览内容">
        <div id="preView">
        </div>
    </div>
    <div id="dialog-form-add" title="预警新增" style="display: none; height: 100%; font-size: 18px; line-height: 40px;">
        <form>
            <fieldset style="border-width: 0px">
                <div id="wrnTypeAdd">
                </div>
                <div style="margin-left: 20px; margin-top: 20px;" id="contentS">
                </div>
                <input type="submit" tabindex="-1" style="position: absolute; top: -1000px">
            </fieldset>
        </form>
    </div>
       
</body>
</html>
