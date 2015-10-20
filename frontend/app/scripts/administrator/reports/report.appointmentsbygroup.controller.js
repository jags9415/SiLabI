(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AppointmentsByGroupController', AppointmentsByGroupController);

    AppointmentsByGroupController.$inject = ['$scope', '$location', 'ReportingService', 'MessageService', 'FileService', 'moment'];

  function AppointmentsByGroupController($scope, $location, ReportingService, MessageService, FileService, moment) {
    var vm = this;

    vm.loaded = false;
    vm.appointments = [];
    vm.limit = 20;
    vm.period = {};
    vm.group = null;

    vm.request = {
      fields : 'id,date,state,student.username,laboratory.name,software.code,group.number,group.course.name',
      sort: [
        {field: 'student.username', type: 'ASC'},
        {field: 'date', type: 'ASC'}
      ]
    };

    vm.isEmpty = isEmpty;
    vm.isLoaded = isLoaded;
    vm.loadPage = loadPage;
    vm.generateReport = generateReport;

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
      var app_request = ReportingService.getAppointmentsByGroupRequest();
      if(app_request != null)
      {
        vm.period = app_request.period;
        setDate(app_request);
        vm.request.query['group.id'] = {
          operation: 'eq',
          value: app_request.groupID
        };
        vm.promise = ReportingService.GetAppointments(vm.request)
        .then(setAppointments)
        .catch(handleError);
      }
    }

    function setDate (app_request) {
      vm.request.query = {};
      if(app_request.period.start_date != null && app_request.period.end_date != null)
        {
          vm.request.query['date'] = [
            {
              operation: 'ge',
              value: app_request.period.start_date
            },
            {
              operation: 'lt',
              value: app_request.period.end_date
            }
          ];
        }
      if(app_request.period.start_date != null)
      {
        vm.request.query['date'] = {
          operation: 'ge',
          value: app_request.period.start_date
        };
      }
      if(app_request.period.end_date != null)
      {
        vm.request.query['date'] = {
          operation: 'lt',
          value: app_request.period.end_date
        }
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
      if(vm.appointments.length > 0)
      {
        vm.group = vm.appointments[0].group;
      }
    }

    function generateReport () {
      var html = $('#reportContent').get(0);
      FileService.downloadHtmlToPDF(html, "citas_por_grupo"+vm.group.course.name+"["+vm.group.number+"]");
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
