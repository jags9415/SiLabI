(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AdminController', AdminController);
        AdminController.$inject = ['$scope', '$routeParams', 'AdminService'];

    function AdminController($scope, $routeParams, AdminService) {
      
      	init();
      	loadPage(1);
    	
    	$scope.loadPage = loadPage;

    	 function loadPage(number)
    	{
    		if(typeof(sessionStorage) == 'undefined')
    		{
    			var access_token = sessionStorage.getItem('access_token');
	    		var pnumber = $scope.pageNumber + number;
	    		if(pnumber > 0 && pnumber <= $scope.totalPages)
	    		{
		    		AdminService.getAdmins($scope.pageNumber + number, "a"/*access_token*/ ).
		    		then(function(response)
				        {
							$scope.adminsArray = response.results;
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
    		$scope.adminsArray = [];
    		$scope.pageNumber = 0;
    		$scope.totalPages = 1;
    	}
    }
})();