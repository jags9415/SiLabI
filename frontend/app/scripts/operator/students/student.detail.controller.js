(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentsDetailController', StudentsDetail);

    StudentsDetail.$inject = ['$routeParams', '$location', 'StudentService', 'MessageService'];

    function StudentsDetail($routeParams, $location, StudentService, MessageService) {
      var vm = this;
        vm.student = {};
        vm.username = $routeParams.username;
        vm.updateInfo = updateInfo;
        vm.delete = deleteStudent;
        vm.genders = ['Masculino', 'Femenino'];

        activate();

        function activate() {
          StudentService.GetOne(vm.username)
          .then(handleGetOneSuccess)
          .catch(handleRequestError);
        }

        function handleGetOneSuccess(result) {
          vm.student = result;
        }

        function updateInfo() {
          StudentService.Update(vm.student.id, vm.student)
          .then(function(result) {
            vm.student = result;
          })
          .catch(showError);
        }

        function deleteStudent() {
          if (vm.student) {
            MessageService.confirm("¿Desea realmente eliminar este estudiante?")
            .then(function (argument) {
              StudentService.Delete(vm.student.id)
              .then(handleDeleteSuccess)
              .catch(handleRequestError);
              }
            );
          }
        }

        function handleDeleteSuccess(result) {
          $location.path('/Operador/Estudiantes');
        }

        function handleRequestError(data) {
          if (data.code === 404)
            MessageService.error("No se pudo conectar con el servidor");
          else
            MessageService.error(data.description);
        }
    }
})();
