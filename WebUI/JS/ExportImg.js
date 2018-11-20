Ext.onReady(function () {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    InitTableTest();

    var chart_PM25 = $("#container0").highcharts();
    var chart_PM10 = $("#container1").highcharts();
    var chart_NO2 = $("#container2").highcharts();
    var svg = chart_PM25.getSVG() + "&" + chart_PM10.getSVG() + "&"+chart_NO2.getSVG();
    $("#svgContent").val(svg);
});


function InitTable() {
//    var dateTime = Ext.getDom("H00").value;
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.EvalutionCaculate', 'IAQIChart'),
        params: { dateTime: dateTime },
        success: function (response) {
            if (response.responseText != "") {
                var result = Ext.util.JSON.decode(response.responseText);
                for (var obj in result) {
                    if (obj == "0" || obj == "1" || obj == "2")
                    RenderChart(result[obj], obj);
                }
            }
            else
                Ext.Msg.alert("提示", "没有满足条件的信息。");
        },
        failure: function (response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });
}

function InitTableTest() {
    $('#container0').highcharts({
        title: {
            text: 'Monthly Average Temperature',
            x: -20 //center
        },
        subtitle: {
            text: 'Source: WorldClimate.com',
            x: -20
        },
        xAxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
        },
        yAxis: {
            title: {
                text: 'Temperature (°C)'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: '°C'
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: [{
            name: 'Tokyo',
            data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
        }, {
            name: 'New York',
            data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5]
        }, {
            name: 'Berlin',
            data: [-0.9, 0.6, 3.5, 8.4, 13.5, 17.0, 18.6, 17.9, 14.3, 9.0, 3.9, 1.0]
        }, {
            name: 'London',
            data: [3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8]
        }],
        exporting: {
            enabled: true,
            buttons: {
                exportButton: {
                    menuItems: [{
                        text: '导出PNG图片(宽度为250px)',
                        onclick: function () {
                            this.exportChart({
                                width: 200 //导出报表的宽度  
                            });
                        }
                    }, {
                        text: '导出PNG图片(宽度为800px)',
                        onclick: function () {
                            this.exportChart(); // 800px by default  
                        }
                    },
                     null,
                     null
                     ]
                },
                printButton: {
                    enabled: false
                }
            }
        }
    });

    $('#container1').highcharts({
        title: {
            text: 'Monthly Average Temperature',
            x: -20 //center
        },
        subtitle: {
            text: 'Source: WorldClimate.com',
            x: -20
        },
        xAxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
        },
        yAxis: {
            title: {
                text: 'Temperature (°C)'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: '°C'
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: [{
            name: 'Tokyo',
            data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
        }, {
            name: 'New York',
            data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5]
        }, {
            name: 'Berlin',
            data: [-0.9, 0.6, 3.5, 8.4, 13.5, 17.0, 18.6, 17.9, 14.3, 9.0, 3.9, 1.0]
        }, {
            name: 'London',
            data: [3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8]
        }],
        exporting: {
            enabled: true,
            buttons: {
                exportButton: {
                    menuItems: [{
                        text: '导出PNG图片(宽度为250px)',
                        onclick: function () {
                            this.exportChart({
                                width: 200 //导出报表的宽度  
                            });
                        }
                    }, {
                        text: '导出PNG图片(宽度为800px)',
                        onclick: function () {
                            this.exportChart(); // 800px by default  
                        }
                    },
                     null,
                     null
                     ]
                },
                printButton: {
                    enabled: false
                }
            }
        }
    });

    $('#container2').highcharts({
        title: {
            text: 'Monthly Average Temperature',
            x: -20 //center
        },
        subtitle: {
            text: 'Source: WorldClimate.com',
            x: -20
        },
        xAxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
        },
        yAxis: {
            title: {
                text: 'Temperature (°C)'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: '°C'
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: [{
            name: 'Tokyo',
            data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
        }, {
            name: 'New York',
            data: [-0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5]
        }, {
            name: 'Berlin',
            data: [-0.9, 0.6, 3.5, 8.4, 13.5, 17.0, 18.6, 17.9, 14.3, 9.0, 3.9, 1.0]
        }, {
            name: 'London',
            data: [3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8]
        }],
        exporting: {
            enabled: true,
            buttons: {
                exportButton: {
                    menuItems: [{
                        text: '导出PNG图片(宽度为250px)',
                        onclick: function () {
                            this.exportChart({
                                width: 200 //导出报表的宽度  
                            });
                        }
                    }, {
                        text: '导出PNG图片(宽度为800px)',
                        onclick: function () {
                            this.exportChart(); // 800px by default  
                        }
                    },
                     null,
                     null
                     ]
                },
                printButton: {
                    enabled: false
                }
            }
        }
    });
}