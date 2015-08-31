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
        vm.user_type = vm.$storage['user_type'];

        vm.create = create;
        vm.cancel = cancel;

        function create() {
          if (vm.student) {
            var hashedPassword = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
            vm.student.password = hashedPassword;
            StudentService.Create(vm.student)
            .then(handleSuccess)
            .catch(handleError);

          }
        }

        function handleSuccess(result) {
          MessageService.success("Estudiante creado con éxito.");
          vm.student = {};
          vm.password = '';
        }

        function handleError(error) {
          MessageService.error(error.description);
        }

        function cancel() {
          $location.path('/' + vm.user_type );
        }
    }
})();
