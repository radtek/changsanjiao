<%@ Page Language="C#" AutoEventWireup="true" CodeFile="visibilityForecast.aspx.cs" Inherits="XGB_visibilityForecast" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../EvaluateHtml/JS/jquery-1.9.1.js"></script>
    <link href="css/visibilityForecast.css?V=20180802" rel="stylesheet" />
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="../EvaluateHtml/css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="../DatePicker/skin/WdatePicker.css" rel="stylesheet" />

    <script src="js/leaflet.js"></script>
    <link href="../AQI/css/leaflet.css" rel="stylesheet" />
    <link href="css/easyui.css" rel="stylesheet" />
    <link href="lib/bootstrap-table/bootstrap-table.css" rel="stylesheet" />

    <script src="lib/bootstrap.min.js"></script>
    <script src="../HealthyWeather/js/bootstrap-select.min.js"></script>
    <script src="../DatePicker/WdatePicker.js"></script>

    <script src="js/highcharts.js"></script>
    <script src="js/jquery.easyui.min.js"></script>
    
    <script src="js/contour.js"></script>

    <script src="lib/bootstrap-table/bootstrap-table.js"></script>
    <script src="lib/bootstrap-table/bootstrap-table-zh-CN.js"></script>

    <script src="lib/leaflet/esri-leaflet.js"></script>
    <script src="lib/leaflet/leaflet-windbarb.js"></script>
    <script src="lib/leaflet/leaflet-isosceles.js"></script>


    <script src="js/visibilityForecast.js?V=20180803"></script>

</head>
<body>
    <%--    <form id="form1" runat="server">
        
    </form>--%>
    <%--    <div class="top">
            <div class="form-group col-sm-4 " style="display: flex;align-items: center;">
                <div class="time-wrap">时间：</div>
                <div class="col-sm-8">
                    <input type="text" class="form-control" id="time" onclick=" WdatePicker({ dateFmt: 'yyyy-MM-dd HH:00:00' });">
                </div>
            </div>

            <div class="selected-wrap"><input type="button" class="btn btn-default" style="display: inherit;" onclick="query()" value="查询" /></div>
        </div>--%>
    <div class="map-container-wrap">
        <div id="map" class="map-container">
            <div id="sitePanel">
                <div class="PanleTop">
                    <span id="panelTitle"></span><span  id="closeButton">×</span>

                </div>

                 <p id="typeName">
            </p>
            <div class="typeButtons">
                <label class="checkbox-inline">
                    <input type="radio" class="chartType" name="optionsRadiosinline" style="vertical-align: middle; margin-top: 0px;cursor:pointer;margin-right:10px;"
                        id="Radio1" value="visClass" checked="checked" />能见度等级</label>
                <label class="checkbox-inline">
                    <input type="radio" class="chartType" name="optionsRadiosinline" style="vertical-align: middle; margin-top: 0px;cursor:pointer;margin-right:10px;"
                        id="Radio2" value="visValue" />能见度值</label>
            </div>

                <div id="chartContainer">
                </div>
            </div>
        </div>

        <div id="rightBar" class="right-bar-wrap">
            <div class="model1-wrap">
                <div class="time-type-wrap">
                    <div class="time-wrap">
                        <div class="time-text1">查询时间：</div>
                        <div class="col-sm-4">
                            <input type="text" id="startDate" class="form-control"  onclick=" WdatePicker({ dateFmt: 'yyyy-MM-dd HH', onpicked: toDOFun });">
                        </div>
                        <div class="time-text2">至</div>
                        <div class="col-sm-4">
                            <input type="text" id="endDate" class="form-control" onclick=" WdatePicker({ dateFmt: 'yyyy-MM-dd HH', onpicked: toDOFun });">
                        </div>
                    </div>
                    <div class="time-wrap">
                        <div class="time-text3">时效：</div>
                        <div class="ipnut-wrap">
                            <div class="radio-wrap">
                                <input type="radio" class="radioHours" name="hours" value="24" checked="checked" /><b>&nbsp;&nbsp;24小时</b></div>
                            <div class="radio-wrap">
                                <input type="radio" class="radioHours" name="hours" value="48" /><b>&nbsp;&nbsp;48小时</b></div>
                            <div class="radio-wrap">
                                <input type="radio" class="radioHours" name="hours" value="72" /><b>&nbsp;&nbsp;72小时</b></div>
                        </div>
                    </div>
                </div>
                <div class="line"></div>



                <div class="table-wrap">
                    <table data-toggle="table">
                        <thead>
                            <tr>
                                <th>站点</th>
                                <th>平均偏差</th>
                                <th>均方根偏差</th>
                                <th>相关系数</th>
                                <th>TS评分</th>
                            </tr>
                        </thead>
                        <tbody>
<%--                            <tr>
                                <td>1</td>
                                <td>2</td>
                                <td>3</td>
                                <td>4</td>
                                <td>5</td>
                            </tr>
                            <tr>
                                <td>1</td>
                                <td>2</td>
                                <td>3</td>
                                <td>4</td>
                                <td>5</td>
                            </tr>
                            <tr>
                                <td>1</td>
                                <td>2</td>
                                <td>3</td>
                                <td>4</td>
                                <td>5</td>
                            </tr>
                            <tr>
                                <td>1</td>
                                <td>2</td>
                                <td>3</td>
                                <td>4</td>
                                <td>5</td>
                            </tr>--%>
                        </tbody>
                    </table>
                </div>

                <%--<table class="easyui-datagrid" title="Basic DataGrid" style="width:700px;height:250px"
			data-options="singleSelect:true,collapsible:true,url:'datagrid_data1.json',method:'get'">
		<thead>
			<tr>
				<th data-options="field:'itemid',width:80">Item ID</th>
				<th data-options="field:'productid',width:100">Product</th>
				<th data-options="field:'listprice',width:80,align:'right'">List Price</th>
				<th data-options="field:'unitcost',width:80,align:'right'">Unit Cost</th>
				<th data-options="field:'attr1',width:250">Attribute</th>
				<th data-options="field:'status',width:60,align:'center'">Status</th>
			</tr>
		</thead>
	</table>--%>
            </div>
        </div>

        <div id="legend" class="legend" style="display:none">
            <div class="legend-title">能见度等级</div>
            <div class="icon-text-wrap"><div class="icon color0"></div><span class="text-style">0 (5000m)</span></div>
            <div class="icon-text-wrap"><div class="icon color1"></div><span class="text-style">1 (3000-5000m)</span></div>
            <div class="icon-text-wrap"><div class="icon color2"></div><span class="text-style">2 (1000-3000m)</span></div>
            <div class="icon-text-wrap"><div class="icon color3"></div><span class="text-style">3 (500-1000m)</span></div>
            <div class="icon-text-wrap"><div class="icon color4"></div><span class="text-style">4 (<500m)</span></div>

        </div>
    </div>


</body>

</html>
