// JScript 文件

var nd = new TreeNode(); //创建树对象
nd.container = $("tree"); //找到容器
nd.text = "在线文档管理";
nd.Show();
currentNode = nd;
currentNode.Refersh();

var dialog=new Dialog();
dialog.ImgZIndex = 110;
dialog.DialogZIndex = 111;
Ext.onReady(function(){
    supportInnerText();//使得火狐支持innerText
   }
)
var iframeOnload = function () { };
//上传文件
function uploadFile()
{
 
 iframeOnload=function(){};
 dialog.Content = "<iframe id='uploadFrm' frameborder='no' border='0' scrolling='no' allowtransparency='yes' onload='iframeOnload()' name='uploadFrm' style='width:0px; height:0px; display:none;'></iframe><form name='actionForm' id='actionForm' action='" + defaultURL + "?action=UPLOAD&value1=" +currentNode.path + "' method='POST' target='uploadFrm' enctype='multipart/form-data'><input name='selectFile' width='150' type='file' /></form><div id='uploadStatus' style='display:none;'><img src='images/process.gif' /><div style='color:#ccc;'>正在上传，如果长时间不响应，可能是上传文件太大导致出错！</div></div>";
 dialog.Text = "上传文件";
 dialog.Show();
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
}