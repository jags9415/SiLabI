(function() {
    'use strict';

    angular
        .module('silabi')
        .service('StudentService', LoginService);

    LoginService.$inject = ['RequestService'];

    function LoginService(RequestService) {
        this.GetAll = GetAll;
        this.GetOne = GetOne;
        this.Update = Update;

        function GetAll(Page) {
          return RequestService.get('/students?page=' + Page + '&access_token=123'); // Insert a real access_token
        }

        function GetOne(Username) {
          return RequestService.get('/students/' + Username);
        }

        function Update(StudentID, NewStudentInfo) {
          return RequestService.put('/students/' + StudentID, {
            'student': NewStudentInfo,
            'access_token': '123'     // Insert a real access_token
          });
        }

    }
})();
