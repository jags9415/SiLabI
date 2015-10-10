(function() {
  'use strict';

  angular
    .module('silabi')
    .service('SoftwareService', SoftwareService);

  SoftwareService.$inject = ['RequestService', '$localStorage'];

  function SoftwareService(RequestService, $localStorage) {
    this.GetAll = GetAll;
    this.GetOne = GetOne;
    this.Update = Update;
    this.Create = Create;
    this.Delete = Delete;

    function GetAll(request) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/software', request);
    }


    function GetOne(id, request) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/software/' + id, request);
    }

    function Update(id, software) {
      var request = {};
      request.software = software;
      request.access_token = $localStorage['access_token'];
      return RequestService.put('/software/' + id, request);
    }

    function Create(software) {
      var request = {};
      request.software = software;
      request.access_token = $localStorage['access_token'];
      return RequestService.post('/software', request);
    }

    function Delete(id) {
      var request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.delete('/software/' + id, request);
    }
  }
})();
