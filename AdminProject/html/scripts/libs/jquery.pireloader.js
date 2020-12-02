/*
    Pireloader v1.0 - Simple and usefully jQuery Preloader
    Created By: Toygun Ozbek 
    Website: http://www.toygunozbek.com

    Version Update: 7 March 2017
*/

$.fn.pireloader = (function (options) {
    var settings = {
        type: "inline", // page, inline
        skin: 1,
    }

    var s = $.extend(settings, options);

    return this.each(function () {
        var $this = $(this);

        function callSkins(skin) {
            if (skin == 1) return '<div id="pire-1"> <div class="spinner"> <div class="bounce1"></div> <div class="bounce2"></div> <div class="bounce3"></div> </div> </div>';
            if (skin == 2) return '<div id="pire-2"> <div id="circularG"> <div id="circularG_1" class="circularG"></div> <div id="circularG_2" class="circularG"></div> <div id="circularG_3" class="circularG"></div> <div id="circularG_4" class="circularG"></div> <div id="circularG_5" class="circularG"></div> <div id="circularG_6" class="circularG"></div> <div id="circularG_7" class="circularG"></div> <div id="circularG_8" class="circularG"></div> </div></div>';
            if (skin == 3) return '<div id="pire-3"> <div class="cssload-loader"> <div class="cssload-inner cssload-one"></div> <div class="cssload-inner cssload-two"></div> <div class="cssload-inner cssload-three"></div> </div> </div>';
            else return '<div id="pire-1"> <div class="spinner"> <div class="bounce1"></div> <div class="bounce2"></div> <div class="bounce3"></div> </div> </div>';
        }

        if (s.type == "page") {
            $(this).prepend('<div id="pire-page"> ' + callSkins(s.skin) + ' </div>');

        }

        if (s.type == "inline") {

            $(this).prepend('<div id="pire-inline"> ' + callSkins(s.skin) + ' </div>');
        }
    });
});

$.fn.pireShow = (function (options) {
    var settings = {}
    var s = $.extend(settings, options);

    return this.each(function () {
        $(this).find('#pire-page').fadeIn(options);
        $(this).find('#pire-inline').fadeIn(options);
    });
});

$.fn.pireHide = (function (options) {
    var settings = {}
    var s = $.extend(settings, options);

    return this.each(function () {
        $(this).find('#pire-page').delay(300).fadeOut(options);
        $(this).find('#pire-inline').delay(300).fadeOut(options);
    });
});