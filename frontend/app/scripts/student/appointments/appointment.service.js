(function() {
  'use strict';

  angular
    .module('silabi')
    .service('StudentAppService', AppointmentService);

  AppointmentService.$inject = ['RequestService', '$localStorage'];

  function AppointmentService(RequestService, $localStorage) {
    this.GetAll = GetAll;
    this.GetOne = GetOne;
    this.Update = Update;
    this.Create = Create;
    this.Delete = Delete;

    function GetAll(username, request, cached) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/students/' + username + '/appointments', request, cached);
    }

    function GetOne(username, id, request, cached) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/students/' + username + '/appointments/' + id, request, cached);
    }

    function Update(username, id, appointment) {
      var request = {};
      request.appointment = appointment;
      request.access_token = $localStorage['access_token'];
      return RequestService.put('/students/' + username + '/appointments/' + id, request);
    }

    function Create(appointment, username) {
      var request = {};
      request.appointment = appointment;
      request.access_token = $localStorage['access_token'];
      return RequestService.post('/students/'+ username +'/appointments', request);
    }

    function Delete(username, id) {
      var request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.delete('/students/' + username + '/appointments/' + id, request);
    }
  }
})();
