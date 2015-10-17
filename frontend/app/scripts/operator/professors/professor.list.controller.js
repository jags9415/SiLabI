(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('ProfessorListController', ProfessorListController);

    ProfessorListController.$inject = ['$routeParams', '$location', 'ProfessorService', 'StateService', 'MessageService'];

    function ProfessorListController($routeParams, $location, ProfessorService, StateService, MessageService) {
      var vm = this;

      vm.loaded = false;
      vm.advanceSearch = false;
      vm.limit = 20;
      vm.searched = {};

      vm.request = {
        fields: 'id,full_name,username,email,phone,state',
        sort: {field: 'full_name', type: 'ASC'}
      };

    	vm.loadPage = loadPage;
    	vm.seeProfessorDetail = seeProfessorDetail;
      vm.toggleAdvanceSearch = toggleAdvanceSearch;
      vm.search = searchProfessors;
      vm.isEmpty = isEmpty;
      vm.isLoaded = isLoaded;
      vm.createProfessor = createProfessor;
      vm.deleteProfessor = deleteProfessor;

      activate();

      function activate() {
        var page = parseInt($location.search()['page']);

        if (isNaN(page)) {
            page = 1;
        }

		    vm.totalPages = page;
        vm.page = page;
        loadPage();

        StateService.GetProfessorStates()
        .then(setStates)
        .catch(handleError);
      }

    	function loadPage() {
        $location.search('page', vm.page);

        vm.request.page = vm.page;
        vm.request.limit = vm.limit;

    		vm.promise = ProfessorService.GetAll(vm.request)
    		.then(setProfessors)
        .catch(handleError);
    	}

      function setProfessors(data) {
        vm.professors = data.results;
        vm.page = data['current_page'];
        vm.totalPages = data['total_pages'];
        vm.totalItems = vm.limit * vm.totalPages;
        vm.loaded = true;
      }

      function setStates(states) {
        vm.states = states;
      }

      function handleError(response) {
        MessageService.error(response.description);
      }

      function searchProfessors() {
        vm.request.query = {};

        if (vm.searched.fullName) {
          vm.request.query['full_name'] = {
            operation: 'like',
            value: '*' + vm.searched.fullName.replace(' ', '*') + '*'
          };
        }

        if (vm.searched.username) {
          vm.request.query['username'] = {
            operation: 'like',
            value: '*' + vm.searched.username + '*'
          };
        }

        if (vm.searched.state) {
          vm.request.query['state'] = {
            operation: 'like',
            value: vm.searched.state.value
          };
        }

        if (vm.searched.email) {
          vm.request.query['email'] = {
            operation: 'like',
            value: '*' + vm.searched.email + '*'
          };
        }

        loadPage();
      }

      function isEmpty() {
        return vm.professors.length === 0;
      }

      function isLoaded() {
        return vm.loaded;
      }

      function toggleAdvanceSearch() {
        vm.advanceSearch = !vm.advanceSearch;
        delete vm.searched.state;
        delete vm.searched.email;
        delete vm.searched.username;

      }

    	function seeProfessorDetail (username) {
    		$location.path('/Operador/Docentes/' + username);
    	}

      function deleteProfessor(id) {
        MessageService.confirm('¿Desea realmente eliminar este docente?')
        .then(function() {
          ProfessorService.Delete(id)
          .then(loadPage)
          .catch(handleError);
        });
      }

      function createProfessor() {
        $location.path('/Operador/Docentes/Agregar');
      }
    }
})();
