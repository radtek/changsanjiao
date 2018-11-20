<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PubUserSet.aspx.cs" Inherits="HealthyWeather_PubUserSet" %>

<!doctype html>

<html class="no-js" lang="zh-cn">
<head id="Head1" runat="server">
    <title></title>
    <link href="../media/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/PubUserSet.css?v=0414101" rel="stylesheet" type="text/css" />
    <!-- Latest compiled and minified CSS -->
    <link href="css/bootstrap-select.min.css" rel="stylesheet" type="text/css" />
   <!-- Latest compiled and minified JavaScript -->
    
</head>
<body class="container-fluid">
    <div class="row">
      <div class="col-sm-3 col-md-3">
          <div class="form-horizontal row">
            <label for="selDisease" class="col-sm-4 control-label">疾病类型：</label>
            <div class="col-sm-8"><select class="form-control" id="selDisease" value=""></select></div>
          </div>
     
          <div class="form-horizontal row" style=" margin-top:10px  ">
            <label for="selDisease" class="col-sm-4 control-label">所属区域：</label>
            <div class="col-sm-8"><select class="form-control" id="selDiseaseII" value=""></select></div>
          </div>
        <div id="groupList" class="list-group"></div>
          <div id="barcon" class="barcon"></div>
        <div class="center gap"><button class="btn btn-default" id="btnAddGroup">创建分组</button></div>
        <div class="center gap"><button class="btn btn-default" id="btnEditGroup">编辑分组</button></div>
        <div class="center gap"><button class="btn btn-default" id="btnDelGroup" onclick="DelGroup()">删除分组</button></div>        
      </div>
      <div class="col-sm-9 col-md-9" style="padding-left:0">
        <table class="display dataTable no-footer" id="tblUsers" role="grid" aria-describedby="example_info"
            border="0" cellspacing="0" cellpadding="0" style="text-align: center">
        </table>
        <div id="floatDiv">
            <label class="checkbox-inline" style="margin-right:20px;"><input type="checkbox" id="chkAll"> 全选</label>
            <button type="button" class="btn btn-xs btn-info" id="btnAddUser" onclick="ShowUserWin(this.innerText.substring(1))"><strong>+</strong>创建用户</button>
              <!--王斌  2016.4.11-->
            <button type="button" class="btn btn-xs btn-info import" id="import" onclick="dataImport()"><strong>+</strong>用户导入</button>
              <!--王斌  2017.5.11-->
            <button type="button" class="btn btn-xs btn-info import" id="export" onclick="dataExport()"><strong>+</strong>用户导出</button>
        </div>
        <div id="btnBack">
            <button type="button" class="btn btn-xs btn-info" onclick="ShowSendWin(this.innerText)">邮件发送</button>
            <button type="button" class="btn btn-xs btn-info" onclick="ShowSendWin(this.innerText)">短信发送</button>
        </div>
      </div>
    </div>

    <div class="modal fade" id="mdlGroup" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title"></h4>
          </div>
          <div class="modal-body">
            <form class="form-horizontal">
              <div class="form-group">
                <label for="groupName" class="col-sm-3 control-label">分组名称:</label>
                <div class="col-sm-7"><input type="text" class="form-control" id="groupName"/></div><span class="need">*</span>
              </div>
              <div class="form-group edit">
                <label for="editGroupName" class="col-sm-3 control-label">新组名:</label>
                <div class="col-sm-7"><input type="text" class="form-control" id="editGroupName"/></div><span class="need">*</span>
              </div>
              <div class="form-group">
                <label for="pubRegion" class="col-sm-3 control-label">所属区域:</label>
                <div class="col-sm-7"><select name="usertype" class="selectpicker show-tick form-control" multiple data-live-search="false" id="selPubRegion"></select></div><span class="need">*</span>
              </div>
              <div class="form-group">
                <label for="pubHealType" class="col-sm-3 control-label">疾病类型:</label><div class="col-sm-8" id="pubHealType"></div>
              </div>
              <div class="form-group">
                <label for="message-text" class="col-sm-3 control-label">短信发送:</label><div class="col-sm-8" id="publishLvl"></div>
                <div class="col-sm-offset-3 col-sm-8" id="publishPeriod"></div>
              </div>
              <div class="form-group">
                <label for="emailLvl" class="col-sm-3 control-label">邮件发送:</label><div class="col-sm-8" id="emailLvl"></div>
                <div class="col-sm-offset-3 col-sm-8" id="emailPeriod"></div>
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">取消</button><button type="button" class="btn btn-info" onclick="EditGroup()">确定</button>
          </div>
        </div>
      </div>
    </div>
    
    <div class="modal fade" id="mdlUser" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title"></h4>
          </div>
          <div class="modal-body">
            <form class="form-horizontal">
              <div class="form-group">
                <label for="iptUserName" class="col-sm-3 control-label">姓名:</label>
                <div class="col-sm-7"><input type="text" class="form-control" id="iptUserName"/></div><span class="need">*</span>
              </div>
              <div class="form-group">
                <label for="iptUserPhone" class="col-sm-3 control-label">手机号码:</label>
                <div class="col-sm-7"><input type="text" class="form-control" id="iptUserPhone"/></div>
              </div>
              <div class="form-group">
                <label for="iptUserEmail" class="col-sm-3 control-label">邮件地址:</label>
                <div class="col-sm-7"><input type="email" class="form-control" id="iptUserEmail"/></div>
              </div>
              <div class="form-group">
                <label for="pubHealType" class="col-sm-3 control-label">分组名称:</label>
                <div class="col-sm-7"><select class="form-control" id="selUserGroup"></select></div><span class="need">*</span>
              </div>
              <div class="form-group">
                <label for="pubHealType" class="col-sm-3 control-label">疾病类型:</label><div class="col-sm-8" id="selUserHealType"></div>
              </div>
              <div class="form-group">
                <label class="col-sm-3 control-label checkbox-inline" style="font-weight:600;"><input type="checkbox" id="selUserPub"/> 允许发送短信:</label>
                <div class="col-sm-8" id="selUserPubLvl"></div>
                <div class="col-sm-offset-3 col-sm-8" id="selUserPubTime"></div>
              </div>
              <div class="form-group">
                <label class="col-sm-3 control-label checkbox-inline" style="font-weight:600;"><input type="checkbox" id="selUserEmail"/> 允许发送邮件:</label>
                <div class="col-sm-8" id="selUserEmailLvl"></div>
                <div class="col-sm-offset-3 col-sm-8" id="selUserEmailTime"></div>
              </div>
              <div class="form-group">
                <label for="selUserRemark" class="col-sm-3 control-label">备注信息:</label><div class="col-sm-7"><textarea class="form-control" rows="3" id="selUserRemark"></textarea></div>
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">取消</button><button type="button" class="btn btn-info" id="btnEditUser" onclick="EditUser()">确定</button>
          </div>
        </div>
      </div>
    </div>

    
    <div class="modal fade" tabindex="-1" role="dialog" aria-hidden="true" id="mdlSend">
        <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title"></h4>
          </div>
          <div class="modal-body">
            <div style="text-align:right;"><a href="##" onclick="ToggleCustom(this);">编辑自定义内容</a></div>
            <div id="regularDiv" class="center">
            </div>
              <div class="form-group center" id='regularDivII' style=" height:80px;margin-left: 28px;">
                <label for="pubHealType" class="col-sm-1 control-label">  &nbsp;&nbsp;</label><div class="col-sm-9" id="pubHealTypeII"></div>
              </div>
            
            <form class="form-horizontal row hide" id="customDiv" role="form">
              <div class="form-group" id="divTitle">
                <label for="iptQuery" class="col-sm-3 control-label">邮件标题：</label>
                <div class="col-sm-7"><input type="text" class="form-control" /></div>
              </div>
              <div class="form-group">
                <label for="iptQuery" class="col-sm-3 control-label">发送内容：</label>
                <div class="col-sm-7"><textarea class="form-control" rows="5" id="txtContent"></textarea></div>
              </div>
            </form>
            
          </div>
          <div class="modal-footer">
            <label class="control-label">发送人数：<span></span>人<strong></strong></label>
            <button type="button" class="btn btn-sm btn-default" data-dismiss="modal">取消</button>
            <button type="button" class="btn btn-sm btn-info" onclick="Send()">发送</button>
          </div>
        </div>
        </div>
    </div>


     <div class="modal fade" tabindex="-1" role="dialog" aria-hidden="true" id="customSend">
        <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title"></h4>
          </div>
          <div class="modal-body">
            <form class="form-horizontal row" id="Form1" role="form">
             <div class="form-group" id="customURL">
                <label for="iptQuerys" class="col-sm-3 control-label">邮箱地址：</label>
                <div class="col-sm-7"><input type="text" class="form-control"  placeholder="请输入邮箱地址，多个用逗号','分割" /></div>
              </div>
              <div class="form-group" id="customTitle">
                <label for="iptQuerys" class="col-sm-3 control-label">邮件标题：</label>
                <div class="col-sm-7"><input type="text" class="form-control" /></div>
              </div>
              <div class="form-group">
                <label for="iptQuerys" class="col-sm-3 control-label">发送内容：</label>
                <div class="col-sm-7"><textarea class="form-control" rows="5" id="txt_content"></textarea></div>
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-sm btn-default" data-dismiss="modal">取消</button>
            <button type="button" class="btn btn-sm btn-info" onclick="CustomSend('邮件')">发送</button>
          </div>
        </div>
        </div>
    </div>


     <div class="modal fade" tabindex="-1" role="dialog" aria-hidden="true" id="customSendMessage">
        <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title"></h4>
          </div>
          <div class="modal-body">
            <form class="form-horizontal row" id="Form2" role="form">
             <div class="form-group" id="tels">
                <label for="iptQuerys" class="col-sm-3 control-label">手机号码：</label>
                <div class="col-sm-7"><input type="text" class="form-control"  placeholder="请输入手机号码，多个用逗号','分割" /></div>
              </div>
              <div class="form-group">
                <label for="iptQuerys" class="col-sm-3 control-label">发送内容：</label>
                <div class="col-sm-7"><textarea class="form-control" rows="5" id="msgContent"></textarea></div>
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-sm btn-default" data-dismiss="modal">取消</button>
            <button type="button" class="btn btn-sm btn-info" onclick="CustomSend('短信')">发送</button>
          </div>
        </div>
        </div>
    </div>

        <!--王斌  2016.4.11-->
    <iframe id="uploadFrm" frameborder="no" border="0" scrolling="no" allowtransparency="yes" onload="iframeOnload()" name="uploadFrm" style="width:0px; height:0px; display:none;"></iframe>
    
    <div class="modal fade" id="mdlImport" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog" id="mdlDia" >
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" arial-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">数据导入</h4>
                </div>
                <div class="modal-body">
                
                    <form name="actionForm" id="actionForm" action="WebExplorerss.ashx" method="post" target="uploadFrm"  enctype="multipart/form-data" runat="server" >
                        <div class="form-group">
                            <label>文件类型：xlsx/xls</label>
                        </div>
                        <div class="form-group">
                            <input type="file" id="inputFile" accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" class="filestyle" runat="server" data-buttonText="选择文件" />
                        </div>
                    </form>
                    <div id='uploadStatus' style='display:none; margin-top:70px; '>
                               <img src='images/process.gif' style="width:350px;height:50px" />
                               <div style='color:#ccc;' id="dstext">正在上传，如果长时间不响应，可能是上传文件太大导致出错！
                               </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-sm btn-default" data-dismiss="modal">取消</button>
                    <button type="button" class="btn btn-sm btn-info" onclick="upLoad()">上传</button>
                </div>
            </div>
        </div>
    
    </div>



    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.min.js" type="text/javascript"></script>
   <script src="js/bootstrap.min.js" type="text/javascript"></script>
   
    <script src="js/PubUserSet.js?v=20170712001" type="text/javascript"></script>
    <script src="js/bootstrap-select.min.js"></script>
    <script src="js/bootstrap-filestyle.min.js" type="text/javascript"></script>
</body>
</html>
