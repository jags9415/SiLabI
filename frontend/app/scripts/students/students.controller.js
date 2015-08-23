(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentsController', StudentsController);
        StudentsController.$inject = ['$scope', '$routeParams', 'StudentsService'];

    function StudentsController($scope, $routeParams, StudentsService) {
      
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
		    		StudentsService.getAppointments($scope.pageNumber + number, "a"/*access_token*/ ).
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
    	};

    	function init()
    	{
    		$scope.appsArray = 
            [
                {
                    "appId":"123",
                    "date": Date(),
                    "course": "Inglés 1 Computación",
                    "lab": "Laboratorio A",
                    "software": "SF-65"
                }
            ];
    		$scope.pageNumber = 1;
    		$scope.totalPages = 1;
    	}
    }
})();