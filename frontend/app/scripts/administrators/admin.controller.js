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
    		var pnumber = $scope.pageNumber + number;
    		if(pnumber > 0 && pnumber <= $scope.toalPages)
    		{
	    		AdminService.getAdmins($scope.pageNumber + number, "a"/*access_token*/ ).
	    		then(function(response)
			        {
						$scope.adminsArray = response.results;
						$scope.pageNumber = pnumber;
						$scope.toalPages = response.total_pages;
					},
					function(error)
			        {
						LoggerService.logError(error);
					}
				);
	    	}
    	};

    	function init()
    	{
    		$scope.adminsArray = [];
    		$scope.pageNumber = 0;
    		$scope.toalPages = 1;
    	}
    }
})();