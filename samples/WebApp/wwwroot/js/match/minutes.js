(function ($) {
    // This JavaScript code helps to simulates how the page is reloading each minute (now 5s) and show new info
    // In a real match we will receive the new info from server

    // The idea of this code is to simulate that each minute (5s) the page reloads and retrieves more info from server
    var POSITION_KEY = 'last_position';
    var TIME = 5 * 1000;
    var MAX = 60;
    var HIDDEN_CLASS = 'is-hidden';
    var lastPosition = sessionStorage[POSITION_KEY];

    if (!lastPosition) {
        lastPosition = '0';
    }

    lastPosition = Number(lastPosition);
    [].reverse.call($('.js-minute')).each(function (i, minute) {
        if (i > lastPosition) {
            return;
        }

        $(minute).removeClass(HIDDEN_CLASS);
    });

    $('.js-minutes').removeClass(HIDDEN_CLASS);
    $('.js-minutes-progress').css('width', lastPosition + '%');

    // We can reset when the number is too big for demo
    if (lastPosition > MAX) {
        lastPosition = -1;
    }

    setTimeout(function () {
        sessionStorage[POSITION_KEY] = lastPosition + 1;
        location.reload();
    }, TIME);
})(jQuery);