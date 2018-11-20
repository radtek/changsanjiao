//和界面交互的主文件

//初始化树
var lastTab = "会商PPT_0";
var nd = new TreeNode(); //创建树对象
nd.container = $("tree"); //找到容器
nd.text = "在线文档管理";
nd.Show();

currentNode = nd;
var date = new Date();
var year = date.getFullYear();
var month = date.getMonth() + 1;
var day = date.getDate();
if (month.toString().length == 1)
    month = "0" + month;
if (day.toString().length == 1)
    day = "0" + day;

Ext.getDom("H00").value = year + '-' + month + '-' + day;

currentNode.Refersh();

//加载对话框
var dialog = new Dialog();
dialog.ImgZIndex = 110;
dialog.DialogZIndex = 111;

//下载操作
clickFile = function (fName) {
    window.onbeforeunload = function () { }
    window.location = defaultURL + "?action=DOWNLOAD&value1=" + encodeURIComponent(currentNode.path + fName);
//    window.onbeforeunload = function () {
//        return "无法保存当前状态，请退出！"
//    };
}

clickFileII = function (fName,fSize) {
    window.onbeforeunload = function () { }
    window.location = defaultURL + "?action=DOWNLOADII&value1=" + encodeURIComponent(currentNode.path + fName + '#' + fSize.innerHTML);
    //    window.onbeforeunload = function () {
    //        return "无法保存当前状态，请退出！"
    //    };
}
//在线编辑操作
var fileEditor = new Dialog();
fileEditor.Content = "<div id='editorDiv'><input id='switchEditor' type='hidden'/><br><textarea id='FileContentTextArea' name='FileContentTextArea' cols='80' rows='30' style='width:600px;height:400px'></textarea></div>";
fileEditor.Width = 626;

var oFCKeditor; //富文本编辑器对象
var oEditor; //定义富文本比机器示例
var isRichEditor = false; //当前编辑器状态
var currentContent = ""; //当前内容
var textareaEditor = null; //文本编辑器
var switchButton = null; //切换按钮
var list = null; //选中的文件
var cutCopyOperate = null; //当前操作
var cutCopyFiles = null; //操作的文件
var currentEditFile = null;//要编辑的文件
var dialogs;
//在切换回普通文本框时发生
function FCKeditor_OnComplete(editorInstance) {
    oEditor = editorInstance;
    if (currentContent != null) {
        oEditor.SetData(currentContent);
    }
}

function queryData() {
    window.location = "../ChangjiangDataII/WebExplorerssII.htm";
}

function update() {
    alert("xxx");
}

function radioClick(id) {
    var el = Ext.getDom(id);
    if (el.className == "radioUnChecked") {
        el.className = "radioChecked";
        getGroupEl(id);
        currentNode.Refersh();
    }
}

function tabClick(id) {
    var lastParts = lastTab.split("_");
    var lastID = lastParts[1];
    var curParts = id.split("_");
    currentNode.CurTab = curParts[0];
    var curEl = Ext.getDom(id);
    var lastEl = Ext.getDom(lastTab);
    lastEl.className = "";
    lastEl.innerHTML = curEl.innerHTML.replace(id, lastTab).replace(curParts[0], lastParts[0]);
    curEl.className = "tabHighlight";
    curEl.innerHTML = curParts[0];
    var currentID = curParts[1];
    var tableTitle = document.getElementById("Tb" + currentID);
    var forecastTable = document.getElementById("Tb" + lastID);
    tableTitle.className = "show";
    forecastTable.className = "hidden";
    lastTab = id;
    currentNode.Refersh();
}

function refesh() {
    currentNode.Refersh();
}

function getGroupEl(id) {
    var groupEl = "";
    switch (id) {
        case "rd1":
            groupEl = "rd2";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            groupEl = "rd3";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            groupEl = "rd4";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            break;
        case "rd2":
            groupEl = "rd1";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            groupEl = "rd3";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            groupEl = "rd4";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            break;
        case "rd3":
            groupEl = "rd1";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            groupEl = "rd2";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            groupEl = "rd4";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            break;
        case "rd4":
            groupEl = "rd1";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            groupEl = "rd2";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            groupEl = "rd3";
            el = Ext.getDom(groupEl);
            el.className = "radioUnChecked";
            break;
    }
}

function switchEditor() {
    getEditorContent(); //先获取要编辑的内容
    if (isRichEditor) {
        isRichEditor = false;
        //创建新文件
        newFile();
    }
    else {
        isRichEditor = true;
        //创建编辑器
        createEditor();
    }
}
//创建一个编辑器
function createEditor() {
    if (oFCKeditor == null) {
        oFCKeditor = new FCKeditor('FileContentTextArea');
        oFCKeditor.BasePath = "../fckeditor/";
        oFCKeditor.Width = '600';
        oFCKeditor.Height = '400';
        oFCKeditor.ToolbarSet = 'Dialog';
        oFCKeditor.Config['FullPage'] = true;
    }
    oFCKeditor.ReplaceTextarea();
}

//获取编辑器的内容
function getEditorContent() {
    if (isRichEditor) {
        currentContent = oEditor.GetXHTML(true); //这是FCK的情况
    }
    else {
        currentContent = textareaEditor.value;
    }
}

//创建新文件
function newFile() {
    isRichEditor = false;
    fileEditor.Text = "新建文件";
    fileEditor.Show(2);
    //找到编辑器的元素
    textareaEditor = $("FileContentTextArea");
    switchButton = $("switchEditor");

    textareaEditor.value = currentContent;
    switchButton.onclick = switchEditor;
    fileEditor.Close = closeConfirm;
    fileEditor.OK = function () {
        dialog.Content = "<span>请输入文件名：</span><input id='inputFile' type='textbox' style='width:100px'/>";
        dialog.Show(2);
        dialog.OK = saveNewFile;//调用保存新文件
    }
}

//保存新建的文件
function saveNewFile() {
    var result = saveFile("NEWFILE", $("inputFile").value);
    dialog.Text = "提示";
    if (result == "OK") {
        dialog.Content = "文件创建成功！";
        currentNode.Refersh();
        fileEditor.Hide();
        dialog.Show(1);
        dialog.OK = dialog.Close;
    }
}
//保存文件
function saveFile(action,fileName)
{
getEditorContent();
var url=defaultURL+"?action="+action+"&value1="+encodeURIComponent(currentNode.path+fileName);
return executeHttpRequest("POST",url,"content="+encodeURIComponent(currentContent));
}
//退出确认对话框
function closeConfirm() {
    dialog.Text = "提示";
    dialog.Content = "<span style='color:red;'>文本已经改变，确定要退出吗？</span>";
    dialog.Show();
    dialog.OK = function () {
        dialog.Close();
        fileEditor.Hide();
        currentContent = "";//***************
    }
}
//编辑文件
editFile = function (fName) {
    isRichEditor = false;
    currentEditFile = fName;
    fileEditor.Text = "修改文件：" + fName;
    //    fileEditor.
    fileEditor.Show(3);

    textareaEditor = $("FileContentTextArea");
    switchButton = $("switchEditor");

    textareaEditor.value = loadFileContent(fName); //获取要编辑的文件 
    switchButton.onclick = switchEditor;
    fileEditor.Close = closeConfirm;
    fileEditor.OK = function () {
        var result = saveFile("SAVEEDITFILE", fName);
        dialog.Text = "提示";
        if (result == "OK") {
            dialog.Content = "文件保存成功！";
            currentNode.Refersh();
            fileEditor.Hide();
        }
        else {
            dialog.Content = "文件保存失败！";
        }
        dialog.OK = dialog.Close;
        dialog.Show(1);
    }
    fileEditor.Retry = function () {
        if (isRichEditor) {//如果是富文本编辑器
            oEditor.SetData(loadFileContent(fName));
        }
        else {
            textareaEditor.value = loadFileContent(fName);
        }
    }

}

//读取服务器文件
loadFileContent=function(fName)
{
    var url = defaultURL + "?action=GETEDITFILE&value1=" + encodeURIComponent(currentNode.path + fName);
var content=executeHttpRequest("GET",url,null);

if(content=="ERROR")
{
dialog.Text="错误";
dialog.Content="读取文件出错";
 dialog.Show(1);
 content="";
 dialog.OK=dialog.Close;
}
return content;
}

//****************************************************************
//***********************    以下是新加代码  *********************
//****************************************************************

//返回上级目录
function gotoParentDirectory() {
    if (currentNode != null) {
        currentNode.GotoParentNode();
    }
}

//根目录
function goRoot() {
    currentNode = nd;
    currentNode.Refersh();
}

//刷新
function refersh() {
    if (currentNode != null) {
        currentNode.Refersh();
    }
}

//全选
function selectAll() {
    var checkBoxes = $("fileList").getElementsByTagName("input");

    for (var i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].type == "checkbox") {
            checkBoxes[i].checked = $("checkAll").checked;
        }
    }
}
//新建目录
function newDirectory() {
    dialog.Content = "<div><span>请输入新目录名称：</span><input id='dirName' type='textbox' style='width:100px;' /></div>";
    dialog.Text = "新建目录";
    dialog.Show();

    var dir = $("dirName");
    dir.focus();

    dialog.OK = function () {
        if (!(/^[\w\u4e00-\u9fa5-\.]+$/.test(dir.value))) {
            dialog.Content = "<span style='color:red;'>目录名称不正确！</span>";

            dialog.OK = function () {
                newDirectory();
            }

            dialog.Show(1);
            return;
        }

        var url = defaultURL + "?action=NEWDIR&value1=" + encodeURIComponent(currentNode.path + dir.value);
        var result = executeHttpRequest("GET", url, null);

        if (result == "OK") {
            currentNode.Refersh();
            dialog.Content = "目录创建成功！";
            dialog.Show(1);
            dialog.OK = dialog.Close;
        }
        else {
            dialog.Content = "<span style='color:red;'>目录创建失败，请重试！</span>";
            dialog.Show();

            dialog.OK = function () {
                newDirectory();
            }
        }
    }
}
//检查文件名合法性
function checkFileName() {
    return (/^[\w\u4e00-\u9fa5-\.]+\.[a-zA-Z0-9]{1,8}$/.test($("inputFile").value));
}
//获取选中的文件
function getSelectedFile() {
    list = [];
    var checkBoxes = $("fileList").getElementsByTagName("input");

    for (var i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].type == "checkbox") {
            if (checkBoxes[i].checked) {
                list.push(currentNode.path + checkBoxes[i].title);
            }
        }
    }
}
function checkChange()
{
   getSelectedFile();
   var checkBoxes = $("fileList").getElementsByTagName("input");
   if(list.length==checkBoxes.length)
     $("checkAll").checked=true; 
   else 
     $("checkAll").checked=false; 
}
function del() {
    getSelectedFile();
    dialog.Text = "删除文件";

    if (list.length <= 0) {
        dialog.Content = "请选择要删除的文件！";

        dialog.OK = dialog.Close;

        dialog.Show(1);
        return;
    }

    dialog.Content = "这些文件删除后将不可恢复，是否确定？";
    dialog.Show();

    dialog.OK = function () {
        var url = defaultURL + "?action=DELETE&value1=" + encodeURIComponent(list.join("|"));
        var result = executeHttpRequest("GET", url, null);

        if (result == "OK") {
            currentNode.Refersh();

            dialog.Content = "目录删除成功！";
            dialog.OK = dialog.Close;
            dialog.Show(1);
        }
        else {
            dialog.Content = "<span style='color:red;'>目录删除失败，请重试！</span>";
            dialog.Show(1);
            currentNode.Refersh();
            dialog.OK = dialog.Close;
        }
    }
}

function cut() {
    getSelectedFile();
    dialog.Text = "剪切";

    if (list.length < 1) {
        dialog.Content = "请选择要剪切的文件及文件夹。";
    }
    else {
        dialog.Content = "已剪切以下文件及文件夹：<br /><div style='text-align:left;'>" + list.join("<br />") + "</div>";
        cutCopyOperate = "CUT";
        cutCopyFiles = list;
    }

    dialog.Show(1);
    dialog.OK = dialog.Close;
}

function copy() {
    getSelectedFile();
    dialog.Text = "复制";

    if (list.length < 1) {
        dialog.Content = "请选择要复制的文件及文件夹。";
    }
    else {
        dialog.Content = "已复制以下文件及文件夹：<br /><div style='text-align:left;'>" + list.join("<br />") + "</div>";
        cutCopyOperate = "COPY";
        cutCopyFiles = list;
    }

    dialog.Show(1);
    dialog.OK = dialog.Close;
}

function paste() {
    dialog.Text = "粘贴";

    if (cutCopyOperate != 'CUT' && cutCopyOperate != 'COPY' || cutCopyFiles.length < 1) {
        dialog.Content = "剪贴板没有任何文件及文件夹。";
        dialog.OK = dialog.Close;
        dialog.Show(1);
        return;
    }

    dialog.Content = "确认将以下文件粘贴到当前目录：" + currentNode.path + "？<br /><div style='text-align:left;'>" + cutCopyFiles.join("<br />") + "</div>";
    dialog.Show();

    dialog.OK = function () {
        var url = defaultURL + "?action=" + cutCopyOperate + "&value1=" + encodeURIComponent(currentNode.path) + "&value2=" + encodeURIComponent(cutCopyFiles.join("|"));
        var result = executeHttpRequest("GET", url, null);

        if (result == "OK") {
            dialog.Content = "粘贴操作成功！";
            currentNode.Refersh();
        }
        else {
            dialog.Content = "<span style='color:red;'>粘贴操作失败！</span>";
        }

        dialog.Show(1);
        dialog.OK = dialog.Close;
    }
}

var iframeOnload = function () { }
//上传文件
function uploadFile() {
    iframeOnload = function () { }
    dialog.Content = "<iframe id='uploadFrm' frameborder='no' border='0' scrolling='no' allowtransparency='yes' onload='iframeOnload()' name='uploadFrm' style='width:0px; height:0px; display:none;'></iframe><form name='actionForm' id='actionForm' action='" + defaultURL + "?action=UPLOAD&value1=" + currentNode.path + "' method='POST' target='uploadFrm' enctype='multipart/form-data'><input name='selectFile' width='150' type='file' /></form><div id='uploadStatus' style='display:none;'><img src='images/process.gif' /><div style='color:#ccc;'>正在上传，如果长时间不响应，可能是上传文件太大导致出错！</div></div>";
    dialog.Text = "上传文件";
    dialog.Show();
    alert("xxx");
    dialog.OK = function () {
        iframeOnload = function () {
            dialog.Text = "提示";
            dialog.Content = "文件上传成功！";
            dialog.OK = dialog.Close;
            dialog.Show(1);
            currentNode.Refersh();
        }

        $("actionForm").submit();
        $("actionForm").style.display = "none";
        $("uploadStatus").style.display = "";
    }
}
function downLoad()
{
   downloadsFiles();
}
    //下载多个文件 
function downloadsFiles() {
    getSelectedFile();
    dialog.Text="下载";
    if(list.length<1)
    {
    dialog.Content="请选择要下载的文件";
    dialog.Show();
    dialog.OK=dialog.Close;
    return;
    }
    window.onbeforeunload=function(){}
    window.location.href=defaultURL + "?action=DOWNLOADS&value1=" + list.join("|");
    window.onbeforeunload=function(){
    return "cuole ";
    };
}

//重命名文件
renameFile = function (fName) {
    dialog.Text = "重命名：" + fName;
    dialog.Content = "请输入新名称：<input id='newName' type='textbox' style='width:120px;' value='" + fName + "' />";
    dialog.Show();

    var txtNewName = $("newName");
    txtNewName.value = fName;
    txtNewName.focus();

    dialog.OK = function () {
        if (!(/^[\w\u4e00-\u9fa5-\.]+[a-zA-Z0-9.]*$/.test(txtNewName.value))) {
            dialog.Content = "<span style='color:red;'>输入的新名称不正确！</span>";

            dialog.OK = function () {
                renameFile(fName);
            }

            dialog.Show(1);
            return;
        }

        var url = defaultURL + "?action=RENAME&value1=" + encodeURIComponent(currentNode.path + fName) + "&value2=" + encodeURIComponent(currentNode.path + txtNewName.value);
        var result = executeHttpRequest("GET", url, null);

        if (result == "OK") {
            currentNode.Refersh();

            dialog.Content = "重命名成功！";

            dialog.OK = dialog.Close;

            dialog.Show(1);
        }
        else {
            dialog.Content = "<span style='color:red;'>重命名失败，请重试！</span>";

            dialog.OK = function () {
                renameFile(fName);
            }

            dialog.Show();
        }
    }
}
function zipFile() {
    getSelectedFile();

    if (list.length < 1) {
        dialog.Text = "提示";
        dialog.Content = "请选择要压缩的文件和文件夹！";
        dialog.Show(1);
        dialog.OK = dialog.Close;
        return;
    }

    dialog.Text = "压缩";
    dialog.Content = "<span>请输入压缩文件名：</span><input id='inputFile' type='textbox' style='width:100px;' />";
    dialog.Show();

    dialog.OK = function () {
        if (checkFileName()) {
            var url = defaultURL + "?action=ZIP&value1=" + encodeURIComponent(currentNode.path + $("inputFile").value) + "&value2=" + encodeURIComponent(list.join("|"));
            var result = executeHttpRequest("GET", url, null);

            dialog.Text = "提示";

            if (result == "OK") {
                dialog.Content = "压缩成功！";
                currentNode.Refersh();
            }
            else {
                dialog.Content = "压缩失败！";
            }

            dialog.OK = dialog.Close;
            dialog.Show(1);
        }
        else {
            dialog.Content = "输入的文件名不正确！";
            dialog.Text = "提示";
            dialog.Show(1);

            dialog.OK = function () {
                dialog.Close();
                zipFile();
            }
        }
    }
}

function unZipFile() {
    getSelectedFile();

    if (list.length < 1) {
        dialog.Text = "提示";
        dialog.Content = "请选择要解压的文件！";
        dialog.Show(1);

        dialog.OK = dialog.Close;

        return;
    }

    var url = defaultURL + "?action=UNZIP&value1=" + encodeURIComponent(currentNode.path) + "&value2=" + encodeURIComponent(list.join("|"));
    var result = executeHttpRequest("GET", url, null);

    dialog.Text = "提示";

    if (result == "OK") {
        dialog.Content = "解压成功！";
        currentNode.Refersh();
    }
    else {
        dialog.Content = "解压失败！";
    }

    dialog.OK = dialog.Close;
    dialog.Show(1);
}

$("tree").style.height = document.documentElement.clientHeight + "px";

if (document.compatMode != 'CSS1Compat') {
    $("tree").style.height = document.body.clientHeight + "px";
}
//menuitem 鼠标移上变色        
var menuItems = $("menu").getElementsByTagName("div");

for (var i = 0; i < menuItems.length; i++) {
    menuItems[i].onmouseover = function () {
        this.style.color = "red";
        this.style.backgroundColor = "#eef";
    }

    menuItems[i].onmouseout = function () {
        this.style.color = "";
        this.style.backgroundColor = "transparent";
    }
}
