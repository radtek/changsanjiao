
window.onload=function InitTable() {
    var name = "中心城区";
    $.ajax({
        url: "HealthyWeatherView.aspx/GetContents",
        type: "POST",
        contentType: "application/json",
        data: "{selectSite:'" + name + "'}",
        dataType: 'json',
        success: function (results) {
            $("#publish").html(results.d);
        },
        error: function (ex) {
            //alert("异常，" + ex.responseText + "！");
        }
    });
}