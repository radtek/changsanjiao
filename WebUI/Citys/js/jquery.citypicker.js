/**
 * @作者 周晶
 * @版本 jquery.citypicker.js(version="1.0.0")
 * @使用方法：
 * 1.jquery.citypicker就jquery插件，使用此插件前必须先引用jquery.js库文件
 * 2.再引用此库文件，引用方式：<script type="text/javascript" src="jquery.citypicker.js"></script>
 * 3.引用CSS样式文件和图片：<link type="text/css" href="css/jquery.citypicker.css" rel="stylesheet" />
 */
cities = new Array();
pro = new Array("热门城市", "直辖市", "河北", "山西", "辽宁", "吉林", "江苏", "浙江", "安徽", "福建", "江西", "山东", "河南", "湖北", "湖南", "广东", "广西", "海南", "四川", "贵州", "云南", "陕西", "甘肃", "青海", "宁夏", "新疆", "内蒙", "黑龙江");
cities['热门城市'] = new Array("上海", "苏州", "杭州", "南京","合肥","济南");
cities['直辖市'] = new Array("北京", "上海", "重庆", "天津");
cities['河北'] = new Array("唐山", "邯郸", "保定", "承德", "秦皇岛", "石家庄");
cities['山西'] = new Array("太原", "大同", "阳泉", "长治", "晋城", "晋中", "运城", "临汾");
cities['内蒙'] = new Array("呼和浩特", "呼伦贝尔", "包头", "乌海", "赤峰", "兴安盟");
cities['辽宁'] = new Array("沈阳", "大连", "鞍山", "抚顺", "本溪", "丹东", "锦州", "阜新", "辽阳", "铁岭");
cities['吉林'] = new Array("长春", "吉林", "四平", "辽源", "通化", "松原");
cities['黑龙江'] = new Array("哈尔滨", "大兴安岭", "鸡西", "大庆", "黑河", "齐齐哈尔");
cities['江苏'] = new Array("南京", "无锡", "徐州", "常州", "苏州", "南通", "扬州", "镇江", "连云港");
cities['浙江'] = new Array("杭州", "宁波", "温州", "嘉兴", "绍兴", "金华", "衢州", "台州", "丽水");
cities['安徽'] = new Array("合肥", "芜湖", "蚌埠", "淮南", "淮北", "铜陵", "安庆", "黄山", "马鞍山");
cities['福建'] = new Array("福州", "厦门", "莆田", "三明", "泉州");
cities['江西'] = new Array("南昌", "九江", "鹰潭", "赣州", "上饶", "景德镇");
cities['山东'] = new Array("济南", "青岛", "烟台", "德州", "聊城", "滨州", "荷泽");
cities['河南'] = new Array("郑州", "开封", "洛阳", "安阳", "许昌", "南阳", "周口", "驻马店", "三门峡");
cities['湖北'] = new Array("武汉", "黄石", "宜昌", "荆门", "荆州", "黄冈", "咸宁", "恩施");
cities['湖南'] = new Array("长沙", "湘潭", "岳阳", "常德", "益阳", "郴州", "湘西", "张家界","株洲");
cities['广东'] = new Array("广州", "韶关", "深圳", "珠海", "汕头", "佛山", "江门", "湛江", "惠州", "东莞", "中山");
cities['广西'] = new Array("南宁", "柳州", "桂林", "北海", "玉林");
cities['海南'] = new Array("海口", "三亚", "海南沿革");
cities['四川'] = new Array("成都", "泸州", "德阳", "绵阳", "乐山", "眉山", "宜宾", "攀枝花");
cities['贵州'] = new Array("贵阳", "遵义", "安顺", "铜仁", "六盘水");
cities['云南'] = new Array("昆明", "曲靖", "玉溪", "保山", "丽江", "大理");
cities['陕西'] = new Array("西安", "铜川", "宝鸡", "咸阳", "延安", "汉中");
cities['甘肃'] = new Array("兰州", "金昌", "白银", "天水", "嘉峪关");
cities['青海'] = new Array("西宁", "海东", "海北", "黄南", "海南", "果洛", "玉树", "海西");
cities['宁夏'] = new Array("银川", "吴忠", "固原", "中卫");
cities['新疆'] = new Array("乌鲁木齐", "克拉玛依", "哈密", "伊犁", "吐鲁番");



function Bind() {
    $.each($(".pro .pro1"), function (i, n) {
        $(n).click(function () {
            $("#citypicker_city").empty();
            for (c in cities[$(this).text()]) {
                var lb = cities[$(this).text()][c];
                if (lb.indexOf("function") < 0) {
                    if (c == 0) {
                        $("#citypicker_city").append("<label id='city" + c + "' class='pro2' style='color:white;' onclick='clk(this)'>" + lb + "</label>");
                    } else {
                        $("#citypicker_city").append("<label id='city" + c + "' class='pro2' onclick='clk(this)'>" + lb + "</label>");
                    }
                }
            }
        });
    });
}

function clk(obj) {
    $("#city").val(obj.innerHTML + '市');
    obj.style.color = "blue";

    $.each($(".cityy .pro2"), function (i, n) {
            var t = $(n).text();
            if (t != obj.innerHTML) {
                document.getElementById(("city" + i)).style.color = "white";
            }
        });

        doQueryChart(obj.innerHTML + '市');
}
