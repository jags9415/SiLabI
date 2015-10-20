(function() {
  'use strict';

  angular
    .module('silabi')
    .service('ReportingService', ReportingService);

  ReportingService.$inject = ['RequestService', '$localStorage'];

  function ReportingService(RequestService, $localStorage) {

    var appointments_by_student_request = null;
    var appointments_by_group_request = null;
    var reservations_by_professor_request = null;
    var reservations_by_group_request = null;
    this.setAppointmentsByStudentRequest = setAppointmentsByStudentRequest;
    this.getAppointmentsByStudentRequest = getAppointmentsByStudentRequest;
    this.setAppointmentsByGroupRequest = setAppointmentsByGroupRequest;
    this.getAppointmentsByGroupRequest = getAppointmentsByGroupRequest;
    this.getReservationsByProfessorRequest = getReservationsByProfessorRequest;
    this.setReservationsByProfessorRequest = setReservationsByProfessorRequest;
    this.setReservationsByGroupRequest = setReservationsByGroupRequest;
    this.getReservationsByGroupRequest = getReservationsByGroupRequest;
    this.GetAppointments = GetAppointments;
    this.GetReservations = GetReservations;

    function setAppointmentsByStudentRequest (request) {
      appointments_by_student_request = request;
    }

    function getAppointmentsByStudentRequest () {
      return appointments_by_student_request;
    }

    function setAppointmentsByGroupRequest (request) {
      appointments_by_group_request = request;
    }

    function getAppointmentsByGroupRequest () {
      return appointments_by_group_request;
    }

    function setReservationsByProfessorRequest (request) {
      reservations_by_group_request = request;
    }

    function getReservationsByProfessorRequest () {
      return reservations_by_group_request;
    }

    function setReservationsByGroupRequest (request) {
      reservations_by_group_request = request;
    }

    function getReservationsByGroupRequest () {
      return reservations_by_group_request;
    }

    function GetAppointments(request) {
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/appointments', request);
    }

    function GetReservations(request) {
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/reservations', request);
    }

  }
})();