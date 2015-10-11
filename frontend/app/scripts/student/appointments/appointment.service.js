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

    function GetAll(StudentID, request) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('students/' + StudentID + '/appointments', request);
    }

    function GetOne(StudentID, AppointmentID) {
      var request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('students/' + StudentID + '/appointments/' + AppointmentID, request);
    }

    function Update(StudentID, AppointmentID, NewAppointmentInfo) {
      var requestBody = {};
      requestBody.appointment = NewAppointmentInfo;
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.put('students/' + StudentID + '/appointments/' + AppointmentID, requestBody);
    }

    function Create(Appointment, StudentID) {
      var requestBody = {};
      requestBody.appointment = Appointment;
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.post('students/'+ StudentID +'/appointments', requestBody);
    }

    function Delete(StudentID, AppointmentID) {
      var requestBody = {};
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.delete('students/' + StudentID + '/appointments/' + AppointmentID, requestBody);
    }
  }
})();
