//Ext.onReady(function () {
//    var txtContent = $("#txtHidePeriodAQITxtTemplete").text();
//    alert($("txtHidePeriodAQITxtTemplete"));
//    var win = new Ext.Panel({
//        width: 1000,
//        height: 500,
//        layout: 'fit', //设置窗口内部布局
//        renderTo: "aqiPeriodDetail",
//        items: new Ext.TabPanel({//窗体中中是一个一个TabPanel
//            autoTabs: true,
//            activeTab: 0,
//            deferredRender: false,
//            border: false,
//            buttonAlign: "center",
//            items: [
//                        {
//                            id: "tabTxt",
//                            title: '24小时AQI分时段预报',
//                            html: '<textarea id="textArea" class="textPrev">'+txtContent+'</textarea>' // 内部显示内容
//                        },
//                        {
//                            id: "tabMsg",
//                            title: 'AQI分时段预报短信',
//                            html: '<textarea id="msgArea" class="textPrev">123</textarea>'
//                        }
//                    ]
//        })
//    });
//            });

            //获取AQI分时段的文本内容
            function GetAQIPeriodTxt(releaseDate) {
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.AQIForecast', 'GetAQIPeriodLogDetail'),
                    params: { releaseTime: releaseDate },
                    success: function (response) {
                        if (response.responseText == "success") {
                            alert("保存成功");
                        }
                        else {
                            alert("保存失败");
                        }
                    },
                    failure: function (response) {
                        Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                    }
                });
            }


            //获取AQI分时段的短信内容
            function GetAQIPeriodMsg() {
               
            }