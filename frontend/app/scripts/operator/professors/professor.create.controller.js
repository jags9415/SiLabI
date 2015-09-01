(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ProfessorsCreateController', ProfessorsCreateController);
        ProfessorsCreateController.$inject = ['$scope', '$routeParams', '$location', 'ProfessorService', 'MessageService', 'CryptoJS'];

    function ProfessorsCreateController($scope, $routeParams, $location, ProfessorService, MessageService, CryptoJS) {
      var vm = this;
      vm.professor = {};
      vm.genders = ["Masculino", "Femenino"];
	    vm.create = createProfessor;
      vm.generateUserName = generateUserName;

      activate();

      function activate() {
        vm.professor.gender = vm.genders[0];
      }

      function generateUserName() {
          if (vm.professor.name && vm.professor.last_name_1) {
              vm.professor.username = (vm.professor.name.substring(0, 1) + vm.professor.last_name_1).toLowerCase();
          }
      }

    	function createProfessor() {
    		if (vm.professor) {
          var hash = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
          vm.professor.password = hash;
    			ProfessorService.Create(vm.professor)
	    		.then(handleCreateSuccess)
          .catch(handleError);
	    	}
    	}

      function handleCreateSuccess(data) {
        MessageService.success("Docente creado con éxito.");
        vm.professor = {};
        $scope.$broadcast('show-errors-reset');
        vm.professor.gender = vm.genders[0];
        vm.password = null;
        vm.password_confirm = null;
      }

      function handleError(data) {
        MessageService.error(data.description);
      }
    }
})();
