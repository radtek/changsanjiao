var data = {
    //sites: [{ "name": "站点", "value": [] }]
    info: [{ "name": "来源：", "value": "环境保护科", "id": "source" }
        //, { "name": "预报员：", "value": "张三", "id": "forecaster" }
        , { "name": "文件名称：", "value": "/oyfwcp/nwp/", "id": "fileName" }
        , { "name": "更新日期：", "value": "2017-12-07 02:00:00", "id": "time" }
        , { "name": "IP：", "value": "172.21.3.50", "id": "ip" }
        , { "name": "描述：", "value": "描述", "id": "description" }
    ]
};
var vm = new Vue({
    el: "#app",
    data: {
        data: data
        , left: ["数值预报", "中心预报", "ECMWF预报", "区县城镇预报", "会商预报"]
        , selected: 0
        , hover: 0
        ,color:0
    }
     , mounted: function () {
         this.$nextTick(function () {
             this.getModuleName();
             //this.init();
         })
     }
    , methods: {
        getModuleName: function () {
            this.$http.post('DataSource.aspx/GetModuleName').then(function (response) {
                var json = response.data.d.rows;
                if (json.length > 0) {
                    this.left.length = 0;
                    for (i = 0; i < json.length; i++) {
                        this.left.push(json[i].SrcInfo);
                    }
                    this.click(0, event);
                }
            });
        }
        , click: function (index,event) {
            this.selected = index;
            this.color = index;
            var type = this.left[index];
            this.$http.post('DataSource.aspx/GetInfo', { type: type }).then(function (response) {
                var data = eval('(' + response.data.d + ')').data[0];
                for (i = 0; i < data.length; i++) {
                    this.data.info[i].value = data[i];
                }
            });
        }
       , enter: function (index,event) {
           this.hover = index;
       }
       , leave: function () {
           this.hover = -1;
       }
    }
});