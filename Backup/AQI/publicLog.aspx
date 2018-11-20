<%@ Page Language="C#" AutoEventWireup="true" CodeFile="publicLog.aspx.cs" Inherits="AQI_publicLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>发布日志</title>
 <script language="javascript" type="text/javascript">
//        <%if (m_UnLogin) {%>
//            top.location.href = "../Default.aspx";
//        <%} %>
 </script>
 
 <link href="images/css/css.css" rel="stylesheet" type="text/css" />
 <link href="images/css/publicLog.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 
 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="js/publicLog.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>

</head>
<body  style="-webkit-overflow-scrolling:touch; overflow: auto;">
   <div class="content" style="width: 100%" id="height">
    <div id="contentNone">
        <div class="publicSty">发布日志</div>
        <div id="tool">
             <div class="checkStyle">
              <div class="checkLable" style="width: 60px">数据源：</div>
              <select name="dataReSource" class="listStytel" id="selectSource" onchange="selectResourceChange(this.options[this.options.selectedIndex].value)";>
               <option value="所有">所有</option>
              <option value="综合预报">综合预报</option>
              <option value="预报更新">预报更新</option>
              </select>
            </div>
            <div class="checkStyle">
              <div class="checkLable">发布方式：</div>
              <select name="dataReSource" class="listStytel" id="select" onchange="selectStyleChange(this.options[this.options.selectedIndex].value)";>
               <option value="所有">所有</option>
              <option value="移动短信">移动短信</option>
              <option value="电信联通">电信联通</option>
              <option value="宣教中心">宣教中心</option>
              <option value="市环保局">市环保局</option>
              <option value="新浪微博">新浪微博</option>
              <option value="腾讯微博">腾讯微博</option>
              <option value="实时发布系统">实时发布系统</option>
              </select>
            </div>
            <div class="checkStyle">
                 <div class="checkLable">开始时间：</div>
                 <input id="H00" type="text" class="selectDateFormStyle" value="<%= m_FromDate%>"  onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/>
            </div>
            <div class="checkStyle">
                 <div class="checkLable">结束时间：</div>
                 <input id="H01" type="text" class="selectDateFormStyle" value="<%= m_ToDate%>" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/>
            </div>
            <div id="tool_btn_area" style="float: left; width: 100px; margin-left: 50px;">
            <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="publicLogQuery()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
             <%--<input type="button"  id="btnQuery" class="normal-btn input-btnQuery" value="查询" onclick="publicLogQuery()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'"/>--%>
          <%--<input type="button" id="btnExport" class="normal-btn input-btnQuery" value="导出" onclick="queryExportData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'"/>--%>
<%--            <input type="button" id="btnQuery" class="queryButton defaultQueryButton"  onmouseover="this.className = 'queryButton overQueryButton';" onmouseout ="this.className ='queryButton defaultQueryButton';"  onclick="publicLogQuery()"  />  --%>              
            </div>
        </div>
    </div>
    <div id="content" onmouseout="contentMouseOut()">
    </div> 
    <div id="webViewDiv" class="webScan" style="display:none;">
    </div>
    <div id="messageDiv"  class="webScan" style="display:none;">
    </div>
    </div>
</body>
</html>
