(function () {
    if (!window.beautyJson) {
        window.beautyJson = function (id, json) {
            const identifier = `#${id}`;
            $(identifier).empty();
            $(identifier).jsonView(json, { collapsed: true });
        };
    }
})();