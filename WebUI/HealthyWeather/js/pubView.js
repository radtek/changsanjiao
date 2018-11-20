
var tp = "";
var json={
    "title": [{ "status": "状态" }, { "status": "FTP" }, { "status": "短信" }, { "status": "邮件" }],
    "sucess": [{ "status": "成功" }, { "status": "0" }, { "status": "0" }, { "status": "0" }],
    "failure": [{ "status": "失败" }, { "status": "0" }, { "status": "0" }, { "status": "0" }],
    "total": [{ "status": "总数" }, { "status": "0" }, { "status": "0" }, { "status": "0" }]
}
var vm = null;
$(function () {
    var top = $("#sendTypeTab").outerHeight(true)  + $("#Span1").outerHeight(true)-20;
    $("#send-Tab").css('top', top);
    $('#sendTypeTab a').click(function (obj) {
        if (obj.stopPropagation) {
            obj.preventDefault();
            obj.stopPropagation();
        }
        else if (window.event)// this code is for IE8
            window.event.cancelBubble = true;
        var theClass = $(this.parentElement).attr("class");
        if (!theClass || theClass.indexOf("disabled") == -1) {
            $(this).tab('show');
            tp = $(this).attr("aria-controls").replace("Div", "");
            GetContent(tp);
        }
    });

    InitRegion();
    $($("#sendTypeTab a")[0]).click();
    GetLastTime();
    GetLastWSTime();
    //1123王斌   获取发送的数量
    vm = new Vue({
        el: "#send-Tab",
        data: {
            "json": json,
            "morning": "<上午 ",
            "afternoon":" 下午>"
        }
        , mounted: function () {
            this.$nextTick(function () {
                this.clickTime("morning");
            })
        }
        , methods: {
            getSendNum: function (sd) {
                var that = this;
                this.$http.post('PubView.aspx/GetSendNum',{'sd':sd}).then(function (response) {
                    var data = response.data.d;
                    var json = data.split('*');
                    for (i = 0; i < json.length; i++) {
                        eval('('+json[i]+')');
                    }
                });
            }
            , clickTime: function (id) {
                var sd = "上午";
                $(".selected").removeClass("selected");
                $("#" + id).addClass("selected");
                
                if (id == "afternoon") {
                    sd = "下午";
                }
                this.getSendNum(sd)
            }
        }
    })
});

function gradeChange() {
    GetContent(tp);
}

function InitRegion() {
    var datas = station.split(',');
    var html = "  <label class='radio-inline'><input type='radio' name='radioselUserPubLvl' value='全部'  onClick='gradeChange()' checked=true>全部</label>";
    for (var i = 0; i < datas.length; i++) {
        html += "<label class='radio-inline'><input type='radio' name='radioselUserPubLvl' value='" + datas[i] + "' onClick='gradeChange()'>" + datas[i] + "</label>";
    }
    $("#selName").html(html);
}

function GetContent(type) {

    //var name = $("#selName").val();
   var name= $("input[name='radioselUserPubLvl']:checked").val();
    $.ajax({
        url: "PubView.aspx/GetContents",
        type: "POST",
        contentType: "application/json",
        data: "{type:'" + type + "',selectSite:'" + name + "'}",
        dataType: 'json',
        success: function (results) {
            $("#" + type + "Div").html(results.d);
        },
        error: function (ex) {
            //alert("异常，" + ex.responseText + "！");
        }
    });
}

function GetLastTime() {
    var html = '<table cellspacing="1" cellpadding="5" border="0" bgcolor="#e9e9e9">';
    $.ajax({
        url: "PubView.aspx/GetLastTime",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            if (results.d) {
                var info = eval("("+results.d+")");
                $("#spanLastTime").html(info.time);
                $("#spanUser").html(info.user);
            }
        },
        error: function (ex) {
           // alert("异常，" + ex.responseText + "！");
        }
    });
}

//获取上一次更新的服务时间
function GetLastWSTime() {
    $.ajax({
        url: "PubView.aspx/GetLastWSTime",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            if (results.d) {
                var info = eval("(" + results.d + ")");
                $("#last_wsTime").html("最后WebService更新时间</br>" + info.time);
            }
        },
        error: function (ex) {
            // alert("异常，" + ex.responseText + "！");
        }
    });
}

function SendAll() {
//    GetEmailReceiver();
    //    SendMessage();
    SendAll();
}

function SendAll() {

    if (!confirm("是否要全部发送？")) return;

    $.ajax({
        url: "PubView.aspx/SendAll",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            if (results.d) {
                if (results.d.toString().indexOf("成功发送到后台处理") >= 0) {
                    updateWSII(); //全部发送自动更新时间。
                }
                alert(results.d);
            }
        },
        error: function (ex) {
            alert("发送失败，" + ex.responseText + "！");
        }
    });
}

function SendMessage() {
    $.ajax({
        url: "PubView.aspx/SendMessage",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        error: function (ex) {
            alert("异常，" + ex.responseText + "！");
        }
    });
}
function GetEmailReceiver() {
    $.ajax({
        url: "PubView.aspx/GetEmailReceiver",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {
            if (results.d) SendEmail(results.d);
        },
        error: function (ex) {
           // alert("异常，" + ex.responseText + "！");
        }
    });
}
function SendEmail(userID) {

    var loginParams = getCookie("UserInfo");
    var logResult = loginParams.split(","); //{Alias:'管理员'
    var userName = logResult[0].replace("{Alias:'", "").replace("'", "");

    $.ajax({
        url: "PubUserSet.aspx/EmailRegular",
        type: "POST",
        contentType: "application/json",
        data: "{ UserIDS: '" + userID + "',time:'01_02_03_04',isAll:0,m_aliass:'" + userName + "'}",
        dataType: 'json',
        success: function (results) {
            alert(results.d);
        },
        error: function (ex) {
            alert("发送失败，请联系技术人员！异常信息，" + ex.responseText + "！");
        }
    });
}

//wb  2017.6.9
//点击更新webService
function updateWS() {
    if (!confirm("是否更新服务？")) return;
    $.ajax({
        url: "PubView.aspx/UpdateSend",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {

            GetLastWSTime(); // xuehui 07-19

            if (results.d) alert(results.d);
        },
        error: function (ex) {
            alert("更新失败，" + ex.responseText + "！");
        }
    });
}

function updateWSII() {
 
    $.ajax({
        url: "PubView.aspx/UpdateSend",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (results) {

             GetLastWSTime(); // xuehui 07-19

            //if (results.d) alert(results.d);
        },
        error: function (ex) {
            alert("更新webService失败，" + ex.responseText + "！");
        }
    });
}

//单独发送保存的数据
function onlySend(id) {
    var type = $("#sendTypeTab .active").text();
    var compare = $("#"+id+"").parent().text();
    if (type != compare) return;
    if (!confirm("是否单独发送？")) return;
    $.ajax({
        url: "PubView.aspx/OnlySend",
        type: "POST",
        contentType: "application/json",
        data: "{ type: '" + type + "'}",
        dataType: 'json',
        success: function (results) {
            GetLastTime();
             
            if (results.d) alert(results.d);
        },
        error: function (ex) {
            alert("发送失败，" + ex.responseText + "！");
        }
    });
}
