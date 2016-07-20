(function (app) {
    'use strict';

    app.factory('NetworkService', ['$resource',
        function ($resource) {
            return $resource('/api/network/:id', { id: "@id" },
                {
                    update: { method: 'PUT' }
                });
        }
    ]);

})(window.app);