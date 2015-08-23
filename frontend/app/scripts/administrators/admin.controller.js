(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AdminController', AdminController);
        AdminController.$inject = ['$scope', '$routeParams'];

    function AdminController($scope, $routeParams) {
      
      init();

    	function init()
    	{
    		$scope.adminsArray = [];
    	}
    }
})();