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

    function GetAll(request) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/appointments', request);
    }


    function GetOne(AppointmentID) {
      var request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/appointments/' + AppointmentID, request);
    }

    function Update(AppointmentID, NewAppointmentInfo) {
      var requestBody = {};
      requestBody.appointment = NewAppointmentInfo;
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.put('/appointments/' + AppointmentID, requestBody);
    }

    function Create(Appointment) {
      var requestBody = {};
      requestBody.appointment = Appointment;
      requestBody.access_token = $localStorage['access_token'];
      console.log(requestBody);
      return RequestService.post('/appointments', requestBody);
    }

    function Delete(AppointmentID) {
      var requestBody = {};
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.delete('/appointments/' + AppointmentID, requestBody);
    }
  }
})();
