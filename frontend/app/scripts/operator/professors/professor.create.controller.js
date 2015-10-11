(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('ProfessorsCreateController', ProfessorsCreateController);

    ProfessorsCreateController.$inject = ['$scope', 'ProfessorService', 'GenderService', 'MessageService', 'CryptoJS'];

    function ProfessorsCreateController($scope, ProfessorService, GenderService, MessageService, CryptoJS) {
      var vm = this;
      vm.professor = {};
	    vm.create = createProfessor;
      vm.generateUserName = generateUserName;

      activate();

      function activate() {
        GenderService.GetAll()
        .then(setGenders)
        .catch(handleError);
      }

      function generateUserName() {
          if (vm.professor.name && vm.professor.last_name_1) {
              vm.professor.username = (vm.professor.name.substring(0, 1) + vm.professor.last_name_1).toLowerCase();
          }
      }

    	function createProfessor() {
    		if (!_.isEmpty(vm.professor)) {
          var hash = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
          vm.professor.password = hash;
    			ProfessorService.Create(vm.professor)
	    		.then(handleCreateSuccess)
          .catch(handleError);
	    	}
    	}

      function handleCreateSuccess(data) {
        MessageService.success("Docente creado.");

        // Reset form data.
        vm.professor = {};
        vm.professor.gender = vm.genders[0];
        delete vm.password;
        delete vm.password_confirm;

        // Reset form validations.
        $scope.$broadcast('show-errors-reset');
      }

      function setGenders(genders) {
        vm.genders = genders;
        vm.professor.gender = genders[0];
      }

      function handleError(data) {
        MessageService.error(data.description);
      }
    }
})();
