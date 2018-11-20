var win;
//界面上多个图片选中替换的那个的ID
var selImgId = "";
Ext.onReady(function () {
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    userName = logResult["Alias"];
    $("#forecaster").html(logResult["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");
    $("#productType").text("重要天气专报");
    $("#PO_docDate").val(getFormatDate("") + "17时");


    //获取主要城市的数据
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetMainCityForecastData'),
        //params: { data: cells, strForecastDate: "", strPeriod: "24" },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                for (var obj in result) {
                    $("#PO_" + obj).val(result[obj]);
                }
            }
        },
        failure: function (response) {
            //Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });

    if (!win) {//如果不存在win对象择创建
//        win = new Ext.Window({
//            title: '图片选择',
//            width: 700,
//            height: 500,
//            layout: 'fit', //设置窗口内部布局
//            closeAction: 'hide',
//            plain: true, //true则主体背景透明，false则和主体背景有些差别
//            collapsible: true, //是否可收缩
//            modal: true, //是否为模式窗体
//            items: new Ext.Panel({//窗体中中是一个一个TabPanel
//                autoTabs: true,
//                activeTab: 0,
//                deferredRender: false,
//                border: false,
//                buttonAlign: "center",
//                items: [
//                        {
//                            id: "imgSel",
//                            html: '<div><div id="recImg" class="recImg"><img id="sourceImg" class="sourceImg" src="../css/images/noImg.GIF" alt=""></div><div class="selImgContent"><img id="selDisImg" class="selDisImg"  src="../css/images/noImg.GIF" alt=""><input id="selImgFromLocal" class="selLocalBtn" type="file" onchange="loadImageFile();" /><div type="submit" id="replaceImgOK" class="replaceBtn">确定</div></div>'
//                        }
//                    ]
//            }),
//            buttons: [
//                    ]
        //        });
        win = new Ext.Window({
            title: '图片选择',
            width: 700,
            height: 500,
            layout: 'fit', //设置窗口内部布局
            closeAction: 'hide',
            plain: true, //true则主体背景透明，false则和主体背景有些差别
            collapsible: true, //是否可收缩
            modal: true, //是否为模式窗体
            contentEl: "upLoadFileArea"
        });
    }


    //保存按钮，可根据状态表显示为发布等
    $("#foreSave").click(function () {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveFutureTenDaysWord'),
            params: { wordTempContent: getWordContent(), productName: "ImportWeather" },
            success: function (response) {
                if (response.responseText == "success") {
                    alert("保存成功");
                    $("#forePreview").show();
                    $("#forePub").show();
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    })


    //一键查询
    $("#clickQuery").click(function () {

        var searchDate = $("#searchDate").val();
        if (searchDate == "") {
            alert("请选择查询日期！");
            return;
        }
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'QueryWordWithYearAndIssue'),
            params: { productName: "ImportWeather", searchDate: searchDate },
            success: function (response) {
                if (response.responseText != "") {
                    var result = Ext.util.JSON.decode(response.responseText);
                    for (var obj in result) {
                        $("#" + obj).val(result[obj])
                    }
                    alert("查询获取成功！");
                }
                else {
                    alert("获取失败！");
                }
            },
            failure: function (response) {
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
      
    })

    //读取模板
    $("#readMOdel").click(function () {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'ReadPreviewModelModel'),
            params: { productName: "ImportWeather" },
            success: function (response) {
                if (response.responseText != "") {
                    var content = s = response.responseText.replace('\n', '');
                    var result = Ext.util.JSON.decode(response.responseText);
                    for (var obj in result) {
                        $("#" + obj).val(result[obj])
                    }
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    })


    //发布
    $("#forePub").click(function () {
        var ftpString = $("#FtpCollection").val();
        var issueNum = $("#PO_issueNum").text();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'PublishFutureTenDaysWord'),
            params: { ftpString: ftpString, functionName: "ImportWeather", issueNum: issueNum, userName: userName },
            success: function (response) {
                if (response.responseText == "success") {
                    alert("发布成功！");
                }
                else {
                    alert("发布失败！");
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });

    //历史记录，读取上一次存储的word内容json文本，替换到页面上，但是日期是最新的

    $("#getHihstory").click(function () {
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetWordHistory'),
            params: { productName: "ImportWeather" },
            success: function (response) {
                if (response.responseText != "") {
                    var result = Ext.util.JSON.decode(response.responseText);
                    for (var obj in result) {
                        $("#" + obj).val(result[obj])
                    }
                }
                else {
                    alert("获取历史失败！");
                }
            },
            failure: function (response) {
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });
    });


    //点击图片，弹出图片选择界面
    //    $("#PO_tomHazeImg").click(function () {
    //        win.show();
    //    })

//    $('#replaceImgOK').live('click', function () {
//        //        $("#PO_tomHazeImg").attr("src", $("#selDisImg").attr("src"));
//        //        $("#" + selImgId).attr("src", $("#selDisImg").attr("src"));
//        //file类型的input选中的文件路径
//        if (selectImgPath != "") {

//            var picDis = document.getElementById(selImgId);
//            $("#" + selImgId).width("280px");
//            $("#" + selImgId).attr("src", "");
//            //            picDis.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='image',src=\"" + selectImgPath + "\")";
//            picDis.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale',src=\"" + selectImgPath + "\")";
//            //            $("#" + selImgId).height();


//            var picSoureDis = document.getElementById("sourceImg");
//            picSoureDis.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale',src=\"" + selectImgPath + "\")";

//            //            $("#" + selImgId).attr("src", selectImgPath);
//            //            $("#sourceImg").attr("src", selectImgPath);
//        }
//        else {
//            $("#" + selImgId).attr("src", $("#selDisImg").attr("src"));
//            $("#sourceImg").attr("src", $("#selDisImg").attr("src"));
//        }
//        //        $("#sourceImg").attr("src", $("#selDisImg").attr("src"));
//        $("#selDisImg").attr("src", "")
//        if (win) {
//            win.hide();
//        }
//    });

//    $.each($(".selImg"), function (i, n) {
//        $(n).click(function () {
//            selImgId = $(n).next()[0].id;

//            //                        $("#selDisImg").attr("src", $($(n).next()[0]).attr("src"));
//            $("#selDisImg").attr("src", "../css/images/noImg.GIF");
//            $("#sourceImg").attr("src", $($(n).next()[0]).attr("src"));

//            win.show();
//            //$("#selDisImg").attr("src", $($(".scrollImg")[i]).attr("src"))

//        });
    //    });

    $('#replaceImgOK').live('click', function () {
        //        $("#PO_tomHazeImg").attr("src", $("#selDisImg").attr("src"));
        //        $("#" + selImgId).attr("src", $("#selDisImg").attr("src"));
        //file类型的input选中的文件路径
        if (selectImgPath != "") {

            var picDis = document.getElementById(selImgId);
            $("#" + selImgId).width("280px");
            $("#" + selImgId).attr("src", "");
            //            picDis.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='image',src=\"" + selectImgPath + "\")";
            picDis.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale',src=\"" + selectImgPath + "\")";
            //            $("#" + selImgId).height();


            var picSoureDis = document.getElementById("sourceImg");
            picSoureDis.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale',src=\"" + selectImgPath + "\")";

            //            $("#" + selImgId).attr("src", selectImgPath);
            //            $("#sourceImg").attr("src", selectImgPath);
        }
        else {
            $("#" + selImgId).attr("src", $("#selDisImg").attr("src"));
            $("#sourceImg").attr("src", $("#selDisImg").attr("src"));
        }
        //        $("#sourceImg").attr("src", $("#selDisImg").attr("src"));
        $("#selDisImg").attr("src", "")
        if (win) {
            win.hide();
        }
    });

    $.each($(".selImg"), function (i, n) {
        $(n).click(function () {
            selImgId = $(n).next()[0].id;
            $("#SelImgID").val($(n).next()[0].id);
            //                        $("#selDisImg").attr("src", $($(n).next()[0]).attr("src"));
            //$("#selDisImg").attr("src", "../css/images/noImg.GIF");
            $("#sourceImg").attr("src", $($(n).next()[0]).attr("src"));
            win.show();
            //$("#selDisImg").attr("src", $($(".scrollImg")[i]).attr("src"))

        });
    });

});


//将页面上的内容传给PageOffice Word预览页面
function prepare() {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'SaveWordContentToText'),
        params: { wordPartContent: getWordContent(), pruductFileName: "ImportWeather" },
        success: function (response) {
            if (response.responseText == "success") {
                alert("暂存成功");
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function getWordContent() {
    var pageContent = "";
    pageContent += "PO_year" + "=" + $("#PO_year").val() + "&";
    pageContent += "PO_issueNum" + "=" + $("#PO_issueNum").val() + "&";
    pageContent += "PO_docDate" + "=" + $("#PO_docDate").val() + "&";
    pageContent += "PO_TodayDay" + "=" + $("#PO_TodayDay").val() + "&";
    pageContent += "PO_SHWeatherSys" + "=" + $("#PO_SHWeatherSys").val() + "&";

    pageContent += "PO_TomHazeDropZone" + "=[image]" + $("#PO_TomHazeDropZone").attr("src") + "[/image]&";
    pageContent += "PO_AfterHazeDropZone" + "=[image]" + $("#PO_AfterHazeDropZone").attr("src") + "[/image]&";

    pageContent += "PO_ImgAnno1" + "=" + $("#PO_ImgAnno1").val() + "&";
    pageContent += "PO_ImgAnno2" + "=" + $("#PO_ImgAnno2").val() + "&";
    pageContent += "PO_TomAQIDropZone" + "=[image]" + $("#PO_TomAQIDropZone").attr("src") + "[/image]&";
    pageContent += "PO_AfterAQIDropZone" + "=[image]" + $("#PO_AfterAQIDropZone").attr("src") + "[/image]&";
    
    pageContent += "PO_TableTitle1" + "=" + $("#PO_TableTitle1").val() + "&";
    var cityArray = ["Shanghai", "Nanjing", "Suzhou", "Hangzhou", "Ningbo", "Hefei", "Fuzhou", "Xiamen", "Nanchang", "Jinan", "Qingdao"];
    for (var i = 0; i < cityArray.length; i++) {
        pageContent += "PO_PoLevel_" + cityArray[i] + "=" + $("#" + "PO_PoLevel_" + cityArray[i]).val() + "&";
        pageContent += "PO_FirstItem_" + cityArray[i] + "=" + $("#" + "PO_FirstItem_" + cityArray[i]).val() + "&";
        pageContent += "PO_AQI_" + cityArray[i] + "=" + $("#" + "PO_AQI_" + cityArray[i]).val() + "&";
        pageContent += "PO_AirPolLevel_" + cityArray[i] + "=" + $("#" + "PO_AirPolLevel_" + cityArray[i]).val() + "&";
        pageContent += "PO_Haze_" + cityArray[i] + "=" + $("#" + "PO_Haze_" + cityArray[i]).val() + "&";
    }
    pageContent += "PO_ForeText" + "=" + $("#PO_ForeText").val() + "&";
    var cityArray = ["Shanghai", "Nanjing", "Suzhou", "Hangzhou", "Ningbo", "Hefei", "Fuzhou", "Xiamen", "Nanchang", "Jinan", "Qingdao"];
    for (var i = 0; i < cityArray.length; i++) {
        pageContent += "PO_PoLevel_" + cityArray[i] + "=" + $("#" + "PO_PoLevel_" + cityArray[i]).val() + "&";
        pageContent += "PO_FirstItem_" + cityArray[i] + "=" + $("#" + "PO_FirstItem_" + cityArray[i]).val() + "&";
        pageContent += "PO_AQI_" + cityArray[i] + "=" + $("#" + "PO_AQI_" + cityArray[i]).val() + "&";
        pageContent += "PO_AirPolLevel_" + cityArray[i] + "=" + $("#" + "PO_AirPolLevel_" + cityArray[i]).val() + "&";
        pageContent += "PO_Haze_" + cityArray[i] + "=" + $("#" + "PO_Haze_" + cityArray[i]).val() + "&";
    }
    pageContent += "PO_reportFocus" + "=" + $("#PO_reportFocus").val() + "&";
    pageContent += "PO_ImgTwoLeft" + "=[image]" + $("#PO_ImgTwoLeft").attr("src") + "[/image]&";
    pageContent += "PO_ImgTwoRight" + "=[image]" + $("#PO_ImgTwoRight").attr("src") + "[/image]&";    
    pageContent += "PO_ImgAnno3" + "=" + $("#PO_ImgAnno3").val() + "&";
    pageContent += "PO_ImgThreeLeft" + "=[image]" + $("#PO_ImgThreeLeft").attr("src") + "[/image]&";
    pageContent += "PO_ImgThreeRight" + "=[image]" + $("#PO_ImgThreeRight").attr("src") + "[/image]&";
    pageContent += "PO_ImgAnno4" + "=" + $("#PO_ImgAnno4").val() + "&";
    pageContent += "PO_SendOras" + "=" + $("#PO_SendOras").val() + "&";
    pageContent += "PO_reportTo" + "=" + $("#PO_reportTo").val() + "&";
    

    pageContent += "PO_editor" + "=" + $("#PO_editor").val() + "&";
    pageContent += "PO_reporter" + "=" + $("#PO_reporter").val() + "&";


    pageContent += "PO_editor" + "=" + $("#PO_editor").val() + "&";
    pageContent += "PO_Sign" + "=" + $("#PO_Sign").val();
    return pageContent;
}

//预报时间改变是的事件
function changeDate(el) {
    var value = el.value;
    $("#docDate").text(value);
}


//点击图片弹出替换图片的窗体
function setReplaceImgWinContent() {

}

var loadImageFile = (function () {
    if (window.FileReader) {
        var oPreviewImg = null, oFReader = new window.FileReader(),
rFilter = /^(?:image\/bmp|image\/cis\-cod|image\/gif|image\/ief|image\/jpeg|image\/jpeg|image\/jpeg|image\/pipeg|image\/png|image\/svg\+xml|image\/tiff|image\/x\-cmu\-raster|image\/x\-cmx|image\/x\-icon|image\/x\-portable\-anymap|image\/x\-portable\-bitmap|image\/x\-portable\-graymap|image\/x\-portable\-pixmap|image\/x\-rgb|image\/x\-xbitmap|image\/x\-xpixmap|image\/x\-xwindowdump)$/i;

        oFReader.onload = function (oFREvent) {
            //            if (!oPreviewImg) {
            //                var newPreview = document.getElementById("recImg");
            //                oPreviewImg = new Image();
            //                oPreviewImg.style.width = (newPreview.offsetWidth).toString() + "px";
            //                oPreviewImg.style.height = (newPreview.offsetHeight).toString() + "px";
            //                newPreview.appendChild(oPreviewImg);
            //            }
            //            var url = decodeURI(oFREvent.target.result)

            $("#selDisImg").attr("src", oFREvent.target.result)
            //oPreviewImg.src = oFREvent.target.result;
        };
        //        var aFiles = document.getElementById("selImgFromLocal").files;
        //        if (aFiles.length === 0) { return; }
        //        if (!rFilter.test(aFiles[0].type)) { alert("You must select a valid image file!"); return; }
        //        oFReader.readAsDataURL(aFiles[0]);
        //        $("#selDisImg").attr("src", aFiles[0])
        return function () {
            var aFiles = document.getElementById("selImgFromLocal").files;
            if (aFiles.length === 0) { return; }
            if (!rFilter.test(aFiles[0].type)) { alert("You must select a valid image file!"); return; }
            oFReader.readAsDataURL(aFiles[0]);
            //oFReader.readAsArrayBuffer(aFiles[0]);
            //oFReader.readAsBinaryString(aFiles[0]);
        }

    }
    if (navigator.appName === "Microsoft Internet Explorer") {
        return function () {
            //            //document.getElementById("recImg").filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = document.getElementById("selImgFromLocal").value;
            //            var file_upl = document.getElementById('selImgFromLocal');
            //            file_upl.select();
            //            var realpath = document.selection.createRange().text;
            //            //$("#selDisImg").attr("src", document.getElementById("selImgFromLocal").value);
            //            $("#selDisImg").attr("src", realpath);

            var pic = document.getElementById("selDisImg");
            var file = document.getElementById("selImgFromLocal");
            file.select();
            var reallocalpath = document.selection.createRange().text//IE下获取实际的本地文件路径
            if (window.ie6) pic.src = reallocalpath; //IE6浏览器设置img的src为本地路径可以直接显示图片
            else { //非IE6版本的IE由于安全问题直接设置img的src无法显示本地图片，但是可以通过滤镜来实现，IE10浏览器不支持滤镜，需要用FileReader来实现，所以注意判断FileReader先
                pic.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale',src=\"" + reallocalpath + "\")";
                selectImgPath = reallocalpath;
                //                pic.style.height = "300px";
                //                pic.style.width = "240px";
                pic.src = 'data:image/gif;base64,R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw=='; //设置img的src为base64编码的透明图片，要不会显示红xx
            }
        }
    }
})();

function getObjectURL(file) {
    var url = null;
    if (window.createObjectURL != undefined) { // basic
        url = window.createObjectURL(file);
    } else if (window.URL != undefined) { // mozilla(firefox)
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) { // webkit or chrome
        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}

function preViewImg(obj) {
    if (obj) {
        if (window.URL) {
            document.getElementById("selDisImg").src = window.URL.createObjectURL(obj.files[0]);
        }
        else {
            document.getElementById("selDisImg").style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale',src=\"" + obj.value + "\")";
        }
    }
}

