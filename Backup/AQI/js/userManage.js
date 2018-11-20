var nameOld;
var passWordOld;
Ext.onReady(function(){
        supportInnerText();//使得火狐支持innerText
        initInputHighlightScript();
        userFunction();
    }
)
function userFunction()
{
 var str="";
 Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.ManageSystem','UserNameInit'),
        success: function(response){
            if(response.responseText!=""){
                
                  var userName= response.responseText.split('|');
                  var UserArray =Ext.getDom("userSelectOpt");
                  str="<select name='userArray' class='listStytel' id='userArraySelect' onchange='selectStyleChange(this.options[this.options.selectedIndex].value);'>"

                  for(var i=0;i<userName.length;i++)
                  {
                      str=str+String.format("<option value='{0}' class='optionCss'>{0}</option>",userName[i]);
                  }
                  str=str+"</select>";
                UserArray.innerHTML=str;
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
function publicLogQuery()
{
    var userName= Ext.getDom("userArraySelect").value;
   Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.ManageSystem','UserManageFun'),
        params: {userName:userName}, 
        success: function(response){
            if(response.responseText!=""){
                
                Ext.getDom("contentNone").innerHTML = response.responseText;
            }
        },
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
}
function selectStyleChange(value)
{
    var userName=value;
    Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.ManageSystem','UserManageFun'),
        params: {userName:userName}, 
        success: function(response){
            if(response.responseText!=""){
                
                Ext.getDom("contentNone").innerHTML = response.responseText;
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
 
}
function AllSelect()
{
 var bool;
 var objParent=document.getElementsByName("AllCheck");
 var obj=document.getElementsByName("UserCheck");
 if(objParent!=null)
 {
   if(!objParent[0].checked)
       bool=false;
   else 
       bool=true;
   if(obj!=null)
    {
      for(var i=0;i<obj.length;i++)
      {
        obj[i].checked=bool;
      }
    }
 }

   
}
function editorUser(userName,password)
{
  nameOld=userName;
  passWordOld=password;
Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.ManageSystem','ReEditor'),
        params: {userName:userName,password:password}, 
        success: function(response){
            if(response.responseText!=""){
                var userInformation=response.responseText.split('|');
                Ext.getDom("EditorName").value=userInformation[0];
                Ext.getDom("EditorpassWord").value=userInformation[1];
                Ext.getDom("EditorpassWord1").value=userInformation[1];
                Ext.getDom("EditorAlias").value=userInformation[2];
                 var arr=document.getElementsByName("Editorindex");
                if(userInformation[2]=="1")
                  arr[0].checked=true;
                else 
                  arr[0].checked=true;
                Ext.getDom("Editoremail").value=userInformation[4];
                hideEl("head");
                showEl("editorHidden"); 
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
}
function OKEditor()
{
  var name=Ext.getDom("EditorName").value;
  var psw=Ext.getDom("EditorpassWord").value;
  var psw1=Ext.getDom("EditorpassWord1").value;
  if(psw!=psw1)
  {
      Ext.Msg.alert("信息", "两次密码不一致！"); 
      Ext.getDom("EditorpassWord1").value="";
      return;
      
   }
  var alias=Ext.getDom("EditorAlias").value;
    var bz;
  var arr=document.getElementsByName("Editorindex");
  if(arr[0].checked)
    bz=arr[0].value;
  else 
     bz=arr[1].value;
  var reg = /^\w+((-\w+)|(\.\w+))*\@{1}\w+\.{1}\w{2,4}(\.{0,1}\w{2}){0,1}/ig;
  var email=Ext.getDom("email").value;
  if(email!=""&&!reg.test(email))
     {
        Ext.Msg.alert("信息", "邮箱格式不正确！"); 
        Ext.getDom("Editoremail").value="";
        return;
      }


  
      Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.ManageSystem','OkEditor'),
        params: {name:name,psw:psw,alias:alias,bz:bz,email:email,nameOld:nameOld,passWordOld:passWordOld}, 
        success: function(response){
            if(response.responseText!=""){
                if(name!=nameOld)
                {
                     var UserArray =Ext.getDom("userArraySelect");
                     for(var i=0;i<UserArray.options.length;i++)
                     {
                       if(UserArray.options[i].value==nameOld)
                       {
                         UserArray.options[i].value=name;
                         UserArray.options[i].text=name;
                         }
                     }
                 }
                hideEl("editorHidden");
                showEl("head"); 
                Ext.getDom("EditorName").value="";
                Ext.getDom("EditorpassWord").value="";
                Ext.getDom("EditorpassWord1").value="";
                Ext.getDom("EditorAlias").value="";
                Ext.getDom("Editoremail").value="";
                publicLogQuery();
                
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
  
}
function addUser()
{                  
    hideEl("head");
    showEl("addHidden");
}
function AddUserEditor()
{
  var name=Ext.getDom("Name").value;
  var psw=Ext.getDom("passWord").value;
  var psw1=Ext.getDom("passWord1").value;
  if(psw!=psw1)
  {
      Ext.Msg.alert("信息", "两次密码不一致！"); 
      Ext.getDom("passWord1").value="";
      return;
      
   }
  var alias=Ext.getDom("Alias").value;
  var bz;
  var arr=document.getElementsByName("index");
  if(arr[0].checked)
    bz=arr[0].value;
  else 
     bz=arr[1].value;
  var email=Ext.getDom("email").value;
   var reg = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
  if(email!=""&&!reg.test(email))
     {
        Ext.Msg.alert("信息", "邮箱格式不正确！"); 
        Ext.getDom("email").value="";
        return;
      }
  if(name==""||psw==""||bz=="")
  {
     Ext.Msg.alert("信息", "用户名或密码或者角色不能为空！"); 
     return;
   }
      Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.ManageSystem','AddUserEditor'),
        params: {name:name,psw:psw,alias:alias,bz:bz,email:email}, 
        success: function(response){
            if(response.responseText!=""){
                if(response.responseText=="用户已存在")
                   Ext.Msg.alert("信息", response.responseText+"！"); 
                else
                {    
                   var UserArray =Ext.getDom("userArraySelect");
                   var childOption = document.createElement("option");
                   childOption.text=name;
                   childOption.value=name;
                   UserArray.appendChild(childOption); 
                    hideEl("addHidden");
                    showEl("head");                                
                    Ext.getDom("Name").value="";
                    Ext.getDom("passWord").value="";
                    Ext.getDom("passWord1").value="";
                    Ext.getDom("Alias").value="";
                    Ext.getDom("email").value="";
                    publicLogQuery();
                }
                
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
}
function deleteUser(userName,password)
{
 Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.ManageSystem','DeleteUser'),
        params: {userName:userName,password:password}, 
        success: function(response){
            if(response.responseText!=""){
                if(response.responseText=="删除成功")
                {
                         var UserArray =Ext.getDom("userArraySelect");
                         for(var i=0;i<UserArray.options.length;i++)
                         {
                           if(UserArray.options[i].value==userName)
                           {
                             UserArray.options.remove(i); 
                             }
                         }
                         publicLogQuery();
                 }
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
}
function deleteSomeUsers()
{
   var checkUser=getCheckBValue("UserCheck");
   Ext.Ajax.request({ 
        url: getUrl('MMShareBLL.DAL.ManageSystem','DeleteCheckUser'),
        params: {checkUser:checkUser}, 
        success: function(response){
            if(response.responseText!=""){
                if(response.responseText=="删除成功")
                {
                         var UserArray =Ext.getDom("userArraySelect");
                         for(var i=0;i<UserArray.options.length;i++)
                         {
                           if(checkUser.indexOf(UserArray.options[i].value)>0)
                           {
                             UserArray.options.remove(i); 
                             }
                         }
                         publicLogQuery();
                 }
            }
        }, 
        failure: function(response) { 
            Ext.Msg.alert("错误", "请求失败，错误代码为：" + response.status); 
        }
     });
}
function getCheckBValue(objName)
{
    var postJson = "(";
    var obj=document.getElementsByName(objName);
    if(obj!=null)
    {
      for(var i=0;i<obj.length;i++)
      {
           if(obj[i].checked)
           {
                postJson=postJson+"'"+obj[i].value+"'"+",";
           }
      }
    }
    if(postJson.length>0)
    {
         postJson=postJson.substring(0,postJson.length-1);
    }
    return postJson+")";
}
function closeAdd()
{
    hideEl("addHidden");
    showEl("head");                                
    Ext.getDom("Name").value="";
    Ext.getDom("passWord").value="";
    Ext.getDom("passWord1").value="";
    Ext.getDom("Alias").value="";
    Ext.getDom("email").value="";
 
}
function closeReviseEditor()
{
    hideEl("editorHidden");
    showEl("head");                                
    Ext.getDom("EditorName").value="";
    Ext.getDom("EditorpassWord").value="";
    Ext.getDom("EditorpassWord1").value="";
    Ext.getDom("EditorAlias").value="";
    Ext.getDom("Editoremail").value="";
 
}
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
