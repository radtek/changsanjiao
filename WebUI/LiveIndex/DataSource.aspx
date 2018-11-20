<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataSource.aspx.cs" Inherits="LiveIndex_DataSource" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/bootstrap-o.css" rel="stylesheet" />
    <link href="css/DataSource.css?v=20171213011" rel="stylesheet" />
    <script src="js/jquery-1.12.4.js"></script>
    <script src="js/vue.js"></script>
    <script src="js/vue-resource.js"></script>

</head>
<body style="background-color:#ddd">
    <form id="form1" runat="server">
    <div style="width:80%;margin:0 auto;background-color:white;height:500px;" id="frame">
        <div id="app"style="width:100%;margin:0px auto;padding-top:40px;position:relative;z-index: 2;" >
            <div class="left">
                <ul style="list-style:none;" >
                    <li v-for="(row,index) in left":class="{selected:index===selected,hover:index===hover }"  @mouseleave="leave()" @mouseenter="enter(index,$event)" v-on:click="click(index,$event)"><a :class="{color:index===color}">{{row}}</a></li>
                </ul>
            </div>
            <div v-for="(item, key,index) in data" class="right" style="width:70%;border:1px solid #ccc;float:left;margin-left:-1px;">
                <div class="form-group col-sm-12 info" v-for="station in item">
                    <label  style="margin-top:5px;float:left;margin-left:15px;width:100px;text-align:right;" class="control-label">{{station.name}}</label>
                    <div class="col-sm-10">
                        <input type="text" class="form-control" id="" v-model="station.value"/>
                    </div>
                </div>
            </div>
        </div>
    
    </div>
    </form>
</body>
    <script src="js/DataSource.js?v=1219001"></script>
</html>
