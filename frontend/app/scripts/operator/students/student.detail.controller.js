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
        vm.StudentID;
        vm.requestForUser = requestForUser;
        vm.updateInfo = updateInfo;
        vm.delete = deleteStudent;
        vm.genders = [
          { name: 'Masculino' },
          { name: 'Femenino' }
        ];

        function requestForUser() {
          var username = vm.username;
          StudentService.GetOne(username)
          .then(handleGetOneSuccess)
          .catch(handleRequestError);
        }

        function handleGetOneSuccess(result) {
          vm.student = result;
          vm.StudentID = result.id;
          return vm.student;
        }

        function updateInfo() {
          var newUserInfo = vm.student;
          var StudentID = vm.StudentID;
          StudentService.Update(StudentID, newUserInfo)
          .then(function(result) {
            vm.student = result;
          })
          .catch(showError);
        }

        function deleteStudent() {
          var StudentID = vm.StudentID;
          StudentService.Delete(StudentID)
          .then(handleDeleteSuccess)
          .catch(handleRequestError);
        }

        function handleDeleteSuccess(result) {
          MessageService.success("Estudiante eliminado.");
          $location.path('/Operador/Estudiantes');
        }

        function handleRequestError(data) {
          if (data.status === 404)
            MessageService.error("No se pudo conectar con el servidor");
          else
            MessageService.error(data.description);
        }

    }
})();
