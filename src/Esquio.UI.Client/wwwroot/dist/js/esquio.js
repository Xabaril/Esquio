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
})();