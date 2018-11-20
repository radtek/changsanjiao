<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AQIPeriodLogDetail.aspx.cs" Inherits="AQI_ProductLogDetail_AQIPeriodLogDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../js/ProductLogDetail/AQIPeriodLogDetail.js" type="text/javascript"></script>
    <link href="../css/AQIPeriodLogDetail.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Ext.onReady(function () {
        var txtContent = $("#txtHidePeriodAQITxtTemplete").text();
        var msgContent = $("#txtHidePeriodAQIMsgTemplete").text();
        
        txtContent=txtContent.replace("{RangeTonight}",<%=strTodayAQI %>);
        txtContent=txtContent.replace("{AQIItemTonight}",<%=strTodayItemID %>);
         txtContent=txtContent.replace("{RangeTomMorning}",<%=strTomorrowAQI %>);
        txtContent=txtContent.replace("{AQIItemTomMorning}",<%=strTomorrowItemID %>);
         txtContent=txtContent.replace("{RangeTomAfternoon}",<%=strAfterAQI %>);
        txtContent=txtContent.replace("{AQIItemTomAfternoon}",<%=strAfterItemID %>);

          msgContent=msgContent.replace("{RangeTonight}",<%=strTodayAQI %>);
        msgContent=msgContent.replace("{AQIItemTonight}",<%=strTodayItemID %>);
         msgContent=msgContent.replace("{RangeTomMorning}",<%=strTomorrowAQI %>);
        msgContent=msgContent.replace("{AQIItemTomMorning}",<%=strTomorrowItemID %>);
         msgContent=msgContent.replace("{RangeTomAfternoon}",<%=strAfterAQI %>);
        msgContent=msgContent.replace("{AQIItemTomAfternoon}",<%=strAfterItemID %>);

        var win = new Ext.Panel({
            width: 1000,
            height: 500,
            layout: 'fit', //设置窗口内部布局
            renderTo: "aqiPeriodDetail",
            items: new Ext.TabPanel({//窗体中中是一个一个TabPanel
                autoTabs: true,
                activeTab: 0,
                deferredRender: false,
                border: false,
                buttonAlign: "center",
                items: [
                            {
                                id: "tabTxt",
                                title: '24小时AQI分时段预报',
                                html: '<textarea id="textArea" class="textPrev">'+txtContent+'</textarea>' // 内部显示内容
                            },
                            {
                                id: "tabMsg",
                                title: 'AQI分时段预报短信',
                                html: '<textarea id="msgArea" class="textPrev">'+txtContent+'</textarea>'
                            }
                        ]
            })
        });
    });
    </script>
</head>
<body>
<form id="form1" runat="server">

<div id="aqiPeriodDetail" class="aqiPeriodDetail">    
</div>
<textarea  name="txtHidePeriodAQITxtTemplete" id="txtHidePeriodAQITxtTemplete" cols="20" rows="2" style="display:none;">
上海市空气质量预报			
(	{PublishDate}	{Hour}发布）	
时段	AQI	空气质量等级	首要污染物
今天夜间（20时—06时）	{RangeTonight}	{LevelNight}	{AQIItemTonight}
明天上午（06时—12时）	{RangeTomMorning}	{LevelTomMorning}	{AQIItemTomMorning}
明天下午（12时—20时）	{RangeTomAfternoon}	{LevelTomAfternoon}	{AQIItemTomAfternoon}
			
			
上海中心气象台			
上海市环境监测中心			
联合发布
</textarea>
<textarea name="txtHidePeriodAQIMsgTemplete" id="txtHidePeriodAQIMsgTemplete" cols="20" rows="2" style=" display:none;">上海中心气象台和上海市环境监测中心{PublishDate}{Hour}时联合发布的上海市空气质量分时段预报：今夜{LevelNight}{AQIItemTonight}，AQI为{RangeTonight}；明天上午{LevelTomMorning}{AQIItemTomMorning}，AQI为{RangeTomMorning}；明天下午{LevelTomAfternoon}{AQIItemTomAfternoon}，AQI为{RangeTomAfternoon}。</textarea>
</form>

</body>
</html>
