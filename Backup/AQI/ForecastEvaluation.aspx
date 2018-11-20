<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForecastEvaluation.aspx.cs" Inherits="AQI_ForecastEvaluation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>预报查询</title>
 <script language="javascript" type="text/javascript">
 var lastTab = "<%=m_FirstTab %>";//当前选中的污染物
 </script>
 <link href="images/css/forecastEvaluation.css" rel="stylesheet" type="text/css" />
 <link href="images/css/css.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 <script type="text/javascript" src="http://code.jquery.com/jquery-1.6.1.min.js"></script>
 
 <script language="javascript" type="text/javascript" src="js/AQIUtility.js"></script>
 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="js/ForecastEvaluation.js"></script>
 <script language="javascript" type="text/javascript" src="js/DataShare.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
 
</head>
<body style="-webkit-overflow-scrolling:touch;">
    <form id="form1" runat="server">
    <asp:HiddenField id="Element" runat="server" />
   <div class="content" style="width: 100%">
   <div id="title" >
            <div id="title1h">上海市环境空气质量预报</div>
            <div id="titleDate">考核分数情况</div>
            <input id="nowDateTime" type="hidden" />
        </div>
   <div id="contentNone" style="margin-top: 10px">
      <div id="tool_duibi">
        <div id="tool_text">起报时间：</div>
         <div class="selectNewDate"><input id="H00" runat="server" type="text" class="selectDateFormStyle"  onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/></div>
         <div  class="selectNewDate"><input id="H01" runat="server" type="text" class="selectDateFormStyle" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日'})"/></div>
      <%--    <label id="userNames" style="line-height: 25px; float: left; margin-right: 5px; margin-top:6px;">发布用户：</label>
        <div id="userSelectOpts" style="width: 120px; height: 30px; float: left;">
        </div>--%>
         
        <div id="zonghe_duibi" style=" display:none">
         <div id="name_duibi">综合预报：</div>
         <div id="rd1" class="radioChecked"><a href="javascript:radioClick('rd1');">24h</a></div>
         <div id="rd2" class="radioUnChecked"><a href="javascript:radioClick('rd2');">48h</a></div>
         </div>
         <div id="tool_btn">
           <button type="button" class="normal-btn input-btnQuery"  id="btnEvaluation"   onclick="CreateEvaluatioinData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text" style="width:64px;">数据加工</span>
             </button>
          <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="queryData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
         <button type="button" class="normal-btn input-btnQuery"  id="ExportData"  onclick="queryExportData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-export"></span>
            <span class="select-text">导出</span>
         </button>
         <%-- <input type="button"  id="btnQuery" class="normal-btn input-btnQuery" value="查询" onclick="queryData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'"/>
          <input type="button" id="btnExport" class="normal-btn input-btnQuery" value="导出" onclick="queryExportData()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'"/>--%>
            <%--<input type="button" id="btnQuery" class="queryButton defaultQueryButton"  onmouseover="this.className = 'queryButton overQueryButton';" onmouseout ="this.className ='queryButton defaultQueryButton';"  onclick="queryData()"  />                
            <input type="button" id="btnExport" class="queryButton defaultExportButton"  onmouseover="this.className = 'queryButton overExportButton';" onmouseout ="this.className ='queryButton defaultExportButton';"  onclick="queryExportData()"  />                --%>
         </div>
       </div>
       
       <div id="type_select1row" style=" display:none">
       <label>预报时段：</label>
       <label class="forecast"><input type="checkbox" name="forecasPeriod" checked ="checked"  id="noon" value ="2" />上午</label>
       <label class="forecast"><input type="checkbox" name="forecasPeriod" checked ="checked"  id="afternoon"  value ="3" />下午</label>
       <label class="forecast"><input type="checkbox" name="forecasPeriod" checked ="checked"  id="night" value ="6" />夜晚</label>
       <label class="forecast"><input type="checkbox" name="forecasPeriod" id="dayAve"  checked ="checked"  value ="7" />日平均</label>
       <label  style=" visibility:hidden" class="forecast"><input style=" visibility:hidden" type="checkbox" name="forecasPeriod" id="shangNight"  value ="4" /></label>
       <label style=" visibility:hidden" class="forecast"><input style=" visibility:hidden" type="checkbox" name="forecasPeriod" id="xiaNight"  value ="1" /></label>
       
       </div>
       <div id="type_select2row" style="display:none">
       <label>数据类型：</label>
       <label class="forecast"><input type="checkbox" name="dataType"   checked ="checked" id="shiCe"  value ="1" />实测</label>
       <label class="forecast"><input type="checkbox" name="dataModule" checked ="checked"  id="manual" value ="manual" />综合预报</label>
       <label class="forecast"><input type="checkbox" name="dataModule" id="CMAQ"  value ="CMAQ" />CMAQ </label>
       <label class="forecast"><input type="checkbox" name="dataModule" id="WRF"  value ="WRF" />WRF-CHEM</label>
       </div>
     </div>
          
       <div id="tabbtn" style="width: 1046px; height:0px; margin-top:0px;">
           <ul id="tabItem"  runat="server">
      
           </ul>
       </div>
       <div id="contents" style=" margin-top:0px;">
          
           <%--     <fsvc:GridView ID="gvList" IsOpenOnDblClick="True" DataKeyNames ="id"  
                                  runat="server"  AllowMultiColumnSorting="True" AutoGenerateColumns="False" 
                                  EmptyDataText="无数据" AllowSorting="True" EnableEmptyContentRender="True" 
                                  ShowColNumber="15" >
                            <Columns> 
                                <asp:TemplateField HeaderText="序号">
	                                <headerstyle horizontalalign="Center" width="5%"></headerstyle>
	                                <itemstyle cssclass="ListColumnStyle" horizontalalign="Center"></itemstyle>
	                                <itemtemplate>
		                                <%#  Pager1.CurrentPageIndex * Pager1.PageSize + Container.DataItemIndex  +  1%>
	                                </itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <input type="checkbox" id="Cb_SelectAll" onclick="JavaScript:CheckAll(this)"  />全选
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <input type="checkbox"  <%#  DataBinder.Eval(Container.DataItem,"stateType").ToString() %>   id="CbCId_<%#  DataBinder.Eval(Container.DataItem,"Id").ToString() %>" />
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                <asp:HyperLinkField DataTextField="FlowCode" SortExpression="FlowCode" 
                                    HeaderText="编号"   />
                                <asp:HyperLinkField DataTextField="comName" SortExpression="comName"  HeaderText="货主"   />
                                <asp:HyperLinkField DataTextField="ItemName" SortExpression="ItemName"  HeaderText="产品名称"   />
                                <asp:HyperLinkField DataTextField="ItemsNum" SortExpression="ItemsNum"  HeaderText="数量"   />
                                <asp:HyperLinkField DataTextField="Unit" SortExpression="Unit"  HeaderText="单位"   />
                                <asp:HyperLinkField DataTextField="SAddress" SortExpression="SAddress"  HeaderText="产地"   />
                                <asp:HyperLinkField DataTextField="EAddress" SortExpression="EAddress"  HeaderText="目的地"   />
                                <asp:HyperLinkField DataTextField="CheckTime" HeaderText="检疫时间" SortExpression="CheckTime" />
                                <asp:BoundField DataField="CheckUser" HeaderText="兽医" />
                            </Columns>

                    <HeaderStyle Wrap="False"></HeaderStyle>

                            <RowStyle HorizontalAlign="Center" />
                        </fsvc:GridView>
         <fsvc:Pager ID="Pager1" runat="server">
                        </fsvc:Pager> --%>
           <div id="Tb0" class="hidden">
           
           </div>
            <div id="Tb1" >
            
           </div>
           <div id="Tb2" class="hidden">
           
           </div>
            <div id="Tb3" class="hidden">
            
           </div>
            <div id="Tb4" class="hidden">
            
           </div>
            <div id="Tb5" class="hidden">
            
           </div>
      </div>

  </div>
    <asp:Button ID="SearchBut" runat="server" onclick="Button1_Click" Text="Button"  CssClass="inVisibility"/>
</form>
</body>
</html>
