Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    InitTable();
}
)
//获取鼠标按下时的值
function InitTable() {
    var dateTime = Ext.getDom("H00").value;
    var coutTable0 = Ext.getDom("coutTable0");
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'ReturnChinaScore'),
        params: { dateTime: dateTime },
        success: function (response) {
            if (response.responseText != "") {
                //coutTable0.innerHTML = response.responseText;
                $(".evaluate").remove();
                $("#coutTable0").append(response.responseText);
            }
            hideWrfRow();
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
//取消最后一行显示的wrf行
function hideWrfRow() {
    $(".tableRowOther").each(function (i, n) {
        if ($(n).text().indexOf("WRF")>-1) {
            $(n).hide();
        }
    });
}


var results = {
};
var vm = new Vue({
    el: '#page'
    , data: {
        skybtab: true
        , sk: false
        , yb:false
        , title: ["日期", "", "PM25", "PM10", "NO2", "O3-1", "O3-8"]
        , results: results
    }
    , methods: {
        query: function () {
            this.skybtab = true;
            var Type = document.getElementById("Type");
            if (this.yb == true) {                              //当前点击的是预报数据的按钮
                this.forecast();
            } else if (this.sk == true) {                      //当前显示的是实况数据的表格
                this.real();
            } else {                                                         //当前显示的是评分数据的表格
                InitTable();
            }
        }
        , evaluate: function () {
            InitTable();
            this.skybtab = true;
        }
        , real: function () {
            this.title = ["日期", "", "PM25", "PM10", "NO2", "O3-1", "O3-8"];
            $(".evaluate").hide();
            this.skybtab = false;
            this.sk = true;
            this.yb = false;
            var date = $("#H00").val();      //这里的日期是以前写好的，没有用Vue写
            this.$http.post('ChianScore.aspx/Real', { date: date }).then(function (response) {
                var data = eval('(' + response.data.d + ')');
                this.results = data;
            });
        }
        , forecast: function () {
            this.title = ["日期", "", "PM25", "PM10", "NO2", "O3"];
            this.yb = true;
            this.sk = false;
            $(".evaluate").hide();
            this.skybtab = false;
            var date = $("#H00").val();
            this.$http.post('ChianScore.aspx/Forecast', { date: date }).then(function (response) {
                var data = eval('(' + response.data.d + ')');
                this.results = data;
            });
        }
        , OutTable: function () {
            var Type = document.getElementById("Type");
            if (this.yb == true) {                              //当前实现的是预报数据的表格
                Type.setAttribute("value", "yb");
            } else if (this.sk == true) {                      //当前显示的是实况数据的表格
                Type.setAttribute("value", "sk");
            } else {                                                         //当前显示的是评分数据的表格
                Type.setAttribute("value", "pf");
            }
            var dateTime = Ext.getDom("H00").value;
            var content = dateTime;
            var Element = document.getElementById("Element");
            Element.setAttribute("value", content);
            document.getElementById("btnExport").click();
        }
    }
});