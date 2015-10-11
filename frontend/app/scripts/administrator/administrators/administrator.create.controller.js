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
          vm.user = {};
          if (vm.username) {
            UserService.GetOne(vm.username)
            .then(handleSearchSuccess)
            .catch(handleError);
          }
        }

        function create() {
          if (!_.isEmpty(vm.user)) {
            AdminService.Create(vm.user.id)
            .then(handleCreateSuccess)
            .catch(handleError);
          }
        }

        function handleSearchSuccess(user) {
          vm.user = user;
        }

        function handleCreateSuccess() {
          MessageService.success("Administrador creado.");
          vm.user = {};
          delete vm.username;
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
