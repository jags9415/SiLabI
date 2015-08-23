(function() {
    'use strict';

    angular
        .module('silabi')
        .service('ProfessorsService',ProfessorsService);

    ProfessorsService.$inject = ['$http', '$q'];

    function ProfessorsService($http, $q) {
        this.getAppointments = function(page_number, access_token)
        {
        	var defer = $q.defer();
        	$http.get('http://localhost/api/v1/', {
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