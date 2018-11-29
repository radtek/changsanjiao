<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LifeIndex.aspx.cs" Inherits="LifeIndex_LifeIndex" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=0" />
    <title>【健康气象】今日生活指数</title>
    <link href="css/LifeIndex.css" rel="stylesheet" />
    <script src="js/LifeIndex.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>


    <div class="container">
  <!-- topbar -->
  <div class="topbar font-md">
    上海<span class="pull-right">2018年11月29日发布</span>
  </div>

  <!-- content -->
    <div class="row">
      <div class="col-xs-12 col-md-6">
        <div class="life-content">
          <div class="outline">
            <img src="img/icon/zszs.png" /><span class="font-lg">中暑指数</span>
          </div>

            <div class="item level-4-1">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">上午</div>
                <div class="col-xs-10">不易中暑</div>
              </div>
              <hr class="divider">
            </div>
            <div class="item level-4-1">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">下午</div>
                <div class="col-xs-10">不易中暑</div>
              </div>
              <hr class="divider">
            </div>
          <div class="level-hint">
            <div class="row font-xs">
                <span class="state-4-1">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>不易中暑&nbsp;&nbsp;</span>
                <span class="state-4-2">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>较易中暑&nbsp;&nbsp;</span>
                <span class="state-4-3">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>容易中暑&nbsp;&nbsp;</span>
                <span class="state-4-4">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>极易中暑&nbsp;&nbsp;</span>
            </div>
          </div>
        </div>
      </div>

    </div>
    <div class="row">
      <div class="col-xs-12 col-md-6">
        <div class="life-content">
          <div class="outline">
            <img src="img/icon/swhd.png" /><span class="font-lg">室外活动</span>
          </div>

            <div class="item level-4-3">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">早晨</div>
                <div class="col-xs-10">空气质量较差，不太适宜户外晨练</div>
              </div>
              <hr class="divider">
            </div>
            <div class="item level-4-3">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">上午</div>
                <div class="col-xs-10">空气质量较差，不太适宜户外体育活动</div>
              </div>
              <hr class="divider">
            </div>
            <div class="item level-4-3">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">下午</div>
                <div class="col-xs-10">空气质量较差，不太适宜户外体育活动</div>
              </div>
              <hr class="divider">
            </div>
            <div class="item level-4-2">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">晚上</div>
                <div class="col-xs-10">风不大，适宜户外晚间锻炼</div>
              </div>
              <hr class="divider">
            </div>
          <div class="level-hint">
            <div class="row font-xs">
                <span class="state-4-1">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>非常适宜&nbsp;&nbsp;</span>
                <span class="state-4-2">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>适宜&nbsp;&nbsp;</span>
                <span class="state-4-3">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>不太适宜&nbsp;&nbsp;</span>
                <span class="state-4-4">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>不适宜&nbsp;&nbsp;</span>
            </div>
          </div>
        </div>
      </div>

    </div>
    <div class="row">
      <div class="col-xs-12 col-md-6">
        <div class="life-content">
          <div class="outline">
              <img src="img/icon/cyzs.png" /><span class="font-lg">穿衣指数</span>
          </div>

            <div class="item level-4-3">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">早晨</div>
                <div class="col-xs-10">气温略低，适宜穿夹克类服装</div>
              </div>
              <hr class="divider">
            </div>
            <div class="item level-4-3">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">中午</div>
                <div class="col-xs-10">气温适宜，适宜穿夹克类服装</div>
              </div>
              <hr class="divider">
            </div>
            <div class="item level-4-3">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">晚上</div>
                <div class="col-xs-10">气温略低，适宜穿夹克类服装</div>
              </div>
              <hr class="divider">
            </div>
          <div class="level-hint">
            <div class="row font-xs">
                <span class="state-4-1">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>适宜穿薄短袖类&nbsp;&nbsp;</span>
                <span class="state-4-2">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>适宜穿单衣类&nbsp;&nbsp;</span>
                <span class="state-4-3">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>适宜穿夹克类&nbsp;&nbsp;</span>
                <span class="state-4-4">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>适宜穿棉衣和羽绒类&nbsp;&nbsp;</span>
            </div>
          </div>
        </div>
      </div>

    </div>
    <div class="row">
      <div class="col-xs-12 col-md-6">
        <div class="life-content">
          <div class="outline">
              <img src="img/tiganzhishu.png" /><span class="font-lg">体感指数</span>
          </div>

            <div class="item level-4-2">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">上午</div>
                <div class="col-xs-10">气温略低，人体感觉较舒适</div>
              </div>
              <hr class="divider">
            </div>
            <div class="item level-4-2">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">下午</div>
                <div class="col-xs-10">气温适宜，人体感觉较舒适</div>
              </div>
              <hr class="divider">
            </div>
            <div class="item level-4-2">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">晚上</div>
                <div class="col-xs-10">气温略低，人体感觉较舒适</div>
              </div>
              <hr class="divider">
            </div>
          <div class="level-hint">
            <div class="row font-xs">
                <span class="state-4-1">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>适宜&nbsp;&nbsp;</span>
                <span class="state-4-2">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>较适宜&nbsp;&nbsp;</span>
                <span class="state-4-3">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>较凉或较热&nbsp;&nbsp;</span>
                <span class="state-4-4">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>冷或热&nbsp;&nbsp;</span>
            </div>
          </div>
        </div>
      </div>

    </div>
    <div class="row">
      <div class="col-xs-12 col-md-6">
        <div class="life-content">
          <div class="outline">
              <img src="img/icon/xszs.png" /><span class="font-lg">洗晒指数</span>
          </div>

            <div class="item level-4-2">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">上午</div>
                <div class="col-xs-10">云量较多，光照较好，适宜洗晒</div>
              </div>
              <hr class="divider">
            </div>
            <div class="item level-4-2">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">下午</div>
                <div class="col-xs-10">云量较多，光照较好，适宜洗晒</div>
              </div>
              <hr class="divider">
            </div>
          <div class="level-hint">
            <div class="row font-xs">
                <span class="state-4-1">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>非常适宜&nbsp;&nbsp;</span>
                <span class="state-4-2">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>适宜&nbsp;&nbsp;</span>
                <span class="state-4-3">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>不太适宜&nbsp;&nbsp;</span>
                <span class="state-4-4">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>不适宜&nbsp;&nbsp;</span>
            </div>
          </div>
        </div>
      </div>

    </div>
    <div class="row">
      <div class="col-xs-12 col-md-6">
        <div class="life-content">
          <div class="outline">
              \<img src="img/icon/ktkq.png" /><span class="font-lg">空调开启</span>
          </div>

            <div class="item level-4-1">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">上午</div>
                <div class="col-xs-10">气温略低，建议不开启</div>
              </div>
              <hr class="divider">
            </div>
            <div class="item level-4-1">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">下午</div>
                <div class="col-xs-10">气温适宜，建议不开启</div>
              </div>
              <hr class="divider">
            </div>
            <div class="item level-4-1">
              <div class="row font-md item-row">
                <div class="col-xs-2 text-center">夜间</div>
                <div class="col-xs-10">气温略低，建议不开启</div>
              </div>
              <hr class="divider">
            </div>
          <div class="level-hint">
            <div class="row font-xs">
                <span class="state-4-1">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>建议不开启&nbsp;&nbsp;</span>
                <span class="state-4-2">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>建议短时开启&nbsp;&nbsp;</span>
                <span class="state-4-3">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>建议适时开启&nbsp;&nbsp;</span>
                <span class="state-4-4">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                <span>建议开启&nbsp;&nbsp;</span>
            </div>
          </div>
        </div>
      </div>

    </div>
</div>


</body>
</html>
