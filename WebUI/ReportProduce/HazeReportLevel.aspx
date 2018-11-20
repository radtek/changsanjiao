<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HazeReportLevel.aspx.cs" Inherits="ReportProduce_HazeReportLevel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="../css/WorkSchedule.css" rel="stylesheet" type="text/css" />
    <link href="../css/WorkSchedule_2.css" rel="stylesheet" type="text/css" />
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
        $(function () {
            var pageWidth = document.body.clientWidth;
            var pageHeight = document.documentElement.clientHeight;
            $(".contenttitle2").height(pageHeight - 80);
        })
    </script>
<link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="tabs_middle1_left" style="width:99%">
       <div class="contenttitle">
         <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>霾的预报等级</span>
           </div>
           <div class="pointTitle">一、轻微霾标准：</div>
           <div class="contenttitle3">
    <textarea readonly rows="" cols="" style="width:100%;height:100px;border-width:0px;">        
            1．5.0km≤能见度&lt;10.0km且相对湿度＜80%且PM2.5≤115 µg/m3；
            2．5.0km≤能见度&lt;10.0km且相对湿度≥80%且75µg/m3＜PM2.5≤150 µg/m3。
            服务描述：
            轻微霾天气，儿童、老人、呼吸系统和心脑系统疾病患者注意防护，适量减少户外活动。
            </textarea>
            </div>
             <div class="pointTitle">二、轻度霾标准：</div>
             <div class="contenttitle3">
            <textarea readonly rows="" cols="" style="width:100%;height:150px;border-width:0px;">    
            1．5.0km≤能见度&lt;10.0km且相对湿度&lt;80%且115µg/m3＜PM2.5≤250 µg/m3；
            2．5.0km≤能见度&lt;10.0km且相对湿度≥80%且150µg/m3＜PM2.5≤250 µg/m3；
            3．3.0km≤能见度&lt;5.0km且相对湿度&lt;80%且PM2.5≤150 µg/m3；
            4. 3.0km≤能见度&lt;5.0km且相对湿度≥80%且75 µg/m3＜PM2.5≤150 µg/m3；
            5．能见度&lt;3.0km且相对湿度≥80%且75 µg/m3＜PM2.5≤115 µg/m3。
            服务描述：
            轻度霾天气，请注意防护，儿童、老人、呼吸系统和心脑系统疾病患者减少户外活动。
            </textarea>
            </div>
            <div class="pointTitle">三、中度霾标准：</div>
             <div class="contenttitle3">
            <textarea readonly rows="" cols="" style="width:100%;height:150px;border-width:0px;">            
            1．5.0km≤能见度&lt;10.0km且250µg/m3＜PM2.5≤500µg/m3；
            2．3.0km≤能见度&lt;5.0km且150µg/m3＜PM2.5≤250 µg/m3；
            3．2.0km≤能见度&lt;3.0km且相对湿度&lt;80%且PM2.5≤250 µg/m3；
            4. 2.0km≤能见度&lt;3.0km且相对湿度≥80%且115 µg/m3＜PM2.5≤250 µg/m3；
            5．能见度&lt;2.0km且相对湿度≥80%且115 µg/m3＜PM2.5≤150 µg/m3。
            服务描述：
            中度霾天气，因空气质量明显降低，请适当防护；一般人群适量减少户外活动；儿童、老人、呼吸系统和心脑系统疾病患者减少外出。
            </textarea>
            </div>
            <div class="pointTitle">四、重度霾标准：</div>
             <div class="contenttitle3">
            <textarea readonly rows="" cols="" style="width:100%;height:150px;border-width:0px;">            
            1．5.0km≤能见度&lt;10.0km且PM2.5&gt;500 µg/m3；
            2．2.0km≤能见度&lt;5.0km且250µg/m3＜PM2.5≤500µg/m3；
            3．1.0km≤能见度&lt;2.0km且相对湿度&lt;80%且PM2.5＜500 µg/m3；
            4．1.0km≤能见度&lt;2.0km且相对湿度≥80%且150µg/m3＜PM2.5≤500 µg/m3；
            5．能见度&lt;1.0km且相对湿度≥80%且150µg/m3＜PM2.5≤250 µg/m3。
            服务描述：
            重度霾天气，因空气质量差，请加强防护；一般人群减少户外活动；儿童、老人、呼吸系统和心脑系统疾病患者尽量避免外出；相关部门启动污染减排措施。
            </textarea>
            </div>
            <div class="pointTitle">五、严重霾标准：</div>
             <div class="contenttitle3">
            <textarea readonly rows="" cols="" style="width:100%;height:420px;border-width:0px;">            
            1．1.0km≤能见度&lt;5.0km且PM2.5&gt;500 µg/m3；
            2．能见度&lt;1.0km且相对湿度&lt;80%
            3. 能见度&lt;1.0km且相对湿度≥80%且PM2.5&gt;250 µg/m3；
            服务描述：
            严重霾天气，因空气质量很差，请加强防护；一般人群避免户外活动；儿童、老人、呼吸系统和心脑系统疾病患者应留在室内；驾驶人员谨慎驾驶；相关部门按照职责采取相应措施，控制污染物排放。


            根据以上条件列出下列表格：
            当相对湿度≥80%时：
            &lt;1km	[1-2)km	[2-3)km	[3-5)km	[5-10)km	VIS
            PM2.5(µg/m3)
            轻度	轻度	轻度	轻度	轻微	(75-115]
            中度	中度	中度	轻度	轻微	(115-150]
            重度	重度	中度	中度	轻度	(150-250]
            严重	重度	重度	重度	中度	(250-500]
            严重	严重	严重	严重	重度	&gt;500

            当相对湿度&lt;80%时：
            &lt;1km	[1-2)km	[2-3)km	[3-5)km	[5-10)km	VIS
            PM2.5(µg/m3)
            严重	重度	中度	轻度	轻微	(0-115]
            严重	重度	中度	轻度	轻度	(115-150]
            严重	重度	中度	中度	轻度	(150-250]
            严重	重度	重度	重度	中度	(250-500]
            严重	严重	严重	严重	重度	&gt;500
  </textarea>
  </div>

 
       </div>
    </div>
</body>
</html>
