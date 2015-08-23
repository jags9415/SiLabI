(function() {
    'use strict';

    angular
        .module('silabi')
        .factory('LoginService', LoginService);

    LoginService.$inject = ['$http', 'API_URL'];

    function LoginService($http, apiUrl) {
        var LoginDataService = {
        };

        return LoginDataService;
    }
})();
