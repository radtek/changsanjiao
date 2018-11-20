<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkGroup.aspx.cs" Inherits="WorkGroup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>防御指引管理</title>
    <script language="javascript" type="text/javascript">
        var lastTab = "<%=m_FirstTab %>";
    </script>
    <link href="images/WorkGroup.css?v=2211" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../media/css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="../TableTools/css/dataTables.tableTools.css" />
    <link rel="stylesheet" type="text/css" href="../resources/syntax/shCore.css" />
    <link rel="stylesheet" href="../themes/base/jquery.ui.all.css" />
    <link rel="stylesheet" href="../css/demos.css" />
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css" />
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="js/AQIUtility.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/jquery-1.9.1.js"></script>
    <script type="text/javascript" language="javascript" src="../media/js/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../media/js/jquery.dataTables.js"></script>
    <script type="text/javascript" language="javascript" src="../resources/syntax/shCore.js"></script>
    <script type="text/javascript" language="javascript" src="../resources/demo.js"></script>
    <script type="text/javascript" language="javascript" src="../TableTools/js/dataTables.tableTools.js"></script>
    <script type="text/javascript" src="../ui/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../ui/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../ui/jquery.ui.accordion.js"></script>
    <script language="javascript" type="text/javascript" src="js/WorkGroup.js?v=0821"></script>
    <script language="javascript" type="text/javascript" src="js/DataShare.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
 <%--         <link href="https://cdn.datatables.net/1.10.12/css/dataTables.bootstrap.min.css" rel="stylesheet" type="text/css" />   
            <script language="javascript" type="text/javascript" src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
                 <script language="javascript" type="text/javascript" src="https://cdn.datatables.net/1.10.12/js/dataTables.jqueryui.min.js"></script>--%>

    <script language="javascript" type="text/javascript">

 
        Ext.onReady(function () {
            //设置界面高度
            var pageHeight = document.documentElement.clientHeight;
            $(".wrapborder").height(pageHeight);
        });
        </script>
    <link rel="stylesheet" href="images/jquery-ui.css" />
    <script src="js/jquery-ui.js"></script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#demo').html('<table cellpadding="0" cellspacing="0" border="0" class="display" id="example"></table>');
        });
        $(function () {
            $("#radio").buttonset();
        });
    </script>
    <style type="text/css">
        label, input
        {
            display: block;
        }
        input.text
        {
            margin-bottom: 12px;
            width: 95%;
            padding: .4em;
        }
        fieldset
        {
            padding: 0;
            border: 0;
            margin-top: 25px;
        }
        h1
        {
            font-size: 1.2em;
            margin: .6em 0;
        }
        div#users-contain
        {
            width: 350px;
            margin: 20px 0;
        }
        div#users-contain table
        {
            margin: 1em 0;
            border-collapse: collapse;
            width: 100%;
        }
        div#users-contain table td, div#users-contain table th
        {
            border: 1px solid #eee;
            padding: .6em 10px;
            text-align: left;
        }
        .ui-dialog .ui-state-error
        {
            padding: .3em;
        }
    </style>
</head>
<body style="-webkit-overflow-scrolling: touch; overflow: auto; ">

    
    <div class="page_top">
        <div class="titleName">
            工作组人员分配</div>
    </div>
    <div class="wrapborder">
    <div class="border1">
    <div class="wrap">
    <div class="outer_content" >
    <div class="inner_content">
        <div id="tabbtn" style=" display:none;" >
            <ul id="tabItem" runat="server">
            </ul>
        </div>
        
        <div id="content" style="width: 100%; border:0px solid #0000ff">
  
            <div id="Tb0" style="width: 100%;">
                <div class="page_top">
                    <div class="titleName">
                        工作组人员分配</div>
                </div>
                <div style="float: left; margin-top: 10px; margin-bottom: 10px;">
                    <div style="float: left;" id="workGroup">
                    </div>
                    <%--<select name="Grouplist" onchange="" id="Grouplist" class="grouplist" style="width:150px;">
	                <option selected="selected" value="20">预报组</option>
	                <option value="21">日报组</option>
	                <option value="22">预报副组</option>
	                <option value="23">数据审核</option>
                   </select>--%>
                </div>
                <div style="float: left; margin-bottom: 10px; margin-top: 10px; margin-left: 100px">
                    <div style="float: left;">
                        判断要素：</div>
                    <select name="TypeList" onchange="queryData1();" id="TypeList" class="grouplist"
                        style="width: 150px;">
                                  <option selected="selected" value="-1">全部</option>
                                  <option  value="1">降温</option>
                                  <option value="2">温差</option>
                                  <option value="3">风力</option>
                                  <option value="4">湿度</option>
                                  <option value="5">温度</option>
                                  <option value="6">高温</option>
                                   <option value="7">中暑指数</option>
                                  <option value="8">PM2.5</option>
                                  <option value="9">PM10</option>
                                  <option value="10">O3</option>
                                  <option value="11">NO2</option>
                                  <option value="12">SO2</option>
                    </select>
                </div>
                <br />
                <div>
                    <p style="margin-top: 40px; text-align: left; margin-bottom:20px; font-size:15px; color:Blue">
                        >>防御信息列表</p>
                    <button type="button" style="margin-top: -82px; margin-right: 320px; margin-bottom: 13px;
                        font-size: 14px; float: right;" class="normal-btn input-btnQuery" id="Button1"
                        onclick="openUserInfo();" onmouseover="this.className='normal-btn-h input-btnQuery'"
                        onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'"
                        onmouseup="this.className='normal-btn input-btnQuery'">
                        <span class="select-add"></span><span class="select-text">添加指引</span>
                          
                    </button>
                     <button type="button" style="margin-top: -82px; margin-right: 220px; margin-bottom: 13px;
                        font-size: 14px; float: right;" class="normal-btn input-btnQuery" id="Button2"
                        onclick="cal()" onmouseover="this.className='normal-btn-h input-btnQuery'"
                        onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'"
                        onmouseup="this.className='normal-btn input-btnQuery'">
                        <span class="select-now"></span><span class="select-text">重新计算</span>
                    </button>
                     <button type="button" style="margin-top: -82px; margin-right: 120px; margin-bottom: 13px;
                        font-size: 14px; float: right;" class="normal-btn input-btnQuery" id="Button3"
                        onclick="show('','')" onmouseover="this.className='normal-btn-h input-btnQuery'"
                        onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'"
                        onmouseup="this.className='normal-btn input-btnQuery'">
                        <span class="select-now"></span><span class="select-text">导入指引</span>
                    </button>
                    <button type="button" style="margin-top: -82px; margin-right: 20px; margin-bottom: 13px;
                        font-size: 14px; float: right; " class="normal-btn input-btnQuery" id="Button4"
                        onclick="dataExport()" onmouseover="this.className='normal-btn-h input-btnQuery'"
                        onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'"
                        onmouseup="this.className='normal-btn input-btnQuery'">
                        <span class="select-now"></span><span class="select-text">导出指引</span>
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
    </div></div>
    </div>
    </div>


   <iframe id="uploadFrm" frameborder="no" border="0" scrolling="no" allowtransparency="yes" onload="iframeOnload()" name="uploadFrm" style="width:0px; height:0px; display:none;"></iframe>
    <div id="dialog-formS" title="文件上传" style=" display:none;  height:100%;">
                        <form name="actionForm" id="actionForm" action="WebExplorerss.ashx" method="post" target="uploadFrm"  enctype="multipart/form-data" runat="server" >
                                <asp:Button ID="ButtonExports" runat="server" Text="Button" CssClass="inVisibility" />
                            <fieldset  style=" border:0 none white;">
      
                                 <div id="Div2" class="show editorContent" style="position: absolute; margin-top:10px;"/>
                                 <div id="Tb1">
                                  <span style=" margin-bottom:20px;">文件类型：xls<label id="lx" ></label></span>
                                  <br /><br />
                                  <input id="File1"  type="file" style=" width:180px;" runat="server"/>
                                 </div>
                            </fieldset>
                              </form>
                           <div id='uploadStatus' style='display:none; margin-top:70px; '>
                               <img src='images/process.gif' />
                               <div style='color:#ccc;' id="dstext">正在上传，如果长时间不响应，可能是上传文件太大导致出错！
                               </div>
                           </div>
                           
                        </div>
    <div id="dialog-form" title="组信息" style="display: none; height: 100%;">
        <form>
        <fieldset>
            <div id="editorHidden" class="show editorContent" style="position: absolute; margin-top: -20px;">
                <table style="width: 100%; height: 200px">
                    <tbody>
                        <tr>
                            <td style="width: 120px;">
                                <label id="EditorNameLabel" class="useNameLabel">
                                    名称:</label>
                            </td>
                            <td style="width: 220px;">
                                <input id="EditorName" style="width: 180px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label id="Label4" class="useNameLabel">
                                    类型:</label>
                            </td>
                            <td>
                                <table id="rblSize" class="radiolist" border="0" style="width: 180px;">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <input id="rblSize_0" type="radio" name="rblSize" value="0" checked="checked" onclick="javascript:radioClick(this);">
                                                <label for="rblSize_0">
                                                    默认</label>
                                            </td>
                                            <td>
                                                <input id="rblSize_1" type="radio" name="rblSize" value="1" onclick="javascript:radioClick(this);">
                                                <label for="rblSize_1">
                                                    正组</label>
                                            </td>
                                            <td>
                                                <input id="rblSize_2" type="radio" name="rblSize" value="2" onclick="javascript:radioClick(this);">
                                                <label for="rblSize_2">
                                                    副组</label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label id="Label5" class="useNameLabel">
                                    副组列表:</label>
                            </td>
                            <td>
                                <div id="Deputylist">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label id="Label10" class="useNameLabel">
                                    描述:</label>
                            </td>
                            <td>
                                <textarea name="description" id="description" style="height: 40px;"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label id="Label11" class="useNameLabel">
                                    备注:</label>
                            </td>
                            <td>
                                <textarea name="memo" id="memo" style="height: 40px;"></textarea>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <input type="submit" tabindex="-1" style="position: absolute; top: -1000px">
        </fieldset>
        </form>
    </div>
    <div id="dialog-confirm" title="提示">
        <p>
            <span class="ui-icon-alert" style="float: left; margin: 0 7px 20px 0; overflow: hidden;">
            </span>
            <label id="toolstr">
            </label>
        </p>
    </div>
    <div id="dialog-form-user" title="添加指引" style="display: none; height: 100%;">
        <form>
        <fieldset>
                     产品类型：
                     <select id="wg2" style="width:150px;" onchange="">
                       <option value="儿童感冒" selected="selected">儿童感冒</option>
                       <option value="成人感冒">成人感冒</option>
                       <option value="老人感冒">老人感冒</option>
                       <option value="COPD">COPD</option>
                       <option value="儿童哮喘">儿童哮喘</option>
                       <option value="中暑">中暑</option>
                       <option value="重污染">重污染</option>
                     </select>
                       <br />
                       <div style=" margin-left:20px;"> <span style=" margin-bottom:20px; position:absolute; margin-top:40px">指引1：</span><textarea name="description" id="zy1" style="height: 60px; width:480px; margin-top:20px; margin-left:55px;" class=" "></textarea></div>
                       <br />
                       <div style=" margin-left:20px;"><span style=" margin-bottom:20px; position:absolute; margin-top:20px">指引2：</span><textarea name="description" id="zy2" style="height: 60px; margin-left:5px;width:480px;margin-left:55px;" class=" "></textarea></div>
                    <div style=" margin-bottom: 10px; margin-top: 16px;">
               <div style="float: left;">判断要素：</div>
                    <select name="TypeList2" onchange="" id="items" class="grouplist" style="width:150px; margin-left:2px;">
	                   <option selected="selected" value="降温">降温</option>
                                  <option value="温差">温差</option>
                                  <option value="风力">风力</option>
                                  <option value="湿度">湿度</option>
                                  <option value="温度">温度</option>
                                  <option value="高温">高温</option>
                                   <option value="中暑指数">中暑指数</option>
                                  <option value="PM2.5">PM2.5</option>
                                  <option value="PM10">PM10</option>
                                  <option value="O3">O3</option>
                                  <option value="NO2">NO2</option>
                                  <option value="SO2">SO2</option>
                    </select>
                    <br />
                 <div  style=" margin-top:10px">判断条件：
                    <select name="TypeList22" onchange="" id="s1" class="grouplist" style="width:85px; margin-left:-2px;">
	                   <option selected="selected" value="大于">大于</option>
                         <option value="大于等于">大于等于</option>
                          <option value="小于">小于</option>
                            <option value="小于等于">小于等于</option>
                                  <option value="等于">等于</option>
                    </select>
                    <input type="text"  style=" width:40px;margin-top: -23px;margin-left: 165px;" id='s2'/> 
            
                    </div>
                    <select name="TypeList2" onchange="" id="s3" class="grouplist" style="width:55px; margin-left: 212px; margin-top: -23px; position:absolute">
	                              <option selected="selected" value="X"></option>
                                  <option value="并且">并且</option>
                               
                    </select>
              <select name="TypeList22" onchange="" id="s4" class="grouplist" style="width:85px; margin-left: 272px; margin-top: -23px; position:absolute">
	                   <option selected="selected" value="小于">小于</option>
                         <option value="小于等于">小于等于</option>
                          <option value="大于">大于</option>
                            <option value="大于等于">大于等于</option>
                                  <option value="等于">等于</option>
                    </select>
                     <input type="text"  style=" width:40px;margin-left: 362px; margin-top: -23px; position:absolute" id='s5'/> 


                     <div  style=" margin-top:20px" id='months'>判断月份：
                          <table cellpadding="0" cellspacing="0" class="style1" style=" margin-top:-12px; margin-left:75px">
                            <tr>
                            <td>
                                       <input type="checkbox" id="Checkbox01"/></td>
                                <td>
                                    1月</td>
                                <td>
                                       <input type="checkbox" id="Checkbox02"/></td>
                                <td>
                                    2月</td>
                                <td>
                                        <input type="checkbox" id="Checkbox03"/></td>
                                <td>
                                      3月</td>
                                <td>
                                       <input type="checkbox" id="Checkbox04"/></td>
                                <td>
                                        4月</td>
                                <td>
                                 <input type="checkbox" id="Checkbox05"/></td>
                                <td>
                                5月</td>
                                <td>
                                     <input type="checkbox" id="Checkbox06"/></td>
                                <td>
                                    6月</td>
                                <td>
                                  <input type="checkbox" id="Checkbox07"/></td>
                                <td>
                                    7月</td>
                                <td>
                                 <input type="checkbox" id="Checkbox08"/></td>
                                <td>
                                   8月</td>
                                    <td>
                                 <input type="checkbox" id="Checkbox09"/></td>
                                <td>
                                   9月</td>
                                    <td>
                                 <input type="checkbox" id="Checkbox10"/></td>
                                <td>
                                   10月</td>
                                    <td>
                                 <input type="checkbox" id="Checkbox11"/></td>
                                <td>
                                   11月</td>
                                    <td>
                                 <input type="checkbox" id="Checkbox12"/></td>
                                <td>
                                   12月</td>
                            </tr>
                        </table>
                     </div>

            </div>
      
            <input type="submit" tabindex="-1" style="position: absolute; top: -1000px">
        </fieldset>
        </form>
    </div>
    <div id="dialog-form-edit" title="编辑指引信息" style="display: none; height: 100%;">
        <form>
        <fieldset>
            产品类型：
                     <select id="wg3" style="width:150px;" onchange="">
                       <option value="儿童感冒" selected="selected">儿童感冒</option>
                       <option value="成人感冒">成人感冒</option>
                       <option value="老人感冒">老人感冒</option>
                       <option value="COPD">COPD</option>
                       <option value="儿童哮喘">儿童哮喘</option>
                       <option value="中暑">中暑</option>
                       <option value="重污染">重污染</option>
                     </select>
                       <br />
                       <div style=" margin-left:20px;"> <span style=" margin-bottom:20px; position:absolute; margin-top:40px">指引1：</span><textarea name="description" id="zy1_3" style="height: 60px; width:480px; margin-top:20px; margin-left:55px;" class=" "></textarea></div>
                       <br />
                       <div style=" margin-left:20px;"><span style=" margin-bottom:20px; position:absolute; margin-top:20px">指引2：</span><textarea name="description" id="zy2_3" style="height: 60px; margin-left:5px;width:480px;margin-left:55px;" class=" "></textarea></div>
                    <div style=" margin-bottom: 10px; margin-top: 16px;">
               <div style="float: left;">判断要素：</div>
                    <select name="TypeList2" onchange="" id="items_3" class="grouplist" style="width:150px; margin-left:2px;">
	                   <option selected="selected" value="降温">降温</option>
                                  <option value="温差">温差</option>
                                  <option value="风力">风力</option>
                                  <option value="湿度">湿度</option>
                                  <option value="温度">温度</option>
                                  <option value="高温">高温</option>
                                   <option value="中暑指数">中暑指数</option>
                                  <option value="PM2.5">PM2.5</option>
                                  <option value="PM10">PM10</option>
                                  <option value="O3">O3</option>
                                  <option value="NO2">NO2</option>
                                  <option value="SO2">SO2</option>
                    </select>
                    <br />
                 <div  style=" margin-top:10px">判断条件：
                    <select name="TypeList22" onchange="" id="s1_3" class="grouplist" style="width:85px; margin-left:-2px;">
	                   <option selected="selected" value="大于">大于</option>
                         <option value="大于等于">大于等于</option>
                          <option value="小于">小于</option>
                            <option value="小于等于">小于等于</option>
                                  <option value="等于">等于</option>
                    </select>
                    <input type="text"  style=" width:40px;margin-top: -23px;margin-left: 165px;" id='s2_3'/> 
            
                    </div>
                    <select name="TypeList2" onchange="" id="s3_3" class="grouplist" style="width:55px; margin-left: 212px; margin-top: -23px; position:absolute">
	                              <option selected="selected" value="X"></option>
                                  <option value="并且">并且</option>
                                  
                    </select>
              <select name="TypeList22" onchange="" id="s4_3" class="grouplist" style="width:85px; margin-left: 272px; margin-top: -23px; position:absolute">
	                   <option selected="selected" value="小于">小于</option>
                         <option value="小于等于">小于等于</option>
                          <option value="大于">大于</option>
                            <option value="大于等于">大于等于</option>
                                  <option value="等于">等于</option>
                    </select>
                     <input type="text"  style=" width:40px;margin-left: 362px; margin-top: -23px; position:absolute" id='s5_3'/> 


                     <div  style=" margin-top:20px" id='months_3'>判断月份：
                          <table cellpadding="0" cellspacing="0" class="style1" style=" margin-top:-12px; margin-left:75px">
                            <tr>
                            <td>
                                <input type="checkbox" id="Checkbox01_3"/></td>
                                <td>
                                    1月</td>
                                <td>
                                 <input type="checkbox" id="Checkbox02_3"/></td>
                                <td>
                                    2月</td>
                                <td>
                                   <input type="checkbox" id="Checkbox03_3"/></td>
                                <td>
                                      3月</td>
                                <td>
                                  <input type="checkbox" id="Checkbox04_3"/></td>
                                <td>
                                        4月</td>
                                <td>
                                 <input type="checkbox" id="Checkbox05_3"/></td>
                                <td>
                                5月</td>
                                <td>
                                     <input type="checkbox" id="Checkbox06_3"/></td>
                                <td>
                                    6月</td>
                                <td>
                                  <input type="checkbox" id="Checkbox07_3"/></td>
                                <td>
                                    7月</td>
                                <td>
                                 <input type="checkbox" id="Checkbox08_3"/></td>
                                <td>
                                   8月</td>
                                    <td>
                                 <input type="checkbox" id="Checkbox09_3"/></td>
                                <td>
                                   9月</td>
                                    <td>
                                 <input type="checkbox" id="Checkbox10_3"/></td>
                                <td>
                                   10月</td>
                                    <td>
                                 <input type="checkbox" id="Checkbox11_3"/></td>
                                <td>
                                   11月</td>
                                    <td>
                                 <input type="checkbox" id="Checkbox12_3"/></td>
                                <td>
                                   12月</td>
                            </tr>
                        </table>
                     </div>
                     <input type="hidden" value=""  id="standID"/>
                     <input type="hidden" value="" id="monthID" />
                     <input type="hidden" value=""  id="itemName"/>
                     <input type="hidden" value="" id="typeName" />
            </div>
            <input type="submit" tabindex="-1" style="position: absolute; top: -1000px">
        </fieldset>
        </form>
    </div>
</body>
</html>
