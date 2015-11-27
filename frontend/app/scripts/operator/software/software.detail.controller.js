(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('SoftwareDetailController', SoftwareDetailController);

  SoftwareDetailController.$inject = ['$routeParams', '$location', '$scope', 'SoftwareService', 'MessageService', 'StateService', 'lodash'];

  function SoftwareDetailController($routeParams, $location, $scope, SoftwareService, MessageService, StateService, _) {
    var vm = this;
    vm.code = $routeParams.code;
    vm.update = updateSoftware;
    vm.delete = deleteSoftware;

    activate();

    function activate() {

      StateService.GetLabStates()
      .then(setStates)
      .catch(handleError);

      SoftwareService.GetOne(vm.code)
      .then(setSoftware)
      .catch(handleError);
    }

    function updateSoftware() {
      if (!_.isEmpty(vm.software)) {
        SoftwareService.Update(vm.software.id, vm.software)
        .then(handleUpdateSuccess)
        .catch(handleError);
      }
    }

    function deleteSoftware() {
      if (!_.isEmpty(vm.software)) {
        MessageService.confirm('¿Desea realmente eliminar este software?')
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

    function setStates(States) {
      vm.states = [];
      for (var i = 0; i < States.length; i++) {
        if(States[i].value  != "*")
        {
          vm.states.push(States[i].value);
        }
      };
    }

    function redirectToSoftware() {
      $location.path('/Operador/Software');
    }

    function handleUpdateSuccess(software) {
      setSoftware(software);
      MessageService.success('Software actualizado.');
      $scope.$broadcast('show-errors-reset');
    }

    function handleError(error) {
      MessageService.error(error.description);
    }
  }
})();
