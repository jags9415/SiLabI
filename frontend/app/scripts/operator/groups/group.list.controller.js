(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('GroupListController', GroupListController);

    GroupListController.$inject = ['$scope', 'GroupService', 'MessageService', '$location', 'StateService', 'PeriodService'];

function GroupListController($scope, GroupService, MessageService, $location, StateService, PeriodService) {
    var vm = this;
    vm.advanceSearch = false;
    vm.loaded = false;
    vm.groups = [];
    vm.searched = {
      professor : {}
    };
    vm.limit = 20;
    vm.request = {
      fields : 'id,number,course.name,professor.full_name,period,state',
      sort: [
        {field: 'period.year', type: 'DESC'},
        {field: 'period.type', type: 'DESC'},
        {field: 'period.value', type: 'DESC'},
        {field: 'number', type: 'ASC'},
        {field: 'course.name', type: 'ASC'}
      ]
    };
    vm.states = [];
    vm.periods = [];
    vm.open = openGroup;
    vm.delete = deleteGroup;
    vm.search = searchGroup;
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

      StateService.GetLabStates()
      .then(setStates)
      .catch(handleError);

      PeriodService.GetAll(vm.request)
      .then(setPeriods)
      .catch(handleError);
    }

    function loadPage() {
      $location.search('page', vm.page);

      vm.request.page = vm.page;
      vm.request.limit = vm.limit;

      vm.promise = GroupService.GetAll(vm.request)
      .then(setGroups)
      .catch(handleError);
    }

    function openGroup(id) {
      $location.url('/Operador/Grupos/' + id);
    }

    function setStates(states) {
      vm.states = states;
    }

    function setPeriods(periods) {
      vm.periods = periods;
    }

    function searchGroup() {
      vm.request.query = {};

      if (vm.searched.professor.name) {
        vm.request.query['professor.full_name'] = {
          operation: 'like',
          value: '*' + vm.searched.professor.name.replace(' ', '*') + '*'
        };
      }

      if (vm.searched.state) {
        vm.request.query.state = {
          operation: 'eq',
          value: vm.searched.state.value
        };
      }

      if (vm.searched.period) {
        vm.request.query['period.value'] = {
          operation: 'eq',
          value: vm.searched.period.value
        };

        vm.request.query['period.type'] = {
          operation: 'eq',
          value: vm.searched.period.type
        };
      }

      if (vm.searched.year) {
        vm.request.query['period.year'] = {
          operation: 'eq',
          value: vm.searched.year
        };
      }

      if (vm.searched.name) {
        vm.request.query['course.name'] = {
          operation: 'like',
          value: '*' + vm.searched.name.replace(' ', '*') + '*'
        };
      }

      loadPage();
    }

    function isEmpty() {
      return vm.groups.length === 0;
    }

    function isLoaded() {
      return vm.loaded;
    }

    function toggleAdvanceSearch() {
      vm.advanceSearch = !vm.advanceSearch;
      delete vm.searched.professor.name;
      delete vm.searched.state;
      delete vm.searched.period;
      vm.searched.year = null;
    }

    function setGroups(data) {
      vm.groups = data.results;
      vm.page = data['current_page'];
      vm.totalPages = data['total_pages'];
      vm.totalItems = vm.limit * vm.totalPages;
      vm.loaded = true;
    }

    function deleteGroup(id) {
      MessageService.confirm('¿Desea realmente eliminar este grupo?')
      .then(function() {
        GroupService.Delete(id)
        .then(loadPage)
        .catch(handleError);
      });
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
