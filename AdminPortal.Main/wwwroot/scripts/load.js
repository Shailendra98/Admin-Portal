jQuery.loadScript = function (url, callback) {
    jQuery.ajax({
        url: url,
        cache: true,
        dataType: 'script',
        success: callback
    });
};
jQuery.loadStyle = function (url, callback) {
    if (!$('link[href="' + url + '"]').length) {
        var elem = document.createElement("link");
        elem.rel = "stylesheet";
        elem.href = url;
        $('head').append(elem);
    }
};