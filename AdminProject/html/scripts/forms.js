$(document).ready(function () {
    var imgTobase64;
    function readURL(input) {
        $('.media-preloader').pireShow(0);
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                imgTobase64 = e.target.result;
                console.log(imgTobase64)
            }

            reader.readAsDataURL(input.files[0]);
            $('.media-preloader').pireHide(0);
        } else {
            imgTobase64 = null;
            $('.media-preloader').pireHide(0);
        }
    }

    $("form").validationEngine({
        validateNonVisibleFields: true,
        updatePromptsPosition: true,
    });

    $('.media-preloader').pireloader({
        type: "page",
        skin: 1
    });

    $("form").validationEngine('attach', { scroll: false });

    function errorPattern(dataText) {
        return '<div class="alert alert-dismissible alert-danger"> <button type="button" class="close" data-dismiss="alert">×</button> <p>' + dataText + '</p> </div>'
    }

    function successPattern(dataText) {
        return '<div class="alert alert-dismissible alert-success"> <button type="button" class="close" data-dismiss="alert">×</button> <p>' + dataText + '</p> </div>'
    }

    var userStatus = false;

    //> Login Status
    function userLoggedIn(data) {
        // set div status
        $('.header-buttons .membership-button').hide();
        $('.header-buttons .user-menu').show();

        // get user params
        var email = data.Email;
        var name = data.Name;
        var surname = data.Surname;
        var gsm = data.Gsm;

        var bday = data.Day;
        var bmonth = data.Month;
        var byear = data.Year;

        var country;
        if (data.Country) country = data.Country;
        else country = "Türkiye";

        var city = data.City;
        var gender = data.Gender;

        var image;
        if (data.Picture) image = data.Picture;
        else image = "https://www.sihirdarvadisi.com/html/assets/img/no-image-user.png";

        $("#avatar-2").fileinput({
            overwriteInitial: true,
            maxFileSize: 1000,
            showClose: false,
            showCaption: false,
            showRemove: false,
            browseLabel: '',
            removeLabel: '',
            browseIcon: '<i class="fa fa-picture-o"></i>',
            removeIcon: '<i class="fa fa-remove"></i>',
            removeTitle: 'Avatarı Kaldır',
            browseTitle: 'Avatar Seç',
            elErrorContainer: '#kv-avatar-errors-1',
            msgErrorClass: 'alert alert-block alert-danger',
            defaultPreviewContent: '<img src="/Content/User/' + image + '" alt="Your Avatar">',
            layoutTemplates: { main2: '{preview} {remove} {browse}' },
            allowedFileExtensions: ["jpg", "png", "jpeg", "PNG", "JPG", "JPEG"]
        });

        // fill header fields
        $('.header-buttons .user-menu #loggedInUser').prepend(name + ' ' + surname);

        // fill modal fields
        $('#update-user-form').find('#u-name').val(name);
        $('#update-user-form').find('#u-surname').val(surname);
        $('#update-user-form').find('#u-email').val(email);
        $('#update-user-form').find('#u-phone').val(gsm);
        $('#update-user-form').find('#u-birthday').val(bday);
        $('#update-user-form').find('#u-birthdate').val(bmonth);
        $('#update-user-form').find('#u-birthyear').val(byear);
        $('#update-user-form').find('#u-country').val(country);
        $('#update-user-form').find('#u-city').val(city);
        $('#update-user-form').find('#u-gender').val(gender);


        if ($('#update-user-form').find('#u-country').val() != "Türkiye") {
            $('#u-city').attr('disabled', 'disabled');
            $("#u-city").prepend("<option class='no-city' value='0' selected='selected'>-</option>");
        } else {
            $('#u-city').removeAttr('disabled');
            $('#u-city option[value="0"]').remove();
        }
        userStatus = true;
    }

    function userNotLogin() {
        $('.header-buttons .membership-button').show();
        $('.header-buttons .user-menu').hide();
        userStatus = false;
    }

    $.ajax({
        url: "/kullanici/bilgileri",
        type: "GET",
        success: function (data, textStatus, jqXHR) {
            userLoggedIn(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            userNotLogin();
        }
    });

    //> Login
    $("#login-form").submit(function (event) {
        event.preventDefault();

        var valid = $(this).validationEngine('validate');
        var vars = $(this).serialize();

        if (valid) {
            $('.media-preloader').pireShow(0);
            var formData = { email: $('#login-email').val(), password: $('#login-password').val() };

            $.ajax({
                url: "/kullanici",
                type: "POST",
                data: formData,
                success: function (data, textStatus, jqXHR) {
                    $('#login-dialog').modal('hide');
                    $('#login-form').find('input[type="text"]').val('');
                    $('#login-form').find('input[type="password"]').val('');
                    userLoggedIn(data);
                    $('.media-preloader').pireHide(150);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $('#login-form').find('.alert').remove();
                    $('#login-form').prepend(errorPattern(errorThrown));
                    $('#login-form').find('input[type="text"]').val('');
                    $('#login-form').find('input[type="password"]').val('');
                    $('.media-preloader').pireHide(150);
                }
            });
        }
    });

    //> Logout
    $('#logout-button').click(function (event) {
        $.ajax({
            url: "/kullanici/cikis",
            type: "GET",
            success: function (data, textStatus, jqXHR) {
                location.reload();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.statusText);
            }
        });

        event.preventDefault();
    });

    //> Update User
    $("#avatar-2").change(function () {
        readURL(this);
    });

    $('#u-country').change(function () {
        if ($(this).val() != "Türkiye") {
            $('#u-city').attr('disabled', 'disabled');
            $("#u-city").prepend("<option class='no-city' value='0' selected='selected'>-</option>");
        } else {
            $('#u-city').removeAttr('disabled');
            $('#u-city option[value="0"]').remove();
        }
    });

    $('#update-user-form').submit(function (event) {
        event.preventDefault();

        var valid = $(this).validationEngine('validate');
        var vars = $(this).serialize();

        var field1 = $('#u-name').val();
        var field2 = $('#u-surname').val();
        var field3 = $('#u-email').val();
        var field4 = $('#u-password').val();
        var field5 = $('#u-password-again').val();
        var field6 = $('#u-phone').val();
        var field7 = $('#u-city').val();
        var field8 = $('#u-gender').val();
        var field9 = $('#u-birthday').val();
        var field12 = $('#u-birthdate').val();
        var field13 = $('#u-birthyear').val();
        var field11 = $('#u-country').val();

        if (valid) {
            $('.media-preloader').pireShow(0);
            var formData = {
                Name: field1,
                Surname: field2,
                Email: field3,
                Password: field4,
                Password2: field5,
                Gsm: field6,
                Day: field9,
                Month: field12,
                Year: field13,
                Country: field11,
                City: field7,
                Gender: field8,
                Picture: imgTobase64
            };

            $.ajax({
                url: "/kullanici/bilgileri",
                type: "POST",
                data: formData,
                success: function (data, textStatus, jqXHR) {
                    $('#update-user-form').find('.alert').remove();
                    $('#update-user-form').prepend(successPattern(data));
                    $('.media-preloader').pireHide(150);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $('#update-user-form').find('.alert').remove();
                    $('#update-user-form').prepend(errorPattern(errorThrown));
                    $('.media-preloader').pireHide(150);
                }
            });
        }
    });

    //> Update Password
    $("#update-password-form").submit(function (event) {
        event.preventDefault();

        var valid = $(this).validationEngine('validate');
        var vars = $(this).serialize();

        var field1 = $('#up-oldPassword').val();
        var field2 = $('#up-password').val();
        var field3 = $('#up-password2').val();


        if (valid) {
            $('.media-preloader').pireShow(0);
            var formData = { password: field2, password2: field3, oldPassword: field1 };

            $.ajax({
                url: "/kullanici/sifre",
                type: "POST",
                data: formData,
                success: function (data, textStatus, jqXHR) {
                    $('#update-password-form').find('.alert').remove();
                    $('#update-password-form').prepend(successPattern(data));
                    $('#update-password-form').find('input[type="text"]').val('');
                    $('#update-password-form').find('input[type="password"]').val('');
                    $('.media-preloader').pireHide(150);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $('#update-password-form').find('.alert').remove();
                    $('#update-password-form').prepend(errorPattern(errorThrown));
                    $('.media-preloader').pireHide(150);
                }
            });
        }
    });

    //> Forgot Password
    $("#forgot-password-form").submit(function (event) {
        event.preventDefault();

        var valid = $(this).validationEngine('validate');
        var vars = $(this).serialize();
        var field1 = $('#fg-email').val();

        if (valid) {
            $('.media-preloader').pireShow(0);
            var formData = { email: field1 };

            $.ajax({
                url: "kullanici/sifremi-unuttum",
                type: "POST",
                data: formData,
                success: function (data, textStatus, jqXHR) {
                    $('#forgot-password-form').find('.alert').remove();
                    $('#forgot-password-form').prepend(successPattern(data));
                    $('#forgot-password-form').find('input[type="text"]').val('');
                    $('.media-preloader').pireHide(150);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $('#forgot-password-form').find('input[type="text"]').val('');
                    $('#forgot-password-form').find('.alert').remove();
                    $('#forgot-password-form').prepend(errorPattern(errorThrown));
                    $('.media-preloader').pireHide(150);
                }
            });
        }
    });

    //> Create User
    $("#avatar-1").change(function () {
        readURL(this);
    });

    $('#cu-country').change(function () {
        if ($(this).val() != "Türkiye") {
            $('#cu-city').attr('disabled', 'disabled');
            $("#cu-city").prepend("<option class='no-city' value='0' selected='selected'>-</option>");
        } else {
            $('#cu-city').removeAttr('disabled');
            $('#cu-city option[value="0"]').remove();
        }
    });

    $('#register-form').submit(function (event) {
        event.preventDefault();

        var valid = $(this).validationEngine('validate');
        var vars = $(this).serialize();

        var field1 = $('#cu-name').val();
        var field2 = $('#cu-surname').val();
        var field3 = $('#cu-email').val();
        var field4 = $('#cu-password').val();
        var field5 = $('#cu-password-again').val();
        var field6 = $('#cu-phone').val();
        var field7 = $('#cu-city').val();
        var field8 = $('#cu-gender').val();
        var field9 = $('#cu-birthday').val();
        var field12 = $('#cu-birthdate').val();
        var field13 = $('#cu-birthyear').val();
        var field11 = $('#cu-country').val();


        if (valid) {
            $('.media-preloader').pireShow(0);
            var formData = {
                Name: field1,
                Surname: field2,
                Email: field3,
                Password: field4,
                Password2: field5,
                Gsm: field6,
                Day: field9,
                Month: field12,
                Year: field13,
                Country: field11,
                City: field7,
                Gender: field8,
                Picture: imgTobase64
            };

            $.ajax({
                url: "/kullanici/olustur",
                type: "POST",
                data: formData,
                success: function (data, textStatus, jqXHR) {
                    $('#register-form').find('.alert').remove();
                    $('#register-form').prepend(successPattern(data));
                    $('#register-form').find('input[type="text"]').val('');
                    $('#register-form').find('input[type="password"]').val('');
                    $('.media-preloader').pireHide(150);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $('#register-form').find('.alert').remove();
                    $('#register-form').prepend(errorPattern(errorThrown));
                    $('.media-preloader').pireHide(150);
                }
            });
        }
    });

    //> Survey
    function getSurveyResults() {
        $('.media-preloader').pireShow(150);

        var surveyID = $('#survey-id').val();

        $.ajax({
            url: "/anket/" + surveyID + "/sonucu",
            type: "GET",
            success: function (data, textStatus, jqXHR) {
                $('#survey-form').hide();
                $('#survey-results').show();

                for (i = 0; i < data.result.Answers.length; i++) {
                    $('#survey-results ul').append('<li class="col-md-12"><p>' + data.result.Answers[i].Answer + '</p> <div><span style="width:' + data.result.Answers[i].PercentageVote + '%">' + data.result.Answers[i].PercentageVote + '%</span><em>' + data.result.Answers[i].Vote + ' Oy</em></div></li>')
                }
                $('.media-preloader').pireHide(150);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $('.media-preloader').pireHide(150);
                //console.log(errorThrown)
                alert(errorThrown);
            }
        });
    }

    $("#survey-form").submit(function (event) {
        event.preventDefault();

        var valid = $(this).validationEngine('validate');
        var vars = $(this).serialize();

        var answerID = $('input[name=optionsRadios]:checked').val();
        var surveyID = $('#survey-id').val();
        if (valid) {
            $('.media-preloader').pireShow(150);

            var formData = { answerId: answerID, surveyId: surveyID };

            $.ajax({
                url: "/anket/" + surveyID + "/" + answerID,
                type: "POST",
                data: formData,
                success: function (data, textStatus, jqXHR) {
                    getSurveyResults();
                    $('.media-preloader').pireHide(150);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $('.media-preloader').pireHide(150);
                    alert(errorThrown);
                }
            });
        }
    });

    $('#get-survey-results').click(function () {
        getSurveyResults();
    });

    //> Video
    var pageFirstState = 2;
    function getVideos() {
        var valid = $(this).validationEngine('validate');
        var vars = $(this).serialize();

        var categoryUrl = $('#video-category-url').val();
        var gameType = $('#gameType').val();
        $('#load-video').show();
        $('.no-video').hide();
        $('.media-preloader').pireShow(150);


        if (valid) {
            var formData = { categoryUrl: categoryUrl, gameType: gameType, page: pageFirstState };

            $.ajax({
                url: "/video/" + categoryUrl + "/ajax",
                type: "POST",
                data: formData,
                success: function (data, textStatus, jqXHR) {
                    for (i = 0; i < data.result.length; i++) {
                        $('#list-videos').append('<div class="video col-md-3"> <div class="video-item" style="background:url(/Content/' + data.result[i].Picture + '_200x120.jpg);"> <a class="play-inside-box" href="/video/' + data.result[i].Url + '/detay"> <div class="play-overlay"><i class="fa fa-play-circle"></i></div> </a> </div> <div class="video-text"> <p class="flexible-color">' + data.result[i].GameType + '</p> <em class="multi-ellipsis">' + data.result[i].Subject + '</em> </div> </div>')
                    }

                    if (data.buttonStatus == 0) {
                        $('#load-video').hide();
                        $('.no-video').show();
                    }

                    pageFirstState++;
                    $('.media-preloader').pireHide(150);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText);
                    $('.media-preloader').pireHide(150);
                }
            });
        }
    }

    var pageSecondState = 1;
    function getVideosByGameType() {
        var valid = $(this).validationEngine('validate');
        var vars = $(this).serialize();

        var categoryUrl = $('#video-category-url').val();
        var gameType = $('#gameType').val();
        $('#load-video').show();
        $('.no-video').hide();
        $('.media-preloader').pireShow(150);

        if (valid) {
            var formData = { categoryUrl: categoryUrl, gameType: gameType, page: pageSecondState };

            $.ajax({
                url: "/video/" + categoryUrl + "/ajax",
                type: "GET",
                data: formData,
                success: function (data, textStatus, jqXHR) {
                    $('#list-videos').html('');
                    for (i = 0; i < data.result.length; i++) {
                        $('#list-videos').append('<div class="video col-md-3"> <div class="video-item" style="background:url(/Content/' + data.result[i].Picture + '_200x120.jpg);"> <a class="play-inside-box" href="/video/' + data.result[i].Url + '/detay"> <div class="play-overlay"><i class="fa fa-play-circle"></i></div> </a> </div> <div class="video-text"> <p class="flexible-color">' + data.result[i].GameType + '</p> <em class="multi-ellipsis">' + data.result[i].Subject + '</em> </div> </div>')
                    }
                    $('.media-preloader').pireHide(150);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText);
                    $('.media-preloader').pireHide(150);
                }
            });

        }
    }

    $('#load-video').click(function () {
        getVideos();
        pageSecondState = 1;
    });

    $('#gameType').on('change', function () {
        getVideosByGameType();
        pageFirstState = 2;
    });

    //> Gallery
    var galPageFirstState = 2;
    function getGalleries() {
        var valid = $(this).validationEngine('validate');
        var vars = $(this).serialize();

        var categoryUrl = $('#gallery-category-url').val();
        var gameType = $('#gameTypeGallery').val();
        $('#load-gallery').show();
        $('.no-video').hide();
        $('.media-preloader').pireShow(150);


        if (valid) {
            var formData = { categoryUrl: categoryUrl, gameType: gameType, page: galPageFirstState };

            $.ajax({
                url: "/fotograf-galerileri/" + categoryUrl + "/ajax",
                type: "POST",
                data: formData,
                success: function (data, textStatus, jqXHR) {
                    for (i = 0; i < data.result.length; i++) {
                        $('#list-videos').append('<div class="video col-md-3"> <div class="video-item" style="background:url(/Content/Gallery/' + data.result[i].Picture + ');"> <a class="play-inside-box" href="/fotograf-galerileri/fotograf-galerileri/fotograf-galerileri/' + data.result[i].Url + '/detay"></a> </div> <div class="video-text"> <p class="flexible-color">' + data.result[i].GameTypeText + '</p> <em class="multi-ellipsis">' + data.result[i].Subject + '</em> </div> </div>')
                    }

                    if (data.buttonStatus == 0) {
                        $('#load-gallery').hide();
                        $('.no-video').show();
                    }

                    galPageFirstState++;
                    $('.media-preloader').pireHide(150);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //console.log(jqXHR.responseText);
                    $('.media-preloader').pireHide(150);
                }
            });
        }
    }

    var galPageSecondState = 1;
    function getGalleriesByGameType() {
        var valid = $(this).validationEngine('validate');
        var vars = $(this).serialize();

        var categoryUrl = $('#gallery-category-url').val();
        var gameType = $('#gameTypeGallery').val();
        $('#load-gallery').show();
        $('.no-video').hide();
        $('.media-preloader').pireShow(150);

        if (valid) {
            var formData = { categoryUrl: categoryUrl, gameType: gameType, page: galPageSecondState };

            $.ajax({
                url: "/fotograf-galerileri/" + categoryUrl + "/ajax",
                type: "GET",
                data: formData,
                success: function (data, textStatus, jqXHR) {
                    $('#list-videos').html('');
                    for (i = 0; i < data.result.length; i++) {
                        $('#list-videos').append('<div class="video col-md-3"> <div class="video-item" style="background:url(/Content/Gallery/' + data.result[i].Picture + ');"> <a class="play-inside-box" href="/fotograf-galerileri/fotograf-galerileri/' + data.result[i].Url + '/detay"></a> </div> <div class="video-text"> <p class="flexible-color">' + data.result[i].GameTypeText + '</p> <em class="multi-ellipsis">' + data.result[i].Subject + '</em> </div> </div>')
                    }

                    $('.media-preloader').pireHide(150);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText);
                    $('.media-preloader').pireHide(150);
                }
            });

        }
    }

    $('#load-gallery').click(function () {
        getGalleries();
        galPageSecondState = 1;
    });

    $('#gameTypeGallery').on('change', function () {
        getGalleriesByGameType();
        galPageFirstState = 2;
    });

    //> Like Module
    var likeID = $('#LikeId').val();

    $('#like-button').click(function () {
        if (userStatus != false) {
            var field1 = $('#ContentId').val();
            var field2 = $('#ContentUrl').val();
            var field3 = $('#ContentGameType').val();
            var formData = { ContentID: field1, ContentUrl: field2, ContentType: field3 };
            var likedCount = $('#like-button em b').html();

            if ($(this).children('i').hasClass("fa-heart")) { // Beğenmişse

                $.ajax({
                    url: "/kullanici/begenilenler/" + likeID + "/kaldir",
                    type: "POST",
                    //data: formData,
                    success: function (data, textStatus, jqXHR) {
                        $('#like-button i').removeClass('fa-heart').addClass('fa-heart-o');
                        $('#like-button em b').html(likedCount - 1);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert(errorThrown);
                    }
                });
            } else { // Beğenmemişse
                $.ajax({
                    url: "/kullanici/begenilenler/" + field1 + "/" + field2 + "/" + field3 + "",
                    type: "POST",
                    data: formData,
                    success: function (data, textStatus, jqXHR) {
                        $('#like-button i').removeClass('fa-heart-o').addClass('fa-heart');
                        $('#like-button em b').html(parseInt(likedCount) + 1);
                        likeID = data;
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert(errorThrown);
                    }
                });
            }
        } else {
            alert('"Lütfen üye girişi yapınız."');
        }
    });

    // Initilaze



});