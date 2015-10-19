(function() {
  'use strict';

  angular
    .module('silabi')
    .service('AppointmentService', AppointmentService);

  AppointmentService.$inject = ['RequestService', '$localStorage'];

  function AppointmentService(RequestService, $localStorage) {
    this.GetAll = GetAll;
    this.GetOne = GetOne;
    this.Update = Update;
    this.Create = Create;
    this.Delete = Delete;

    function GetAll(request, cached) {
      if (!request) { request = {}; }
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/appointments', request, cached);
    }

    function GetOne(id, request, cached) {
      if (!request) { request = {}; }
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/appointments/' + id, request, cached);
    }

    function Update(id, appointment) {
      var request = {};
      request.appointment = appointment;
      request['access_token'] = $localStorage['access_token'];
      return RequestService.put('/appointments/' + id, request);
    }

    function Create(appointment) {
      var request = {};
      request.appointment = appointment;
      request['access_token'] = $localStorage['access_token'];
      return RequestService.post('/appointments', request);
    }

    function Delete(id) {
      var request = {};
      request['access_token'] = $localStorage['access_token'];
      return RequestService.delete('/appointments/' + id, request);
    }

    function GetAvailable(username, request) {
      if (!request) { request = {}; }
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/students/' + username + '/appointments/available', request);
    }
  }
})();
