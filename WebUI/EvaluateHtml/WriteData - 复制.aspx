<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WriteData - 复制.aspx.cs" Inherits="EvaluateHtml_WriteData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
  <%--  <link href="css/bootstrap.css" rel="stylesheet" />--%>
    <link href="../My97DatePicker/skin/WdatePicker.css" rel="stylesheet" />
    <link href="css/WriteData.css" rel="stylesheet" />

    <script src="JS/Utility.js"></script>
    <script src="../My97DatePicker/WdatePicker.js"></script>
    <script src="JS/vue.js"></script>
    <script src="JS/vue-resource.js"></script>
    <script type="text/x-template" id="template">
        <div class="wrap" :class="[group.className]">
            <div class="header">
                <div class="header-con">
                    <div class="point"></div>
                    <span class="h">{{group.header}}</span>
                </div>
            </div>
            <div class="content">
                <div class="rows-con"  v-for="(item,key,index) in group.data">
                    <label>{{key}}:</label>
                    <input :value=item :class="{val:true}" :readonly="group.edit" type="text" />
                </div>
            </div>
        </div>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="app">
        <div class="_page-header">
            <div class="header-item">
                <label>预报员：</label>
                <label id="forecaster">管理员</label>
            </div>
            <div class="header-item">
                <label>时间：</label>
                <input type="text"  v-once :value="date" id="time" class="Wdate" onclick="WdatePicker({ dchanging: cDayFunc });" />
            </div>
            <div class="header-item">
                <input type="button" id="query" class="header-btn" value="查询"  @click="getData()" />
            </div>
             <div class="header-item">
                <input type="button" id="confirm" class="header-btn" value="确认"/ @click="confirm">
            </div>
        </div>
        <div class="page-container" style="position:relative;margin-top:10px;">
            <div class="page-left">
                <my-component v-for="g in groups" :group="g"></my-component>
            </div>
            <div class="page-right">
                <div class="header">
                    <div class="header-con">
                        <div class="point"></div>
                        <span class="h">{{periodData.header}}</span>
                    </div>
                </div>
                <div class="period-content">
                    <table style="width:100%;border-collapse:collapse;">
                        <thead>
                            <tr>
                                <td></td>
                                <td v-for="(item,key) in periodData.data.上午">{{key}}</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(item,key) in periodData.data">
                                <td>{{key}}</td>
                                <td v-for="val in item">{{val}}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        
    </div>
    </form>
</body>
<script src="JS/WriteData.js"></script>
</html>
