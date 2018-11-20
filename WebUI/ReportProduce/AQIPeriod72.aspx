<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AQIPeriod72.aspx.cs" Inherits="ReportProduce_AQIPeriod72" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/bootstrap-o.css" rel="stylesheet" />
    <link href="css/AQIPeriod72.css" rel="stylesheet" />

    <script src="js/jquery-1.12.4.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/vue.js"></script>
    <script src="js/vue-resource.js"></script>
    <script src="js/Utility.js"></script>
    <script src="js/AQIPeriodData.js"></script>
    <script src="js/AQIPeriod72.js"></script>
    <script language="javascript" type="text/javascript" >
        var period = "<%=period %>";
        console.log(period);
    </script>
    <%--创建一个组件模板--%>
    <script type="text/x-template" id="template">
        <table class="contentTab" style="margin:0 auto;width:97%;margin-bottom:40px;">
            <tr>
                <td style='width:4%;'class='tabletitleOut grid'>预报时效</td>
                <td style='width:4%;'class='tabletitleOut grid'>日期</td>
                <td style='width:4%;'class='tabletitleOut grid'>时段</td>
                <td v-for="(title,keys) in titles">
                    <table>
                        <tr>
                            <td colspan="2" class='tabletitleOut grid'>{{title[2].val}}</td>
                        </tr>
                        <tr>
                            <td colspan="1" class='tabletitleOut grid cellWidth'>{{title[0].val}}</td>
                            <td colspan="1" class='tabletitleOut grid cellWidth'>{{title[1].val}}</td>
                        </tr>
                    </table>
                </td>
            </tr>
             <tr v-for="(items,keys) in results" style="width:100%;">
                <td  style='width:4%;' :rowspan='items.Period[1].rowspan' :class="[items.Period[3].class, {tableLeftPart:true},{grid:true}]">{{items.Period[0].val}}</td>
                <td  style='width:4%;' :rowspan='items.Date[1].rowspan' :class="[items.Date[3].class, {tableLeftPart:true},{grid:true}]">{{items.Date[0].val}}</td>
                <td  style='width:4%;'  :class="[{tableLeftPart:true},{grid:true}]">{{items.Interval[0].val}}</td>
                <td  v-for="(item,key) in items.Ele" class="grideW" v-if="items.Interval[0].val!='全天'">
                    <table>
                        <tr>    
                            <td class='center grid cellWidth' colspan='1'><input type='text' style='border:none;' :value='item[0].val' v-model='item[0].val' disabled="disabled"/></td>
                            <td class='mete grid cellWidth'  colspan='1'><input :disabled='disable':class="[{inBorder:inborder}]" @keyup="keyup(key,1,item)" @blur="blur(key,1,item,item[1].val,items)"  @focus="focus(key,1,item,item[1].val,$event)" type='text' :value='item[1].val' v-model='item[1].val' /></td>
                        </tr>
                        <tr>
                            <td colspan="2" class='general grid'><input :disabled='disable' :class="[{inBorder:inborder}]" :value='item[2].val'type='text' v-model='item[2].val' @keyup="keyup(key,2,item)" @blur="blur(key,2,item,item[2].val,items)"  @focus="focus(key,2,item,item[2].val,$event)"/></td>
                       </tr>
                    </table>
                </td>
                <td  v-for="item in items.Ele" class="grideW" v-else colspan='12' >
                    <table>
                        <tr>    
                            <td class='center mete grid'>
                                <div style="float:left">
                                    <label>AQI:</label><input disabled='disabled' class='sel' type='text' v-model='item[0].val'/>
                                    <label>首要污染物:</label>
                                    <select v-model='item[0].poll' disabled='disabled' class='sel'>
                                        <option v-for="op in options">{{op}}</option>                   
                                    </select>
                                    <label>AQI:</label><input :disabled='disable' class="sel" type='text' v-model='item[1].val'/>
                                    <label>首要污染物:</label>
                                    <select v-model='item[1].poll' class='sel':disabled='disable' >
                                        <option v-for="op in options">{{op}}</option>                   
                                    </select>
                                </div>                   
                            </td>
                        </tr>
                        <tr>
                            <td class='general grid'>
                                <div style="float:left">
                                    <label>AQI:</label><input class='sel':disabled='disable' type='text' v-model='item[2].val'/>
                                    <label>首要污染物:</label>
                                    <select v-model='item[2].poll':disabled='disable'  class='sel'>
                                        <option v-for="op in options">{{op}}</option>                   
                                    </select>
                                </div>
                            </td>
                       </tr>
                    </table>
                </td>
            </tr>
        </table>
    </script>
</head>
<body>
               <%--     <td  style='width:5%;' :rowspan='items.Period[1].rowspan' :class="[items.Period[3].class, {tableLeftPart:true},{grid:true}]">{{items.Period[0].val}}</td>
                <td  style='width:5%;' :rowspan='items.Date[1].rowspan' :class="[items.Date[3].class, {tableLeftPart:true},{grid:true}]">{{items.Date[0].val}}</td>--%>

    <form id="form1" runat="server">
        <div id="page">
            <div class="content" style="background:#eee;">
                    <div id="topInfo" style="height: 30px;line-height:30px;background:#f8f8f8;">
                        <div class="y-item f">
                            <div class="f">预报员：</div>
                            <div class="f" id="forecaster">{{userName}}</div>
                        </div>
                        <div class="y-item f">
                            <div class="f">预报时间：</div>
                            <div class="f" id="lst">{{forecastDate}}</div>
                        </div>
                        <div class="y-item f">
                            <div class="f">预报时次：</div>
                            <div class="f" id="period">{{period}}</div>
                        </div>
                    </div>
                <div class="outLine">
                    <div class="mapTitle">
                        <div class="titlePoint"></div>
                        <div class="titleText"><span>AQI分时段预报表格</span></div>
                        <div class="importBtns">
                            <div id="subjImport" class="importBtn" @click="getSubData('sub')">主观预报</div>
                            <div id="historyData" class="importBtn" @click="getHistoryData()">历史数据</div>
                        </div>
                    </div>
                    <div id="forecastTable" class="forecastTable">
                        <div id="contentTable">
                            <my-component :results="result" :options="option" :disable="disabled" :inborder="inborder"></my-component>
                        </div>
                        <div id="leftTable">

                        </div>
                    </div>

                </div>

            </div>
            <div class="btnArea">
                <div class="btns">
                    <input type="button" class="button_Bottom" @click="preView()"  value="预览"/>
                    <input type="button" class="button_Bottom" :disabled='disabled'  @click="save()" value="保存"/>
                    <input type="button" class="button_Bottom" value="审核"/>
                    <div id="forePub" class="button_Bottom"  @click="test()">测试</div>
                </div>
            </div>
            <!-- 模态框（Modal） -->
            <div class="modal fade" id="myModal" ref="myModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
	            <div class="modal-dialog" style="width:1100px;">
		            <div class="modal-content">
			            <div class="modal-header">
				            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
					            &times;
				            </button>
				            <h4 class="modal-title">
					            AQI分时段预报产品预览
				            </h4>
			            </div>
			            <div class="modal-body">
                                <ul class="nav nav-tabs">
                                    <li class="active">
		                                <a href="#text" data-toggle="tab">72小时AQI分时段预报</a>
	                                </li>
	                                <li>
                                        <a href="#message" data-toggle="tab">AQI分时段预报短信</a>
                                    </li>
                                </ul>
                                <div class="tab-content">
                                    <textarea class="tab-pane fade in active" id="text":disabled='disabled' >
	                                </textarea>
	                                <textarea class="tab-pane fade" id="message" :disabled='disabled'></textarea>
                                </div>
			            </div>
			            <div class="modal-footer">
				            <button type="button" class="btn btn-default":disabled='disabled' @click="txtSave()">保存</button>
				            <button type="button" class="btn btn-default":disabled='disabled' @click="publish()">发布</button>
                            <button type="button" class="btn btn-primary" data-dismiss="modal">关闭</button>
			            </div>
		            </div><!-- /.modal-content -->
	            </div><!-- /.modal -->
            </div>
        </div>
    </form>
<textarea id="TxtTemplete_17" style="display:none;" cols="20" rows="2">
上海市空气质量预报			
(	{PublishDate}	{Hour}时发布）
时段	AQI	空气质量等级	首要污染物
第一个夜间（20时—06时）	{firstYJAQI}	{firstYJLevel}  {firstYJItem}
第一个上午（06时—12时）	{firstSWAQI}	{firstSWLevel}	{firstSWItem}
第一个下午（12时—20时）	{firstXWAQI}	{firstXWLevel}	{firstXWItem}
第二个夜间（20时—06时）	{secondYJAQI}	{secondYJLevel}	{secondYJItem}
第一个    （00时—24时）  {firstQTAQI}	{firstQTLevel}	{firstQTItem}
第二个    （00时—24时）  {secondQTAQI}	{secondQTLevel}	{secondQTItem}

上海中心气象台
上海市环境监测中心
联合发布
</textarea>
<textarea id="TxtTemplete_06" style="display:none;">
上海市空气质量预报			
(	{PublishDate}	{Hour}时发布）
时段	AQI	空气质量等级	首要污染物
第一个上午（06时—12时）	{firstSWAQI}	{firstSWLevel}	{firstSWItem}
第一个下午（12时—20时）	{firstXWAQI}	{firstXWLevel}	{firstXWItem}
第二个夜间（20时—06时）	{secondYJAQI}	{secondYJLevel}	{secondYJItem}
第一个    （00时—24时）  {firstQTAQI}	{firstQTLevel}	{firstQTItem}
第二个    （00时—24时）  {secondQTAQI}	{secondQTLevel}	{secondQTItem}
			
上海中心气象台
上海市环境监测中心
联合发布
</textarea>
</body>
        
</html>
