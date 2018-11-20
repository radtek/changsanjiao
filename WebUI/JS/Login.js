//登录界面,用户进行用户的合法性验证
var loginReault;
function LoginJB() {
    var jb = document.getElementById("LoginJB");
    return jb;
}
Login = function (renderDiv, LoginBackFun) {
    //注册事件，一般都放在最前面，保证所有触发的地方都可以引用
    //登录成功后触发的事件
    // this.addEvents({ afterLogin: true });
    //alert(LoginBackFun);
    if (isShow) {
        Ext.getDom("txtUser").focus();
    }
    Ext.getDom("loginClick").onclick = function () { doSuccess() };
    Ext.getDom("loginCancel").onclick = function () { CancelClick() };
    function doSuccess() {
        var userName = Ext.getDom("txtUser").value;
        var password = Ext.getDom("txtPWD").value;
        if (userName != "" && password != "") {
            var myMask = new Ext.LoadMask(Ext.getBody(), { msg: "用户验证，请稍候..." });
            myMask.show();
            Ext.Ajax.request({
                url: getUrl('MMShareBLL.DAL.UserManager', 'Login'),
                params: { userName: userName, pssword: password, ip: "" },
                success: function (response) {
                    if (response.responseText != "") {
                        var result = Ext.util.JSON.decode(response.responseText);
                        addCookie("UserInfo", response.responseText, 0);
                        var result = Ext.util.JSON.decode(response.responseText);

                        window.location.href = "MainPage.aspx?JB=" + result["JB"] + "&userName=" + result.Alias;
                        loginValue = userName + ";" + password;
                        addCookie("URLSTR", (titleNamestr + ";" + loginValue), 0);
                        //用户通过验证
                        Ext.removeNode(Ext.getDom(renderDiv));

                    } else {
                        Ext.Msg.alert("提示", "登陆失败:用户名或密码不正确，请重新输入。");
                    }
                    myMask.hide();
                },
                //提交失败时执行的方法
                failure: function () {
                    myMask.hide();
                    Ext.Msg.alert("提示", "登陆失败:<br>未捕获到异常");
                },
                scope: this
            })
        }
        else {
            Ext.Msg.alert("提示", "用户名和密码不能为空");
        }
    }
    function CancelClick() {
        Ext.getDom("txtUser").value = "";
        Ext.getDom("txtPWD").value = "";
        Ext.getDom("txtUser").focus();
    }
    document.onkeydown = function (event) {
        e = event ? event : (window.event ? window.event : null);
        if (e.keyCode == 13)
            doSuccess();
    }
}
//继承，添加函数，或者重写函数
Ext.extend(Login, Ext.form.FormPanel, {
});


