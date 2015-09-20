(function() {
  'use strict';

  angular
    .module('silabi')
    .service('LabService', LabService);

  LabService.$inject = ['RequestService', '$localStorage'];

  function LabService(RequestService, $localStorage) {
    this.GetAll = GetAll;
    this.GetOne = GetOne;
    this.Update = Update;
    this.Create = Create;
    this.Delete = Delete;

    function GetAll(request) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/laboratories', request);
    }


    function GetOne(LabID) {
      var request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/laboratories/' + LabID, request);
    }

    function Update(LabID, NewLabInfo) {
      var requestBody = {};
      requestBody.laboratory = NewLabInfo;
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.put('/laboratories/' + LabID, requestBody);
    }

    function Create(Lab) {
      var requestBody = {};
      requestBody.laboratory = Lab;
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.post('/laboratories', requestBody);
    }

    function Delete(LabID) {
      var requestBody = {};
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.delete('/laboratories/' + LabID, requestBody);
    }
  }
})();
