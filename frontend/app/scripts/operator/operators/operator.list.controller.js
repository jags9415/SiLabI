(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorListController', OperatorListController);

    OperatorListController.$inject = ['OperatorService', 'MessageService', '$location'];

    function OperatorListController(OperatorService, MessageService, $location) {
        var vm = this;
        vm.loaded = false;
        vm.current_page = 1;
        vm.total_pages = 1;
        vm.operators = [];
        vm.request = {};
        vm.request.fields = "id,full_name,username,state,period.value,period.year,period.type";
        vm.createOperator = createOperator;
        vm.open = openOperator;
        vm.delete = deleteOperator;
        vm.isEmpty = isEmpty;
        vm.isLoaded = isLoaded;

        activate();

        function activate() {
          loadOperators(vm.current_page);
        }

        function loadOperators(page) {
          OperatorService.GetAll(vm.request)
          .then(handleGetSuccess)
          .catch(handleError);
        }

        function handleDeleteSuccess() {
          MessageService.success("Operador eliminado.");
          loadOperators(vm.current_page);
        }

        function handleGetSuccess(data) {
          vm.operators = data.results;
          vm.current_page = data.current_page;
          vm.total_pages = data.total_pages;
          vm.loaded = true;
        }

        function handleError(data) {
          MessageService.error(data.description);
          vm.loaded = true;
        }

        function createOperator() {
          $location.path('/Operador/Operadores/Agregar');
        }

        function openOperator(id) {
          $location.path('/Operador/Operador/' + id);
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
