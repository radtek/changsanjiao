/*
这个js页面在别的页面也有引用，修改请注意
*/
var win,winPre,pan;
var useName = "",user="",temp = [];
var invalidValue = "999.9", invalidLevel = "-1级";
$(function () {
    var loginParams = getCookie('UserInfo');
    var loginResult = Ext.util.JSON.decode(loginParams);
    useName = loginResult['Alias'];
    user = loginResult['UserName'];

    $('input[type="radio"]')[0].setAttribute("checked", "checked");
    var nowdate = new Date();
    $("#fore-time").text(nowdate.getFullYear() + "年" + proDate(nowdate));
   // getSite();
    $('#site').change(function () {
        getTable("publiced", "已发布指数");
        getTable("unpublic", "未发布指数");
    });
    var h = window.screen.height;
    $(".right-bar .box").css("height", h + "px");
    //$("#right-content").css("height", h + "px");
    //var a = $(document).height()-80;
    $("#page_navigation").css({
        "bottom": "0",
        "height": "45px",
        "position": "fixed",
        "width":"95%"
    });
    rightBarAnimate("_box");
    rightBarAnimate("box_display");
    $("#box_display").click(displayRContent);
    $("#dropDownMenu").on("click", getDropTable);
    $("#J_cancel").on("click", hideRow);
    $("#J_drop-down .drop-table tr:eq(0) td").css({
        "border-left": "1px solid #999",
    });
    //下拉菜单等级“+”号展开或关闭
    $("#J_drop-down").on("click", ".y-bgPlus", function () {
        if ($(this).hasClass('open')) {
            $(this).removeClass('open');
            $(this).addClass('clo');
            $(this).parent().next().hide();
        }
        else {
            $(this).addClass('open');
            $(this).parent().next().show();
            //执行函数，用来获取该等级的详细信息
            //var grade = $(this).next().text();
            //getTabGData(id);
        }
    });
    $('.top-time input[type="radio"]').on('click',this,changeDate)
    $("#J_save").on("click", save);
    //鼠标移到对应的单元格时出现提示
    $("#publiced,#unpublic").on("mouseover","tbody tr .t",function(){
        var title=$(this).text();
        $(this).attr("title",title);
    });
    //ie浏览器右边要素信息表格样式会产生兼容性问题
    if(isIE()){
        $(".feature-tab tr td:nth-child(2)").css("width","52%");
    }else{
        $(".feature-tab tr td:nth-child(2)").css("width","56%");
    }
});

//点击取消，隐藏展开的内容
function hideRow(){
    $("#J_detail").hide('fast', function () {
        $(".temp").parent().parent().remove();
        $(".details-control").parents().removeClass('shown');
    });
    temp = [];
    $('#J_reason').empty();
    $('#J_reason').selectpicker('render');
    $('#J_reason').selectpicker('refresh');
}

//点击最上面的radio按钮日期发生变化以及其他的一些操作
function changeDate(e) {
    $('#J_detail').hide();
    $('#publiced').dataTable().fnClearTable();   //再次使用前将表格清空
    $('#unpublic').dataTable().fnClearTable();
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
function getSite(obj) {
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
            if (obj == "factor") {
                getFactorTable();
            } else {
                getTable("publiced", "已发布指数");
                getTable("unpublic", "未发布指数");
            }
        }
    });
}
function displayRContent() {
    var arr = ["#left-content", "#page_navigation", ".tab-header"];
    if ($("#right-content").css("display")== "none") {
        $("#right-content").show();
        for (var i = 0; i < arr.length; i++) {
            $(arr[i]).stop().animate({
                "width": "75%"
            });
        }
        $("#right-content").stop().animate({
            "width": "20%",
            "height": window.screen.height + "px"
        });
        $("#right-content div").eq(0).show();
        $('tr[role="row"]').height("42px");
    } else {
        for (var i = 0; i < arr.length; i++) {
            $(arr[i]).stop().animate({
                "width": "95%"
            });
        }
        $("#right-content").stop().animate({
            "width": "0",
            "height": "0"
        });
        $("#right-content").hide();
    }
    
}

function getTable(id, obj) {
    hideRow();
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
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.LiveIndex&method=GetTable&status=" + encodeURI(obj) + "&site=" + encodeURI(site) + "&lst=" + encodeURI(lst)+"",
        "columns": [
             {
                 "class": 'details-control',
                 "orderable": false,
                 "data": null,
                 "defaultContent": ""
             },
            { "visible": false },
            { "title": "指数名称", class: "m indexName t"},
            { "title": "指数值", class: "s indexVal"},
            { "title": "指数级别", class: "s levelVal"},
            { "title": "含义", class: "m indexMean t"},
            { "title": "简短提示", class: "l shortTip t"},
            { "title": "详细提示", class: "l longTip t" },
            { "title": "原因", class: "invisible"  }
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
    //点击“+”号，展开或关闭
    $('#' + id + ' tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        display(tr, row);
    });

    //双击行展开表格以及获取要素信息表格
    $('#' + id + ' tbody').on('dblclick', 'tr', function () {
        var tr = $(this).closest('tr');     //这里this和变量tr相等，即var tr=this;
        var row = table.row(tr);
        display(tr, row);
    });

    //单击行获取要素信息表格
    $('#' + id + ' tbody').on('click', 'tr', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        getFeature(row.data()[2]);
    });
}


//单击“+”号或双击行是添加类，改变"+"号的状态
function display(tr, row) {
    $(".temp").parent().parent().remove();
    //除过该行以外的details-control的父元素带有shown类的把shown移除，
    //否则第二次点击或双击的时候总是判断改行没有shown就“+”号的状态不改变
    $(".details-control").parents().not(tr).removeClass('shown');
    temp = [];
    $('#J_reason').empty();
    $('#J_reason').selectpicker('refresh');
    $('#J_reason').selectpicker('render');
    if (tr.hasClass("shown")) {
        //row.child().empty();
        // This row is already open - close it
        row.child.hide();
        $("#J_detail").hide();
        tr.removeClass('shown');
        $("#J_drop-down").hide();
    }
    else {
        // Open this row
        getHtml(tr,row);
        tr.addClass("shown");
    }
}

//点击行获取要素信息
function getFeature(code, target) {
    var lst =  $('#fore-time').text();
    if (target == "index") {
        lst = $('#lst').text();
    }
    var site = $('#site').val();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetFeatureInfo'),
        params: { code: code,target:target,lst:lst,site:site},
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
//点击“+”号或双击行获取展开的html
//data为要展开行的code编码，row表示哪一行
function getHtml(tr,row) {
    var html = "<div class='temp'></div>";
    row.child(html).show();
    var top = $(tr).offset().top - $("#_page").height();
    var s_top = $(tr).scrollTop();
    $("#J_detail").show();
    //$(tr + ">td>div").height($("#J_detail").height());
    $("#left-content .temp").height($("#J_detail").height());
    $("#J_detail").css({
        "top": top,
        "left":0
    });
    getHtmlValue(tr, row);
    $("#J_drop-down").hide();
}

//行单击展开的文本赋值
function getHtmlValue(tr,row) {
    //首先清掉所有值
    $("#J_lvalueDropDown").val("");
    $("#J_reason").val("");
    $("#J_IndexValue").val("");
    $("#J_tip").val("");
    //赋值
    var data = row.data();
    var tds=$(tr).find('td');
    var flag=data[3]==$(tds).eq(2).text() && data[4]==$(tds).eq(3).text() && data[5]==$(tds).eq(4).text() && data[6]==$(tds).eq(5).text()  && data[7]==$(tds).eq(6).text();
    if(!flag){    //如果不相等则说明是保存之后的（没刷新页面），需要对data重新赋值
        data.length=0;
        data[0]=$(tds).eq(1).text();
        data[1]=$(tds).eq(0).attr("id");
        for(var i=1;i<tds.length;i++){
            data[i+1]=$(tds).eq(i).text();
        }
    }
    var reason = data[8];
    var lvalueDropDown = data[6]    //有两行被隐藏了    下拉菜单级别
    var indexValue = data[3];    //指数值
    var tip = data[7];      //提示语
    $("#J_lvalueDropDown").val(lvalueDropDown);
    $("#J_IndexValue").val(indexValue);
    $("#J_tip").val(tip);
    if (reason != "") {
        addToSelect(reason);
        $('#J_reason').on('change', reaChange);
    }
    else {
        $("#J_reason").attr("disabled",true);
    }
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
        $("#"+obj).find('.shown').removeClass('shown');
        $("#" + obj).hide();
        $("#J_drop-down").hide();
        $("table.dataTable").css("border - collapse","collapse");

        $(".temp").parent().parent().remove();
        temp = [];
        $("#J_detail").hide();
        $('#J_reason').empty();
        $('#J_reason').selectpicker('refresh');
        $('#J_reason').selectpicker('render');
        
    }
}

//获得下拉菜单表格
function getDropTable(e) {
    var width=$("#J_lvalueDropDown").outerWidth(true)+$("#dropDownMenu").outerWidth(true);
    var left=$("#J_lvalueDropDown").offset().left-10;
    //当点击下拉菜单按钮时不阻止冒泡，否则会执行document点击事件，影响效果
    if ($(e.target).id != "J_drop-down") {
        event.stopPropagation();
    }
    $("#J_drop-down").slideToggle();
    $("#left-content .tab-header").height();
    var top = $("#dropDownMenu").offset().top;
    $("#J_drop-down").css({
        "top": top-($("#_page").height())-7,
        "left": left,
        "background-color": "white",
        "width":width
    });
    getTabGradeData();
}
//点击文档任何地方下拉菜单框消失
$(document).on("click", function (e) {
    //alert($(e.target).attr("id"));
    if (($(e.target).parents("#J_drop-down").attr("id") != "J_drop-down") && ($(e.target).attr("id") != "J_drop-down")) {
        $("#J_drop-down").hide();
    }
})

//获得下拉菜单后的表格数据
function getTabGradeData() {
    //获取点击行的code
    $("#J_drop-down .drop-table .y-bgPlus").parent().remove();
    var code = $(".shown .details-control").attr("id");
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetTabGradeData'),
        params: { code: code },
        success: function (response) {
            var data = response.responseText;
            $("#J_drop-down .drop-table tr:eq(0)").siblings().remove();
            //$("#J_drop-down .drop-table tbody").html(data);
            $("#J_drop-down .drop-table").append(data);
//            var num = $("#J_drop-down .drop-table .y-bgPlus").length;  //获得下拉菜单框下面有几个等级，即除过第一个tr的数量，循环调用getTabGData方法获得展开的数据，初始情况每一tr展开
//            for (var i = 1; i <= num; i++) {
//                var objtr = $("#" + i).parent()
//                getTabGData(i);
//            }
        }
    });
}

//获取下拉菜单等级展开后的数据
function getTabGData(grade) {    //grade等级，obj把后台传来的数据追加到那个tr下面
    $("#" + grade).parent().find('div').empty();
    var code = $(".shown .details-control").attr("id");
    var mainLevel = $('#' + grade).next().attr('month');
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'getTabGData'),
        params: { code: code,grade:grade,mainLevel:mainLevel },
        success: function (response) {
            var data = response.responseText;
            $("#" + grade).parent().append(data);
        }
    });
}

//点击radio按钮获取相对应的值填入对应的框中
function getMValue(obj) {
    temp = [];    //情空
    $("#J_lvalueDropDown").val("");
    $("#J_IndexValue").val("");
    $("#J_tip").val("");
    $("#J_drop-down input[type='radio']").prop("checked", false);
    $(obj).prop('checked', true);
    var britip = $(obj).parents('.change').find('.britip').text();
    var value = $(obj).parents('.change').find('.value').text();
    var tip = $(obj).parents('.change').next().text().split('：')[1];
    var reason = $(obj).parents('.change').find('.reason').text();
    $(".shown .invisible").text(reason);   //把原因保存到最后一列，方便把原因保存到保存表中
    var indexGrade = $(obj).parents('tr').prev().find('.y-dj').text().split('：')[1];    //指数等级
    var meanName = $(obj).parents('.change').find('.mean').text();   //指数含义
    $('.shown td').eq(3).attr('indexGrade', indexGrade);    //添加一个自定义属性，方面取等级值
    $('.shown td').eq(4).attr('meanName', meanName);
    $("#J_lvalueDropDown").val(britip);
    $("#J_IndexValue").val(value);
    $("#J_tip").val(tip);
    getReason(britip, tip, reason);
    $("#J_drop-down").hide();
    $('#J_reason').on('change', reaChange);
}

function getReason(britip, tip, reason) {
    if (britip.indexOf('原因') > -1 || tip.indexOf('原因') > -1) {
        addToSelect(reason);
    }
    else {
        $('#J_reason').attr('disabled',true);
    }
}

//把原因添加到select多选框中
function addToSelect(reason) {
    $('#J_reason').empty();
    $('#J_reason').attr("disabled",false);
    //var sub_reason = reason.substring(0, reason.length - 1).split(';');
    reason=reason.replace(/;$/,"")   //最后一个分号有可能有有可能没有
    var sub_reason = reason.split(';');
    var html = "";
    for (var i = 0; i < sub_reason.length; i++) {
        var reaValue = sub_reason[i].split(',');
        for (var j = 0; j < reaValue.length; j++) {
            html += "<option value=" + reaValue[j] + ">" + reaValue[j] + "</option>";
        }
        html += "<option value=-间隔符->-间隔符-</option>";
    }
    $('.selectpicker').html(html);
    $('#J_reason').selectpicker('render');
    $('#J_reason').selectpicker('refresh');
}

function remove(arr,str) {
    for (var m = 0; m < arr.length; m++) {
        if (arr[m] == str) {
            arr.splice(m, 1);
        }
    }
}
//select值发生变化执行的函数
function reaChange() {
    var flag = 1;
    var mainName = $("#J_lvalueDropDown").val();
    var tip = $("#J_tip").val();
    var selValue = $("#J_reason").val();
    if (selValue == "-间隔符-" ) return;   //若选择的是间隔符则不做任何处理
    for (var k = 0; k < selValue.length; k++) {      //若选项中包含有间隔符则把间隔符去掉
        if (selValue[k] == "-间隔符-") {
            selValue.splice(k, 1);
        }
    }
    var value = [];
    if (temp.length > 0) {
        for (var t = 0; t < temp.length; t++) {
            value.push(temp[t]+"");
        }
    }
    var str = "";
    if (selValue.length > temp.length) {
        str = getDiffStr(selValue, temp)[0];
        value.push(str);
    }
    else {
        str = getDiffStr(temp, selValue)[0];
        remove(value,str);
    }
    var count = $("#J_reason option[value='-间隔符-']").length;    //有几个间隔符
    var index = $("#J_reason option[value=" + str + "]").index();
    for (var i = 0; i < count; i++) {
        var max = $("#J_reason option[value='-间隔符-']")[i].index;
        var min = -1;
        if (i != 0) {
            min = $("#J_reason option[value='-间隔符-']")[i-1].index;
        }
        if (index < max) {
            var a = "";
            if (mainName.indexOf("需选择原因") > -1 || tip.indexOf("需选择原因") > -1) {
                a = mainName.indexOf("需选择原因") > -1 ? mainName.indexOf("需选择原因") : tip.indexOf("需选择原因");
            } else {
                a = mainName.indexOf("需选择第" + (i + 1) + "原因") > -1 ? mainName.indexOf("需选择第" + (i + 1) + "原因") : tip.indexOf("需选择第" + (i + 1) + "原因");
            }
            if ((index > min && i != 0 && a > -1) || (i == 0 && a > -1)) {
                flag = 0;       //等于0表示第一次选择第i个原因，即前面没有“且”字
            }
            if (flag == 0) {
                var main = strReplace(mainName, i);
                var tipinfo = strReplace(tip, i);
                if (mainName.indexOf("需选择原因") > -1 || tip.indexOf("需选择原因") > -1) {
                    main = main.replace("需选择原因", str);
                    tipinfo = tipinfo.replace("需选择原因", str);
                } else {
                    main = main.replace("需选择第" + (i + 1) + "原因", str);
                    tipinfo = tipinfo.replace("需选择第" + (i + 1) + "原因", str);
                }
                $("#J_lvalueDropDown").val(main);
                $("#J_tip").val(tipinfo);
                temp.push(str +"");
                flag ++;
            } else {
                var mark = 0;
                for (var j = 0; j < temp.length; j++) {
                    var num = $("#J_reason option[value='" + temp[j] + "']").index();
                    if (num > min && num < max && mark == 0) {
                        if (temp.length < value.length) {   //增加了某一个选项
                            var index_main = mainName.indexOf(temp[j]);
                            var wz_main = index_main + temp[j].length;    //记录要插入值的位置
                            mainName = mainName.substring(0, wz_main) + "且" + value[value.length - 1] + mainName.substring(wz_main, mainName.length);
                            var index_tip = tip.indexOf(temp[j]);
                            var wz_tip = index_tip + temp[j].length;
                            tip = tip.substring(0, wz_tip) + "且" + value[value.length - 1] + tip.substring(wz_tip, mainName.length);
                            temp.push(str + "");
                        }
                        if (temp.length > value.length) {   //删除了某一个选项
                            if (mainName.charAt(mainName.indexOf(str) - 1) == "且") {
                                mainName = mainName.replace("且" + str, "");
                                tip = tip.replace("且" + str, "");
                            }
                            else {
                                mainName = mainName.replace(str+"且", "");
                                tip = tip.replace(str + "且", "");
                            }
                            remove(temp, str);
                        }
                        mark++;
                    }
                }
                $("#J_lvalueDropDown").val(mainName);
                $("#J_tip").val(tip);
                flag++;
            }
            break;
        }
    }
}

//提取两个字符串中不同的字符 arr1是长数组  arr2是短数组
function getDiffStr(arr1,arr2) {
    var newArr = [];
    var temp = [];   //临时数组
    for (var i = 0; i < arr2.length; i++) {
        temp[arr2[i]] = true;
    }
    for (var j = 0; j < arr1.length; j++) {
        if (!temp[arr1[j]]) {
            newArr.push(arr1[j]);
        }
    }
    if (newArr.length > 0) {
        return newArr;
    }
}
function strReplace(str, k) {
    var newStr = "";
    var rea_k = str.indexOf("第" + (k + 1) + "原因");    //第k+1个原因出现的位置
    var index_l = str.lastIndexOf("{", rea_k); //记录第k+1个原因之前的“{”的索引值
    var strObj = str.substring(0, index_l) + str.substring(index_l + 1, str.length);//把上面索引值的字符删除
    var index_r = strObj.indexOf("}", rea_k);   //记录第k+1个原因之后的“}”的索引值
    var result = strObj.substring(0, index_r) + strObj.substring(index_r + 1, str.length);//把上面索引值的字符删除
    return result;
}

function changeColor(obj, flag) {   //1表示鼠标移上去事件，2表示鼠标移开事件，3表示点击事件
    if (flag == 1 && $(obj).css('background-color') != 'rgb(204, 204, 204)') {
       $(obj).css("background-color","rgb(204, 204, 204)");
    } else if (flag == 2 && $(obj).css('background-color') == 'rgb(204, 204, 204)') {
        obj.style.backgroundColor = "transparent";
    } else if (flag == 3) {
        $(".dj").css("background-color","transparent");
        $(obj).css("background-color","rgb(204, 204, 204)");
    }
}

function save() {
    var briefTip = $("#J_lvalueDropDown").val();    //短提示
    var params=$(".shown .invisible").text();
    var tip = $("#J_tip").val();  //详细提示
    if(briefTip.indexOf("原因")==-1&&tip.indexOf("原因")==-1){     //如果能搜到原因则说明原因没有填写完整，需要把选项保存到数据库
        params="";
    }
    var code = $(".shown .details-control").attr('id');
    var station = $("#site").val();
    var LST = $("#fore-time").text();
    var tag=$(".shown td");
    var levelValue = $(tag).eq(3).attr('indexGrade') == undefined ?$(tag).eq(3).text().split('级')[0]: $(tag).eq(3).attr('indexGrade');   //等级  如果等于underfined则说明没有选择模板，是直接保存，则需要从表格的td中读取
    //必须满足以下两个条件才能读取表格td的值$("#J_IndexValue").val()=0有可能指数值正好等于0
    var value = ($("#J_IndexValue").val() == 0) && ($(tag).eq(3).attr('indexGrade') == undefined) ? $(tag).eq(2).text() : $("#J_IndexValue").val();   //指数值
    var name = $(tag).eq(1).text();  //指数名称
    var meanName = $(tag).eq(4).attr('meanname') == undefined ? $(tag).eq(4).text() : $(tag).eq(4).attr('meanname');   //含义
    var status = $('.shown').parents('.publiced').children()[0].innerHTML+"指数";  //发布状态
    if (!confirm("是否要保存？")) return;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'Save'),
        params: { briefTip: briefTip, value: value, tip: tip, LST: LST, code: code, station: station, forecaster: useName,levelValue:levelValue,meanName:meanName,name:name,status:status,param:params },
        success: function (response) {
            if (response.responseText == "ok") {
                alert("保存成功！");    //保存成功不刷新页面，只改变改行的数据显示，刷新用户体验不是特别好
                var data={
                    "tag":tag,
                    "levelValue":levelValue,
                    "value":value,
                    "meanName":meanName,
                    "briefTip":briefTip,
                    "tip":tip,
                    "params":params
                };
                bindRowData(data);
            } else {
                alert("保存失败！请仔细核查后再保存");
            }
            hideRow();
//            $('#publiced').dataTable().fnClearTable();   //再次使用前将表格清空
//            $('#unpublic').dataTable().fnClearTable();
//            getTable("publiced", "已发布指数");
//            getTable("unpublic", "未发布指数");
        }
    });
}

//保存成功后在改行绑定新的数据
function bindRowData(data){
    $(data.tag).eq(2).text(data.value);
    $(data.tag).eq(3).text(data.levelValue+"级");
    $(data.tag).eq(4).text(data.meanName);
    $(data.tag).eq(5).text(data.briefTip);
    $(data.tag).eq(6).text(data.tip);
    $(data.tag).eq(7).text(data.params);
    if(data.value!=invalidValue){
        $(data.tag).eq(2).css("color","black");
    }
    if(data.levelValue!=invalidLevel){
        $(data.tag).eq(3).css("color","black");
    }
}

function saveAll() {
    if (!confirm("是否要全部保存！")) return;
    var code = getText("tbody .details-control", "attr");
    var name = getText("#publiced tbody .indexName,#unpublic tbody .indexName");
    var indexVal = getText("#publiced tbody .indexVal,#unpublic tbody .indexVal");
    var levelVal = getText("#publiced tbody .levelVal,#unpublic tbody .levelVal");
    var indexMean = getText("#publiced tbody .indexMean,#unpublic tbody .indexMean");
    var shortTip = getText("#publiced tbody .shortTip,#unpublic tbody .shortTip");
    var longTip = getText("#publiced tbody .longTip,#unpublic tbody .longTip");
    var site = $('#site').val();
    var LST = $('#fore-time').text();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'SaveAll'),
        params: { forecaster:useName, code: code, name: name, indexVal: indexVal, indexMean: indexMean, levelVal: levelVal, shortTip: shortTip, longTip: longTip, site: site, LST: LST },
        success: function (response) {
            var t = response.responseText;
            if (t.indexOf('error') > -1) {
                alert("保存失败！");
            } else {
                alert("保存成功！");
            }
        }
    });
}
function getText(id,text) {
    var txt = "";
    for (var i = 0; i < $(id).length; i++) {
        if (text == "attr") {
            txt += $(id)[i].getAttribute('id')+ "#";
        }
        else {
            txt += $(id)[i].innerHTML + "#";
        }
    }
    return txt;
}

function exportToExcel(){
    var _lst=$('#fore-time').text();
    var _site=$('#site').val()+"#"+$("#site option:selected").text();
    document.getElementById('siteid').setAttribute("value",_site);
    document.getElementById('lst').setAttribute("value",_lst);
    document.getElementById('btnExport').click();
}

//短信文本预览
function messPreview(){
//清掉
    $("#MessArea").val("");
//1、先获取短信内容文本
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetMessTxt'),
        params: { type:"message",userName:user },
        success: function (response) {
            var data = response.responseText;
            if(data!=""){
                $("#MessArea").val(data);
            }
        }
    });
//2、把获取的短信文本与文本域绑定
    if(!win){
        win = new Ext.Window({
            title: '短信文本预览',
            id:"message",
            width: 725,
            height: 400,
            layout: 'fit', //设置窗口内部布局
            closeAction: 'hide',
            plain: true, //true则主体背景透明，false则和主体背景有些差别
            collapsible: true, //是否可收缩
            modal: true, //是否为模式窗体
            items: [{
                id: "messTxt",
                html: '<textarea id="MessArea"  readonly="readonly" class="messPrev"></textarea>'//内部显示内容
            }],
            buttons: [{
                id:"sendMess",
                text:"发送",
                handler: function () {//点击时触发的事件
                    var messTxt=$("#MessArea").val();
                    var mk = new Ext.LoadMask(document.body, {
                        msg: '正在发布，请稍候！',
                        removeMask: true //完成后移除  
                    });
                    mk.show();
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.LiveIndex', 'MessTxtUpload'),
                        params: { type:"message",txt:messTxt,num:"1",name:"短信"},
                        success: function (response) {
                            var data = response.responseText;
                            if(data=="success"){
                                alert("发布成功！");
                            }
                            else{
                                alert("发布失败！");
                            }
                            mk.hide();
                        }
                    });
                }},{
                    id:"mess_close",
                    text:"关闭",
                    handler: function () {//点击时触发的事件
                        win.hide();
                   }  
                }
            ]
        });
    }
    win.show();
    $("#message").css("top",130+"px");
    $("#message").prev().css("top",130+"px");
    new WinPosition({id:$("#message"),clo_id:$("#mess_close")});
    $("#MessArea").focus(function(){
        $("#MessArea").css({
             "border": "none",
             "box-shadow":"none"
        });
    });
}
function centerPre(){
    $("#todayPreTxt").text("");
    $("#sevenPreTxt").text("");
    var mk = new Ext.LoadMask(document.body, {
        msg: '正在加载数据，请稍候！',
        removeMask: true //完成后移除  
    });
    mk.show();
    //获取ftp接口数据
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetPreviewData'),
        params: { type:"message",userName:user },
        success: function (response) {
            var data = response.responseText;
            mk.hide();
            if(data!="error"){
                var arr=data.split('#');
                $("#todayPreTxt").text(arr[1]);
                $("#sevenPreTxt").text(arr[0]);
            }else{
                $("#todayPreTxt").text("error");
                $("#sevenPreTxt").text("error");
            }
        },
        failure:function(){
            mk.hide();
        }
    });
    if(!pan){
        pan=new Ext.TabPanel({
        autoTabs: true,
        activeTab: 0,
        deferredRender: false,
        border: false,
        buttonAlign: "center",
        items: [
                {
                    id: "todayPre",
                    title:"今日预报",
                    html: '<textarea id="todayPreTxt" readonly="readonly" class="messPrev" ></textarea>'//内部显示内容
                },
                {
                    id: "sevenPre",
                    title:"七日预报",
                    html: '<textarea id="sevenPreTxt"  readonly="readonly" class="messPrev" ></textarea>'//内部显示内容
                }
            ]
        });
    }
    if(!winPre){
        winPre = new Ext.Window({
            title: '中心气象预报',
            width: 725,
            height: 450,
            id:'predict',
            layout: 'fit', //设置窗口内部布局
            closeAction: 'hide',
            plain: true, //true则主体背景透明，false则和主体背景有些差别
            collapsible: true, //是否可收缩
            modal: true, //是否为模式窗体
            items: [pan],
            buttons:[{
                    id:"pre_close",
                    text:"关闭",
                    handler: function () {//点击时触发的事件
                        winPre.hide();
                   }  
                }
            ]
        });
    }
    winPre.show();
    new WinPosition({id:$("#predict"),clo_id:$("#pre_close")});
    $("#predict .x-tab-right").eq(0).css("background-color","rgb(55,156,224)");
    $("#predict .x-tab-right").click(function(){
        $("#predict .x-tab-right").css("background-color","#99bbe8");
        $(this).css("background-color","rgb(55,156,224)");
    });
}
