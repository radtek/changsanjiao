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
    $("#PO_year").val(new Date().getFullYear());
    $("#PO_docDate").val(getFormatDate("") + "16时");
    $("#productType").text("一周污染天气展望");
    $(".contentTextBox").each(function () {
        this.style.height = this.scrollHeight + 'px';
    });
    //    $(".contentTextBox").bind({
    //        input: function () {
    //            this.style.height = this.scrollHeight + 'px';
    //        },
    //        propertychange: function () {
    //            this.style.height = this.scrollHeight + 'px';
    //        }
    //    });
    //获取当天预报的未来三天霾图片

    //设置表格内和图片标注的日期
    $("#PO_mondayDate").val(GetDateStrNoYear(0));
    $("#PO_tueDate").val(GetDateStrNoYear(1));
    $("#PO_wedDate").val(GetDateStrNoYear(2));
    $("#PO_thurDate").val(GetDateStrNoYear(3));
    $("#PO_friDate").val(GetDateStrNoYear(4));
    $("#PO_saturDate").val(GetDateStrNoYear(5));
    $("#PO_sunDate").val(GetDateStrNoYear(6));

    //设置星期几
    $("#PO_monWeekday").val(GetDateWeekDay(0));
    $("#PO_tueWeekday").val(GetDateWeekDay(1));
    $("#PO_wedWeekday").val(GetDateWeekDay(2));
    $("#PO_thurWeekday").val(GetDateWeekDay(3));
    $("#PO_friWeekday").val(GetDateWeekDay(4));
    $("#PO_saturWeekday").val(GetDateWeekDay(5));
    $("#PO_sunWeekday").val(GetDateWeekDay(6));

    $("#PO_ImgAnno1").val($("#PO_ImgAnno1").val().replace('{PastStart}', GetDateStrNoYear(-6)));
    $("#PO_ImgAnno1").val($("#PO_ImgAnno1").val().replace('{PastEnd}', GetDateStrNoYear(0)));

    $("#PO_ImgAnno2").val($("#PO_ImgAnno2").val().replace('{FutureStart}', GetDateStrNoYear(0)));
    $("#PO_ImgAnno2").val($("#PO_ImgAnno2").val().replace('{FutureEnd}', GetDateStrNoYear(6)));

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
        //                            html: '<div><div id="recImg" class="recImg"><img id="selDisImg" class="selDisImg" style="height:300px;" src="../css/images/noImg.GIF" alt=""></div><div class="selBtn"><div class="selImgArea"><input id="selImgFromLocal" class="selLocalBtn" type="file" onchange="loadImageFile();" /></div><div type="submit" id="replaceImgOK" class="replaceBtn">确定</div></div>'
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
            params: { wordTempContent: getWordContent(), productName: "WeekPolWeather" },
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
            params: { productName: "WeekPolWeather", searchDate: searchDate },
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
            params: { productName: "WeekPolWeather" },
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
        var issueNum = $("#PO_issueNum").val();
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.AQIForecast', 'PublishFutureTenDaysWord'),
            params: { ftpString: ftpString, functionName: "WeekPolWeather", issueNum: issueNum, userName: userName },
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
            params: { productName: "WeekPolWeather" },
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


    if ($.browser.msie) {
        //实况值改变 
        $("#issueSet").get(0).attachEvent("onpropertychange", function (o) {
            $("#PO_issueNum").val($("#issueSet").val());
        });

    }
    else {
        //实况值改变
        $("#issueSet").get(0).addEventListener("input", function (o) {
            $("#PO_issueNum").val($("#issueSet").val());
        }, false);
    }



    //点击图片，弹出图片选择界面
    //    $("#PO_tomHazeImg").click(function () {
    //        win.show();
    //    })

    //    $('#replaceImgOK').live('click', function () {
    //        //        $("#PO_tomHazeImg").attr("src", $("#selDisImg").attr("src"));
    //        $("#" + selImgId).attr("src", $("#selDisImg").attr("src"));
    //        if (win) {
    //            win.hide();
    //        }
    //    });

    //    $.each($(".selImg"), function (i, n) {
    //        $(n).click(function () {
    //            selImgId = $(n).next()[0].id;
    //            $("#selDisImg").attr("src", $($(n).next()[0]).attr("src"))
    //            win.show();
    //        });
    //    });

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

    //选择图片之后预览
    $('#replaceImgOK').live('click', function () {

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
        params: { wordPartContent: getWordContent(), pruductFileName: "WeekPolWeather" },
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
    pageContent += "PO_SHWeatherSys" + "=" + $("#PO_SHWeatherSys").val() + "&";

    pageContent += "PO_TomHazeDropZone" + "=[image]" + $("#PO_TomHazeDropZone").attr("src") + "[/image]&";
    pageContent += "PO_ImgAnno1" + "=" + $("#PO_ImgAnno1").val() + "&";
    pageContent += "PO_reportFocus" + "=" + $("#PO_reportFocus").val() + "&";
    pageContent += "PO_TomAQIDropZone" + "=[image]" + $("#PO_TomAQIDropZone").attr("src") + "[/image]&";
    pageContent += "PO_ImgAnno2" + "=" + $("#PO_ImgAnno2").val() + "&";
    //表格内日期
    pageContent += "PO_monWeekday" + "=" + $("#PO_monWeekday").val() + "&";
    pageContent += "PO_mondayDate" + "=" + $("#PO_mondayDate").val() + "&";
    pageContent += "PO_tueWeekday" + "=" + $("#PO_tueWeekday").val() + "&";
    pageContent += "PO_tueDate" + "=" + $("#PO_tueDate").val() + "&";
    pageContent += "PO_wedWeekday" + "=" + $("#PO_wedWeekday").val() + "&";
    pageContent += "PO_wedDate" + "=" + $("#PO_wedDate").val() + "&";
    pageContent += "PO_thurWeekday" + "=" + $("#PO_thurWeekday").val() + "&";
    pageContent += "PO_thurDate" + "=" + $("#PO_thurDate").val() + "&";
    pageContent += "PO_friWeekday" + "=" + $("#PO_friWeekday").val() + "&";
    pageContent += "PO_friDate" + "=" + $("#PO_friDate").val() + "&";
    pageContent += "PO_saturWeekday" + "=" + $("#PO_saturWeekday").val() + "&";
    pageContent += "PO_saturDate" + "=" + $("#PO_saturDate").val() + "&";
    pageContent += "PO_sunWeekday" + "=" + $("#PO_sunWeekday").val() + "&";
    pageContent += "PO_sunDate" + "=" + $("#PO_sunDate").val() + "&";
    
    //各个城市部分
    var cityArray = ["Shanghai", "Hefei", "Nanjing", "Suzhou", "Hangzhou", "Ningbo"];
    for (var i = 0; i < cityArray.length; i++) {
        pageContent += "PO_HazeMonday_" + cityArray[i] + "=" + $("#" + "PO_HazeMonday_" + cityArray[i]).val() + "&";
        pageContent += "PO_HazeTue_" + cityArray[i] + "=" + $("#" + "PO_HazeTue_" + cityArray[i]).val() + "&";
        pageContent += "PO_HazeWed_" + cityArray[i] + "=" + $("#" + "PO_HazeWed_" + cityArray[i]).val() + "&";
        pageContent += "PO_HazeThur_" + cityArray[i] + "=" + $("#" + "PO_HazeThur_" + cityArray[i]).val() + "&";
        pageContent += "PO_HazeFriday_" + cityArray[i] + "=" + $("#" + "PO_HazeFriday_" + cityArray[i]).val() + "&";
        pageContent += "PO_HazeSaturday_" + cityArray[i] + "=" + $("#" + "PO_HazeSaturday_" + cityArray[i]).val() + "&";
        pageContent += "PO_HazeSunday_" + cityArray[i] + "=" + $("#" + "PO_HazeSunday_" + cityArray[i]).val() + "&";        
    }


    pageContent += "PO_SendOras" + "=" + $("#PO_SendOras").val() + "&";
    pageContent += "PO_editor" + "=" + $("#PO_editor").val() + "&";
    pageContent += "PO_reporter" + "=" + $("#PO_reporter").val();      
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
            //document.getElementById("recImg").filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = document.getElementById("selImgFromLocal").value;
            $("#selDisImg").attr("src", document.getElementById("selImgFromLocal").value);
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

