<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkAreaHomePage.aspx.cs" Inherits="ReportProduce_WorkAreaHomePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    <link href="../css/WorkAreaHomePage.css" rel="stylesheet" type="text/css" />
    <link href="../css/WorkAreaHome_WarningInfo.css" rel="stylesheet" type="text/css" />
    <script src="../AQI/js/WorkAreaHomePage.js" type="text/javascript"></script>
<link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="table1" style="margin: 5px 2px 0 2px">
        <%--<div class="tabs" style="width: 99%;">
       <ul>
       <li class="li1 limouseon" id="li1" onclick="showdiv('DIV1')" onmouseover="showdiv('DIV1')">交班日志提醒</li>
       <li id="li4" onclick="showdiv('DIV5')" onmouseover="showdiv('DIV5')">国家级指导预报产品</li>
       <li id="li3" onclick="showdiv('DIV3')" onmouseover="showdiv('DIV3')" "="">最新时次产品</li>
       <li id="li8" onclick="showdiv('DIV12')" onmouseover="showdiv('DIV12')">产品监控</li>
       <li id="li5" onclick="showdiv('DIV4')" onmouseover="showdiv('DIV4')">预报质量登记</li>
       <li id="li6" onclick="showdiv('DIV10')" onmouseover="showdiv('DIV10')">交接班日志</li>
       </ul>
    </div>--%>
        <div class="tabs_middle1" style="display: block; margin-top: 2px;" align="center"
            id="DIV1">
            <table border="0" class="outerTable" width="100%">
                <tbody>
                    <tr>
                        <td class="td_left" width="55%">
                            <table border="0" class="leftTable" width="99%">
                                <tbody>
                                    <tr style="height: 20">
                                        <td style="width: 25%">
                                            <div class="tabs_middle1_left_new_pro">
                                                <div class="contenttitle">
                                                    <div class="ct_left">
                                                    </div>
                                                    <div class="mapTitle">
                                                        <div class="titlePoint">
                                                        </div>
                                                        <span>值班信息</span>
                                                    </div>
                                                    <div class="ct_right">
                                                    </div>
                                                    <div class="contenttitle_Top">
                                                        <table class="table_td" id="tb_WorkScheduleInfo">
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                        <!--值班信息-->
                                    </tr>
                                    <tr>
                                        <td style="width: 35%; display: none;">
                                            <div class="tabs_middle1_left_new_pro">
                                                <div class="contenttitle">
                                                    <div class="ct_left">
                                                    </div>
                                                    <div class="ct_mid">
                                                        会商安排</div>
                                                    <div class="ct_right">
                                                    </div>
                                                    <div class="contenttitle2">
                                                        <table class="table_td">
                                                            <tbody>
                                                                <tr>
                                                                    <th>
                                                                        首席预报员&nbsp;：
                                                                    </th>
                                                                    <td>
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <th>
                                                                        会商时间&nbsp;&nbsp;&nbsp;：
                                                                    </th>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <th>
                                                                        会商重点&nbsp;&nbsp;&nbsp;：
                                                                    </th>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <br />
                                                        <table align="center" border="1" cellpadding="0" cellspacing="0" style="width: 99%;
                                                            text-align: center; border-color: #bfcde1">
                                                            <tbody>
                                                                <tr>
                                                                    <th style="width: 15%; height: 20px; background-color: #FCF9CE; color: #1979bf;">
                                                                        发言岗位
                                                                    </th>
                                                                    <th style="width: 20%; height: 20px; background-color: #FCF9CE; color: #1979bf;">
                                                                        时间
                                                                    </th>
                                                                    <th style="width: 65%; height: 20px; background-color: #FCF9CE; color: #1979bf;">
                                                                        重点
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <td style="background-color: #FCF9CE; color: #1979bf; height: 20px;">
                                                                        G2
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="background-color: #FCF9CE; color: #1979bf; height: 20px;">
                                                                        G3
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="background-color: #FCF9CE; color: #1979bf; height: 20px;">
                                                                        G4
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <!-- <h4 style="text-align:left">上海会商</h4>
       <p style="text-align:left">暂无会商</p>
       
      <br />
      <h4 style="text-align:left">华东区域</h4>
       <p style="text-align:left">暂无会商</p>-->
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                        <!--会商安排-->
                                    </tr>
                                    <tr style="height: 10">
                                        <td style="width: 40%">
                                            <div class="tabs_middle1_left_new_pro" style="width: 100%; margin-top: 2px;">
                                                <div class="contenttitle">
                                                    <div class="ct_left">
                                                    </div>
                                                    <div class="mapTitle">
                                                        <div class="titlePoint">
                                                        </div>
                                                        <span>预警信号</span>
                                                    </div>
                                                    <div class="ct_right">
                                                    </div>
                                                    <div class="contenttitle_Top" id="div_AlarmProduct">
                                                        暂无最新预警信号......</div>
                                                </div>
                                            </div>
                                        </td>
                                        <!--预警信号-->
                                    </tr>
                                    <tr>
                                        <td style="width: 100%">
                                            <div class="tabs_middle">
                                                <!-- margin:-2px 8px 10px -2px;-->
                                                <div class="contenttitle">
                                                    <div class="ct_left">
                                                        <div class="mapTitle">
                                                            <div class="titlePoint">
                                                            </div>
                                                            <span>重要通知</span>
                                                        </div>
                                                        <div class="ct_border">
                                                            <div id="Logcontent" class="contenttitle_Bottom">
                                                                <ul style="margin-top: 0px;">
                                                                    <li style="display: list-item;">
                                                                        <p>
                                                                            <span class="span_worklog">[城环中心通知]</span>&nbsp;<span class="span_time">2015-09-17 13:59</span><br/>
                                                                            6-9月广播稿不放火险，细菌食物每年4-10月制作，6-8月指数短信有中暑，其他月份为干燥</p>
                                                                    </li>
                                                                    <li style="display: list-item;">
                                                                        <p>
                                                                            <span class="span_worklog">[值班日志]</span>&nbsp;<span class="span_time">2015-09-27 09:52</span>&nbsp;<span
                                                                                class="span_time">甄新蓉</span><br/>
                                                                            10月10日~18日每天上午9时前提供未来三天空气质量指数、紫外线、人体舒适度预报。</p>
                                                                    </li>
                                                                
                                                            </div>
                                                        </div>
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                        <!--重要通知-->
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td class="td_right" width="45%">
                            <table border="0" class="bottomTable" width="99%">
                                <tbody>
                                    <tr>
                                        <td>
                                            <div class="tabs_middle">
                                                <div class="contenttitle">
                                                    <div class="ct_left">
                                                    </div>
                                                    <div class="mapTitle" style="margin-top: 5px">
                                                        <div class="titlePoint">
                                                        </div>
                                                        <span>常用联系方式</span>
                                                    </div>
                                                    <div class="ct_right">
                                                    </div>
                                                    <div id="Contact" class="contenttitle_Right">
                                                        <div class="contact_top">
                                                            <p class="secondTitle" style="margin-top: 10px">
                                                                环境平台故障：</p>
                                                            <p class="formalText">
                                                                <span class="name">小吴：</span>18616839388&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="name">QQ：</span>531749300</p>
                                                            <p class="secondTitle">
                                                                地听公司（平台支撑）联系方式：</p>
                                                            <p class="formalText">
                                                                <span class="name">张小意</span>13122012309&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="name">张伟锋:</span>18149772683</p>
                                                            <p class="secondTitle">
                                                                微信平台联系方式：</p>
                                                            <p class="formalText">
                                                                <span class="name">李毅杰:</span>18616591019</p>
                                                            <p class="secondTitle">
                                                                发生重大紧急情况报告：</p>
                                                            <p class="formalText">
                                                                <span class="name">中国气象局值班室：</span> 010-68406351&nbsp; 010-68407351&nbsp; <span class="name">
                                                                    fax：</span>010-62174239</p>
                                                            <p class="formalText">
                                                                <span class="name">市局总值班室 ：</span> 64874620&nbsp; 54896406&nbsp;&nbsp; &nbsp; &nbsp; &nbsp;  <span class="name">
                                                                    传真：</span>64875023</p>
                                                            <p class="formalText">
                                                                <span class="name">首席服务官：</span> 54896375 &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <span class="name">业务处：</span>64391766&nbsp;
                                                                54896338</p>
                                                            <p class="formalText">
                                                                <span class="name">首席服务管助理：</span>54896180&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <span class="name">首席预报员：</span>54896357</p>
                                                            <p class="formalText">
                                                                <span class="name">预警报告：</span>中心台54896273、64386380&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;<span class="name">FAX：</span>64383221</p>
                                                            <p class="formalText">
                                                                <span class="name">可视会商：</span>64390631</p>
                                                            <p class="formalText">
                                                                <span class="name">细菌性食物中毒预警信息联系电话：</span>郑雷军副科长：13601626384</p>
                                                            <p class="tips" style="text-align: right">
                                                                （其它联系人见工作手册相关信息）</p>
                                                            <p class="secondTitle">
                                                                服务器出问题时联系人：</p>
                                                            <p class="formalText">
                                                                <span class="name">周坤：</span>18918206799</p>
                                                            <p class="secondTitle">
                                                                业务常用</p>
                                                            <p class="formalText">
                                                                <span class="name">霾会商：</span>54896354</p>
                                                            <p class="secondTitle">
                                                                手机短信故障：</p>
                                                            <p class="formalText">
                                                                <span class="name">短信值班电话:</span>&nbsp;54896685&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="name">信息集团小李:</span>&nbsp;54896686</p>
                                                            <p class="formalText">
                                                                <span class="name">宿舍:</span>&nbsp;54896221&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="name">陈浩科长:</span>&nbsp;18918206056</p>
                                                            <p class="tips">
                                                                注：因非主观原因造成环境预报不能上传或迟传国家局，须通过Notes进行说明。</p>
                                                        </div>
                                                        <div class="contact_mid">
                                                            <div class="contact_mid_rec">
                                                                <br />
                                                                <p>
                                                                    <span class="greenText">地址：</span>&nbsp;<a href="mailto:地址：张建春/业务处/nmc@nmc">张建春/业务处/nmc@nmc</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <span class="greenText">电话：</span>&nbsp;010—68406424</p>
                                                                <p>
                                                                    <span class="greenText">办公室地点：</span>&nbsp;国家气象中心905室&nbsp;&nbsp;&nbsp; <span class="greenText">
                                                                        通信地址：</span>&nbsp;国家气象中心业务科技处&nbsp;</p>
                                                                <p>
                                                                    <span class="greenText">潘亚茹电话：</span>&nbsp;010—68408532&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <span class="greenText">指数soweather网页更改通知：</span>&nbsp;54896172</p>
                                                                <p align="left">
                                                                    <span class="greenText">欧洲中心集合预报：</span></p>
                                                                <p>
                                                                    &nbsp;&nbsp;<a href="http://www.ecmwf.int/products/forecasts/d/charts/medium/epsgramswmo/"
                                                                        target="_blank">http://www.ecmwf.int/products/forecasts/d/charts/medium/epsgramswmo/</a></p>
                                                                <p align="left">
                                                                    <span class="greenText">Name:</span>&nbsp;321eLnin&nbsp;&nbsp;&nbsp;&nbsp;<span class="greenText">Password:</span>&nbsp;x3125936</p>
                                                                <p align="left">
                                                                    &nbsp;&nbsp; 江旭东 18918206396 54896396</p>
                                                                <br />
                                                            </div>
                                                        </div>
                                                        <div class="conatact_bottom">
                                                        <p class="secondTitle">监测中心预报员联系方式：</p>
                                                            <table class="contact_table" border="0" cellpadding ="1px" cellspacing="1">
                                                                <tbody bgcolor="#ebebeb">
                                                                    <tr class="firstline" >
                                                                        <td style="width: 140px; height: 33px;"><strong>
                                                                            姓名</strong>
                                                                        </td>
                                                                        <td style="width: 188px; height: 33px;"><strong>
                                                                            手机</strong>
                                                                        </td>
                                                                        <td style="width: 216px; height: 33px;"><strong>
                                                                            座机</strong>
                                                                        </td></tr>
                                                                        <tr style="height: 33px;">
                                                                        <td><span style="font-size: 16px;">林陈渊</span></td>
                                                                        <td><span style="font-size: 16px;">137-6493-4248</span></td>
                                                                        <td><span style="font-size: 16px;">2401-1583</span></td>
                                                                        </tr>
                                                                        <tr style="height: 33px;">
                                                                        <td>
					<span style="font-size: 16px;">王&nbsp;茜</span></td>
                                                                        <td>
					<span style="font-size: 16px;">137-0166-0901</span></td>
                                                                        <td>
					<span style="font-size: 16px;">2401-1920</span></td>
                                                                        </tr>
                                                                        <tr style="height: 33px;">
                                                                        <td>
					<span style="font-size: 16px;">王晓浩</span></td>
                                                                        <td>
					<span style="font-size: 16px;">135-6463-7345</span></td>
                                                                        <td>
					<span style="font-size: 16px;">2401-1917</span></td>
                                                                        </tr>
                                                                        <tr style="height: 33px;">
                                                                        <td>
					<span style="font-size: 16px;">胡&nbsp;鸣</span></td>
                                                                        <td>
					<span style="font-size: 16px;">156-1859-0512</span></td>
                                                                        <td>
					<span style="font-size: 16px;">2401-1927</span></td>
                                                                        </tr>
                                                                        <tr style="height: 33px;">
                                                                        <td>
					<span style="font-size: 16px;">霍俊涛</span></td>
                                                                        <td>
				<p align="center">
					<span style="font-size: 16px;">159-0081-0153</span></p>
			                                                                </td>
                                                                        <td>
					<span style="font-size: 16px;">2401-1916</span></td>
                                                                        </tr>
                                                                        <tr style="height: 33px;">
                                                                       <td>
				<p align="center">
					<span style="font-size: 16px;">赵倩彪</span></p>
			                                                                </td>
                                                                        <td>
				<p align="center">
					<span style="font-size: 16px;">189-1728-6820</span></p>
			                                                                </td>
                                                                        <td>
					<span style="font-size: 16px;">2401-1919</span></td>
                                                                        </tr>
                                                                        <tr style="height: 33px;">
                                                                        <td>
					<span style="font-size: 16px;">张懿华</span></td>
                                                                        <td>
				<p align="center">
					<span style="font-size: 16px;">150-0040-1142</span></p>
			                                                                </td>
                                                                        <td>
					<span style="font-size: 16px;">2401-1922</span></td>
                                                                        </tr>
                                                                        <tr style="height: 33px;">
                                                                        <td>
					<span style="font-size: 16px;">林燕芬</span></td>
                                                                        <td>
				<p align="center">
					<span style="font-size: 16px;">135-2408-2909</span></p>
			                                                                </td>
                                                                        <td>
					<span style="font-size: 16px;">2401-1807</span></td>
                                                                        </tr>
                                                                       
                                                                </tbody>
                            </table>
        </div>
    </div>
    </div> </div> </td> </tr> </tbody> </table>
    <p>
        &nbsp;</p>
    </td></tr> </tbody> </table> </div> </div>
    <!--联系方式-->
    <!-- 最新时次产品 -->
    <div class="tabs_middle1" style="display: none; margin-top: 2px;" align="center"
        id="DIV3">
        <table border="0" width="99%">
            <tbody>
                <tr>
                    <td style="width: 33%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: -2px; margin-right: 0px;
                            margin-top: 2px; width: 99.5%">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../NewImages/NewIcon/aqi.png" alt="" width="20" height="20"></div>
                                    AQI</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; height: 230px; overflow: auto;
                                    background-color: #f4f9ff">
                                    <div class="title2" id="div_AQITxt">
                                        <h1>
                                            上海市空气质量预报</h1>
                                        <h2>
                                            (2016年2月28日 17时发布）</h2>
                                        <table align="center" border="0" cellspacing="1" bgcolor="#cdd7e3" width="98%">
                                            <tbody>
                                                <tr>
                                                    <td align="center" bgcolor="#FFFFFF">
                                                        时段
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        AQI
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        空气质量等级
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        首要污染物
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" bgcolor="#FFFFFF">
                                                        今天夜间（20时—06时）
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        75—95
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        良
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        PM2.5
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" bgcolor="#FFFFFF">
                                                        明天上午（06时—12时）
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        60—80
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        良
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        PM2.5
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" bgcolor="#FFFFFF">
                                                        明天下午（12时—20时）
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        55—75
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        良
                                                    </td>
                                                    <td bgcolor="#FFFFFF">
                                                        PM10
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="group">
                                            <ul>
                                                <li></li>
                                                <li>上海中心气象台</li>
                                                <li>上海市环境监测中心</li>
                                                <li>联合发布</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="width: 33%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: -1px; margin-right: 0px;
                            margin-top: 2px; width: 99.5%">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../NewImages/NewIcon/haze.png" alt="" width="20" height="20"></div>
                                    霾</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; height: 230px; overflow: auto;
                                    background-color: #f4f9ff">
                                    <div class="title2" id="div_HazeProduct">
                                        <h1>
                                            上海市霾的预报</h1>
                                        <h2>
                                            (2016年02月28日 17时发布)</h2>
                                        <table width="90%" border="0" cellspacing="1" bgcolor="#cdd7e1" class="weater" align="center"
                                            style="width: 95%; text-align: center; margin-left: 1px; margin-right: 1px;">
                                            <tbody>
                                                <tr>
                                                    <td height="24" align="center" bgcolor="#FFFFFF">
                                                        2016年02月29日
                                                    </td>
                                                    <td height="24" align="center" bgcolor="#FFFFFF">
                                                        无霾
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="24" align="center" bgcolor="#FFFFFF">
                                                        2016年03月01日
                                                    </td>
                                                    <td height="24" align="center" bgcolor="#FFFFFF">
                                                        无霾
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="24" align="center" bgcolor="#FFFFFF">
                                                        2016年03月02日
                                                    </td>
                                                    <td height="24" align="center" bgcolor="#FFFFFF">
                                                        无霾
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div class="group">
                                            <ul>
                                                <li>上海市城市环境气象中心</li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="width: 33%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: 1px; margin-right: 0px;
                            margin-top: 2px; width: 99.5%">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../images/img_03.png" alt="" width="20" height="20"></div>
                                    空气污染气象条件分区预报</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; height: 230px; overflow: auto;
                                    background-color: #f4f9ff">
                                    <div class="title2" id="div_AirTxt">
                                        <h1>
                                            2016年02月29日上海市空气污染气象条件分区预报</h1>
                                        <h2>
                                            (上海市城市环境气象中心2016年02月28日20时发布)</h2>
                                        <table width="95%" align="center" border="0" cellspacing="1" bgcolor="#cdd7e1">
                                            <tbody>
                                                <tr>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        中心城区
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        二级
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        浦东新区
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        二级
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        闵行区
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        二级
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        宝山区
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        二级
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        松江区
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        二级
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        金山区
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        二级
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        青浦区
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        二级
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        奉贤区
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        二级
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        嘉定区
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        二级
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        崇明县
                                                    </td>
                                                    <td height="24" bgcolor="#FFFFFF">
                                                        二级
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <table border="0" width="99%">
            <tbody>
                <tr>
                    <td style="width: 33%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: -2px; margin-right: 0px;
                            margin-top: -5px; width: 99.5%">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../images/img_03.png" alt="" width="20" height="20"></div>
                                    霾落区图</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; background-color: #f4f9ff">
                                    <a href="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/Haze/2016022720_haze_0229.GIF"
                                        class="tooltip-keleyi-com" id="a_AQILQPic_Haze">
                                        <img src="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/Haze/2016022720_haze_0229.GIF"
                                            width="99%" height="400" style="border-color: White;" alt="" id="img_AQILQPic_Haze"></a>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="width: 33%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: -1px; margin-right: 0px;
                            margin-top: -5px; width: 99.5%">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../images/img_03.png" alt="" width="20" height="20"></div>
                                    空气污染气象条件落区图</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; background-color: #f4f9ff">
                                    <a href="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/WRTJ/2016022720_diffusion_0229.GIF"
                                        class="tooltip-keleyi-com" id="a_AQILQPic_WRTJ">
                                        <img src="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/WRTJ/2016022720_diffusion_0229.GIF"
                                            width="99%" height="400" style="border-color: White;" alt="" id="img_AQILQPic_WRTJ"></a>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="width: 33%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: 1px; margin-right: 0px;
                            margin-top: -5px; width: 99.5%">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../images/img_03.png" alt="" width="20" height="20"></div>
                                    PM2.5落区图</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; background-color: #f4f9ff">
                                    <a href="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/AQI/2016022720_pm25_0229.GIF"
                                        class="tooltip-keleyi-com" id="a_AQILQPic_PM25">
                                        <img src="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/AQI/2016022720_pm25_0229.GIF"
                                            width="99%" height="400" style="border-color: White;" alt="" id="img_AQILQPic_PM25"></a>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <table border="0" width="99%">
            <tbody>
                <tr>
                    <td style="width: 33%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: -2px; margin-right: 0px;
                            margin-top: -5px; width: 99.5%">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../images/img_03.png" alt="" width="20" height="20"></div>
                                    PM<sub>10</sub>落区图</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; background-color: #f4f9ff">
                                    <a href="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/AQI/2016022720_pm10_0229.GIF"
                                        class="tooltip-keleyi-com" id="a_AQILQPic_PM10">
                                        <img src="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/AQI/2016022720_pm10_0229.GIF"
                                            width="99%" height="400" style="border-color: White;" alt="" id="img_AQILQPic_PM10"></a>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="width: 33%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: -1px; margin-right: 0px;
                            margin-top: -5px; width: 99.5%">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../images/img_03.png" alt="" width="20" height="20"></div>
                                    O<sub>3</sub>8小时落区图</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; background-color: #f4f9ff">
                                    <a href="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/AQI/2016022720_o3_8h_0229.GIF"
                                        class="tooltip-keleyi-com" id="a_AQILQPic_O38h">
                                        <img src="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/AQI/2016022720_o3_8h_0229.GIF"
                                            width="99%" height="400" style="border-color: White;" alt="" id="img_AQILQPic_O38h"></a>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="width: 33%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: 1px; margin-right: 0px;
                            margin-top: -5px; width: 99.5%">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../images/img_03.png" alt="" width="20" height="20"></div>
                                    NO<sub>2</sub>落区图</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; background-color: #f4f9ff">
                                    <a href="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/AQI/2016022720_no2_0229.GIF"
                                        class="tooltip-keleyi-com" id="a_AQILQPic_NO2">
                                        <img src="http://222.66.83.21:8087/CEWF/ProductFiles/ProductFiles_YB/LQPicture/AQI/2016022720_no2_0229.GIF"
                                            width="99%" height="400" style="border-color: White;" alt="" id="img_AQILQPic_NO2"></a>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <!--  国家级指导预报产品 -->
    <div class="tabs_middle1" style="display: none; margin-top: 2px;" align="center"
        id="DIV5">
        <table border="0" width="99%">
            <tbody>
                <tr>
                    <td style="width: 50%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: -2px; margin-right: 0px;
                            margin-top: 2px; width: 99.5%">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../NewImages/NewIcon/wu.png" alt="" width="20" height="20"></div>
                                    全国雾24小时预报</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; background-color: #f4f9ff"
                                    id="div6">
                                    <a href="http://image.weather.gov.cn/product/2014/201405/20140530/WTFC/medium/SEVP_NMC_WTFC_SFER_EFG_ACHN_L88_P9_20140530000002412.JPG"
                                        id="a1" class="tooltip-keleyi-com"></a>
                                    <img id="Image1" alt="" src="" style="height: 400px; width: 99%; border-width: 0px;
                                        border-color: White;">
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="width: 50%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: 1px; margin-right: 0px;
                            margin-top: 2px; width: 99.5%;">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../NewImages/NewIcon/haze.png" alt="" width="20" height="20"></div>
                                    全国霾24小时预报</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; background-color: #f4f9ff"
                                    id="div7">
                                    <a href="http://image.weather.gov.cn/product/2014/201406/20140603/WTFC/medium/SEVP_NMC_WTFC_SFER_EHZ_ACHN_L88_P9_20140603000002412.JPG"
                                        id="a2" class="tooltip-keleyi-com"></a>
                                    <img id="Image2" alt="" src="" style="height: 400px; width: 99%; border-width: 0px;
                                        border-color: White;">
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: -2px; margin-right: 0px;
                            margin-top: -5px; width: 99.5%;">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../NewImages/NewIcon/shach.png" alt="" width="20" height="20"></div>
                                    全国沙尘预报</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; background-color: #f4f9ff"
                                    id="div8">
                                    <img id="Image3" alt="" src="" style="height: 400px; width: 100%; border-width: 0px;
                                        border-color: White;">
                                    <a href="http://image.weather.gov.cn/product/2014/201406/20140603/APWF/medium/SEVP_NMC_APWF_SFER_EAIRP_ACHN_LNO_P9_20140603000002424.JPG"
                                        id="a3" class="tooltip-keleyi-com"></a>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="width: 50%">
                        <div class="tabs_middle1_left_new_pro" style="margin-left: 1px; margin-right: 0px;
                            margin-top: -5px; width: 99.5%;">
                            <div class="contenttitle">
                                <div class="ct_left">
                                </div>
                                <div class="ct_mid">
                                    <div class="ct_icon">
                                        <img src="../NewImages/NewIcon/wrtj.png" alt="" width="20" height="20"></div>
                                    全国空气污染扩散气象条件预报图</div>
                                <div class="ct_right">
                                </div>
                                <div class="contenttitle2" style="text-align: center; background-color: #f4f9ff"
                                    id="div9">
                                    <a href="http://image.weather.gov.cn/product/2014/201406/20140603/WTFC/medium/SEVP_NMC_WTFC_SFER_EHZ_ACHN_L88_P9_20140603000002412.JPG"
                                        id="a4" class="tooltip-keleyi-com"></a>
                                    <img id="Image4" alt="" src="" style="height: 400px; width: 99%; border-width: 0px;
                                        border-color: White;">
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <!--  预报质量登记 -->
    <div class="tabs_middle1" style="display: none; margin-top: -3px;" id="DIV4">
        <div class="tabs_middle1_left" style="width: 99%; margin-left: 2px; float: none;
            margin-right: 0px; padding-left: 2px;">
            <div class="contenttitle">
                <div class="ct_left">
                </div>
                <div class="ct_mid">
                    <div class="ct_icon">
                        <img src="../images/img_03.png" width="20" height="20" alt=""></div>
                    预报质量登记(当前登记人：<span id="lblRegUser">甄新蓉</span>)</div>
                <div class="ct_right">
                </div>
                <div class="contenttitle2" style="background-color: #f4f9ff">
                    <br>
                    <div class="box">
                        <div class="label">
                            <span>AQI</span></div>
                        <div class="oper" style="right: 10px;">
                        </div>
                        <div class="box_content" style="width: 100%; margin-top: 25px; display: none;">
                            <div>
                            </div>
                            <div class="tabs" style="width: 99%;">
                                <ul>
                                    <!--
              <li id="ui1" onclick="show('showli1')">06时</li>
              <li id="ui2" onclick="show('showli2')">11时</li>
              -->
                                    <li id="ui1">06时</li>
                                    <li id="ui2">11时</li>
                                    <li class="limouseon" id="ui3" onclick="show('showli3')">17时</li>
                                </ul>
                                <div id="divopbtn_aqi" style="text-align: right; display: none;">
                                    <span style="position: absolute; left: 320px;">单位：微克/立方米(CO为毫克/立方米)</span>
                                    <input type="button" value="自动生成" class="button" style="position: absolute; right: 210px;"
                                        onclick="javascript: ReadNormalAQIData();">
                                    <input type="button" value="保存" class="button" style="position: absolute; right: 100px;"
                                        onclick="javascript: SaveOrUpdateAQI();">
                                </div>
                            </div>
                            <div class="tabs_middle1" style="display: none;" id="showli1">
                                <table class="default_table" align="center">
                                    <tbody>
                                        <tr>
                                            <th colspan="2">
                                                时段
                                            </th>
                                            <th>
                                                PM2.5
                                            </th>
                                            <th>
                                                PM10
                                            </th>
                                            <th>
                                                O<sub>3</sub>一小时
                                            </th>
                                            <th>
                                                O<sub>3</sub>八小时
                                            </th>
                                            <th>
                                                NO<sub>2</sub>
                                            </th>
                                            <th>
                                                SO<sub>2</sub>
                                            </th>
                                            <th>
                                                CO
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="6">
                                                24小时
                                            </th>
                                            <th>
                                                日均值
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                夜间（20-06时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上半夜（20-00时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下半夜（00-06时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上午（06-12时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下午（12-20时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th rowspan="6">
                                                48小时
                                            </th>
                                            <th>
                                                日均值
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                夜间（20-06时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上半夜（20-00时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下半夜（00-06时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上午（06-12时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下午（12-20时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="tabs_middle1" style="display: none;" id="showli2">
                                <table class="default_table">
                                    <tbody>
                                        <tr>
                                            <th colspan="2">
                                                时段
                                            </th>
                                            <th>
                                                PM2.5
                                            </th>
                                            <th>
                                                PM10
                                            </th>
                                            <th>
                                                O<sub>3</sub>一小时
                                            </th>
                                            <th>
                                                O<sub>3</sub>八小时
                                            </th>
                                            <th>
                                                NO<sub>2</sub>
                                            </th>
                                            <th>
                                                SO<sub>2</sub>
                                            </th>
                                            <th>
                                                CO
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="6">
                                                24小时
                                            </th>
                                            <th>
                                                日均值
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                夜间（20-06时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上半夜（20-00时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下半夜（00-06时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上午（06-12时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下午（12-20时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th rowspan="6">
                                                48小时
                                            </th>
                                            <th>
                                                日均值
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                夜间（20-06时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上半夜（20-00时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下半夜（00-06时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上午（06-12时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下午（12-20时）
                                            </th>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                            <td class="AQIDJ">
                                                <input type="text" style="display: none;">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="tabs_middle1" style="display: block;" id="showli3">
                                <table class="default_table" cellpadding="0" cellspacing="0" align="center">
                                    <tbody>
                                        <tr>
                                            <th colspan="2">
                                                时段
                                            </th>
                                            <th>
                                                PM2.5
                                            </th>
                                            <th>
                                                PM10
                                            </th>
                                            <th>
                                                O<sub>3</sub>一小时
                                            </th>
                                            <th>
                                                O<sub>3</sub>八小时
                                            </th>
                                            <th>
                                                NO<sub>2</sub>
                                            </th>
                                            <th>
                                                SO<sub>2</sub>
                                            </th>
                                            <th>
                                                CO
                                            </th>
                                        </tr>
                                        <tr>
                                            <th rowspan="6">
                                                24小时
                                            </th>
                                            <th>
                                                日均值
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_6_PM25"
                                                    class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_6_PM10"
                                                    class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_6_O3"
                                                    class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_6_O38"
                                                    class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_6_NO2"
                                                    class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_6_SO2"
                                                    class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_6_CO"
                                                    class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                夜间（20-06时）
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_3_PM25"
                                                    class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_3_PM10"
                                                    class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_3_O3"
                                                    class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_3_O38"
                                                    class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_3_NO2"
                                                    class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_3_SO2"
                                                    class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H24_3_CO"
                                                    class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上半夜（20-00时）
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" style="text-align: center;" id="H17_H24_1_PM25" class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" style="text-align: center;" id="H17_H24_1_PM10" class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" style="text-align: center;" id="H17_H24_1_O3" class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" style="text-align: center;" id="H17_H24_1_O38" class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" style="text-align: center;" id="H17_H24_1_NO2" class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" style="text-align: center;" id="H17_H24_1_SO2" class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" style="text-align: center;" id="H17_H24_1_CO" class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下半夜（00-06时）
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" style="text-align: center;" id="H17_H24_2_PM25" class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" style="text-align: center;" id="H17_H24_2_PM10" class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" style="text-align: center;" id="H17_H24_2_O3" class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" style="text-align: center;" id="H17_H24_2_O38" class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" style="text-align: center;" id="H17_H24_2_NO2" class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" style="text-align: center;" id="H17_H24_2_SO2" class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" style="text-align: center;" id="H17_H24_2_CO" class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上午（06-12时）
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" style="text-align: center;" id="H17_H24_4_PM25" class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" style="text-align: center;" id="H17_H24_4_PM10" class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" style="text-align: center;" id="H17_H24_4_O3" class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" style="text-align: center;" id="H17_H24_4_O38" class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" style="text-align: center;" id="H17_H24_4_NO2" class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" style="text-align: center;" id="H17_H24_4_SO2" class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" style="text-align: center;" id="H17_H24_4_CO" class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下午（12-20时）
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" style="text-align: center;" id="H17_H24_5_PM25" class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" style="text-align: center;" id="H17_H24_5_PM10" class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" style="text-align: center;" id="H17_H24_5_O3" class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" style="text-align: center;" id="H17_H24_5_O38" class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" style="text-align: center;" id="H17_H24_5_NO2" class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" style="text-align: center;" id="H17_H24_5_SO2" class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" style="text-align: center;" id="H17_H24_5_CO" class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th rowspan="6">
                                                48小时
                                            </th>
                                            <th>
                                                日均值
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_6_PM25"
                                                    class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_6_PM10"
                                                    class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_6_O3"
                                                    class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_6_O38"
                                                    class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_6_NO2"
                                                    class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_6_SO2"
                                                    class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_6_CO"
                                                    class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                夜间（20-06时）
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_3_PM25"
                                                    class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_3_PM10"
                                                    class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_3_O3"
                                                    class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_3_O38"
                                                    class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_3_NO2"
                                                    class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_3_SO2"
                                                    class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" readonly="readonly" style="text-align: center;" id="H17_H48_3_CO"
                                                    class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上半夜（20-00时）
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" style="text-align: center;" id="H17_H48_1_PM25" class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" style="text-align: center;" id="H17_H48_1_PM10" class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" style="text-align: center;" id="H17_H48_1_O3" class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" style="text-align: center;" id="H17_H48_1_O38" class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" style="text-align: center;" id="H17_H48_1_NO2" class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" style="text-align: center;" id="H17_H48_1_SO2" class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" style="text-align: center;" id="H17_H48_1_CO" class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下半夜（00-06时）
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" style="text-align: center;" id="H17_H48_2_PM25" class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" style="text-align: center;" id="H17_H48_2_PM10" class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" style="text-align: center;" id="H17_H48_2_O3" class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" style="text-align: center;" id="H17_H48_2_O38" class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" style="text-align: center;" id="H17_H48_2_NO2" class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" style="text-align: center;" id="H17_H48_2_SO2" class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" style="text-align: center;" id="H17_H48_2_CO" class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                上午（06-12时）
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" style="text-align: center;" id="H17_H48_4_PM25" class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" style="text-align: center;" id="H17_H48_4_PM10" class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" style="text-align: center;" id="H17_H48_4_O3" class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" style="text-align: center;" id="H17_H48_4_O38" class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" style="text-align: center;" id="H17_H48_4_NO2" class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" style="text-align: center;" id="H17_H48_4_SO2" class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" style="text-align: center;" id="H17_H48_4_CO" class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                下午（12-20时）
                                            </th>
                                            <td class="AQIDJ c_pm25">
                                                <input type="text" style="text-align: center;" id="H17_H48_5_PM25" class="c_pm25 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_pm10">
                                                <input type="text" style="text-align: center;" id="H17_H48_5_PM10" class="c_pm10 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o3">
                                                <input type="text" style="text-align: center;" id="H17_H48_5_O3" class="c_o3 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_o38">
                                                <input type="text" style="text-align: center;" id="H17_H48_5_O38" class="c_o38 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_no2">
                                                <input type="text" style="text-align: center;" id="H17_H48_5_NO2" class="c_no2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_so2">
                                                <input type="text" style="text-align: center;" id="H17_H48_5_SO2" class="c_so2 c000 c200 imemodel">
                                            </td>
                                            <td class="AQIDJ c_co">
                                                <input type="text" style="text-align: center;" id="H17_H48_5_CO" class="c_co c000 c200 imemodel">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <br>
                    <div class="box">
                        <div class="label">
                            <span>霾预报</span>
                        </div>
                        <div class="oper" style="right: 10px;">
                        </div>
                        <div class="box_content" style="width: 100%; margin-top: 25px; display: none;">
                            <div class="tabs" style="width: 99%;">
                                <ul>
                                    <!--<li id="ul1" onclick="showui('showul1')">05时</li>-->
                                    <li id="ul1">05时</li>
                                    <li class="limouseon" id="ul2" onclick="showui('showul2')">17时</li>
                                </ul>
                                <div id="divopbtn_haze" style="text-align: right; display: none;">
                                    <input type="button" value="自动生成" class="button" style="position: absolute; right: 210px;"
                                        onclick="javascript: ReadNormalHazeData();">
                                    <input type="button" value="保存" class="button" style="position: absolute; right: 100px;"
                                        onclick="javascript: SaveHaze();">
                                </div>
                            </div>
                            <div class="tabs_middle1" style="display: none;" id="showul1">
                                <table class="default_table" align="center">
                                    <tbody>
                                        <tr>
                                            <th style="width: 11%;">
                                                2016年02月28日
                                            </th>
                                            <td style="width: 89%; text-align: left">
                                                <textarea id="TextArea2" cols="20" rows="2" style="width: 80%;"></textarea>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                2016年02月29日
                                            </th>
                                            <td style="text-align: left">
                                                <textarea id="TextArea3" cols="20" rows="2" style="width: 80%;"></textarea>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                2016年03月01日
                                            </th>
                                            <td style="text-align: left">
                                                <textarea id="TextArea4" cols="20" rows="2" style="width: 80%;"></textarea>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="tabs_middle1" style="display: block; height: 100px" id="showul2">
                                <table class="default_table" align="center">
                                    <tbody>
                                        <tr>
                                            <th style="width: 11%;">
                                                2016年02月29日
                                            </th>
                                            <td style="width: 15%;">
                                                <table border="0" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <input id="Checkbox01" type="checkbox" name="chkHaze" value="无霾" onclick="wmShow1();"
                                                                    class="WM" style="width: 20px;">
                                                            </td>
                                                            <td style="">
                                                                &nbsp;无霾
                                                            </td>
                                                            <td>
                                                                <input id="Checkbox02" type="checkbox" name="chkHaze" value="有霾" onclick="ymShow1();"
                                                                    class="YM" style="width: 20px;">
                                                            </td>
                                                            <td style="">
                                                                &nbsp;有霾
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <th style="width: 8%;">
                                                等级
                                            </th>
                                            <td class="YM01" style="width: 15%;">
                                                <select style="display: none; width: 90%;" id="HazeLevel1_17" class="fontColor">
                                                    <option value="1">轻微霾</option>
                                                    <option value="2">轻度霾</option>
                                                    <option value="3">中度霾</option>
                                                    <option value="4">重度霾</option>
                                                    <option value="5">严重霾</option>
                                                </select>
                                                <span id="H16_H24_HAZE_LEVEL"></span>
                                            </td>
                                            <th style="width: 8%;">
                                                描述
                                            </th>
                                            <td class="HAZEDJ">
                                                <textarea cols="15" rows="2" style="width: 98%; height: 30px;" id="HazeDesc1_17"
                                                    class="input fontColor"></textarea>
                                                <span id="H16_H24_HAZE_DESC"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="width: 11%;">
                                                2016年03月01日
                                            </th>
                                            <td style="width: 10%;">
                                                <table border="0" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <input id="Checkbox001" type="checkbox" name="chkHaze" value="无霾" onclick="wmShow2();"
                                                                    class="WM" style="width: 20px;">
                                                            </td>
                                                            <td>
                                                                &nbsp;无霾
                                                            </td>
                                                            <td>
                                                                <input id="Checkbox002" type="checkbox" name="chkHaze" value="有霾" onclick="ymShow2();"
                                                                    class="YM" style="width: 20px;">
                                                            </td>
                                                            <td>
                                                                &nbsp;有霾
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <th style="width: 8%;">
                                                等级
                                            </th>
                                            <td class="YM02" style="width: 15%;">
                                                <select style="display: none; width: 90%;" id="HazeLevel2_17" class="fontColor">
                                                    <option value="1">轻微霾</option>
                                                    <option value="2">轻度霾</option>
                                                    <option value="3">中度霾</option>
                                                    <option value="4">重度霾</option>
                                                    <option value="5">严重霾</option>
                                                </select>
                                                <span id="H16_H48_HAZE_LEVEL"></span>
                                            </td>
                                            <th style="width: 8%;">
                                                描述
                                            </th>
                                            <td class="HAZEDJ">
                                                <textarea cols="15" rows="2" style="width: 98%; height: 30px;" id="HazeDesc2_17"
                                                    class="input fontColor"></textarea>
                                                <span id="H16_H48_HAZE_DESC"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th style="width: 11%;">
                                                2016年03月02日
                                            </th>
                                            <td style="width: 10%;">
                                                <table border="0" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <input id="Checkbox0001" type="checkbox" name="chkHaze" value="无霾" onclick="wmShow3();"
                                                                    class="WM" style="width: 20px;">
                                                            </td>
                                                            <td>
                                                                &nbsp;无霾
                                                            </td>
                                                            <td>
                                                                <input id="Checkbox0002" type="checkbox" name="chkHaze" value="有霾" onclick="ymShow3();"
                                                                    class="YM" style="width: 20px;">
                                                            </td>
                                                            <td>
                                                                &nbsp;有霾
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <th style="width: 8%;">
                                                等级
                                            </th>
                                            <td class="YM03" style="width: 15%;">
                                                <select style="display: none; width: 90%;" id="HazeLevel3_17" class="fontColor">
                                                    <option value="1">轻微霾</option>
                                                    <option value="2">轻度霾</option>
                                                    <option value="3">中度霾</option>
                                                    <option value="4">重度霾</option>
                                                    <option value="5">严重霾</option>
                                                </select>
                                                <span id="H16_H72_HAZE_LEVEL"></span>
                                            </td>
                                            <th style="width: 8%;">
                                                描述
                                            </th>
                                            <td class="HAZEDJ">
                                                <textarea cols="15" rows="2" style="width: 98%; height: 30px;" id="HazeDesc3_17"
                                                    class="input fontColor"></textarea>
                                                <span id="H16_H72_HAZE_DESC"></span>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <br>
                    <div class="box">
                        <div class="label">
                            <span>紫外线预报</span><span id="uvStep"></span>
                        </div>
                        <div class="oper" style="right: 10px;">
                        </div>
                        <div class="box_content" style="width: 100%; margin-top: 25px; display: none;">
                            <div class="tabs" style="width: 99%;" align="center">
                                <ul>
                                    <li class="limouseon" id="uli1" onclick="showOne('showulli1')">10时</li>
                                    <li id="uli2" onclick="showOne('showulli2')">16时</li>
                                </ul>
                                <div id="divopbtn_uv" style="text-align: right; display: none;">
                                    <input type="button" value="自动生成" class="button" style="position: absolute; right: 210px;"
                                        onclick="javascript: ReadNormalUVData();">
                                    <input type="button" value="保存" class="button" style="position: absolute; right: 100px;"
                                        onclick="javascript: SaveOrUpdateUV();">
                                </div>
                            </div>
                            <div class="tabs_middle1" style="display: block;" id="showulli1">
                                <table class="default_table" align="center">
                                    <tbody>
                                        <tr>
                                            <th style="width: 11%;">
                                                2016年02月28日
                                            </th>
                                            <th style="width: 8%;">
                                                等级
                                            </th>
                                            <td class="UVDJ" style="width: 15%;">
                                                <select id="H10_H24_UV_LEVEL_SELECT" style="width: 90%;" class="fontColor">
                                                    <option value="1">一级</option>
                                                    <option value="2">二级</option>
                                                    <option value="3">三级</option>
                                                    <option value="4">四级</option>
                                                    <option value="5">五级</option>
                                                </select>
                                                <span id="Span1"></span>
                                            </td>
                                            <th style="width: 8%;">
                                                预报指数
                                            </th>
                                            <td class="UVDJ">
                                                <input type="text" style="width: 98%;" id="txtUVIndex_10" class="input fontColor">
                                                <span id="Span2"></span>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="tabs_middle1" style="display: none;" id="showulli2">
                                <table class="default_table" align="center">
                                    <tbody>
                                        <tr>
                                            <th style="width: 11%;">
                                                2016年02月28日
                                            </th>
                                            <th style="width: 8%;">
                                                等级
                                            </th>
                                            <td class="UVDJ" style="width: 15%;">
                                                <select id="H16_H24_UV_LEVEL_SELECT" style="display: none; width: 90%;" class="fontColor">
                                                    <option value="1">一级</option>
                                                    <option value="2">二级</option>
                                                    <option value="3">三级</option>
                                                    <option value="4">四级</option>
                                                    <option value="5">五级</option>
                                                </select>
                                                <span id="H16_H24_UV_LEVEL"></span>
                                            </td>
                                            <th style="width: 8%;">
                                                预报指数
                                            </th>
                                            <td class="UVDJ">
                                                <input type="text" style="display: none; width: 98%;" maxlength="5" id="txtUVIndex_16"
                                                    class="input fontColor">
                                                <span id="H16_H24_UV_DESC"></span>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="clear: both">
        </div>
    </div>
    <!--  值班日志 -->
    <div class="tabs_middle1" style="display: none; margin-top: 10px" id="DIV10">
        <div style="width: 100%; background-color: #f4f9ff">
            <table border="0" cellspacing="0" cellpadding="0" width="100%" id="tb_detail">
                <tbody>
                    <tr>
                        <td style="width: 10%; text-align: center;">
                            <label>
                                接班人:</label>
                        </td>
                        <td style="width: 90%; text-align: left;">
                            <select name="ddlSuccessor" id="ddlSuccessor" class="s_k3 fontColor">
                                <option value="-1">==选择接班人==</option>
                                <option value="2b0cd0e8-08a3-45b6-a14d-85409bc2b936">甄新蓉</option>
                                <option value="3c10d923-5860-402c-882d-cf3b58b9beae">陈镭</option>
                                <option value="501989ad-4a29-49cb-9213-3f253f3cfda1">张欣</option>
                                <option value="75e64b30-20c0-4ecb-9b0d-5dddad0ba482">毛卓成</option>
                                <option value="76acf686-25e7-49ae-8248-6112e53479dd">马井会</option>
                                <option value="8e6ed08e-d853-41ca-a715-140a5811c5fe">陈敏</option>
                                <option value="95177df5-78f1-4bf2-832c-1c09e5bceb0e">马雷鸣</option>
                                <option value="d41f1a43-5771-4233-ada3-b0fcc925ed80">瞿元昊</option>
                                <option value="e6c213ff-a6d7-48bc-b7da-8ebc1190f72c">陆佳麟</option>
                                <option value="f3473acf-efc8-4a83-a57b-afcbb58e4e79">无名</option>
                                <option value="fabaeafa-0b85-4732-924d-968749a8e41d">周伟东</option>
                                <option value="fb963faf-bff6-4e3d-a30a-18bbbb9b2a2f">曹钰</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <label>
                                值班日志:</label>
                        </td>
                        <td>
                            <textarea name="txtLogContent" id="txtLogContent" class=" input fontColor" rows="2"
                                cols="20" style="width: 90%; height: 200px;"></textarea>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td style="text-align: center;">
                            重要通知:
                        </td>
                        <td style="text-align: left;">
                            <input id="chkNotice" type="checkbox">
                        </td>
                    </tr>
                    <tr id="tr_Department" style="display: none;">
                        <td style="text-align: center;">
                            部门:
                        </td>
                        <td style="text-align: left;">
                            <select id="ddlDepartment" class="s_k3">
                            </select>
                        </td>
                    </tr>
                    <tr id="tr_Notice" style="display: none;">
                        <td style="width: 10%; text-align: center;">
                            通知内容:
                        </td>
                        <td style="width: 90%; text-align: left;">
                            <textarea id="txtNotice" rows="2" cols="20" style="width: 90%; height: 200px;"></textarea>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="text-align: center;">
                <a href="javascript:void(0)" class="easyui-linkbutton l-btn" iconcls="icon-ok" onclick="jjbSave()"
                    id=""><span class="l-btn-left"><span class="l-btn-text icon-ok" style="padding-left: 20px;">
                        保存</span></span></a>
            </div>
        </div>
    </div>
    <!-- 产品监控 -->
    <div class="tabs_middle1" style="display: none; margin-top: 10px;" id="DIV12">
        <div class="bg_middle" style="height: 100%;">
            <div class="m_control">
                <h2>
                    上海预报预警产品<span> </span>
                </h2>
                <div class="m_control_1">
                    <h3>
                        AQI分时段预报</h3>
                    <ul style="margin-left: 5px; margin-right: 5px;">
                        <li><span id="span_aqi06" class="finish1"><a id="a_aqi06" style="color: White; text-decoration: none;"
                            href="AQIForecast.aspx?type=1&amp;PGUID=0D941D7F-366D-4516-A571-5F0CCE369C3D">待完成</a></span><b>06时预报</b></li>
                        <li><span id="span_aqi11" class="finish1"><a id="a_aqi11" style="color: White; text-decoration: none;"
                            href="AQIForecast.aspx?type=2&amp;PGUID=0D941D7F-366D-4516-A571-5F0CCE369C3D">待完成</a></span><b>11时预报</b></li>
                        <li><span id="span_aqi17" class="finish"><a id="a_aqi17" style="color: White; text-decoration: none;"
                            href="AQIForecast.aspx?CPID=3919&amp;type=3&amp;PGUID=0D941D7F-366D-4516-A571-5F0CCE369C3D">
                            已发布</a></span><b>17时预报</b></li>
                    </ul>
                </div>
                <div class="m_control_1">
                    <h3>
                        AQI分区指导预报</h3>
                    <ul>
                        <li><span id="span_aqi_region_14" class="finish"><a id="a_aqi_region_14" style="color: White;
                            text-decoration: none;" href="AQISubareaForecast_One.aspx?CPID=3918&amp;type=3&amp;PGUID=DEB9DF3D-2158-4754-AB4A-854CA3BF07BD">
                            已发布</a></span><b>16时预报</b></li>
                    </ul>
                    <h3>
                        AQI分区预报文件</h3>
                    <ul>
                        <li><span id="span_aqi_region_14_Txt" class="finish"><a id="a_aqi_region_14_Txt"
                            style="color: White; text-decoration: none;" href="AQISubareaForecast_One.aspx?CPID=3918&amp;type=3&amp;PGUID=DEB9DF3D-2158-4754-AB4A-854CA3BF07BD">
                            已发布</a></span><b>16时预报</b></li>
                    </ul>
                </div>
                <div class="m_control_1">
                    <h3>
                        霾预报</h3>
                    <ul>
                        <li><span id="span_haze05" class="finish"><a id="a_haze05" style="color: White; text-decoration: none;"
                            href="HazeForecast.aspx?PID=3523&amp;type=1&amp;PGUID=99F3C5D1-8699-45E3-9F3C-995D1E7A5F75">
                            已发布</a></span><b>05时预报</b></li>
                        <li><span id="span_haze17" class="finish"><a id="a_haze17" style="color: White; text-decoration: none;"
                            href="HazeForecast.aspx?PID=3527&amp;type=2&amp;PGUID=99F3C5D1-8699-45E3-9F3C-995D1E7A5F75">
                            已发布</a></span><b>17时预报</b></li>
                    </ul>
                </div>
                <div class="m_control_1">
                    <h3>
                        UV预报</h3>
                    <ul>
                        <li><span id="span_uv10" class="finish"><a id="a_uv10" style="color: White; text-decoration: none;"
                            href="UVForecast.aspx?PID=3519&amp;type=1&amp;PGUID=FF4A0C98-EDF0-493B-A5D7-E750F56E38EF">
                            已发布</a></span><b>10时预报</b></li>
                        <li><span id="span_uv16" class="finish"><a id="a_uv16" style="color: White; text-decoration: none;"
                            href="UVForecast.aspx?PID=3525&amp;type=2&amp;PGUID=FF4A0C98-EDF0-493B-A5D7-E750F56E38EF">
                            已发布</a></span><b>16时预报</b></li>
                    </ul>
                </div>
                <div class="m_control_1">
                    <h3>
                        空气污染气象条件</h3>
                    <ul>
                        <li><span id="span_wrtj08" class="finish"><a id="a_wrtj08" style="color: White; text-decoration: none;"
                            href="AirPollutionForecast.aspx?CPID=3915&amp;type=1&amp;PGUID=2F25F65C-3D76-401C-98F4-F1AB8115F8F0">
                            已发布</a></span><b>08时预报</b></li>
                        <li><span id="span_wrtj20" class="finish"><a id="a_wrtj20" style="color: White; text-decoration: none;"
                            href="AirPollutionForecast.aspx?CPID=3917&amp;type=2&amp;PGUID=2F25F65C-3D76-401C-98F4-F1AB8115F8F0">
                            已发布</a></span><b>20时预报</b></li>
                    </ul>
                </div>
                <div class="m_control_1">
                    <h3>
                        臭氧预报</h3>
                    <ul>
                        <li><span id="span_O317" class="finish"><a id="a_O317" style="color: White; text-decoration: none;"
                            href="OzoneForecast.aspx?PID=3526&amp;type=1&amp;PGUID=230B32D7-C7C9-40BB-8B62-5B35D5E04A4D">
                            已发布</a></span><b>17时预报</b></li>
                    </ul>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="m_control">
                <h2>
                    华东区域预报预警产品<span> </span>
                </h2>
                <div class="m_control_1 m_control_1_height">
                    <h3>
                        指导预报文件</h3>
                    <ul>
                        <li><span id="span_aqi_region_14_hd" class="finish"><a id="a_aqi_region_14_hd" style="color: White;
                            text-decoration: none;" href="AQISubareaForecast_HD.aspx?CPID=3916&amp;type=3&amp;PGUID=994DCCD9-DDE6-4A4E-AC45-09353CF1FB56">
                            已发布</a></span><b>10:30预报</b></li>
                    </ul>
                </div>
                <div class="m_control_1 m_control_1_height">
                    <h3>
                        AQI落区预报</h3>
                    <ul>
                        <li><span id="span_AQILQ" class="finish"><a id="a_AQILQ" style="color: White; text-decoration: none;"
                            href="AQILQForecast.aspx?PID=1873&amp;LQType=1&amp;PGUID=0D941D7F-366D-4516-A571-5F0CCE369C3D">
                            已发布</a></span><b>14时预报</b></li>
                    </ul>
                </div>
                <div class="m_control_1 m_control_1_height">
                    <h3>
                        霾落区预报</h3>
                    <ul>
                        <li><span id="span_HazeLQ" class="finish"><a id="a_HazeLQ" style="color: White; text-decoration: none;"
                            href="CommonLQForecast.aspx?PID=1874&amp;LQType=6&amp;PGUID=99F3C5D1-8699-45E3-9F3C-995D1E7A5F75">
                            已发布</a></span><b>14时预报</b></li>
                    </ul>
                </div>
                <div class="m_control_1 m_control_1_height">
                    <h3>
                        空气污染气象条件落区预报</h3>
                    <ul>
                        <li><span id="span_WRTJLQ" class="finish"><a id="a_WRTJLQ" style="color: White; text-decoration: none;"
                            href="CommonLQForecast.aspx?PID=1875&amp;LQType=7&amp;PGUID=2F25F65C-3D76-401C-98F4-F1AB8115F8F0">
                            已发布</a></span><b>14时预报</b></li>
                    </ul>
                </div>
                <div class="m_control_1 m_control_1_height">
                    <h3>
                        华东区域重点城市预报</h3>
                    <ul>
                        <li><span id="span_HDCity" class="finish1"><a id="a_HDCity" style="color: White;
                            text-decoration: none;" href="HDCityProduct.aspx?PGUID=9349918F-ED70-42E0-86EA-C52C6C757529">
                            待完成</a></span><b>14时预报</b></li>
                    </ul>
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
    </div>
    </div>
</body>
</html>



