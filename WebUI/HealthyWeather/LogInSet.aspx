<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogInSet.aspx.cs" Inherits="HealthyWeather_LogInSet" %>

<!doctype html>

<html class="no-js" lang="zh-cn">
<head runat="server">
    <title></title>
    <link href="../media/css/jquery.dataTables.min.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="css/LogInSet.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-select.min.css" rel="stylesheet" type="text/css" />
</head>
<body class="container-fluid">
    <div class="row">
      <div class="col-sm-3 col-md-3">
        <div class="form-horizontal row">
          <label for="SearchbyStation" class="col-sm-4 control-label">角色：</label>
          <div class="col-sm-8"><select class="form-control" id="SearchbyStation" value=""></select></div>
          <div class="form-group">
            <label for="SearchByArea" class="col-sm-4  control-label">所属区域:</label>
            <div class="col-sm-8" id="SearchByArea" style=" margin-top:10px;"></div>
          </div>    
        </div>
      </div> 
      <div class="col-sm-9 col-md-9" style="padding-left:0">
        <table class="display dataTable no-footer" id="register" role="grid" aria-describedby="example_info"
            border="0" cellspacing="0" cellpadding="0" style="text-align: center">
        </table>
        <div id="floatDiv">
            <button type="button" class="btn btn-xs btn-info" id="btnAddUser" onclick="ShowUserWin(this.innerText.substring(1))"><strong>+</strong>创建用户</button>
        </div>
      </div>
    </div>
    <div class="modal fade" id="mdlUser" tabindex="-1" role="dialog">
      <div class="modal-dialog" role="document" style="width:700px;">
        <div class="modal-content">
          <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title"></h4>
          </div>
          <div class="modal-body">
            <form class="form-horizontal">
              <div class="form-group">
                <label for="iptName" class="col-sm-2 control-label">姓名:</label>
                <div class="col-sm-8"><input type="text" class="form-control" id="iptName"/></div>
              </div>
              <div class="form-group">
                <label for="iptuserName" class="col-sm-2 control-label">用户名:</label>
                <div class="col-sm-8"><input type="text" class="form-control" id="iptuserName"/></div><span class="need">*</span>
              </div>
              <div class="form-group">
                <label for="iptuserName" class="col-sm-2 control-label">密码:</label>
                <div class="col-sm-8"><input type="text" class="form-control" id="key"/></div><span class="need">*</span>
              </div>
              <div class="form-group">
                <label for="iptStation" class="col-sm-2 control-label">角色:</label>
                <div class="col-sm-9" id="iptStation"></div><span class="need">*</span>
              </div>
              <div class="form-group">
                <label for="iptEmail" class="col-sm-2 control-label">邮件地址:</label>
                <div class="col-sm-8"><input type="email" class="form-control" id="iptEmail"/></div>
              </div>
              <div class="form-group">
                <label for="iptArea" class="col-sm-2 control-label">所属区域:</label>
                <div class="col-sm-9" id="iptArea"></div>
              </div>
              <div class="form-group">
                <label for="iptArea" class="col-sm-2 control-label">所属单位:</label>
                <div class="col-sm-8"><select name="usertype" style="color:Black" class="selectpicker show-tick form-control"  data-live-search="false" id="selPubRegion"></select></div>
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
            <button type="button" class="btn btn-info" id="btnEditUser" onclick="EditUser()">确定</button>
          </div>
        </div>
      </div>
    </div>
    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../media/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
     <script src="js/bootstrap-select.min.js"></script>
       <script src="js/bootstrap-filestyle.min.js" type="text/javascript"></script>
    <script src="js/LogInSet.js?v=20171207" type="text/javascript"></script>
</body>
</html>
