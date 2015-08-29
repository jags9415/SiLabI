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
          if (!request) request = {};
          request.access_token = $sessionStorage['access_token'];
          return RequestService.get('/operators', request);
        }

        function GetOne(id) {
          var request = {};
          request.access_token = $sessionStorage['access_token'];
          return RequestService.get('/operators/' + id, request);
        }

        function Create(id, period) {
          var request = {}
          request.period = period;
          request.access_token = $sessionStorage['access_token'];
          return RequestService.post('/operators/' + id, request);
        }

        function Delete(id) {
          var request = {};
          request.access_token = $sessionStorage['access_token'];
          return RequestService.delete('/operators/' + id, request);
        }
    }
})();
