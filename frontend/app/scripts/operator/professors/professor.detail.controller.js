(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('ProfessorsDetailController', ProfessorsDetailController);

    ProfessorsDetailController.$inject = ['$routeParams', '$location', 'ProfessorService', 'GenderService', 'MessageService'];

    function ProfessorsDetailController($routeParams, $location, ProfessorService, GenderService, MessageService) {
        var vm = this;
        vm.username = $routeParams.username;
        vm.update = updateProfessor;
        vm.delete = deleteProfessor;

        activate();

        function activate() {
          GenderService.GetAll().
          then(setGenders)
          .catch(handleError);

          ProfessorService.GetOne(vm.username).
          then(setProfessor)
          .catch(handleError);
      	}

        function updateProfessor() {
          if (vm.professor) {
            if (vm.password) {
              var hash = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
              vm.professor.password = hash;
            }
            ProfessorService.Update(vm.professor.id, vm.professor)
            .then(setProfessor)
            .catch(handleError);
          }
        }

        function deleteProfessor() {
          if (vm.professor) {
            MessageService.confirm("¿Desea realmente eliminar este docente?")
            .then(function() {
              ProfessorService.Delete(vm.professor.id)
              .then(redirectToProfessors)
              .catch(handleError)
            });
          }
        }

        function setGenders(genders) {
          vm.genders = genders;
        }

        function setProfessor(professor) {
          vm.professor = professor;
        }

        function redirectToProfessors() {
          $location.path("/Operador/Docentes");
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
