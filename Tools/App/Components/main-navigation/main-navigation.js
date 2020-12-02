(function () {
    var MainNavigationCtrl = function ($scope, $location) {
        $scope.locationPath = $location.url();
    };

    angular
        .module("tools.mainNavigation", [])
        .controller("MainNavigationCtrl", ["$scope", "$location", MainNavigationCtrl]);
})();