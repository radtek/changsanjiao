<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AirPollutionForecast.aspx.cs" Inherits="ReportProduce_AirPollutionForecast" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <script src="../AQI/js/AirPollutionForecast.js?v=2016060811111111" type="text/javascript"></script>
    <link href="../css/AirPollutionForecast.css?v=2017081611111" rel="stylesheet" type="text/css" />
    <script src="../JS/jquery.nicescroll.min.js" type="text/javascript"></script>
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="tableTop">
            <div id="topInfo" class="titleContent">
                <table>
                    <tr>
                        <th class="attrName">预报员：</th>
                        <td class="attrValue" id="forecaster"></td>
                        <th class="attrName">预报时间：</th>
                        <td id="forecastTime" class="attrValue"></td>
                        <th class="attrName">预报时次：</th>
                        <td id="forecastTimeLevel" class="attrValue"></td>
                        <td class="btnTd">
                            <div id="savebutton" class="button_Bottom">保存</div>
                            <div id="authbutton" class="button_Bottom" style="display: none;">审核</div>
                            <div id="pulishbutton" class="button_Bottom" style="display: none;">发布</div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="outLine">
            <div class="totalContent">
                <div id="map" class="map">
                    <div class="mapTitle">
                        <span class="mapName">上海地区空气污染气象条件预报</span>
                    </div>
                    <div class="mapDate"><span>预报时间：</span><span id="mapDate"></span></div>
                    <div class="mapControl" id="mapControl">
                        <div class="districtArea" id="districtArea">
                            <div id="XuHui" pos="pos" >
                                <div id="HuHuiSym" class="symSize"></div>
                            </div>
                            <div id="PuDong"  pos="pos">
                                <div id="PuDongSym" class="symSize"></div>
                            </div>
                            <div id="MinHang" pos="pos">
                                <div id="MinHangSym" class="symSize"></div>
                            </div>
                            <div id="BaoShanArea" pos="pos">
                                <div id="BaoShanAreaSym" class="symSize"></div>
                            </div>
                            <div id="SongJiang" pos="pos">
                                <div id="SongJiangSym" class="symSize"></div>
                            </div>
                            <div id="JinShan" pos="pos">
                                <div id="JinShanSym" class="symSize"></div>
                            </div>
                            <div id="QingPu" pos="pos">
                                <div id="QingPuSym" class="symSize"></div>
                            </div>
                            <div id="FengXian" pos="pos">
                                <div id="FengXianSym" class="symSize"></div>
                            </div>
                            <div id="JiaDing" pos="pos">
                                <div id="JiaDingSym" class="symSize"></div>
                            </div>
                            <div id="ChongMing" pos="pos">
                                <div id="ChongMingSym" class="symSize"></div>
                            </div>
                            <div class="mapMask"></div>
                            <div class="legend">
                                <%--<div class="legendSym" style="margin-left: 130px">
                                <div style="background:url(../css/images/01.png) no-repeat;height:35px;"></div>--%>
                            </div>
                            </div>
                            
                            <div class="pubTime">上海中心气象台<span id="pubTime"></span><span id="pubHour">08</span>时发布</div>
                        </div>
                    </div>
                </div>
                <div class="content" id="rightContent">
                    <div class="tablePart">
                        <div class="editLabel">
                            <div class="titlePoint"></div>
                            <span class="partTitle">空气污染气象条件</span>
                        </div>

                        <div id="airPolTable" class="tableContent">
                            <table id="areaForecast" class="table2">
                                <tbody>
                                    <tr>
                                        <th scope="col">区域</th>
                                        <th scope="col">PM2.5日均指数</th>
                                        <th scope="col">空气质量等级</th>
                                        <th class="style3" scope="col">污染气象条件等级</th>
                                        <th class="style3" scope="col">气象符号等级</th>
                                    </tr>
                                    <tr class="trforecast" style="display: table-row;">

                                        <td class="c_RegionName">中心城区</td>
                                        <td class="aqiValueTd">
                                            <input id="58367_Value" class="PM25" type="text" value="" />
                                        </td>
                                        <td class="aqiLevelTd">

                                            <div class="airQuaSelect">
                                                <div id="selectID" class="aiqQuaDiv">
                                                    <div class="airQuaText" id="58367_airQua">一级</div>
                                                    <div id="selIcon" class="selIcon"></div>
                                                </div>
                                                <ul id="airQuaUl" class="airQuaUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="aqiItemTd">
                                            <div class="airpolConSelect" id="Div28">
                                                <div id="Div29" class="airpolConDiv">
                                                    <div class="airpolConText" id="58367_aqiPolCon">一级</div>
                                                    <div id="Div30" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul10" class="airpolConUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="symbolTd">
                                            <div class="symbolConSelect">
                                                <div class="symbolConDiv">
                                                    <div class="symbolConText">无</div>
                                                    <div class="selIcon"></div>
                                                </div>
                                                <ul class="hide symbolConUl">
                                                    <li>
                                                        <div>无</div>
                                                    </li>
                                                    <li>
                                                        <div>浮尘</div>
                                                    </li>
                                                    <li>
                                                        <div>扬沙</div>
                                                    </li>
                                                    <li>
                                                        <div>沙尘暴</div>
                                                    </li>
                                                    <li>
                                                        <div>霾</div>
                                                    </li>
                                                    <li>
                                                        <div>光化学烟雾</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="trforecast" style="display: table-row;">
                                        <td class="c_RegionName">浦东新区</td>
                                        <td class="aqiValueTd">
                                            <input id="58370_Value" class="PM25" type="text" value="" />
                                        </td>
                                        <td class="aqiLevelTd">

                                            <div class="airQuaSelect" id="Div1">
                                                <div id="Div2" class="aiqQuaDiv">
                                                    <div class="airQuaText" id="58370_airQua">一级</div>
                                                    <div id="Div3" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul1" class="airQuaUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="aqiItemTd">
                                            <div class="airpolConSelect" id="Div31">
                                                <div id="Div32" class="airpolConDiv">
                                                    <div class="airpolConText" id="58370_aqiPolCon">一级</div>
                                                    <div id="Div33" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul11" class="airpolConUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="symbolTd">
                                            <div class="symbolConSelect">
                                                <div class="symbolConDiv">
                                                    <div class="symbolConText">无</div>
                                                    <div class="selIcon"></div>
                                                </div>
                                                <ul class="hide symbolConUl">
                                                    <li>
                                                        <div>无</div>
                                                    </li>
                                                    <li>
                                                        <div>浮尘</div>
                                                    </li>
                                                    <li>
                                                        <div>扬沙</div>
                                                    </li>
                                                    <li>
                                                        <div>沙尘暴</div>
                                                    </li>
                                                    <li>
                                                        <div>霾</div>
                                                    </li>
                                                    <li>
                                                        <div>光化学烟雾</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>

                                    </tr>
                                    <tr class="trforecast" style="display: table-row;">
                                        <td class="c_RegionName">闵行区</td>
                                        <td class="aqiValueTd">
                                            <input id="58361_Value" class="PM25" type="text" value="" />
                                        </td>
                                        <td class="aqiLevelTd">
                                            <div class="airQuaSelect" id="Div4">
                                                <div id="Div5" class="aiqQuaDiv">
                                                    <div class="airQuaText" id="58361_airQua">一级</div>
                                                    <div id="Div6" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul2" class="airQuaUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="aqiItemTd">
                                            <div class="airpolConSelect" id="Div34">
                                                <div id="Div35" class="airpolConDiv">
                                                    <div class="airpolConText" id="58361_aqiPolCon">一级</div>
                                                    <div id="Div36" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul12" class="airpolConUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                                                                <td class="symbolTd">
                                            <div class="symbolConSelect">
                                                <div class="symbolConDiv">
                                                    <div class="symbolConText">无</div>
                                                    <div class="selIcon"></div>
                                                </div>
                                                <ul class="hide symbolConUl">
                                                    <li>
                                                        <div>无</div>
                                                    </li>
                                                    <li>
                                                        <div>浮尘</div>
                                                    </li>
                                                    <li>
                                                        <div>扬沙</div>
                                                    </li>
                                                    <li>
                                                        <div>沙尘暴</div>
                                                    </li>
                                                    <li>
                                                        <div>霾</div>
                                                    </li>
                                                    <li>
                                                        <div>光化学烟雾</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>

                                    </tr>
                                    <tr class="trforecast" style="display: table-row;">

                                        <td class="c_RegionName">宝山区</td>
                                        <td class="aqiValueTd">
                                            <input id="58362_Value" class="PM25" type="text" value="" />
                                        </td>
                                        <td class="aqiLevelTd">
                                            <div class="airQuaSelect" id="Div7">
                                                <div id="Div8" class="aiqQuaDiv">
                                                    <div class="airQuaText" id="58362_airQua">一级</div>
                                                    <div id="Div9" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul3" class="airQuaUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="aqiItemTd">
                                            <div class="airpolConSelect" id="Div37">
                                                <div id="Div38" class="airpolConDiv">
                                                    <div class="airpolConText" id="58362_aqiPolCon">一级</div>
                                                    <div id="Div39" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul13" class="airpolConUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                                                                <td class="symbolTd">
                                            <div class="symbolConSelect">
                                                <div class="symbolConDiv">
                                                    <div class="symbolConText">无</div>
                                                    <div class="selIcon"></div>
                                                </div>
                                                <ul class="hide symbolConUl">
                                                    <li>
                                                        <div>无</div>
                                                    </li>
                                                    <li>
                                                        <div>浮尘</div>
                                                    </li>
                                                    <li>
                                                        <div>扬沙</div>
                                                    </li>
                                                    <li>
                                                        <div>沙尘暴</div>
                                                    </li>
                                                    <li>
                                                        <div>霾</div>
                                                    </li>
                                                    <li>
                                                        <div>光化学烟雾</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>

                                    </tr>
                                    <tr class="trforecast" style="display: table-row;">
                                        <td class="c_RegionName">松江区</td>
                                        <td class="aqiValueTd">
                                            <input id="58462_Value" class="PM25" type="text" value="" />
                                        </td>
                                        <td class="aqiLevelTd">
                                            <div class="airQuaSelect" id="Div10">
                                                <div id="Div11" class="aiqQuaDiv">
                                                    <div class="airQuaText" id="58462_airQua">一级</div>
                                                    <div id="Div12" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul4" class="airQuaUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="aqiItemTd">
                                            <div class="airpolConSelect" id="Div40">
                                                <div id="Div41" class="airpolConDiv">
                                                    <div class="airpolConText" id="58462_aqiPolCon">一级</div>
                                                    <div id="Div42" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul14" class="airpolConUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                                                                <td class="symbolTd">
                                            <div class="symbolConSelect">
                                                <div class="symbolConDiv">
                                                    <div class="symbolConText">无</div>
                                                    <div class="selIcon"></div>
                                                </div>
                                                <ul class="hide symbolConUl">
                                                    <li>
                                                        <div>无</div>
                                                    </li>
                                                    <li>
                                                        <div>浮尘</div>
                                                    </li>
                                                    <li>
                                                        <div>扬沙</div>
                                                    </li>
                                                    <li>
                                                        <div>沙尘暴</div>
                                                    </li>
                                                    <li>
                                                        <div>霾</div>
                                                    </li>
                                                    <li>
                                                        <div>光化学烟雾</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>

                                    </tr>
                                    <tr class="trforecast" style="display: table-row;">

                                        <td name="58460" class="c_RegionName">金山区</td>
                                        <td class="aqiValueTd">
                                            <input id="58460_Value" class="PM25" type="text" value="" />
                                        </td>
                                        <td class="aqiLevelTd">
                                            <div class="airQuaSelect" id="Div13">
                                                <div id="Div14" class="aiqQuaDiv">
                                                    <div class="airQuaText" id="58460_airQua">一级</div>
                                                    <div id="Div15" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul5" class="airQuaUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="aqiItemTd">
                                            <div class="airpolConSelect" id="Div43">
                                                <div id="Div44" class="airpolConDiv">
                                                    <div class="airpolConText" id="58460_aqiPolCon">一级</div>
                                                    <div id="Div45" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul15" class="airpolConUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                                                                <td class="symbolTd">
                                            <div class="symbolConSelect">
                                                <div class="symbolConDiv">
                                                    <div class="symbolConText">无</div>
                                                    <div class="selIcon"></div>
                                                </div>
                                                <ul class="hide symbolConUl">
                                                    <li>
                                                        <div>无</div>
                                                    </li>
                                                    <li>
                                                        <div>浮尘</div>
                                                    </li>
                                                    <li>
                                                        <div>扬沙</div>
                                                    </li>
                                                    <li>
                                                        <div>沙尘暴</div>
                                                    </li>
                                                    <li>
                                                        <div>霾</div>
                                                    </li>
                                                    <li>
                                                        <div>光化学烟雾</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>

                                    </tr>
                                    <tr class="trforecast" style="display: table-row;">

                                        <td name="58461" class="c_RegionName">青浦区</td>
                                        <td class="aqiValueTd">
                                            <input id="58461_Value" class="PM25" type="text" value="" />
                                        </td>
                                        <td class="aqiLevelTd">
                                            <div class="airQuaSelect" id="Div16">
                                                <div id="Div17" class="aiqQuaDiv">
                                                    <div class="airQuaText" id="58461_airQua">一级</div>
                                                    <div id="Div18" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul6" class="airQuaUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="aqiItemTd">
                                            <div class="airpolConSelect" id="Div46">
                                                <div id="Div47" class="airpolConDiv">
                                                    <div class="airpolConText" id="58461_aqiPolCon">一级</div>
                                                    <div id="Div48" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul16" class="airpolConUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                                                                <td class="symbolTd">
                                            <div class="symbolConSelect">
                                                <div class="symbolConDiv">
                                                    <div class="symbolConText">无</div>
                                                    <div class="selIcon"></div>
                                                </div>
                                                <ul class="hide symbolConUl">
                                                    <li>
                                                        <div>无</div>
                                                    </li>
                                                    <li>
                                                        <div>浮尘</div>
                                                    </li>
                                                    <li>
                                                        <div>扬沙</div>
                                                    </li>
                                                    <li>
                                                        <div>沙尘暴</div>
                                                    </li>
                                                    <li>
                                                        <div>霾</div>
                                                    </li>
                                                    <li>
                                                        <div>光化学烟雾</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>

                                    </tr>
                                    <tr class="trforecast" style="display: table-row;">

                                        <td name="58463" class="c_RegionName">奉贤区</td>
                                        <td class="aqiValueTd">
                                            <input id="58463_Value" class="PM25" type="text" value="" /></input>
                                        </td>
                                        <td class="aqiLevelTd">
                                            <div class="airQuaSelect" id="Div19">
                                                <div id="Div20" class="aiqQuaDiv">
                                                    <div class="airQuaText" id="58463_airQua">一级</div>
                                                    <div id="Div21" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul7" class="airQuaUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="aqiItemTd">
                                            <div class="airpolConSelect" id="Div49">
                                                <div id="Div50" class="airpolConDiv">
                                                    <div class="airpolConText" id="58463_aqiPolCon">一级</div>
                                                    <div id="Div51" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul17" class="airpolConUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                                                                <td class="symbolTd">
                                            <div class="symbolConSelect">
                                                <div class="symbolConDiv">
                                                    <div class="symbolConText">无</div>
                                                    <div class="selIcon"></div>
                                                </div>
                                                <ul class="hide symbolConUl">
                                                    <li>
                                                        <div>无</div>
                                                    </li>
                                                    <li>
                                                        <div>浮尘</div>
                                                    </li>
                                                    <li>
                                                        <div>扬沙</div>
                                                    </li>
                                                    <li>
                                                        <div>沙尘暴</div>
                                                    </li>
                                                    <li>
                                                        <div>霾</div>
                                                    </li>
                                                    <li>
                                                        <div>光化学烟雾</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>

                                    </tr>
                                    <tr class="trforecast" style="display: table-row;">

                                        <td name="58365" class="c_RegionName">嘉定区</td>
                                        <td class="aqiValueTd">
                                            <input id="58365_Value" class="PM25" type="text" value="" />
                                        </td>
                                        <td class="aqiLevelTd">
                                            <div class="airQuaSelect" id="Div22">
                                                <div id="Div23" class="aiqQuaDiv">
                                                    <div class="airQuaText" id="58365_airQua">一级</div>
                                                    <div id="Div24" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul8" class="airQuaUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="aqiItemTd">
                                            <div class="airpolConSelect" id="Div52">
                                                <div id="Div53" class="airpolConDiv">
                                                    <div class="airpolConText" id="58365_aqiPolCon">一级</div>
                                                    <div id="Div54" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul18" class="airpolConUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                           <td class="symbolTd">
                                            <div class="symbolConSelect">
                                                <div class="symbolConDiv">
                                                    <div class="symbolConText">无</div>
                                                    <div class="selIcon"></div>
                                                </div>
                                                <ul class="hide symbolConUl">
                                                    <li>
                                                        <div>无</div>
                                                    </li>
                                                    <li>
                                                        <div>浮尘</div>
                                                    </li>
                                                    <li>
                                                        <div>扬沙</div>
                                                    </li>
                                                    <li>
                                                        <div>沙尘暴</div>
                                                    </li>
                                                    <li>
                                                        <div>霾</div>
                                                    </li>
                                                    <li>
                                                        <div>光化学烟雾</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>

                                    </tr>
                                    <tr class="trforecast" style="display: table-row;">

                                        <td name="58366" class="c_RegionName">崇明区</td>
                                        <td class="aqiValueTd">
                                            <input id="58366_Value" class="PM25" type="text" value="" />
                                        </td>
                                        <td class="aqiLevelTd">
                                            <div class="airQuaSelect" id="Div25">
                                                <div id="Div26" class="aiqQuaDiv">
                                                    <div class="airQuaText" id="58366_airQua">一级</div>
                                                    <div id="Div27" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul9" class="airQuaUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="aqiItemTd">
                                            <div class="airpolConSelect" id="Div55">
                                                <div id="Div56" class="airpolConDiv">
                                                    <div class="airpolConText" id="58366_aqiPolCon">一级</div>
                                                    <div id="Div57" class="selIcon"></div>
                                                </div>
                                                <ul id="Ul19" class="airpolConUl hide">
                                                    <li>
                                                        <div>一级</div>
                                                    </li>
                                                    <li>
                                                        <div>二级</div>
                                                    </li>
                                                    <li>
                                                        <div>三级</div>
                                                    </li>
                                                    <li>
                                                        <div>四级</div>
                                                    </li>
                                                    <li>
                                                        <div>五级</div>
                                                    </li>
                                                    <li>
                                                        <div>六级</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                        <td class="symbolTd">
                                            <div class="symbolConSelect">
                                                <div class="symbolConDiv">
                                                    <div class="symbolConText">无</div>
                                                    <div class="selIcon"></div>
                                                </div>
                                                <ul class="hide symbolConUl">
                                                    <li>
                                                        <div>无</div>
                                                    </li>
                                                    <li>
                                                        <div>浮尘</div>
                                                    </li>
                                                    <li>
                                                        <div>扬沙</div>
                                                    </li>
                                                    <li>
                                                        <div>沙尘暴</div>
                                                    </li>
                                                    <li>
                                                        <div>霾</div>
                                                    </li>
                                                    <li>
                                                        <div>光化学烟雾</div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="editBtns">
                            <div id="btnCopyCenter" class="button">复制中心城区</div>
                            <div id="btnLastOne" class="button">历史记录</div>
                            <div id="btnAutoGet" class="button">自动获取</div>
                        </div>
                    </div>

                    <div class="textPart" id="textPart">
                        <div class="editLabel">
                            <div class="titlePoint"></div>
                            <span class="partTitle">空气质量预报图</span>
                        </div>
                        <div id="aqiText" class="textContent">
                            <textarea id="textContentArea" class="textPrev"></textarea>
                        </div>
                    </div>
                </div>
                <div style="clear: both"></div>
                <div>
                </div>
            </div>
        </div>

        <textarea name="txtHide_AreaForecast" id="txtHide_AreaForecast" rows="2" cols="20" style="display: none; width: 801px; height: 42px;">
{ForecastDate}上海市空气污染气象条件分区预报
(上海市城市环境气象中心{PubDate}{Hour}发布)
{RegionName_Level}
</textarea>
        <textarea name="txtHideDetailAreaForecast" id="txtHideDetailAreaForecast" rows="2" cols="20" style="display: none;">[ {"RAPID":"13881","CPID":"3479","Station":"58367","StationName":"中心城区","PM25":"141","AirQtyLevel":"二级","APLevel":"二级","_ForcastDate":"0001/1/1 0:00:00","_Step":"0"},{"RAPID":"13882","CPID":"3479","Station":"58370","StationName":"浦东新区","PM25":"141","AirQtyLevel":"二级","APLevel":"二级","_ForcastDate":"0001/1/1 0:00:00","_Step":"0"},{"RAPID":"13883","CPID":"3479","Station":"58361","StationName":"闵行区","PM25":"141","AirQtyLevel":"二级","APLevel":"二级","_ForcastDate":"0001/1/1 0:00:00","_Step":"0"},{"RAPID":"13884","CPID":"3479","Station":"58362","StationName":"宝山区","PM25":"141","AirQtyLevel":"二级","APLevel":"二级","_ForcastDate":"0001/1/1 0:00:00","_Step":"0"},{"RAPID":"13885","CPID":"3479","Station":"58462","StationName":"松江区","PM25":"141","AirQtyLevel":"二级","APLevel":"二级","_ForcastDate":"0001/1/1 0:00:00","_Step":"0"},{"RAPID":"13886","CPID":"3479","Station":"58460","StationName":"金山区","PM25":"141","AirQtyLevel":"二级","APLevel":"二级","_ForcastDate":"0001/1/1 0:00:00","_Step":"0"},{"RAPID":"13887","CPID":"3479","Station":"58461","StationName":"青浦区","PM25":"141","AirQtyLevel":"二级","APLevel":"二级","_ForcastDate":"0001/1/1 0:00:00","_Step":"0"},{"RAPID":"13888","CPID":"3479","Station":"58463","StationName":"奉贤区","PM25":"141","AirQtyLevel":"二级","APLevel":"二级","_ForcastDate":"0001/1/1 0:00:00","_Step":"0"},{"RAPID":"13889","CPID":"3479","Station":"58365","StationName":"嘉定区","PM25":"141","AirQtyLevel":"二级","APLevel":"二级","_ForcastDate":"0001/1/1 0:00:00","_Step":"0"},{"RAPID":"13890","CPID":"3479","Station":"58366","StationName":"崇明区","PM25":"141","AirQtyLevel":"二级","APLevel":"二级","_ForcastDate":"0001/1/1 0:00:00","_Step":"0"}]</textarea>
        <!--FTP地址集合 -->
        <input type="hidden" id="FtpCollection" value="InfoCenterFtp,YYYYMMDDHH_YyYyMmDdHh_diffusion_sh_mmdd.GIF;InfoCenterFtpIII,YYYYMMDDHH_YyYyMmDdHh_diffusion_sh_mmdd.GIF;port21,YYYYMMDDHH_YyYyMmDdHh_diffusion_sh_mmdd.GIF;AirPollutionForecast2,YYYYMMDDHH_YyYyMmDdHh_diffusion_sh_mmdd.GIF;zxt,WRTJ_YYYYMMDDHH.txt;InfoCenterFtp,WRTJ_YYYYMMDDHH.txt;InfoCenterFtpIII,WRTJ_YYYYMMDDHH.txt;62WebSite,scuem_WRTJ_YYYYMMDDHHmm.txt" />
    </form>
</body>
</html>
