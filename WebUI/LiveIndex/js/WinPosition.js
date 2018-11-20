function WinPosition(option) {
    //var WinPosition = function (id,clo_id) {
    this.id = option.id;
    this.clo_id = option.clo_id;
    this._init();
    //WinPosition.prototype.init();
}
WinPosition.prototype = {
    _init: function () {
        this.setWinPosition();
        this.winFixed();
    }
    , setWinPosition: function () {
        var width = $(window).width();
        var left = (width - $(this.id).width()) / 2;
        var top = ($(window).height() - $(this.id).height()+96/2) / 2;
        $(this.id).css("top", top + "px");
        $(this.id).prev().css("top", top + "px");
        $(this.id).css("left", left + "px");
        $(this.id).prev().css("left", left-3 + "px");
    }
    , winFixed: function () {
        var currentTop1 = 0;
        var scrollTop = 0;
        var old = $(this.id).position().top;
        var that = this;
        $(window).scroll(function () {
            if (old != currentTop1) {
                currentTop1 = $(that.id).position().top;
                old = currentTop1;
            }
            //判断当前窗体的top值与页面滚动的差值如果不等于第一次滚动时所记录的top值，说明窗体的位置发生拖动，需要重新读取
            if (($(that.id).position().top) - scrollTop != currentTop1) {
                //currentTop=currentTop+($("#predict").position().top-scrollTop-currentTop);
                currentTop1 = $(that.id).position().top - scrollTop;
            }
            scrollTop = $(document).scrollTop();
            $(that.id).css("top", scrollTop + currentTop1 + "px");
            $(that.id).prev().css("top", scrollTop + currentTop1 + "px");
            console.log(scrollTop);
        })
        $(".x-tool.x-tool-close").click(function () {
            $(window).off("scroll");
        })
        $(this.clo_id).click(function () {
            $(window).off("scroll");
        })
    }
}