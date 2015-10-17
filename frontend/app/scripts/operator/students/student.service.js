(function() {
    'use strict';

    angular
        .module('silabi')
        .service('StudentService', StudentService);

    StudentService.$inject = ['RequestService', '$localStorage'];

    function StudentService(RequestService, $localStorage) {
        this.GetAll = GetAll;
        this.GetOne = GetOne;
        this.Update = Update;
        this.Create = Create;
        this.Delete = Delete;
        this.GetGroups = GetGroups;

        function GetAll(request, cached) {
          if (!request) { request = {}; }
          request['access_token'] = $localStorage['access_token'];
          return RequestService.get('/students', request, cached);
        }

        function GetOne(username, request, cached) {
          if (!request) { request = {}; }
          request['access_token'] = $localStorage['access_token'];
          return RequestService.get('/students/' + username, request, cached);
        }

        function Update(id, student) {
          var request = {};
          request.student = student;
          request['access_token'] = $localStorage['access_token'];
          return RequestService.put('/students/' + id, request);
        }

        function Create(student) {
          var request = {};
          request.student = student;
          request['access_token'] = $localStorage['access_token'];
          return RequestService.post('/students', request);
        }

        function Delete(id) {
          var request = {};
          request['access_token'] = $localStorage['access_token'];
          return RequestService.delete('/students/' + id, request);
        }

        function GetGroups(username, request, cached) {
          if (!request) { request = {}; }
          request['access_token'] = $localStorage['access_token'];
          return RequestService.get('/students/' + username +'/groups', request, cached);
        }
    }
})();
