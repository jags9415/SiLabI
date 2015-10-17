(function() {
  'use strict';

angular
  .module('silabi')
  .service('ReservationService', ReservationService);

ReservationService.$inject = ['RequestService', '$localStorage'];

  function ReservationService(RequestService, $localStorage) {
    this.GetAll = GetAll;
    this.GetOne = GetOne;
    this.Update = Update;
    this.Create = Create;
    this.Delete = Delete;

    function GetAll(request, cached) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/reservations', request, cached);
    }

    function GetOne(id, cached) {
      var request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/reservations/' + id, request, cached);
    }

    function Update(id, reservation) {
      var request = {};
      request.reservation = reservation;
      request.access_token = $localStorage['access_token'];
      return RequestService.put('/reservations/' + id, request);
    }

    function Create(reservation) {
      var request = {};
      request.reservation = reservation;
      request.access_token = $localStorage['access_token'];
      return RequestService.post('/reservations', request);
    }

    function Delete(id) {
      var request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.delete('/reservations/' + id, request);
    }
  }
})();
