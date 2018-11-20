<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductScreeningX.aspx.cs" Inherits="ReportProduce_ProductScreeningX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>科技服务预报监控</title>
    <link href="../Ext/resources/css/ext-all.css?v=20160429" rel="stylesheet" type="text/css" />
    <script src="../Ext/adapter/ext/ext-base.js" type="text/javascript"></script>
    <script src="../Ext/ext-all.js" type="text/javascript"></script>
    <script src="../JS/Utility.js" type="text/javascript"></script>
    <script src="../JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../JS/highlight-active-input.js" type="text/javascript"></script>
    <script src="js/ProductScreeningX.js?V=20160605" type="text/javascript"></script>
    <link href="../css/ProductScreening.css?V=201602170842" rel="stylesheet" type="text/css" />
<link href="../css/Title.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="totalContent">
      <div class="outLine" id="outLine">
         <div class="shArea">
           <div class="mapTitle">
           <div class="titlePoint"></div>
           <span>科技服务预报监控</span>
           </div>           
           <div class="displayItem" id="KJFW">
           <div class="singleTitle">科技服务预报</div>
             <div class="innerContent">
             <div class="singleConDis">
                 <div class="condition">待完成</div>
                 <div class="pubTime">17时预报</div>
               </div>
             </div>
           </div>
         </div>
    </div>
    </div>
    </form>
</body>
</html>
