$(document).ready(function () {
    //> Preloader
    //$(window).on('load', function () { // makes sure the whole site is loaded 
    //    $('#status').fadeOut(); // will first fade out the loading animation 
    //    $('#preloader').delay(100).fadeOut('slow'); // will fade out the white DIV that covers the website. 
    //    $('body').delay(100).css({ 'overflow': 'visible' });
    //})

    $('#status').delay(100).fadeOut(); // will first fade out the loading animation 
    $('#preloader').delay(100).fadeOut('slow'); // will fade out the white DIV that covers the website. 
    $('body').delay(100).addClass('visible');

    //> Set Menu and Parameters
    $().jetmenu();

    //$('#site-navigation').find('.jetmenu li:nth-child(10)').attr('data-status', 'coming-soon');
    //$('#site-navigation').find('.jetmenu li:nth-child(7)').attr('data-status', 'new');

    $('#site-navigation').find('.jetmenu > li').each(function () {
        if ($(this).attr('data-status') == 'new') $(this).prepend('<span class="menu-tag new">Yeni</span>');
        if ($(this).attr('data-status') == 'coming-soon') {
            $(this).prepend('<span class="menu-tag coming-soon">Yakında</span>');
            $(this).find('.dropdown').remove();
            $(this).find('a').attr('href', 'javascript:;');
        }
    });


    //> Get page color
    var dataColor = $('#page-color-code').val();
    $('.flexible-color').css('color', dataColor);
    $('.flexible-background').css('background-color', dataColor);
    $('.flexible-border').css('border-left', '6px solid' + dataColor);

    //> Material design init
    $.material.init();

    //> Sliders
    $("#homepage-slider").responsiveSlides({
        manualControls: '#homepage-slider-pager',
        nav: true,
        namespace: "callbacks",
    });

    $("#homepage-news-slider").responsiveSlides({
        manualControls: '#homepage-news-slider-pager',
        nav: true,
        namespace: "callbacks",
    });

    //> Musthead banner
    var mastheadStatus = true;

    function mastheadInit(mastheadStatus) {
        if (mastheadStatus) {
            //$('.masthead-area').show(0);
            //$('#page-container').animate({ marginTop: "150px" }, 0);
        }
    }

    $.fn.mastheadClose = function () {
        $('#page-container').animate({ marginTop: "0" }, 300, function () {
            $('.masthead-area').hide(0);
        });
    };

    //> Search
    $.fn.searchOpen = function () {
        $('.window-overlay').fadeIn(150);
        $('.search-area').fadeIn(150);
    };

    $('*').click(function (e) {
        if (!$(e.target).is('.search-area') && !$(e.target).is('.search-area *') && !$(e.target).is('.search-button *') && !$(e.target).is('.news-holder *')) {
            $('.window-overlay').fadeOut(150);
            $('.search-area').fadeOut(150);
        }
    });

    $(document).keyup(function (e) {
        if (e.keyCode == 27) { // escape key maps to keycode `27`
            $('.window-overlay').fadeOut(150);
            $('.search-area').fadeOut(150);
        }
    });

    //> for Multi-line Ellipsis
    $('.multi-ellipsis').dotdotdot();

    //>Videos Page Tab
    $.fn.openVideoTab = function (target) {
        $('.video-buttons').find('.selected').removeClass('selected');
        $(this).addClass('selected');
        $('.latest-video').find('.tab').hide();
        $(target).show();
        return false;
    };

    //> Fixed Video
    var fixedVideoStatus = false;
    $.fn.toggleVideoScrollClose = function () {
        fixedVideoStatus = false;

        $('body').find('.video-pip-on').removeClass("video-pip-on");
    };

    function toggleVideoScroll() {
        wWidth = $(window).width();

        if (wWidth > 1000 && fixedVideoStatus) {
            // determine video Y pos break point
            var offset = $('.latest-video .video-item').offset();
            var videoBreakYPos = offset.top + $('.latest-video .video-item').height();

            // if scroll pos is below video
            if ($(window).scrollTop() > videoBreakYPos) { //jQuery(?#background-video').height() - 50) {
                if (!$('.latest-video .video-item').hasClass('video-pip-on')) {
                    $('.latest-video .video-item').addClass('video-pip-on');
                }
            }
            else {
                $('.latest-video .video-item').removeClass('video-pip-on');
            }
        }
    }

    //> Video Inside
    $.fn.loadVideo = function (url) {
        var videoUrl = url;
        $(this).parent().prepend('<iframe style="width:100%; height:100%;" src="' + videoUrl + '?autoplay=1" frameborder="0" scrolling="no" allowfullscreen="allowfullscreen" mozallowfullscreen="mozallowfullscreen" msallowfullscreen="msallowfullscreen" oallowfullscreen="oallowfullscreen" webkitallowfullscreen="webkitallowfullscreen"></iframe>');
        $(this).remove();
        fixedVideoStatus = true;
    };

    // Initilaze
    mastheadInit(mastheadStatus);

    $(window).scroll(function (e) {
        toggleVideoScroll();
    });
});

$(window).resize(function () {
    $('.multi-ellipsis').dotdotdot();
});
