(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('SoftwareDetailController', SoftwareDetailController);

  SoftwareDetailController.$inject = ['$routeParams', '$location', '$scope', 'SoftwareService', 'MessageService'];

  function SoftwareDetailController($routeParams, $location, $scope, SoftwareService, MessageService) {
    var vm = this;
    vm.code = $routeParams.code;
    vm.update = updateSoftware;
    vm.delete = deleteSoftware;

    activate();

    function activate() {
      SoftwareService.GetOne(vm.code)
      .then(setSoftware)
      .catch(handleError);
    }

    function updateSoftware() {
      if (vm.software) {
        SoftwareService.Update(vm.software.id, vm.software)
        .then(handleUpdateSuccess)
        .catch(handleError);
      }
    }

    function deleteSoftware() {
      if (vm.software) {
        MessageService.confirm("¿Desea realmente eliminar este software?")
        .then(function() {
          SoftwareService.Delete(vm.software.id)
          .then(redirectToSoftware)
          .catch(handleError);
          }
        );
      }
    }

    function setSoftware(software) {
      vm.software = software;
    }

    function redirectToSoftware() {
      $location.path('/Operador/Software');
    }

    function handleUpdateSuccess(software) {
      setSoftware(software);
      MessageService.success("Software actualizado.");
      $scope.$broadcast('show-errors-reset');
    }

    function handleError(error) {
      MessageService.error(error.description);
    }
  }
})();
