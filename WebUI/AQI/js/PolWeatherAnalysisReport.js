var win;
var selectImgPath = "";
//界面上多个图选中替换的那个的ID
var selImgId = "";
Ext.onReady(function () {
    var loginParams = getCookie("UserInfo");
    var logResult = Ext.util.JSON.decode(loginParams);
    userName = logResult["Alias"];
    $("#forecaster").html(logResult["Alias"]);
    $("#forecastTime").html(getNowFormatDate());
    $("#forecastTimeLevel").html("17时");
    $("#PO_year").val(new Date().getFullYear());

    $("#PO_docDate").val(getFormatDate("") + "17时");
    $("#productType").text("污染天气过程跟踪解析专报");
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
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetFutureThreeDayHazrDropImgs'),
        //        params: { forecastDate: forecastDate },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                for (var obj in result) {
                    $($(".Img_" + obj)[0]).attr("src", "../" + result[obj])
                }
            }
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
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
        //                            //html: '<div><div id="recImg" class="recImg"><img id="sourceImg" class="sourceImg" src="../css/images/noImg.GIF" alt=""></div><div class="selImgContent"><img id="selDisImg" class="selDisImg"  src="../css/images/noImg.GIF" alt=""><input id="selImgFromLocal" style="display:none;" class="selLocalBtn" type="file" onchange="loadImageFile();" /><asp:FileUpload ID="FileUpload1" class="upFile" runat="server" /><asp:Button ID="BtnUpload" class="upBtn" runat="server" Text="上传" OnClick="BtnUpload_Click" /><div type="submit" id="replaceImgOK" class="replaceBtn">确定</div></div>'
        //                             //html: '<div><div id="recImg" class="recImg"><img id="selDisImg" class="selDisImg" style="height:300px;" src="../css/images/noImg.GIF" alt=""></div><div class="selBtn"><div class="selImgArea"><input id="selImgFromLocal" class="selLocalBtn" type="file" onchange="loadImageFile(this);" /></div><div type="submit" id="replaceImgOK" class="replaceBtn">确定</div></div>'
        //                        }
        //                    ]
        //            }),
        //            buttons: [
        //                    ]
        //                });
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
            params: { wordTempContent: getWordContent(), productName: "PolWeatherAnalysis" },
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
            params: { productName: "PolWeatherAnalysis", searchDate: searchDate },
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
            params: { productName: "PolWeatherAnalysis" },
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
            params: { ftpString: ftpString, functionName: "PolWeatherAnalysis", issueNum: issueNum, userName: userName },
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
            params: { productName: "FutureTenDays" },
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
        params: { wordPartContent: getWordContent(), pruductFileName: "PolWeatherAnalysis" },
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
    pageContent += "PO_tomDate" + "=" + $("#PO_tomDate").val() + "&";
    pageContent += "PO_tomWeekDay" + "=" + $("#PO_tomWeekDay").val() + "&";
    pageContent += "PO_tomHaze" + "=" + $("#PO_tomHaze").val() + "&";
    pageContent += "PO_tomAirPol" + "=" + $("#PO_tomAirPol").val() + "&";
    pageContent += "PO_afterDate" + "=" + $("#PO_afterDate").val() + "&";
    pageContent += "PO_afterWeekday" + "=" + $("#PO_afterWeekday").val() + "&";
    pageContent += "PO_afterHaze" + "=" + $("#PO_afterHaze").val() + "&";
    pageContent += "PO_afterAirPol" + "=" + $("#PO_afterAirPol").val() + "&";
    pageContent += "PO_afterDate2" + "=" + $("#PO_afterDate2").val() + "&";
    pageContent += "PO_afterWeekday2" + "=" + $("#PO_afterWeekday2").val() + "&";
    pageContent += "PO_afterHaze2" + "=" + $("#PO_afterHaze2").val() + "&";
    pageContent += "PO_afterAirPol2" + "=" + $("#PO_afterAirPol2").val() + "&";
    pageContent += "PO_reportFocus" + "=" + $("#PO_reportFocus").val() + "&";
    pageContent += "PO_mainFocus" + "=" + $("#PO_mainFocus").val() + "&";
    pageContent += "PO_editor" + "=" + $("#PO_editor").val() + "&";
    pageContent += "PO_reporter" + "=" + $("#PO_reporter").val() + "&";
//    pageContent += "PO_tomHazeImg" + "=[image]" + $("#PO_tomHazeImg").attr("src") + "[/image]&";
//    pageContent += "PO_aferHazeImg" + "=[image]" + $("#PO_aferHazeImg").attr("src") + "[/image]&";
    //    pageContent += "PO_afterHazeImg2" + "=[image]" + $("#PO_afterHazeImg2").attr("src") + "[/image]";
    if ($("#PO_tomHazeImg").attr("src").indexOf("?V=") > 0) {
        pageContent += "PO_tomHazeImg" + "=[image]" + $("#PO_tomHazeImg").attr("src").substring(0, $("#PO_tomHazeImg").attr("src").indexOf("?V=")) + "[/image]&";
    }
    else {
        pageContent += "PO_tomHazeImg" + "=[image]" + $("#PO_tomHazeImg").attr("src") + "[/image]&";
    }

    if ($("#PO_aferHazeImg").attr("src").indexOf("?V=") > 0) {
        pageContent += "PO_aferHazeImg" + "=[image]" + $("#PO_aferHazeImg").attr("src").substring(0, $("#PO_aferHazeImg").attr("src").indexOf("?V=")) + "[/image]&";
    }
    else {
        pageContent += "PO_aferHazeImg" + "=[image]" + $("#PO_aferHazeImg").attr("src") + "[/image]&";
    }

    if ($("#PO_afterHazeImg2").attr("src").indexOf("?V=") > 0) {
        pageContent += "PO_afterHazeImg2" + "=[image]" + $("#PO_afterHazeImg2").attr("src").substring(0, $("#PO_afterHazeImg2").attr("src").indexOf("?V=")) + "[/image]";
    }
    else {
        pageContent += "PO_afterHazeImg2" + "=[image]" + $("#PO_afterHazeImg2").attr("src") + "[/image]";
    }

//    pageContent += "PO_tomHazeImg" + "=[image]" + $("#PO_tomHazeImg").attr("src") + "[/image]&";
//    pageContent += "PO_aferHazeImg" + "=[image]" + $("#PO_aferHazeImg").attr("src").substring(0, $("#PO_aferHazeImg").attr("src").indexOf("?V=")) + "[/image]&";
//    pageContent += "PO_afterHazeImg2" + "=[image]" + $("#PO_afterHazeImg2").attr("src").substring(0, $("#PO_afterHazeImg2").attr("src").indexOf("?V=")) + "[/image]";
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
                pic.src = 'data:image/gif;base64,R0lGODlhAQABAIAAAP///wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw=='; //设置img的src为base64编码的透明图片，要不会显示红xx
            }
        }
    }
})();

function getObjectURL(file) {
    var url = null;
    if  (window.createObjectURL != undefined) { // basic
        url = window.createObjectURL(file);
    } else if (window.URL != undefined) { // mozilla(firefox)
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) { // webkit or chrome
        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}

function getPath(obj) {
    if (obj) {
        if (window.navigator.userAgent.indexOf("MSIE") >= 1) {
            obj.select();
            return document.selection.createRange().text;
        }
        else if (window.navigator.userAgent.indexOf("Firefox") >= 1) {
            if (obj.files) {
                return obj.files.item(0).getAsDataURL();
            }
            return obj.value;
        }
        return obj.value;
    }
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
