(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AdministratorDetailController', AdministratorDetailController);

    AdministratorDetailController.$inject = ['$location', '$routeParams', 'AdminService', 'MessageService', 'lodash'];

    function AdministratorDetailController($location, $routeParams, AdminService, MessageService, _) {
        var vm = this;
        vm.id = $routeParams.id;
        vm.user = {};
        vm.delete = deleteAdministrator;

        activate();

        function activate() {
          AdminService.GetOne(vm.id)
          .then(handleGetSuccess)
          .catch(handleError);
        }

        function deleteAdministrator() {
          if (!_.isEmpty(vm.user)) {
            MessageService.confirm('¿Desea realmente eliminar este administrador?')
            .then(function() {
              AdminService.Delete(vm.id)
              .then(handleDeleteSuccess)
              .catch(handleError);
            });
          }
        }

        function handleGetSuccess(data) {
          vm.user = data;
        }

        function handleDeleteSuccess() {
          $location.path('/Administrador/Administradores');
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
