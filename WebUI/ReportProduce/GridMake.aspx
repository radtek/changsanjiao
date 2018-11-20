<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GridMake.aspx.cs" Inherits="ReportProduce_GridMake" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/GridMake.css" rel="stylesheet" type="text/css" />
    <link href="../EvaluateHtml/css/Evaluate.css" rel="stylesheet" type="text/css" />
    <link href="../Ext/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>


 <script language="javascript" type="text/javascript" src="../JS/jquery-1.9.1.js"></script>
<script type="text/javascript" language="javascript" src="../media/js/jquery.js"></script>
<script type="text/javascript" language="javascript" src="../resources/syntax/shCore.js"></script>
<script type="text/javascript" language="javascript" src="../resources/demo.js"></script>
 <script type="text/javascript" src="../ui/jquery.ui.core.js"></script>
 <script type="text/javascript" src="../ui/jquery.ui.widget.js"></script>

    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="../JS/Chart/highstock.js" type="text/javascript"></script>
    <script src="../JS/Chart/modules/exporting.js" type="text/javascript"></script>
    
    <script src="../AQI/js/AQIUtility.js" type="text/javascript"></script>
    
    <script src="../JS/jquery.nicescroll.min.js" type="text/javascript"></script>
    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../css/Title.css" rel="stylesheet" type="text/css" />
    <script src="../JS/WebExplorerX.js" type="text/javascript"></script>
    <script src="../AQI/js/GridMake.js" type="text/javascript"></script>
    <script src="../JS/jquery-ui.js" type="text/javascript"></script>
</head>
<body>
<div style=" width:80%; margin-left:auto; margin-right:auto; ">
    <div class="divTop" >
    <div>
        <div class="checkStyle">
            <div class="checkLable" style=" margin-top:4px;">起报时间</div>
            <input id="H00" type="text" class="selectDateFormStyle" style="width:180px;" runat="server" onclick="WdatePicker({dateFmt:'yyyy年MM月dd日 20:00'})"/>
            <div class="checkLable" style=" margin-top:4px;margin-left:20px;">预报时效</div>
             <select id="forePeriodSel" style=" float:left;  margin-left:20px; margin-top:4px;">
              <option>24</option>
              <option>48</option>
             </select>
             <div class="checkLable" style=" margin-top:4px;margin-left:20px;">预报员</div>
             <input type="text" id="forecaster" style=" float:left;  margin-left:20px; margin-top:4px;" />
        </div>
         
    </div>
    </div>
    <div class="divTop" style="border-top:1px solid White">
    <div class="checkStyle" style="width:80%;">
         <div class="checkLable" style=" margin-top:4px;color:Red;">预报时效</div>
         <div class="checkLable" id="forePeriodHour" style=" margin-top:4px;margin-left:10px;color:Red;">24</div>
         <input type="button" style=" float:right;  margin-left:20px;" id="ScanBack" class="button" value="产品入库" />
         <input type="button" style=" float:right;  margin-left:20px;" id="saveData" class="button" value="保存" />
         <input type="button" style=" float:right;  margin-left:20px;" id="Button2" class="button" value="刷新预览"/>
         <input type="button" style=" float:right;  margin-left:20px;" id="Button3" class="button" value="载入订正产品"  />
         <input type="button" style=" float:right;  margin-left:20px;" id="importText" class="button" onclick="show('会上文件')" value="载入客观产品"  />
         <input type="button" style=" float:right;  margin-left:20px;" id="WRFData" class="button" value="载入模式产品"  />
         </div>
    </div>
    <div style=" clear:both;"></div>
    <div id="leftTable" class="score">
    <div id="siteDataTable" >
       <table   width='100%' border='0' cellpadding='0' cellspacing='0'>
         <tr>
           <td class='tabletitle'>站点</td>
           <td class='tabletitle'>SO2(μg/m3)</td>
           <td class='tabletitle'>NO2(μg/m3)</td>
           <td class='tabletitle'>PM10(μg/m3)</td>
           <td class='tabletitle'>CO(μg/m3)</td>
           <td class='tabletitle'>O3-8h_ave(μg/m3)</td>
           <td class='tabletitle'>PM2.5(μg/m3)</td>
           <td class='tabletitle'>首要污染物</td>
           <td class='tabletitle'>空气质量指数(AQI)</td>
         </tr>
         <tr>
           <td class="tableRowCity">南昌</td>
           <td class="tableRow" id="58606_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58606_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58606_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58606_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58606_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58606_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58606_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58606_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
         <tr>
           <td class="tableRowCity">宜春</td>
           <td class="tableRow" id="57793_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57793_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57793_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57793_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57793_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57793_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57793_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57793_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
         <tr>
           <td class="tableRowCity">新余</td>
           <td class="tableRow" id="57796_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57796_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57796_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57796_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57796_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57796_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57796_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57796_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
         <tr>
           <td class="tableRowCity">吉安</td>
           <td class="tableRow" id="57799_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57799_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57799_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57799_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57799_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57799_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57799_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57799_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
         <tr>
           <td class="tableRowCity">赣州</td>
           <td class="tableRow" id="57993_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57993_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57993_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57993_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57993_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57993_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57993_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57993_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
         <tr>
           <td class="tableRowCity">九江</td>
           <td class="tableRow" id="58502_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58502_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58502_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58502_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58502_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58502_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58502_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58502_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
         <tr>
           <td class="tableRowCity">景德镇</td>
           <td class="tableRow" id="58527_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58527_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58527_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58527_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58527_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58527_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58527_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58527_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
         <tr>
           <td class="tableRowCity">抚州</td>
           <td class="tableRow" id="58619_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58619_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58619_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58619_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58619_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58619_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58619_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58619_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
         <tr>
           <td class="tableRowCity">萍乡</td>
           <td class="tableRow" id="57786_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57786_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57786_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57786_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57786_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57786_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57786_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="57786_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
         <tr>
           <td class="tableRowCity">鹰潭</td>
           <td class="tableRow" id="58627_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58627_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58627_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58627_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58627_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58627_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58627_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58627_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
         <tr>
           <td class="tableRowCity">上饶</td>
           <td class="tableRow" id="58637_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58637_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58637_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58637_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58637_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58637_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58637_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58637_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
         <tr>
           <td class="tableRowCity">浮梁</td>
           <td class="tableRow" id="58524_7" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58524_3" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58524_2" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58524_6" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58524_5" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58524_1" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58524_First" onclick = "showInput(event,this)">--</td>
           <td class="tableRow" id="58524_AQI" onclick = "showInput(event,this)">--</td>
         </tr>
       </table>
    </div>
    </div>
    <div class="chartArea">
      <div id="leftChart"></div>
      <div id="rightChart"></div>
    </div>
    </div>
<iframe id="uploadFrm" frameborder="no" border="0" scrolling="no" onload="iframeOnload()" name="uploadFrm" style="border: 2px solid #0066FF; width:0px; height:0px; display:none;"></iframe>
<div id="dialog-form" style=" display:none;  height:100%;">
  <form  name="actionForm" id="actionForm" action="WebExplorerss.ashx" method="post" target="uploadFrm"  enctype="multipart/form-data">
    <fieldset  style=" border:0 none white;">
         <div id="editorHidden" class="show editorContent" style="position: absolute; margin-top:10px;"/>
         <div id="Tb1">
          <div >
           <span >省市:</span>
           <select name="Province" id="Province"><option value="区域中心">区域中心</option><option value="江苏省">江苏省</option><option value="浙江省">浙江省</option><option value="上海市">上海市</option><option value="安徽省">安徽省</option></select> 
           <span style="padding-left: 30px; line-height: 20px; padding-right:5px;">上传人:</span><input type="text" id="upName"  runat="server"  value="" style="width: 140px; text-align:center;" />
          </div>
          <div style="padding-top: 10px; padding-bottom: 10px;">
          <span >标题：</span><input type="text" id="title" runat="server"   value="" style="width: 295px; text-align:center;" />
          </div>
          <div>
          <span >文件：</span><input  type="file" id="fileAccept"  style="width:305px;" runat="server"/>
          </div>
          <div style="padding-top:10px; padding-left:50px;">
          <input type="button" class="Upload-btn upload-btnQuery" id="Button1" onclick="add()"onmouseover="this.className='Upload-btn-h upload-btnQuery'" onmouseout="this.className='Upload-btn upload-btnQuery'" onmousedown="this.className='Upload-btn-d upload-btnQuery'" onmouseup ="this.className='Upload-btn upload-btnQuery'" value="上传" />
          <input type="button"  class="UploadRelese-btn upload-relese" id="Button1" onclick="closeDialog()"onmouseover="this.className='UploadRelese-btn-h upload-relese'" onmouseout="this.className='UploadRelese-btn upload-relese'" onmousedown="this.className='UploadRelese-btn-d upload-relese'" onmouseup ="this.className='UploadRelese-btn upload-relese'" value="取消"/>
          </div>
         </div>
    <%--  <input type="submit" tabindex="-1" style="position:absolute; top:-1000px"/>--%>
    </fieldset>
  </form>
   <div id='uploadStatus' style='display:none; margin-top:50px; margin-left:35px;'><img src='images/process.gif' /><div style='color:#ccc;' id="dstext">正在上传，如果长时间不响应，可能是上传文件太大导致出错！</div></div>
</div>
</body>
</html>
