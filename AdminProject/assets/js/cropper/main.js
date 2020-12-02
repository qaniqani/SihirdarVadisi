$(function () {
    var $previews = $('.preview');
    var $image = $('#previewImage');

    //sizes
    var cropHeight = parseInt($("#hfCropHeight").val());
    var cropWidth = parseInt($("#hfCropWidth").val());
    var index = parseInt($("#hfIndex").val());
    var id = parseInt($("#hfId").val());
    var sizeId = parseInt($("#hfSizeId").val());
    var name = $("#hfName").val();

    $image.cropper({
        viewMode: 1,
        dragMode: 'move',
        restore: false,
        guides: true,
        highlight: false,
        cropBoxMovable: false,
        cropBoxResizable: false,
        ready: function (e) {
            var $clone = $(this).clone().removeClass('cropper-hidden');
            var containerData = $image.cropper('getContainerData');
            $clone.css({
                display: 'block',
                width: '100%',
                minWidth: 0,
                minHeight: 0,
                maxWidth: 'none',
                maxHeight: 'none'
            });

            $previews.css({
                width: '100%',
                overflow: 'hidden'
            }).html($clone);

            $image.cropper('setCropBoxData', {
                left: (containerData.width - cropWidth) / 2,
                top: (containerData.height - cropHeight) / 2,
                width: cropWidth,
                height: cropHeight
            });
        },

        crop: function (e) {
            var imageData = $(this).cropper('getImageData');
            var previewAspectRatio = e.width / e.height;

            $previews.each(function () {
                var $preview = $(this);
                var previewWidth = $preview.width();
                var previewHeight = previewWidth / previewAspectRatio;
                var imageScaledRatio = e.width / previewWidth;

                $preview.height(previewHeight).find('img').css({
                    width: imageData.naturalWidth / imageScaledRatio,
                    height: imageData.naturalHeight / imageScaledRatio,
                    marginLeft: -e.x / imageScaledRatio,
                    marginTop: -e.y / imageScaledRatio
                });
            });
        }
    });

    $("#browserCrop").click(function (e) {
        var canvasSize = { width: cropWidth, height: cropHeight }; // Canvasın width-height parametresi
        var canvas = $image.cropper('getCroppedCanvas', canvasSize).toDataURL(); // Image Data Url burada <--

        $.ajax({
            type: 'POST',
            url: '/Admin/Picture/SendPicture',
            data: JSON.stringify({
                id: id,
                sizeId: sizeId,
                index: index,
                width: cropWidth,
                height: cropHeight,
                name: name,
                imageBaseData: canvas
            }),
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
                $('#status').show(0); // will first fade out the loading animation 
                $('#preloader').show(0); // will fade out the white DIV that covers the website. 
                $('body').css({ 'overflow': 'hidden' });
            },
            complete: function () {
            },
            success: function (msg) {
                console.log(msg);
                if (msg.isNext == true) {
                    var nextUrl = "?width=" + msg.width + "&height=" + msg.height;
                    location.href = nextUrl;
                } else
                    location.href = "/Admin/Content/View/" + id;

            },
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr + " - " + textStatus + " - " + errorThrown);

                alert("xhr: " + xhr + " text: " + textStatus + " error: " + errorThrown);
            }
        });
    });
});