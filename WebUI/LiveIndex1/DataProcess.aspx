<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataProcess.aspx.cs" Inherits="LiveIndex_DataProcess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/bootstrap-o.css" rel="stylesheet" type="text/css" />
    <link href="css/DataProcess.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-1.12.4.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/Utility.js" type="text/javascript"></script>
    <script src="js/vue.js" type="text/javascript"></script>
    <script src="js/DataProcess.js" type="text/javascript"></script>
</head>
<body style="background-color:#efefef;">
    <form id="form1" runat="server">
    <div id="dataPro" class="form-horizontal">
        <div class="title" id="J_title">
            <div :style="{paddingLeft: '10px',width:'90%'}">72小时指数自动计算控制-起报日：{{startDate}}</div>
        </div>
        <div :class='{content:true}'>
            <div :class='{nav:true}'>
                <div :class="{icon:true}"></div>
                <ul>
                    <libtn ref="mybox" v-on:leave="_leave($event)" v-on:enter="_enter($event)" v-on:dj="_click($event)" v-for="(item,index) in btnData" :msg="item.text" :key="item.text" :data-index="index" :class='{liBtn:true}' ></libtn>
                </ul>
            </div>
            <div :class='{dataSource:true}'>
                <div :class='{_dataSource:true}'>
                    <label style="width:130px;float:left;padding-left:4%;">数据来源：</label>
                    <div class="radio" style="float:left;padding:0;padding-right:25px">
                        <label>
                            <input type="radio" v-model="source" value="refine">精细化预报数据
                        </label>
                    </div>
                    <div class="radio" style="float:left;padding:0;">
                        <label>
                            <input type="radio" v-model="source" value="num">数值预报数据
                        </label>
                    </div>
                </div>
            </div>
            <div :style='{backgroundColor:"white",width:"100%"}'>
                <div :class='{frame:true}':style='{position:"relative"}' v-for="items in info">
                    <div :class='{wordInfo:true}'>
                        <span>{{items.header}}</span>
                    </div>
                    <div :class="{dataInfo:true}">
                        <div :class='{fileDateContent:true}'>
                            <div class="form-group" v-for="(item,index) in items.label">
                                <label for="inputPassword" class="col-sm-2 control-label">{{item.text}}：</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control"/>
                                </div>
                           </div>
                        </div>
                    </div>
                </div>

                <div :class='{frame:true}':style='{position:"relative"}'>
                    <div :class='{wordInfo:true}'>
                        <span>执行输出信息</span>
                    </div>
                    <div :class="{dataInfo:true}">
                        <div :class='{fileDateContent:true}'>
                            <div class="form-group">
                                <div class="col-sm-10" style="margin-left:2%;">
                                    <textarea class="form-control outputArea"></textarea>
                                </div>
                           </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    </form>
</body>
</html>
