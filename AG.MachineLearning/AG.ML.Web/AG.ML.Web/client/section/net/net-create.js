(function (app) {
    app.controller('NetCreate', ['$scope', 'NetworkService', function ($scope, networkService) {
        $scope.model = {};
        $scope.save = _save;

        function _save() {
            networkService.save($scope.model);
        }
    }]);
})(window.app);
