var mapPanel;
var forecastPanel;

var isQuery=false;

var dateNumId=0;

var loadingMask;

var marker = null;

var geocoder; 
var map;
var imageViewer = new ImageViewer(Ext.BLANK_IMAGE_URL);


function showLoadingMask()
{
	loadingMask=new Ext.LoadMask(Ext.getBody(), {msg:"数据读取中,请稍候.."});
	loadingMask.show();
}

function hideLoadingMask()
{
	loadingMask.hide();
}

function refreshResultTable()
{
	document.getElementById('resultTable').src="table.html";
}

function initMapPanel()
{
	mapPanel = new Ext.Panel({
		region:'center',
		frame:true,
		html:"<div id='map_canvas' style='width: 100%; height: 100%'></div>"
	})
}

function showLabelInfo(_string)
{
	Ext.getCmp("textLabel").setText(_string);
}

function initForecastPanel()
{

    var tabs = new Ext.TabPanel({
        activeTab: 1,
        items: [{
            title: '9天预报',
            html: "<iframe id='resultTable' src='table.html'frameborder='0' height='100%' width='100%' style='margin: 0px'></iframe>"
        },{
            title: '形势分析',
            id: 'fImage',
            layout:'border',// 边框布局
            items:[imageViewer]
        }]
    });


    imageViewer.on("afterrender",function(e){
		tabs.setActiveTab(0);
	});
	
	
	forecastPanel = new Ext.Panel({
		region:'west',
		frame:true,
		title:'任意点预报',
		width:450,
		border:false,
		layout:'border',
		items:
		[{
			xtype:'fieldset',
			title:'请选择操作方式',
			region:'north',
			height:50,
			margins:{top:0, right:0, bottom:10, left:0},
			items:
            [{
                //第一行，查询条件
                layout: 'column',
                items:
                [{
                    columnWidth: .8,
                    layout: 'form',
                    items: [{  xtype:'label',id:'textLabel',text:'点击"查询"，获取地图上任意位置的气象预报'}]
                },{
                    columnWidth: .2,
                    layout: 'form',
                    items: [{  xtype:'button',width:60,text:'查  询',
                        handler:function(){
							isQuery=true;
							dateNumId=0;
					 		if(marker!=null)
					 			marker.setMap(null);
					 		//showLabelInfo('点击"查询"，在地图上任意处点击将获取该地区的气象预报');
						}
                    }]
                }]
            }, {
                //第二行，查询条件
                layout: 'column',
                items:
                [{
                    columnWidth: .8,
                    layout: 'form',
                    labelWidth: 80,
                    items: [{ xtype:"textfield", id:"address", fieldLabel:"或输入地点名"}]
                }, {
                    columnWidth: .2,
                    layout: 'form',
                    items: [{ 
                         xtype:'button',width:60,text:'地点查询',
                         handler:function(){
							codeAddress();
						}
                    }]
                }]
            }]
		},{
			xtype:'panel',
			region:'center',
			layout:"fit",//此处的fit可以使得tab自适应
			items:[tabs]
			//html:"<iframe id='resultTable' src='table.html'frameborder='0' height='100%' width='100%' style='margin: 0px'></iframe>"
		}]
	})
	

}

function codeAddress() 
{     
    var address = document.getElementById("address").value;  
    geocoder.geocode( { 'address': address}, 
        function(results, status) {  
                if (status == google.maps.GeocoderStatus.OK) {    
                    map.setCenter(results[0].geometry.location);       
                    //var marker = new google.maps.Marker({map: map,position: results[0].geometry.location});  
                    retriveWeather(results[0].geometry.location);
                } else {     
                    alert("Geocode was not successful for the following reason: " + status);   
                }    
        });  
} 

function getWindDirection(_windDirNum)
{
	var directionStr;
	if(_windDirNum >=359 ){
		directionStr = "偏北"
	}
	else if(_windDirNum < 1){
		directionStr = "偏北"
	}
	else if(_windDirNum >1 && _windDirNum < 44){
		directionStr = "北北东"
	}
	else if(_windDirNum >=44 && _windDirNum <= 46){
		directionStr = "东北"
	}
	else if(_windDirNum >46 && _windDirNum < 89){
		directionStr = "东北东"
	}
	else if(_windDirNum >=89 && _windDirNum <= 91){
		directionStr = "偏东"
	}
	else if(_windDirNum >91 && _windDirNum < 134){
		directionStr = "东南东"
	}
	else if(_windDirNum >=134 && _windDirNum <= 136){
		directionStr = "东南"
	}
	else if(_windDirNum >136 && _windDirNum < 179){
		directionStr = "南南东"
	}
	else if(_windDirNum >=179 && _windDirNum <= 181){
		directionStr = "偏南"
	}
	else if(_windDirNum >181 && _windDirNum < 224){
		directionStr = "南南西"
	}
	else if(_windDirNum >=224 && _windDirNum <= 226){
		directionStr = "西南"
	}
	else if(_windDirNum >226 && _windDirNum < 269){
		directionStr = "西南西"
	}
	else if(_windDirNum >=269 && _windDirNum <= 271){
		directionStr = "偏西"
	}
	else if(_windDirNum >271 && _windDirNum < 314){
		directionStr = "西北西"
	}
	else if(_windDirNum >=314 && _windDirNum <= 316){
		directionStr = "西北"
	}
	else if(_windDirNum >316 && _windDirNum < 359){
		directionStr = "北北西"
	}
	
	return directionStr;
}


function getWeatherPic(_metroData, _hours)
{
	var weatherPicStr;
	var weatherNameStr;
	if(_metroData.cloudcover==1 && _metroData.prec_type=="none" && _metroData.prec_amount==0){
		if(_hours>=20 || _hours<=5){
			weatherPicStr = "img/0n.gif";			
		}else{
			weatherPicStr = "img/0.gif";			
		}
	}else if(_metroData.cloudcover>=2 && _metroData.cloudcover<=8 && _metroData.prec_type=="none" && _metroData.prec_amount==0){
		if(_hours>=20 || _hours<=5){
			weatherPicStr = "img/1n.gif";			
		}else{
			weatherPicStr = "img/1.gif";			
		}
	}else if(_metroData.cloudcover==9 && _metroData.prec_type=="none" && _metroData.prec_amount==0){
			weatherPicStr = "img/2.gif";			
	}else if(_metroData.cloudcover==9 && _metroData.prec_type=="rain" && _metroData.prec_amount>0){
			weatherPicStr = "img/43.gif"; 			
	}else if(_metroData.cloudcover==9 && _metroData.prec_type=="none" && _metroData.prec_amount>0){
			weatherPicStr = "img/19.gif";			

	}else if(_metroData.cloudcover==1 && _metroData.prec_type=="rain" && _metroData.prec_amount>0){
		if(_hours>=20 || _hours<=5){
			weatherPicStr = "img/44n.gif";			
		}else{
			weatherPicStr = "img/44.gif";			
		}
    }else if(_metroData.cloudcover==1 && _metroData.prec_type=="none" && _metroData.prec_amount>0){
		if(_hours>=20 || _hours<=5){
			weatherPicStr = "img/18n.gif";			
		}else{
			weatherPicStr = "img/18.gif";			
		}

	}else if(_metroData.cloudcover>=2 && _metroData.cloudcover<=8 && _metroData.prec_type=="rain" && _metroData.prec_amount>0){
		if(_hours>=20 || _hours<=5){
			weatherPicStr = "img/45n.gif";			
		}else{
			weatherPicStr = "img/45.gif";			
		}
    }else if(_metroData.cloudcover>=2 && _metroData.cloudcover<=8 && _metroData.prec_type=="none" && _metroData.prec_amount>0){
		if(_hours>=20 || _hours<=5){
			weatherPicStr = "img/31n.gif";			
		}else{
			weatherPicStr = "img/31.gif";			
		}

	}else if(_metroData.cloudcover>=2 && _metroData.cloudcover<=8 && _metroData.prec_type=="rain" && _metroData.prec_amount>0 && _metroData.lifted_index<(-5)){
		if(_hours>=20 || _hours<=5){
			weatherPicStr = "img/32n.gif";			
		}else{
			weatherPicStr = "img/32.gif";			
		}
	}else if(_metroData.cloudcover==1 && _metroData.prec_type=="rain" && _metroData.prec_amount>0 && _metroData.lifted_index<(-5)){
		if(_hours>=20 || _hours<=5){
			weatherPicStr = "img/37n.gif";			
		}else{
			weatherPicStr = "img/37.gif";			
		}
	}else if(_metroData.prec_type=="none" && _metroData.prec_amount==0 && _metroData.lifted_index<(-5)){
			weatherPicStr = "img/7.gif";			
	}else if(_metroData.prec_type=="rain" && _metroData.prec_amount>0 && _metroData.lifted_index<(-5)){
			weatherPicStr = "img/71.gif";			
	}else{
			weatherPicStr = null	//未描述天气
	}
	return weatherPicStr;
}

function getWeatherName(_weatherPicStr)
{
	var weatherNameStr;
	if(_weatherPicStr=="img/0n.gif" || _weatherPicStr=="img/0.gif"){
			weatherNameStr = "晴";	
	}else if(_weatherPicStr=="img/1n.gif" || _weatherPicStr=="img/1.gif"){
			weatherNameStr = "多云";		
	}else if(_weatherPicStr=="img/2.gif"){
			weatherNameStr = "阴";
	}else if(_weatherPicStr=="img/43.gif"){			
			weatherNameStr = "阴有雨";
	}else if(_weatherPicStr=="img/19.gif"){			
			weatherNameStr = "阴有时有雨";
	}else if(_weatherPicStr=="img/44n.gif" || _weatherPicStr=="img/44.gif"){		
			weatherNameStr = "晴有雨";		
    }else if(_weatherPicStr=="img/18n.gif" || _weatherPicStr=="img/18.gif"){		
			weatherNameStr = "晴有时有雨";		
	}else if(_weatherPicStr=="img/45n.gif" || _weatherPicStr=="img/45.gif"){		
			weatherNameStr = "多云有雨";		
    }else if(_weatherPicStr=="img/31n.gif" || _weatherPicStr=="img/31.gif"){		
			weatherNameStr = "多云有时有雨";		
	}else if(_weatherPicStr=="img/32n.gif" || _weatherPicStr=="img/32.gif"){		
			weatherNameStr = "多云有时有雷阵雨";		
	}else if(_weatherPicStr=="img/37n.gif" || _weatherPicStr=="img/37.gif"){		
			weatherNameStr = "晴有时有雷阵雨";		
	}else if(_weatherPicStr=="img/7.gif"){			
			weatherNameStr = "雷电";
	}else if(_weatherPicStr=="img/71.gif"){			
			weatherNameStr = "雷阵雨";
	}else{
			weatherNameStr = null	//未描述天气
	}
	return weatherNameStr;
}


function getResult(_jsonObj)
{
	var forecastDate = new Date();
	
	var currentDate;
	var formerDate=0;
	
	//设置初始时间（UTC时间）
	forecastDate.setUTCFullYear(_jsonObj.init.substring(0,4));
  	forecastDate.setUTCMonth(_jsonObj.init.substring(4,6));
  	forecastDate.setUTCDate(_jsonObj.init.substring(6,8));
  	forecastDate.setUTCHours(parseInt(_jsonObj.init.substring(8,10)));
  	
  	
  	for(var i=0;i<_jsonObj.dataseries.length;i++)
  	{
  		var metroData = _jsonObj.dataseries[i];
  		
  		
  		//获取预报数据，首先设置预报时间
  		forecastDate.setHours(forecastDate.getHours() + 3);
  		
  		currentDate = forecastDate.getDate();
//  		alert(forecastDate.toString())
//  		alert((formerDate != currentDate)+";;;"+formerDate+".."+currentDate)	
  		//如果日期发生变化，计数器加1
  		if((formerDate != currentDate) || (formerDate == 0))
  		{
  			dateNumId++;
  			window.frames["resultTable"].document.getElementById("d"+dateNumId).innerText = forecastDate.getMonth()+"月"+currentDate+"日";
  			window.frames["resultTable"].document.getElementById("fs_d"+dateNumId+"_"+forecastDate.getHours()).innerText = metroData.wind10m.speed;
  			window.frames["resultTable"].document.getElementById("wd_d"+dateNumId+"_"+forecastDate.getHours()).innerText = metroData.temp2m;
  			window.frames["resultTable"].document.getElementById("fx_d"+dateNumId+"_"+forecastDate.getHours()).innerText = getWindDirection(metroData.wind10m.direction);
  			
  			var imgFileName = getWeatherPic(metroData, forecastDate.getHours());
			var imgWeatherName = getWeatherName(imgFileName);
			
			if(imgFileName!=null)
			{
				//var dom_img = document.createElement('img');
				//dom_img.src = imgFileName;
				window.frames["resultTable"].document.getElementById("tq_d"+dateNumId+"_"+forecastDate.getHours()).src = imgFileName;
				//window.frames["resultTable"].document.getElementById("tq_d"+dateNumId+"_"+forecastDate.getHours()).appendChild(dom_img);
				window.frames["resultTable"].document.getElementById("tq_d"+dateNumId+"_"+forecastDate.getHours()).title = imgWeatherName;
			}
  		}
  		else
  		{
  			window.frames["resultTable"].document.getElementById("fs_d"+dateNumId+"_"+forecastDate.getHours()).innerText = metroData.wind10m.speed;
  			window.frames["resultTable"].document.getElementById("wd_d"+dateNumId+"_"+forecastDate.getHours()).innerText = metroData.temp2m;
  			window.frames["resultTable"].document.getElementById("fx_d"+dateNumId+"_"+forecastDate.getHours()).innerText = getWindDirection(metroData.wind10m.direction);
  			
  			var imgFileName = getWeatherPic(metroData, forecastDate.getHours());
			var imgWeatherName = getWeatherName(imgFileName);
			
			if(imgFileName!=null)
			{
				//var dom_img = document.createElement('img');
				//dom_img.src = imgFileName;
				window.frames["resultTable"].document.getElementById("tq_d"+dateNumId+"_"+forecastDate.getHours()).src = imgFileName;
				//window.frames["resultTable"].document.getElementById("tq_d"+dateNumId+"_"+forecastDate.getHours()).appendChild(dom_img);
				window.frames["resultTable"].document.getElementById("tq_d"+dateNumId+"_"+forecastDate.getHours()).title = imgWeatherName;
			}
  		}
  		
  		formerDate = currentDate;
  	}
  	
//  	alert(initDate.getDate()+""+initDate.getHours());
  	
//  	initDate.setUTCHours(parseInt(_jsonObj.init.substring(8,10))+10);
  	
//  	alert(initDate.getDate()+""+initDate.getHours());
}

//根据经纬度获取天气预报
function retriveWeather(latLng){
    dateNumId=0;
	if(marker!=null)
	{
		marker.setMap(null);
	}
	marker = new google.maps.Marker({
        position: latLng, 
        map: map
    });
    //Ext.getCmp("xsfxUrl").html ="";
	showLoadingMask();
	refreshResultTable();
	showLabelInfo("经度："+latLng.lng()+";"+"纬度"+latLng.lat())
	Ext.Ajax.request({            url:getUrl('MMShareBLL.DAL.Forecast','HelloMetro'),            params: { lon: latLng.lng(),lat: latLng.lat()},            success:function(response){                if(response.responseText !=""){                    var result = Ext.util.JSON.decode(response.responseText);                     getResult(result);                }else {					Ext.MessageBox.alert(" 提示","读取数据失败，请稍候再试！");				}				//myMask.hide();				hideLoadingMask();            },			//提交失败时执行的方法			failure : function() {			    //myMask.hide();			    hideLoadingMask();				Ext.MessageBox.alert("提示","读取数据失败，请稍候再试！");			},            scope: this        })
        var htmlUrl = "http://www.7timer.com/v4/bin/meteo.php?lon="+latLng.lng()+"&lat="+latLng.lat()+"&lang=zh-CN&ac=0&unit=metric&output=internal&tzshift=0";
        //Ext.get('forecastImage').dom.src = htmlUrl;
        //Ext.getCmp("xsfxUrl").getUpdater().refresh();
        imageViewer.setImageSrc(htmlUrl);
 	    
		//document.getElementById('xsfxUrl').src="http://www.7timer.com/v4/bin/meteo.php?"+latLng.lng()+"&"+latLng.lat()+"&lang=zh-CN&ac=0&unit=metric&output=internal&tzshift=0";
		//window.open("http://www.7timer.com/v4/bin/meteo.php?"+latLng.lng()+"&"+latLng.lat()+"&lang=zh-CN&ac=0&unit=metric&output=internal&tzshift=0","xsfxUrl"); 

}


Ext.onReady(function(){
	
	initMapPanel();
	initForecastPanel();
	
	var viewPort = new Ext.Viewport
	({
		layout : 'border',
		items : [mapPanel,forecastPanel]
	});
			
	Ext.get(document).on("contextmenu",function(e){
		e.preventDefault();
	})
	
	geocoder = new google.maps.Geocoder();   
	var mapLatlng = new google.maps.LatLng(31, 121);
    var mapOptions = {
      zoom: 5,
      center: mapLatlng,
      mapTypeId: google.maps.MapTypeId.SATELLITE
    }
    map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
	
	
//	var map = new GMap2(document.getElementById("map_canvas"));
//    map.setCenter(new GLatLng(31, 121), 5);
//    map.setUIToDefault();
    
    
    google.maps.event.addListener(map, 'click', function(event) {
		if(isQuery)
	 	{
	 	    retriveWeather(event.latLng);
//	 		alert(latlng.x+";"+latlng.y)
	 		
	 		
		//openNewWindow('http://www.163.com/');
		//var o=window.open("http://www.shmmc.cn/shiprouteing/index.html");   
            //o.frames["iframe1"].location="2.html";
			//addHtmlPanel("http://www.shmmc.cn/shiprouteing/index.html");
		}
    });
    
    
//    GEvent.addListener(map, "click", function(overlay,latlng,overlaylatlng) {
//	 	if(isQuery)
//	 	{
////	 		alert(latlng.x+";"+latlng.y)
//	 		
//	 		showLoadingMask();
//	 		refreshResultTable();
//	 		showLabelInfo("经度："+latlng.x+";"+"纬度"+latlng.y)
//	 		var conn = new Ext.data.Connection();
//          	conn.request({
//	            url:"http://localhost/MeteoServices/Service.asmx/HelloMetro", //注意引用的路径
//	            params:{lon:latlng.x,lat:latlng.y},
//	            method: 'post',
//	            scope: this,
//	            callback:function(options,success, response){ 
//	            	hideLoadingMask();
//		            if(success){   
//		              isQuery = false;
//		              var dom = new ActiveXObject('Microsoft.XMLDOM'); // 得到XML操作对象
//		              dom.loadXML(response.responseText); // 此处就是XML的内容
//		              	
//		              var jsonObj=Ext.util.JSON.decode(dom.text);
//		              getResult(jsonObj);
//		            }    
//		            else{
//		               Ext.MessageBox.alert("提示","读取数据失败，请稍候再试！");
//		            }     
//		         }  
//            })
//	 	}
//	});
})