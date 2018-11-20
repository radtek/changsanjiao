var oldID = "L0";
Ext.onReady(function () {
    initInputHighlightScript();
    supportInnerText(); //使得火狐支持innerText
    CreateHtml();
})
function CreateHtml() {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.Forecast', 'GetEntity'),
        params: { entityName: id },
        success: function (response) {
            var result = Ext.util.JSON.decode(response.responseText);
            var entity = {
                align: "U",
                alias: "",
                realtime: "",
                name: ""
            }
            var selectTime = Ext.getDom("selectTime");
            selectTime.innerHTML = "";
            var ImageFrameEntity = new ImageFrame(result, entity);
            ImageFrameEntity.render("selectTime");
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
//改变日期成功后,，刷新获取的值
function changeDateSucessed(result) {
    for (var obj in result) {
        if (obj == "src") {
            imageViewer.setImageSrc(result[obj], id, "");
        }
        else {
            divContaner = Ext.getDom(obj);
            if (obj == "period")
                divContaner.className = "hourBut";
            if (divContaner != null) {
                if (divContaner.tagName == "INPUT" || divContaner.tagName == "TEXTAREA")
                    divContaner.value = result[obj];
                else {
                    if (result[obj] == "")
                        divContaner.innerHTML = "\\"; //日平均值
                    else
                        divContaner.innerHTML = result[obj]; //日平均值


                }
            }
        }
    }
}