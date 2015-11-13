(function() {
    'use strict';

    angular
        .module('silabi')
        .service('ReportingService', ReportingService);

    ReportingService.$inject = ['RequestService', '$localStorage'];

    function ReportingService(RequestService, $localStorage) {

        this.GetAppointmentsByStudent = GetAppointmentsByStudent;

        function GetAppointmentsByStudent(username, request) {
          if (!request) { request = {}; }
          request['access_token'] = $localStorage['access_token'];
          return RequestService.pdf('/reports/appointments/students/' + username, request);
        }

    }
})();
