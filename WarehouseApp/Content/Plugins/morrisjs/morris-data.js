
var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

function lineChart(url,paramData) {


    $.ajax({
        url: url,
        type: 'POST',
        data: JSON.stringify({ deptId: paramData }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
           
            Morris.Line({
                element: 'morris-line-chart',
                // colors: ["#3c8dbc", "#f56954", "#00a65a"],
                data: data.arrayList,
                xkey: data.xkey,
                ykeys: data.ykeys,
                labels: data.labels,
                //ykeys: ['aquarium', 'game', 'mirror', 'zone3d'],
                //labels: ['Aquarium', 'Gamming Zone', 'Mirror Tunnel', '3D Zone'],
                resize: true,
                xLabelAngle: 70,
                xLabelFormat: function (x) { // <--- x.getMonth() returns valid index
                    var month = months[x.getMonth()];
                    // var year = new Date(x).getFullYear();
                     var date = new Date(x).getDate();
                     return date+ "-" + month;
                },
                dateFormat: function (x) {
                    var month = months[new Date(x).getMonth()];
                    var date = new Date(x).getDate();
                    var year = new Date(x).getFullYear();
                    return date + "-" + month + "-" + year;
                },
            });

        },
        
    });
}

function barChart(filter, paramData) {
    var barUrl = '/Home/FindMonthlyAnalytics';

    if (paramData == 6) {
        if (filter == "yearFilter") {
            barUrl = '/Home/FindYearlyAnalytics';
        } 
    }
    else{
    if (filter == "yearFilter") {
        barUrl = '/Home/FindAllTicketYearly';
        
    } else {
        barUrl = '/Home/FindAllTicketMonthly';
    }
    }

    $('#morris-bar-chart').empty();
    $.ajax({
        url: barUrl,
        type: 'POST',
        data: JSON.stringify({ deptId: paramData }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            Morris.Bar({
                element: 'morris-bar-chart',
               // colors: ["#33414e", "#1caf9a"],
                data: data.arrayList,
                xkey: data.xkey,
                ykeys: data.ykeys,
                labels: data.labels,
                hideHover: 'auto',
                resize: true,
                xLabelAngle: 70,
              
                //xLabelFormat: labelFormate,
                //dateFormat: dateFormate,
            });

        },
        
    });
}

function donut(paramData) {
    
    $('#morris-donut-chart').empty();
    $.ajax({
        url: '/Home/FindAllGameAnalytic',
        type: 'POST',
        data: JSON.stringify({ filter: paramData }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            Morris.Donut({
                element: 'morris-donut-chart',
               //colors: ["#3c8dbc", "#f56954", "#00a65a"],
                data: data.games,
                resize: true
            });
            renderDonutFooter(paramData, data.totalTickets);
        },
        
    });
}

function renderDonutFooter(paramData, totalTickets) {
    var date = new Date();
    if (paramData == "today") {
        $('#filter-donut-chart').text("Date: " + date.getDate() + "-" + months[date.getMonth()] + "-" + date.getFullYear() + ", Total: " + totalTickets);
    }
    if (paramData == "currentMonth") {
        $('#filter-donut-chart').text("Month: " + months[date.getMonth()] + "-" + date.getFullYear() + ", Total: " + totalTickets);
    }
    if (paramData == "currentYear") {
        $('#filter-donut-chart').text("Year: " + date.getFullYear() + ", Total: " + totalTickets);
    }

}