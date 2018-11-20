$(function () {
    var vm = new Vue({
        el: '#dataPro'
        , data: {
            startDate: new Date().Format("yyyy年MM月dd日")
            ,info:{
                "fileTxt":{"label":[
                             {text:'数值报文件日期'}
                            ,{text:'城镇报文件日期'}
                            ,{text:'中心台获取日期'}
                            ,{text:'欧洲报文件日期'}
                            ,{text:'臭氧报文件日期'}
                            ,{text:'紫外线文件日期'}
                            ,{text:'精细报文件日期'}
                            ,{text:'能见度文件日期'}
                            ,{text:'上传文件日期'}
                         ],
                        "header":'FTP服务器下载（上传）数据信息'
                }
                ,"eleInfo":{"label":[
                                 {text:'气象要素信息'}
                                ,{text:'气象指数信息'}
                            ],
                    "header":'要素数据信息'
                }
            }
            , btnData: [
                  { text: '获取气象要素数据' }
                , { text: '气象要素计算' }
                , { text: '气象指数预报' }
                , { text: '系统业务数据备份' }
                , { text: '上传指数预报' }
                , { text: '短信分析' }
            ]
            ,source:''
        }
        , components: {
            libtn: {
                props: ['msg', 'dataIndex'],
                template: `<li @mouseleave="$emit('leave',$event)" @mouseenter="$emit('enter',$event)" @click="$emit('dj',$event)"><a>{{dataIndex+1}}、{{msg}}</a></li>`
                //template: '<li @mouseleave='+$emit('leave',$event)+' @mouseenter="$emit(enter,$event)" @click="$emit(dj,$event)"><a>{{dataIndex+1}}、{{msg}}</a></li>'
            }

        }
        , methods: {
            _click: function (el) {
                this.$nextTick(function(){
                    $(".liBtn").removeClass('active');
                    $(el.currentTarget).addClass('active');
                    $(el.currentTarget).find("a").css("color","black");
                });
            },
            _enter:function(el){
                this.$nextTick(function(){
                    $(".liBtn").css("background-color",'#efefef');
                    $(el.currentTarget).css({"background":'rgb(57,120,163)'});
                    $(el.currentTarget).find("a").css("color","white");
                });
            },
            _leave:function(el){
                this.$nextTick(function(){
                    $(".liBtn").css("background-color",'#efefef');
                    $(el.currentTarget).css("background-color",'#efefef');
                    $(el.currentTarget).find("a").css("color","black");
                });
            }
        }
});
});
