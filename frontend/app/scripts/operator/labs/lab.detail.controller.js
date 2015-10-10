
(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('LabDetailController', LabDetail);

    LabDetail.$inject = ['$routeParams', '$location', 'LabService', 'SoftwareService', 'MessageService'];

    function LabDetail($routeParams, $location, LabService, SoftwareService, MessageService) {
      var vm = this;
      vm.lab = {};
      vm.id = $routeParams.id;
      vm.software = [];
      vm.slicedSoftware = [];
      vm.page = 1;
      vm.limit = 20;

      vm.update = updateLab;
      vm.delete = deleteLab;
      vm.searchSoftware = searchSoftware;
      vm.deleteSoftware = deleteSoftware;
      vm.sliceSoftware = sliceSoftware;

      activate();

      function activate() {
        LabService.GetOne(vm.id)
        .then(setLab)
        .catch(handleError);

        LabService.GetSoftware(vm.id)
        .then(initSoftware)
        .catch(handleError);
      }

      function updateLab() {
        if (vm.id) {
          var software_codes = getAddedSoftware();
          LabService.Update(vm.lab.id, vm.lab, software_codes)
          .then(setLab)
          .catch(handleError);
        }
      }

      function deleteLab() {
        if (vm.lab) {
          MessageService.confirm("¿Desea realmente eliminar esta Sala de Laboratorio?")
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

      function initSoftware(software) {
        vm.software = software;
        sliceSoftware();
      }

      function redirectToLabs(result) {
        $location.path('/Operador/Laboratorios');
      }

      function handleError(data) {
        MessageService.error(data.description);
      }

      function searchSoftware() {
        if (vm.software_code) {
          SoftwareService.GetOne(vm.software_code)
          .then(setSoftware)
          .catch(handleError);
        }
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
          vm.software_code = "";
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

      function sliceSoftware() {
        vm.slicedSoftware = vm.software.slice((vm.page - 1) * vm.limit, vm.page * vm.limit);
      }
    }
})();
