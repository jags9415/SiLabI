(function() {
    'use strict';

    angular
        .module('silabi')
        .service('ReportingService', ReportingService);

    ReportingService.$inject = ['RequestService', '$localStorage'];

    function ReportingService(RequestService, $localStorage) {

        this.GetAppointmentsByStudent = GetAppointmentsByStudent;
        this.GetAppointmentsByGroup = GetAppointmentsByGroup;
        this.GetReservationsByProfessor = GetReservationsByProfessor;
        this.GetReservationsByGroup = GetReservationsByGroup;

        function GetAppointmentsByStudent(username, request) {
          if (!request) { request = {}; }
          request['access_token'] = $localStorage['access_token'];
          return RequestService.pdf('/reports/appointments/students/' + username, request);
        }

        function GetAppointmentsByGroup(group, request) {
          if (!request) { request = {}; }
          request['access_token'] = $localStorage['access_token'];
          return RequestService.pdf('/reports/appointments/groups/' + group, request);
        }

        function GetReservationsByProfessor(username, request) {
          if (!request) { request = {}; }
          request['access_token'] = $localStorage['access_token'];
          return RequestService.pdf('/reports/reservations/professors/' + username, request);
        }

        function GetReservationsByGroup(group, request) {
          if (!request) { request = {}; }
          request['access_token'] = $localStorage['access_token'];
          return RequestService.pdf('/reports/reservations/groups/' + group, request);
        }

    }
})();
