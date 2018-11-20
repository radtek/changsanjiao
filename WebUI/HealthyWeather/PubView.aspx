<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PubView.aspx.cs" Inherits="HealthyWeather_PubView" %>
<!doctype html>
<html class="no-js" lang="zh-cn">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/PubUserSet.css?v=0109" rel="stylesheet" type="text/css" />
      <script language="javascript" type="text/javascript">
          var station = "<%=m_station %>"
    </script>
</head>
<body class="container-fluid">
    <div style=" width:100%">最后发送时间：<strong id="spanLastTime">----</strong>发送人：<strong id="spanUser">----</strong>
     
       <%--  <button type="button" class="btn btn-sm btn-info" style="float:right; " onclick="SendAll();">全部发送</button>--%>
           区域：
    <div class="col-sm-8" id="selName" style=" width:55%; padding-left: 0px; float:right; padding-right: 0px; text-align: left; ">
        <label class="radio-inline"><input type="radio" name="radioselUserPubLvl" value="01" >全部</label>
    </div>
              
    </div>
    <div class="le">
        <ul class="nav nav-pills nav-stacked col-md-2" role="tablist" id="sendTypeTab">
          <li role="presentation" style=" text-align:center"><a href="#emailDiv" aria-controls="emailDiv" role="tab" data-toggle="tab">邮件发送</a><span class="pop"id="email" title="是否单独发送" onclick="onlySend(id)"></span></li>
          <li role="presentation" style=" text-align:center"><a href="#messageDiv" aria-controls="messageDiv" role="tab" data-toggle="tab">短信发送</a><span class="pop"id="message" title="是否单独发送" onclick="onlySend(id)"></span></li>
          <li role="presentation" style=" text-align:center"><a href="#ftpDiv" aria-controls="ftpDiv" role="tab" data-toggle="tab">FTP发送</a><span class="pop"id="ftp" title="是否单独发送" onclick="onlySend(id)"></span></li>
          <li role="presentation" style=" text-align:center"><a href="#ftpDiv2" aria-controls="" role="tab" data-toggle="tab" onclick="updateWS()">更新WebService</a></li>
       </ul>
     <%--   <button onclick="updateWS()"style=" text-align:center" class="col-md-2 btn-default btn update">更新webService <br /> </button>--%>
        <span id="last_wsTime" style=" text-align:center; margin-top:40px; cursor:default " class="col-md-2 update"></span>
        <span id="Span1" style="color:blue;text-align:center; margin-top:180px; cursor:default " class="col-md-2 update">注：上午12点前只发布10时数据，12点后发布17时数据！</span>
        <button onclick="SendAll()"style=" text-align:center; margin-top:120px;" class="col-md-2 btn-default btn update btn-info">一键发布</button>
        <%-- 短信发送成功状态表格--%>
      <div class="send-Tab col-sm-2" id="send-Tab">
          <table style="width:100%;" >
              <tr v-for="(item,index) in json">
                  <td v-for="(td,index) in item" class="send-td">{{td.status}}</td>
              </tr>
          </table>
          <div class="time-tab">
              <div id="morning" class="morning" v-on:click="clickTime('morning')">{{morning}}</div>
              <div id="afternoon" class="afternoon" v-on:click="clickTime('afternoon')">{{afternoon}}</div>
          </div>
      </div>
      
        <div class="tab-content col-md-10" style="padding:0px;">
          <div role="tabpanel" class="tab-pane" id="emailDiv"></div>
          <div role="tabpanel" class="tab-pane" id="messageDiv"></div>
          <div role="tabpanel" class="tab-pane" id="ftpDiv"></div>
        </div>
    </div>
    <script src="js/vue.js"></script>
    <script src="js/vue-resource.js"></script>
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
     <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="js/pubView.js?v=20171207" type="text/javascript"></script>
</body>
</html>
