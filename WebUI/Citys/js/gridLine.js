/**
 * @license Highcharts JS v5.0.0 (2016-09-29)
 *
 * (c) 2009-2016 Torstein Honsi
 *
 * License: www.highcharts.com/license
 */
(function(factory) {
    if (typeof module === 'object' && module.exports) {
        module.exports = factory;
    } else {
        factory(Highcharts);
    }
}(function(Highcharts) {
    (function(Highcharts) {
        /**
         * (c) 2010-2016 Torstein Honsi
         *
         * License: www.highcharts.com/license
         * 
         * Grid-light theme for Highcharts JS
         * @author Torstein Honsi
         */

        'use strict';
        /* global document */
        // Load the fonts
        Highcharts.createElement('link', {
            href: 'https://fonts.googleapis.com/css?family=Dosis:400,600',
            rel: 'stylesheet',
            type: 'text/css'
        }, null, document.getElementsByTagName('head')[0]);

        Highcharts.theme = {
            colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066', '#eeaaee',
                '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'
            ],
            chart: {
                backgroundColor: null,
                style: {
                    fontFamily: 'Dosis, sans-serif',
                }
            },
            title: {
                style: {
                    fontSize: '16px',
                    fontWeight: 'bold',
                    textTransform: 'uppercase'
                }
            },
            tooltip: {
                borderWidth: 0,
                backgroundColor: 'rgba(219,219,216,0.8)',
                shadow: false
            },
            legend: {
                itemStyle: {
                    fontWeight: 'bold',
                    fontSize: '13px'
                }
            },
            xAxis: {
                gridLineWidth: 1,
                labels: {
                    style: {
                        fontSize: '15px',
                        top: '10px'
                    }
                }
            },
            yAxis: {
                minorTickInterval: 'auto',
                title: {
                    style: {
                        textTransform: 'uppercase'
                    }
                },
                labels: {
                    style: {
                        fontSize: '16px'
                    }
                }
            },
            plotOptions: {
                candlestick: {
                    lineColor: '#404048'
                }
            },


            // General
            background2: '#F0F0EA'

        };

        // Apply the theme
        Highcharts.setOptions(Highcharts.theme);

    }(Highcharts));
}));