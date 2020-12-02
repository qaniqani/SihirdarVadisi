(function () {
    var SidebarCtrl = function ($scope, $http) {
        $http({
            method: 'GET',
            url: '/api/top-content'
        }).then(function successCallback(response) {
            $scope.newslist = response.data;
        }, function () {
        })
    };

    angular
        .module("tools.sidebar", [])
        .controller("SidebarCtrl", ["$scope", "$http", SidebarCtrl]);
})();