// JScript 文件

var ImageFrameEntity;

var htmlStr="";

ImageProduct = function (center, node) {



    var leftPanel = new Ext.Panel({
        layout: 'fit',
        border: false,
        header: false,
        collapsible: false,
        listeners: {

    }

});
$.ajax({ url: getUrl('MMShareBLL.DAL.Forecast', 'GetLeftPanel'),
    type: 'get',
    cache: false,
    dataType: 'json',
    data: { node: node.id },
    success: function (data) {
        var theHtml = "", firstNodeText = "", secondNodeText = "", firstNodeTextNew = "", secondNodeTextNew = "";
        for (var i = 0; i < data.length; i++) {
            if (firstNodeText == "") {
                theHtml += "<h1 class='l1'>" + data[i].CLASS + "</h1>";
                theHtml += "<div class='slist'>";
                firstNodeText = data[i].CLASS;
                if (secondNodeText == "") {
                    theHtml += "<h2 class='l2'>" + data[i].MenuName + "</h2>";
                    theHtml += " <ul class='sslist'>";
                    theHtml += "<li  class='13'>" + data[i].HINT + "</li>";
                    secondNodeText = data[i].MenuName;
                }
            }
            else {
                firstNodeTextNew = data[i].CLASS;
                secondNodeTextNew = data[i].MenuName;
                if (firstNodeText != firstNodeTextNew) {
                    theHtml += "</ul></div>";
                    theHtml += "<h1 class='l1'>" + firstNodeTextNew + "</h1>";
                    theHtml += "<div class='slist'>";
                    theHtml += "<h2 class='l2'>" + secondNodeTextNew + "</h2>";
                    theHtml += "<ul class='sslist'>";
                    theHtml += "<li  class='13'>" + data[i].HINT + "</li>";
                }
                else {
                    if (secondNodeText != secondNodeTextNew) {
                        theHtml += "</ul>";
                        theHtml += "<h2 class='l2'>" + secondNodeTextNew + "</h2>";
                        theHtml += "<ul class='sslist'>";
                        theHtml += "<li  class='13'>" + data[i].HINT + "</li>";
                    }
                    else
                        theHtml += "<li  class='13'>" + data[i].HINT + "</li>";
                }

            }
        }
        var htmPanel = new Ext.Panel({
            border: false,
            header: false,
            layout: "fit",
            html: theHtml
        });
        leftPanel.add(htmPanel);
        leftPanel.doLayout();
//        var firstNode = $(".menu").first()[0].firstChild;
    },
    error: function (ex) {
        if (ex.statusText != "OK") alert("异°¨?常?ê，ê?请?检¨?查¨|！ê?");
    }
});



//    Ext.Ajax.request({
//        url: getUrl('MMShareBLL.DAL.Forecast', 'GetLeftPanel'),
//        params: { node: node.id },
//        success: function (response) {
//            htmlStr = response.responseText;

//            var htmPanel = new Ext.Panel({
//                border: false,
//                header: false,
//                layout: "fit",
//                html: htmlStr
//            });
//            leftPanel.add(htmPanel);
//            leftPanel.doLayout();

//        },
//        failure: function (response) {
//            Ext.MessageBox.show({
//                title: '操作提示',
//                msg: "连接服务器失败",
//                buttons: Ext.MessageBox.OK,
//                icon: Ext.MessageBox.ERROR
//            });
//        }
//    });

ImageProduct.superclass.constructor.call(this, {
    border: false,
    header: false,
    layout: "fit",
    layoutConfig: { animate: true },
    items: [leftPanel]
});


}


//继承，添加函数，或者重写函数
Ext.extend(ImageProduct, Ext.Panel, {

});




//注册，可以通过xtype:forecastPanel来创建面板
Ext.reg('imageProduct', ImageProduct); 
