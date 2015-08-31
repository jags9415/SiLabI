(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorDetailController', OperatorDetailController);

    OperatorDetailController.$inject = ['$location', '$routeParams', 'OperatorService', 'MessageService'];

    function OperatorDetailController($location, $routeParams, OperatorService, MessageService) {
        var vm = this;
        vm.id = $routeParams.id;
        vm.user = {};
        vm.delete = deleteOperator;

        activate();

        function activate() {
          OperatorService.GetOne(vm.id)
          .then(handleGetSuccess)
          .catch(handleError);
        }

        function deleteOperator() {
          if (vm.user) {
            MessageService.confirm("¿Desea realmente eliminar este operador?")
            .then(function() {
              OperatorService.Delete(vm.id)
              .then(handleDeleteSuccess)
              .catch(handleError);
            });
          }
        }

        function handleGetSuccess(data) {
          vm.user = data;
        }

        function handleDeleteSuccess() {
          $location.path('/Administrador/Operadores')
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
