<%@ Page Language="C#" AutoEventWireup="true" CodeFile="userManage.aspx.cs" Inherits="AQI_userManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户管理</title>
 <script language="javascript" type="text/javascript">
 </script> 
  <link href="images/css/UserManage.css" rel="stylesheet" type="text/css" />
 <link href="images/css/css.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 
 <script language="javascript" type="text/javascript" src="js/AQIUtility.js"></script>
 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="js/userManage.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>
</head>
<body>
    <div class="content" style="width:100%;-webkit-overflow-scrolling:touch; overflow: auto;">
    <div id="head" class="show">
        <div id="UserContent">
      <label id="userName" style="line-height: 25px; float: left; margin-right: 5px;">用户名 </label>
        <div id="userSelectOpt" style="width: 120px; height: 30px; float: left; padding-right:30px;">
<%--        <select name="userArray"  class="listStytel" id="userArraySelect">
        </select>--%>
        </div>
           <%--    <input type="button"  id="btnQuery" class="normal-btn input-btnQuery" value="查询" onclick="publicLogQuery()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" style="margin-left: 8px" />
              <input type="button" id="ExportData" class="normal-btn input-btnQuery" value="添加" onclick="addUser()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'"/>
              <input type="button" id="ButDelete" class="normal-btn input-btnQuery" value="删除" onclick="deleteSomeUsers()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'"/>--%>
        <div id="div_btn">
          <button type="button" class="normal-btn input-btnQuery"  id="btnQuery"  onclick="publicLogQuery()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-Query"></span>
            <span class="select-text">查询</span>
             </button>
          <button type="button" class="normal-btn input-btnQuery"  id="ExportData"  onclick="addUser()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-add"></span>
            <span class="select-text">添加</span>
          </button>
          <button type="button" class="normal-btn input-btnQuery"  id="ButDelete"  onclick="deleteSomeUsers()" onmouseover="this.className='normal-btn-h input-btnQuery'" onmouseout="this.className='normal-btn input-btnQuery'" onmousedown="this.className='normal-btn-d input-btnQuery'" onmouseup ="this.className='normal-btn input-btnQuery'">
            <span class="select-delete"></span>
            <span class="select-text">删除</span>
         </button>
         </div>
        </div>
        <div id="contentNone">
        </div>
        </div>
    <div id="addHidden" class="hidden editorContent" style="position: absolute">
    <label style="margin: 5px; padding: 0px;font-size: 15px; text-align: left; line-height: 30px;">新增用户</label>
    <div class="border"></div>
    <table>
        <tr>
            <td><label id="Label1" class="useNameLabel" >用户名:</label></td>
            <td><input id="Name" class="divEditor"/></td>
        </tr>
        <tr>
            <td><label id="Label6" class="useNameLabel" >密码:</label></td>
            <td><input id="passWord" type="password" class="divEditor" onfocus="this.className = 'divEditor inputHighlighted';" onblur="this.className = 'divEditor';"/></td>
        </tr>
        <tr>
            <td><label id="Label2" class="useNameLabel" >确认密码:</label></td>
            <td><input id="passWord1" type="password" class="divEditor"  onfocus="this.className = 'divEditor inputHighlighted';" onblur="this.className = 'divEditor';" /></td>
        </tr>
        <tr>
            <td><label id="Label7" class="useNameLabel" >别名:</label></td>
            <td><input id="Alias" class="divEditor" /></td>
        </tr>
        <tr>
            <td><label id="Label8" class="useNameLabel" >角色:</label></td>
            <td>
            <input type="radio" name="index" value="1"  checked="checked"/> 环境监测中心
            <input type="radio" name="index" value="2" style="padding-left: 10px" /> 气象局
            </td>
        </tr>
        <tr>
            <td><label id="Label9" class="useNameLabel" >邮箱:</label></td>
            <td><input id="email" class="divEditor"/></td>
        </tr>
    </table>
    <div style="padding-top: 20px;height:20px; padding-bottom: 10px;"> 
            <div style="float: left; padding-left: 200px;"  id="Addbutton">
                <input type="submit" name="btnSettingSites" value="确定" onclick="AddUserEditor()" id="Submit1" class="button_default">
                &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="关闭" onclick="closeAdd()" class="button_default">
            </div>
        </div>
    </div>
        <div id="editorHidden" class="hidden editorContent" style="position: absolute">
    <label style="margin: 5px; padding: 0px;font-size: 15px; text-align: left; line-height: 30px;">用户编辑</label>
    <div class="border"></div>
    <table>
        <tr>
            <td><label id="EditorNameLabel" class="useNameLabel" >用户名:</label></td>
            <td><input id="EditorName" class="divEditor" /></td>
        </tr>
        <tr>
            <td><label id="Label4" class="useNameLabel" >密码:</label></td>
            <td><input id="EditorpassWord" type="password" class="divEditor" onfocus="this.className = 'divEditor inputHighlighted';" onblur="this.className = 'divEditor';"/></td>
        </tr>
        <tr>
            <td><label id="Label5" class="useNameLabel" >确认密码:</label></td>
            <td><input id="EditorpassWord1" type="password" class="divEditor" onfocus="this.className = 'divEditor inputHighlighted';" onblur="this.className = 'divEditor';" /></td>
        </tr>
        <tr>
            <td><label id="Label10" class="useNameLabel" >别名:</label></td>
            <td><input id="EditorAlias" class="divEditor"/></td>
        </tr>
        <tr>
            <td><label id="Label11" class="useNameLabel" >角色:</label></td>
            <td>
            <input type="radio" name="Editorindex" value="1"  checked="checked"/> 环境监测中心
            <input type="radio" name="Editorindex" value="2" style="padding-left: 10px" /> 气象局
            </td>
        </tr>
        <tr>
            <td><label id="Label12" class="useNameLabel" >邮箱:</label></td>
            <td><input id="Editoremail" class="divEditor"/></td>
        </tr>
    </table>
    <div style="padding-top: 20px;height:20px; padding-bottom: 10px;"> 
            <div style="float: left; padding-left: 200px;" id="editorButton">
                <input type="submit" name="btnSettingSites" value="确定" onclick="OKEditor()" id="Submit2" class="button_default">
                &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="关闭" onclick="closeReviseEditor()" class="button_default">
            </div>
        </div>
    </div>
    </div>
</body>
</html>
