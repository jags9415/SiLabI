(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ProfessorListController', ProfessorListController);
        ProfessorListController.$inject = ['$routeParams', 'ProfessorService', '$location', 'MessageService'];

    function ProfessorListController($routeParams, ProfessorService, $location, MessageService) {
      var vm = this;
      vm.advanceSearch = false;
      vm.limit = 20;
      vm.searched = {};
      vm.request = {};
      vm.request.fields = "id,full_name,username,email,phone,state";

    	vm.loadPage = loadPage;
    	vm.seeProfessorDetail = seeProfessorDetail;
      vm.toggleAdvanceSearch = toggleAdvanceSearch;
      vm.search = searchProfessors;
      vm.createProfessor = createProfessor;
      vm.deleteProfessor = deleteProfessor;

      activate();

      function activate() {
        vm.page = parseInt($location.search()['page']);

        if (isNaN(vm.page))
        {
            vm.page = 1;
        }

        vm.professors = [];
		    vm.totalPages = vm.page;

        vm.states = [
          {
            name: 'Cualquiera',
            value: '*'
          },
          {
            name: 'Activo',
            value: 'Activo'
          },
          {
            name: 'Inactivo',
            value: 'Inactivo'
          }
        ];

        loadPage();
      }

    	function loadPage() {
        $location.search('page', vm.page);

        vm.request.page = vm.page;
        vm.request.limit = vm.limit;

    		ProfessorService.GetAll(vm.request)
    		.then(handleGetSuccess)
        .catch(handleError);
    	}

      function handleGetSuccess(response) {
        vm.professors = response.results;
        vm.page = response.current_page;
        vm.totalPages = response.total_pages;
        vm.totalItems = vm.limit * vm.totalPages;
      }

      function handleError(response) {
        MessageService.error(response.description);
      }

      function searchProfessors() {
        vm.request.query = {};

        if (vm.searched.full_name) {
          vm.request.query.full_name = {
            operation: "like",
            value: '*' + vm.searched.full_name.replace(' ', '*') + '*'
          }
        }

        if (vm.searched.username) {
          vm.request.query.username = {
            operation: "like",
            value: '*' + vm.searched.username + '*'
          }
        }

        if (vm.searched.state) {
          vm.request.query.state = {
            operation: "like",
            value: vm.searched.state.value
          }
        }

        if (vm.searched.email) {
          vm.request.query.email = {
            operation: "like",
            value: '*' + vm.searched.email + '*'
          }
        }

        if (vm.searched.phone) {
          vm.request.query.phone. = {
            operation: "like",
            value: '*' + vm.searched.phone + '*'
          }
        }

        loadPage();
      }

      function toggleAdvanceSearch() {
        vm.advanceSearch = !vm.advanceSearch;
        delete vm.searched.state;
        delete vm.searched.email;
        delete vm.searched.phone;
        delete vm.searched.username;
      }

    	function seeProfessorDetail (username) {
    		$location.path("/Operador/Docentes/" + username);
    	}

      function deleteProfessor(id) {
        ProfessorService.Delete(id)
        .then(handleDeleteSuccess)
        .catch(handleError);
      }

      function handleDeleteSuccess(response) {
        loadPage();
      }

      function createProfessor() {
        $location.path('/Operador/Docentes/Agregar');
      }
    }
})();
