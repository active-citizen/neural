(function (app) {
    app.controller('NetManage', ['$scope', 'NetworkService', '$stateParams', function ($scope, networkService, $stateParams) {
        $scope.networkInfo = networkService.get({ id: $stateParams.id });
    }]);
})(window.app);
