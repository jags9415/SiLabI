(function() {
  'use strict';

  angular
    .module('silabi')
    .service('ProfessorReservationService', ProfessorReservationService);

  ProfessorReservationService.$inject = ['RequestService', '$localStorage'];

  function ProfessorReservationService(RequestService, $localStorage) {

    this.GetAll = GetAll;
    this.GetOne = GetOne;
    this.Update = Update;
    this.Create = Create;
    this.Delete = Delete;

    function GetAll(username, request, cached) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/professors/' + username + '/reservations', request, cached);
    }

    function GetOne(username, id, request, cached) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/professors/' + username + '/reservations/' + id, request, cached);
    }

    function Update(username, id, reservation) {
      var request = {};
      request.reservation = reservation;
      request.access_token = $localStorage['access_token'];
      return RequestService.put('/professors/' + username + '/reservations/' + id, request);
    }

    function Create(reservation, username) {
      var request = {};
      request.reservation = reservation;
      request.access_token = $localStorage['access_token'];
      return RequestService.post('/professors/' + username + '/reservations', request);
    }

    function Delete(username, id) {
      var request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.delete('/professors/' + username + '/reservations/' + id, request);
    }
  }
})();
