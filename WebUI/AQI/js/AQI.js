var oldID = "H07";
Ext.onReady(function () {
    //    supportInnerText(); //使得火狐支持innerText
    today();
});

function imageChange(el) {
    if (oldID != el.id) {
        var value = el.innerHTML;
        var nowDate = convertDate(value);
        getImg(nowDate);
        el.className = "foucs";
        Ext.getDom(oldID).className = "line";
        oldID = el.id;
    }
}

function today() {
    var nowDate = new Date();
    getImg(nowDate);
    if (oldID != "H07") {
        Ext.getDom("H07").className = "foucs";
        Ext.getDom(oldID).className = "line";
        oldID = "H07";
    }
}

function changeDate(el) {
    var value = el.value;
    var nowDate = convertDate(value);
    getImg(nowDate);
}

function getImg(nowDate) {    
    var trueTime = nowDate.add('d', -1);
    var year = trueTime.getFullYear();
    var month = trueTime.getMonth() + 1;
    var day = trueTime.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var src = "../Product/PollutionWeather/" + year + "/" + year + month + day + "/";
    Ext.getDom("M0").src = src + "C_24AQI_M0_" + year + month + day + "0000_000.png";
    Ext.getDom("M1").src = src + "C_24AQI_M1_" + year + month + day + "0000_000.png";
    Ext.getDom("M2").src = src + "C_24AQI_M2_" + year + month + day + "0000_000.png";
    Ext.getDom("M3").src = src + "C_24AQI_M3_" + year + month + day + "0000_000.png";
    Ext.getDom("M4").src = src + "C_24AQI_M4_" + year + month + day + "0000_000.png";
    Ext.getDom("M5").src = src + "C_24AQI_M5_" + year + month + day + "0000_000.png";
}