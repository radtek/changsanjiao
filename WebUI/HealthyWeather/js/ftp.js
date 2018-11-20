var selctRegion = "全部";   //在创建用户或编辑用户时读取左边所选的区域
var tip   //创建和编辑确认完成时弹出提醒
var userid="";
var count="";
$(function () {
    countII();
    ftpQuery(selctRegion);
    selRegion();


    var html = "", types = ["儿童感冒", "青年感冒", "老年感冒", "COPD", "儿童哮喘", "中暑", "重污染"];
    for (var i = 0; i < types.length; i++) {
        html += "<option value='" + types[i] + "'>" + types[i] + "</option>";
    }


    $("#selPubRegion").html(html);

    $('#selPubRegion').selectpicker({
        'selectedText': 'cat',
        'noneSelectedText': '请选择'
    });
});
function selRegion() {
    var html = "<a href='#' class='list-group-item active' id='list-item0'>" +"全部"+ "<span class='badge' id='count0'><span></a>";
    var regionCr="";  //区域下拉选项
    var region = ["中心城区", "浦东新区", "闵行区", "宝山区", "松江区", "金山区", "青浦区", "奉贤区", "嘉定区", "崇明"];
    for (var i = 0; i < region.length; i++) {
        html += "<a href='#' class='list-group-item' id='list-item" + i + "'>" + region[i] +"</a>";
        regionCr +="<option>"+region[i]+"</option>";
    }
    $("#regionCre").html(regionCr);
    $("#region").html(html);
    $("#region .list-group-item").click(function () {
        $("#region .active").removeClass("active");
        $(this).addClass("active");
        selctRegion = this.innerHTML.split('<span')[0];
        ftpQuery(selctRegion);
    });
}
function delFTP(ftpid){
    Ext.Ajax.request({
    url:getUrl('MMShareBLL.DAL.FTP_Manage','delFTP'),
    params:{ FTPID:ftpid },
    success: function (response) {
        alert("恭喜您，成功删除该条信息");
        ftpQuery(selctRegion);
        countII();
        }
    });
}
function ftpQuery(region) {
    $('#ftpTab').empty();
    var ftpTable = $('#ftpTab').DataTable({
        "bProcessing": true,
        "sScrollX": "100%",
        "sScrollXInner": "100%",
        "bDestroy": true,
        "bFilter": true, "bPaginate": true,
        "bSortClasses": false,
        "order": [[ 6, "desc" ]],
        "sAjaxSource": "PatrolHandler.do?provider=MMShareBLL.DAL.FTP_Manage&method=ftpQuery&regions=" + region,
        "columns": [
            {"title":"FTPID","class":"center"},
            { "title": "FTP账号", "class": "center" },
            { "title": "密码", "class": "center" },
            { "title": "地址", "class": "center" },
            { "title": "端口号", "class": "center" },
            { "title": "目录", "class": "center" },
            { "title": "创建日期", "class": "center" },
            { "title": "区域", "class": "center" },
            { "title": "接收单位", "class": "center" },
            { "title": "接收产品", "class": "center" },
            { "title": "编辑", "class": "center" },
            { "title": "删除", "class": "center" }],
        "oLanguage": {
            "sProcessing": "<img src='./images/loading.gif'/></br>",
            "sLengthMenu": "每页显示 _MENU_ 条记录",
            "sZeroRecords": "对不起，查询不到相关数据！",
            "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
            "sInfoEmpty": "当前显示 0 到 0 条记录，共 0 条",
            "sEmptyTable": "表中无数据存在！",
            "sInfoFiltered": "数据表中共为 _MAX_ 条记录",
            "sSearch": "用户搜索",
            "oPaginate": { "sFirst": "首页", "sPrevious": "上一页", "sNext": "下一页", "sLast": "末页" }
        },
         "columnDefs": [
         {
             "targets":0,
             "visible":false
         }, 
         {
            "targets": 10,"data": null,
            "defaultContent": "<img src=\"images/edit.png\" title=\"编辑\" style=\"cursor: pointer\"></img>"
        }, {
            "targets": 11,"data": null,
            "defaultContent": "<img src=\"images/deleteIcon.png\" title=\"删除\" style=\"cursor: pointer\"></img>"
        }]
    });
    $("#ftpTab tbody").on("click","img",function(obj){
        var myData=ftpTable.row($(this).parents("tr")).data();
        if(this.title=="删除"){
            if(confirm("您确认要删除这条信息吗？")){
                delFTP(myData[0]);
                }
        }else{
            create("编辑FTP",myData);
        }
    });
}
function create(title,info) {
    tip="恭喜您，创建成功！"  //创建用户时提醒
    document.getElementById('address').value="";
    document.getElementById('port').value="";
    document.getElementById('password').value="";
    document.getElementById('accout').value = "";
    document.getElementById('contents').value = "";
    document.getElementById('reciver').value = "";
    $("#selPubRegion").selectpicker('val', "儿童感冒, 青年感冒, 老年感冒");
    if(selctRegion=="全部"){
        $('#myModal').modal('show');
        $('.modal-body').css("height","189px");
        $("#regionT").css("display","block");
        $("#regionCre").css("display","block");
        $("#tip").addClass("hidden");
        document.getElementById("tipCon").innerHTML = "提示：/表示根目录";  //王斌  2017.5.16
    }
    else{
        $('#myModal').modal('show');
        $('.modal-body').css("height", "189px");    //王斌  2017.5.16
        $("#regionT").css("display","none");
        $("#regionCre").css("display","none");
        $("#tip").removeClass("hidden");
        document.getElementById("tipCon").innerHTML = "，/表示根目录";   //王斌  2017.5.16
        document.getElementById('tipR').innerHTML=selctRegion;
    }
    $('#myModalTitle').text(title);
    var obj = ['address', 'port', 'accout', 'password', 'contents', 'reciver', 'selPubRegion']
    if(title.indexOf('编辑')>-1){
        $("#"+obj[0]).val(info[3]);
        $("#"+obj[1]).val(info[4]);
        $("#"+obj[2]).val(info[1]);
        $("#" + obj[3]).val(info[2]);
        $("#" + obj[4]).val(info[5]);
        $("#" + obj[5]).val(info[8]);  // xuehui 06-15
        var arr = info[9].split(',');
        $("#" + obj[6]).selectpicker('val', arr); // xuehui 07-25
        tip="恭喜您，编辑成功";   //编辑用户时提醒
        userid=info[0];
    }
    else{
        userid="";
    }
    countII();
}

function queren() {
    var accout = document.getElementById("accout").value;
    var password = document.getElementById("password").value;
    var address = document.getElementById("address").value;
    var port = document.getElementById("port").value;
    var content = document.getElementById("contents").value;  //王斌  2017.5.16
    var revicer = document.getElementById("reciver").value;  //王斌  2017.5.16
    var products = $("#selPubRegion").val();  //xuehui  2017.7.25

    if (content.charAt(content.length - 1) != "/") {           //wb   2017.5.25
        content = content + "/";
    }


    if(selctRegion=="全部"){
        selctRegion=document.getElementById("regionCre").value;
    }
    if (accout == "" || password == "" || address == "" || port == "" || content == "" || revicer == "" || products == "") {   //王斌  2017.5.16
        alert("选项不能为空，请仔细核查");
    }
    else{
         Ext.Ajax.request({
            url:getUrl('MMShareBLL.DAL.FTP_Manage', 'confirm'),
            params: { accouts: accout, passwords: password, addresses: address, ports: port, idd: userid, regions: selctRegion, contents: content,reciver: revicer,Products:products }, //xuehui  2017.6.15
            success: function (response) {
                alert(tip);
                $('#myModal').modal('hide');
                selctRegion=$("#region .active").html().split('<span')[0];
                ftpQuery(selctRegion);
                countII();
            },
            failure:function(){
                alert("操作失败，请仔细检查！");
            }
        });
    }
}

//统计数量
function countII(){
    Ext.Ajax.request({
        url:getUrl('MMShareBLL.DAL.FTP_Manage', 'count'),
        success:function(response){
            count=response.responseText;
            document.getElementById("count0").innerHTML=count;
        }
    });
}