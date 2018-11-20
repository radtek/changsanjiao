// JScript 文件

//广播会商
function Broadcast(elements){
    //var els = ["H05","H06","H09","H10","PH09"];//获取气象和环境共享的输入单元格
    var postJson = "{";
    for(i=0;i<elements.length;i++){
        var el = Ext.getDom(elements[i]);
        postJson = postJson + elements[i] + ":'" +  el.value + "',";
    }
    postJson = postJson.substr(0,postJson.length-1) + "}";
    
    Ext.Ajax.request({ 
        url: "broadcast.asyn",
        params: { content: postJson}
    });  
}

function wait() {
     Ext.Ajax.request({ 
        url: "broadcast.asyn",
        params: { content: "-1"}, 
        timeout:0,
        success: function(response){
            if(response.responseText != ""){
                var result = Ext.util.JSON.decode(response.responseText);
                changeDateSucessed(result);
            }
            //服务器返回消息,再次立连接
            wait();
        }
     });  
}