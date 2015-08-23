(function() {
    'use strict';

    angular
        .module('silabi')
        .factory('LoginService', LoginService);

    LoginService.$inject = ['$http', 'API_URL'];

    function LoginService($http, apiUrl) {
        var LoginDataService = {
          getUser: getUser
        };

        return LoginDataService;

        function getUser(username, password) {
          return $http.post(apiUrl + '/authenticate' , {
            'username': username,
            'password': password
          })
          .then(function(response) {
            return response.data;
          });
        }
    }
})();
