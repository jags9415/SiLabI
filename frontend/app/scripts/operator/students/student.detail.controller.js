(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('StudentsDetailController', StudentsDetail);

    StudentsDetail.$inject = ['$scope', '$routeParams', '$location', 'StudentService', 'GenderService', 'StateService','MessageService', 'CryptoJS', 'lodash'];

    function StudentsDetail($scope, $routeParams, $location, StudentService, GenderService, StateService, MessageService, CryptoJS, _) {
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

        StateService.GetStudentStates()
        .then(setStates)
        .catch(handleError);

        StudentService.GetOne(vm.username)
        .then(setStudent)
        .catch(handleError);
      }

      function updateStudent() {
        if (!_.isEmpty(vm.student)) {
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
        if (!_.isEmpty(vm.student)) {
          MessageService.confirm('¿Desea realmente eliminar este estudiante?')
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

      function setStates(States) {
      	vm.states = [];
      	for (var i = 0; i < States.length; i++) {
      		if(States[i].value  != "*")
      		{
      			vm.states.push(States[i].value);
      		}
      	};
      }

      function setStudent(student) {
        vm.student = student;
      }

      function redirectToStudents(result) {
        $location.path('/Operador/Estudiantes');
      }

      function handleUpdate(student) {
        setStudent(student);
        MessageService.success('Estudiante actualizado.');
        $scope.$broadcast('show-errors-reset');
      }

      function handleError(data) {
        MessageService.error(data.description);
      }
    }
})();
