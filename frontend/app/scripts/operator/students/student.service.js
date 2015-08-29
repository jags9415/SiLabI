(function() {
    'use strict';

    angular
        .module('silabi')
        .service('StudentService', StudentService);

    StudentService.$inject = ['RequestService', '$sessionStorage'];

    function StudentService(RequestService, $sessionStorage) {
        this.GetAll = GetAll;
        this.GetOne = GetOne;
        this.Update = Update;
        this.Create = Create;
        this.Delete = Delete;

        function GetAll(Page) {
          return RequestService.get('/students?page=' + Page + '&access_token=' + $sessionStorage['access_token']); // Insert a real access_token
        }

        function GetOne(Username) {
          return RequestService.get('/students/' + Username);
        }

        function Update(StudentID, NewStudentInfo) {
          var requestBody = {};
          requestBody.student = NewStudentInfo;
          requestBody.access_token = $sessionStorage['access_token'];
          return RequestService.put('/students/' + StudentID, requestBody);
        }

        function Create(Student) {
          var requestBody = {};
          requestBody.student = Student;
          requestBody.access_token = $sessionStorage['access_token'];
          return RequestService.post('/students', requestBody);
        }

        function Delete(StudentID) {
          var requestBody = {};
          requestBody.access_token = $sessionStorage['access_token'];
          return RequestService.delete('/students/' + StudentID, requestBody);
        }

    }
})();
