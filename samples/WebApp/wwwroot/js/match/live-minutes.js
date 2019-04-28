(function () {
    // This JavaScript code helps to simulates how the data is live loading each minute (now 10s)
    // In a real match we will receive the new info from server

    document.querySelector('body').addEventListener('scriptsLoaded', function () {
        (function ($) {
            // Use requestAnimationFrame if you care about performance (not in this demo :D)
            var TIME = 10 * 1000;
            var POSITION_KEY = 'last-position';
            var MAX = 60;
            var HIDDEN_CLASS = 'is-hidden';

            function loadInfo() {
                // The idea of this code is to simulate that each minute (10s) the loads more info from server
                var lastPosition = sessionStorage[POSITION_KEY];

                if (!lastPosition) {
                    lastPosition = '0';
                }

                lastPosition = Number(lastPosition);
                [].reverse.call($('.js-live-minute')).each(function (i, minute) {
                    if (i > lastPosition) {
                        return;
                    }

                    $(minute).removeClass(HIDDEN_CLASS);
                });

                $('.js-live-minutes').removeClass(HIDDEN_CLASS);
                $('.js-live-minutes-progress').css('width', lastPosition + '%');

                // We can reset when the number is too big for demo
                if (lastPosition > MAX) {
                    lastPosition = -1;
                }

                sessionStorage[POSITION_KEY] = lastPosition + 1;
            }

            setInterval(loadInfo, TIME);
            loadInfo();
        })(jQuery);
    });
})();