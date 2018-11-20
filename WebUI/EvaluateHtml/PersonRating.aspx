<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonRating.aspx.cs" Inherits="EvaluateHtml_PersonRating" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>个人排名评分</title>
    <link href="css/Evaluate.css?v=0315" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="css/QueryFilter.css?v=0315" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css" />
    <link href="css/PersonRating2.css?v=0315" rel="stylesheet" />

    <script src="JS/jquery-1.9.1.js"></script>
    <script src="JS/bootstrap.min.js"></script>
    <script src="JS/bootstrap-filestyle.min.js"></script>
    <script src="JS/Utility.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
    <script src="JS/vue.js" type="text/javascript"></script>
    <script src="JS/vue-resource.js" type="text/javascript"></script>
    <script src="JS/jquery.table2excel.js"></script>
</head>
<body>
  <div style=" width:95%; margin-left:auto; margin-right:auto;"id="page">
    <div class="divTop" >
        <div>
            <div class="checkStyle">
                <div class="checkLable">评分时间</div>
                <%--<input id="H00" type="text" class="selectDateFormStyle" runat="server" onchange="InitTable()" onclick="WdatePicker({ dateFmt: 'yyyy年MM月' })"/>--%>
<%--                <input type="button" style=" float:left;  margin-left:20px;" id="ScanBack" class="button" value="查询" onclick="InitTable()" />
                <input type="button" style=" float:left;  margin-left:20px;" id="Button2" class="button" value="保存" onclick="ResoreData()" />
                <input type="button" style=" float:left;  margin-left:20px;" value="导入" class="button" onclick="importData()" /> 
                <input type="button" style=" float:left;  margin-left:20px;" id="Button1" class="button" value="导出" onclick="OutTable()" />
                <input type="button" style=" float:left;  margin-left:20px;" id="tempDownload" class="button" value="模板下载" onclick="tempDownload()" />--%>
            <input type="text" id="H00" :class='{topBtn:true}' style="margin-left:0;padding-left:0px;"  @click="wp"/>
            <input type="button" :class='{topBtn:true,topBtnHover:true}' @click="btnQuery"  value="查询"/>
            <input type="button" :class='{topBtn:true,topBtnHover:true}' @click="save" value="保存"/>
            <input type="button" :class='{topBtn:true,topBtnHover:true}' @click="importData" value="导入"/>
            <input type="button" :class='{topBtn:true,topBtnHover:true}' @click="exportDate" value="导出"/>
            <input type="button" :class='{topBtn:true,topBtnHover:true}' @click="tempDownload" value="模板下载"/>
             <input type="button" :class='{topBtn:true,topBtnHover:true}' @click="evaluate" value="评分"/>
            </div>
        </div>
    </div>
    <div style=" clear:both;"></div>
    <div id="leftTable" class="score">
        <div id="coutTable0" ></div>
    </div>
    <div class="bg" id="bg"  onclick="fadeOut()"></div>
    <div id="showImg" class="hidden">
        <div  id="personMonth" class="OnlyOne">
        </div>
    </div>
      <div id="showEveryDay" class="hidden">
          <div id="personDay" class="OnlyOne">
          </div>
      </div>
      <div id="secondTab">
        <div :class="{tableContent:true}">
            <table class="contentTab" style="margin:0 auto;width:100%;margin-bottom:40px;">
                <tr>
                    <td class="title" :colspan="tabOne.colspan" v-html="tabOne.val" v-for="(tabOne,index) in tabTitleOne"></td>
                </tr>
                <tr>
                    <td class="title" v-html="tabTitleTwo[index]" v-for="(tabTwo,index) in tabTitleTwo"></td>
                </tr>
                <tr v-for="(resultRow,key) in results">
                    <td class="conCell" v-if="resultRow[0].val!='没有数据'" v-for="item in resultRow">
                        <div v-for="(col,key) in item">{{col}}</div>
                    </td>
                    <td class="conCell" v-else colspan="26">
                        <div style="text-align:left;">注：{{resultRow[0].val}}</div>
                    </td>
                </tr>
            </table>
        </div>
        <iframe id="uploadFrm" frameborder="no" border="0" scrolling="no" allowtransparency="yes" name="uploadFrm" style="width:0px; height:0px; display:none;"></iframe>
        <div class="modal fade" id="mdlImport" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog" id="mdlDia" >
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" arial-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">数据导入</h4>
                    </div>
                    <div class="modal-body">
                
                        <form name="actionForm" id="actionForm" action="WebExplorer.ashx"  method="post" target="uploadFrm"  enctype="multipart/form-data" runat="server" >
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
                        <button type="button" class="btn btn-sm btn-info" @click="upLoad">上传</button>
                    </div>
                </div>
            </div>
        </div>
      </div>

    </div>
</body>
     <script language="javascript" type="text/javascript" src="JS/PersonRating.js?v=0315"></script>
</html>
