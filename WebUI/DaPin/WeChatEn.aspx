<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WeChatEn.aspx.cs" Inherits="HealthyWeather_WeChatEn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/WeChatEn.css?v=22212111211211" rel="stylesheet" type="text/css" />

    <script src="../JS/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="js/vue.js" type="text/javascript"></script>
    <script src="js/vue-resource.js" type="text/javascript"></script>

    <script type="text/x-template" id="template">
            <div class="struct">
                <div class="_mid">
                    <div class="img-bg">
                        <img :src=imgs alt="" style="width:100%;" />
                    </div>
                    <div class="search">
                       <img src="images/search.png" style="width:5vw;" alt="search">
                    </div>
                    <div class="type">
                        <p style="text-align:center" v-for="txt in type">{{txt.text}}</p>
                    </div>
                </div>
                <div class="date">
                    <div class="one" style="height:4vw; overflow:hidden">
                        <label>{{day.one}}</label>
                        <div v-if="val[0].one==1" :class="{grade: true, 'ml':sty ,active1:true}"></div>
                        <div v-else :class="{grade:true,'ml':sty}"></div>
                        <div v-if="val[0].two==2" :class="{grade:true,active2:true}"></div>
                        <div v-else :class="{grade:true}"></div>
                        <div v-if="val[0].three==3" :class="{grade:true,active3:true}"></div>
                        <div v-else :class="{grade:true}"></div>
                        <div v-if="val[0].four==4" :class="{grade:true,active4:true}"></div>
                        <div v-else :class="{grade:true}"></div>
                        <div v-if="val[0].five==5" :class="{grade:true,active5:true}"></div>
                        <div v-else :class="{grade:true}"></div>
                    </div>
                    <div class="two" style="height:4vw; overflow:hidden">
                        <label>{{day.two}}</label>
                        <div v-if="val[1].one==1" :class="{grade:true,active1:true}"></div>
                        <div v-else :class="{grade:true}"></div>
                        <div v-if="val[1].two==2" :class="{grade:true,active2:true}"></div>
                        <div v-else :class="{grade:true}"></div>
                        <div v-if="val[1].three==3" :class="{grade:true,active3:true}"></div>
                        <div v-else :class="{grade:true}"></div>
                        <div v-if="val[1].four==4" :class="{grade:true,active4:true}"></div>
                        <div v-else :class="{grade:true}"></div>
                        <div v-if="val[1].five==5" :class="{grade:true,active5:true}"></div>
                        <div v-else :class="{grade:true}"></div>
                    </div>
                </div>
            </div>

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="page"><%-- <div v-if="val[0].one==1" :class="{grade: true, 'ml': mlIs ,active1:true}"></div>--%>
        <div style="height:100vh;width:100vw;">
            <img src="images/mobile.jpeg" alt="背景" style="height:100vh;width:100vw;" />
        </div>
        <div style="position:fixed;top:0;left:0;">
            <div class="top">
                <span style="font-size:4.2vw;margin-left:2vw;float:left;">Shanghai</span>
                <span style="font-size:4.2vw; float:right;margin-right:2vw;">{{date}}</span>
            </div>
            <my-component v-for="item in result" :type="item.type" :imgs="item.img" :day="day" :val="item.val" :sty="mlIs1"></my-component>
            <div class="struct"></div>
            <div class="bottom" style=" height:15vh;width:100%;float:left;margin-bottom:0;padding: 3vh 8vw;">
                <div class="level" style="background: rgb(57, 176, 110);">Low</div>
                <div class="level" style="background: rgb(26, 161, 230);">Mild</div>
                <div class="level" style="background: rgb(254, 239, 53);">Medium</div>
                <div class="level" style="background: rgb(241, 151, 37);">High</div>
                <div class="level" style="background: rgb(252, 13, 27);">Serious</div>
            </div>
        </div>
    </div>
    </form>
</body>
    <script src="js/WeChatEn.js?v=1011111113133" type="text/javascript"></script>
</html>
