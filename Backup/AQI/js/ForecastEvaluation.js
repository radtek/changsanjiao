

Ext.onReady(function() {
    supportInnerText(); //使得火狐支持innerText
    initInputHighlightScript();
    queryData();
    //FixTable("table1", 2, 600, 400);
    //userFunction();
      
    }
)
function mouseOver(obj)
{
if(obj!=null)
    {
       obj.style.backgroundColor = "#badbff";
    }

}
function mouseOut(obj)
{
 if(obj!=null)
    {
       obj.style.backgroundColor = "#fff";
    }
}
function getRadioValue()
{
 var obj=document.getElementsByName("period");
   if(obj!=null)
    {
      for(var i=0;i<obj.length;i++)
      {
       if(obj[i].checked)
       { 
        return obj[i].value;
       }
      }
    }
}
function getCheckBValue(objName)
{
var postJson = "";
 var forecasArray=new Array();
 var obj=document.getElementsByName(objName);
   if(obj!=null)
    {
      for(var i=0;i<obj.length;i++)
      {
       if(obj[i].checked)
       {
       postJson=postJson+obj[i].value+",";
       }
      }
    }
    if(postJson.length>0)
    {
     postJson=postJson.substring(0,postJson.length-1);
    }
   return postJson;
}


function getGroupEl(id){
    var groupEl = "";
    switch(id){
    case "rd1":
        groupEl = "rd2";
        break;
    case "rd2":
        groupEl = "rd1";
        break;
    }   
    return Ext.getDom(groupEl);
}

function CreateEvaluatioinData() {

    var fromDate = Ext.getDom("H00").value;
    var toDate = Ext.getDom("H01").value;
    var period;
    if (rd1.className == "radioUnChecked")
        period = "48";
    else period = "24";
    var forecasPeriod = getCheckBValue("forecasPeriod");
    var dataType = getCheckBValue("dataType");
    var dataModule = getCheckBValue("dataModule");
    var myMask = new Ext.LoadMask(document.body, { msg: "数据正在处理中...请稍候!" });
    myMask.show();
    Ext.Ajax.request({
        async: false,
        url: getUrl('MMShareBLL.DAL.ForecastEvaluation', 'GetEvaluationDataTables'),
        params: { fromDate: fromDate, toDate: toDate, period: period, forecasPeriod: forecasPeriod, dataType: dataType, dataModule: dataModule },
        success: function() {
            Ext.Msg.alert("提示", "数据加工成功！");
            myMask.hide();
        },
        failure: function(response) {

            Ext.Msg.alert("错误", "数据加工失败，错误代码为：" + response.status); 
            myMask.hide();
        }
    });
}

function  queryData()
{
    var fromDate=Ext.getDom("H00").value;
    var toDate=Ext.getDom("H01").value;
    var period;
    if (rd1.className == "radioUnChecked")
       period = "48";
    else period="24";
    var forecasPeriod=getCheckBValue("forecasPeriod");
    var dataType=getCheckBValue("dataType");
    var dataModule=getCheckBValue("dataModule");
    var myMask = new Ext.LoadMask(document.body, {msg:"正在查询..."});
    myMask.show();
    Ext.Ajax.timeout = 900000; 
    Ext.Ajax.request({
            async: false,
            url: getUrl('MMShareBLL.DAL.ForecastEvaluation', 'GetEvaluationString'),
       params: { fromDate: fromDate,toDate:toDate,period:period,forecasPeriod:forecasPeriod,dataType:dataType,dataModule:dataModule},
            success: function(response){
                        myMask.hide();
                        if(response.responseText!=""){
                            var tableHtml=new Array();
                            Ext.getDom("Tb1").innerHTML = response.responseText;
                            }
            }, 
            failure: function(response) { 
                myMask.hide();
                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
            }
         }); 
 
}
function queryExportData()
{
    document.getElementById("SearchBut").click();
}


function FixTable(TableID, FixColumnNumber, width, height) {
    /// <summary>
    ///     锁定表头和列
    /// </summary>
    /// <param name="TableID" type="String">
    ///     要锁定的Table的ID
    /// </param>
    /// <param name="FixColumnNumber" type="Number">
    ///     要锁定列的个数
    /// </param>
    /// <param name="width" type="Number">
    ///     显示的宽度
    /// </param>
    /// <param name="height" type="Number">
    ///     显示的高度
    /// </param>
    //alert("xxx");

    //alert($("#" + TableID).ID.toString());
    if ($("#" + TableID + "_tableLayout").length != 0) {
        $("#" + TableID + "_tableLayout").before($("#" + TableID));
        $("#" + TableID + "_tableLayout").empty();
    }
    else {
        $("#" + TableID).after("<div id='" + TableID + "_tableLayout' style='overflow:hidden;height:" + height + "px; width:" + width + "px;'></div>");
    }
    $('<div id="' + TableID + '_tableFix"></div>'
    + '<div id="' + TableID + '_tableHead"></div>'
    + '<div id="' + TableID + '_tableColumn"></div>'
    + '<div id="' + TableID + '_tableData"></div>').appendTo("#" + TableID + "_tableLayout");
    var oldtable = $("#" + TableID);
    var tableFixClone = oldtable.clone(true);
    tableFixClone.attr("id", TableID + "_tableFixClone");
    $("#" + TableID + "_tableFix").append(tableFixClone);
    var tableHeadClone = oldtable.clone(true);
    tableHeadClone.attr("id", TableID + "_tableHeadClone");
    $("#" + TableID + "_tableHead").append(tableHeadClone);
    var tableColumnClone = oldtable.clone(true);
    tableColumnClone.attr("id", TableID + "_tableColumnClone");
    $("#" + TableID + "_tableColumn").append(tableColumnClone);
    $("#" + TableID + "_tableData").append(oldtable);
    $("#" + TableID + "_tableLayout table").each(function() {
        $(this).css("margin", "0");
    });
    var HeadHeight = $("#" + TableID + "_tableHead thead").height();
    HeadHeight += 2;
    $("#" + TableID + "_tableHead").css("height", HeadHeight);
    $("#" + TableID + "_tableFix").css("height", HeadHeight);
    var ColumnsWidth = 0;
    var ColumnsNumber = 0;
    $("#" + TableID + "_tableColumn tr:last td:lt(" + FixColumnNumber + ")").each(function() {
        ColumnsWidth += $(this).outerWidth(true);
        ColumnsNumber++;
    });
    ColumnsWidth += 2;
    if ($.browser.msie) {
        switch ($.browser.version) {
            case "7.0":
                if (ColumnsNumber >= 3) ColumnsWidth--;
                break;
            case "8.0":
                if (ColumnsNumber >= 2) ColumnsWidth--;
                break;
        }
    }
    $("#" + TableID + "_tableColumn").css("width", ColumnsWidth);
    $("#" + TableID + "_tableFix").css("width", ColumnsWidth);
    $("#" + TableID + "_tableData").scroll(function() {
        $("#" + TableID + "_tableHead").scrollLeft($("#" + TableID + "_tableData").scrollLeft());
        $("#" + TableID + "_tableColumn").scrollTop($("#" + TableID + "_tableData").scrollTop());
    });
    $("#" + TableID + "_tableFix").css({ "overflow": "hidden", "position": "relative", "z-index": "50", "background-color": "Silver" });
    $("#" + TableID + "_tableHead").css({ "overflow": "hidden", "width": width - 17, "position": "relative", "z-index": "45", "background-color": "Silver" });
    $("#" + TableID + "_tableColumn").css({ "overflow": "hidden", "height": height - 17, "position": "relative", "z-index": "40", "background-color": "Silver" });
    $("#" + TableID + "_tableData").css({ "overflow": "scroll", "width": width, "height": height, "position": "relative", "z-index": "35" });
    if ($("#" + TableID + "_tableHead").width() > $("#" + TableID + "_tableFix table").width()) {
        $("#" + TableID + "_tableHead").css("width", $("#" + TableID + "_tableFix table").width());
        $("#" + TableID + "_tableData").css("width", $("#" + TableID + "_tableFix table").width() + 17);
    }
    if ($("#" + TableID + "_tableColumn").height() > $("#" + TableID + "_tableColumn table").height()) {
        $("#" + TableID + "_tableColumn").css("height", $("#" + TableID + "_tableColumn table").height());
        $("#" + TableID + "_tableData").css("height", $("#" + TableID + "_tableColumn table").height() + 17);
    }
    $("#" + TableID + "_tableFix").offset($("#" + TableID + "_tableLayout").offset());
    $("#" + TableID + "_tableHead").offset($("#" + TableID + "_tableLayout").offset());
    $("#" + TableID + "_tableColumn").offset($("#" + TableID + "_tableLayout").offset());
    $("#" + TableID + "_tableData").offset($("#" + TableID + "_tableLayout").offset());
}

function userFunction() {
    var str = "";
    Ext.Ajax.request({
        url: getUrl('MMShareBLL.DAL.ManageSystem', 'UserNameInit'),
        success: function(response) {
            if (response.responseText != "") {

                var userName = response.responseText.split('|');
                var UserArray = Ext.getDom("userSelectOpts");
                str = "<select name='userArray' class='listStytel' id='userArraySelect' onchange='selectStyleChange(this.options[this.options.selectedIndex].value);'>"

                for (var i = 0; i < userName.length; i++) {
                    str = str + String.format("<option value='{0}' class='optionCss'>{0}</option>", userName[i]);
                }
                str = str + "</select>";
                UserArray.innerHTML = str;
                //                  for(var i=0;i<userName.length;i++)
                //                  {
                //                   var childOption = document.createElement("option");
                //                   childOption.text=userName[i];
                //                   childOption.value=userName[i];
                //                   UserArray.appendChild(childOption);
                //                  }
                publicLogQuery();
            }
        },
        failure: function(response) {
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
        }
    });


}