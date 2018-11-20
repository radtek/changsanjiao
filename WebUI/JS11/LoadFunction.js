var loginParams = getCookie("UserInfo");
var totalModuleList=['outPdPreview','InteractiveOUT','productMake','dataService'];
if (loginParams != "") {
    result = Ext.util.JSON.decode(loginParams);
    var logJB = result["JB"];
    var funModules = result["Functions"].split(',');
    var useModules = new Array();
    var strWebGISType = "webGIS";
    var strMAKEOnclick = "ReportWorkArea,reprotProduce,EastChinaReprotProduce";
    var strDataOnclick = "WeatherPollution,SuperStation";
    //江西权限
    if(logJB == "999") {
        strWebGISType = "webGISJiangXi";
        strMAKEOnclick = "JiangXiAQIPart";
    }
    else {
        if (funModules.length > 0) {
            for (var i = 0; i < funModules.length; i++) {
                useModules.push(totalModuleList[funModules[i]-1]);
            }
        }
    }

    if (useModules.length > 0) {
        for (var j = 0; j < useModules.length; j++) {
            //产品预览模块
            if (useModules[j] == "outPdPreview") {
                $("#" + useModules[j]).removeClass();
                $("#" + useModules[j]).addClass("outPdPreview");
                
                $("#" + useModules[j]).live("click", function () {
                    tomainviewer('airQuality,jgRadar,guidance,diagnostic');
                });
            }
            else if (useModules[j] == "InteractiveOUT") {
                $("#" + useModules[j]).removeClass();
                $("#" + useModules[j]).addClass("InteractiveOUT");
                $("#" + useModules[j]).live("click", function () {
                    tomainviewer(strWebGISType, 'webGIS1');
                });
               
            }
            else if (useModules[j] == "productMake") {
                $("#" + useModules[j]).removeClass();
                $("#" + useModules[j]).addClass("MakeOUT");
                $("#" + useModules[j]).live("click", function () {
                   tomainviewer(strMAKEOnclick); 
                });
           }
           else if (useModules[j] == "dataService") {
               $("#" + useModules[j]).removeClass();
               $("#" + useModules[j]).addClass("DataserviceOUT");
               $("#" + useModules[j]).live("click", function () {
                   tomainviewer(strDataOnclick);
               });
           }
        }
    }
}

 
  
