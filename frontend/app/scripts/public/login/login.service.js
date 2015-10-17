(function() {
    'use strict';

    angular
        .module('silabi')
        .service('AuthenticationService', AuthenticationService);

    AuthenticationService.$inject = ['RequestService', '$localStorage', 'jwtHelper'];

    function AuthenticationService(RequestService, $localStorage, jwtHelper) {
        this.authenticate = authenticate;
        this.isAuthenticated = isAuthenticated;
        this.getUserData = getUserData;

        function authenticate(username, password) {
          var credentials = {
            'username': username,
            'password': password
          };
          return RequestService.post('/authenticate', credentials);
        }

        function isAuthenticated() {
          var token = $localStorage['access_token'];

          if (!token) {
            return false;
          }
          else {
            return !jwtHelper.isTokenExpired(token);
          }
        }

        function getUserData() {
          var data = {};
          var token = $localStorage['access_token'];
          var payload = jwtHelper.decodeToken(token);

          data['access_token'] = token;
          data.username = payload.username;
          data.id = payload.id;
          data.type = payload.type;

          return data;
        }
    }
})();
