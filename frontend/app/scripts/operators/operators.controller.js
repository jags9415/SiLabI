(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorsController', OperatorsController);
        OperatorsController.$inject = ['$scope', '$routeParams', 'OperatorsService', 'ProfessorsService', '$location'];

    function OperatorsController($scope, $routeParams, OperatorsService, ProfessorsService, $location) {
      
      	init();
      	//loadHomePage(1);
    	
    	$scope.loadHomePage = loadHomePage;
    	$scope.loadProfessorsPage = loadProfessorsPage;
    	$scope.checkProfessorSearch = checkProfessorSearch;
    	$scope.createProfessor = createProfessor;
    	$scope.cleanProfessorCreate = cleanProfessorCreate;
    	$scope.seeProfessorDetail = seeProfessorDetail;

    	 function loadHomePage(number)
    	{
    		if(typeof(sessionStorage) == 'undefined')
    		{
    			$scope.appsArray = [];
	    		var pnumber = $scope.pageNumber + number;
	    		if(pnumber > 0 && pnumber <= $scope.totalPages)
	    		{
		    		OperatorsService.getAppointments($scope.pageNumber + number, "a"/*access_token*/ ).
		    		then(function(response)
				        {
							$scope.appsArray = response.results;
							$scope.pageNumber = pnumber;
							$scope.totalPages = response.total_pages;
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
    		var pnumber = $scope.pageNumber + number;
    		if(pnumber > 0 && pnumber <= $scope.totalPages)
    		{
    			console.log("Retrieving page number: " + pnumber);
    			$scope.professorsArray = [];
	    		ProfessorsService.getProfessorsByPage(pnumber, "a"/*$scope.access_token*/ ).
	    		then(function(response)
			        {
						$scope.professorsArray = response.results;
                        console.log(response.results[0].username);
						$scope.pageNumber = pnumber;
						$scope.totalPages = response.total_pages;
						console.log("Total pages: "+response.total_pages);
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
    		if($scope.searchResults.length == 0 && !$scope.onSearch)
    		{
    			searchProfessor($scope.searchText);
    		}
    	}

    	function searchProfessor(name)
    	{
    		$scope.onSearch = true;
    		ProfessorsService.searchProfessorByName(name, "a"/*$scope.access_token*/ ).
	    		then(function(response)
			        {
						$scope.professorsArray.push.apply($scope.professorsArray, response.results);
						$scope.onSearch = false;
					},
					function(error)
			        {
						alert("Error al ontener datos de docente.");
						$scope.onSearch = false;
					}
				);
    	}

    	
    	function  checkProfessorCreateInput() 
    	{
    		if(!isNaN($scope.professorinputName))
    		{
    			alert("Nombre incorrecto.");
    			return false;
    		}
    		else if(!isNaN($scope.professorinputLastName1))
    		{
    			alert("Primer Apellido incorrecto.");
    			return false;
    		}
    		else if(!isNaN($scope.professorinputLastName2))
    		{
    			alert("Segundo Apellido incorrecto.");
    			return false;
    		}
    		else if(isNaN($scope.professorinputPhoneNumber))
    		{
    			alert("Número telefónico incorrecto.");
    			return false;
    		}
    		else
    		{
    			return true;
    		}
    	}

    	function seeProfessorDetail (user_name) 
        {
    		$location.path("/Operador/Docentes/"+user_name);
    	}

    	function cleanProfessorCreate()
    	{
    		$scope.professorinputEmail = null;
    		 $scope.professorinputLastName1 = null;
    		 $scope.professorinputLastName2 = null;
    		 $scope.professorinputName = null;
    		 $scope.professorinputPhoneNumber = null;
    		 $scope.professorinputPassword = null;
    	}

    	function createProfessor()
    	{
    		if(checkProfessorCreateInput() && $scope.professor != null)
    		{
    			var jsonObject = 
				{
	              $scope.professor,
	              "access_token":$scope.access_token
	            }
	            console.log(jsonObject);
    			ProfessorsService.createProfessor(jsonObject).
	    		then(function(response)
			        {
						cleanProfessorCreate();
						console.log("Creado. "+response.created_at);
					},
					function(error)
			        {
						alert("Error al ontener datos de docente: "+response);
					}
				);
	    	}
    	}

    	function init()
    	{
    		$scope.appsArray = 
            [
                {
                    "cardId": "201230825",
                    "date": Date(),
                    "course": "Inglés 1 Computación",
                    "lab": "Laboratorio A",
                    "software": "SF-65"
                }
            ];
            $scope.professorsArray = [];
    		$scope.pageNumber = 0;
    		$scope.totalPages = 1;
    		$scope.onSearch = false;
    		$scope.genders = [{"name":"Masculino"}, {"name":"Femenino"}];
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
            $scope.user_name = name;
            $scope.access_token = accessToken;
    	}
    }
})();