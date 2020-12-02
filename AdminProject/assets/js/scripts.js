﻿// Table Order Function Begin
$(document).ready(function () {
    $('.topnav').find('.menu-link').each(function () {
        var checkMenu = $(this).parent().find('ul');
        if (checkMenu[0] != undefined) {
            $(this).append('<h4 style="display: inline-block; float: none;"><img src="/assets/img/plus.png"></h4>')
        }
    });

    $('.topnav').find('.menu-link').click(function () {
        $('.topnav').find('ul').stop().slideUp(250);
        $(this).parent().children('ul').stop().slideDown(250);

        $('.topnav').find('.menu-link').find('h4').children('img').attr('src', '/assets/img/plus.png');
        $(this).find('h4').children('img').attr('src', '/assets/img/minus.png');

    });

    $('.topnav ul').each(function () {
        var urls = $(this).attr('data-url').split("|");
        var currentUrl = window.location.pathname.split("/");
        $(this).attr('style', '');
        for (var i = 0; i < urls.length; i++) {
            var url = urls[i];
            if (url == currentUrl[2]) {
                $(this).css('display', 'block');
            }
        }
    });

    $('.topnav ul li a').each(function () {
        var aUrl = $(this).attr('href');
        if (window.location.pathname == aUrl) $(this).addClass('selected');
    });

    var fixHelperModified = function (e, tr) {
        var $originals = tr.children();
        var $helper = tr.clone();
        $helper.children().each(function (index) {
            $(this).width($originals.eq(index).width())
        });
        return $helper;
    },
    updateIndex = function (e, ui) {
        $('td.index', ui.item.parent()).each(function (i) {
            var generatedIndex = i + 1;

            $(this).html(generatedIndex);
            var id = $(this).parent().children('.itemId').val();
            $(this).parent().children('.hiddenField').val(id + "|" + generatedIndex);
        });
    };


    $(".table-order tbody").sortable({
        helper: fixHelperModified,
        stop: updateIndex,
        items: '> tr',
        start: function (event, ui) {
            // Build a placeholder cell that spans all the cells in the row
            var cellCount = 0;
            $('td, th', ui.helper).each(function () {
                // For each TD or TH try and get it's colspan attribute, and add that or 1 to the total
                var colspan = 1;
                var colspanAttr = $(this).attr('colspan');
                if (colspanAttr > 1) {
                    colspan = colspanAttr;
                }
                cellCount += colspan;
            });

            // Add the placeholder UI - note that this is the item's content, so TD rather thanTR
            ui.placeholder.html('<td colspan="' + cellCount + '">&nbsp;</td>');
        }
    }).disableSelection();
});
// Table Order Function End


// Menu Function Begin
$(document).ready(function () {
    $('.customDatepicker').datetimepicker({ format: 'd.m.Y', step: 10 });
    $('.customDatepickerTime').datetimepicker({ format: 'd.m.Y H:i', step: 10 });

    $(".skinCheck").click(function () {
        var id = $(this).attr("data-value");
        if ($(this).is(':checked'))
            $(this).parent("td").find("input[type=hidden]").attr("value", id);
        else
            $(this).parent("td").find("input[type=hidden]").removeAttr("value");
    });

    $(".spellCheck").click(function () {
        var id = $(this).attr("data-value");
        if ($(this).is(':checked')) 
            $(this).parent("td").find("input[type=hidden]").attr("value", id);
        else
            $(this).parent("td").find("input[type=hidden]").removeAttr("value");
    });
});

function champSetSkins() {
    var skins = $("#skinsIds").val();
    if (skins == "")
        return;

    var skinIds = skins.split(",");
    $.each(skinIds, function (index, id) {
        if (id && id != "") {
            $(".skin-" + id).prop("checked", "checked");
            $(".skin-value-" + id).attr("value", id);
        }
    });
}

function champSetSpells() {
    var spells = $("#spellsIds").val();
    if (spells == "")
        return;

    var spellIds = spells.split(",");
    $.each(spellIds, function (index, id) {
        if (id && id != "") {
            $(".spell-" + id).prop("checked", "checked");
            $(".spell-value-" + id).attr("value", id);
        }
    });
}

// Menu Function End

function VideoTypeChange() {
    var value = $("#VideoType").val();
    if (value == "0") {
        $("#VideoUrl").hide();
        $("#EmbedCode").hide();
        $("#PictureUpload").show();
    }

    if (value == "1") {
        $("#VideoUrl").show();
        $("#EmbedCode").hide();
        $("#PictureUpload").hide();
    }

    if (value == "2") {
        $("#VideoUrl").hide();
        $("#EmbedCode").show();
        $("#PictureUpload").hide();
    }
}

function save_order() {
    var serialized = $('ol.sortable').nestedSortable('toArray', { startDepthCount: 0 });
    var list = [];

    $.each(serialized, function (index, value) {
        var row = {
            Depth: value.depth,
            ItemId: value.item_id,
            Left: value.left,
            ParentId: value.parent_id,
            Right: value.right
        };

        list.push(row);
    });

    list = JSON.stringify({ 'order': list });

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: "/Admin/Menu/OrderMenu",
        data: list,
        success: function () {
            $(".savechange").show();
        }
    });

    setTimeout($(".savechange").hide(), 5000);
}

$(document).ready(function () {
    jQuery("form").validationEngine();

    $(".fancy").fancybox({
        width: '70%'
    });

    $(".lblSelectGalleryCategory").click(function () {
        $(this).parent("div").children(".galleryCategories").show();
    });

    $(".GalleryCategory .galleryCategories em").click(function () {
        $(this).parent("div.galleryCategories").hide();
    });

    $(".GalleryCategory .galleryCategories div").click(function () {
        var text = $(this).text();
        var id = $(this).attr("data-id");

        $(this).parents("div.GalleryCategory").children("i").text(text);
        $(this).parents("div.GalleryCategory").children("input").val(id);

        $(this).parents("div.galleryCategories").hide();
    });

    try {
        tinymce.PluginManager.add('addadvert', function (editor, url) {
            editor.addButton('addadvert',
                {
                    title: 'Add Ads',
                    text: 'Reklam Kodu Ekle',
                    onclick: function () {
                        editor.insertContent('[ADVERTCODE]');
                    }
                });
        });

        tinymce.init({
            selector: ".richEditor",
            height: 300,
            forced_root_block : 'p',
            theme: "modern",
            plugins: [
              "advlist autolink lists link image charmap print preview hr pagebreak",
              "searchreplace wordcount visualblocks visualchars code fullscreen",
              "insertdatetime media nonbreaking save table contextmenu directionality",
              "template paste textcolor colorpicker textpattern imagetools addadvert"
            ],
            toolbar1: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | addadvert",
            toolbar2: "print preview media | forecolor backcolor | fontsizeselect | formatselect",
            fontsize_formats: '12px 14px 16px 18px 24px 36px 48px 72px',
            image_advtab: true,
            content_css: [
              "//fast.fonts.net/cssapi/e6dc9b99-64fe-4292-ad98-6974f93cd2a2.css",
              "//www.tinymce.com/css/codepen.min.css"
            ]
        });
    } catch (e) {
        console.log("Error Tinymce Editor plugin.");
        console.log(e);
    }

    try {
        $(".sortable").nestedSortable({
            handle: "div",
            items: "li",
            toleranceElement: "> div"
            //stop: function () {  }
        });
    } catch (e) { }

    $("#saveOrderMenu").click(function () {
        save_order();
    });

    $(".btnRemoveImage").click(function () {
        var id = $(this).attr("data-id");
        var divName = "#image-" + id;
        console.log(divName);
        $("div").remove(divName);
    });

    $("#VideoType").change(function () {
        VideoTypeChange();
    });

    $('.footable-res').footable();

    $('#footable-res2').footable().bind('footable_filtering', function (e) {
        var selected = $('.filter-status').find(':selected').text();
        if (selected && selected.length > 0) {
            e.filter += (e.filter && e.filter.length > 0) ? ' ' + selected : selected;
            e.clear = !e.filter;
        }
    });

    $('.clear-filter').click(function (e) {
        e.preventDefault();
        $('.filter-status').val('');
        $('table.demo').trigger('footable_clear_filter');
    });

    $('.filter-status').change(function (e) {
        e.preventDefault();
        $('table.demo').trigger('footable_filter', {
            filter: $('#filter').val()
        });
    });

    $('.filter-api').click(function (e) {
        e.preventDefault();

        //get the footable filter object
        var footableFilter = $('table').data('footable-filter');

        alert('about to filter table by "tech"');
        //filter by 'tech'
        footableFilter.filter('tech');

        //clear the filter
        if (confirm('clear filter now?')) {
            footableFilter.clearFilter();
        }
    });
});