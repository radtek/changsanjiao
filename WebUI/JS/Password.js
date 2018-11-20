Password = function(userName) {

    var xjcCenter = new Ext.form.FormPanel({
        frame: true,
        labelWidth: 80,
        layout: 'form',
        buttonAlign: 'center',
        items:
        [
            { xtype: 'textfield', id: 'userName', inputType: 'text', vtype: 'alphanum', emptyText:userName,disabled:true, width: 120, fieldLabel: '用户名' },
            { xtype: 'textfield', id: 'passOld', inputType: 'password', vtype: 'alphanum', aboutBlank: false, maxLength: 16, minLength: 4, width: 120, fieldLabel: '原始密码' },
            { xtype: 'textfield', id: 'passNew', inputType: 'password', vtype: 'alphanum', aboutBlank: false, maxLength: 16, minLength: 4, width: 120, fieldLabel: '新密码' },
            { xtype: 'textfield', id: 'passNewCheck', inputType: 'password', vtype: 'alphanum', aboutBlank: false, maxLength: 16, minLength: 4, width: 120, fieldLabel: '确认新密码' }
        ],
        buttons:
        [
            { xtype: 'button', text: '确认', width: 80, listeners: { 'click': AlertPass} },
            { xtype: 'button', text: '关闭', width: 80, listeners: { 'click': CloseWin} }
        ]
    });
    //继承函数
    Password.superclass.constructor.call(this, {
        id: 'password',
        width: 250,
        autoHeight:true,
        title: '修改密码',
//        layout: 'form',
//        bodyStyle: "padding:5 5 5 5",
        resizable: false,
        items: [xjcCenter]
    });
    //删除用户
    function AlertPass() {
        var passold = Ext.getCmp('passOld').getValue();
        if (passold == "") {
            Ext.Msg.alert('提示', '请输入旧密码!');
            return;
        }
        var passnew = Ext.getCmp('passNew').getValue();
        if (passnew == "") {
            Ext.Msg.alert('提示', '请输入新密码!');
            return;
        }
        var passnewc = Ext.getCmp('passNewCheck').getValue();
        if (passnewc == "") {
            Ext.Msg.alert('提示', '请输入确认的新密码!');
            return;
        }
        if (passnew != passnewc) {
            Ext.Msg.alert('提示', '新密码与确定密码不一致，请重新输入!');
            Ext.getCmp('passNew').setValue('');
            Ext.getCmp('passNewCheck').setValue('');
            Ext.getCmp('passNew').focus(true ,1000);
            return;
        }
        Ext.MessageBox.confirm("请确认", "是否真的要修改该用户的登录密码", function(button, text) {
            if (button == 'yes') {
                Ext.Ajax.request({
                    url: getUrl('MMShareBLL.DAL.UserManager', 'ChangePassword'),
                    success: function(response) {
                        if (response.responseText == "2") {
                            Ext.Msg.alert('提示', "原始密码输入错误.");
                            Ext.getCmp('passOld').setValue('');
                            Ext.getCmp('passOld').focus(true, 1000);
                        }
                        else {
                            Ext.Msg.alert('提示', "密码修改成功.");
                            CloseWin();
                        }
                    },
                    failure: function(response) {
                        Ext.Msg.alert('提示', "服务器未响应,密码修改失败.");
                    },
                    params: { userName: userName, passold: passold, passnew: passnew }
                });
            }
        });

    }
    //关闭
    function CloseWin() {
        var passWin = Ext.getCmp('password');
        passWin.close();
    }

}
//继承，添加函数，或者重写函数
Ext.extend(Password, Ext.Window, {

});

Ext.reg('password', Password);