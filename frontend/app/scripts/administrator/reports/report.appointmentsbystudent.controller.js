(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AppointmentsByStudentController', AppointmentsByStudentController);

    AppointmentsByStudentController.$inject = ['$scope', '$location', 'ReportingService', 'MessageService', 'FileService', 'moment'];

  function AppointmentsByStudentController($scope, $location, ReportingService, MessageService, FileService, moment) {
    var vm = this;

    vm.loaded = false;
    vm.appointments = [];
    vm.limit = 20;


    vm.request = {
      fields : 'id,date,state,student.username,student.full_name,laboratory.name,software.code',
      sort: [
        {field: 'date', type: 'ASC'},
        {field: 'laboratory.name', type: 'ASC'},
        {field: 'software.code', type: 'ASC'},
      ]
    };

    vm.isEmpty = isEmpty;
    vm.isLoaded = isLoaded;
    vm.loadPage = loadPage;

    activate();

    function activate() {
      var page = parseInt($location.search()['page']);

      if (isNaN(page)) {
        page = 1;
      }

      vm.totalPages = page;
      vm.page = page;
      loadPage(true);
    }

    function loadPage(cached) {
      $location.search('page', vm.page);

      vm.request.page = vm.page;
      vm.request.limit = vm.limit;
      var app_request = ReportingService.getAppointmentsByStudentRequest();
      if(app_request != null)
      {
        console.log(app_request.student);
        vm.request.query = {
            'date':[
              {
                operation: 'ge',
                value: app_request.period.start_date
              },
              {
                operation: 'lt',
                value: app_request.period.end_date
              }
            ]
          };

        vm.promise = ReportingService.GetAppointmentsByStudent(app_request.student, vm.request)
        .then(setAppointments)
        .catch(handleError);
      }
    }


    function isEmpty() {
      return vm.appointments.length === 0;
    }

    function isLoaded() {
      return vm.loaded;
    }

    function setAppointments(data) {
      vm.appointments = data.results;
      vm.page = data['current_page'];
      vm.totalPages = data['total_pages'];
      vm.totalItems = vm.limit * vm.totalPages;
      vm.loaded = true;
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
