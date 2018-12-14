<%@ Page Language="C#" AutoEventWireup="true" CodeFile="foreData.aspx.cs" Inherits="EvaluateHtml_foreData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/foreData.css" rel="stylesheet" />
    <script src="JS/vue.js"></script>
    <script src="JS/vue-resource.js"></script>
    <script type="text/x-template" id="template">
        <table>
            <thead class='thead'>
                <tr if="parentxt!=''">
                    <td :colspan="col" style="text-align:center;border-bottom:1px solid white;">{{parentxt}}</td>
                </tr>
                <tr>
                    <template v-for="head in colname">
                        <td class="sub-txt" v-if="(head.sub==undefined || head.sub.length==0)">{{head.label}}</td>
                        <td v-else style="">
                            <my-component :col=head.col :parentxt="head.label" :colname="head.sub"></my-component>
                        </td>
                    </template>
                </tr>
            </thead>
        </table>
    </script>
</head>
    <%--<div class="parent-txt">{{head.label}}</div>--%>
<body>
    <form id="form1" runat="server">
    <div id="app">
        <my-component :parentxt="''" :colname="thead"></my-component>
    </div>
    </form>
</body>
<script src="JS/foreData.js"></script>
</html>
