var userName = "", userAcount = "";
$(function () {
    var loginParams = getCookie('UserInfo');
    var loginResult = Ext.util.JSON.decode(loginParams);
    useName = loginResult['Alias'];
    userAcount = loginResult['UserName'];
    //计算七日预报的高度
    var sevHeight = $(document).height() - $('.todayFore').outerHeight(true) - 2*parseFloat($('.sevenFore').css('margin-top'));
    $('.sevenFore').outerHeight(sevHeight);
    $('#exportIndex').on('click', getUploadTxt);
    getSelFtp();
    $('#upload').click(ftpUpload);
});

//上传
function ftpUpload() {
    var value = $('#sName').val();
    var txt = $("#txt").val();
    var name = $('#sName option:selected').text();
    var mk = new Ext.LoadMask(document.body, {
        msg: '正在上传，请稍候！',
        removeMask: true //完成后移除  
    });
    if (!confirm("是否要上传？")) return;
    mk.show();
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'UploadFtp'),
        params: { address: value, txt: txt, type: "forecaster", num: "1",name:name },
        success: function (response) {
            var data = response.responseText;
            if (data == "success") {
                alert("上传成功！");
            } else {
                alert("上传失败！");
            }
            mk.hide();
        }
    });
}

//获取上传的文本
function getUploadTxt() {
    var hour = (new Date()).getHours();
    $(".sevenFore #txt").remove();
    var address = $("#sName").val();
    var name = $('#sName option:selected').text();
    if (name.indexOf("11点后")>-1) {
        if (hour < 11) {
            tip("失败！", "未能获取气象指数数据");
            return;
        } 
    }
    if (address == 0||address=="") {alert("请选择有效的地址！");return;}
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetUploadTxt'),
        params: { userAccount: userAcount, address: address, lst: "", type: "forecaster", num: "1", name_ftp: name },
        success: function (response) {
            // var data = eval(response.responseText);
            var data = response.responseText;
            if (data.length > 0) {
                $(".sevenFore").append($("<textarea id='txt' readonly='readonly'></textarea>"));
                $("#txt").height($('.sevenFore').height());
                $("#txt").width("94%");
                $('#txt').text(data);
                //$('#upload').removeAttr('disabled');
                $('#upload').attr('disabled', false);
                tip("成功！", "已成功获取气象指数数据");
            } else {
                tip("失败！", "未能获取气象指数数据");
            }
        }
    });
}

//点击导出气象数据时右小角的小提示框
function tip(txt1, txt2) {
    $(".title2").text(txt1);
    $(".div-txt span").text(txt2);
    $(".frame").css("display", "block");
    $(".frame").stop().animate({
        "bottom":"30px"
    }, 1000);
    setTimeout(function () {
        $(".frame").fadeOut(300, function () {
            $(this).css("bottom", "-90px");
         });
    }, 3000);
}
//获取select选择的下拉框
function getSelFtp() {
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.LiveIndex', 'GetSelFtp'),
        params: { type: "forecaster" },
        success: function (response) {
            var data = JSON.parse(response.responseText).data;
            var html = "<option value=0>==请选择==</option>", name = "", address = "";
            for (var i = 0; i < data.length; i++) {
                name = data[i][0];
                address = data[i][1];
                html += "<option value=" + address + ">" + name + "</option>";
            }
            $('#sName').html(html);
            $("#sName").val("0");
            //$("#sName").change(getFtpOption);
            $('#sName').change(function () {
                $('#upload').attr('disabled', true);
            });
        }
    });
}

function getFtpOption() {
}