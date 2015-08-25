(function() {
    'use strict';

    angular
        .module('silabi')
        .service('ProfessorsService',ProfessorsService);

    ProfessorsService.$inject = ['$http', '$q', 'API_URL'];

    function ProfessorsService($http, $q, apiUrl) {

        this.getReservations = function(page_number, access_token)
        {
        	var defer = $q.defer();
        	$http.get(apiUrl + '/reservations', {
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

        this.getProfessorsByPage = function(page_number, access_token)
        {
            var defer = $q.defer();
            $http.get(apiUrl + '/professors', {
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