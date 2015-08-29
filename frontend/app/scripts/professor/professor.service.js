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
				    defer.reject(data);
				 });
			return defer.promise;
        };

        this.getProfessorsByPage = function(page_number, access_token)
        {
            var defer = $q.defer();
            $http.get(apiUrl + '/professors?page='+page_number+'&access_token='+access_token).
                success(function(data, status, headers, config) 
                {
                    defer.resolve(data);    
                }).
                error(function(data, status, headers, config) 
                {
                    defer.reject(data);
                 });
            return defer.promise;
        };

        this.searchProfessorByName = function(name, access_token)
        {
            var defer = $q.defer();
            $http.get(apiUrl + '/professors?q=name+like+*'+name+'*', {
                'access_token': access_token
            }).
                success(function(data, status, headers, config) 
                {
                    defer.resolve(data);    
                }).
                error(function(data, status, headers, config) 
                {
                    defer.reject(data);
                 });
            return defer.promise;
        };

        this.getProfessorByUserName = function(user_name, access_token)
        {
            var defer = $q.defer();
            $http.get(apiUrl + '/professors/'+user_name,{
                'access_token': access_token
            }).success(function(data, status, headers, config) 
            {
                defer.resolve(data);    
            }).
            error(function(data, status, headers, config) 
            {
                defer.reject(data);
             });
            return defer.promise;
        };


        this.createProfessor = function(jsonObject)
        {
            var defer = $q.defer();
            $http.post(apiUrl + '/professors', jsonObject).
                success(function(data, status, headers, config) 
                {
                    defer.resolve(data);    
                }).
                error(function(data, status, headers, config) 
                {
                    defer.reject(data);
                 });
            return defer.promise;
        };

        this.updateProfessor = function(id, jsonObject)
        {
            var defer = $q.defer();
            $http.put(apiUrl + '/professors/'+id, jsonObject).
            success(function(data, status, headers, config) 
            {
                defer.resolve(data);    
            }).
            error(function(data, status, headers, config) 
            {
                defer.reject(data);
             });
            return defer.promise;
        };

        this.deleteProfessor = function(id, access_token)
        {
            var defer = $q.defer();
            $http({method: "DELETE", headers: {"Content-Type": "application/json"}, url: apiUrl + '/professors/' + id, data: { 'access_token': access_token }})
            .success(function(data, status, headers, config) 
            {
                defer.resolve(data);    
            }).
            error(function(data, status, headers, config) 
            {
                defer.reject(data);
             });
            return defer.promise;
        }
        
    }
})();