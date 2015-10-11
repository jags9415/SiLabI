(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentAppListController', AppointmentListController);

    AppointmentListController.$inject = ['$routeParams','StudentAppService', '$localStorage','MessageService', 'StateService', '$location'];

  function AppointmentListController($routeParams, StudentAppService, $localStorage, MessageService, StateService, $location) {
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
      fields : "id,laboratory,state,software,date"
    };
    vm.states = [];
    vm.student_id = $routeParams.student_id;

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

      StudentAppService.GetAll(vm.student_id, vm.request)
      .then(setAppointments)
      .catch(handleError);
    }

    function openAppointment(id) {
      $location.url('/Estudiante/' + vm.student_id + '/Citas/' + id);
    }

    function searchAppointment() {
      vm.request.query = {};

      if (vm.searched.softwareCode) {
        vm.request.query['software.code'] = {
          operation: "like",
          value: '*' + vm.searched.softwareCode.replace(' ', '*') + '*'
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
        operation: "like",
        value: vm.searched.state.value
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

    function deleteAppointment(appID) {
      MessageService.confirm("¿Desea realmente eliminar esta cita?")
      .then(function() {
        StudentAppService.Delete(vm.student_id, appID)
        .then(loadPage)
        .catch(handleError);
      });
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
