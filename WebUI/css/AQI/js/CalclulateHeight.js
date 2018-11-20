$(function () {
    //设置界面宽度
    var pageHeight = document.documentElement.clientHeight;
    $("body").css("min-width", $(window).width() + "px");
    $(".tabs_middle1_left").height(pageHeight - 20);
});

