(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('SoftwareDetailController', SoftwareDetailController);

  SoftwareDetailController.$inject = ['$routeParams', '$location', '$scope', 'SoftwareService', 'MessageService'];

  function SoftwareDetailController($routeParams, $location, $scope, SoftwareService, MessageService) {
    var vm = this;
    vm.software = {};
    vm.code = $routeParams.code;
    vm.update = update;
    vm.delete = deleteSoftware;
    activate();

    function activate()
    {
      SoftwareService.GetOne(vm.code)
      .then(setSoftware)
    .catch(handleError);
    }

    function update() {
      if (vm.software) {
        SoftwareService.Update(vm.software.id, vm.software)
        .then(handleUpdateSuccess)
        .catch(handleError);
      }
    }

    function deleteSoftware() {
    if (vm.software) {
      MessageService.confirm("¿Desea realmente eliminar este software?")
      .then(function () {
        SoftwareService.Delete(vm.software.id)
        .then(redirectToSoftware)
        .catch(handleError);
        }
      );
    }
  }

    function setSoftware (data) {
      vm.software = data;
    }

    function redirectToSoftware(result) {
    $location.path('/Operador/Software');
  }

    function handleUpdateSuccess(result) {
      $location.url('/Operador/Software/' + result.code);
      MessageService.success("Software actualizado con éxito.");
      vm.software = {};
    }

    function handleError(error) {
      MessageService.error(error.description);
    }
  }
})();
