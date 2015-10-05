(function() {
  'use strict';

  angular
    .module('silabi')
    .service('StudentAppService', AppointmentService);

  AppointmentService.$inject = ['RequestService', '$localStorage'];

  function AppointmentService(RequestService, $localStorage) {
    this.GetAll = GetAll;

    function GetAll(student_id, request) {
      if (!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('students/' + student_id + '/appointments', request);
    }
  }
})();
