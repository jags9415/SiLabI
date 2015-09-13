(function() {
    'use strict';

    angular
        .module('silabi')
        .service('CourseService', CourseService);

    CourseService.$inject = ['RequestService', '$localStorage'];

    function CourseService(RequestService, $localStorage) {
        this.GetAll = GetAll;
        this.Delete = Delete;

        function GetAll(request) {
          if (!request) request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/courses', request);
        }

        function Delete(CourseID) {
          var requestBody = {};
          requestBody.access_token = $localStorage['access_token'];
          return RequestService.delete('/courses/' + CourseID, requestBody);
        }
    }
})();
