<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkSchedule.aspx.cs" Inherits="ReportProduce_WorkSchedule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/WorkSchedule.css" rel="stylesheet" type="text/css" />
    <link href="../css/WorkSchedule_2.css" rel="stylesheet" type="text/css" />
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<link href="../css/Title.css" rel="stylesheet" type="text/css" />
<script src="../AQI/js/CalclulateHeight.js" type="text/javascript"></script>
    <script type="text/javascript">
        function showli(objId) {

            $("#ulli1").removeClass("subLi_Sel");
            $("#ulli2").removeClass("subLi_Sel");
            $("#ulli3").removeClass("subLi_Sel");
            $("#ulli4").removeClass("subLi_Sel");

            if (objId == "show1") {
                $("#ulli1").addClass("subLi_Sel");
                $("#show1").show();
                $("#show2").hide();
                $("#show3").hide();
                $("#show4").hide();
            }
            else if (objId == "show2") {
                $("#ulli2").addClass("subLi_Sel");
                $("#show1").hide();
                $("#show2").show();
                $("#show3").hide();
                $("#show4").hide();

            }
            else if (objId == "show3") {
                $("#ulli3").addClass("subLi_Sel");
                $("#show1").hide();
                $("#show2").hide();
                $("#show3").show();
                $("#show4").hide();
            }
            else if (objId == "show4") {
                $("#ulli4").addClass("subLi_Sel");
                $("#show1").hide();
                $("#show2").hide();
                $("#show3").hide();
                $("#show4").show();
            }
        }
    </script>
</head>
<body>
    <div class="tabs_middle1_left" style="width:99%">
    <div class="contenttitle">
     <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>工作日程</span>
           </div>
       <div class="contenttitle2">
    <div class="tabs" style="width:99%">
       <ul>
       <li class="subLi subLi_Sel" id="ulli1" onmousemove="showli('show1')">环境领班</li>
       <li id="ulli2" class="subLi" onmousemove="showli('show2')" >环境主班</li>
       <li id="ulli4" class="subLi" onmousemove="showli('show4')" >数值预报班</li>
       <li id="ulli3" class="subLi" onmousemove="showli('show3')" >环境副班</li>
       </ul>
    </div>
    <div class="tabs_middle1" style="display: block;" id="show1"> 
    <table class="tabletd" style="width:90%;margin-top:10px;margin-bottom:30px;margin-left:auto;margin-right:auto;" border="0" cellspacing="1">
      <tbody><tr>
        <td width="98" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>时间</strong></td>
        <td width="500" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>内容</strong></td>
        <td width="205" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>备注</strong></td>
     </tr>
     <tr bgcolor="#FFFFFF">
        <td height="24" align="center" class="tablerow4Out">09：00—11：00</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">调看各种气象资料，综合分析，掌握天气变化趋势，制作内部会商ppt；</td>
        <td height="24" align="left" class="tablerowOut"></td>
     </tr>
     <tr bgcolor="#FFFFFF">
        <td height="24" align="center" class="tablerow4Out">11：00—11：30</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"> 组织本局远程会商，把关污染天气图的制作和分析。</td>
        <td height="24" align="left" class="tablerowOut"></td>
     </tr>
     <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">11：30—12：00</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">午餐</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
     </tr>
     <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">不定期</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut" class="tablerowOut">负责组织区域环境气象预报会商，负责全国霾天气预报会商发言；</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut" class="tablerowOut"></td>
     </tr>
     <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">不定期</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut" class="tablerowOut">负责组织和制作区域环境气象决策服务材料，组织上海市环境气象决策服务材料的制作和审核；</td>
       <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut" class="tablerowOut"></td>
     </tr>
     <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">13:00—14:00</td>
       <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">负责制作区域重点城市环境气象预报产品；</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
     </tr>
     <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">不定期</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">把关环境气象预报预警结论。</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
     </tr>
    
  </tbody></table>
 </div>
    <div class="tabs_middle1" style="display: none;" id="show2"> 
    <table class="tabletd" style="width:90%;margin-top:10px;margin-bottom:30px;margin-left:auto;margin-right:auto;"border="0" cellspacing="1" >
       <tbody><tr>
        <td width="98" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>时间</strong></td>
        <td width="500" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>内容</strong></td>
        <td width="205" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>备注</strong></td>
      </tr>
      <tr bgcolor="#FFFFFF">
        <td height="24" align="center" class="tablerow4Out">09：00—09：10</td>
        <td height="24" align="left" class="tablerowOut">生活气象指数短信发布</td>
        <td height="24" align="left" class="tablerowOut"></td>
        </tr>
        <tr bgcolor="#FFFFFF">
        <td height="24" align="center" class="tablerow4Out">09：10—09：20</td>
        <td height="24" align="left" class="tablerowOut">细菌性食物中毒等级预报（每年4-10月）</td>
        <td height="24" align="left" class="tablerowOut"></td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">09：20—11：00</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">制作化学天气综合图，准备内部会商材料。（如果下午发言，准备PPT）</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">认真填写天气记录簿相关内容。</td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">09：50</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">检查紫外线上传</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">01.URP文件是否上传至172.21.1.3</td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">10:10</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">运行大气扩散模式</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
     </tr>
     <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">10：30</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">检查华东区域的AQI指导预报文件是否上传成功	</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">10:50—11:00</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">生活指数预报</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">11:00—11:30</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">环境内部会商</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">11:30—12:30</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">午餐</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">注意设置值班电话呼叫转移。 </td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">12:30</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">1、AQI上传国家局 2、下发区县局AQI预报</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">12:30—14:00</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">调看各种气象资料，综合分析天气形势</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">14:00—14:10</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">紫外线预报并上传国家局</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">14:10—14:30		</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">制作并上传华东区域AQI落区预报图（霾、空气污染气象条件、PM10、PM2.5、O3-1h、O3-8h）</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">注意确保传送至相关服务器上。</td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">15：00—15：30</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">参与预报会商（一体化预报发言）。</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">认真撰写会商记录并签字！</td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">15：30—16：00</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">各类生活指数预报（包括分人群感冒和人体综合舒适度指数）</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">注意各网页更新后的检查及查看产品发布日志，确认已成功发送到指定的服务器上。</td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">16：00—16：30	</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">1.	与环境监测中心会商</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">16:40—17:00</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">发布环境预报产品 <br>
    ①发布AQI预报产品（AQI预报文本产品上传至：fserver、62平台、公服中心和科技服务中心，同时发布预报短信）<br>
    ②发布与AQI预报相关的人体舒适度预报产品<br>
    ③发布三天霾和臭氧预报产品<br>
    ④上传上海地区空气污染气象条件文本+图片<br>
      </td>
      <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">每天</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">所有环境预报班预报产品检查</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">不定期	</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">霾、浮尘等天气	区县监测实况咨询和短信发布</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">注意与首席预报员的沟通。</td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">不定期</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">如果预报有霾或火险等级4级，电话通知中心台首席</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">不定期</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">霾专题报告</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">不定期	</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">霾预警信号</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">注意各网页更新后的检查。</td>
      </tr>

       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">不定期</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">极端天气内部通报</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">注意时间并确认！</td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">不定期	</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">任务单</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">完成市局下发的任务单内容号</td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">不定期	</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">重污染预警（短信+NOTES——短信发布人群和notes发布地址如下，重污染预警期间主班要值守）</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">
        1.	杨引明/台长/中心气象台/上海/CMA@CMA,<br> 
        张晖/处长/应急与减灾处/上海/CMA@CMA,<br>
         耿福海/浦东新区气象局/区县气象局/上海/CMA@CMA, <br>
         许建明/浦东新区气象局/区县气象局/上海/CMA@CMA,<br>
          刘静/公共服务管理/应急与减灾处/上海/CMA@CMA,<br>
           首席服务官/首席服务官办公室/公共气象服务中心/上海/CMA@CMA
           </td>
      </tr>
   </tbody></table>
  </div>
    <div class="tabs_middle1" style="display: none;" id="show3"> 
    <table class="tabletd" style="width:90%;margin-top:10px;margin-bottom:30px;margin-left:auto;margin-right:auto;" border="0" cellspacing="1" >
       <tbody><tr>
        <td width="98" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>时间</strong></td>
        <td width="500" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>内容</strong></td>
        <td width="205" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>备注</strong></td>
     </tr>
      <tr>
      <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">09：00—11：00</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">调看各种气象资料，综合分析，掌握天气变化趋势。</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">11：00—11：30</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">负责登陆本局远程会商系统。</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">11：30—12：00	</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">午餐</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">预警发布期间</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">协助领班及主班完成预警发布工作。</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">重污染期间	</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">协助领班制作提供决策服务材料及会商材料所需素材，负责一体化平台的沟通、协调以及与环保、区域其他气象部门的联动；</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
       <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">危险化学品泄露事件时</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">当发生危险化学品和有毒气体泄漏事件时，负责预报产品的制作和服务联动；</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut"></td>
      </tr>
  </tbody></table>
  </div>   
    <div class="tabs_middle1" style="display: none;" id="show4">
    <table class="tabletd" style="width:90%;margin-top:10px;margin-bottom:30px;margin-left:auto;margin-right:auto;" border="0" cellspacing="1" >
       <tbody><tr>
        <td width="98" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>时间</strong></td>
        <td width="500" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>内容</strong></td>
        <td width="205" height="24" align="center" bgcolor="#FFFFFF" class="tabletitleDateOut"><strong>备注</strong></td>
     </tr>
      <tr>
      <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">9:00-09:15</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">检查数值预报系统是否正常运行；如未能正常运行，联系系统运维人员检查并补算</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">数值预报检验</td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">09:15-10:50</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">1)昨日预报效果评估<br>2)全国及上海监测实况<br>3)数值与结果(区域及上海)</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">会商材料准备</td>
      </tr>
      <tr>
        <td height="24" align="center" bgcolor="#FFFFFF" class="tablerow4Out">11:00-11:30</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">参加内部会商，给出前一日数值预报结果评估、污染实况和数值预报结论</td>
        <td height="24" align="left" bgcolor="#FFFFFF" class="tablerowOut">内部会商</td>
      </tr>
  </tbody></table>
 </div>

    </div>
   </div>
  </div>
</body>
</html>
