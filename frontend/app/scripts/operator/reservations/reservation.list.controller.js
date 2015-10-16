(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ReservationListController', ProfessorReservationListController);

   ProfessorReservationListController.$inject = ['ReservationService', 'MessageService', 'StateService', '$location'];

  function ProfessorReservationListController(ReservationService, MessageService, StateService, $location) {
    var vm = this;
    vm.advanceSearch = false;
    vm.loaded = false;
    vm.reservations = [];
    vm.searched = {};
    vm.limit = 20;
    vm.request = {
      fields : "id,professor,laboratory,state,software,group,start_time"
    };
    vm.states = [];
    vm.open = openReservation;
    vm.delete = deleteReservation;
    vm.search = searchReservation;
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

      StateService.GetReservationStates()
      .then(setStates)
      .catch(handleError);

      loadPage();
    }

    function loadPage() {
      $location.search('page', vm.page);

      vm.request.page = vm.page;
      vm.request.limit = vm.limit;

      vm.promise = ReservationService.GetAll(vm.request)
      .then(setReservations)
      .catch(handleError);
    }

    function openReservation(id) {
      $location.url('/Operador/Reservaciones/' + id);
    }

    function searchReservation() {
      vm.request.query = {};

      if (vm.searched.professor && vm.searched.professor.full_name) {
        vm.request.query['professor.full_name'] = {
          operation: "like",
          value: '*' + vm.searched.professor.full_name.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.group && vm.searched.group.course.name) {
        vm.request.query['group.course.name'] = {
          operation: "like",
          value: '*' + vm.searched.group.course.name.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.laboratory && vm.searched.laboratory.name) {
        vm.request.query['laboratory.name'] = {
          operation: "like",
          value: '*' + vm.searched.laboratory.name.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.software && vm.searched.software.code) {
        vm.request.query['software.code'] = {
          operation: "like",
          value: '*' + vm.searched.software.code.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.state) {
        vm.request.query.state = {
          operation: "eq",
          value: vm.searched.state.value
        }
      }

      loadPage();
    }

    function toggleAdvanceSearch() {
      vm.advanceSearch = !vm.advanceSearch;
    }


    function isEmpty() {
      return vm.reservations.length == 0;
    }

    function isLoaded() {
      return vm.loaded;
    }

    function setReservations(data) {
      vm.reservations = data.results;
      vm.page = data.current_page;
      vm.totalPages = data.total_pages;
      vm.totalItems = vm.limit * vm.totalPages;
      vm.loaded = true;
    }

    function setStates(states) {
      vm.states = states;
    }

    function deleteReservation(id) {
      MessageService.confirm("¿Desea realmente eliminar esta reservación?")
      .then(function() {
        ReservationService.Delete(id)
        .then(loadPage)
        .catch(handleError);
      });
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
