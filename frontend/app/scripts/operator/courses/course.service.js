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

        function GetAll(request, cached) {
          if (!request) { request = {}; }
          request['access_token'] = $localStorage['access_token'];
          return RequestService.get('/courses', request, cached);
        }


        function GetOne(id, cached) {
          var request = {};
          request['access_token'] = $localStorage['access_token'];
          return RequestService.get('/courses/' + id, request, cached);
        }

        function Update(id, course) {
          var request = {};
          request.course = course;
          request['access_token'] = $localStorage['access_token'];
          return RequestService.put('/courses/' + id, request);
        }

        function Create(course) {
          var request = {};
          request.course = course;
          request['access_token'] = $localStorage['access_token'];
          return RequestService.post('/courses', request);
        }

        function Delete(id) {
          var request = {};
          request['access_token'] = $localStorage['access_token'];
          return RequestService.delete('/courses/' + id, request);
        }
    }
})();
