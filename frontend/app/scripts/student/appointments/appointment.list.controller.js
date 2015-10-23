(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('StudentAppListController', AppointmentListController);

  AppointmentListController.$inject = ['$location', '$localStorage', 'StudentAppService', 'MessageService', 'StateService', 'moment'];

  function AppointmentListController($location, $localStorage, StudentAppService, MessageService, StateService, moment) {
    var vm = this;

    vm.advanceSearch = false;
    vm.datePickerOpen = false;
    vm.loaded = false;
    vm.laboratories = ['Laboratorio A', 'Laboratorio B'];
    vm.appointments = [];
    vm.states = [];
    vm.$storage = $localStorage;
    vm.username = vm.$storage['username'];
    vm.limit = 20;

    vm.searched = {
      laboratory : {},
      student : {}
    };

    vm.request = {
      fields : 'id,date,state,laboratory.name,software.code',
      sort: [
        {field: 'date', type: 'ASC'},
        {field: 'laboratory.name', type: 'ASC'},
        {field: 'software.code', type: 'ASC'},
      ]
    };

    vm.open = openAppointment;
    vm.delete = deleteAppointment;
    vm.search = searchAppointment;
    vm.isEmpty = isEmpty;
    vm.isLoaded = isLoaded;
    vm.loadPage = loadPage;
    vm.toggleAdvanceSearch = toggleAdvanceSearch;
    vm.openDatePicker = openDatePicker;

    activate();

    function activate() {
      var page = parseInt($location.search()['page']);

      if (isNaN(page)) {
        page = 1;
      }

      vm.totalPages = page;
      vm.page = page;
      loadPage(true);

      StateService.GetAppointmentStates()
      .then(setStates)
      .catch(handleError);
    }

    function loadPage(cached) {
      $location.search('page', vm.page);

      vm.request.page = vm.page;
      vm.request.limit = vm.limit;

      vm.promise = StudentAppService.GetAll(vm.username, vm.request, cached)
      .then(setAppointments)
      .catch(handleError);
    }

    function openAppointment(id) {
      $location.url('/Estudiante/Citas/' + id);
    }

    function searchAppointment() {
      vm.request.query = {};

      if (vm.searched.software) {
        vm.request.query['software.code'] = {
          operation: 'like',
          value: '*' + vm.searched.software.replace(' ', '*') + '*'
        };
      }

      if (vm.searched.laboratory.name) {
        vm.request.query['laboratory.name'] = {
          operation: 'like',
          value: '*' + vm.searched.laboratory.name.replace(' ', '*') + '*'
        };
      }

      if (vm.searched.state) {
        vm.request.query.state = {
          operation: 'like',
          value: vm.searched.state.value
        };
      }

      if (vm.searched.date) {
        var date = moment(vm.searched.date).hours(0).minutes(0).seconds(0).milliseconds(0);
        var start = moment(date);
        var end = moment(date).add(1, 'days');

        vm.request.query['date'] = [
          {
          	operation: 'ge',
          	value: start.toJSON()
          },
          {
          	operation: 'lt',
          	value: end.toJSON()
          }
        ];
      }

      loadPage();
    }

    function toggleAdvanceSearch() {
      vm.advanceSearch = !vm.advanceSearch;
      delete vm.searched.software;
      delete vm.searched.laboratory.name;
      delete vm.searched.state;
      delete vm.searched.date;
    }

    function openDatePicker($event) {
      if ($event) {
        $event.preventDefault();
        $event.stopPropagation();
      }
      vm.datePickerOpen = true;
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

    function setStates(states) {
      vm.states = states;
    }

    function deleteAppointment(appID) {
      MessageService.confirm('¿Desea realmente eliminar esta cita?')
      .then(function() {
        StudentAppService.Delete(vm.username, appID)
        .then(loadPage)
        .catch(handleError);
      });
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
