// Panel
(function ($) {
    var $panel = $('.js-panel');
    var $trigger = $('.js-panel-trigger');
    var visibleClass = 'is-visible';

    $trigger.on('click', function () {
        $panel.toggleClass(visibleClass);
    });
}) (jQuery);