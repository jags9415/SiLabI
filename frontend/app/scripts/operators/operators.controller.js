(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('OperatorsController', OperatorsController);
        OperatorsController.$inject = ['$scope', '$routeParams', 'OperatorsService', 'ProfessorsService'];

    function OperatorsController($scope, $routeParams, OperatorsService, ProfessorsService) {
      
      	init();
      	//loadHomePage(1);
    	
    	$scope.loadHomePage = loadHomePage;
    	$scope.loadProfessorsPage = loadProfessorsPage;

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