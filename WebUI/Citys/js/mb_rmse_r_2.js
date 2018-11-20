function DrawTable(){
   var fromDate = Ext.getDom("H00").value;
   var toDate = Ext.getDom("H01").value;
   var cityName= Ext.getDom("city").value;
   var duration="10";
   Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.CitysForecast', 'GetAQIChartDB'),
        params: { fromDate: fromDate, toDate: toDate, city: cityName,duration:duration },
        success:function(response){
        	  if (response.responseText != "") {
        		var result;
        		var sps=response.responseText.toString().split("&");
        		for(var i=0;i<4;i++){
        			if(sps[i]!=""){
        				var spsII=sps[i].split(",");
        				for(var j=1;j<6;j++){
        					if(spsII[0]!=""){
        						var sk=spsII[0].split("*");
        						var shikuang=sk[1].split("|");
        						var spsIII=spsII[j].split("*");
        						var spsIV=spsIII[1].split("|");
        						result+=(GetItem(i)+","+GetModel(j)+","+MeanBias(shikuang,spsIV)+","+RootMeanSqrError(shikuang,spsIV)+","+CorrelationCoe(shikuang,spsIV)+"#");
        					}
        					else{
        						for(var z=1;z<6;z++){
        							result+=(GetItem(i)+","+GetModel(z)+","+","+","+"#");
        						}
        						break;
        					}
        				}
        			}
        			else{
        				for(var z=1;z<6;z++){
        					result+=(GetItem(i)+","+GetModel(z)+","+","+","+"#");
        				}
        			}
                }
                // document.getElementById("test").innerHTML = result;
        	    //return result;
        	}
        	else{
        		Ext.Msg.alert("提示", "没有满足条件的信息。");
        	}
        }


function GetItem(itemNumber){
	var item;
	switch (itemNumber){
		case 0:
			item="pm25";
			break;
		case 1:
			item="pm10";
			break;
		case 2:
			item="o3";
			break;
		case 3:
			item="no2";
			break;
	}
	return item;
}

function GetModel(modelNumber){
	var model;
	switch (modelNumber) {
		case 0:
			item="shikuang";
			break;
		case 1:
			item="cuace";
			break;
		case 2:
			item="wrf_chem";
			break;
		case 3:
			item="cmaq";
			break;
		case 4:
			item="cmaq10";
			break;
		case 5:
			item="cuace9km";
			break;
	}
	return model;
}

//字符串转float
function ConvertToFloat(x) {
  if (x != "NULL") {
        //var floatTemp = parseFloat(x);
        var floatValue = parseFloat(x);
        return floatValue;
    }
    else{
        return null;
    }
}

//Mean bias
function MeanBias(measured,predicted){
	var mea=measured;//实况数据，数组类型
	var pre = predicted; //预测数据，数组类型
	var sum;
	var n=1;
	for(var i=0;i<mea.length;i++){
		if(ConvertToFloat(pre[i])!=null&&ConvertToFloat(mea[i])!=null){
			sum+=(ConvertToFloat(pre[i])-ConvertToFloat(mea[i]));
			n+=1;//计算求差次数
		}
	}
	var mb=sum/n;
	return mb;
}

//Root Mean Square Error
function RootMeanSqrError(measured,predicted){
	var mea=measured;//实况数据，数组类型
	var pre = predicted; //预测数据，数组类型
	var bias;
	var sum;
	var n=1;
	var rmse;
	for(var i=0;i<mea.length;i++){
		if(ConvertToFloat(pre[i])!=null&&ConvertToFloat(mea[i])!=null){
			bias=ConvertToFloat(pre[i])-ConvertToFloat(mea[i]);
			sum+=Math.pow(bias,2);
			n+=1//计算求差次数
		}
	}
	rmse=Math.pow(sum/n,0.5);
	return rmse;
}

//Correlation coefficient
function CorrelationCoe(measured,predicted){
	var mea=measured;//实况数据，数组类型
	var pre = predicted; //预测数据，数组类型
	var sumPre;
	var sumMea;
	var n=1;
	var avgPre;
	var avgMea;
	var dvaluePre;
	var dvalueMea;
	var sumPreMea;
	var sumSqrMea;
	var sumSqrPre;
	var sqrt;
	//实况和模式数据都存在，求平均
	for(var i=0;i<mea.length;i++){
		if(ConvertToFloat(pre[i])!=null&&ConvertToFloat(mea[i])!=null){
			sumPre+=ConvertToFloat(pre[i]);
			sumMea+=ConvertToFloat(mea[i]);
			n+=1;
		}
	}
	avgPre=sumPre/n;
	avgMea=sumMea/n;
	//求相关系数
	for(var i=0;i<mea.length;i++){
		if(pre[i]!=null&&mea[i]!=null){
			dvaluePre=(ConvertToFloat(pre[i])-avgPre);
			dvalueMea=(ConvertToFloat(mea[i])-avgMea);
			sumPreMea+=(dvaluePre*dvalueMea);
			sumSqrPre+=Math.pow(dvaluePre,2);
			sumSqrMea+=Math.pow(dvalueMea,2);
		}
	}
	sqrt=Math.pow(sumSqrPre*sumSqrMea,0.5);
	r=sumSqrMea/sqrt;
	return r;
}