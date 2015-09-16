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

        function GetAll(request) {
          if (!request) request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/groups', request);
        }


        function GetOne(GroupID) {
          var request = {};
          request.access_token = $localStorage['access_token'];
          return RequestService.get('/groups/' + GroupID, request);
        }

        function Update(GroupID, GroupInfo) {
          var requestBody = {};
          requestBody.group = GroupInfo;
          requestBody.access_token = $localStorage['access_token'];
          return RequestService.put('/groups/' + GroupID, requestBody);
        }

        function Create(Group) {
          var requestBody = {};
          requestBody.group = Group;
          requestBody.access_token = $localStorage['access_token'];
          return RequestService.post('/groups', requestBody);
        }

        function Delete(GroupID) {
          var requestBody = {};
          requestBody.access_token = $localStorage['access_token'];
          return RequestService.delete('/groups/' + CourseID, requestBody);
        }
    }
})();
