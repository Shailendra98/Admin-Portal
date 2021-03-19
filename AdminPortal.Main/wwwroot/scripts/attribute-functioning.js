$(function () {
    /// For links on non-anchor elements
    $(document).on("click", "[data-href]", function (e) {
        e.preventDefault();
        window.location.href = $(this).data("href");
    });
    /// For loading content of a Modal or Tab using a button or link
    $(document).on('click', "[data-bs-toggle][data-load-url]", function (e) {
        e.preventDefault();
        var elem = $(this);
        var loadedAttr = elem.attr('data-loaded');
        var loadOnce = false;
        if (typeof loadedAttr !== typeof undefined && loadedAttr !== false) {
            if (loadedAttr === "true") {
                return;
            }
            loadOnce = true;
        }
        var target = "";
        var tagName = elem.prop("tagName").toLowerCase();
        if (tagName === 'a') {
            target = elem.data('bs-target');
            if (typeof target === typeof undefined || target === false)
                target = elem.attr("href");
            
        } else {
            target = elem.data('bs-target');
        }
        var toggle = elem.data('bs-toggle');
        var LoadIn;
        switch (toggle) {
            case "modal":
                LoadIn = $(target).find(".modal-content").html("<div class='modal-body'><div class='text-center'><img src='/images/ripple.gif' alt='Loading..'/></div></div>");
                break;
            case "tab":
                LoadIn = $(target).html("<div class='text-center'><img src='/images/ripple.gif' alt='Loading..'/></div>");
                break;
        }
        LoadIn.load(elem.data("load-url"), function () {
            if(loadOnce){
                elem.attr('data-loaded', 'true');
            }
        });
    });
    // Loads a modal page using "data-modal-href" within same or other(specified in "data-target" attribute) modal attribute. 
    $(".modal").on('click',"[data-modal-href]", function (e) {
        e.preventDefault();
        var elem = $(this);
        var href = elem.data("modal-href");
        var target = elem.data('bs-target');
        if (typeof target === typeof undefined || target === false || target==="self") {
            elem.closest(".modal-content").html("<div class='modal-body'><div class='text-center'><img src='/images/ripple.gif' alt='Loading..'/></div></div>")
            .load(href);
        } else {
            elem.closest(".modal").modal('hide');
            $(target).modal('show');
            $(target).find(".modal-content").html("<div class='modal-body'><div class='text-center'><img src='/images/ripple.gif' alt='Loading..'/></div></div>")
            .load(href);
        }
    });
});