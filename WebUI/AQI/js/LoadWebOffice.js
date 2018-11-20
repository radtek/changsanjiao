var s = "";
if(navigator.userAgent.indexOf("MSIE")>0){
	s = "<OBJECT id='WebOffice1' align='middle' style='LEFT: 0px; WIDTH: 100%; TOP: 0px; HEIGHT:100%'"
		+ "classid=clsid:E77E049B-23FC-4DB8-B756-60529A35FAD5 codebase='../AQI/WebOffice.ocx'>"
		+ "</OBJECT>";
}

if(navigator.userAgent.indexOf("Chrome")>0){
//	s = "<object id='WebOffice1' type='application/x-itst-activex' align='baseline' border='0'"
//		+ "style='LEFT: 0px; WIDTH: 100%; TOP: 0px; HEIGHT: 100%'"
//		+ "clsid='{E77E049B-23FC-4DB8-B756-60529A35FAD5}'"
    //		+ "event_NotifyCtrlReady='WebOffice1_NotifyWordEvent' codebase='../AQI/WebOffice.ocx'>"
    //		+ "</object>";	
    s = "<object id='WebOffice1' align='baseline' border='0'"
		+ "style='LEFT: 0px; WIDTH: 100%; TOP: 0px; HEIGHT: 100%'"
		+ "clsid='{E77E049B-23FC-4DB8-B756-60529A35FAD5}'"
		+ "event_NotifyCtrlReady='WebOffice1_NotifyWordEvent' codebase='..AQI/WebOffice.ocx'>"
		+ "</object>";	
}

if(navigator.userAgent.indexOf("Firefox")>0){
	s = "<object id='WebOffice1' type='application/x-itst-activex' align='baseline' border='0'"
		+ "style='LEFT: 0px; WIDTH: 100%; TOP: 0px; HEIGHT: 100%'" 
		+ "clsid='{E77E049B-23FC-4DB8-B756-60529A35FAD5}'"
		+ "event_NotifyCtrlReady='WebOffice1_NotifyCtrlReady' codebase='../AQI/WebOffice.ocx'>"
		+ "</object>";
	s += "<object id='ffPlugin' type='application/x-itst-activex' align='baseline' border='0'"
		+ "style='LEFT: 0px; WIDTH: 100%; TOP: 0px; HEIGHT: 100%'" 
		+ "clsid='{E77E049B-23FC-4DB8-B756-60529A35FAD5}'"
		+ "event_NotifyCtrlReady='WebOffice1_NotifyCtrlReady' codebase='../AQI/ffactivex-setup-r39.exe'>"
		+ "</object>";	
}
document.write(s)

//var s = ""
//s += "<object id=WebOffice1 height=768 width='100%' style='LEFT: 0px; TOP: 0px'  classid='clsid:E77E049B-23FC-4DB8-B756-60529A35FAD5'>"
//s += "<param name='_ExtentX' value='6350'><param name='_ExtentY' value='6350'>"
//s += "</OBJECT>"
//document.write(s)