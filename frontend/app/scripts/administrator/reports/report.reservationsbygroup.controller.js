(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ReservationsByGroupController', ReservationsByGroupController);

    ReservationsByGroupController.$inject = ['$scope', '$location', 'ReportingService', 'MessageService', 'FileService', 'moment'];

  function ReservationsByGroupController($scope, $location, ReportingService, MessageService, FileService, moment) {
    var vm = this;

    vm.loaded = false;
    vm.reservations = [];
    vm.limit = 20;
    vm.period = {};
    vm.group = null;

    vm.request = {
      fields : 'id,start_time,end_time,state,laboratory.name,software.code,group.number,group.course.name',
      sort: [
        {field: 'start_time', type: 'ASC'}
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
      var res_request = ReportingService.getReservationsByGroupRequest();
      if(res_request != null)
      {
        vm.period = res_request.period;
        setDate(res_request);
        vm.request.query['group.id'] = {
          operation: 'eq',
          value: res_request.groupID
        };
        vm.promise = ReportingService.GetReservations(vm.request)
        .then(setReservations)
        .catch(handleError);
      }
    }

    function setDate (res_request) {
      vm.request.query = {};
      if(res_request.period.start_date != null && res_request.period.end_date != null)
        {
          vm.request.query['start_time'] = [
            {
              operation: 'ge',
              value: res_request.period.start_date
            },
            {
              operation: 'lt',
              value: res_request.period.end_date
            }
          ];
        }
      if(res_request.period.start_date != null)
      {
        vm.request.query['start_time'] = {
          operation: 'ge',
          value: res_request.period.start_date
        };
      }
      if(res_request.period.end_date != null)
      {
        vm.request.query['start_time'] = {
          operation: 'lt',
          value: res_request.period.end_date
        }
      }
    }

    function isEmpty() {
      return vm.reservations.length === 0;
    }

    function isLoaded() {
      return vm.loaded;
    }

    function setReservations(data) {
      vm.reservations = data.results;
      vm.page = data['current_page'];
      vm.totalPages = data['total_pages'];
      vm.totalItems = vm.limit * vm.totalPages;
      vm.loaded = true;
      if(vm.reservations.length > 0)
      {
        vm.group = vm.reservations[0].group;
      }
    }

    function generateReport () {
      var html = $('#reportContent').get(0);
      FileService.downloadHtmlToPDF(html, "reservaciones_por_grupo"+vm.group.course.name+"["+vm.group.number+"]");
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
