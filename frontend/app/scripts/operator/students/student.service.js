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

        function GetAll(request) {
          if (!request) request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/students', request);
        }

        function GetOne(Username) {
          var request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/students/' + Username, request);
        }

        function Update(StudentID, NewStudentInfo) {
          var requestBody = {};
          requestBody.student = NewStudentInfo;
          requestBody.access_token = $localStorage['access_token'];
          return RequestService.put('/students/' + StudentID, requestBody);
        }

        function Create(Student) {
          var requestBody = {};
          requestBody.student = Student;
          requestBody.access_token = $localStorage['access_token'];
          return RequestService.post('/students', requestBody);
        }

        function Delete(StudentID) {
          var requestBody = {};
          requestBody.access_token = $localStorage['access_token'];
          return RequestService.delete('/students/' + StudentID, requestBody);
        }

        function GetGroups (Username, request) {
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/students/' + Username +'/groups', request);
        }

    }
})();
