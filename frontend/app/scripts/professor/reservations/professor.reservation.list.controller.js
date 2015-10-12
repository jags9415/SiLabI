(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ProfessorReservationListController', ProfessorReservationListController);

   ProfessorReservationListController.$inject = ['$scope', 'ProfessorReservationService', 'MessageService', 'StateService', '$location', '$localStorage'];

  function ProfessorReservationListController($scope, ProfessorReservationService, MessageService, StateService, $location, $localStorage) {
    var vm = this;
    vm.advanceSearch = false;
    vm.loaded = false;
    vm.reservations = [];
    vm.searched = {
      laboratory: {},
      group: { course: {} }
    };
    vm.limit = 20;
    vm.request = {
      fields : "id,laboratory,state,software,group,start_time,end_time"
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

    activate();

    function activate() {
      var page = parseInt($location.search()['page']);

      if (isNaN(page)) {
        page = 1;
      }

      vm.totalPages = page;
      vm.page = page;
      loadPage();

      StateService.GetReservationStates()
    .then(setStates)
    .catch(handleError);
    }

    function loadPage() {
      $location.search('page', vm.page);

      vm.request.page = vm.page;
      vm.request.limit = vm.limit;
      if(vm.$storage['username'])
      {
        vm.username = vm.$storage['username'];
      }

      ProfessorReservationService.GetAll(vm.username, vm.request)
      .then(setReservations)
      .catch(handleError);
    }

    function openReservation(id) {
      $location.url('/Docente/Reservaciones/' + id);
    }

    function searchReservation() {
      vm.request.query = {};

      if (vm.searched.group.course.name) {
        vm.request.query['group.course.name'] = {
          operation: "like",
          value: '*' + vm.searched.group.course.name.replace(' ', '*') + '*'
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
    }

    if (vm.searched.software) {
      vm.request.query['software.code'] = {
        operation: "like",
        value: '*' + vm.searched.software.replace(' ', '*') + '*'
      }
    }

    if (vm.searched.start_time) {
      if(vm.searched.hour)
        {
          var date = new Date(vm.searched.start_time.getFullYear(), vm.searched.start_time.getMonth(), vm.searched.start_time.getUTCDate(), vm.searched.hour.slice(0, 2));
          console.log(vm.searched.hour.slice(0, 2));
          vm.request.query.start_time = {
            operation: "eq",
            value: date.toJSON()
          }
        }
        else
        {
          var date = new Date(vm.searched.start_time.getFullYear(), vm.searched.start_time.getMonth(), vm.searched.start_time.getUTCDate(), "18");
          vm.request.query.start_time = {
          operation: "le",
          value: date.toJSON()
          }
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
        ProfessorReservationService.Delete(id)
        .then(loadPage)
        .catch(handleError);
      });
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
