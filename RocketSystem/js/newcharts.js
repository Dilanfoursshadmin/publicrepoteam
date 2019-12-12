window.onload = function () {

    var chart = new CanvasJS.Chart("chartContainer", {
        animationEnabled: true,
        title: {
            text: "Sales Statistics"
        },
        axisX: {
            valueFormatString: "DDD"
        },
        axisY: {
            prefix: "$"
        },
        toolTip: {
            shared: true
        },
        legend: {
            cursor: "pointer",
            itemclick: toggleDataSeries
        },
        data: [{
            type: "stackedBar",
            name: "Meals",
            showInLegend: "true",
            xValueFormatString: "DD, MMM",
            yValueFormatString: "$#,##0",
            dataPoints: [
                { x: new Date(2017, 0, 30), y: 56 },
                { x: new Date(2017, 0, 31), y: 45 },
                { x: new Date(2017, 1, 1), y: 71 },
                { x: new Date(2017, 1, 2), y: 41 },
                { x: new Date(2017, 1, 3), y: 60 },
                { x: new Date(2017, 1, 4), y: 75 },
                { x: new Date(2017, 1, 5), y: 98 }
            ]
        },
        {
            type: "stackedBar",
            name: "Snacks",
            showInLegend: "true",
            xValueFormatString: "DD, MMM",
            yValueFormatString: "$#,##0",
            dataPoints: [
                { x: new Date(2017, 0, 30), y: 86 },
                { x: new Date(2017, 0, 31), y: 95 },
                { x: new Date(2017, 1, 1), y: 71 },
                { x: new Date(2017, 1, 2), y: 58 },
                { x: new Date(2017, 1, 3), y: 60 },
                { x: new Date(2017, 1, 4), y: 65 },
                { x: new Date(2017, 1, 5), y: 89 }
            ]
        },
        {
            type: "stackedBar",
            name: "Drinks",
            showInLegend: "true",
            xValueFormatString: "DD, MMM",
            yValueFormatString: "$#,##0",
            dataPoints: [
                { x: new Date(2017, 0, 30), y: 48 },
                { x: new Date(2017, 0, 31), y: 45 },
                { x: new Date(2017, 1, 1), y: 41 },
                { x: new Date(2017, 1, 2), y: 55 },
                { x: new Date(2017, 1, 3), y: 80 },
                { x: new Date(2017, 1, 4), y: 85 },
                { x: new Date(2017, 1, 5), y: 83 }
            ]
        },
        {
            type: "stackedBar",
            name: "Dessert",
            showInLegend: "true",
            xValueFormatString: "DD, MMM",
            yValueFormatString: "$#,##0",
            dataPoints: [
                { x: new Date(2017, 0, 30), y: 61 },
                { x: new Date(2017, 0, 31), y: 55 },
                { x: new Date(2017, 1, 1), y: 61 },
                { x: new Date(2017, 1, 2), y: 75 },
                { x: new Date(2017, 1, 3), y: 80 },
                { x: new Date(2017, 1, 4), y: 85 },
                { x: new Date(2017, 1, 5), y: 105 }
            ]
        },
        {
            type: "stackedBar",
            name: "Takeaway",
            showInLegend: "true",
            xValueFormatString: "DD, MMM",
            yValueFormatString: "$#,##0",
            dataPoints: [
                { x: new Date(2017, 0, 30), y: 52 },
                { x: new Date(2017, 0, 31), y: 55 },
                { x: new Date(2017, 1, 1), y: 20 },
                { x: new Date(2017, 1, 2), y: 35 },
                { x: new Date(2017, 1, 3), y: 30 },
                { x: new Date(2017, 1, 4), y: 45 },
                { x: new Date(2017, 1, 5), y: 25 }
            ]
        }]
    });
    chart.render();

    function toggleDataSeries(e) {
        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
            e.dataSeries.visible = false;
        }
        else {
            e.dataSeries.visible = true;
        }
        chart.render();
    }

//Second chart
    
    var chart = new CanvasJS.Chart("chartContainernew", {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: "Monthly Sales Data"
        },
        axisX: {
            valueFormatString: "MMM"
        },
        axisY: {
            prefix: "$",
            labelFormatter: addSymbols
        },
        toolTip: {
            shared: true
        },
        legend: {
            cursor: "pointer",
            itemclick: toggleDataSeries
        },
        data: [
            {
                type: "column",
                name: "Actual Sales",
                showInLegend: true,
                xValueFormatString: "MMMM YYYY",
                yValueFormatString: "$#,##0",
                dataPoints: [
                    { x: new Date(2016, 0), y: 20000 },
                    { x: new Date(2016, 1), y: 30000 },
                    { x: new Date(2016, 2), y: 25000 },
                    { x: new Date(2016, 3), y: 70000, indexLabel: "High Renewals" },
                    { x: new Date(2016, 4), y: 50000 },
                    { x: new Date(2016, 5), y: 35000 },
                    { x: new Date(2016, 6), y: 30000 },
                    { x: new Date(2016, 7), y: 43000 },
                    { x: new Date(2016, 8), y: 35000 },
                    { x: new Date(2016, 9), y: 30000 },
                    { x: new Date(2016, 10), y: 40000 },
                    { x: new Date(2016, 11), y: 50000 }
                ]
            },
            {
                type: "line",
                name: "Expected Sales",
                showInLegend: true,
                yValueFormatString: "$#,##0",
                dataPoints: [
                    { x: new Date(2016, 0), y: 40000 },
                    { x: new Date(2016, 1), y: 42000 },
                    { x: new Date(2016, 2), y: 45000 },
                    { x: new Date(2016, 3), y: 45000 },
                    { x: new Date(2016, 4), y: 47000 },
                    { x: new Date(2016, 5), y: 43000 },
                    { x: new Date(2016, 6), y: 42000 },
                    { x: new Date(2016, 7), y: 43000 },
                    { x: new Date(2016, 8), y: 41000 },
                    { x: new Date(2016, 9), y: 45000 },
                    { x: new Date(2016, 10), y: 42000 },
                    { x: new Date(2016, 11), y: 50000 }
                ]
            },
            {
                type: "area",
                name: "Profit",
                markerBorderColor: "white",
                markerBorderThickness: 2,
                showInLegend: true,
                yValueFormatString: "$#,##0",
                dataPoints: [
                    { x: new Date(2016, 0), y: 5000 },
                    { x: new Date(2016, 1), y: 7000 },
                    { x: new Date(2016, 2), y: 6000 },
                    { x: new Date(2016, 3), y: 30000 },
                    { x: new Date(2016, 4), y: 20000 },
                    { x: new Date(2016, 5), y: 15000 },
                    { x: new Date(2016, 6), y: 13000 },
                    { x: new Date(2016, 7), y: 20000 },
                    { x: new Date(2016, 8), y: 15000 },
                    { x: new Date(2016, 9), y: 10000 },
                    { x: new Date(2016, 10), y: 19000 },
                    { x: new Date(2016, 11), y: 22000 }
                ]
            }]
    });
    chart.render();

    function addSymbols(e) {
        var suffixes = ["", "K", "M", "B"];
        var order = Math.max(Math.floor(Math.log(e.value) / Math.log(1000)), 0);

        if (order > suffixes.length - 1)
            order = suffixes.length - 1;

        var suffix = suffixes[order];
        return CanvasJS.formatNumber(e.value / Math.pow(1000, order)) + suffix;
    }

    function toggleDataSeries(e) {
        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
            e.dataSeries.visible = false;
        } else {
            e.dataSeries.visible = true;
        }
        e.chart.render();
    }

    //third chart

    var chart = new CanvasJS.Chart("chartContainer3", {
        animationEnabled: true,
        title: {
            text: "Position Date Wise"
        },
        axisX: {
            valueFormatString: "DDD",
            minimum: new Date(2017, 1, 5, 23),
            maximum: new Date(2017, 1, 12, 1)
        },
        axisY: {
            title: "Number of Position"
        },
        legend: {
            verticalAlign: "top",
            horizontalAlign: "right",
            dockInsidePlotArea: true
        },
        toolTip: {
            shared: true
        },
        data: [{
            name: "Top Package",
            showInLegend: true,
            legendMarkerType: "square",
            type: "area",
            color: "rgba(40,175,101,0.6)",
            markerSize: 0,
            dataPoints: [
                { x: new Date(2017, 1, 6), y: 220 },
                { x: new Date(2017, 1, 7), y: 120 },
                { x: new Date(2017, 1, 8), y: 144 },
                { x: new Date(2017, 1, 9), y: 162 },
                { x: new Date(2017, 1, 10), y: 129 },
                { x: new Date(2017, 1, 11), y: 109 },
                { x: new Date(2017, 1, 12), y: 129 }
            ]
        },

            {
                name: "Middle Package",
                showInLegend: true,
                legendMarkerType: "square",
                type: "area",
                color: "rgba(38,13,162,0.6)",
                markerSize: 0,
                dataPoints: [
                    { x: new Date(2017, 1, 6), y: 45 },
                    { x: new Date(2017, 1, 7), y: 85 },
                    { x: new Date(2017, 1, 8), y: 170 },
                    { x: new Date(2017, 1, 9), y: 32 },
                    { x: new Date(2017, 1, 10), y: 160 },
                    { x: new Date(2017, 1, 11), y: 200 },
                    { x: new Date(2017, 1, 12), y: 90 }
                ]
            },



        {
            name: "Small Package",
            showInLegend: true,
            legendMarkerType: "square",
            type: "area",
            color: "rgba(0,75,141,0.7)",
            markerSize: 0,
            dataPoints: [
                { x: new Date(2017, 1, 6), y: 42 },
                { x: new Date(2017, 1, 7), y: 34 },
                { x: new Date(2017, 1, 8), y: 29 },
                { x: new Date(2017, 1, 9), y: 42 },
                { x: new Date(2017, 1, 10), y: 53 },
                { x: new Date(2017, 1, 11), y: 15 },
                { x: new Date(2017, 1, 12), y: 12 }
            ]
        }]
    });
    chart.render();

}


