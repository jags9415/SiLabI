(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ProfessorListController', ProfessorListController);
        ProfessorListController.$inject = ['$routeParams', 'OperatorsService', 'ProfessorsService', '$location', 'MessageService'];

    function ProfessorListController($routeParams, OperatorsService, ProfessorsService, $location, MessageService) {
      
        var vm = this;
        vm.advanceSearch = false;
        vm.searched = {};
      	activate();
    	
    	vm.loadPage = loadPage;
    	vm.checkProfessorSearch = checkProfessorSearch;
    	vm.seeProfessorDetail = seeProfessorDetail;
        vm.toggleAdvanceSearch = toggleAdvanceSearch;
        vm.search = searchProfessors;
        vm.createProfessor = createProfessor;



    	function loadPage()
    	{
    		if(vm.pageNumber > 0 && vm.pageNumber <= vm.totalPages)
    		{
    			vm.professorsArray = [];
	    		ProfessorsService.GetAll(vm.request)
	    		.then(handleGetSuccess)
                .catch(handleError);
	    	}
    	}

        function handleGetSuccess(response)
        {
            vm.professorsArray = response.results;
            vm.pageNumber = response.current_page;
            vm.totalPages = response.total_pages;
        }

        function handleError(response)
        {
            MessageService.error(response.description);
        }

    	function checkProfessorSearch()
    	{
            if(vm.searchText.length == 0)
            {
                var quant = vm.professorsArray.length - 20;
                vm.professorsArray.splice(20, quant);
            }
    		else if(vm.searchResults.length == 0 && !vm.onSearch)
    		{
    			searchProfessor();
    		}
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
            if (vm.searched.period.value) {
              vm.request.query["email"] = {
                operation: "eq",
                value: '*' + vm.searched.email + '*'
              }
            }
            if (vm.searched.phone) {
              vm.request.query["phone"] = {
                operation: "eq",
                value: vm.searched.phone
              }
            }
          }

          loadPage();
        }

        function toggleAdvanceSearch() 
        {
          vm.advanceSearch = !vm.advanceSearch;
          vm.searched.state = vm.states[0];
          delete vm.searched.year;
          delete vm.searched.period;
          delete vm.searched.username;
        }


    	function seeProfessorDetail (user_name) 
        {
    		$location.path("/Operador/Docentes/"+user_name);
    	}

        function deleteProfessor(id)
        {
            ProfessorsService.Delete(id)
            .then(handleDeleteSuccess)
            .catch(handleError);
        }

        function handleDeleteSuccess(response)
        {
            MessageService.success("Docente eliminado.");
            loadPage();
        }

        function createProfessor()
        {
            $location.path('/Operador/Docentes/Agregar');
        }

    	function activate()
    	{
            vm.professorsArray = [];
    		vm.pageNumber = 1;
    		vm.totalPages = 1;
    		vm.onSearch = false;
            vm.orderType = "ASC";
            vm.orderCriteria = "name";
            vm.searchText = "";
    		vm.genders = [{"name":"Masculino"}, {"name":"Femenino"}];
    		var name = "undefined";
            var accessToken = -1;
            if(typeof(sessionStorage) != 'undefined' && sessionStorage.getItem('access_token') != null)
            {
                accessToken = sessionStorage.getItem('access_token');
                if(sessionStorage.getItem('user_name') != null)
                {
                    name = sessionStorage.getItem('user_name');
                }
            }
            vm.user_name = name;
            vm.access_token = accessToken;
            vm.professor = 
            {
                "email": "",
                "gender": "",
                "last_name_1": "",
                "last_name_2": "",
                "name": "",
                "phone": "",
                "username": "",
                "password": ""
          };
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
          vm.request = 
            {
                "fields" : "id,full_name,username,email,phone,state", 
                "sort" : 
                {
                "field": vm.orderCriteria,
                "type": vm.orderType
                }
            };
    	}
    }
})();