(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorListController', OperatorListController);

    OperatorListController.$inject = ['$location', 'OperatorService', 'StateService', 'PeriodService', 'MessageService'];

    function OperatorListController($location, OperatorService, StateService, PeriodService, MessageService) {
        var vm = this;

        vm.loaded = false;
        vm.operators = [];
        vm.searched = {};
        vm.limit = 20;
        vm.advanceSearch = false;

        vm.request = {
          fields : 'id,full_name,username,email,phone,state,period',
          sort: [
            {field: 'period.year', type: 'DESC'},
            {field: 'period.type', type: 'DESC'},
            {field: 'period.value', type: 'DESC'},
            {field: 'full_name', type: 'ASC'},
          ]
        };

        vm.createOperator = createOperator;
        vm.open = openOperator;
        vm.delete = deleteOperator;
        vm.search = searchOperators;
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
          loadPage(true);

          StateService.GetOperatorStates()
          .then(setStates)
          .catch(handleError);

          PeriodService.GetAll()
          .then(setPeriods)
          .catch(handleError);
        }

        function loadPage(cached) {
          $location.search('page', vm.page);

          vm.request.page = vm.page;
          vm.request.limit = vm.limit;

          vm.promise = OperatorService.GetAll(vm.request, cached)
          .then(setOperators)
          .catch(handleError);
        }

        function createOperator() {
          $location.path('/Administrador/Operadores/Agregar');
        }

        function openOperator(id) {
          $location.path('/Administrador/Operadores/' + id);
        }

        function deleteOperator(id) {
          MessageService.confirm('¿Desea realmente eliminar este operador?')
          .then(function() {
            OperatorService.Delete(id)
            .then(loadPage)
            .catch(handleError);
          });
        }

        function searchOperators() {
          vm.request.query = {};

          if (vm.searched.fullName) {
            vm.request.query['full_name'] = {
              operation: 'like',
              value: '*' + vm.searched.fullName.replace(' ', '*') + '*'
            };
          }

          if (vm.searched.username) {
            vm.request.query['username'] = {
              operation: 'like',
              value: '*' + vm.searched.username + '*'
            };
          }

          if (vm.searched.state) {
            vm.request.query['state'] = {
              operation: 'like',
              value: vm.searched.state.value
            };
          }

          if (vm.searched.period) {
            if (vm.searched.period.value) {
              vm.request.query['period.value'] = {
                operation: 'eq',
                value: vm.searched.period.value
              };
            }
            if (vm.searched.period.type) {
              vm.request.query['period.type'] = {
                operation: 'eq',
                value: vm.searched.period.type
              };
            }
          }

          if (vm.searched.year) {
            vm.request.query['period.year'] = {
              operation: 'eq',
              value: vm.searched.year
            };
          }

          loadPage();
        }

        function toggleAdvanceSearch() {
          vm.advanceSearch = !vm.advanceSearch;
          delete vm.searched.state;
          delete vm.searched.period;
          delete vm.searched.username;
          vm.searched.year = null;
        }

        function isEmpty() {
          return vm.operators.length === 0;
        }

        function isLoaded() {
          return vm.loaded;
        }

        function setOperators(data) {
          vm.operators = data['results'];
          vm.page = data['current_page'];
          vm.totalPages = data['total_pages'];
          vm.totalItems = vm.limit * vm.totalPages;
          vm.loaded = true;
        }

        function setStates(states) {
          vm.states = states;
        }

        function setPeriods(periods) {
          vm.periods = periods;
        }

        function handleError(data) {
          MessageService.error(data.description);
          vm.loaded = true;
        }
    }
})();
