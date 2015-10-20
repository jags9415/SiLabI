(function() {
  'use strict';

  angular
    .module('silabi')
    .service('ReportingService', ReportingService);

  ReportingService.$inject = ['RequestService', '$localStorage'];

  function ReportingService(RequestService, $localStorage) {

    var appointmentsByStudentRquest = null;
    this.setAppointmentsByStudentRequest = setAppointmentsByStudentRequest;
    this.getAppointmentsByStudentRequest = getAppointmentsByStudentRequest;
    this.GetAppointmentsByStudent = GetAppointmentsByStudent;
    this.GetAppointmentsByGroup = GetAppointmentsByGroup;

    function setAppointmentsByStudentRequest (request) {
      appointmentsByStudentRquest = request;
    }

    function getAppointmentsByStudentRequest () {
      return appointmentsByStudentRquest;
    }

    function GetAppointmentsByStudent(request) {
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/appointments', request);
    }

    function GetAppointmentsByGroup(Group, startDate, endDate) {
      var result = [];
      var group_request = {
        fields: "username,full_name",
        limit: -1,
        localStorage: $localStorage['access_token']
      };

      var student_request = {
        fields: "id,date,software.code,software.name",
        limit: -1,
        localStorage: $localStorage['access_token'],
        query: {
          'date':[
            {
              operation: 'ge',
              value: startDate
            },
            {
              operation: 'lt',
              value: endDate
            }
          ]
        }
      };

      var students = RequestService.get('/groups/' + Group.id + '/students', group_request);

      for (var i = 0; i < students.length; i++) {
        var current_student = students[i];
        student_request.query['student_request'] ={
          operation: "eq",
          value:current_student.username
        };
        var apps = GetAppointmentsByStudent(student_request);
        var json = {
          student_name: current_student.full_name,
          username: current_student.username,
          appointments: apps
        };

        result.push(json);
      }
      
      return result;
    }

  }
})();