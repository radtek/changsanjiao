var flag1,flag2;

function CheckDiv(){
	var divs = document.getElementsByTagName("DIV");

	for(var i=0;i<divs.length;i++){
		if(divs[i].src!=null){
			if(divs[i].src.indexOf("poweredby")>-1){				
				divs[i].style.display="none";
				flag1 = true;
			}
		}
		if(divs[i].innerHTML.indexOf("Mapabc") > -1 && divs[i].innerHTML.indexOf("DIV") == -1){
			divs[i].style.display="none";
			flag2 = true;
		}
	}

	var imgs = document.images;

	for(var i=0;i<imgs.length;i++){
		if(imgs[i].src!=null){
			if(imgs[i].src.indexOf("poweredby")>-1){				
				imgs[i].style.display="none";
				flag1 = true;
				break;
			}
		}
	}

	if(flag1 && flag2){
		return;
	}
	window.setTimeout("CheckDiv()",100);
}