
(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('LabDetailController', LabDetail);

    LabDetail.$inject = ['$scope', '$routeParams', '$location', 'LabService', 'SoftwareService', 'MessageService'];

    function LabDetail($scope, $routeParams, $location, LabService, SoftwareService, MessageService) {
      var vm = this;
      vm.lab = {};
      vm.id = $routeParams.id;
      vm.software = [];
      vm.slicedSoftware = [];
      vm.isSoftwareModified = false;
      vm.page = 1;
      vm.limit = 15;

      vm.update = updateLab;
      vm.delete = deleteLab;
      vm.searchSoftware = searchSoftware;
      vm.deleteSoftware = deleteSoftware;
      vm.sliceSoftware = sliceSoftware;

      vm.softwareRequest = {
        fields: "id,code,name"
      }

      activate();

      function activate() {
        LabService.GetOne(vm.id)
        .then(setLaboratory)
        .catch(handleError);

        LabService.GetSoftware(vm.id, vm.softwareRequest)
        .then(setSoftware)
        .catch(handleError);
      }

      function updateLab() {
        if (!_.isEmpty(vm.lab)) {
          if (vm.isSoftwareModified) {
            vm.lab.software = getSoftwareCodes();
          }
          LabService.Update(vm.lab.id, vm.lab)
          .then(updateLaboratory)
          .catch(handleError);
        }
      }

      function deleteLab() {
        if (!_.isEmpty(vm.lab)) {
          MessageService.confirm("¿Desea realmente eliminar esta Sala de Laboratorio?")
          .then(function () {
            LabService.Delete(vm.lab.id)
            .then(redirectToLabs)
            .catch(handleError);
            }
          );
        }
      }

      function setLaboratory(lab) {
        vm.lab = lab;
      }

      function updateLaboratory(lab) {
        setLaboratory(lab);
        $scope.$broadcast('show-errors-reset');
        vm.isSoftwareModified = false;
        MessageService.success("Laboratorio actualizado.");
      }

      function contains(software) {
        return _.any(vm.software, _.matches(software));
      }

      function redirectToLabs(result) {
        $location.path('/Operador/Laboratorios');
      }

      function handleError(data) {
        MessageService.error(data.description);
      }

      function searchSoftware() {
        if (vm.software_code) {
          SoftwareService.GetOne(vm.software_code, vm.softwareRequest)
          .then(addSoftware)
          .catch(handleError);
        }
      }

      function setSoftware(software) {
        vm.software = software;
        sliceSoftware();
      }

      function addSoftware(software) {
        if (!contains(software)) {
          vm.software.unshift(software);
          vm.software_code = "";
          vm.isSoftwareModified = true;
          sliceSoftware();
        }
        else {
          MessageService.info("El software seleccionado ya se encuentra en la lista.")
        }
      }

      function deleteSoftware(code) {
        for (var i = 0; i < vm.software.length; i++){
          if(vm.software[i].code === code){
            vm.software.splice(i, 1);
            vm.isSoftwareModified = true;
            sliceSoftware();
            break;
          }
        }
      }

      function getSoftwareCodes() {
        return _.map(vm.software, 'code');
      }

      function sliceSoftware() {
        vm.slicedSoftware = vm.software.slice((vm.page - 1) * vm.limit, vm.page * vm.limit);
      }
    }
})();
