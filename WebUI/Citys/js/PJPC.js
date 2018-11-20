// JScript 文件
Ext.onReady(function () {
      initInputHighlightScript();
      //默认进入的时候点击查询按钮所做的查询
      doQueryChart();
})

function doQueryChart() {
    var fromDate ="2016-12-27 20:00:00";
    var toDate = "2016-12-28 13:00:00"
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.CitysForecast', 'GetAQIChartDBPJPC'),
        params: { forecastDate: fromDate, ybDate: toDate },
        success: function (response) {
            if (response.responseText != "") {
                var x = response.responseText.toString().split('#');
                for (var y in x) {
                    var v1, v2;
                    v1 = x[y].toString().split(':')[0];
                    v2 = x[y].toString().split(':')[1];
                    document.getElementById(v1).innerHTML = v2;
                }

            }
        },
        failure: function (response) {
        }
    });
}

function ConvertToFloat(x) {
  if (x != "null") {
        var floatTemp = parseFloat(x).toFixed(1);
        var floatValue = parseFloat(floatTemp);
        if(floatValue>0)
            return floatValue;
        else
            return null;
    }
    else
        return null;
}