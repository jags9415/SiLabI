(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AdministratorCreateController', AdministratorCreateController);

    AdministratorCreateController.$inject = ['AdminService', 'UserService', 'MessageService'];

    function AdministratorCreateController(AdminService, UserService, MessageService) {
        var vm = this;
        vm.search = search;
        vm.create = create;

        function search() {
          if (vm.username) {
            UserService.GetOne(vm.username)
            .then(handleSearchSuccess)
            .catch(handleError);
          }
          else {
            vm.user = null;
          }
        }

        function create() {
          AdminService.Create(vm.user.id)
          .then(handleCreateSuccess)
          .catch(handleError);
        }

        function handleSearchSuccess(user) {
          vm.user = user;
        }

        function handleCreateSuccess() {
          MessageService.success("Administrador creado con éxito.");
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
