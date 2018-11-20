<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Scheduling.aspx.cs" Inherits="WorkGroup22" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<meta http-equiv="Content-Type" contentType="text/xml; charset=gb2312" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>排班管理</title>
 <script language="javascript" type="text/javascript">
  var lastTab = "<%=m_FirstTab %>";
 </script>
 <link href="images/css/Scheduling.css?v=4" rel="stylesheet" type="text/css" />
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
 <script language="javascript" type="text/javascript" src="js/Scheduling.js?v=1"></script>
 <script language="javascript" type="text/javascript" src="js/DataShare.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>

 <link href='../fullcalendar/fullcalendar.css' rel='stylesheet' />
<link href='../fullcalendar/fullcalendar.print.css' rel='stylesheet' media='print' />
<script language="javascript" type="text/javascript" src='../fullcalendar/lib/moment.min.js'></script>
<%--<script language="javascript" type="text/javascript" src='../fullcalendar/lib/jquery.min.js'></script>--%>
<script language="javascript" type="text/javascript" src='../fullcalendar/fullcalendar.min.js'></script>
<script language="javascript" type="text/javascript">
    Ext.onReady(function () {
        //设置界面高度
        var pageHeight = document.documentElement.clientHeight;
        $(".wrapborder").height(pageHeight);
    });
</script>
 <%-- <link rel="stylesheet" href="images/css/jquery-ui.css"/>--%>
  <script src="js/jquery-ui.js"></script>

<script  language="javascript" type="text/javascript">
    $(document).ready(function () {
       // $('#demo').html('<table cellpadding="0" cellspacing="0" border="0" class="display" id="example_us"></table>');
    });
</script>

</head>
<body style="-webkit-overflow-scrolling:touch; overflow: auto;" >
  <form id="Form2" runat="server">
       <asp:HiddenField id="Element" runat="server" />
        <asp:Button ID="btnExport" runat="server" onclick="Button1_Click" Text="Button"  CssClass="inVisibility" />
    <input id="userNames" type="hidden" runat="server"  />
    <div class="wrapborder">
    <div class="border1" style="width:100%;">
    <div class="wrap">
   <div class="content" style="width: 100%">
     <div id='calendar'></div>
   </div></div></div></div>
 </form>
 
 <div id="dialog-form" title="手动排班设置" style=" display:none;  height:100%;">
  <form>
    <fieldset >
         <div id="editorHidden" class="show editorContent" style="position: absolute; margin-top:10px;"></div>
         <div id="Tb1" >
            <div id="tool_textNew">起始时间：</div>
            <div class="selectNewDate" ><input id="H00" runat="server" type="text" class="selectDateFormStyle" style="width:150px"  onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"/></div>
            <br />
            <div id="tool_text22">终止时间：</div>
            <div  class="selectNewDate"  ><input id="H01" runat="server" type="text" class="selectDateFormStyle" style="width:150px" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})"/></div>
             <div style="float:left; margin-top: 20px; margin-bottom: 10px; margin-left:-55px;" id="workGroup">

             </div>
             <div style=" margin-bottom: 10px; margin-top: 60px;">
               <div style="float: left;">值班类型：</div>
                    <select name="TypeList" onchange="queryData1();" id="TypeList" class="grouplist" style="width:150px;">
	                <option selected="selected" value="0">日常值班</option>
	                <option value="1">节假日值班</option>
                    </select>
               </div>
               <div  style="width:390px; height:130px;">
            <table  class="display dataTable no-footer" id="example_us" role="grid" aria-describedby="example_info" border="0" cellspacing="0" cellpadding="0" style=" text-align:center;  ">
            </table>
            </div>
         </div>
        
      <input type="submit" tabindex="-1" style="position:absolute; top:-1000px" />
    </fieldset>
  </form>
</div>

</body>
</html>
