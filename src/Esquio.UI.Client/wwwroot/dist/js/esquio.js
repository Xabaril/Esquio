(function () {
    if (!window.beautyJson) {
        window.beautyJson = function (id, json) {
            const identifier = `#${id}`;
            $(identifier).empty();
            $(identifier).jsonView(json, { collapsed: true });
        };
    }
    if (!window.copyToClipboard) {
        window.copyToClipboard = function (text) {
            if (!navigator.clipboard) {
                var textArea = document.createElement("textarea");
                textArea.value = text;

                // Avoid scrolling to bottom
                textArea.style.top = "0";
                textArea.style.left = "0";
                textArea.style.position = "fixed";

                document.body.appendChild(textArea);
                textArea.focus();
                textArea.select();

                try {
                    document.execCommand('copy');
                } catch (err) {
                    console.error('Fallback: Oops, unable to copy', err);
                }

                document.body.removeChild(textArea);
            }
            else {
                navigator.clipboard.writeText(text).then(function () {
                    console.log('Async: Copying to clipboard was successful!');
                }, function (err) {
                    console.error('Async: Could not copy text: ', err);
                });
            }
        };
    }

    if (!window.drawSuccessChart) {
        window.drawSuccessChart = function (id, sucessPercentage) {
            var pieChartCanvas = $(`#${id}`).get(0).getContext('2d');
            var data = {
                labels: [
                    'Success',
                    'NotFound'
                ],
                datasets: [
                    {
                        data: [sucessPercentage, (100 - sucessPercentage)],
                        backgroundColor: ['#00a65a','#f56954']
                    }
                ]
            };
            var pieOptions = {
                maintainAspectRatio: false,
                responsive: true
            };
            //Create pie or douhnut chart
            // You can switch between pie and douhnut using the method below.
            var pieChart = new Chart(pieChartCanvas, {
                type: 'pie',
                data: data,
                options: pieOptions
            });
        };
    }

    if (!window.drawTopFeaturesChart) {
        window.drawTopFeaturesChart = function (id, labels,data) {
            var donutChartCanvas = $(`#${id}`).get(0).getContext('2d');
            
            var donutData = {
                labels: labels,
                datasets: [
                    {
                        data: data,
                        backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc'],
                    }
                ]
            };

            var donutOptions = {
                maintainAspectRatio: false,
                responsive: true
            };

            var donutChart = new Chart(donutChartCanvas, {
                type: 'doughnut',
                data: donutData,
                options: donutOptions
            });
        };
    }

    if (!window.drawPlotChart) {
        window.drawPlotChart = function (id, labels, data) {
            var lineChartCanvas = $(`#${id}`).get(0).getContext('2d');

            var areaChartOptions = {
                maintainAspectRatio: false,
                responsive: true,
                legend: {
                    display: false
                },
                scales: {
                    xAxes: [{
                        type: 'time',
                        time: {
                            unit:'hour'
                        },
                        gridLines: {
                            display: false
                        }
                    }],
                    yAxes: [{
                        gridLines: {
                            display: false
                        }
                    }]
                }
            };

            var areaChartData = {
                labels: labels,
                datasets: [
                    {
                        label: 'Accumulated Requests',
                        backgroundColor: 'rgba(60,141,188,0.9)',
                        borderColor: 'rgba(60,141,188,0.8)',
                        pointRadius: false,
                        pointColor: '#3b8bba',
                        pointStrokeColor: 'rgba(60,141,188,1)',
                        pointHighlightFill: '#fff',
                        pointHighlightStroke: 'rgba(60,141,188,1)',
                        data: data
                    }
                ]
            };

            var lineChartOptions = jQuery.extend(true, {}, areaChartOptions);
            var lineChartData = jQuery.extend(true, {}, areaChartData);
            lineChartData.datasets[0].fill = false;
            lineChartOptions.datasetFill = false;

            var lineChart = new Chart(lineChartCanvas, {
                type: 'line',
                data: lineChartData,
                options: lineChartOptions
            });
        };
    }
})();