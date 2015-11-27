(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('ProfessorsDetailController', ProfessorsDetailController);

    ProfessorsDetailController.$inject = ['$scope', '$routeParams', '$location', 'ProfessorService', 'GenderService', 'MessageService', 'StateService', 'CryptoJS', 'lodash'];

    function ProfessorsDetailController($scope, $routeParams, $location, ProfessorService, GenderService, MessageService, StateService, CryptoJS, _) {
        var vm = this;
        vm.username = $routeParams.username;
        vm.update = updateProfessor;
        vm.delete = deleteProfessor;

        activate();

        function activate() {
          GenderService.GetAll().
          then(setGenders)
          .catch(handleError);

          StateService.GetStudentStates()
          .then(setStates)
          .catch(handleError);

          ProfessorService.GetOne(vm.username).
          then(setProfessor)
          .catch(handleError);
      	}

        function updateProfessor() {
          if (!_.isEmpty(vm.professor)) {
            if (vm.password) {
              var hash = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
              vm.professor.password = hash;
            }
            ProfessorService.Update(vm.professor.id, vm.professor)
            .then(handleUpdate)
            .catch(handleError);
          }
        }

        function deleteProfessor() {
          if (!_.isEmpty(vm.professor)) {
            MessageService.confirm('¿Desea realmente eliminar este docente?')
            .then(function() {
              ProfessorService.Delete(vm.professor.id)
              .then(redirectToProfessors)
              .catch(handleError);
            });
          }
        }

        function setGenders(genders) {
          vm.genders = genders;
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

        function setProfessor(professor) {
          vm.professor = professor;
        }

        function redirectToProfessors() {
          $location.path('/Operador/Docentes');
        }

        function handleUpdate(professor) {
          setProfessor(professor);
          MessageService.success('Docente actualizado.');
          $scope.$broadcast('show-errors-reset');
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
