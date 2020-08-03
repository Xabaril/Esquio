(function () {


    if (!window.addAuthenticationScript) {
        window.addAuthenticationScript = function (isAzureAd) {
            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = isAzureAd ? "_content/Microsoft.Authentication.WebAssembly.Msal/AuthenticationService.js" : "_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js";
            $("head").append(script);
        };
    }

    if (!window.exportProduct) {
        window.exportProduct = function (json) {
            let a = document.createElement('a');
            let blob = new Blob([json], { 'type': "text/json" });
            a.href = window.URL.createObjectURL(blob);
            a.download = "product.json";
            a.click();
        };
    }

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
                        backgroundColor: ['#00a65a', '#f56954']
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
        window.drawTopFeaturesChart = function (id, labels, data) {
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
                            unit: 'hour'
                        },
                        gridLines: {
                            display: false
                        }
                    }],
                    yAxes: [{
                        gridLines: {
                            display: false
                        },
                        ticks: {
                            min: 0
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

    if (!window.initTagsInput) {
        window.initTagsInput = function (id, className) {
            $(id).tagsinput('refresh');
            $(id).on('itemAdded itemRemoved', function () {
                $(id).val($(id).val().replace(/,/g, ';'));
                $(id).get(0).dispatchEvent(new Event('change'));
            });
            if (className) $('.bootstrap-tagsinput').addClass(className);
        };
    }

    if (!window.reloadTagsInput) {
        window.reloadTagsInput = function (id, tags) {
            $(id).tagsinput('removeAll');
            for (let tag of tags) {
                $(id).tagsinput('add', tag);
            }
        };
    }

    if (!window.initDateTimePicker) {
        window.initDateTimePicker = function (id) {
            $(id).datetimepicker({
                format: 'YYYY-MM-DD HH:mm:ss',
                icons: {
                    time: "far fa-clock",
                    date: "far fa-calendar-alt",
                    up: "fa fa-arrow-up",
                    down: "fa fa-arrow-down"
                }
            });
            $(id).on("change.datetimepicker", function () {
                $(id).children("input[type=text]").get(0).dispatchEvent(new Event('change'));
            });
        };
    }

    if (!window.initRangeSlider) {
        window.initRangeSlider = function (id, from, disable) {
            $(id).ionRangeSlider({
                min: 0,
                max: 100,
                from: from,
                prettify: function (n) {
                    return `On: ${n}% users<br />Off: ${100 - n}% users`;
                },
                onChange: function () {
                    $(id).get(0).dispatchEvent(new Event('change'));
                },
                disable
            });
        };
    }

    if (!window.reloadRangeSlider) {
        window.reloadRangeSlider = function (id, from) {
            $(id).data("ionRangeSlider").update({ from });
        };
    }

    if (!window.initEditable) {
        $.fn.editable.defaults.mode = 'inline';
        window.initEditable = function (id, required, emptyText) {
            $(id).editable({
                showbuttons: false,
                highlight: '#a0d0d0',
                onblur: "submit",
                emptytext: emptyText,
                clear: false,
                success: function (response, newValue) {
                    $(id + "-hidden").val(newValue);
                    $(id + "-hidden").get(0).dispatchEvent(new Event('change'));
                },
                validate: function (value) {
                    if (required && !$.trim(value)) {
                        return 'This field is required';
                    }
                }
            });
            $(id).on('shown', function (e, editable) {
                editable.input.$input.attr("spellcheck", "false");
            });
        };
    }

    if (!window.historyReplaceState) {
        window.historyReplaceState = function (segment) {
            window.history.replaceState(window.history.state, document.title, window.location.href.replace(/\/[^\/]*$/, `/${segment}`));
        };
    }
})();
