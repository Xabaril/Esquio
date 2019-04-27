(function () {
    // This JavaScript code helps to simulates how the data is live loading each minute (now 10s)
    // In a real match we will receive the new info from server

    document.querySelector('body').addEventListener('scriptsLoaded', function () {
        (function ($) {
            $('.js-alpha-live-disable').remove();

            alert('live');
        })(jQuery);
    });
})();