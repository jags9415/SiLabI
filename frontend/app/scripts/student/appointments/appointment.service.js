(function() {
  'use strict';

  angular
    .module('silabi')
    .service('StudentAppService', AppointmentService);

  AppointmentService.$inject = ['RequestService', '$localStorage'];

  function AppointmentService(RequestService, $localStorage) {
    this.GetAll = GetAll;
    this.Create = Create;

    function GetAll(student_id, request) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('students/' + student_id + '/appointments', request);
    }

    function Create(Appointment, student_id) {
      var requestBody = {};
      requestBody.appointment = Appointment;
      requestBody.access_token = $localStorage['access_token'];
      return RequestService.post('students/'+ student_id +'/appointments', requestBody);
    }
  }
})();
