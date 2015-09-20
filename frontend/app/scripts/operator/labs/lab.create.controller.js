(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('LabAddController', LabAddController);

    LabAddController.$inject = ['$scope', 'LabService', 'MessageService'];

    function LabAddController($scope, LabService, MessageService) {
        var vm = this;
        vm.lab = {};
        vm.create = create;

        function create() {
          if (vm.lab) {
            LabService.Create(vm.lab)
            .then(handleCreateSuccess)
            .catch(handleError);
          }
        }

        function handleCreateSuccess(result) {
          MessageService.success("Laboratorio creado con éxito.");

          // Reset form data.
          vm.lab = {};

          // Reset form validations.
          $scope.$broadcast('show-errors-reset');
        }

        function handleError(error) {
          MessageService.error(error.description);
        }
    }
})();
