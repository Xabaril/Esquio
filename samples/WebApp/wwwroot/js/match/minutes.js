(function() {
    // This JavaScript code helps to simulates how the page is reloading each minute (now 10s) and show new info
    // In a real match we will receive the new info from server

    document.querySelector('body').addEventListener('scriptsLoaded', function () {
        (function ($) {
            if (!$('.js-alpha-live-disable').get(0)) {
                return;
            }

            alert('in progress');
        })(jQuery);
    });
})();