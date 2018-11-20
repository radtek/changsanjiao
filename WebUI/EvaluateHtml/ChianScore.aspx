<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChianScore.aspx.cs" Inherits="EvaluateHtml_ChianScore" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>国家局日评分</title>
 <link href="css/Evaluate.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>

    <script src="JS/jquery-1.9.1.js"></script>
    <script src="JS/jquery.table2excel.js"></script>
    <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="../Ext/ext-all.js"></script>
    <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>

    <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
    <script src="../My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="JS/vue.js" type="text/javascript"></script>
    <script src="JS/vue-resource.js" type="text/javascript"></script>

</head>
<body>
   
    <div style=" width:80%; margin-left:auto; margin-right:auto; " id="page">
        <div class="divTop" >
            <div>
                <div class="checkStyle">
                    <div class="checkLable" style="margin-top: 4px;">评分时间</div>
                    <input id="H00" type="text" class="selectDateFormStyle" runat="server" onchange="InitTable()" onclick="WdatePicker({ dateFmt: 'yyyy年MM月' })" />
                    <input type="button" style="float: left; margin-left: 20px;" id="evaluate" class="button" value="查询"  @click="evaluate" />
                    <input type="button" style="float: left; margin-left: 20px;" id="real" class="button" value="实况" @click="real" />
                    <input type="button" style="float: left; margin-left: 20px;" id="forecast" class="button" value="预报" @click="forecast" />
                    <input type="button" style="float: left; margin-left: 20px;" id="ScanBack" hidden class="button" value="查询" @click="query" />
                    <input type="button" style="float: left; margin-left: 20px;" id="Button1" class="button" value="导出" @click="OutTable()" /> 

                </div>
            </div>
        </div>
        <div style=" clear:both;"></div>
        <div id="leftTable" class="score">
            <div id="coutTable0" class="chinaTable" >
                <table :class="{skybtab:skybtab,tab:true}">
                    <tr>
                        <td :class="{tabletitle:true,yb:yb,sk:sk}" style="line-height: 48px;" v-for="ele in title">{{ele}}</td>
                    </tr>
                    <tr v-for="item in results" v-if="results['row0'][0].val!='没有数据'">
                        <td v-for="v in item" :class="{tableRow:true,yb:yb,sk:sk}">{{v.val}}</td>
                    </tr>
                    <tr v-for="item in results" v-else>
                        <td style="text-align:left;" :colspan="item[0].colspan" :class="{tableRow:true,yb:yb,sk:sk}">注：{{item[0].val}}</td>
                    </tr>
                    
                </table>
            </div>
        </div>
    </div>
     <form id="form1" runat="server">
     <asp:HiddenField id="Element" runat="server" />
    <asp:HiddenField id="Type" runat="server" />
    <asp:Button ID="btnExport" runat="server" onclick="Button1_Click" Text="Button1" CssClass="inVisibility" />
    </form>
</body>
<script language="javascript" type="text/javascript" src="JS/ChianScore.js?v=000"></script>
</html>