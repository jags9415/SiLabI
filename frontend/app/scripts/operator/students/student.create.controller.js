(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentsAddController', StudentsAddController);

    StudentsAddController.$inject = ['$location', '$localStorage', 'StudentService', 'MessageService'];

    function StudentsAddController($location, $localStorage, StudentService, MessageService) {
        var vm = this;

        vm.student = {};
        vm.genders = [
          { name: 'Masculino' },
          { name: 'Femenino' }
        ];
        vm.$storage = $localStorage;
        vm.user_type = vm.$storage['user_type'];

        vm.create = create;
        vm.cancel = cancel;

        function create() {
          vm.student.gender = vm.student.gender.name;
          var student = vm.student;
          StudentService.Create(student)
          .then(handleSuccess)
          .catch(handleError);
        }

        function handleSuccess(result) {
          MessageService.success("Estudiante creado con éxito.");
          vm.student = {};
        }

        function handleError(error) {
          MessageService.error(error.description);
        }

        function cancel() {
          $location.path('/' + vm.user_type );
        }
    }
})();
