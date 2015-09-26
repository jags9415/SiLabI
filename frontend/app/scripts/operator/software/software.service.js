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


    function GetOne(SoftwareID) {
      var request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/software/' + SoftwareID, request);
    }

    function Update(SoftwareID, NewSoftwareInfo) {
      var requestBody = {};
      requestBody.software = NewSoftwareInfo;
      requestBody.access_token = $localStorage['access_token'];
      console.log(requestBody);
      return RequestService.put('/software/' + SoftwareID, requestBody);
    }

    function Create(Software) {
      var requestBody = {};
      requestBody.software = Software;
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.post('/software', requestBody);
    }

    function Delete(SoftwareID) {
      var requestBody = {};
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.delete('/software/' + SoftwareID, requestBody);
    }
  }
})();
