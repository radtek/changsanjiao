var useName = "", user = "";
var temp = [];
var invalidValue = "999.9", invalidLevel = "-1级";
$(function () {
    var loginParams = getCookie('UserInfo');
    var loginResult = Ext.util.JSON.decode(loginParams);
    useName = loginResult['Alias'];
    user = loginResult['UserName'];
    $('input[type="radio"]')[0].setAttribute("checked", "checked");
    var nowdate = new Date();
    $("#fore-time").text(nowdate.getFullYear() + "年" + proDate(nowdate));
    getSite();
    var h = window.screen.height;
    $(".right-bar .box").css("height", h + "px");
    //$("#right-content").css("height", h + "px");
    //var a = $(document).height()-80;
    $("#page_navigation").css({
        "bottom": 0,
        "height": "45px",
        "position": "fixed",
        "width":"95%"
    });
    rightBarAnimate("_box");
    rightBarAnimate("box_display");
    $("#box_display").click(displayRContent);
    $('.top-time input[type="radio"]').on('click',this,changeDate)
    $('#site').change(function () {
        getTable("publiced", "已发布指数");
        getTable("unpublic", "未发布指数");
    });
    //鼠标移到对应的单元格时出现提示
    $("#publiced,#unpublic").on("mouseover", "tbody tr .t", function () {
        var title = $(this).text();
        $(this).attr("title", title);
    })
    //ie浏览器右边要素信息表格样式会产生兼容性问题
    if(isIE()){
        $(".feature-tab tr td:nth-child(2)").css("width","52%");
    }else{
        $(".feature-tab tr td:nth-child(2)").css("width","56%");
    }
});

function isIE() { //ie?   判断是否是ie
    if (!!window.ActiveXObject || "ActiveXObject" in window)
        return true;
    else
        return false;
}

//点击最上面的radio按钮日期发生变化以及其他的一些操作
function changeDate(e) {
    $('#publiced').dataTable().fnClearTable();   //再次使用前将表格清空
    //$('#publiced').dataTable().fnDestroy();
    $('#unpublic').dataTable().fnClearTable();
    //$('#unpublic').dataTable().fnDestroy();
   
    var dNow = new Date();
    var data = e.target;
    $('.top-time input[type="radio"]').prop('checked', false);
    $(data).prop('checked', true);
    var values = data.value;
    if (values == "明天") {
        dNow.setDate(dNow.getDate() + 1);
    }
    else if (values == "后天") {
        dNow.setDate(dNow.getDate() + 2);
    }
    $('#fore-time').text(dNow.getFullYear() + "年" + proDate(dNow));
    getTable("publiced", "已发布指数");
    getTable("unpublic", "未发布指数");
}

function rightBarAnimate(id) {
    $("#"+id).mouseover(function () {
        $("#box_display ._display").stop().animate({
            "width": "50px",
            "height": "30px",
            "top": "0"
        }, 500);
        $("#box_display .triangle").stop().animate({
            "width": "15px",
            "height": "15px",
            "right": "3px",
            "top": "8px"
        }, 500);
       // $("#box_display").stop().show();
        $("#box_display .display-span").stop().show();
    });
    $("#"+id).mouseleave(function () {
        $("#box_display ._display").stop().animate({
            "width": "0",
            "height": "0"
        }, 500);
        $("#box_display .triangle").stop().animate({
            "width": "0",
            "height": "0",
            "right": "3px"
        }, 500);
        //$("#box_display").stop().hide();
        $("#box_display .display-span").stop().hide();
    });
}
function proDate(date) {
    var month = (date.getMonth() + 1);
    var day = date.getDate();
    month = month < 10 ? "0" + month : month;   //若时间小于10则在前面加0
    day = day < 10 ? "0" + day : day;
    return month + "月" + day+"日";
}
function getSite() {
    //var obj = ["徐家汇", "浦东", "奉贤", "金山", "松江", "青浦", "嘉定", "宝山", "闵行", "崇明"];
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetSite'),
        success: function (response) {
            var data = eval(response.responseText);
            var html = "";
            for (var i = 0; i < data.length; i++) {
                html += "<option value=" + data[i].stationCo + ">" + data[i].name + "</option>"
            }
            $("#site").html(html);
            getTable("publiced", "已发布指数");
            getTable("unpublic", "未发布指数");
        }
    });
}
function displayRContent() {
    var a=parseInt($("#right-content").css("width"));
    //if (parseInt($("#right-content").css("width")) <= 0) {
    if ($("#right-content").css("display")== "none") {
        $("#right-content").show();
        $("#left-content").stop().animate({
            "width": "75%"
        });
        $(".tab-header").stop().animate({
            "width": "75%"
        });
        $("#right-content").stop().animate({
            "width": "20%",
            "height": window.screen.height + "px"
        });
        $("#right-content div").eq(0).show();
        $('tr[role="row"]').height("42px");
    } else {
        $("#left-content").stop().animate({
            "width": "95%"
        });
        $(".tab-header").stop().animate({
            "width": "95%"
        });
        $("#right-content").stop().animate({
            "width": "0",
            "height": "0"
        });
        $("#right-content").hide();
    }
    
}

function getTable(id, obj) {
    //右边要素信息表格清空
    $("#feature-tab table td").remove(".cl");
    $("#right-content").css("background", "rgba(204, 204, 204, 0.2)");
    var site = $('#site').val();
    var lst = $('#fore-time').text();   //预报时间   这里的id号相反了
    $('#' + id).empty();
    var table = $('#' + id).DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "bFilter": true, "bPaginate": true,
        "bSortClasses": false,
        "bPaginate": false,
        "searching": false,
        "bAutoWidth": false,
        "info": false ,
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.LiveIndex&method=GetTable&status=" + encodeURI(obj) + "&site=" + encodeURI(site) + "&lst=" + encodeURI(lst),
        "columns": [
             {
                 "class": null,
                 "orderable": false,
                 "data": null,
                 "defaultContent": ""
             },
            { "visible": false },
            { "title": "指数名称", class: "m indexName t" },
            { "title": "指数值", class: "s indexVal" },
            { "title": "指数级别", class: "s levelVal" },
            { "title": "含义", class: "m indexMean t" },
            { "title": "简短提示", class: "sl shortTip t l" },
            { "title": "详细提示", class: "tl longTip t l" },
            { "title": "原因", class: "invisible" }
        ],
        "aoColumnDefs": [{
            //设置第一列不排序
            "bSortable": false,
            "aTargets": [0]
        }
        ],
        "fnCreatedRow": function (nRow, aData, iDataIndex) {
            $('td:eq(0)', nRow).attr("id", aData[1]);
            $('td:eq(3)', nRow).text("");
            $('td:eq(3)', nRow).text(aData[4] + "级");
            
            //$('td:eq(0)', nRow).html("<span class='row-details row-details-close' data_id='" + aData[1] + "'></span>");
        },
        "fnInitComplete": function (oSettings, json) {
            $("#" + id).parents(".publiced").find(".y-total").text(json.data.length + "个指数");
            if (id == "unpublic") {
                var num = $('#publiced tbody tr,#unpublic tbody tr').length;
                $(document).PaginBar('page_navigation', id, 'publiced', num);
            }
            //显示红色字体
            for (var i = 0; i < $(".indexVal").length; i++) {
                var indexV = $(".indexVal")[i].innerHTML;
                var level = $(".levelVal")[i].innerHTML;
                if (indexV == invalidValue) {
                    $(".indexVal")[i].style.color = "red";
                }
                if (level == invalidLevel) {
                    $(".levelVal")[i].style.color = "red";
                }
            }
            //改变人体指数的值
            //bodyIndex();
        },
        "oLanguage": {
            "sProcessing": "<img src='./images/loading.gif'/></br>",
            "sLengthMenu": "每页显示 _MENU_ 条记录",
            "sZeroRecords": "对不起，查询不到相关数据！",
            "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
            "sInfoEmpty": "当前显示 0 到 0 条记录，共 0 条",
            "sEmptyTable": "表中无数据存在！",
            "sInfoFiltered": "数据表中共为 _MAX_ 条记录",
            "sSearch": "用户搜索",
            "oPaginate": {
                "sFirst": "首页", "sPrevious": "上一页", "sNext": "下一页", "sLast": "末页"
            }
        }
    });
    //单击行获取要素信息表格
    $('#' + id + ' tbody').on('click', 'tr', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        getFeature(row.data()[2]);
    });
}
//人体指数
function bodyIndex() {
    var lst = $("#fore-time").text();
    var siteId = $("#site").val();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetWeather_IndexTip'),
        params: { lst: lst, station: siteId },
        success: function (response) {
            var data = eval('(' + response.responseText + ')').data;
            for (j = 0; j < data.length; j++) {
                var temp = data[j];
                var code = temp[0];
                var val = temp[1];
                var jb = temp[2] + "级";
                var mean = temp[3];
                var shortTip = temp[4];
                var longTip = temp[5];
                var tr = $("#" + code).parent("tr");
                var className = ["indexVal", "levelVal", "indexMean", "shortTip", "longTip"];
                var v = ["val", "jb", "mean", "shortTip", "longTip"];
                for (i = 0; i < className.length; i++) {
                    $(tr).find("." + className[i]).text(eval('(' + v[i] + ')'));
                }
            }
        }
    });
}

//点击行获取要素信息
function getFeature(code) {
    var site = $('#site').val();
    var lst = $('#fore-time').text();
    var target;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetFeatureInfo'),
        params: { code: code, target: target, lst: lst, site: site },
        success: function (response) {
            var html = response.responseText;
            $("#feature-tab table td").remove(".cl");
            //row.child(details).show();
            if (html != "") {
                $("#right-content").css("background", "none");
            } else {
                $("#right-content").css("background", "rgba(204, 204, 204, 0.2)");
            }
            $("#feature-tab table").append(html);
        }
    });
}

//单击发布或未发布的行改变其“+”号的状态
function details(id,obj) {
    if ($("#"+id).hasClass("clo")) {
        $("#" + id).removeClass("clo");
        $("#" + id).addClass("open");
        $("#"+obj).show();
    } else {
        $("#" + id).removeClass("open");
        $("#" + id).addClass("clo");
        $("#" + obj).hide();
        $("table.dataTable").css("border - collapse","collapse");
    }
}

function Check() {
    if (!confirm("是否要检查！")) return;
    var code = getText($('#publiced tbody tr'));
    var mk = new Ext.LoadMask(document.body, {
        msg: '正在发布，请稍候！',
        removeMask: true //完成后移除  
    });
    var name = "首席";
    mk.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'Check'),
        timeout: 100000,
        params: { code: code, userName: user, name: name, operater: useName },
        success: function (response) {
            var t = response.responseText;
            if (t.indexOf('success') > -1) {
                alert("发布成功！");
            } else {
                alert("发布失败！");
            }
            mk.hide();
        }
    });
}
function getText(id) {
    var txt = "";
    $(id).each(function(index,element) {
        txt += $(element).children().eq(0).attr('id') + "#";
    });
    return txt;
}