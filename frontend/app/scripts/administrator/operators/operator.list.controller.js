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
        vm.limit = 20;
        vm.request = {
          fields : "id,full_name,username,state,period.value,period.year,period.type"
        };

        vm.createOperator = createOperator;
        vm.open = openOperator;
        vm.delete = deleteOperator;
        vm.isEmpty = isEmpty;
        vm.isLoaded = isLoaded;
        vm.loadPage = loadPage;

        activate();

        function activate() {
          var page = parseInt($location.search()['page']);

          if (isNaN(page)) {
            page = 1;
          }

          vm.totalPages = page;
          vm.page = page;
          loadPage();
        }

        function loadPage() {
          $location.search('page', vm.page);

          vm.request.page = vm.page;
          vm.request.limit = vm.limit;

          OperatorService.GetAll(vm.request)
          .then(handleGetSuccess)
          .catch(handleError);
        }

        function handleDeleteSuccess() {
          MessageService.success("Operador eliminado.");
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

        function createOperator() {
          $location.path('/Administrador/Operadores/Agregar');
        }

        function openOperator(id) {
          $location.path('/Administrador/Operadores/' + id);
        }

        function deleteOperator(id) {
          OperatorService.Delete(id)
          .then(handleDeleteSuccess)
          .catch(handleError);
        }

        function isEmpty() {
          return vm.operators.length == 0;
        }

        function isLoaded() {
          return vm.loaded;
        }
    }
})();
