(function() {
    'use strict';

    angular
        .module('silabi')
        .service('OperatorService', OperatorService);

    OperatorService.$inject = ['RequestService', '$sessionStorage'];

    function OperatorService(RequestService, $sessionStorage) {
        this.GetAll = GetAll;
        this.GetOne = GetOne;
        this.Create = Create;
        this.Delete = Delete;

        function GetAll(request) {
          request.access_token = $sessionStorage['access_token'];
          return RequestService.get('/operators', request);
        }

        function GetOne(id) {
          var request = {};
          request.access_token = $sessionStorage['access_token'];
          return RequestService.get('/operators/' + id, request);
        }

        function Create(object) {
          return RequestService.post('/operators', object);
        }

        function Delete(id) {
          var request = {};
          request.access_token = $sessionStorage['access_token'];
          return RequestService.delete('/operators/' + id, request);
        }
    }
})();
