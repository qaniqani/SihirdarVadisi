(function () {
    var LoLChampionsKCtrl = function ($scope, $http) {

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

        $scope.getResult = function () {
            jQuery('#preloader').show();
            jQuery('#status').show();
            $http({
                method: 'GET',
                url: '/api/how-much-level?username=' + $scope.selectedUserName + '&region=' + $scope.selectedServer.label + ''
            }).then(function successCallback(response) {
                $scope.results = response.data;
                $scope.showResult = true;
                $scope.showResultError = false;
                jQuery('#preloader').hide();
                jQuery('#status').hide();
            }, function () {
                $scope.showResult = false;
                $scope.showResultError = true;
                jQuery('#preloader').hide();
                jQuery('#status').hide();
            })
        }

        jQuery('[data-toggle="tooltip"]').tooltip()

    };

    angular
        .module("tools.lol-champions-k", [])
        .controller("LoLChampionsKCtrl", ["$scope", "$http", LoLChampionsKCtrl]);
})();