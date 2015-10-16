(function() {
    'use strict';

    angular
        .module('silabi')
        .service('GroupService', GroupService);

    GroupService.$inject = ['RequestService', '$localStorage'];

    function GroupService(RequestService, $localStorage) {
        this.GetAll = GetAll;
        this.GetOne = GetOne;
        this.Update = Update;
        this.Create = Create;
        this.Delete = Delete;
        this.GetStudents = GetStudents;

        function GetAll(request, cached) {
          if (!request) request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/groups', request, cached);
        }


        function GetOne(id, request, cached) {
          if (!request) request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/groups/' + id, request, cached);
        }

        function Update(id, group) {
          var request = {};
          request.group = group;
          request.access_token = $localStorage['access_token'];
          return RequestService.put('/groups/' + id, request);
        }

        function Create(group) {
          var request = {};
          request.group = group;
          request.access_token = $localStorage['access_token'];
          return RequestService.post('/groups', request);
        }

        function Delete(id) {
          var request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.delete('/groups/' + id, request);
        }

        function GetStudents(id, request, cached) {
          if (!request) request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/groups/' + id +'/students', request, cached);
        }
    }
})();
