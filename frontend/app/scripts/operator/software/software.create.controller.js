(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('SoftwareCreateController', SoftwareCreateController);

    SoftwareCreateController.$inject = ['$scope', 'SoftwareService', 'MessageService'];

    function SoftwareCreateController($scope, SoftwareService, MessageService) {
        var vm = this;
        vm.software = {};
        vm.create = create;

        function create() {
          if (vm.software) {
            SoftwareService.Create(vm.software)
            .then(handleCreateSuccess)
            .catch(handleError);
          }
        }

        function handleCreateSuccess(result) {
          MessageService.success("Software creado con éxito.");
          vm.software = {};
          $scope.$broadcast('show-errors-reset');
        }

        function handleError(error) {
          MessageService.error(error.description);
        }
    }
})();
