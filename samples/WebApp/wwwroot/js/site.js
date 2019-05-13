// Panel
(function ($) {
    var $body = $('body');
    var $panel = $('.js-panel');
    var $trigger = $('.js-panel-trigger');
    var visibleClass = 'is-visible';

    $trigger.on('click', function () {
        $panel.toggleClass(visibleClass);
    });

    var $darkMode = $('.js-mode-trigger');
    var darkKey = 'isDarkEnabled';
    var localDarkValue = localStorage[darkKey] === 'true';

    var toggleDarkMode = function () {
        $body.addClass('dark-transition');
        $body.toggleClass('dark');
        $darkMode.toggleClass('is-semi');
        setTimeout(function () {
            $body.removeClass('dark-transition');
        }, 500);
    };

    if (localDarkValue) {
        toggleDarkMode();
    }

    $darkMode.on('click', function () {
        toggleDarkMode();
        localStorage[darkKey] = !localDarkValue;
    });

})(jQuery);