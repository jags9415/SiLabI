(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('LabAddController', LabAddController);

    LabAddController.$inject = ['$scope', 'LabService', 'SoftwareService', 'MessageService'];

    function LabAddController($scope, LabService, SoftwareService, MessageService) {
        var vm = this;
        vm.lab = {};
        vm.software = [];

        vm.create = create;

        vm.searchSoftware = searchSoftware;
        vm.deleteSoftware = deleteSoftware;

        function create() {
          if (vm.lab) {
            vm.lab.software = getAddedSoftware();
            LabService.Create(vm.lab)
            .then(handleCreateSuccess)
            .catch(handleError);
          }
        }

        function handleCreateSuccess(result) {
          MessageService.success("Sala de Laboratorio creada con éxito.");

          // Reset form data.
          vm.lab = {};
          vm.software = [];

          // Reset form validations.
          $scope.$broadcast('show-errors-reset');
        }

        function handleError(error) {
          MessageService.error(error.description);
        }

        function searchSoftware() {
          if (vm.software_code) {
            SoftwareService.GetOne(vm.software_code)
            .then(setSoftware)
            .catch(handleError);
          }
          vm.software_code = "";
        }

        function contains(software) {
          for (var i = 0; i < vm.software.length; i++){
            if(vm.software[i].code == software.code)
              return true;
          }
          return false;
        }

        function setSoftware(software) {
          if (!contains(software)) {
            vm.software.push(software);
          }
          else {
            MessageService.info("El software seleccionado ya se encuentra en la lista.")
          }
        }

        function getAddedSoftware() {
          var software_codes = [];
          for (var i = 0; i < vm.software.length; i++){
            software_codes.push(vm.software[i].code);
          }
          return software_codes;
        }

        function deleteSoftware(code) {
          for (var i = 0; i < vm.software.length; i++){
            if(vm.software[i].code === code){
              vm.software.splice(i, 1);
              break;
            }
          }
        }
    }
})();
