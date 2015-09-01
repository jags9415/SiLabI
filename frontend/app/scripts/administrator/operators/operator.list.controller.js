(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorListController', OperatorListController);

    OperatorListController.$inject = ['OperatorService', 'MessageService', '$location'];

    function OperatorListController(OperatorService, MessageService, $location) {
        var vm = this;
        vm.loaded = false;
        vm.operators = [];
        vm.searched = {};
        vm.limit = 20;
        vm.advanceSearch = false;
        vm.request = {
          fields : "id,full_name,username,email,phone,state,period.value,period.year,period.type"
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
          loadPage();

          vm.states = [
            {
              name: 'Cualquiera',
              value: '*'
            },
            {
              name: 'Activo',
              value: 'Activo'
            },
            {
              name: 'Inactivo',
              value: 'Inactivo'
            }
          ];

          vm.periods = [
            {
              value: 1,
              type: 'Semestre'
            },
            {
              value: 2,
              type: 'Semestre'
            },
            {
              value: 1,
              type: 'Cuatrimestre'
            },
            {
              value: 2,
              type: 'Cuatrimestre'
            },
            {
              value: 3,
              type: 'Cuatrimestre'
            },
            {
              value: 1,
              type: 'Trimestre'
            },
            {
              value: 2,
              type: 'Trimestre'
            },
            {
              value: 3,
              type: 'Trimestre'
            },
            {
              value: 4,
              type: 'Trimestre'
            },
            {
              value: 1,
              type: 'Bimestre'
            },
            {
              value: 2,
              type: 'Bimestre'
            },
            {
              value: 3,
              type: 'Bimestre'
            },
            {
              value: 4,
              type: 'Bimestre'
            },
            {
              value: 5,
              type: 'Bimestre'
            },
            {
              value: 6,
              type: 'Bimestre'
            }
          ];
        }

        function loadPage() {
          $location.search('page', vm.page);

          vm.request.page = vm.page;
          vm.request.limit = vm.limit;

          OperatorService.GetAll(vm.request)
          .then(handleGetSuccess)
          .catch(handleError);
        }

        function createOperator() {
          $location.path('/Administrador/Operadores/Agregar');
        }

        function openOperator(id) {
          $location.path('/Administrador/Operadores/' + id);
        }

        function deleteOperator(id) {
          MessageService.confirm("¿Desea realmente eliminar este operador?")
          .then(function() {
            OperatorService.Delete(id)
            .then(handleDeleteSuccess)
            .catch(handleError);
          });
        }

        function searchOperators() {
          vm.request.query = {};

          if (vm.searched.full_name) {
            vm.request.query.full_name = {
              operation: "like",
              value: '*' + vm.searched.full_name.replace(' ', '*') + '*'
            }
          }

          if (vm.searched.username) {
            vm.request.query.username = {
              operation: "like",
              value: '*' + vm.searched.username + '*'
            }
          }

          if (vm.searched.state) {
            vm.request.query.state = {
              operation: "like",
              value: vm.searched.state.value
            }
          }

          if (vm.searched.period) {
            if (vm.searched.period.value) {
              vm.request.query["period.value"] = {
                operation: "eq",
                value: vm.searched.period.value
              }
            }
            if (vm.searched.period.type) {
              vm.request.query["period.type"] = {
                operation: "eq",
                value: vm.searched.period.type
              }
            }
          }

          if (vm.searched.year) {
            vm.request.query["period.year"] = {
              operation: "eq",
              value: vm.searched.year
            }
          }

          loadPage();
        }

        function toggleAdvanceSearch() {
          vm.advanceSearch = !vm.advanceSearch;
          delete vm.searched.state;
          delete vm.searched.year;
          delete vm.searched.period;
          delete vm.searched.username;
        }

        function isEmpty() {
          return vm.operators.length == 0;
        }

        function isLoaded() {
          return vm.loaded;
        }

        function handleDeleteSuccess() {
          loadPage();
        }

        function handleGetSuccess(data) {
          vm.operators = data.results;
          vm.page = data.current_page;
          vm.totalPages = data.total_pages;
          vm.totalItems = vm.limit * vm.totalPages;
          vm.loaded = true;
        }

        function handleError(data) {
          MessageService.error(data.description);
          vm.loaded = true;
        }
    }
})();
