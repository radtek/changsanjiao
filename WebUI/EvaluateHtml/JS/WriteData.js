function cDayFunc(){
    var newDateStr=$dp.cal.newdate['y']+"-"+$dp.cal.newdate['M']+"-"+$dp.cal.newdate['d'];
    var newDate=new Date(newDateStr).Format("yyyy-MM-dd");
    if(newDate!=new Date().Format("yyyy-MM-dd")){
        document.getElementById("confirm").disabled=true;
        vm.his=true;
    }else{
        document.getElementById("confirm").disabled=false;
        vm.his=false;
        if(new Date().Format("h")>=18){   //18点过后不能编辑
            vm.his=true;
        }
    }
}
var group=[{
    "header": "霾预报",
    "edit": true,
    "className": "mai",
    "data": {
        "05时值": "",
        "17时值": ""
    }
}, {
    "header": "紫外线预报",
    "edit": true,
    "className": "UV",
    "data": {
        "10时值": "",
        "16时值": ""
    }
}, {
    "header": "国家局预报",
    "edit": true,
    "className": "country",
    "data": {
        "PM25": "",
        "PM10": "",
        "NO2": "",
        "O3": ""
    }
},{
    "header": "24小时浓度预报",
    "edit": true,
    "className": "aqi24",
    "data": {
        "PM2.5": "",
        "PM10": "",
        "&nbsp&nbspNO2": "",
        "03-1h":"",
        "03-8h": "",
        
        
    }
}];
var periodData={
    "header": "分时段预报",
    "edit": true,
    "className": "period",
    "data": {
        "上半夜": [
            { "val": "","isEdit":false,"poll":"PM25"},
            {"val": "","isEdit":false,"poll":"PM10"},
            {"val": "","isEdit":false,"poll":"NO2"},
            {"val": "","isEdit":false,"poll":"03-1h"},
            {"val": "","isEdit":false,"poll":"03-8h"}
        ],
        "下半夜": [
            { "val": "","isEdit":false,"poll":"PM25"},
            {"val": "","isEdit":false,"poll":"PM10"},
            {"val": "","isEdit":false,"poll":"NO2"},
            {"val": "","isEdit":false,"poll":"03-1h"},
            {"val": "","isEdit":false,"poll":"03-8h"}
        ],
        "上午":[
           { "val": "","isEdit":false,"poll":"PM25"},
            {"val": "","isEdit":false,"poll":"PM10"},
            {"val": "","isEdit":false,"poll":"NO2"},
            {"val": "","isEdit":false,"poll":"03-1h"},
            {"val": "","isEdit":false,"poll":"03-8h"}
        ],
        "下午": [
            { "val": "","isEdit":false,"poll":"PM25"},
            {"val": "","isEdit":false,"poll":"PM10"},
            {"val": "","isEdit":false,"poll":"NO2"},
            {"val": "","isEdit":false,"poll":"03-1h"},
            {"val": "","isEdit":false,"poll":"03-8h"}
        ],
        "上半夜(明)": [
            { "val": "","isEdit":false,"poll":"PM25"},
            {"val": "","isEdit":false,"poll":"PM10"},
            {"val": "","isEdit":false,"poll":"NO2"},
            {"val": "","isEdit":false,"poll":"03-1h"},
            {"val": "","isEdit":false,"poll":"03-8h"}
        ]
    }
}

Vue.component("my-component", {
    template: "#template",
    props: {
        group: Object
    }
});
var vm = new Vue({
    el: "#app",
    data: {
        "date": new Date().Format("yyyy-MM-dd"),
        "groups": group,
        "periodData": periodData,
        "his":false,
        "userName":""
    },
    mounted: function () {
        this.$nextTick(function () {
            if(new Date().Format("h")>=18){   //18点过后不能编辑
                this.his=true;
            }
            this.init();
            this.getData();
        });
    },
    methods: {
        init: function () {
            var loginParams = getCookie('UserInfo');
            var loginResult = eval('(' + loginParams + ')');
            this.userName = loginResult['Alias'];
            //user = loginResult['UserName'];
            document.getElementById("forecaster").innerText = this.userName;
            var bodyWidth = document.body.offsetWidth;
            var wrap = document.getElementsByClassName("wrap");
            var pageRightObj = document.getElementsByClassName("page-right")[0];
            var left = (bodyWidth - wrap[0].offsetWidth - pageRightObj.offsetWidth) / 2; //计算左边三个框的left值
            for (let i = 0; i < wrap.length; i++) {
                wrap[i].style.left = left + "px";
                wrap[i].style.opacity = 1;
            }
            var right = bodyWidth - (left + wrap[0].offsetWidth) - pageRightObj.offsetWidth;  //计算右边框的right值
            pageRightObj.style.right = right - 10 + "px";
            pageRightObj.style.opacity = 1;
        },
        getData: function () {
            let time = document.getElementById("time").value;
            this.$http.post('WriteData.aspx/GetData', { time: time }).then(function (response) {
                let data = response.data.d.split("#");
                data.forEach(function (ele, i) {
                   // try{
                        var obj = eval('(' + ele + ')');
                    //}catch{}
                    for (let key in obj) {
                        if (key=="dataPeriod") {
                            vm.periodData.data = obj[key];
                            return;
                        }
                        vm.groups[i].data = obj[key];
                    }
                });
            })
        },
        confirm: function () {
            if (!confirm("您是否确认数据无误？")) return;
            let data = this.getSaveData();
            this.$http.post('WriteData.aspx/Confirm',{data:data,userName:this.userName}).then(function (response) {
                let data = response.data.d;
                if (data == "ok") {
                    alert("已确认！");
                } else {
                    alert("确认失败！");
                }
            })
        },
        focus:function(ev,isEdit){
            if(isEdit==1){
                ev.currentTarget.style.textAlign="left";
                ev.currentTarget.select();
            }
        },
        blur:function(ev){
            ev.currentTarget.style.textAlign="center";
        },
        getSaveData:function (){
            let data=this.periodData.data;
            var arrObj =new Array();
            for(let key in data){
                data[key].map(function (item,index,input){
                    if(item.isEdit=="1"){   //表示可以编辑的
                        let obj= {};
                        obj["poll"]=item.poll;
                        obj["val"]=item.val;
                        obj["duration"]=key;
                        arrObj.push(JSON.stringify(obj));
                    }
                })
            }
            return JSON.stringify(arrObj);
        }
    }
});

