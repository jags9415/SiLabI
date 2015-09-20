
(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('LabDetailController', LabDetail);

    LabDetail.$inject = ['$routeParams', '$location', 'LabService', 'MessageService'];

    function LabDetail($routeParams, $location, LabService, MessageService) {
      var vm = this;
      vm.lab = {};
      vm.id = $routeParams.id;
      vm.update = updateLab;
      vm.delete = deleteLab;

      activate();

      function activate() {
        LabService.GetOne(vm.id)
        .then(setLab)
        .catch(handleError);
      }

      function updateLab() {
        if (vm.id) {
          LabService.Update(vm.lab.id, vm.lab)
          .then(setLab)
          .catch(handleError);
        }
      }

      function deleteLab() {
        if (vm.lab) {
          MessageService.confirm("¿Desea realmente eliminar este laboratorio?")
          .then(function () {
            LabService.Delete(vm.lab.id)
            .then(redirectToLabs)
            .catch(handleError);
            }
          );
        }
      }

      function setLab(lab) {
        vm.lab = lab;
      }

      function redirectToLabs(result) {
        $location.path('/Operador/Laboratorios');
      }

      function handleError(data) {
        MessageService.error(data.description);
      }
    }
})();
