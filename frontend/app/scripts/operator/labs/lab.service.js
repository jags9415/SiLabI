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
    this.GetSoftware = GetSoftware;

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

    function Update(LabID, NewLabInfo, SoftwareCodes) {
      var requestBody = {};
      var softwareRequest = {};
      requestBody.laboratory = NewLabInfo;
      softwareRequest.software = SoftwareCodes;
      requestBody.access_token = $localStorage['access_token'];
      softwareRequest.access_token = $localStorage['access_token'];
      RequestService.put('/laboratories/' + LabID + '/software', softwareRequest);
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

    function GetSoftware(LabID) {
      var request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/laboratories/' + LabID + '/software', request);
    }
  }
})();
