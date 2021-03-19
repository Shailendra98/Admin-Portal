jQuery.fn.arrowNavigation = function (trigger,navgateParentObj,navgateElement) {
    var element = $(this);
    var liSelected;
    element.on(trigger, function (e) {
        var li = navgateParentObj.find(navgateElement);
        if (e.which === 40) {
            if (liSelected) {
                e.preventDefault();
                liSelected.removeClass('selected');
                next = liSelected.next();
                if (next.length > 0) {
                    liSelected = next.addClass('selected');
                } else {
                    liSelected = li.eq(0).addClass('selected');
                }
            } else {
                liSelected = li.eq(0).addClass('selected');
            }
            return;
        } else if (e.which === 38) {
            if (liSelected) {
                e.preventDefault();
                liSelected.removeClass('selected');
                next = liSelected.prev();
                if (next.length > 0) {
                    liSelected = next.addClass('selected');
                } else {
                    liSelected = li.last().addClass('selected');
                }
            } else {
                liSelected = li.last().addClass('selected');
            }
            return;
        } else if (e.which === 13) {
            if(liSelected){
            e.preventDefault();
            if (liSelected.hasClass('selected')) {
                e.preventDefault();
                liSelected.find("a").click();
                return;
            }
            }
        }
    });
}



//$("[data-arrownavigation]").each(function (index) {
//    var navElement = $(this);
//var navElementTrigger = navElement.data("trigger");
//var navElementTarget = navElement.data("target");
//var liSelected;
//$(navElementTarget).on(navElementTrigger, function (e) {
//    var li = navElement.find($.trim(navElement.data("navchild")));
//    if (e.which === 40) {
//        if (liSelected) {
//            liSelected.removeClass('selected');
//            next = liSelected.next();
//            if (next.length > 0) {
//                liSelected = next.addClass('selected');
//            } else {
//                liSelected = li.eq(0).addClass('selected');
//            }
//        } else {
//            liSelected = li.eq(0).addClass('selected');
//        }
//        return;
//    } else if (e.which === 38) {
//        if (liSelected) {
//            liSelected.removeClass('selected');
//            next = liSelected.prev();
//            if (next.length > 0) {
//                liSelected = next.addClass('selected');
//            } else {
//                liSelected = li.last().addClass('selected');
//            }
//        } else {
//            liSelected = li.last().addClass('selected');
//        }
//        return;
//    } else if (e.which === 13) {
//        if (liSelected.hasClass('selected')) {
//            e.preventDefault();
//            liSelected.find("a").click();
//            return;
//        }
//    }
//});
//});