<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebUserControl.ascx.cs" Inherits="Citys_WebUserControl" %>

  <script type="text/javascript" src="js/jquery.citypicker.js"></script>
  <link rel="stylesheet" type="text/css" href="css/jquery.citypicker.css?v=11"/>

     <input id = "1" class ="cp" type="text" value="" style=" display:none"/>

<div id="citypicker_container" style=" display: block; top:-350px;"  >
<div id="citypicker_caption" ><h1>选择城市</h1><span style=" " id="clos" title="展开/折叠"></span></div>
<div id="citypicker_pro" class="pro"><label class="pro1">热门城市</label><label class="pro1">直辖市</label><label class="pro1">河北</label><label class="pro1">山西</label><label class="pro1">辽宁</label><label class="pro1">吉林</label><label class="pro1">江苏</label><label class="pro1">浙江</label><label class="pro1">安徽</label><label class="pro1">福建</label><label class="pro1">江西</label><label class="pro1">山东</label><label class="pro1">河南</label><label class="pro1">湖北</label><label class="pro1">湖南</label><label class="pro1">广东</label><label class="pro1">广西</label><label class="pro1">海南</label><label class="pro1">四川</label><label class="pro1">贵州</label><label class="pro1">云南</label><label class="pro1">陕西</label><label class="pro1">甘肃</label><label class="pro1">青海</label><label class="pro1">宁夏</label><label class="pro1">新疆</label><label class="pro1">内蒙</label><label class="pro1">黑龙江</label></div>
<div id="citypicker_city" class="cityy">
<label id="city0" class="pro2" style="color:blue;" onclick="clk(this)">上海</label><label id="city1" class="pro2" onclick="clk(this)">苏州</label><label id="city2" class="pro2" onclick="clk(this)">杭州</label><label id="city3" class="pro2" onclick="clk(this)">南京</label><label id="city4" class="pro2" onclick="clk(this)">合肥</label><label id="city5" class="pro2" onclick="clk(this)">济南</label>
</div></div>
