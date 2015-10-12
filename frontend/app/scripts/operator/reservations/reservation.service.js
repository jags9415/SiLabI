(function() {
  'use strict';

angular
  .module('silabi')
  .service('ReservationService', ReservationService);

AppointmentService.$inject = ['RequestService', '$localStorage'];

function ReservationService(RequestService, $localStorage) {
  this.GetAll = GetAll;
  this.GetOne = GetOne;
  this.Update = Update;
  this.Create = Create;
  this.Delete = Delete;
  this.GetAvailable = GetAvailable;
  this.ParseAvailableDates = ParseAvailableDates;

  function GetAll(request) {
    if (!request) request = {};
    request.access_token = $localStorage['access_token'];
    return RequestService.get('/reservations', request);
  }


  function GetOne(ReservationID) {
    var request = {};
    request.access_token = $localStorage['access_token'];
    return RequestService.get('/reservations/' + ReservationID, request);
  }

  function Update(ReservationID, NewReservationInfo) {
    var requestBody = {};
    requestBody.appointment = NewReservationInfo;
    requestBody.access_token = $localStorage['access_token'];
    return RequestService.put('/reservations/' + ReservationID, requestBody);
  }

  function Create(Reservation) {
    var requestBody = {};
    requestBody.appointment = Reservation;
    requestBody.access_token = $localStorage['access_token'];
    return RequestService.post('/reservations', requestBody);
  }

  function Delete(ReservationID) {
    var requestBody = {};
    requestBody.access_token = $localStorage['access_token'];
    return RequestService.delete('/reservations/' + ReservationID, requestBody);
  }

  }
})();
