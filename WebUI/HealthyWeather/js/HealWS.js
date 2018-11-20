//$(function () {
    
//})
function confirm() {
    var key = $("#key").val();
    if (key == "") {
        alert("密钥不能为空");
        return;
    }
    
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.HealWS', 'GetCrows'),
        params: { authCode: key },
        success: function (response) {
            document.getElementById("xmll").innerText = response.responseText;
            
        }
    });
}