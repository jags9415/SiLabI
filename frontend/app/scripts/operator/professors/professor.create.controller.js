(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorsProfessorsCreateController', OperatorsProfessorsCreateController);
        OperatorsProfessorsCreateController.$inject = ['$scope', '$routeParams', 'ProfessorsService', '$location'];

    function OperatorsProfessorsCreateController($scope, $routeParams, OperatorsService, ProfessorsService, $location) {
      
        var vm = this;
      	activate();
    	

    	$scope.checkProfessorSearch = checkProfessorSearch;
    	$scope.createProfessor = createProfessor;
    	$scope.cleanProfessorCreate = cleanProfessorCreate;
        $scope.generateUserName = generateUserName;



    	function loadProfessorsPage(number)
    	{
    		var pnumber = $scope.pageNumber + number;
    		if(pnumber > 0 && pnumber <= $scope.totalPages)
    		{
    			$scope.professorsArray = [];
	    		ProfessorsService.getProfessorsByPage(pnumber, "a"/*$scope.access_token*/ ).
	    		then(function(response)
			        {
						$scope.professorsArray = response.results;
						$scope.pageNumber = pnumber;
						$scope.totalPages = response.total_pages;
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
            if($scope.searchText.length == 0)
            {
                var quant = $scope.professorsArray.length - 20;
                $scope.professorsArray.splice(20, quant);
            }
    		else if($scope.searchResults.length == 0 && !$scope.onSearch)
    		{
    			searchProfessor();
    		}
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

    	function activate()
    	{
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