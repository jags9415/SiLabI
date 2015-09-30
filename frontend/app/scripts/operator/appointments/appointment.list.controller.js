(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AppointmentListController', AppointmentListController);

    AppointmentListController.$inject = ['$scope', 'AppointmentService', 'MessageService', 'StateService', '$location'];

  function AppointmentListController($scope, AppointmentService, MessageService, StateService, $location) {
    var vm = this;
    vm.advanceSearch = false;
    vm.loaded = false;
    vm.appointments = [];
    vm.searched = {
      laboratory : {},
      student : {}
    };
    vm.limit = 20;
    vm.request = {
      fields : "id,student,laboratory,state,software,date"
    };
    vm.states = [];  

    vm.open = openAppointment;
    vm.delete = deleteAppointment;
    vm.search = searchAppointment;
    vm.isEmpty = isEmpty;
    vm.isLoaded = isLoaded;
    vm.loadPage = loadPage;
    vm.toggleAdvanceSearch = toggleAdvanceSearch;

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

      AppointmentService.GetAll(vm.request)
      .then(setAppointments)
      .catch(handleError);
    }

    function openAppointment(id) {
      $location.url('/Operador/Appointment/' + id);
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
      vm.request.query.state = {
        operation: "eq",
        value: vm.searched.state.value
      }
      console.log(vm.searched.state.value);
    }

    if (vm.searched.date) {
      vm.request.query.date = {
        operation: "eq",
        value: vm.searched.date
      }
    }

      loadPage();
    }

    function toggleAdvanceSearch() {
    vm.advanceSearch = !vm.advanceSearch;
    delete vm.searched.code;
    delete vm.searched.name;
    delete vm.searched.state;
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
      console.log(id);
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
