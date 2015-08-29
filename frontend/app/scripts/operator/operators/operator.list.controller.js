(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorListController', OperatorListController);

    OperatorListController.$inject = ['OperatorService', 'MessageService'];

    function OperatorListController(OperatorService, MessageService) {
        var vm = this;
        vm.loaded = false;
        vm.current_page = 1;
        vm.total_pages = 1;
        vm.operators = [];
        vm.request = {};
        vm.open = openOperator;
        vm.delete = deleteOperator;
        vm.isEmpty = isEmpty;
        vm.isLoaded = isLoaded;

        activate();

        function activate() {
          loadOperators(1);
        }

        function loadOperators(page) {
          OperatorService.GetAll(vm.request)
          .then(handleSuccess)
          .catch(handleError);
        }

        function handleSuccess(data) {
          vm.operators = data.results;
          vm.current_page = data.current_page;
          vm.total_pages = data.total_pages;
          vm.loaded = true;
        }

        function handleError(data) {
          MessageService.error(data.description);
          vm.loaded = true;
        }

        function openOperator(id) {
          $location.path('/Operador/Operadores/' + id);
        }

        function deleteOperator(id) {
          MessageService.warning("Función no implementada.");
        }

        function isEmpty() {
          return vm.operators.length == 0;
        }

        function isLoaded() {
          return vm.loaded;
        }
    }
})();
