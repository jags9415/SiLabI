(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ProfessorReservationListController', ProfessorReservationListController);

   ProfessorReservationListController.$inject = ['$scope', 'ProfessorReservationService', 'GroupService', 'PeriodService', 'MessageService', 'StateService', '$location', '$localStorage'];

  function ProfessorReservationListController($scope, ProfessorReservationService, GroupService, PeriodService, MessageService, StateService, $location, $localStorage) {
    var vm = this;

    vm.advanceSearch = false;
    vm.datePickerOpen = false;
    vm.loaded = false;
    vm.laboratories = ['Laboratorio A', 'Laboratorio B'];
    vm.reservations = [];
    vm.limit = 20;

    vm.searched = {
      laboratory: {},
      group: { course: {} }
    };

    vm.groups_request = {
      fields: 'id,number,course.name'
    };

    vm.request = {
      fields: 'id,state,start_time,end_time,laboratory.name,group.course.name,software.code',
      sort: {field: 'start_time', type: 'ASC'}
    };

    vm.states = [];
    vm.$storage = $localStorage;
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

      if (vm.$storage['username']) {
        vm.username = vm.$storage['username'];
      }

      loadPage();
      getGroups();

      StateService.GetReservationStates()
      .then(setStates)
      .catch(handleError);
    }

    function loadPage() {
      $location.search('page', vm.page);

      vm.request.page = vm.page;
      vm.request.limit = vm.limit;

      vm.promise = ProfessorReservationService.GetAll(vm.username, vm.request)
      .then(setReservations)
      .catch(handleError);
    }

    function openReservation(id) {
      $location.url('/Docente/Reservaciones/' + id);
    }

    function searchReservation() {
      vm.request.query = {};

      if (vm.searched.group) {
        vm.request.query['group.course.name'] = {
          operation: 'like',
          value: '*' + vm.searched.group.course.name.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.laboratory.name) {
        vm.request.query['laboratory.name'] = {
          operation: 'like',
          value: '*' + vm.searched.laboratory.name.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.state) {
        vm.request.query.state = {
          operation: 'like',
          value: vm.searched.state.value
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

      loadPage();
    }

    function getGroups() {
      var period = PeriodService.GetCurrentPeriod('Semestre');
      vm.groups_request.query = {};

      vm.groups_request.query['period.type'] = {
        operation: 'eq',
        value: 'Semestre'
      };

      vm.groups_request.query['period.value'] = {
        operation: 'eq',
        value: period.value
      };

      vm.groups_request.query['period.year'] = {
        operation: 'eq',
        value: period.year
      };

      vm.groups_request.query['professor.username'] = {
        operation: 'eq',
        value: vm.username
      };

      GroupService.GetAll(vm.groups_request)
      .then(setGroups)
      .catch(handleError);
    }

    function toggleAdvanceSearch() {
      vm.advanceSearch = !vm.advanceSearch;
      delete vm.searched.group;
      delete vm.searched.laboratory.name
      delete vm.searched.state;
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

    function setGroups (data) {
      vm.groups = data.results;
    }

    function setStates(states) {
      vm.states = states;
    }

    function deleteReservation(id) {
      MessageService.confirm('¿Desea realmente eliminar esta reservación?')
      .then(function() {
        ProfessorReservationService.Delete(vm.username, id)
        .then(loadPage)
        .catch(handleError);
      });
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
