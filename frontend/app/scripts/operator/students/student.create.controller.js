(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentsAddController', StudentsAddController);

    StudentsAddController.$inject = ['$scope', 'StudentService', 'MessageService', 'GenderService', 'CryptoJS'];

    function StudentsAddController($scope, StudentService, MessageService, GenderService, CryptoJS) {
        var vm = this;
        vm.student = {};
        vm.create = create;

        activate();

        function activate() {
          GenderService.GetAll()
          .then(setGenders)
          .catch(handleError);
        }

        function setGenders(genders) {
          vm.genders = genders;
          vm.student.gender = genders[0];
        }

        function create() {
          if (vm.student) {
            var hash = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
            vm.student.password = hash;
            StudentService.Create(vm.student)
            .then(handleCreateSuccess)
            .catch(handleError);
          }
        }

        function handleCreateSuccess(result) {
          MessageService.success("Estudiante creado con éxito.");

          // Reset form data.
          vm.student = {};
          vm.student.gender = vm.genders[0];
          delete vm.password;
          delete vm.password_confirm;

          // Reset form validations.
          $scope.$broadcast('show-errors-reset');
        }

        function handleError(error) {
          MessageService.error(error.description);
        }
    }
})();
