(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AppointmentListController', AppointmentListController);

    AppointmentListController.$inject = ['$scope', 'AppointmentService', 'MessageService', 'StateService', '$location'];

  function AppointmentListController($scope, AppointmentService, MessageService, StateService, $location) {
    var vm = this;
    vm.advanceSearch = false;
    vm.datepicker_open = false;
    vm.loaded = false;
    vm.laboratories = ["Laboratorio A", "Laboratorio B"];
    vm.appointments = [];
    vm.searched = {
      laboratory : {},
      student : {}
    };
    vm.limit = 20;
    vm.request = {
      fields : "id,date,state,student.username,laboratory.name,software.code",
      sort: [
        {field: "date", type: "ASC"},
        {field: "laboratory.name", type: "ASC"},
        {field: "software.code", type: "ASC"},
      ]
    };
    vm.states = [];

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
      loadPage();

      StateService.GetAppointmentStates()
      .then(setStates)
      .catch(handleError);
    }

    function loadPage() {
      $location.search('page', vm.page);

      vm.request.page = vm.page;
      vm.request.limit = vm.limit;

      vm.promise = AppointmentService.GetAll(vm.request)
      .then(setAppointments)
      .catch(handleError);
    }

    function openAppointment(id) {
      $location.url('/Operador/Citas/' + id);
    }

    function searchAppointment() {
      vm.request.query = {};

      if (vm.searched.student.username) {
        vm.request.query['student.username'] = {
          operation: "like",
          value: '*' + vm.searched.student.username.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.laboratory.name) {
        vm.request.query['laboratory.name'] = {
          operation: "like",
          value: '*' + vm.searched.laboratory.name.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.state) {
        vm.request.query['state'] = {
          operation: "like",
          value: vm.searched.state.value
        }
      }

      if (vm.searched.software) {
        vm.request.query['software.code'] = {
          operation: "like",
          value: '*' + vm.searched.software.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.date) {
        var date = moment(vm.searched.date).hours(0).minutes(0).seconds(0).milliseconds(0);
        var start = moment(date);
        var end = moment(date).add(1, 'days');

        vm.request.query["date"] = [
          {
          	operation: "ge",
          	value: start.format()
          },
          {
          	operation: "lt",
          	value: end.format()
          }
        ];
      }

      loadPage();
    }

    function toggleAdvanceSearch() {
      vm.advanceSearch = !vm.advanceSearch;
      delete vm.searched.code;
      delete vm.searched.name;
      delete vm.searched.state;
    }

    function openDatePicker($event) {
      if ($event) {
        $event.preventDefault();
        $event.stopPropagation();
      }
      vm.datepicker_open = true;
    }

    function isEmpty() {
      return vm.appointments.length == 0;
    }

    function isLoaded() {
      return vm.loaded;
    }

    function setAppointments(data) {
      vm.appointments = data.results;
      vm.page = data.current_page;
      vm.totalPages = data.total_pages;
      vm.totalItems = vm.limit * vm.totalPages;
      vm.loaded = true;
    }

    function setStates(states) {
      vm.states = states;
    }

    function deleteAppointment(id) {
      MessageService.confirm("¿Desea realmente eliminar esta cita?")
      .then(function() {
        AppointmentService.Delete(id)
        .then(loadPage)
        .catch(handleError);
      });
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
