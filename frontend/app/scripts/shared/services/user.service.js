(function() {
    'use strict';

    angular
        .module('silabi')
        .service('UserService', UserService);

    UserService.$inject = ['RequestService', '$localStorage'];

    function UserService(RequestService, $localStorage) {
        this.GetAll = GetAll;
        this.GetOne = GetOne;

        function GetAll(request) {
          if (!request) request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/users', request);
        }

        function GetOne(Username) {
          return RequestService.get('/users/' + Username);
        }
    }
})();
