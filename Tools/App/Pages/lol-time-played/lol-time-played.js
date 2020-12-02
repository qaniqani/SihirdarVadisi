(function () {
    var LoLTimePlayedCtrl = function ($scope, $http, $interval) {

        $scope.imgPending = true;

        $scope.showResult = false;

        $scope.servers = [
            { value: 'NA', label: 'NA' },
            { value: 'EUW', label: 'EUW' },
            { value: 'EUNE', label: 'EUNE' },
            { value: 'BR', label: 'BR' },
            { value: 'TR', label: 'TR' },
            { value: 'RU', label: 'RU' },
            { value: 'LAN', label: 'LAN' },
            { value: 'LAS', label: 'LAS' },
            { value: 'OCE', label: 'OCE' }
        ];

        $scope.selectedServer = $scope.servers[4];

        $scope.getLolTime = function () {
            jQuery('#preloader').show();
            jQuery('#status').show();
            $http({
                method: 'POST',
                url: '/api/hour-play-lol?username=' + $scope.selectedUserName + '&region=' + $scope.selectedServer.label + ''
            }).then(function successCallback(response) {
                $scope.results = response.data;

                var gameDay = response.data.playedGameDay;
                var gameHour = response.data.gameTime;
                var gameCount = response.data.playedGameTime;

                roundedGameHour = Math.round(gameCount * 100) / 100;

                var imageID = "";

                if (roundedGameHour < 300) imageID = "ringoImage";
                else if (roundedGameHour >= 300 && roundedGameHour < 600) imageID = "ringoImage1";
                else if (roundedGameHour >= 600 && roundedGameHour < 900) imageID = "ringoImage2";
                else if (roundedGameHour >= 900 && roundedGameHour < 1200) imageID = "ringoImage3";
                else if (roundedGameHour >= 1200 && roundedGameHour < 1500) imageID = "ringoImage4";
                else if (roundedGameHour >= 1500 && roundedGameHour < 1800) imageID = "ringoImage5";
                else if (roundedGameHour >= 1800 && roundedGameHour < 2100) imageID = "ringoImage6";
                else if (roundedGameHour >= 2100 && roundedGameHour < 2400) imageID = "ringoImage7";
                else if (roundedGameHour >= 2400 && roundedGameHour < 2700) imageID = "ringoImage8";
                else if (roundedGameHour >= 2700) imageID = "ringoImage9";

                // Get the image
                var sampleImage = document.getElementById(imageID), canvas = convertImageToCanvas(sampleImage);

                // Actions
                convertCanvasToImage(canvas);

                // Converts image to canvas; returns new canvas element
                function convertImageToCanvas(image) {
                    var canvas = document.createElement("canvas");
                    canvas.width = image.width;
                    canvas.height = image.height;
                    var context = canvas.getContext("2d");
                    context.drawImage(image, 0, 0);
                    context.font = "28pt Calibri";
                    context.fillStyle = 'white';
                    context.fillText(gameDay + " Gün, " + gameCount + " Saat", 665, 393);

                    context.fillText(gameHour + " defa", 665, 467);
                    context.font = "bold 28pt Calibri";
                    context.fillText($scope.selectedUserName, 665, 318);
                    return canvas;
                }

                // Converts canvas to an image
                function convertCanvasToImage(canvas) {
                    var image = new Image();
                    image.src = canvas.toDataURL("image/png");
                    $scope.shareImg = image.src;
                    return image;
                }

                var imageDataRaw = $scope.shareImg;  //.replace(/^data:image\/\w+;base64,/, "");

                $.ajax({
                    url: "http://imgsrv.sihirdarvadisi.com/image/ConvertImage",
                    type: "POST",
                    data: { 'dataUri': imageDataRaw },
                    xhrFields: {
                        withCredentials: false
                    },
                    success: function (data) {
                        $scope.imgUrl = data;
                    },
                    error: function (shr, status, data) {
                        console.log("error " + data + " Status " + shr.status);
                    },
                });

                $scope.showResult = true;
                $scope.showResultError = false;
                jQuery('#preloader').hide();
                jQuery('#status').hide();
                $interval(function () { $scope.imgPending = false; }, 10000)
            }, function () {
                $scope.showResult = false;
                $scope.showResultError = true;
                jQuery('#preloader').hide();
                jQuery('#status').hide();
            })
        }

        $scope.shareBtn = function () {
            var resultImage = $scope.imgUrl;
            var resultName = "Sihirdar Vadisi - Araçlar | Kaç Saat LoL Oynadın?";
            var resultDesc = "Tahmin yürütmekten ciğerin mi soldu? Bırak o ölçümleri biz yapalım sen o matematik yerine YGS'ye hazırlan.";
            FB.ui({
                display: 'popup',
                method: 'feed',
                link: "http://araclar.sihirdarvadisi.com/kac-saat-lol-oynadin",
                picture: "http://imgsrv.sihirdarvadisi.com/uploads/" + resultImage,
                name: resultName,
                caption: 'Ekleyen: sihirdarvadisi.com',
                description: resultDesc
            }, function (response) { });
        }
    };



    angular
        .module("tools.lol-time-played", [])
        .controller("LoLTimePlayedCtrl", ["$scope", "$http", "$interval", LoLTimePlayedCtrl]);
})();