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

    function GetAll(ProfessorUserName, request) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('professors/' + ProfessorUserName + '/reservations', request);
    }

    function GetOne(ProfessorUserName, ReservationID) {
      var request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('professors/' + ProfessorUserName + '/reservations/' + ReservationID, request);
    }

    function Update(ProfessorUserName, ReservationID, NewReservationInfo) {
      var requestBody = {};
      requestBody.reservation = NewReservationInfo;
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.put('professors/' + ProfessorUserName + '/reservations/' + ReservationID, requestBody);
    }

    function Create(Reservation, ProfessorUserName) {
      var requestBody = {};
      requestBody.reservation = Reservation;
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.post('professors/'+ ProfessorUserName +'/reservations', requestBody);
    }

    function Delete(ProfessorUserName, ReservationID) {
      var requestBody = {};
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.delete('professors/' + ProfessorUserName + '/reservations/' + ReservationID, requestBody);
    }
  }
})();