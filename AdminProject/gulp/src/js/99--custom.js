
var hoverOpen = function () {
	$('.dropdown').on('mouseenter mouseleave click tap', function() {
		$(this).toggleClass("open");
	});
};
var hoverRemove = function () {
	$('.dropdown').off('mouseenter mouseleave click tap');
};

$(document).ready(function () {

	$('#status').delay(100).fadeOut();
	$('#preloader').delay(100).fadeOut('slow');
	$('body').delay(100).addClass('visible');

	//> Get page color
	var dataColor = $('#page-color-code').val();
	$('.flexible-color').css('color', dataColor);
	$('.flexible-background').css('background-color', dataColor);
	$('.flexible-border').css('border-left', '6px solid' + dataColor);

	var mainSlider = new Swiper ('.swiper-container', {
		loop: true
	});
	$('.vadi--slider--page--link').on('click tap touchend', function (e) {
		e.preventDefault();
		mainSlider.slideTo((+$(this).data('index') + 1));
	});
	//> Sliders
	/*$(".vadi--slider--slides").responsiveSlides({
		manualControls: '.vadi--slider--pages',
		namespace: "vadi-slider"
	});

	$("#homepage-news-slider").responsiveSlides({
		manualControls: '#homepage-news-slider-pager',
		nav: true,
		namespace: "callbacks"
	});*/

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


	/*
	 * Post Content Improvements
	 */
	var fbIframe = $('.vadi--post--content iframe[src^="https://www.facebook.com/plugins/post.php"]');
	fbIframe.removeAttr('style').addClass('vadi--extra--fb-post');

	var ytIframe = $('.vadi--post--content iframe[src*="youtube"]');
	ytIframe.addClass('vadi--extra--youtube--iframe').wrap("<div class='vadi--extra--youtube'></div>");


	/*
	 * On-hover Dropdown Menu
	 */
	var wv = $( window ).width();
	if(wv > 767)
		hoverOpen();
	else
		hoverRemove();

	/*
	 * Search Button
	 */
	$(window).click(function(event) {
		var searchButton = $('[data-button="search"]');
		if(searchButton.parent().hasClass('active') && event.target.nodeName != "INPUT"){
			TweenMax.to('.vadi--header--top--search--input', 1, {width: 0, marginRight: '-20px'});
			TweenMax.to('.vadi--header--top--search--button', .2, {delay: 1, borderRadius: '3px'});
			$('.vadi--header--top--search').removeClass('active');
		}
	});

	$('[data-button="search"]').on('click tap', function(event){
		event.stopPropagation();
		var search = $('.vadi--header--top--search');
		if(search.hasClass('active') && $('.vadi--header--top--search--input').val().length > 3)
			search.submit();
		else {
			search.addClass('active');
			TweenMax.to('.vadi--header--top--search--input', 1, {width: (search.parent().width() - 110)+'px', marginRight: 0});
			TweenMax.to('.vadi--header--top--search--button', .2, {borderRadius: '0 3px 3px 0'});
		}
	});

	/*
	 * On-air Live Streams
	 */
	var onAir = $('.vadi--header--top--on-air');
	function blink() {
		TweenMax.to(onAir, .3,{color: "#f57474", delay: .5, onComplete: function() {
			TweenMax.to(onAir, .3, {
				color: "#aaa",
				delay: .5,
				onComplete: blink
			});
		}});
	}
	if(onAir.hasClass('active'))
		blink();

	$('.vadi--header--navbar--nav .dropdown-menu li a').on('click tap', function () {
		$(this).parent().parent().hide(0);
		$(this).parent().parent().parent().removeClass('open');
	});

	/*
	 * Tooltips
	 */

	$('body').tooltip({
		selector: '.has-tooltip'
	});
	
	/*
	 * Fullcalendar.io
	 */
	$('#calendar-io').fullCalendar({
		header: {
			left: 'prev,next today',
			center: 'title',
			right: 'month,agendaDay,listWeek'
		},
		defaultView: 'listWeek',
		navLinks: true,
		height: 600,
		events: '/_php/panda/matches/calendar',
		eventRender: function (event, element) {
			if (element.find('.fc-list-item-title').length !== 0) {
				element.attr('data-placement','top');
				element.attr('title',event.game);
				element.addClass('has-tooltip');
			}
		}
	});
	
	/*
	 * Fullcalendar.io Homepage
	 */
	$('#calendar-home').fullCalendar({
		defaultView: 'listWeek',
		navLinks: true,
		height: 262,
		events: '/_php/panda/matches/calendar',
		eventRender: function (event, element) {
			if (element.find('.fc-list-item-title').length !== 0) {
				element.attr('data-placement','top');
				element.attr('title',event.game);
				element.addClass('has-tooltip');
			}
		}
	});
	
});

$(window).resize(function () {
	$('.multi-ellipsis').dotdotdot();

	/*
	 * On-hover Dropdown Menu
	 */
	var wv = $( window ).width();
	if(wv > 767)
		hoverOpen();
	else
		hoverRemove()
});