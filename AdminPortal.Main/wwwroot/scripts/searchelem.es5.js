'use strict';

jQuery.fn.SearchElem = function (listitem, lookinto, options) {
    var settings = $.extend({
        numberOfResult: 10,
        caseSensitive: false
    }, options);
    var searchBox = $(this);
    var matched = false;
    searchBox.on('keyup', function () {
        var val = searchBox.val();
        if (!settings.caseSensitive) val = val.toLowerCase();
        var counter = 0;
        $(listitem).each(function (i) {
            var val1 = "";
            matched = false;
            for (var a in lookinto) {
                if (lookinto[a].type === 'text') {
                    val1 = $(this).find(lookinto[a].elem).text();
                } else if (lookinto[a].type === 'attr') {
                    val1 = $(this).find(lookinto[a].elem).attr(lookinto[a].attr);
                }
                if (!settings.caseSensitive) {
                    val1 = val1.toLowerCase();
                }
                matched = val1.includes(val);
                if (matched) break;
            }
            if (!matched) {
                $(this).hide();
            } else if (counter < settings.numberOfResult || settings.numberOfResult === 0) {
                $(this).show();
                counter++;
            }
        });
    });
};

