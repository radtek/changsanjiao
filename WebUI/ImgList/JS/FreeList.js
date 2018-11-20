
var curImg = "";
var imageViewer;
var oldArr = ["H04", "H03", "H02", "H01"]; 
Ext.onReady(function () {
    initInputHighlightScript();
    supportInnerText(); //使得火狐支持innerText
    $("#areaSelect").on("change", function () {
        var o;
        var value;
        var opt = $(this).find('option');
        opt.each(function (i) {
            if (opt[i].selected == true) {
                o = opt[i].innerHTML;
                value = opt[i].value;
            }
        })
        $(this).find('label').html(o);
        if (value != "")
            trickLayers(value);
    }).trigger('change');
    $("#layers").on("change", function () {
        var o;
        var value;
        var opt = $(this).find('option');
        opt.each(function (i) {
            if (opt[i].selected == true) {
                o = opt[i].innerHTML;
                value = opt[i].value;
            }
        })
        $(this).find('label').html(o);
        if (value != "")
            layersChange(value);
    }).trigger('change');
    $("#types").on("change", function () {
        var o;
        var value;
        var opt = $(this).find('option');
        opt.each(function (i) {
            if (opt[i].selected == true) {
                o = opt[i].innerHTML;
                value = opt[i].value;
            }
        })
        $(this).find('label').html(o);
        if (value != "")
            typesChange(value);
    }).trigger('change');
    $("#hour").on("change", function () {
        var o;
        var value;
        var opt = $(this).find('option');
        opt.each(function (i) {
            if (opt[i].selected == true) {
                o = opt[i].innerHTML;
                value = opt[i].value;
            }
        })
        $(this).find('label').html(o);
    }).trigger('change');

})
function ImgMouseOver(el) {
    var obj = Ext.getDom(el);
    var divUsers = Ext.getDom("delete");
    divUsers.style.left = getElementLeft(obj, divUsers.parent)+obj.offsetWidth-30+ "px";
    divUsers.style.top = getElementTop(obj, divUsers.parent) + "px";
    divUsers.className = "deleteImg";
    divUsers.style.display ="block";
    divUsers.style.zIndex = 100;
    curImg = el;

}
function ImgMouseOut() {
    var divUsers = Ext.getDom("delete");
    divUsers.className = "deleteImg";
    divUsers.style.display = "none";
    
}
function ImgDelete() {
    if (curImg != "") {
        var obj = Ext.getDom(curImg);
        oldArr.push(curImg);
        obj.src = "";
    }
    
}
function typesChange(type) {
    var stationId = $('#area option:selected')[0].value;
    if (Ext.getDom("layersID") == null)
        layers = "";
    else 
        layers = $('#layersID option:selected')[0].value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.FreeQuery', 'typeChange'),
        params: { stationId: stationId, layer: layers, type: type },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                changeDateSucessed(result);
                changeDate(Ext.getDom("time"));
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function layersChange(layer) {
    var stationId = $('#area option:selected')[0].value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.FreeQuery', 'LayersChange'),
        params: { stationId: stationId, layer: layer },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                changeDateSucessed(result);
                changeDate(Ext.getDom("time"));
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function trickLayers(stationId) {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.FreeQuery', 'trickLayers'),
        params: { stationId: stationId },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                changeDateSucessed(result);
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}
function addImgButton() {
    if (oldArr.length>0) {
        var area = $('#area option:selected')[0].value;
        var layers = $('#layersID option:selected')[0].value;
        var types = $('#typeID option:selected')[0].value;
        var forecastDate = Ext.getDom("time").value;
        var hour = $('#hourID option:selected')[0].value;
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.FreeQuery', 'QueryListButton'),
            params: { forecastDate: forecastDate, stationId: area, layers: layers, types: types, hour: hour },
            success: function (response) {
                if (response.responseText != "") {
                    var img = Ext.getDom(oldArr.pop());
                    img.src = "../"+response.responseText;
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    }
    else {
        Ext.Msg.alert("提示", "图片位置已经占满");
    }
}
function changeDate(el) {
    var area = $('#area option:selected')[0].value;
    if (Ext.getDom("layersID") == null)
        layers = "";
    else
        layers = $('#layersID option:selected')[0].value;
    if (Ext.getDom("typeID") == null)
        types = "";
    else
        types = $('#typeID option:selected')[0].value;
    var forecastDate = el.value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.FreeQuery', 'QueryImgList'),
        params: { forecastDate:forecastDate,stationId: area, layers: layers, types: types },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                changeDateSucessed(result);
            }
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