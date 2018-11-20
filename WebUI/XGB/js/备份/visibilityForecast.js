/// <reference path="../../EvaluateHtml/JS/jquery-1.9.1.js" />
/// <reference path="../../AQI/js/Leaflet/leaflet-src.js" />
var map;

$(function () {
    creatMapCommon();
    querySiteData();
})

function creatMapCommon() {
    map = L.map('map', {
        center: [31.187649, 121.451256],
        zoom: 10,
        maxZoom: 19,//原版为11
        minZoom: 4
        //    crs:m_crs
    });

    L.tileLayer("http://mt1.google.cn/vt/lyrs=m&hl=zh-CN&gl=cn&x={x}&y={y}&z={z}&s=", {
        attribution: ''
    }).addTo(map);

    //map.on('moveend', function (e) {
    //    mapmove(e);
    //});
}
//选时间，点击“查询” 请求站点数据，进行地图撒点 
//不选时间，直接查询站点表，加载站点数据，进行撒点
function querySiteData() {
    $.ajax({
        url: "visibilityForecast.aspx/QuerySiteData",
        type: "POST",
        contentType: "application/json",
        dataType: 'json',
        success: function (response) {
            console.log(response.d.rows);
            var data = response.d.rows;
            var incidents = data;

            //重新格式化数据
            function reformat(array) {
                var data = [];
                array.map(function (d,i) {
                    data.push({
                        id: i,
                        type: "Feature",
                        geometry: {
                            coordinates: [+d.lon, +d.lat],
                            type: "Point"
                        },
                        "properties": {
                            "Code": d.siteid,
                            "name": d.sitename,
                            "Lon": d.lon,
                            "Lat":d.lat
                        }
                    });
                });

                return data;
            }

            var geoData = { type: "FeatureCollection", features: reformat(incidents) };

            siteLayer = L.geoJSON(geoData, {
                pointToLayer: function (feature, latlng) {
                    //返回的Marker添加到地图上，同时用canvas绘制每个Marker的样式
                    return L.marker(L.latLng(latlng),//Marker的坐标
                        //Marker的配置项，下面只配置了icon，并且icon的值还是采用canvas画的
                        {
                            icon: L.canvasIcon({
                                iconSize: [40, 40],
                                iconAnchor: [5, 5],

                                drawIcon: function (icon, type) {// 通过canvas画图标
                                    if (type == 'icon') {
                                        var ctx = icon.getContext('2d');
                                        var size = L.point(this.options.iconSize);
                                        w = 20;
                                        h = 20;
                                        ctx.translate(w / 2, h / 2);//将Marker点平移到矩形框中
                                        ctx.beginPath();
                                        ctx.fillStyle = "blue";//为每类的降雨点填充不同的颜色，调用getRainColor()方法
                                        ctx.arc(0, 0, w / 2 - 1, 0, Math.PI * 2, true);

                                        ctx.fill();

                                        ctx.fillStyle = "#000000";//Marker点旁边的标注文本的颜色
                                        ctx.textBaseline = "middle"; //竖直对齐
                                        ctx.textAlign = "center"; //水平对齐　
                                        //ctx.fillText(text(feature), 20, 0);//填充标注文本的内容，调用text()方法
                                        ctx.closePath();
                                    }
                                }
                            })
                        }

                  );
                }
            }).addTo(map);
        },
        error: function (e) {
            console.log(e);
        }
        
    });

}



function mapmove(e) {
    var mapBounds = leafletMap.getBounds();
    var subset = search(qtree, mapBounds.getWest(), mapBounds.getSouth(), mapBounds.getEast(), mapBounds.getNorth());
    console.log("subset: " + subset.length);

    //mapIsMoving = false;
    redrawSubset(subset);

}
function redrawSubset(subset) {
    var scale = getZoomScale();
    var pointRadius = 8 * scale * 100;
    pointRadius = pointRadius < 4 ? 4 : pointRadius > 8 ? 8 : pointRadius;
    path.pointRadius(pointRadius);

    var bounds = path.bounds({ type: "FeatureCollection", features: subset });
    var topLeft = bounds[0];
    var bottomRight = bounds[1];
    //        console.log(bounds)
    //          x-max          x-min
    var width = bounds[1][0] - bounds[0][0];
    //           y-max          y-min
    var height = bounds[1][1] - bounds[0][1];

    //var dx = bounds[1][0] - bounds[0][0], 
    //    dy = bounds[1][1] - bounds[0][1],
    //    x = (bounds[0][0] + bounds[1][0]) / 2,
    //    y = (bounds[0][1] + bounds[1][1]) / 2,
    //    scale = .9 / Math.max(dx / width, dy / height),
    //    translate = [width / 2 - scale * x, height / 2 - scale * y];

    var left = topLeft[0] - 20;
    var top = topLeft[1] - 20;
    var right = bottomRight[0] - 20;
    var bottom = bottomRight[1] - 20;

    svg.attr("width", right - left + 40)
        .attr("height", bottom - top + 40)
        .style("left", left + "px")
        .style("top", top + "px");


    g.attr("transform", "translate(" + -left + "," + -top + ")");

    var start = new Date();


    var points = g.selectAll("path")
                    .data(subset, function (d) {
                        return d.id;
                    });
    points.enter().append("path");
    points.exit().remove();
    points.attr("d", path);

    points.style("fill-opacity", function (d) {
        if (d.group) {
            return (d.group * 0.1) + 0.2;
        }
    }).attr("stroke", function (d) {
        if (d.properties.Name == selectedPoint.name) {
            return "#F60";
        }
        else {
            return "";
        }
    })


    console.log("updated at  " + new Date().setTime(new Date().getTime() - start.getTime()) + " ms ");

    prevWindowPoints = points;
    points.on("click", function (d) {
        //if (!mapIsMoving) {
        pointclick(d);
        //当前点击
        selectedPoint.name = d.properties.Name;
        //历史点击
        d.mAttr.stroke = "#F60";
        d.mAttr.strokeWidth = "2";
        //pointclick -> setView -> redrawSubset
        //            d3.select(this)
        //                .attr("stroke", "red")
        //                .attr("stroke-width", "2")
        //}
    })
    .on("mouseover", function (d, i) {
        d3.select(this)
        .append("title")
        .text(d.properties.Name);

    })
    .on("mouseout", function (d, i) {
        d3.select(this)
        .select("title")
        .remove();
    })

}