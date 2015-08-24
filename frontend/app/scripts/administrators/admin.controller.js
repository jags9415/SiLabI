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
            if($scope.access_token != -1)
            {
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
            var name = "NOT DEFINED";
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