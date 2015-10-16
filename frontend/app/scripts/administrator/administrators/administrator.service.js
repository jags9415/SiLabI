(function() {
    'use strict';

    angular
        .module('silabi')
        .service('AdminService', AdminService);

    AdminService.$inject = ['RequestService', '$localStorage'];

    function AdminService(RequestService, $localStorage) {
      this.GetAll = GetAll;
      this.GetOne = GetOne;
      this.Create = Create;
      this.Delete = Delete;

      function GetAll(request, cached) {
        if (!request) request = {};
        request.access_token = $localStorage['access_token'];
        return RequestService.get('/administrators', request, cached);
      };

      function GetOne(id, cached) {
        var request = {};
        request.access_token = $localStorage['access_token'];
        return RequestService.get('/administrators/' + id, request, cached);
      };

      function Create(id) {
        var request = {}
        request.access_token = $localStorage['access_token'];
        return RequestService.post('/administrators/' + id, request);
      }

      function Delete(id) {
        var request = {};
        request.access_token = $localStorage['access_token'];
        return RequestService.delete('/administrators/' + id, request);
      }
    }
})();
