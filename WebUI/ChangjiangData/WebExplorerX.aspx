<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebExplorerX.aspx.cs" Inherits="ChangjiangData_WebExplorerX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>长三角预报</title>

     <link rel="stylesheet" type="text/css" href="../resources/syntax/shCore.css"/>
 <link rel="stylesheet" href="../themes/base/jquery.ui.all.css"/>
<%-- <link rel="stylesheet" href="../css/demos.css"/>--%>
     <link href="images/css/WorkGroup.css?v=4" rel="stylesheet" type="text/css" />
  <link href="images/css/css.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>

 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="../JS/jquery-1.9.1.js"></script>
<script type="text/javascript" language="javascript" src="../media/js/jquery.js"></script>
<script type="text/javascript" language="javascript" src="../resources/syntax/shCore.js"></script>
<script type="text/javascript" language="javascript" src="../resources/demo.js"></script>
 <script type="text/javascript" src="../ui/jquery.ui.core.js"></script>
 <script type="text/javascript" src="../ui/jquery.ui.widget.js"></script>

 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
  <script language="javascript" type="text/javascript" src="js/WebExplorerX.js"></script>
  <script src="js/jquery-ui.js"></script>

  <link rel="stylesheet" type="text/css" media="screen, projection" href="../CSSNiceMenu/demo.css" />
<%--	<script type="text/javascript" src="../CSSNiceMenu/jquery-1.4.2.min.js"></script>--%>
	<%--	<script type="text/javascript" src="../CSSNiceMenu/jquery.color.js"></script>--%>

  		<script type="text/javascript">

  		    $(document).ready(function () {

  		        $('#menu-jquery li a').hover(

					function () {
					    alert("xxx");

					    $(this).css('padding', '5px 15px')
								 .animate({ 'paddingLeft': '21px',
								     'paddingRight': '21px',
								     'backgroundColor': 'rgba(0,0,0,0.5)'
								 },
											 'fast');
					},

					function () {

					    $(this).css('padding', '5px 21px')
								 .animate({ 'paddingLeft': '15px',
								     'paddingRight': '15px',
								     'backgroundColor': 'rgba(0,0,0,0.2)'
								 },
								 			'fast');

					}).mousedown(function () {

					    $(this).animate({ 'backgroundColor': 'rgba(0,0,0,0.1)' }, 'fast');

					}).mouseup(function () {

					    $(this).animate({ 'backgroundColor': 'rgba(0,0,0,0.5)' }, 'fast');
					});
  		    });
			
		</script>
</head>
<body style=" margin-top:10px;"> 

       <div id="rightPanel" style=" width:920px; margin-left:auto; margin-right:auto; ">
            <div id="zonghe_duibi" style="  width:920px; margin-left:5px; ">

                 <div id="tool_textNew" style=" margin-left:20px; cursor:pointer" >开始时间：</div>
         <div class="selectNewDate"  style="width:150px; cursor:pointer "><input id="H00" runat="server" type="text" class="selectDateFormStyle" style="width:150px; cursor:pointer"  onclick=" WdatePicker({dateFmt:'yyyy-MM-dd',onpicked:function() {}});  "/></div> 

                          <div id="tool_textNew" style=" margin-left:10px;">结束时间：</div>
         <div class="selectNewDate"  style="width:150px;  "><input id="H01" runat="server" type="text" class="selectDateFormStyle" style="width:150px; cursor:pointer"  onclick="WdatePicker({dateFmt:'yyyy-MM-dd',onpicked:function() {}});  "/></div> 
                         <div class="dataTimeHS" style="  margin-top:-11px;  margin-right:80px; ">
         <button type="button" class="normal-btn input-btnQuery" id="yesDay" onclick="queryData();"   onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
         </button>
         <button type="button" class="normal-btn input-btnQuery" id="toDay" onclick="today()"onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-now"></span>
            <span class="select-text">今天</span>
         </button>

         </div>
         <div  class="dataTimeHS" style=" display:none; float:right; margin-right:32px; margin-top:5px; width:900px; margin-bottom:20px; padding-bottom:10px; ">
              <div id="name_duibi" >区域：</div>  
                  <div id="rd5" class="radioChecked" onclick=""><a href="javascript:radioClick('rd5');" title="">全部</a></div>
                  <div id="rd3" class="radioUnChecked" onclick=""><a href="javascript:radioClick('rd3');" title="">上海</a></div>
                  <div id="rd1" class="radioUnChecked" onclick=""><a href="javascript:radioClick('rd1');" title="">江苏</a></div>
                  <div id="rd2" class="radioUnChecked" onclick=""><a href="javascript:radioClick('rd2');" title="">浙江</a></div>
                  <div id="rd4" class="radioUnChecked" onclick=""><a href="javascript:radioClick('rd4');" title="">安徽</a></div>
         </div>
      
      </div>

 <div id="content" style="width: 900px;  position:absolute;  margin-top:60px;  border-top:2px solid #d7d7d7; ">

</div>
    </div>
      <div id="repeat"  style=" visibility:hidden">
       <div style=" border:0px dotted green; width:200px; margin-right:60px; " >
<%--        <button   onclick="alert('xxxxx1');" title="下载" style="width:30px; height:20px; margin-bottom:2px; "  class="ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only" ><span class="ui-button-icon-primary ui-icon ui-icon-up">下载</span></button>
        <button   onclick="alert('xxxxx2');" title="重命名" style="width:35px; height:20px;margin-bottom:2px;"  class="ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only"  ><span class="ui-button-icon-primary ui-icon ui-icon-rename">重命名</span></button>
        <button   onclick="alert('xxxxx3');" title="删除" style="width:35px; height:20px;margin-bottom:2px;margin-right:35px;"  class="ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only" ><span class="ui-button-icon-primary ui-icon ui-icon-del"></span></button>--%>
<%--       <input type="button" title="下载"  value="下载" style=" text-align:left; width:50px;  height:25px;   margin-bottom:2px; " class="ui-button"   onclick="download(this)"  />
        <input type="button" title="重命名"  value="重命名" style=" text-align:left; width:60px; height:25px; margin-bottom:2px;" class="ui-button"   onclick="rename_open(this)"  />
        <input type="button" title="删除"  value="删除" style=" text-align:left; width:50px; height:25px; margin-bottom:2px; " class="ui-button"   onclick="del_open(this)"  />--%>
        	<ul id="menu-css">
		<li ><a  title="删除" href="#"  onclick="del_open(this)" style="width:10px; height:8px;  margin-bottom:-3px; background-image:url('images/sc.png') ; background-repeat:no-repeat; background-position:center" ></a></li>
		<li  ><a title="重命名" href="#" onclick="rename_open(this)" style="width:10px; height:8px; margin-bottom:-3px;background-image:url('images/bj.png'); background-repeat:no-repeat; background-position:center"></a></li>
		<li ><a title="下载" href="#" onclick="downloads(this)" style="width:10px; height:8px;margin-bottom:-3px; background-image:url('images/yj.png') ; background-repeat:no-repeat; background-position:center"></a></li>
		
	</ul>
  <%--      <ul id="menu-jquery">
      	<li><a href="#">Home</a></li>
		<li><a href="#">About</a></li>
		<li><a href="#">Portfolio</a></li>
		<li><a href="#">Contact</a></li>
	   </ul>--%>

        </div>
  </div>
<input id="localTime" type="hidden"  runat="server"/>
<iframe id="uploadFrm" frameborder="no" border="0" scrolling="no" allowtransparency="yes" onload="iframeOnload()" name="uploadFrm" style="width:0px; height:0px; display:none;"></iframe>
<div id="dialog-form" title="文件上传" style=" display:none;  height:100%;">
  <form  name="actionForm" id="actionForm" action="WebExplorerss.ashx" method="post" target="uploadFrm"  enctype="multipart/form-data">
    <fieldset  style=" border:0 none white;">
         <div id="editorHidden" class="show editorContent" style="position: absolute; margin-top:10px;"/>
         <div id="Tb1">
          <span style=" margin-bottom:40px; ">区&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;域：<label id="qy" ></label></span><br /><br />
          <span style=" margin-bottom:20px;">文件类型：<label id="lx" ></label></span><br /><br />
          <input  type="file" style=" margin-left:12px; width:150px;" runat="server"/>
         </div>
    <%--  <input type="submit" tabindex="-1" style="position:absolute; top:-1000px"/>--%>
    </fieldset>
  </form>
   <div id='uploadStatus' style='display:none; margin-top:50px; margin-left:35px;'><img src='images/process.gif' /><div style='color:#ccc;' id="dstext">正在上传，如果长时间不响应，可能是上传文件太大导致出错！</div></div>
</div>
<div id="dialog-rename" title="文件重命名" style=" display:none;  height:100%;">
  <form name="actionForms" id="actionForms" action="WebExplorerss.ashx" method="post" target="uploadFrm"  enctype="multipart/form-data">
    <fieldset  style=" border:0 none white;">
         <div id="Div2" class="show editorContent" style="position: absolute; margin-top:10px;"/>
         <div id="ds_rename">
          <span style=" margin-bottom:20px;">请输入新文件名：<input type="text" id="file_name" /></span><br /><br />
         </div>
    <%--  <input type="submit" tabindex="-1" style="position:absolute; top:-1000px"/>--%>
    </fieldset>
  </form>
  <div style='color:#ccc;  margin-top:30px; margin-left:35px;' id="txt">正在修改中....！</div>
</div>

<div id="dialog-del" title="文件删除" style=" display:none;  height:100%;">
  <form name="actionFormss" id="actionFormss" action="WebExplorerss.ashx" method="post" target="uploadFrm"  enctype="multipart/form-data">
    <fieldset  style=" border:0 none white;">
         <div id="Div3" class="show editorContent" style="position: absolute; margin-top:10px;"/>
         <div id="Div4">
          <span style=" margin-bottom:20px;">是否删除文件： <label id="txt_file" ></label></span><br /><br />
         </div>
    </fieldset>
  </form>
  <div style='color:#ccc;  margin-top:30px; margin-left:35px;' id="txt_del">正在删除中....！</div>
</div>

</body>

</html>
