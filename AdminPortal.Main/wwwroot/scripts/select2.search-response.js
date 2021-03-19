jQuery.fn.select2AutoParent = function (obj) {
    var elem = $(this);
    var modal = elem.closest(".modal");
    if (modal.length) {
        elem.select2($.extend({
            dropdownParent: modal
        },obj));
        return;
    }
    elem.select2(obj);
} 
function select2SearchResponseAjaxConfiguration(url, result, selection, placeholder,data) {
    var obj = {};
    obj.ajax = {
        url: url,
        processResults: function (data, params) {
            return {
                results: data.data,
                pagination: {
                    more: (data.page * data.size) < data.total
                }
            };
        },
        cache: true,
        delay: 500
    };
    obj.escapeMarkup = function (markup) { return markup; };
    obj.minimumInputLength = 1;
    obj.templateResult = result;
        obj.templateSelection = selection;
    if (placeholder)
        obj.placeholder = placeholder;
    return obj;
}
function select2DataConfiguration(data, result, selection, placeholder,textname) {
    var obj = {};
    var text = textname ? textname : "name";
    obj.data = $.map(data, function (obj) {
        obj.text = obj.text || obj[text];
        obj.id = obj.id || obj["Id"];
        return obj;
    });
    obj.escapeMarkup = function (markup) { return markup; },
        obj.templateResult = result,
        obj.templateSelection = selection;
    if (placeholder)
        obj.placeholder = placeholder;
    return obj;
}

function select2ApiDataConfiguration(url, result, selection, placeholder,textname) {
    var obj = {};
    $.ajax({
        url: url,
        async: false,
        success: function (data) {
            obj = select2DataConfiguration(
                data, result, selection, placeholder,textname);
        }
    });
    //var text = textname ? textname : "name";
    //$.ajax({
    //    url: url,
    //    async: false,
    //    success: function (data) {
    //        obj = select2DataConfiguration(
    //            $.map(data, function (obj) {
    //                obj.text = obj.text || obj[text];
    //                return obj;
    //            }), result, selection, placeholder);
    //    }
    //});
    return obj;   
}

function select2SelectionTemplate(obj, heading, body, picture,picturesize, picturePlacement) {
    if (obj.loading) {
        return obj.text;
    }
    if (obj[heading])
        return select2SelectionMarkup(obj[heading], obj[body], obj[picture][picturesize], picturePlacement);
    return "<span class='active'>" + obj.text + "</span>";
}
function select2SelectionMarkup(heading, body, picture, picturePlacement) {
    var markup = "<div class='media'>";
    if (picturePlacement === "left" && picture) {
        markup += "<div class='media-left'><img src='" + picture + "' alt='" + heading + "' height='50'></div>";
    }
    markup += "<div class='media-body'>"
        + "<div class='media-heading'>" + heading + "</div>";
    if (body) {
        markup += "<p class='small'>" + body + "</p>";
    }
    markup += "</div>";
    if (picturePlacement === "right" && picture) {
        markup += "<div class='media-right'><img src='" + picture + "' alt='" + heading + "' height='50'></div>";
    }
    + "</div>";
    return markup;
}
