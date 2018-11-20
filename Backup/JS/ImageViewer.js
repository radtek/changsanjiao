// 实现图片浏览，,可以上下左右移动,也可以用鼠标随意拖动,放大(双击或点加号图片),缩小,复原等
    var count;
    var time;
    var posX;
    var posY
    var startDate;
    var endDate;
    var arrayStation=new Array();
    var entityNameLink;
    var stationCheckStr;
    var imageIndex;
    var PxFunction="";
    var date;
    var filePath;
    var aliasName;
    ImageViewer = function(imageSrc,entityName,name){
    var parent = this;
    /**  
     * 初始化  
     */  
    function pageInit(imageViewer){  
        var image = Ext.get('view-image');
        var table= document.getElementById('tableDiv')  
        table.onmousedown =mouseDown;
        var week= document.getElementById('CommentDiv') 
        week.onmousedown =mouseDownComment; 
        var comment= document.getElementById('WeekDiv') 
        comment.onmousedown =weekmouseDown; 
        aliasName=name;  
        if(entityName=="PM2_5"||entityName=="pm2.5_class"||entityName=="PM10") 
        {
            entityNameLink=entityName;
            Ext.getDom("table").style.display = "block";
            Ext.getDom("quxian").style.display = "block";
        }
        else
        {
            Ext.getDom("table").style.display = "none";
            Ext.getDom("quxian").style.display = "none";
            entityNameLink=entityName;
        }
        
        if(image!=null){   
            image.dom.onmousewheel = bbimg;
            
            image.dom.onload = function(){imageFull(image);}
            listener_onmousewheel(image);
            image.on({   
                'mousedown':{fn:function(){this.setStyle('cursor','url(css/images/imageViewer/closedhand_8_8.cur),default;');},scope:image},   
                'mouseup':{fn:function(){Ext.getCmp(entityName).focus();this.setStyle('cursor','url(css/images/imageViewer/openhand_8_8.cur),move;');},scope:image},   
                'dblclick':{fn:function(){   
                    zoom(image,true,1.2);   
                },
		'onload':{fn:function(){imageFull(image);}}
                }
            });   
            new Ext.dd.DD(image, 'pic');    
            
            Ext.get('up').on('click',function(){imageMove('up',image);});       //向上移动   
            Ext.get('down').on('click',function(){imageMove('down',image);});   //向下移动   
            Ext.get('left').on('click',function(){imageMove('left',image);});   //左移   
            Ext.get('right').on('click',function(){imageMove('right',image);}); //右移动   
            Ext.get('in').on('click',function(){zoom(image,true,1.5);});        //放大   
            Ext.get('out').on('click',function(){zoom(image,false,1.5);});      //缩小   
            Ext.get('zoom').on('click',function(){restore(image);});            //还原   
            Ext.get('zoomf').on('click',function(){imageFull(image);});            //全图显示  
            Ext.get('print').on('click',function(){printImage(image);});            //打印    
            Ext.get('table').on('click',function(){tableImage(image,entityName);});            //表格   
            Ext.get('comment').on('click',function(){commentContent();});            //评论
            Ext.get('perRevise').on('click',function(){WeekContent();}); //每周展望  
            Ext.get('quxian').on('click',function(){quxianImage(entityName);});            //表格    
            Ext.get('stationName').on('click',function(){stationName(entityName);});            //表格      
            Ext.get('tableDiv').on('mousedown',function(){mouseDown(event);});
            Ext.get('CommentDiv').on('mousedown',function(){mouseDownComment(event);})
            Ext.get('WeekDiv').on('mousedown',function(){weekmouseDown(event);})
        
            


        }   
  
    };   
    function printImage(el){
        var imgWidth = images2.width;
        var imgHeight = images2.height;
        var parentWidth = 800;
        var parentHeight = 620;
        var wK = imgWidth/parentWidth;
        var hK = imgHeight/parentHeight;
        
        
        if(imgWidth >1){
            if(wK>hK && wK>1){
                imgWidth = parentWidth;
                imgHeight = imgHeight/wK;
            }
            else if (hK>wK && hK>1){
                imgWidth = imgWidth/hK;
                imgHeight = parentHeight;
            }
        }
        var oWin   =   window.open('','打印','menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
			if(oWin)   { 
				oWin.document.open(); 
				oWin.document.write("<html>"); 
				oWin.document.write("<body>");
				oWin.document.write("<center>"); 
				oWin.document.write("<img id='img' width='" + imgWidth + "' height = '" + imgHeight + "'  src=" + el.dom.src + " border='0'>"); 
				oWin.document.write("</center>"); 
				oWin.document.write("</body>");
				oWin.document.write("</html>"); 
				oWin.document.close(); 
				oWin.print(); 
				oWin.close(); 
			} 
			Ext.getCmp(entityName).focus();
        
    }
     /**  
     * 按比例全图显示 
     */ 
    function imageFull(el)
    {         
        var imgWidth = images2.width;
        var imgHeight = images2.height;
        var parentWidth = parent.getWidth();
        var parentHeight = parent.getHeight();
        var wK = imgWidth/parentWidth;
        var hK = imgHeight/parentHeight;

        if(imgWidth >1){
            if(wK>hK && wK>1)
                el.setSize(parentWidth, imgHeight/wK);
            else if (hK>wK && hK>1)
                el.setSize(imgWidth/hK, parentHeight);
                
            el.center(parent.el);//图片居中   
            el.dom.onload = "";
        }
        Ext.getCmp(entityName).focus();
    }
      
    /**  
     * 图片移动  
     */  
    function imageMove(direction, el) {   
        el.move(direction, 50, true);   
        Ext.getCmp(entityName).focus();
    }   
       
      
      
    /**  
     * 图片还原  
     */  
    function restore(el) {   
        var size = {   
                width:images2.width,   
                height:images2.height   
            }; 
         //el.fadeOut();     
         el.setSize(size.width, size.height);
         el.center(parent.el);  
         //el.fadeIn();
//        //自定义回调函数   
//        function center(el,callback){   
//            callback(el);   
//            el.center(parent.el);   

//        }   
//        el.fadeOut({callback:function(){   
//            el.setSize(size.width, size.height, {callback:function(){   
//                center(el,function(ee){//调用回调   
//                    ee.fadeIn();   
//                });   
//            }});   
//        }   
//        }); 
        Ext.getCmp(entityName).focus();  
    }  

    
    ImageViewer.superclass.constructor.call(this,{   
        border:false,   
        layout : "fit",
        region : "center",
        html :
            '<div id="mapPic"><div class="nav">'  
            +'<div class="up" id="up"></div><div class="right" id="right"></div>'  
            +'<div class="down" id="down"></div><div class="left" id="left"></div>'  
            +'<div class="zoom" id="zoom"></div><div class="in" id="in"></div>'  
            +'<div class="zoomf" id="zoomf"></div><div class="out" id="out"></div>'  
            +'<div class="print" id="print" title = "打印"></div>'
            +'<div class="quxian" id="quxian" title = "小时报曲线" style="display: none;"></div>'
            +'<div class="table" id="table" title = "小时日报" style="display: none;"></div>'
            +'<div class="Datequxian" id="Datequxian" title = "日报曲线" style="display: none;"></div>'
            +'<div class="Datetable" id="Datetable" title = "日报" style="display: none;"></div></div>'
            +'<label id="comment" class="comment" style="cursor: pointer;" >关注</label>'
            +'<label id="perRevise" class="perRevise" style="cursor: pointer;" >会商</label>'
            +'<div id="hiddenPic" style="position:absolute; left:0px; top:0px; width:0px; height:0px; z-index:1; visibility: hidden;">'
            +'<img name="images2" src="'+ imageSrc +'" border="0"></div>'
            +'<div id="CommentDiv" class="hiddenComment" style="display: none;"></div>'
            +'<div id="WeekDiv" class="WeekDiv" style="display: none;"></div>'
            +'<div id="tableDiv" class="tableDiv" style="display: none;"></div>'
            +'<div id="quxianWai" class="quxianDiv" style="display: none;"><div id="quxianDiv"></div><div><div class="lableCls" id="stationName"  title = "城市"></div></div><input type="button"  onclick="closeQuxian()" class="quXClose" onmouseover="this.className = \'quXcloseHover\';" onmouseout ="this.className =\'quXClose\';"   id="quxianClose"></div>'
            +'<div  id="panGroupContainer" style="position: absolute; display: none;width: 400px; height: 250px; background-color: white; border: 2px solid rgb(193, 218, 215);"><div id="chkSiteContainer" style="width: 100%; height: 225px; overflow-y: scroll;border-bottom: solid 1px #C1DAD7"></div>'
            +'<div style="padding-top: 2px;height:20px;"><div style="float: left;">&nbsp;&nbsp;<input type="button" class="button_defaultSelect" value="全选" style="padding-left: 2px;" onclick="allSelecet()"/>'
            +'<input type="button" class="button_defaultSelect" value="反选" onclick="fanSelecet()"/><label class="stationPer">输入通道：</label><input type="checkbox" name="bpx"  id="bpx" value ="北偏西" class="stationPer" onclick="bpxClick()"/><label for="bpx" style="font-size: 13px;">北偏西</label><input type="checkbox" name="xn"  id="xn" value ="西南" class="stationPer" onclick="xnClick()"  /><label for="xn" style="font-size: 13px;">西南</label><input type="checkbox" name="xb"   id="xb" value ="西北" class="stationPer"  onclick="xbClick()" /><label for="xb" style="font-size: 13px;">西北</label></div><div style="float: right"><input type="submit" name="btnSettingSites" value="确定" onclick="OKSelecet()" id="btnSettingSites" class="button_defaultSelect"/>'
            +'<input type="button" value="关闭"  style="padding-left: 2px;"  onclick="closeCheck()" class="button_defaultSelect"/></div></div></div>'
            +'<img id="view-image"  src="'+ imageSrc +'" border="0" style="cursor: url(css/images/imageViewer/openhand_8_8.cur), default;"  > </div>'  
    });

    this.on('afterrender', pageInit);


}

//继承，添加函数，或者重写函数
Ext.extend(ImageViewer, Ext.Panel, {
 setImageSrc: function(imgSrc,star,end){
        var image = Ext.get('view-image'); 
        images2.src = imgSrc;
        image.dom.src = imgSrc;
        var mainIndex=imgSrc;
        if(mainIndex!=imageIndex&&mainIndex.indexOf("Ext/resources")<0)
        {
          if(Ext.getDom("tableDiv").style.display == "block")
              tableImageLianD(mainIndex,entityNameLink);
          
        }
        if(startDate!=star||endDate!=end)
        {
         startDate=star;
         endDate=end;
         if(Ext.getDom("quxianWai").style.display == "block")
              quxianImage(entityNameLink);
        }
        imageIndex=mainIndex;
        if(mainIndex.indexOf("\\"))
           mainIndex =mainIndex.replace("\\",'/');
        if(mainIndex.indexOf("\\"))
           mainIndex =mainIndex.replace("\\",'/');
        if(mainIndex.indexOf("\\"))
           mainIndex =mainIndex.replace("\\",'/');
        filePath=mainIndex;
        //labelValue();        
    }
    
});
//注册，可以通过xtype:imageViewer来创建面板
Ext.reg('imageViewer', ImageViewer); 
 
function WeekContent()
{
var entityName=entityNameLink;
Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog','WeekcommentCotent'),
            params: { entityName:entityName},
            success: function(response){
                if(response.responseText!=""){
                    var obj= document.getElementById('WeekDiv'); 
                    obj.innerHTML=response.responseText;
                    obj.style.display = "block";
                    var el= document.getElementById('CommentDiv');
                    if(el.style.display=="block") 
                        el.style.display="none"; 
                }
                else 
                  Ext.getDom("WeekDiv").innerHTML="";
                
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
}
function PublicComment()
{
      var entityName=entityNameLink;//实体名
      var folder=filePath;//路径
      var fieldControl=Ext.getCmp(entityNameLink); 
      var selectionsArray = fieldControl.getSelectedIndexes();
       var store = fieldControl.getNodes();
       var Imgtime=store[selectionsArray[0]].innerText;
      var textValue = Ext.getDom("textContent").innerHTML;//评论内容
      var commentPeople=Ext.getDom("loginResult").value;//评论人
      var imgName=aliasName;
      var myMask = new Ext.LoadMask(Ext.getBody(), {msg:"数据正在发表中..."});
      myMask.show();
         Ext.Ajax.request({ 
                url: getUrl('MMShareBLL.DAL.PublicLog','SaveCommentCotent'),
                 params: { textValue: textValue,commentPeople:commentPeople,entityName:entityName,folder:folder,Imgtime:Imgtime,imgName:imgName}, 
                success: function(response){
                    if(response.responseText!=""){
                      commentContent();
                       myMask.hide();
                     
                    }
                }, 
                failure: function(response) { 
                    Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
                }
             });
    
} 
function SaveRevise(flag)
{
    var content=getContent();
    var name=Ext.getDom("loginResult").value;//评论人;
    Ext.Ajax.request({ 
            url: getUrl('MMShareBLL.DAL.PublicLog','SaveRevise'),
            params: {content:content,flag:flag,name:name},
            success: function(response){
                if(response.responseText!=""){
                if(response.responseText=="成功")
                    Ext.Msg.alert("信息", "保存成功！");
                else 
                    Ext.Msg.alert("信息", response.responseText); 
                }
                else  
                    Ext.Msg.alert("信息", "保存成功！");               
            }, 
            failure: function(response) { 
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         });
}
function bbimg(ev)
{
    var up;//标识滚轮向上

    var evn = ev || event;//IE11 既包括wheelDelta又包括detail属性，而一般的IE只包括wheelDelta，火狐只包括detail属性
    if(evn.wheelDelta)
    {
        if(evn.wheelDelta >= 120)
          up = true;
       else if(evn.wheelDelta <= -120)
          up = false;
    }
    else if(evn.detail)
    {
      if(evn.detail <= -3)
            up = true;
        else if(evn.detail >= 3)
            up = false;
    }  
    
    zoom(Ext.get('view-image'),up,1.2); 

}
function zoom(el,type,offset){   
    var width = el.getWidth();   
    var height = el.getHeight();   
    var nwidth = type ? (width * offset) : (width / offset);   
    var nheight = type ? (height * offset) : (height / offset);   
    var left = type ? -((nwidth - width) / 2):((width - nwidth) / 2);   
    var top =  type ? -((nheight - height) / 2):((height - nheight) / 2);    
    el.animate(   
        {   
            height: {to: nheight, from: height},   
            width: {to: nwidth, from: width},   
            left: {by:left},   
            top: {by:top}   
        },   
        null,         
        null,        
        'easeOut',   
        'motion'  
    );   
}  
