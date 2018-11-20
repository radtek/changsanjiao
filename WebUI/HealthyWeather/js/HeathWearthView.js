
window.onload=function InitTable() {
    var name = "中心城区";
    $.ajax({
        url: "HeathWearthView.aspx/GetContents",
        type: "POST",
        contentType: "application/json",
        data: "{selectSite:'" + name + "'}",
        dataType: 'json',
        success: function (results) {
            $("#ganmaozhongshu").html(results.d);
        },
        error: function (ex) {
            //alert("异常，" + ex.responseText + "！");
        }
    });
}