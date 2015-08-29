(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('ProfessorsController', ProfessorsController);
        ProfessorsController.$inject = ['$scope', '$routeParams', 'ProfessorsService'];

    function ProfessorsController($scope, $routeParams, ProfessorsService) {
      
      	init();
      	//loadPage(1);
    	
    	$scope.loadPage = loadPage;

    	 function loadPage(number)
    	{
    		if(typeof(sessionStorage) == 'undefined')
    		{
    			var access_token = sessionStorage.getItem('access_token');
	    		var pnumber = $scope.pageNumber + number;
	    		if(pnumber > 0 && pnumber <= $scope.totalPages)
	    		{
		    		ProfessorsService.getAppointments($scope.pageNumber + number, "a"/*access_token*/ ).
		    		then(function(response)
				        {
							$scope.reservationsArray = response.results;
							$scope.pageNumber = pnumber;
							$scope.totalPages = response.total_pages;
						},
						function(error)
				        {
							
						}
					);
		    	}
		    }
    	};

    	function init()
    	{
    		$scope.reservationsArray = 
            [
                {
                    "appId":"123",
                    "date": Date(),
                    "course": "Inglés 4 Computación",
                    "lab": "Laboratorio B",
                    "software": "SF-666"
                }
            ];
    		$scope.pageNumber = 1;
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