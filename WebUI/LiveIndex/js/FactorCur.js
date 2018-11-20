var invalidValueF = "999.9";   //无效值，显示红色
$(function () {
    var loginParams = getCookie('UserInfo');
    var loginResult = Ext.util.JSON.decode(loginParams);
    useName = loginResult['Alias'];
   
    var dNow = new Date();
    $('#lst').text(dNow.getFullYear() + "年" + proDate(dNow));
    $("._top input[type=radio]").eq(0).prop('checked',true);
    $("._top input[type=radio]").on("click", changeDate1);
    var h = window.screen.height;
    $(".right-bar .box").css("height", h + "px");
    getSite("factor");
    $('#table').on('click', '.disp', this, getSubTab);
    $('#site').change(function () {
        getFactorTable();
    });
});

function changeDate1(e) {
    var dNow = new Date();
    var data = e.target;
    $('._top input[type="radio"]').prop('checked', false);
    $(data).prop('checked', true);
    var values = data.value;
    if (values == "明天") {
        dNow.setDate(dNow.getDate() + 1);
    }
    else if (values == "后天") {
        dNow.setDate(dNow.getDate() + 2);
    }
    $('#lst').text(dNow.getFullYear() + "年" + proDate(dNow));
    getFactorTable();
}
function getFactorTable() {
    //右边要素信息表格清空
    $("#feature-tab table td").remove(".cl");
    $("#right-content").css("background", "rgba(204, 204, 204, 0.2)");
    var lst = $('#lst').text();
    var site = $('#site').val()
    var date = new Date();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetFactorTable'),
        params: { site: site,LST:lst },
        success: function (response) {
            var data = response.responseText;
            $("#table").html(data);
            $(".parent").each(function (i, ele) {
                if ($(ele).next().find('table').find('tr').length == 0) {
                    $(this).children().removeClass('open');
                    $(this).children().addClass('clo');
                }
            });
            $('.cont tr').on('click',this,getInfo);
            $('.cont tr:even').css('background-color', '#f9f9f9');
            $('.cont tr').hover(function () {
                $(this).css('background-color', '#f9f9f9'); 
            }, function () {
                if (this.rowIndex % 2 == 0) {
                    $(this).css('background', '#f9f9f9');
                } else {
                    $(this).css('background', 'none');
                }
            });
            //如果单元格的值为999.99则显示红色字体
            for (var i = 0; i < $('.cont .info .span-txt').length; i++) {
                var fValue = $('.cont .info .span-txt')[i].innerHTML;
                if (fValue ==invalidValueF) {
                    $('.cont .info .span-txt')[i].style.color = "red";
                }
            }
            var fValue = $('.cont .info .span-txt')
            $('.cont .info .span-txt').on('click', this, displayInputTxt);
            $('.cont .info .input-txt').on('click', function (e) {
                var evt = e ? e : window.event;
                if (evt.stopPropagation) {//W3C 
                    evt.stopPropagation();
                }
                else { //IE 
                    evt.cancelBubble = true;
                }
            });
            $('.cont tr td img').on('click', this, onlySave);
            if (isIE) {
                $("input, button, select, textarea").css("line-height","normal")
            }
        }
    });
}

//点击“+”号展开数据行显示详细信息
function getSubTab() { 
    if ($(this).parent().hasClass('open')) {
        $(this).parent().removeClass('open');
        $(this).parent().addClass('clo');
        $(this).parents('.parent').next().hide();
    } else {
        $(this).parent().removeClass('clo');
        $(this).parent().addClass('open');
        $(this).parents('.parent').next().show();
    }
}
function displayInputTxt(e) {
    var evt = e ? e : window.event;
    if (evt.stopPropagation) {//W3C 
        evt.stopPropagation();
    }
    else { //IE 
        evt.cancelBubble = true;
    }
    hideInputTxt();
    $('.cont .info .input-txt[show="y"]').val("");
    $('.cont .info .input-txt[show="y"]').attr('show', 'n');
    $(this).hide();
    $(this).prev().attr('show', 'y').show().focus();
}

$(document).click( hideInputTxt);
function hideInputTxt() {
    var txt = $('.cont .info .input-txt[show="y"]').val();
    if (txt == "") {
        txt = $('.cont .info .input-txt[show="y"]').next().text();
    }
    $('.cont .info .input-txt').hide();
    if (txt != invalidValueF) {
        $('.cont .info .input-txt[show="y"]').next().css("color", "black");
    } else {
        $('.cont .info .input-txt[show="y"]').next().css("color", "red");
    }
    $('.cont .info .input-txt[show="y"]').next().show().text(txt);
}

function onlySave() {
    hideInputTxt();
    var id = $(this).parent().siblings();
    var txt = $(id).children('.span-txt').text();
    var fCode = $(id).parent().children().eq(0).text();
    var site = $("#site").val();
    var lst = $('#lst').text();
    if (!confirm("是否保存！")) return;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'OnlySave'),
        params: { txt: txt, LST: lst, fCode: fCode,site:site },
        success: function (response) {
            alert("保存成功！");
            getFactorTable();
        }
    });
}

function getInfo() {
    var code = $(this).children()[0].innerText;
    getFeature(code,"index");
}

