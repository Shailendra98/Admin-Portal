jQuery.fn.SuggInput = function (src,name,options,data) {
    var settings = $.extend({
        multipleInputs: false,
        fixedSrc: false,
        hideOnSelect:true
    }, options);
    var parent = $(this).closest("div.tagged-input");
    var element = $(this);
    var button = parent.find(".button");
    $.loadScript('/js/Suggestions', function () {
        element.Suggestion(src, {
            onSelect: function (key, value, suggContainer) {
                if (settings.multipleInputs) {
                    if (!$("input[id='" + name + "_input_" + key + "']").length) {
                        button.append($("<button type='button' data-key='" + key + "' id='" + name + "_btn_" + key + "' class='btn btn-sm btn-default sugginput-btn'></button>").text(value).append(" &nbsp;&times;"))
                               .append($("<input type='hidden' name='" + name + "' id='" + name + "_input_" + key + "' value='" + key + "' class='sugginput-input' />"));
                    }
                } else {
                    button.html($("<button type='button' data-key='" + key + "' id='" + name + "_btn_" + key + "' class='btn btn-sm btn-default sugginput-btn'></button>").text(value).append(" &nbsp;&times;"))
                        .append($("<input type='hidden' name='" + name + "' id='" + name + "_input_" + key + "' value='" + key + "' class='sugginput-input' />"));
                    suggContainer.hide();
                }
                element.val("");
            },
            fixedSrc: settings.fixedSrc,
            hideOnSelect: settings.hideOnSelect
        },data);
    });
    parent.on("click", "div.button button", function (e) {
        e.preventDefault();
        var element = $(this);
        var key = element.data('key');
        element.remove();
        $("input[id='" + name + "_input_" + key + "']").remove();
    });
}