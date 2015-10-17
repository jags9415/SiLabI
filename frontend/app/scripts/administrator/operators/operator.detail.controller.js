(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorDetailController', OperatorDetailController);

    OperatorDetailController.$inject = ['$location', '$routeParams', 'OperatorService', 'MessageService', 'lodash'];

    function OperatorDetailController($location, $routeParams, OperatorService, MessageService, _) {
        var vm = this;
        vm.id = $routeParams.id;
        vm.user = {};
        vm.delete = deleteOperator;

        activate();

        function activate() {
          OperatorService.GetOne(vm.id)
          .then(setUser)
          .catch(handleError);
        }

        function deleteOperator() {
          if (!_.isEmpty(vm.user)) {
            MessageService.confirm('¿Desea realmente eliminar este operador?')
            .then(function() {
              OperatorService.Delete(vm.id)
              .then(redirectToOperators)
              .catch(handleError);
            });
          }
        }

        function setUser(user) {
          vm.user = user;
        }

        function redirectToOperators() {
          $location.path('/Administrador/Operadores');
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
