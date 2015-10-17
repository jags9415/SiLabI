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
    this.GetPriorities = GetPriorities;

    function GetAll(request, cached) {
      if (!request) { request = {}; }
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/laboratories', request, cached);
    }

    function GetOne(id, request, cached) {
      if (!request) { request = {}; }
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/laboratories/' + id, request, cached);
    }

    function Update(id, laboratory) {
      var request = {};
      request.laboratory = laboratory;
      request['access_token'] = $localStorage['access_token'];
      return RequestService.put('/laboratories/' + id, request);
    }

    function Create(laboratory) {
      var request = {};
      request.laboratory = laboratory;
      request['access_token'] = $localStorage['access_token'];
      return RequestService.post('/laboratories', request);
    }

    function Delete(id) {
      var request = {};
      request['access_token'] = $localStorage['access_token'];
      return RequestService.delete('/laboratories/' + id, request);
    }

    function GetSoftware(id, request, cached) {
      if (!request) { request = {}; }
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/laboratories/' + id + '/software', request, cached);
    }

    function  GetPriorities() {
      return [
        {
          value: 1,
          name: 'Alta'
        },
        {
          value: 2,
          name: 'Media'
        },
        {
          value: 3,
          name: 'Baja'
        }
      ];
    }
  }
})();
