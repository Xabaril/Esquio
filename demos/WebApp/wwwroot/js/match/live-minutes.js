(function ($) {
    // This JavaScript code helps to simulates how the data is live loading each minute (now 5s)
    // In a real match we will receive the new info from server

    var TIME = 5 * 1000;
    var POSITION_KEY = 'last-position';
    var MAX = 60;
    var MIN = 3;
    var HIDDEN_CLASS = 'is-hidden';

    var $liveMinutesProgress = $('.js-live-minutes-progress');

    function loadInfo() {
        // The idea of this code is to simulate that each minute (5s) the loads more info from server
        var lastPosition = sessionStorage[POSITION_KEY];

        if (!lastPosition) {
            lastPosition = MIN + '';
        }

        lastPosition = Number(lastPosition);
        [].reverse.call($('.js-live-minute')).each(function (i, minute) {
            if (i > lastPosition) {
                return;
            }

            var $minute = $(minute);
            $minute.removeClass(HIDDEN_CLASS);
            if (i === lastPosition) {
                var $icon = $minute.find('.js-icon-animate');
                var $tempIcon = $icon.clone();
                $icon.parent().append($tempIcon);
                $tempIcon.addClass('live-animation');

                setTimeout(function () {
                    $tempIcon.removeClass('live-animation');
                    $tempIcon.remove();
                }, 1000);
            }
        });

        $('.js-live-minutes').removeClass(HIDDEN_CLASS);
        $liveMinutesProgress.css('width', lastPosition + '%');

        // We can reset when the number is too big for demo
        if (lastPosition > MAX) {
            lastPosition = MIN;
        }

        sessionStorage[POSITION_KEY] = lastPosition + 1;
    }

    // Use requestAnimationFrame if you care about performance (not in this demo :D)
    setInterval(loadInfo, TIME);
    loadInfo();
})(jQuery);