(function() {
  'use strict';

  angular
    .module('silabi')
    .controller('LabListController', LabListController);

  LabListController.$inject = ['$location', 'LabService', 'MessageService', 'StateService'];

  function LabListController($location, LabService, MessageService, StateService) {
    var vm = this;

    vm.loaded = false;
    vm.advanceSearch = false;
    vm.labs = [];
    vm.searched = {};
    vm.limit = 20;
    vm.request = {
      fields : "id,name,seats,state,appointment_priority,reservation_priority",
      sort: {field: "name", type: "ASC"}
    };

    vm.open = openLab;
    vm.delete = deleteLab;
    vm.search = searchLab;
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

      vm.priorities = LabService.GetPriorities();
      vm.totalPages = page;
      vm.page = page;
      loadPage();

      StateService.GetLabStates()
      .then(setStates)
      .catch(handleError);
    }

    function loadPage() {
      $location.search('page', vm.page);

      vm.request.page = vm.page;
      vm.request.limit = vm.limit;

      vm.promise = LabService.GetAll(vm.request)
      .then(setLabs)
      .catch(handleError);
    }

    function openLab(id) {
      $location.url('/Operador/Laboratorios/' + id);
    }

    function searchLab() {
      vm.request.query = {}

      if (vm.searched.name) {
        vm.request.query.name = {
          operation: "like",
          value: '*' + vm.searched.name.replace(' ', '*') + '*'
        }
      }

      if (vm.searched.seats) {
        vm.request.query.seats = {
          operation: "eq",
          value: vm.searched.seats
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

    function isEmpty() {
      return vm.labs.length == 0;
    }

    function isLoaded() {
      return vm.loaded;
    }

    function toggleAdvanceSearch() {
      vm.advanceSearch = !vm.advanceSearch;
      delete vm.searched.seats;
      delete vm.searched.name;
      delete vm.searched.state;
    }

    function setLabs(data) {
      vm.labs = data.results;
      vm.page = data.current_page;
      vm.totalPages = data.total_pages;
      vm.totalItems = vm.limit * vm.totalPages;
      vm.loaded = true;

      _.each(vm.labs, function(lab) {
        lab.appointment_priority = _.find(vm.priorities, function(p) { return p.value == lab.appointment_priority });
        lab.reservation_priority = _.find(vm.priorities, function(p) { return p.value == lab.reservation_priority });
      });
    }

    function deleteLab(id) {
      MessageService.confirm("¿Desea realmente eliminar esta Sala de Laboratorio?")
      .then(function() {
      LabService.Delete(id)
      .then(loadPage)
      .catch(handleError);
      });
    }

    function setStates(states) {
      vm.states = states;
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
