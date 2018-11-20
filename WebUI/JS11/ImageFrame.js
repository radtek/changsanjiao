// 图片框架，根据产品名称调取图片

var flag = false;
var titleName = "";



var typeID;
var layerID = "";
var m_keyEvent = null;
var theFunct;
var playTask;
ImageFrame = function (fieldInfo, entity) {
    var publicStartDate = "";
    var theHtml = "";
    var publicEndDate = "";
    var publicHour = "";
    var Forcount = 0;
    var startTime;
    var endDateTime;
    var oldID = ""; //时间列表元素的ID
    var oldID_DateTime = ""; //时间列表元素的ID DateTime
    flag = true;
    var type = "";
    var layer = "";
    theFunct = trickQueryList;
    Ext.TaskMgr.stopAll();
    var entityName = fieldInfo[0].EntityName;
    var name = entity.name;
    //定义动画任务
    playTask = {
        run: function () {
            play(entityName);
        },
        interval: 700 //700 毫秒
    }
    var fieldValue;
    for (var i = 0; i < fieldInfo.length; i++) {
        fieldValue = fieldInfo[i];
        createControl(fieldValue);
    }
    if (m_keyEvent == null)
        AddKeyEvent(entityName);
    var imageViewer = new ImageViewer(Ext.BLANK_IMAGE_URL, entityName, name);
    imageViewer.render("ImageView");
    //*************************************************************************************************
    //  下面开始为此类的模块级别函数
    //*************************************************************************************************

    //增加键盘左右键
    function AddKeyEvent(viewID) {
        var el = Ext.getDoc();
        var component = Ext.getCmp(viewID);
        m_keyEvent = new Ext.KeyNav(el, {
            forceKeyDown: true,
            left: function (e) {
                MoveSelection(component, -1);
            },
            up: function (e) {
                MoveSelection(component, -1);
            },
            right: function (e) {
                MoveSelection(component, 1);
            },
            down: function (e) {
                MoveSelection(component, 1);
            },
            scope: el
        }
        );
        m_keyEvent.enable();
    }

    //点击查询以后罗列出查询列表
    function queryList(listID, isInitial) {
        if (entity.align == "F") {
            $("#nav").css("display", "none");
        }
        var fieldValue;
        var endTime;
        for (i = 0; i < fieldInfo.length; i++) {
            fieldValue = fieldInfo[i];
            if (fieldValue.FieldType == 8) {
                //开始时间
                if (Ext.getDom('startDate') == null)
                    startDateCmp = publicStartDate;
                else {
                    startDateCmp = Ext.getDom('startDate').value + " " + $("#startSelect").find("option:selected")[0].value + ":00:00";
                }
                var startDate = convertDate(startDateCmp);
                fieldValue.ShowValue = startDateCmp;

                startTime = startDate;
                //时效存在设置往后多少日期的情况
                if (entity.realtime == "" || entity.realtime.indexOf("R") == 0 || entity.realtime == "null") {
                    //结束时间
                    //结束时间
                    var endDateCmp = "";
                    if (Ext.getDom('endDate') == null)
                        endDateCmp = publicEndDate;
                    else {
                        endDateCmp = Ext.getDom('endDate').value + " " + $("#endSelect").find("option:selected")[0].value + ":00:00";
                    }
                    var endDate = convertDate(endDateCmp);
                    fieldValue.ShowValue = fieldValue.ShowValue + '||' + endDateCmp;

                    endTime = fieldValue.Name + " = '" + endDate.format('Y-m-d H:i:s') + "'";
                    if (endDateTime < startTime)
                        return;
                }



            } else if (fieldValue.FieldType == 4) {
                if (fieldValue.IsEvent == false) {
                    if (fieldValue.DictName.indexOf("_Type") >= 0)
                        fieldValue.ShowValue = type;
                    else
                        fieldValue.ShowValue = layer;

                } else {
                    var period = "";
                    fieldValue.ShowValue = "风场、气压场";
                    var obj = document.getElementsByName("topicOption");
                    if (obj != null && obj.length > 0) {
                        for (var i = 1; i < obj.length; i++) {
                            if (obj[i].checked) {
                                if (obj[i].value.indexOf('预报') > 0)
                                    period = "0" + obj[i].value.substring(0, 2);
                                else
                                    fieldValue.ShowValue = fieldValue.ShowValue + "、" + obj[i].value;

                            }
                        }
                    }
                    if (period != "")
                        fieldValue.ShowValue = fieldValue.ShowValue + "+" + period + "+" + endTime;

                }
            }
        }
        var entityObj = "";
        if (isInitial == false)
            entityObj = Ext.util.JSON.encode(fieldInfo);
        Ext.Ajax.request({
            url: getUrl('MMShareBLL.DAL.Forecast', 'QueryListHtml'),
            params: { entityName: entityName, entityObj: entityObj },
            success: function (response) {//topDiv
                var bottomDiv = "";
                bottomDiv = bottomDiv + response.responseText;
                if ($("#topHtml")[0].innerHTML == "")
                    $("#topHtml").html(theHtml);
                $("#bottomHtml").html(bottomDiv);
                var height = $("#bottomHtml")[0].offsetTop;
                $('.timePic').height($(window).height() - height - 85 - 40);
                //显示新滚动条，隐藏原滚动条
                $("#bottomHtml").find(".timePic").niceScroll({
                    cursorcolor: "#00CC33",
                    cursoropacitymax: 1,
                    touchbehavior: false,
                    cursorwidth: "5px",
                    cursorborder: "0",
                    cursorborderradius: "5px",
                    enablekeyboard: false, //设置nicescroll是否可以管理键盘事件
                    background: "#f2f2f2"
                });
                $("#startSelect").on("change", function () {
                    trickQueryList();
                })
                $("#endSelect").on("change", function () {
                    trickQueryList();
                })

                $("#timeTable tr").on("click", function () {
                    var triggerValue = this.childNodes['3'].innerHTML;
                    this.className = "trClick";
                    this.childNodes['0'].className = "tdClick";
                    if (entity.align == "F") {
                        var pdf_html = "<div id='frame' style='width:100%;height:100%;-webkit-overflow-scrolling:touch; overflow: auto;'><iframe src='" + triggerValue + "' height='100%' width='100%' frameborder='0'></iframe></div>";

                        $("#ImageView").html(pdf_html);
                        $("#ImageView").height($(window).height() - 85);
                    }
                    else {
                        imageViewer.setImageSrc(triggerValue, entityName, "");
                    }
                    if (this.id != oldID) {
                        if (oldID != "" && Ext.getDom(oldID) != null) {//自动播放后的oldID需要重置
                            Ext.getDom(oldID).className = "trClickOut";
                            Ext.getDom(oldID).childNodes['0'].className = "tdhover";
                        }
                    }
                    oldID = this.id;
                    $("#" + oldID)[0].scrollIntoView(false);
                    oldID_DateTime = this.childNodes['1'].innerHTML + " " + this.childNodes['2'].innerHTML;
                });
                var tr_len = $("#timeTable tr").length;
                //2016-02-24区分预报与非预报
                var lastIndex = 1;
                if (!f_Forecast)
                    lastIndex = $("#timeTable tr").length;

                $("#Tr" + lastIndex).click();

                $('input:checkbox').click(function () {
                    trickQueryList();
                });
                $("#timeTable tr").on({
                    mouseenter:
                    function () {
                        if (this.id != oldID)
                            this.className = "trHover";
                    },
                    mouseleave:
                    function () {
                        if (this.id != oldID)
                            this.className = "trOut";
                    }
                });
                $(".tableWidth tr").on({
                    mouseenter:
                    function () {
                        if (this.className != "topicCheckOver")
                            this.className = "topicCheckEnter";
                    },
                    mouseleave:
                    function () {
                        if (this.className != "topicCheckOver")
                            this.className = "topicCheck";
                    }
                });
                $(".tableWidth tr").on("click", function () {
                    var triggerValue = this.childNodes[0].innerHTML; //this.parentNode.parentNode.id
                    if (this.parentNode.parentNode.id == "typeTable") {
                        if (this.id != typeID) {
                            this.className = "topicCheckOver";
                            Ext.getDom(typeID).className = "topicCheck";
                            typeID = this.id;
                            type = triggerValue;
                            trickQueryList();
                        }

                    }
                    else {
                        if (this.id != layerID) {
                            this.className = "topicCheckOver";
                            Ext.getDom(layerID).className = "topicCheck";
                            layerID = this.id;
                            layer = triggerValue;
                            trickQueryList();
                        }
                    }
                });
            },
            failure: function (response) {
                //                Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
            }
        });

    }
    function getCheckBValue(objName) {
        var postJson = "";
        var obj = document.getElementsByName(objName);
        if (obj != null) {
            for (var i = 0; i < obj.length; i++) {
                if (obj[i].checked) {
                    postJson = postJson + obj[i].value + ",";
                }
            }
        }
        if (postJson.length > 0) {
            postJson = postJson.substring(0, postJson.length - 1);
        }
        return postJson;
    }
    //依次选择要素
    function play(listID) {
        //存在列表选项
        var intTimes = $("#speed").find("option:selected")[0].value
        if ($("#timeTable tr").length > 0) {
            oldID = oldID == "" ? $("#timeTable tr")[0].id : oldID;
            var fieldControl = Ext.getDom(oldID);
            var index = parseInt(fieldControl.id.substr(2));
            var totalLength = $("#timeTable tr").length;
            var nowIndex;
            if (index + parseInt(intTimes) <= totalLength)
                nowIndex = index + parseInt(intTimes);
            else
                nowIndex = index + parseInt(intTimes) - totalLength;
            $("#Tr" + nowIndex).click();
        }
    }
    //在查询的结果列表框中选择
    function selectChange(view, nodes) {
        view.focus();
        var nodes = view.getNodes();
        var selectionsArray = view.getSelectedIndexes();
        if (selectionsArray.length > 0) {
            nodes[selectionsArray[0]].scrollIntoView();
            var store = view.getStore();
            var triggerValue = store.getAt(selectionsArray[0]).get("DM");

            imageViewer.setImageSrc(triggerValue, startTime, endDateTime);
        } else {
            imageViewer.setImageSrc(Ext.BLANK_IMAGE_URL, startTime, endDateTime);
        }
        startTask(playTaskRefresh);
    }
    function trickQueryList() { queryList(entityName, false); }

    function createControl(fieldValue) {
        var fieldControl;

        //字典字段
        if (fieldValue.FieldType == 4) {
            if (fieldValue.IsEvent == false) {
                //此处设置为autoLoad，那么就会自动请求服务器
                var store = {};
                if (fieldValue.YField == "" || fieldValue.YField == "Type") {
                    Ext.Ajax.request({
                        url: getUrl('MMShareBLL.DAL.DictionaryManager', 'GetDictionary'),
                        params: { dictionaryName: fieldValue.DictName, orderIndex: fieldValue.OrderIndex },
                        success: function (response) {
                            Forcount++;
                            if (response.responseText != "") {
                                var temp = "";
                                var result = Ext.util.JSON.decode(response.responseText);
                                theHtml = theHtml + "<div class='topic'>";
                                theHtml = theHtml + "<div class='topicLable'>" + fieldValue.Alias + "</div>";
                                theHtml = theHtml + "<div class='MaxHeight'>";
                                if (fieldValue.DictName.indexOf("_Type") >= 0) {
                                    theHtml = theHtml + "<table id='typeTable' class='tableWidth'>";
                                    type = result[0].MC;
                                    temp = "Type";
                                    typeID = "para" + temp + result[0].DM;
                                }
                                else {
                                    theHtml = theHtml + "<table id='layerTable' class='tableWidth'>";
                                    layer = result[0].MC;
                                    temp = "Layer";
                                    layerID = "para" + temp + result[0].DM;
                                }
                                var layerHtml = "";
                                for (var i = 0; i < result.length; i++) {
                                    if (i == 0)
                                        layerHtml = layerHtml + "<tr class='topicCheckOver'  id='para" + temp + result[i].DM + "'>";
                                    else
                                        layerHtml = layerHtml + "<tr class='topicCheck'  id='para" + temp + result[i].DM + "'>";
                                    layerHtml = layerHtml + "<td  id='" + result[i].DM + "'>" + result[i].MC + "</td>";
                                    layerHtml = layerHtml + "</tr>";
                                }
                                theHtml = theHtml + layerHtml + "</table >";
                                theHtml = theHtml + "</div'>";
                                theHtml = theHtml + "</div>";

                                if (Forcount == fieldInfo.length) {
                                    trickQueryList();
                                    Forcount = 0;
                                }

                            }
                        },
                        failure: function (response) {
                            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status);
                        }
                    })
                }
            } else {
                theHtml = theHtml + "<div class='topic'>";
                theHtml = theHtml + "<div class='topicLable'>" + fieldValue.Alias + "</div>";
                theHtml = theHtml + "<div class='topicCheck'><label><input type='checkbox'  disabled='disabled' checked='checked' name='topicOption' value='风场、气压场'/>风场、气压场</label></div>";
                theHtml = theHtml + "<div class='topicCheck'><label><input type='checkbox'  name='topicOption' value='温度'/>温度</label></div>";
                theHtml = theHtml + "<div class='topicCheck'><label><input type='checkbox'  name='topicOption' value='12小时预报</'/>12小时预报</label></div>";
                theHtml = theHtml + "<div class='topicCheck'><label><input type='checkbox'  name='topicOption' value='24小时预报'/>24小时预报</label></div>";
                theHtml = theHtml + "<div class='topicCheck'><label><input type='checkbox'  name='topicOption' value='36小时预报'/>36小时预报</label></div>";
                theHtml = theHtml + "</div>";
                trickQueryList();

            }
        }
        else if (fieldValue.FieldType == 8) {
            Forcount++;
            var lastTimes = fieldValue.ShowValue.split(" ");

            if (entity.realtime == "" || entity.realtime.indexOf("R") == 0 || entity.realtime == "null") {
                f_Forecast = false;
                var startDate = convertDate(lastTimes[0]);
                var backDay = -1;
                //对于实时数据可以通过设置T_ImageProduct表中的Period来控制往后的天数
                if (entity.realtime.indexOf("R") == 0)
                    backDay = -parseInt(entity.realtime.substr(2));
                startDate = startDate.add(Date.DAY, backDay);
                var hour = lastTimes[1].substring(0, 2);
                publicHour = hour;
                var endDate = convertDate(lastTimes[0]);
                publicStartDate = startDate.format('Y-m-d H') + ":00:00";
                endDate = endDate.add(Date.HOUR, hour);
                publicEndDate = endDate.format('Y-m-d H') + ":00:00";
                theHtml = theHtml + "<div class='topselect' id='topDiv'>";
                theHtml = theHtml + "<div class='timeLable'>开始:</div>"; //[parseInt(periods[i], 10)
                theHtml = theHtml + "<div >";
                theHtml = theHtml + "<div class='dispalyFloat'><input type='text' id='startDate' class='selectDateFormStyle'   value='" + startDate.format('Y-m-d') + "'  onclick=\"WdatePicker({dateFmt:'yyyy-MM-dd ',onpicked:inputClick})\"/></div>";
                theHtml = theHtml + "<div class='dispalyFloat'><select class='selectForm' id='startSelect'>";
                var selectHtml = "";
                for (var i = 0; i < 24; i++) {
                    if (parseInt(hour) == i)
                        if (i < 10)
                            selectHtml = selectHtml + "<option value='0" + i.toString() + "' selected='selected'>0" + i.toString() + "</option >";
                        else
                            selectHtml = selectHtml + "<option value='" + i.toString() + "' selected='selected'>" + i.toString() + "</option >";

                    else {
                        if (i < 10)
                            selectHtml = selectHtml + "<option value='0" + i.toString() + "'>0" + i.toString() + "</option >";
                        else
                            selectHtml = selectHtml + "<option value='" + i.toString() + "'>" + i.toString() + "</option >";
                    }
                }
                theHtml = theHtml + selectHtml + "</select></div>";
                theHtml = theHtml + "</div>";
                theHtml = theHtml + "<div class='timeLable'>结束:</div>";
                theHtml = theHtml + "<div class='marginCls'>";
                theHtml = theHtml + "<div class='dispalyFloat'><input  id='endDate' type='text' class='selectDateFormStyle'  value='" + endDate.format('Y-m-d') + "'  onclick=\"WdatePicker({dateFmt:'yyyy-MM-dd ',onpicked:inputClick})\"/></div>";
                theHtml = theHtml + "<div class='dispalyFloat'><select class='selectForm' id='endSelect'>";
                theHtml = theHtml + selectHtml + "</select></div>";
                theHtml = theHtml + "</div>";
                if (Forcount == fieldInfo.length) {
                    trickQueryList();
                    Forcount = 0;
                }
            }
            else {
                f_Forecast = true;
                var periods = entity.realtime.split(",");
                var periodData = new Array;
                var lastHour = lastTimes[1].split(":");
                var startDate = convertDate(lastTimes[0]);
                publicStartDate = startDate.format('Y-m-d') + " " + lastHour[0] + ":00:00";


                theHtml = "<div class='topselect' id='topDiv'>";
                theHtml = theHtml + "<div class='timeLable'>起报时间:</div>"; //[parseInt(periods[i], 10)
                theHtml = theHtml + "<div class='marginCls'>";
                theHtml = theHtml + "<div class='dispalyFloat'><input type='text' id='startDate' class='selectDateFormStyle'   value='" + startDate.format('Y-m-d') + "'  onclick=\"WdatePicker({dateFmt:'yyyy-MM-dd ',onpicked:inputClick})\"/></div>";
                theHtml = theHtml + "<div class='dispalyFloat'><select class='selectForm' id='startSelect'>";
                var selectHtml = "";
                for (var i = 0; i < periods.length; i++) {
                    if (periods[i] == lastHour[0])
                        selectHtml = selectHtml + "<option value='" + periods[i] + "' selected='selected'>" + periods[i] + "</option >";
                    else {
                        selectHtml = selectHtml + "<option value='" + periods[i] + "'>" + i.toString() + "</option >";
                    }
                }
                theHtml = theHtml + selectHtml + "</select></div>";
                theHtml = theHtml + "</div>";
                if ((Forcount == fieldInfo.length - 1 && fieldInfo.length == 3) || fieldInfo.length == 1) {
                    trickQueryList();
                    Forcount = 0;
                }
            }
        }

    }

}

function inputClick() {
    theFunct();
}
function playSpeed(el, speed) {
    if (playTask.interval < 2000) playTask.interval = playTask.interval + parseInt(speed);
}
function playToggle(el) {
    if (el.className == "Speennormal_D" || el.className == "Speennormal_V") {
        startTask(playTask);
        el.className = "SpeennormalStop_D";
    }
    else {
        stopTask(playTask);
        el.className = "Speennormal_D";
    }
}
//继承，添加函数，或者重写函数
Ext.extend(ImageFrame, Ext.Panel, {

});
//注册，可以通过xtype:imageViewer来创建面板
Ext.reg('imageFrame', ImageFrame); 

