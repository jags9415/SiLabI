(function() {
  'use strict';

  angular
    .module('silabi')
    .service('StudentAppService', AppointmentService);

  AppointmentService.$inject = ['RequestService', '$localStorage'];

  function AppointmentService(RequestService, $localStorage) {
    this.GetAll = GetAll;
    this.Create = Create;
    this.Delete = Delete;

    function GetAll(StudentID, request) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('students/' + StudentID + '/appointments', request);
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
