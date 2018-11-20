$(function () {
    $("#TimeView").height($(window).height());

    $("#move").on({
        mouseenter:
                function () {
                    $(this).attr("class", this.className.replace('_D', '_V'));
                },
        mouseleave:
                function () {
                    $(this).attr("class", this.className.replace('_V', '_D'));
                }
    });
    $("#ImageView").width($(window).width() - 170);
    var result = $("#result").html();
    var id = $("#id").html();
    var idInfo = id.split("|");

    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.Forecast', 'GetEntity'),
        params: { entityName: idInfo[0] },
        success: function (response) {
            var result = Ext.util.JSON.decode(response.responseText);
            var entity = {
                align: idInfo[1],
                alias: idInfo[2],
                realtime: idInfo[3],
                name: idInfo[4],
                httpUrl: ''
            }
            ImageFrame(result, entity);
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
});

