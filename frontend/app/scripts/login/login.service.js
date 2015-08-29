(function() {
    'use strict';

    angular
        .module('silabi')
        .service('LoginService', LoginService);

    LoginService.$inject = ['RequestService'];

    function LoginService(RequestService) {
        this.authenticate = authenticate;

        function authenticate(username, password) {
          var credentials = {
            'username': username,
            'password': password
          }
          return RequestService.post('/authenticate', credentials);
        }
    }
})();
