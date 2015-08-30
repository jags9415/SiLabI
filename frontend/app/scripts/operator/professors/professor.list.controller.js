(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ProfessorListController', ProfessorListController);
        ProfessorListController.$inject = ['$routeParams', 'OperatorsService', 'ProfessorsService', '$location'];

    function ProfessorListController($routeParams, OperatorsService, ProfessorsService, $location) {
      
        var vm = this;
        vm.advanceSearch = false;
        vm.searched = {};
      	activate();
    	
    	vm.loadHomePage = loadHomePage;
    	vm.loadProfessorsPage = loadProfessorsPage;
    	vm.checkProfessorSearch = checkProfessorSearch;
    	vm.seeProfessorDetail = seeProfessorDetail;
        vm.toggleAdvanceSearch = toggleAdvanceSearch;

    	 function loadHomePage(number)
    	{
    		if(typeof(sessionStorage) == 'undefined')
    		{
    			vm.appsArray = [];
	    		var pnumber = vm.pageNumber + number;
	    		if(pnumber > 0 && pnumber <= vm.totalPages)
	    		{
		    		OperatorsService.getAppointments(vm.pageNumber + number, "a"/*access_token*/ ).
		    		then(function(response)
				        {
							vm.appsArray = response.results;
							vm.pageNumber = pnumber;
							vm.totalPages = response.total_pages;
						},
						function(error)
				        {
							
						}
					);
		    	}
		    }
    	}


    	function loadProfessorsPage(number)
    	{
    		var pnumber = vm.pageNumber + number;
    		if(pnumber > 0 && pnumber <= vm.totalPages)
    		{
    			vm.professorsArray = [];
	    		ProfessorsService.getProfessorsByPage(pnumber, "a"/*vm.access_token*/ ).
	    		then(function(response)
			        {
						vm.professorsArray = response.results;
						vm.pageNumber = pnumber;
						vm.totalPages = response.total_pages;
					},
					function(error)
			        {
						alert("Error al ontener datos de docentes.");
					}
				);
	    	}
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

        function toggleAdvanceSearch() 
        {
          vm.advanceSearch = !vm.advanceSearch;
          vm.searched.state = vm.states[0];
          delete vm.searched.year;
          delete vm.searched.period;
          delete vm.searched.username;
        }

    	function searchProfessor()
    	{
            console.log("Looking for: "+vm.searchText);
    		vm.onSearch = true;
            var jsonObject = 
            {
                "fields": "id, full_name, email, phone",
                "sort" : {
                "field": vm.orderCriteria,
                "type": vm.orderType
                },
                "query":
                {
                    "full_name":
                    {
                        "operation":"like",
                        "value":"*"+vm.searchText+"*"
                    }
                },
                "limit": 20,
                "access_token": vm.access_token
            };

    		ProfessorsService.searchProfessorByName(jsonObject).
	    		then(function(response)
			        {
                        console.log(response.results.length);
                        console.log(response.results);
						vm.professorsArray.push.apply(vm.professorsArray, response.results);
						vm.onSearch = false;
					},
					function(error)
			        {
						alert("Error al ontener datos de docente.");
						vm.onSearch = false;
					}
				);
    	}


    	function seeProfessorDetail (user_name) 
        {
    		$location.path("/Operador/Docentes/"+user_name);
    	}

        function deleteProfessor(id)
        {
            console.log("Deleting professor: "+id+" access_token: "+vm.access_token);
            ProfessorsService.deleteProfessor(id, vm.access_token).
                then(function(response)
                {
                    alert("Usuario eliminado con éxito");
                    //$location.path("/Operador/Docentes");
                },
                function(error)
                {
                    alert("Error al modificar datos de docente." + error.description);
                });
        }


    	function activate()
    	{
            vm.professorsArray = [];
    		vm.pageNumber = 0;
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
    	}
    }
})();