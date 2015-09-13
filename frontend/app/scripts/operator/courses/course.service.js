(function() {
    'use strict';

    angular
        .module('silabi')
        .service('CourseService', CourseService);

    CourseService.$inject = ['RequestService', '$localStorage'];

    function CourseService(RequestService, $localStorage) {
        this.GetAll = GetAll;
        this.GetOne = GetOne;
        this.Update = Update;
        this.Create = Create;
        this.Delete = Delete;

        function GetAll(request) {
          if (!request) request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/courses', request);
        }


        function GetOne(CourseID) {
          var request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/courses/' + CourseID, request);
        }

        function Update(CourseID, NewCourseInfo) {
          var requestBody = {};
          requestBody.course = NewCourseInfo;
          requestBody.access_token = $localStorage['access_token'];
          return RequestService.put('/courses/' + CourseID, requestBody);
        }

        function Create(Course) {
          var requestBody = {};
          requestBody.course = Course;
          requestBody.access_token = $localStorage['access_token'];
          return RequestService.post('/courses', requestBody);
        }

        function Delete(CourseID) {
          var requestBody = {};
          requestBody.access_token = $localStorage['access_token'];
          return RequestService.delete('/courses/' + CourseID, requestBody);
        }
    }
})();
