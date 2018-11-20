<%@ Page Language="C#" AutoEventWireup="true" CodeFile="副本(2) WorkGroup.aspx.cs" Inherits="WorkGroup222" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>工作组管理</title>
 <script language="javascript" type="text/javascript">
 var lastTab = "<%=m_FirstTab %>";
 </script>
 <link href="images/css/WorkGroup.css?v=4" rel="stylesheet" type="text/css" />
 <link rel="stylesheet" type="text/css" href="../media/css/jquery.dataTables.css"/>
 <link rel="stylesheet" type="text/css" href="../TableTools/css/dataTables.tableTools.css"/>
 <link rel="stylesheet" type="text/css" href="../resources/syntax/shCore.css"/>
 <link rel="stylesheet" href="../themes/base/jquery.ui.all.css"/>
 <link rel="stylesheet" href="../css/demos.css"/>
 <link href="images/css/css.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
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
 <script type="text/javascript"  src="../ui/jquery.ui.accordion.js"></script>
 <script language="javascript" type="text/javascript" src="js/WorkGroup.js?v=1"></script>
 <script language="javascript" type="text/javascript" src="js/DataShare.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>


  <link rel="stylesheet" href="images/css/jquery-ui.css"/>
  
  <script src="js/jquery-ui.js"></script>
  <link rel="stylesheet" href="images/csss/style.css"/>

 	<script type="text/javascript" language="javascript" >
 	    $(document).ready(function () {
 	        $('#demo').html('<table cellpadding="0" cellspacing="0" border="0" class="display" id="example"></table>');
 	    });
 	    $(function () {
 	        $("#radio").buttonset();
 	    });
	</script>
   <style type="text/css">
    label, input { display:block; }
    input.text { margin-bottom:12px; width:95%; padding: .4em; }
    fieldset { padding:0; border:0; margin-top:25px; }
    h1 { font-size: 1.2em; margin: .6em 0; }
    div#users-contain { width: 350px; margin: 20px 0; }
    div#users-contain table { margin: 1em 0; border-collapse: collapse; width: 100%; }
    div#users-contain table td, div#users-contain table th { border: 1px solid #eee; padding: .6em 10px; text-align: left; }
    .ui-dialog .ui-state-error { padding: .3em; }
  </style>
</head>
<body style="-webkit-overflow-scrolling:touch; overflow: auto;" >
  <form runat="server">
      <div class="page_top">
        <div class="titleName">
            工作组人员分配</div>
    </div>
   <div class="content" style="width: 100%">
      <div id="tabbtn" style="width: 1046px">
           <ul id="tabItem"  runat="server">
           </ul>

       </div>
      <div id="content">
      <div class="page_top">
        <div class="titleName">
            工作组管理</div>
    </div>
       <button type="button" class="normal-btn input-btnQuery" id="ExportData" onclick="addUser()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup="this.className='normal-btn input-btnQuery'" style=" margin-top:10px; margin-bottom:10px; font-size:15px;">
            <span class="select-add"></span>
            <span class="select-text">新增</span>
          </button>

           <div id="Tb0"  >
               <table class="display dataTable no-footer" id="example" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style=" text-align:center">
           
            </table>
           </div>
            <div id="Tb1" class="hidden" >
            
           </div>
           <div id="Tb2" class="hidden">
           
           </div>
      </div>
      </div>
    <asp:HiddenField ID="HiddenField_Sites" runat="server" />
     <asp:HiddenField ID="HiddenField_Sites_status" runat="server" />
    <asp:HiddenField ID="HiddenField_currID" runat="server" />
    <asp:HiddenField ID="HiddenField_Groups" runat="server" />
    </form>

    <div id="dialog-form" title="组信息" style=" display:none;  height:100%;">
 
  <form>
    <fieldset>
       <div id="editorHidden" class="show editorContent" style="position: absolute; margin-top:-20px;">
    <table style=" width:100%; height:200px">
        <tbody>
        <tr >
            <td style=" width:120px;"><label id="EditorNameLabel" class="useNameLabel">名称:</label></td>
            <td style=" width:220px;"> <input id="EditorName" style="width:180px;" /></td>
        </tr>
        <tr>
            <td><label id="Label4" class="useNameLabel">类型:</label></td>
            <td >
             <table id="rblSize" class="radiolist" border="0" style="width:180px;">
		<tbody><tr>
			<td>
            <input id="rblSize_0" type="radio" name="rblSize" value="0" checked="checked" onclick="javascript:radioClick(this);">
            <label for="rblSize_0">默认</label></td><td>
            <input id="rblSize_1" type="radio" name="rblSize" value="1" onclick="javascript:radioClick(this);">
            <label for="rblSize_1">正组</label></td><td>
            <input id="rblSize_2" type="radio" name="rblSize" value="2" onclick="javascript:radioClick(this);">
            <label for="rblSize_2">副组</label></td>
		</tr>
	</tbody></table>
            </td>
        </tr>
        <tr>
            <td><label id="Label5" class="useNameLabel">副组列表:</label></td>
            <td>
            <div id="Deputylist">
             
            </div>
        </td>
        </tr>
        <tr>
            <td><label id="Label10" class="useNameLabel">描述:</label></td>
            <td><textarea name="description" id="description" style="height: 40px;"></textarea></td>
        </tr>
        <tr>
            <td><label id="Label11" class="useNameLabel">备注:</label></td>
            <td>
            <textarea name="memo" id="memo" style="height: 40px;"></textarea>
            </td>
        </tr>
    </tbody></table>
    </div>
      <input type="submit" tabindex="-1" style="position:absolute; top:-1000px">
    </fieldset>
  </form>
</div>
 <div id="dialog-confirm" title="提示" >
  <p><span class="ui-icon-alert" style="float:left; margin:0 7px 20px 0; overflow:hidden;"></span><label id="toolstr"></label></p>
</div>
</body>
</html>
