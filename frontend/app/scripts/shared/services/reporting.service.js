(function() {
  'use strict';

  angular
    .module('silabi')
    .service('ReportingService', ReportingService);

  ReportingService.$inject = ['RequestService', '$localStorage'];

  function ReportingService(RequestService, $localStorage) {

    var appointmentsByStudentRquest = null;
    var appointmentsByGroupRquest = null;
    this.setAppointmentsByStudentRequest = setAppointmentsByStudentRequest;
    this.getAppointmentsByStudentRequest = getAppointmentsByStudentRequest;
    this.setAppointmentsByGroupRequest = setAppointmentsByGroupRequest;
    this.getAppointmentsByGroupRequest = getAppointmentsByGroupRequest;
    this.GetAppointments = GetAppointments;

    function setAppointmentsByStudentRequest (request) {
      appointmentsByStudentRquest = request;
    }

    function getAppointmentsByStudentRequest () {
      return appointmentsByStudentRquest;
    }

    function setAppointmentsByGroupRequest (request) {
      appointmentsByGroupRquest = request;
    }

    function getAppointmentsByGroupRequest () {
      return appointmentsByGroupRquest;
    }

    function GetAppointments(request) {
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/appointments', request);
    }
  }
})();