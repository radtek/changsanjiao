
var result = {
    "childgm": {
        "type": [{ "text": "Children" }, { "text": "cold risk"}]
        , "img": "images/child_cold.png"
        , "val": [{ "one": "", "two": "", "three": "", "four": "", "five": "" }, { "one": "", "two": "", "three": "", "four": "", "five": ""}]
    }
    , "childxc": {
        "type": [{ "text": "Children" }, { "text": "asthma risk"}]
        , "img": "images/child_asthma.png"
        , "val": [{ "one": "", "two": "", "three": "", "four": "", "five": "" }, { "one": "", "two": "", "three": "", "four": "", "five": ""}]
    }
    , "adultgm": {
        "type": [{ "text": "Adult" }, { "text": "cold risk"}]
        , "img": "images/adult_cold.png"
        , "val": [{ "one": "", "two": "", "three": "", "four": "", "five": "" }, { "one": "", "two": "", "three": "", "four": "", "five": ""}]
    }
    , "oldgm": {
        "type": [{ "text": "Elderly" }, { "text": "cold risk"}]
        , "img": "images/old_cold.png"
        , "val": [{ "one": "", "two": "", "three": "", "four": "", "five": "" }, { "one": "", "two": "", "three": "", "four": "", "five": ""}]
    }
    , "mxfb": {
        "type": [{ "text": "COPD" }, { "text": "risk"}]
        , "img": "images/lung_disease.png"
        , "val": [{ "one": "", "two": "", "three": "", "four": "", "five": "" }, { "one": "", "two": "", "three": "", "four": "", "five": ""}]
    }
}
var vm=null;
window.onload = function () {
    vm= new Vue({
        el: "#page"
        , data: {
            date: new Date().Format("yyyy-MM-dd")
            , day: { "one": "", "two": "" }
            , "result": result
            , mlIs1: "true"
        }
        , mounted: function () {
            this.$nextTick(function () {
                this.init();
            })
        }
        , methods: {
            init: function () {
                var that = this;
                if (true) { }
                this.$http.post('WeChat.aspx/GetData').then(function (response) {
                    var nowDate = new Date().Format("yyyy/MM/dd");
                    var data = JSON.parse(response.data.d);
                    var date1 = data.json[0][2].split(' ')[0];    //第一个时间  这里只取一个时间，其他时间与这个相同
                    var copd1 = data.json[0][3];
                    var copd2 = data.json[1][3];
                    var childgm1 = data.json[2][3];
                    var childgm2 = data.json[3][3];
                    var childxc1 = data.json[4][3];
                    var childxc2 = data.json[5][3];
                    var oldgm1 = data.json[6][3];
                    var oldgm2 = data.json[7][3];
                    var adultgm1 = data.json[8][3];
                    var adultgm2 = data.json[9][3];
                    result.mxfb.val = [{ "one": copd1, "two": copd1, "three": copd1, "four": copd1, "five": copd1 }, { "one": copd2, "two": copd2, "three": copd2, "four": copd2, "five": copd2}];
                    result.adultgm.val = [{ "one": adultgm1, "two": adultgm1, "three": adultgm1, "four": adultgm1, "five": adultgm1 }, { "one": adultgm2, "two": adultgm2, "three": adultgm2, "four": adultgm2, "five": adultgm2}];
                    result.childgm.val = [{ "one": childgm1, "two": childgm1, "three": childgm1, "four": childgm1, "five": childgm1 }, { "one": childgm2, "two": childgm2, "three": childgm2, "four": childgm2, "five": childgm2}];
                    result.childxc.val = [{ "one": childxc1, "two": childxc1, "three": childxc1, "four": childxc1, "five": childxc1 }, { "one": childxc2, "two": childxc2, "three": childxc2, "four": childxc2, "five": childxc2}];
                    result.oldgm.val = [{ "one": oldgm1, "two": oldgm1, "three": oldgm1, "four": oldgm1, "five": oldgm1 }, { "one": oldgm2, "two": oldgm2, "three": oldgm2, "four": oldgm2, "five": oldgm2}];
                    var days = parseInt(((new Date(date1)).getTime() - (new Date(nowDate)).getTime()) / (1000 * 60 * 60 * 24));
                    if (days == 0) {       //获取的日期与当前的时间做对比，差值如果是0则是今天和明天，否则。。。
                        that.day = { "one": "Today", "two": "Tomorrow" };

                    } else {
                        that.day = { "one": "Tomorrow", "two": "AfterTmr" };
                    }
                    if (that.day.one == "Today") {
                        that.mlIs1 = true;
                    } else {
                        that.mlIs1 = false;
                    }
                }, function (response) {
                    //alert("获取数据失败");
                });
            },
            wid: function () {
                if ($(".one label").text() == "Today") {
                    $(".ml").css("margin-left", "10px")
                }
            }
        }
    });
};
Vue.component("my-component", {
    template: "#template",
    props: {
        type: Array,
        imgs: String,
        day: Object,
        val: Array,
        sty:String
    }
});
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}