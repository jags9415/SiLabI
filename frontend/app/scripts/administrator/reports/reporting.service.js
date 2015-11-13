(function() {
    'use strict';

    angular
        .module('silabi')
        .service('ReportingService', ReportingService);

    ReportingService.$inject = ['RequestService', '$localStorage'];

    function ReportingService(RequestService, $localStorage) {

        this.GetAppointmentsByStudent = GetAppointmentsByStudent;

        function GetAppointmentsByStudent(username, start_date, end_date) {
          return RequestService.get('/report/appointments/student'+username+'/?access_token='
            +$localStorage['access_token']+'&startdate='+start_date+'&enddate='+end_date);
        }

    }
})();
