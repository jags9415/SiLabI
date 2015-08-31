(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ProfessorsDetailController', ProfessorsDetailController);
        ProfessorsDetailController.$inject = ['$routeParams', 'ProfessorsService', '$route', '$location', 'MessageService'];

    function ProfessorsDetailController($routeParams, ProfessorsService, $route, $location, MessageService) {
        var vm = this;
        vm.genders = ["Masculino", "Femenino"];
        vm.username = $routeParams.username;
        vm.update = updateProfessor;
        vm.delete = deleteProfessor;

        activate();

        function activate()
      	{
          ProfessorsService.GetOne(vm.username).
          then(handleGetSuccess)
          .catch(handleError);
      	}

        function updateProfessor()
        {
          if (vm.professor) {
            if (vm.password) {
              var hash = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
              vm.professor.password = hash;
            }
            ProfessorsService.Update(vm.professor.id, vm.professor)
            .then(handleGetSuccess)
            .catch(handleError);
          }
        }

        function deleteProfessor() {
          if (vm.professor) {
            ProfessorsService.Delete(vm.professor.id)
            .then(handleDeleteSuccess)
            .catch(handleError);
          }
        }

        function handleGetSuccess(data) {
          vm.professor = data;
        }

        function handleDeleteSuccess() {
          $location.path("/Operador/Docentes");
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
