(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentsAddController', StudentsAddController);

    StudentsAddController.$inject = ['$location', '$localStorage', 'StudentService', 'MessageService', 'CryptoJS'];

    function StudentsAddController($location, $localStorage, StudentService, MessageService, CryptoJS) {
        var vm = this;

        vm.student = {};
        vm.genders = ['Masculino', 'Femenino'];
        vm.student.gender = vm.genders[0];
        vm.$storage = $localStorage;

        vm.create = create;

        function create() {
          if (vm.student) {
            var hash = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
            vm.student.password = hash;
            StudentService.Create(vm.student)
            .then(handleSuccess)
            .catch(handleError);
          }
        }

        function handleSuccess(result) {
          MessageService.success("Estudiante creado con éxito.");
          vm.student = {};
          vm.student.gender = vm.genders[0];
          vm.password = null;
          vm.password_confirm = null;
        }

        function handleError(error) {
          MessageService.error(error.description);
        }
    }
})();
