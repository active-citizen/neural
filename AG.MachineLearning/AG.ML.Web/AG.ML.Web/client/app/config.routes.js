(function (app) {
    'use strict';

    app.config(['$urlRouterProvider', '$stateProvider', function ($urlRouterProvider, $stateProvider) {

        $urlRouterProvider.otherwise("/net-list");

        $stateProvider.state(
            'net-list', {
                url: '/net-list',
                templateUrl: '/client/section/net/net-list.html',
                controller: 'NetList'
            });

        $stateProvider.state(
            'net-create', {
                url: '/net-create',
                templateUrl: '/client/section/net/net-create.html',
                controller: 'NetCreate'
            });

        $stateProvider.state(
            'net-manage', {
                url: '/net-manage/:id',
                templateUrl: '/client/section/net/net-manage.html',
                controller: 'NetManage'
            });
    }]);

})(window.app);