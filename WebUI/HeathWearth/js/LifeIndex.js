/// <reference path="lib/jquery-3.3.1.js" />
$(function () {
    GetLiveIndexData();
})

function GetLiveIndexData() {

    $.ajax({
        url: "LifeIndex.aspx/GetLiveIndexData",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (resData) {
            //console.log(resData.d)
            var data = resData.d;
            if (data != "") {
                //字符串转化为字符串数组
                var strArr = data.split(";");
                //将图片icon定义成一个数组
                var iconUrl = ["img/icon/zszs.png", "img/icon/swhd.png", "img/icon/cyzs.png", "img/tiganzhishu.png", "img/icon/xszs.png", "img/icon/ktkq.png"];

                var iconIndex = 0;
                var titleFlag = "";

                for (var i = 0; i < strArr.length; i++) {
                    if (i == strArr.length - 1) {
                        var timeStr = strArr[i];
                        $(".container .topbar.font-md").append("<span class='pull-right'>" + timeStr + "</span>")
                    } else {
                        //将数组中的每个元素再根据空格分割成数组，动态的生成HTML
                        var dataArr = strArr[i].split("  ");
                        var title = dataArr[1].split("(")[0];
                        var leavl = dataArr[3].substr(0, 1);
                        var subTitle = (dataArr[1].match(/\((.+?)\)/g).join()).replace("(", "").replace(")", ""); //提取括号中的时间
                        var subContent1 = dataArr[5]; //简略内容
                        var subContent2 = ""; //dataArr[5]; //详细内容
                        //如果后一条记录中的title与前一条记录中的title相同就只添加上午下午的div，否则两个都追加
                        if (title != titleFlag) {
                            //追加两个，即，连标题一起追加（重新追加）
                            var html2 = "<div class='row row" + iconIndex + "'><div class='col-xs-12 col-md-6'><div class='life-content'><div class='outline'><img src='" + iconUrl[iconIndex] + "' /><span class='font-lg'>" + title + "</span></div><div class='item level-4-" + leavl + "'><div class='row font-md item-row'><div class='col-xs-2 text-center'>" + subTitle + "</div><div class='col-xs-10'>" + subContent1 + "</div><div class='col-xs-6'>" + subContent2 + "</div></div><hr class='divider'></div><div class='level-hint'><div class='row font-xs'><span class='state-4-1'>&nbsp;&nbsp;&nbsp;&nbsp;</span><span class='padding-left'>"
                            html2 = returnHtmlStr(title, html2)
                            $("body .container").append(html2);

                            //执行完之后，iconUrl加1
                            iconIndex = iconIndex + 1;
                        } else {
                            //只追加title包含的内容
                            var html1 = "<div class='item level-4-" + leavl + "'><div class='row font-md item-row'><div class='col-xs-2 text-center'>" + subTitle + "</div><div class='col-xs-10'>" + subContent1 + "</div><div class='col-xs-6'>" + subContent2 + "</div></div><hr class='divider'></div>";
                            var className = ".row" + (iconIndex - 1);
                            $("body .container " + className + " .level-hint").before(html1);
                        }
                        titleFlag = title;
                    }
                }
            } else { alert("txt文件中的数据为空") }

        },
        error: function (resData) {
            console.log(resData.responseText);
        }
    });
}

function returnHtmlStr(type, html) {
    //图例
    var zszs = ["不易中暑", "较易中暑", "容易中暑", "极易中暑"]
    var hwhd = ["非常适宜", "适宜", "不太适宜", "不适宜"]
    var cyzs = ["适宜穿薄短袖类", "适宜穿单衣类", "适宜穿夹克类", "适宜穿棉衣和羽绒类"]
    var tgzs = ["较适宜", "较凉", "冷", "寒冷"]
    var xszs = ["非常适宜", "适宜", "不太适宜", "不适宜"]
    var ktkq = ["建议不开启", "建议短时间开启", "建议开启", "建议开启"]
    var arrName=""

    if (type.indexOf("中暑指数") > -1) {
        arrName = zszs;
    } else if (type.indexOf("户外活动") > -1) {
        arrName = hwhd;
    } else if (type.indexOf("穿衣指数") > -1) {
        arrName = cyzs;
    } else if (type.indexOf("体感指数") > -1) {
        arrName = tgzs;
    } else if (type.indexOf("洗晒指数") > -1) {
        arrName = xszs;
    } else if (type.indexOf("空调开启") > -1) {
        arrName = ktkq;
    } else {
        arrName = "zszs";
    }
    if (arrName != "") {
        html = html + arrName[0] + "&nbsp;&nbsp;</span><span class='state-4-2'>&nbsp;&nbsp;&nbsp;&nbsp;</span><span class='padding-left'>" + arrName[1] + "&nbsp;&nbsp;</span><span class='state-4-3'>&nbsp;&nbsp;&nbsp;&nbsp;</span><span class='padding-left'>" + arrName[2] + "&nbsp;&nbsp;</span><span class='state-4-4'>&nbsp;&nbsp;&nbsp;&nbsp;</span><span class='padding-left'>" + arrName[3] + "&nbsp;&nbsp;</span></div></div></div></div></div>";
    } else {
        html = html + "&nbsp;&nbsp;</span><span class='state-4-2'>&nbsp;&nbsp;&nbsp;&nbsp;</span><span class='padding-left'>&nbsp;&nbsp;</span><span class='state-4-3'>&nbsp;&nbsp;&nbsp;&nbsp;</span><span class='padding-left'>&nbsp;&nbsp;</span><span class='state-4-4'>&nbsp;&nbsp;&nbsp;&nbsp;</span><span class='padding-left'>&nbsp;&nbsp;</span></div></div></div></div></div>";
    }

    return html;
}