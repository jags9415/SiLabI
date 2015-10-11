(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('StudentsDetailController', StudentsDetail);

    StudentsDetail.$inject = ['$scope', '$routeParams', '$location', 'StudentService', 'GenderService', 'MessageService', 'CryptoJS'];

    function StudentsDetail($scope, $routeParams, $location, StudentService, GenderService, MessageService, CryptoJS) {
      var vm = this;
      vm.student = {};
      vm.username = $routeParams.username;
      vm.update = updateStudent;
      vm.delete = deleteStudent;

      activate();

      function activate() {
        GenderService.GetAll()
        .then(setGenders)
        .catch(handleError);

        StudentService.GetOne(vm.username)
        .then(setStudent)
        .catch(handleError);
      }

      function updateStudent() {
        if (vm.student) {
          if (vm.password) {
            var hash = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
            vm.student.password = hash;
          }
          StudentService.Update(vm.student.id, vm.student)
          .then(handleUpdate)
          .catch(handleError);
        }
      }

      function deleteStudent() {
        if (vm.student) {
          MessageService.confirm("¿Desea realmente eliminar este estudiante?")
          .then(function () {
            StudentService.Delete(vm.student.id)
            .then(redirectToStudents)
            .catch(handleError);
            }
          );
        }
      }

      function setGenders(genders) {
        vm.genders = genders;
        vm.student.gender = genders[0];
      }

      function setStudent(student) {
        vm.student = student;
      }

      function redirectToStudents(result) {
        $location.path('/Operador/Estudiantes');
      }

      function handleUpdate(student) {
        setStudent(student);
        MessageService.success("Estudiante actualizado.");
        $scope.$broadcast('show-errors-reset');
      }

      function handleError(data) {
        MessageService.error(data.description);
      }
    }
})();
