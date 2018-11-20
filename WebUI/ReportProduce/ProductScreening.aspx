<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductScreening.aspx.cs"
    Inherits="ReportProduce_ProductScreening" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height:95%">
<head runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/ProductScreening.js?V=20171211114" type="text/javascript"></script>
    <link href="../css/ProductScreening.css?V=201602170842111" rel="stylesheet" type="text/css" />
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body style="height:100%">
    <form id="form1" style="height:100%" runat="server">
    <div class="totalContent" style="height:100%">
        <div class="outLine" id="outLine" style="height:100%">
            <div class="shArea">
                <div class="mapTitle">
                    <div class="titlePoint">
                    </div>
                    <span>上海预报预警产品</span>
                </div>
                <div class="displayItem" id="AQIPeriod48">
                    <div class="singleTitle">
                        AQI分时段预报</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition" id="AQIPeriod48_06">
                                待完成</div>
                            <div class="pubTime">
                                06时预报</div>
                        </div>
                        <div class="singleConDis">
                            <div class="condition" id="AQIPeriod48_17">
                                待完成</div>
                            <div class="pubTime">
                                17时预报</div>
                        </div>
                    </div>
                </div>
                <div class="displayItem" id="OzoneForecast">
                    <div class="singleTitle">
                        臭氧预报</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition">
                                待完成</div>
                            <div class="pubTime">
                                17时预报</div>
                        </div>
                    </div>
                </div>
                <div class="displayItem" id="AQIArea">
                    <div class="singleTitle">
                        AQI分区指导预报</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition">
                                待完成</div>
                            <div class="pubTime">
                                17时预报</div>
                        </div>
                    </div>
                </div>
                <div class="displayItem" id="AQIAreaForeFile">
                    <div class="singleTitle">
                        AQI分区预报文件</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition">
                                待完成</div>
                            <div class="pubTime">
                                17时预报</div>
                        </div>
                    </div>
                </div>
                <div class="displayItem" id="HazeForecast">
                    <div class="singleTitle">
                        霾预报</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition" id="HazeForecast_05">
                                待完成</div>
                            <div class="pubTime">
                                05时预报</div>
                        </div>
                        <div class="singleConDis">
                            <div class="condition" id="HazeForecast_17">
                                待完成</div>
                            <div class="pubTime">
                                17时预报</div>
                        </div>
                    </div>
                </div>
                <div class="displayItem" id="UVForecast">
                    <div class="singleTitle">
                        UV预报</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition" id="UVForecast_05">
                                待完成</div>
                            <div class="pubTime">
                                10时预报</div>
                        </div>
                        <div class="singleConDis">
                            <div class="condition" id="UVForecast_17">
                                待完成</div>
                            <div class="pubTime">
                                16时预报</div>
                        </div>
                    </div>
                </div>
                <div class="displayItem" id="AirPollutionForecast">
                    <div class="singleTitle">
                        空气污染气象条件</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition" id="AirPollutionForecast_07">
                                待完成</div>
                            <div class="pubTime">
                                08时预报</div>
                        </div>
                        <div class="singleConDis">
                            <div class="condition" id="AirPollutionForecast_17">
                                待完成</div>
                            <div class="pubTime">
                                20时预报</div>
                        </div>
                    </div>
                </div>
               <div class="displayItem" id="cityForecast">
                    <div class="singleTitle">
                        未来10天AQI滚动预报</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition">
                                待完成</div>
                            <div class="pubTime">
                                17时预报</div>
                        </div>
                    </div>
                </div>
                <div class="displayItem" id="evaluateReport" >
                    <div class="singleTitle">
                        预报登记</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition">
                                待完成</div>
                            <div class="pubTime">
                                17时预报</div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="eastChinaArea" id="AirPolFore">
                <div class="mapTitle">
                    <div class="titlePoint">
                    </div>
                    <span>华东区域预报预警产品</span>
                </div>
                <div class="displayItem" id="guideReportFile">
                    <div class="singleTitle">
                        指导预报文件</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition">
                                待完成</div>
                            <div class="pubTime">
                                10:30预报</div>
                        </div>
                    </div>
                </div>
                <div class="displayItem" id="AQIDropZone">
                    <div class="singleTitle">
                        AQI落区预报</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition">
                                待完成</div>
                            <div class="pubTime">
                                14时预报</div>
                        </div>
                    </div>
                </div>
                <div class="displayItem" id="HazeDropZone">
                    <div class="singleTitle">
                        霾落区预报</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition">
                                待完成</div>
                            <div class="pubTime">
                                14时预报</div>
                        </div>
                    </div>
                </div>
                <div class="displayItem" id="AirPollutionDropZone">
                    <div class="singleTitle">
                        空气污染气象条件落区预报</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition">
                                待完成</div>
                            <div class="pubTime">
                                14时预报</div>
                        </div>
                    </div>
                </div>
                <div class="displayItem" id="eastChinaMainCity" style="display: none;">
                    <div class="singleTitle">
                        华东区域重点城市预报</div>
                    <div class="innerContent">
                        <div class="singleConDis">
                            <div class="condition">
                                待完成</div>
                            <div class="pubTime">
                                14时预报</div>
                        </div>
                    </div>
                </div>
                
            </div>
        </div>
    </div>
    </form>
</body>
</html>
