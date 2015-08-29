(function() {
    'use strict';

    angular
        .module('silabi')
        .service('AdminService', AdminService);

    AdminService.$inject = ['$http', '$q'];

    function AdminService($http, $q) {
        this.getAdmins = function(page_number, access_token)
        {
        	var defer = $q.defer();
        	$http.get('http://localhost/api/v1/administrators', {
            	'access_token': access_token,
            	'page': page_number
          	}).
        		success(function(data, status, headers, config) 
				{
				  	defer.resolve(data); 	
				}).
				error(function(data, status, headers, config) 
				{
				    defer.reject(error);
				 });
			return defer.promise;
        };
    }
})();