(function (app) {
    app.controller('NetList', ['$scope', 'NetworkService', function ($scope, networkService) {
        $scope.networkList = networkService.query();
    }]);
})(window.app);
