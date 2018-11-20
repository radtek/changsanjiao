Ext.onReady(function () {
    var entityList = "AQIAM&AQINM&AQIPM&PM25WHF&YJ&AQIDay";
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.Forecast', 'IndexData'),
        params: { entityList: entityList },
        success: function (response) {
            var result = Ext.util.JSON.decode(response.responseText);
            for (var obj in result) {
                if (result[obj] != "") {
                    $("#" + obj).attr("src", result[obj]);
                }
            }                
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
});