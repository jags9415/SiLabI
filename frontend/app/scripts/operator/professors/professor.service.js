(function() {
    'use strict';

    angular
        .module('silabi')
        .service('ProfessorService', ProfessorService);

    ProfessorService.$inject = ['$localStorage', 'RequestService'];

    function ProfessorService($localStorage, RequestService) {
      this.GetAll = GetAll;
      this.GetOne = GetOne;
      this.Update = Update;
      this.Create = Create;
      this.Delete = Delete;

      function GetAll(request, cached) {
        if (!request) { request = {}; }
        request['access_token'] = $localStorage['access_token'];
        return RequestService.get('/professors', request, cached);
      }

      function GetOne(username, request, cached) {
        var request = {};
        request['access_token'] = $localStorage['access_token'];
        return RequestService.get('/professors/' + username, request, cached);
      }

      function Create(professor) {
        var request = {};
        request.professor = professor;
        request['access_token'] = $localStorage['access_token'];
        return RequestService.post('/professors', request);
      }

      function Update(id, professor) {
        var request = {};
        request.professor = professor;
        request['access_token'] = $localStorage['access_token'];
        return RequestService.put('/professors/' + id, request);
      }

      function Delete(id) {
        var request = {};
        request['access_token'] = $localStorage['access_token'];
        return RequestService.delete('/professors/'+id, request);
      }
    }
})();
