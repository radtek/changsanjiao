<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HazeWarningRuleaspx.aspx.cs" Inherits="ReportProduce_HazeWarningRuleaspx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
            <link href="../css/WorkSchedule.css" rel="stylesheet" type="text/css" />
    <link href="../css/WorkSchedule_2.css" rel="stylesheet" type="text/css" />
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<link href="../css/Title.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
         $(function () {
             var pageWidth = document.body.clientWidth;
             var pageHeight = document.documentElement.clientHeight;
             $(".contenttitle2").height(pageHeight - 80);
         })
    </script>
</head>
<body>
   <div class="tabs_middle1_left" style="width:99%;">
       <div class="contenttitle">
        <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>霾的预警信号标准</span>
           </div>
           <div class="pointTitle">一、霾预警信号分级</div>
           <div class="contenttitle3">
    <textarea readonly rows="" cols="" style="width:100%;height:50px; border-width:0px;">        
            霾预警信号分为三级，以黄色、橙色和红色表示，分别对应预报等级用语的中度霾、重度霾和严重霾。
            </textarea></div>
            <div class="pointTitle">二、霾黄色预警信号</div>
           <div class="contenttitle3">
    <textarea readonly rows="" cols="" style="width:100%;height:200px; border-width:0px;">            
            图标：
            （1）能见度小于2000米，且相对湿度大于等于80%，且PM2.5浓度大于115微克/立方米且小于等于150微克/立方米。
            （2）能见度小于3000米且大于等于2000米，相对湿度小于80%的霾（PM2.5浓度小于等于250微克/立方米）。
            （3）能见度小于3000米且大于等于2000米，且相对湿度大于等于80%，且PM2.5浓度大于115微克/立方米且小于等于250微克/立方米。
            （4）能见度小于5000米且大于等于3000米，且PM2.5浓度大于150微克/立方米且小于等于250微克/立方米。
            （5）能见度小于10000米且大于等于5000米，且PM2.5浓度大于250微克/立方米且小于等于500微克/立方米。
            预报用语：预计未来24小时内将出现中度霾，PM2.5质量浓度可达×××微克/立方米，易形成中度空气污染。
            或：目前已经出现中度霾，并将持续，易形成中度空气污染。
            防御指南：
            1.空气质量明显降低，请适当防护；
            2.一般人群适量减少户外活动，儿童、老人、呼吸系统和心脑系统疾病患者应减少外出。
            </textarea></div>
            <div class="pointTitle">三、霾橙色预警信号</div>
           <div class="contenttitle3">
    <textarea readonly rows="" cols="" style="width:100%;height:250px; border-width:0px;">          
            图标：  
            标准：预计未来24小时内可能出现下列条件之一并且过程持续时间达6小时及以上或实况已达到下列条件之一并且过程持续时间可能达6小时及以上：
            （1）能见度小于1000米且相对湿度大于等于80%，且PM2.5浓度大于150微克/立方米且小于等于250微克/立方米
            （2）能见度小于2000米且大于等于1000米且相对湿度小于80%的霾（PM2.5浓度小于等于500微克/立方米）。
            （3）能见度小于2000米且大于等于1000米，且相对湿度大于等于80%，PM2.5浓度大于150微克/立方米且小于等于500微克/立方米。
            （4）能见度小于5000米且大于等于2000米，PM2.5浓度大于250微克/立方米且小于等于500微克/立方米。
            （5）能见度小于10000米且大于等于5000米，PM2.5浓度大于500微克/立方米。
            预报用语：预计未来24小时内将出现重度霾，PM2.5质量浓度可达×××微克/立方米，易形成重度空气污染。
            或：目前已经出现重度霾，并将持续，易形成重度空气污染。 
            防御指南：
            1.空气质量差，请加强防护；
            2.一般人群减少户外活动，儿童、老人、呼吸系统和心脑系统疾病患者尽量避免外出；
            3.相关部门启动污染减排措施。
            </textarea></div>
             <div class="pointTitle">四、霾红色预警信号</div>
           <div class="contenttitle3">
    <textarea readonly rows="" cols="" style="width:100%;height:250px; border-width:0px;">          
            图标：  
            标准：预计未来24小时内可能出现下列条件之一并且过程持续时间达6小时及以上或实况已达到下列条件之一并过程持续时间可能达6小时及以上：
            （1）能见度小于1000米且相对湿度小于80%的霾。
            （2）能见度小于1000米且相对湿度大于等于80%，PM2.5浓度大于250微克/立方米。
            （3）能见度小于5000米且大于等于1000米，PM2.5浓度大于500微克/立方米。
            预报用语：预计未来24小时内将出现严重霾，PM2.5质量浓度可达×××微克/立方米，易形成严重空气污染。
            或：目前已经出现严重霾，并将持续，易形成严重空气污染。
            防御指南：
            1.空气质量很差，请加强防护；
            2.一般人群避免户外活动；儿童、老人、呼吸系统和心脑系统疾病患者应留在室内；
            3.驾驶人员谨慎驾驶；
            4.相关部门按照职责采取相应措施，控制污染物排放。
</textarea>

           </div>
       </div>
    </div>
</body>
</html>
