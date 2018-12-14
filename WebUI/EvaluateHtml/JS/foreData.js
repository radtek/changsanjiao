var thead = [
    {
        label: "日期",
        property: "date",
        row:3,
        sub:[]
    },
    {
        label: "预报员",
        property: "userName",
        row: 3,
        sub: []
    },
    {
        label: "霾预报",
        property: "haze",
        row: 2,
        col: 2,
        sub: [
            {
                label: "05时",
                property: "05haze",
            }, {
                label: "17时",
                property: "17haze",
            },
        ]
    },
    {
        label: "UV预报",
        property: "uv",
        row: 2,
        col: 2,
        sub: [
            {
                label: "10时",
                property: "10uv",
            }, {
                label: "16时",
                property: "16uv",
            },
        ]
    },
    {
        label: "国家局预报",
        property: "chinaFore",
        row: 2,
        col: 4,
        sub: [
            {
                label: "PM25",
                property: "china_PM25",
            }, {
                label: "PM10",
                property: "china_PM10",
            },
             {
                 label: "NO2",
                 property: "china_NO2",
             }, {
                 label: "O3",
                 property: "china_O3",
             },
        ]
    },
    {
        label: "24小时浓度",
        property: "24aqi",
        row: 2,
        col:5,
        sub: [
         {
             label: "PM25",
             property: "24hour_PM25",
         }, {
             label: "PM10",
             property: "24hour_PM10",
         }, {
            label: "NO2",
            property: "24hour_NO2",
        }, {
            label: "O3-1h",
            property: "24hour_O31",
        }, {
            label: "O3-8h",
            property: "24hour_O38",
        },
        ]
    },
    {
        label: "分时段预报",
        property: "periodFore",
        row: 1,
        col: 25,
        sub: [
            {
                label: "上半夜",
                property: "m_night",
                row: 1,
                col: 5,
                sub: [
                    {
                        label: "PM25",
                        property: "m_night_PM25",
                    }, {
                        label: "PM10",
                        property: "m_night_PM10",
                    }, {
                        label: "NO2",
                        property: "m_night_NO2",
                    }, {
                        label: "O3-1h",
                        property: "m_night_O31",
                    }, {
                        label: "O3-8h",
                        property: "m_night_O38",
                    },
                ]
            },
            {
                label: "下半夜",
                property: "n_night",
                row: 1,
                col: 5,
                sub: [
                    {
                        label: "PM25",
                        property: "n_night_PM25",
                    }, {
                        label: "PM10",
                        property: "n_night_PM10",
                    }, {
                        label: "NO2",
                        property: "n_night_NO2",
                    }, {
                        label: "O3-1h",
                        property: "n_night_O31",
                    }, {
                        label: "O3-8h",
                        property: "n_night_O38",
                    },
                ]
            },
             {
                 label: "上午",
                 property: "morning",
                 row: 1,
                 col: 5,
                 sub: [
                     {
                         label: "PM25",
                         property: "morning_PM25",
                     }, {
                         label: "PM10",
                         property: "morning_PM10",
                     }, {
                         label: "NO2",
                         property: "morning_NO2",
                     }, {
                         label: "O3-1h",
                         property: "morning_O31",
                     }, {
                         label: "O3-8h",
                         property: "morning_O38",
                     },
                 ]
             },
             {
                 label: "下午",
                 property: "afternoon",
                 row: 1,
                 col: 5,
                 sub: [
                     {
                         label: "PM25",
                         property: "afternoon_PM25",
                     }, {
                         label: "PM10",
                         property: "afternoon_PM10",
                     }, {
                         label: "NO2",
                         property: "afternoon_NO2",
                     }, {
                         label: "O3-1h",
                         property: "afternoon_O31",
                     }, {
                         label: "O3-8h",
                         property: "afternoon_O38",
                     },
                 ]
             },
             {
                 label: "上半夜（明）",
                 property: "m_night_next",
                 row: 1,
                 col: 5,
                 sub: [
                     {
                         label: "PM25",
                         property: "m_night_next_PM25",
                     }, {
                         label: "PM10",
                         property: "m_night_next_PM10",
                     }, {
                         label: "NO2",
                         property: "m_night_next_NO2",
                     }, {
                         label: "O3-1h",
                         property: "m_night_next_O31",
                     }, {
                         label: "O3-8h",
                         property: "m_night_next_O38",
                     },
                 ]
             }
        ]
    }
]
Vue.component("my-component", {
    template: "#template",
    props: {
        colname: Array,
        parentxt: String,
        col:Number
    }
});
var vm = new Vue({
    el: "#app",
    data: {
        "thead": thead,
    },
});