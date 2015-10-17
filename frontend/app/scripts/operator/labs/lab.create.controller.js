(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('LabAddController', LabAddController);

    LabAddController.$inject = ['$scope', 'LabService', 'SoftwareService', 'MessageService', 'lodash'];

    function LabAddController($scope, LabService, SoftwareService, MessageService, _) {
        var vm = this;

        vm.lab = {};
        vm.priorities = [];
        vm.software = [];
        vm.slicedSoftware = [];
        vm.isSoftwareModified = false;
        vm.page = 1;
        vm.limit = 15;

        vm.create = create;
        vm.searchSoftware = searchSoftware;
        vm.deleteSoftware = deleteSoftware;
        vm.sliceSoftware = sliceSoftware;

        activate();

        function activate() {
          vm.priorities = LabService.GetPriorities();
        }

        function create() {
          if (!_.isEmpty(vm.lab) && !_.isEmpty(vm['appointment_priority']) && !_.isEmpty(vm['reservation_priority'])) {
            vm.lab['software'] = getSoftwareCodes();
            vm.lab['appointment_priority'] = vm['appointment_priority'].value;
            vm.lab['reservation_priority'] = vm['reservation_priority'].value;

            LabService.Create(vm.lab)
            .then(handleCreateSuccess)
            .catch(handleError);
          }
        }

        function handleCreateSuccess(result) {
          MessageService.success('Laboratorio creado.');

          // Reset form data.
          vm.lab = {};
          vm.software = [];
          delete vm['appointment_priority'];
          delete vm['reservation_priority'];
          vm.isSoftwareModified = false;

          // Reset form validations.
          $scope.$broadcast('show-errors-reset');
        }

        function handleError(error) {
          MessageService.error(error.description);
        }

        function searchSoftware() {
          if (vm.softwareCode) {
            SoftwareService.GetOne(vm.softwareCode)
            .then(addSoftware)
            .catch(handleError);
          }
        }

        function contains(software) {
          return _.any(vm.software, _.matches(software));
        }

        function addSoftware(software) {
          if (!contains(software)) {
            vm.software.unshift(software);
            vm.softwareCode = '';
            vm.isSoftwareModified = true;
            sliceSoftware();
          }
          else {
            MessageService.info('El software seleccionado ya se encuentra en la lista.');
          }
        }

        function deleteSoftware(code) {
          for (var i = 0; i < vm.software.length; i++) {
            if (vm.software[i].code === code) {
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
