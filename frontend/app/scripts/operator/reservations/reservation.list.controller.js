(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ReservationListController', ProfessorReservationListController);

   ProfessorReservationListController.$inject = ['ReservationService', 'MessageService', 'StateService', '$location'];

  function ProfessorReservationListController(ReservationService, MessageService, StateService, $location) {
    var vm = this;
    vm.advanceSearch = false;
    vm.datePickerOpen = false;
    vm.loaded = false;
    vm.reservations = [];
    vm.laboratories = ['Laboratorio A', 'Laboratorio B'];
    vm.searched = {};
    vm.limit = 20;
    vm.states = [];

    vm.request = {
      fields: 'id,state,start_time,end_time,professor.full_name,laboratory.name',
      sort: [
        {field: 'start_time', type: 'ASC'},
        {field: 'professor.full_name', type: 'ASC'}
      ]
    };

    vm.open = openReservation;
    vm.delete = deleteReservation;
    vm.search = searchReservation;
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

      if (vm.searched.professor) {
        vm.request.query['professor.full_name'] = {
          operation: 'like',
          value: '*' + vm.searched.professor.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.laboratory) {
        vm.request.query['laboratory.name'] = {
          operation: 'like',
          value: '*' + vm.searched.laboratory.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.start_time) {
        var start = moment(vm.searched.start_time).startOf('day');
        var end = moment(vm.searched.start_time).endOf('day');

        vm.request.query['start_time'] = [
          { operation: 'ge', value: start.toJSON() },
          { operation: 'le', value: end.toJSON() }
        ]
      }

      if (vm.searched.state) {
        vm.request.query.state = {
          operation: 'like',
          value: vm.searched.state.value
        }
      }

      loadPage();
    }

    function toggleAdvanceSearch() {
      vm.advanceSearch = !vm.advanceSearch;
      vm.searched = {};
    }

    function openDatePicker($event) {
      if ($event) {
        $event.preventDefault();
        $event.stopPropagation();
      }
      vm.datePickerOpen = true;
    }

    function isEmpty() {
      return vm.reservations.length === 0;
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
      MessageService.confirm('¿Desea realmente eliminar esta reservación?')
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
