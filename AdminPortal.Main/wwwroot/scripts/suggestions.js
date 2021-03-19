jQuery.fn.Suggestion = function (src,options,extdata) {
    var element = $(this);
    var suggAjax,a, li;
    var settings = $.extend({
        number: 5,
        onSelect: function () { },
        fixedSrc: false,
        hideOnSelect:true
    }, options);
    var suggContainer = $("<div></div>");
    suggContainer.css("display", "none");
    var list = $("<ul class='suggestion-list'></ul>");
    suggContainer.html(list);
    if (element.parent(".input-group").length) {
        element.parent(".input-group").after(suggContainer);
    } else {
        element.after(suggContainer);
    }
    $.loadScript('/js/ArrowNavigation', function () {
        element.arrowNavigation("keydown", list, "li");
    });
    element.attr("autocomplete", "off");
    if (!settings.fixedSrc && typeof src == 'string') {
        element.on("keyup", function (e) {
            if (e.which === 38 || e.which === 40 || e.which === 13)
                return;
            if (suggAjax && suggAjax.readyState !== 4) {
                suggAjax.abort();
            }
            if (element.val() === "") {
                suggContainer.hide();
            }
            suggAjax = $.ajax({
                url: src,
                method: "POST",
                data: $.extend({ q: element.val(), n: settings.number }, typeof extdata =="function"? extdata() : extdata),
                dataType: "json",
                success: function (data) {
                    list.html("");
                    for (i in data) {
                        a = $("<a href='#' data-key='" + data[i].Key + "'></a>");
                        a.text(data[i].Value);
                        li = $("<li></li>");
                        li.html(a);
                        li.appendTo(list);
                    }
                    if (list.find('li').length) {
                        suggContainer.outerWidth(element.outerWidth());
                        suggContainer.show();
                    } else {
                        suggContainer.hide();
                    }
                }
            });
        });
    } else{
        var data = [];
        if (typeof src == 'string') {
            $.ajax({
                url: src,
                method: "POST",
                dataType: "json",
                success: function (d) {
                    data = d;
                }
            });
        } else if (typeof src == 'object') {
            data = src;
        } else {
            suggContainer.hide();
            return;
        }
        element.on("focus keyup", function (e) {
            if (e.which === 38 || e.which === 40 || e.which === 13)
                return;
            list.html("");
            for (i in data) {
                if (data[i].Value.toLowerCase().indexOf(element.val().toLowerCase()) > -1 || e.type=='focus') {
                    a = $("<a href='#' data-key='" + data[i].Key + "'></a>");
                    a.text(data[i].Value);
                    li = $("<li></li>");
                    li.html(a);
                    li.appendTo(list);
                }
            }
            if (list.find('li').length) {
                suggContainer.outerWidth(element.outerWidth());
                suggContainer.show();
            } else {
                suggContainer.hide();
            }
        });

    }
    suggContainer.on("click", "ul li a", function () {
        var key = $(this).data("key");
        var value = $(this).text();
        settings.onSelect(key, value, suggContainer);
        if (settings.hideOnSelect) {
            suggContainer.hide();
        }
    });
    $(document).click(function () {
        suggContainer.hide();
        });
    suggContainer.click(function (e) {
           e.stopPropagation();
           return false;
    });
    element.click(function (e) {
        e.stopPropagation();
        return false;
    });
}