var hour = new Date().Format("hh");
var day = -1;
if (hour >= 12) {
    day = 0;
}
var date1 = addDate(day).Format("MM月dd日");
var date2 = addDate(day+1).Format("MM月dd日");
var date3 = addDate(day+2).Format("MM月dd日");
var date4 = addDate(day+3).Format("MM月dd日");
function addDate(days) {
    var d = new Date();
    d.setDate(d.getDate() + days);
    return d;
}
var result =
{
    "row1": {
        "Period": [{ "val": "24小时"}, { "rowspan": "5"}, {"colspan": "1"}, { "class": "dis"}],
		"Date": [{ "val": date1}, {"rowspan": "3"}, {"colspan": "1"}, {"class": "dis"}],"Interval": [{"val": "上半夜"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
			"O31": [{"val": "/"}, { "val": "/"}, {"val": "/"}],
			"O38": [{"val": "/"}, { "val": "/"}, { "val": "/"}],
			"AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
    },
    "row2": {
        "Period": [{ "val": "24小时" }, { "rowspan": "5" }, { "colspan": "1" }, { "class": "indis" }],
		"Date": [{ "val": date1}, {"rowspan": "3"}, {"colspan": "1"}, {"class": "indis"}],
		"Interval": [{"val": "下半夜"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	},
	"row3": {
	    "Period": [{"val": "24小时"}, {"rowspan": "5"}, {"colspan": "1"}, {"class": "indis" }],
		"Date": [{ "val": date1}, { "rowspan": "3"}, {"colspan": "1"}, { "class": "indis"}],
		"Interval": [{ "val": "夜间"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	},
	"row4": {
	    "Period": [{"val": "24小时" }, {"rowspan": "5"}, {"colspan": "1" }, {"class": "indis"}],
		"Date": [{"val": date2}, {"rowspan": "2"}, {"colspan": "1"}, {"class": "dis"}],
		"Interval": [{"val": "上午"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	},
	"row5": {
	    "Period": [{"val": "24小时"}, {"rowspan": "5"}, {"colspan": "1"}, {"class": "indis"}],
		"Date": [{"val": date2}, {"rowspan": "2"}, {"colspan": "1"}, {"class": "indis"}],
		"Interval": [{ "val": "下午"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	},
	"row6": {
	    "Period": [{"val": "48小时"}, {"rowspan": "5"}, { "colspan": "1" }, { "class": "dis" }],
		"Date": [{ "val": date2}, {"rowspan": "3"}, { "colspan": "1"}, {"class": "dis"}],
		"Interval": [{ "val": "上半夜"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	},
	"row7": {
	    "Period": [{ "val": "48小时"}, {"rowspan": "5"}, {"colspan": "1"}, { "class": "indis"}],
		"Date": [{ "val": date2}, {"rowspan": "3"}, { "colspan": "1"}, {"class": "indis"}],
		"Interval": [{ "val": "下半夜"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	},
	"row8": {
	    "Period": [{"val": "48小时" }, {"rowspan": "5"}, { "colspan": "1"}, { "class": "indis" }],
		"Date": [{"val": date2}, {"rowspan": "3"}, {"colspan": "1"}, { "class": "indis"}],
		"Interval": [{ "val": "夜间"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	},
	"row9": {
	    "Period": [{"val": "48小时"}, {"rowspan": "5"}, {"colspan": "1" }, {"class": "indis" }],
		"Date": [{ "val": date3}, {"rowspan": "2"}, {"colspan": "1"}, {"class": "dis"}],
		"Interval": [{"val": "上午"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	},
	"row10": {
	    "Period": [{"val": "48小时" }, { "rowspan": "5"}, {"colspan": "1"}, {"class": "indis"}],
		"Date": [{"val": date3}, {"rowspan": "2"}, { "colspan": "1"}, { "class": "indis"}],
		"Interval": [{"val": "下午"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	},
	"row11": {
	    "Period": [{ "val": "72小时"}, { "rowspan": "2"}, {"colspan": "1"}, {"class": "dis" }],
		"Date": [{"val": date3}, {"rowspan": "1"}, { "colspan": "1"}, { "class": "dis"}],
		"Interval": [{"val": "上半夜"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	},
	"row12": {
	    "Period": [{"val": "72小时" }, {"rowspan": "2" }, {"colspan": "1" }, {"class": "indis"}],
		"Date": [{ "val": date4}, {"rowspan": "1"}, {"colspan": "1"}, {"class": "dis"}],
		"Interval": [{ "val": "全天"}],
		"Ele": {
		    "AQI": [{"val": "","poll": ""}, {"val": "","poll": "" }, {"val": "","poll": ""}]
		}
	},
	"row13": {
	    "Period": [{"val": "日平均" }, {"rowspan": "2"}, {"colspan": "1" }, {"class": "dis" }],
		"Date": [{"val": date2}, {"rowspan": "1"}, {"colspan": "1"}, {"class": "dis"}],
		"Interval": [{ "val": "日平均"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	},
	"row14": {
	    "Period": [{"val": "日平均"}, {"rowspan": "2"}, {"colspan": "1"}, {"class": "indis"}],
		"Date": [{"val": date2}, {"rowspan": "1"}, { "colspan": "1"}, {"class": "dis"}],
		"Interval": [{"val": "日平均"}],
		"Ele": {
		    "PM25": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "PM10": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "NO2": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O31": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "O38": [{ "val": "/" }, { "val": "/" }, { "val": "/" }],
		    "AQI": [{ "val": "/" }, { "val": "/" }, { "val": "/" }]
		}
	}
}
function getData() {
    return result;
}
