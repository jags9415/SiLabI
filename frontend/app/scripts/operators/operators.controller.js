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
        $scope.generateUserName = generateUserName;

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
    		if(!isNaN($scope.professor.name))
    		{
    			alert("Nombre incorrecto.");
    			return false;
    		}
    		else if(!isNaN($scope.professor.last_name_1))
    		{
    			alert("Primer Apellido incorrecto.");
    			return false;
    		}
    		else if(!isNaN($scope.professor.last_name_2))
    		{
    			alert("Segundo Apellido incorrecto.");
    			return false;
    		}
    		else if(isNaN($scope.professor.phone))
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
    		$scope.professor.email = null;
    		 $scope.professor.last_name_1 = null;
    		 $scope.professor.last_name_2 = null;
    		 $scope.professor.name = null;
    		 $scope.professor.phone = null;
             $scope.professor.username = null;
    		 $scope.professor.password = null;
    	}

        function generateUserName()
        {
            $scope.professor.username = ($scope.professor.name.substring(0, 1) + $scope.professor.last_name_1).toLowerCase(); 
        }

    	function createProfessor()
    	{
    		if(checkProfessorCreateInput() && $scope.professor != null)
    		{
                var hash = CryptoJS.SHA256($scope.professor.password).toString(CryptoJS.enc.Hex);
    			var jsonObject = 
				{
	              "professor": {
	                "email": $scope.professor.email,
	                "gender": $scope.selected_gender,
	                "last_name_1": $scope.professor.last_name_1,
	                "last_name_2": $scope.professor.last_name_2,
	                "name": $scope.professor.name,
	                "phone": $scope.professor.phone,
	                "username": $scope.professor.username,
	                "password": hash
	              },
	              "access_token":$scope.access_token
	            }
	            console.log(jsonObject);
    			ProfessorsService.createProfessor(jsonObject).
	    		then(function(response)
			        {
						cleanProfessorCreate();
						alert("Usuario creado con éxito: "+response.username);
					},
					function(error)
			        {
						alert("Error al ontener datos de docente: "+ error.description);
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
            $scope.professor = 
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
    	}
    }
})();