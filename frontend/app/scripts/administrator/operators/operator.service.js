(function() {
    'use strict';

    angular
        .module('silabi')
        .service('OperatorService', OperatorService);

    OperatorService.$inject = ['RequestService', '$localStorage'];

    function OperatorService(RequestService, $localStorage) {
        this.GetAll = GetAll;
        this.GetOne = GetOne;
        this.Create = Create;
        this.Delete = Delete;

        function GetAll(request, cached) {
          if (!request) { request = {}; }
          request['access_token'] = $localStorage['access_token'];
          return RequestService.get('/operators', request, cached);
        }

        function GetOne(id, cached) {
          var request = {};
          request['access_token'] = $localStorage['access_token'];
          return RequestService.get('/operators/' + id, request, cached);
        }

        function Create(id, period) {
          var request = {};
          request.period = period;
          request['access_token'] = $localStorage['access_token'];
          return RequestService.post('/operators/' + id, request);
        }

        function Delete(id) {
          var request = {};
          request['access_token'] = $localStorage['access_token'];
          return RequestService.delete('/operators/' + id, request);
        }
    }
})();
