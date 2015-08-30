(function() {
    'use strict';

    angular
        .module('silabi')
        .service('ProfessorsService', ProfessorsService);

    ProfessorsService.$inject = ['$localStorage', 'RequestService'];

    function ProfessorsService($localStorage, RequestService) {


        this.GetAll = function(request)
        {
            if (!request) request = {};
            request.access_token = $localStorage['access_token'];
            return RequestService.get('/professors', request);
        };


        this.GetOne = function(user_name)
        {
            var request = {};
            request.access_token = $localStorage['access_token'];
            return RequestService.get('/professors/' + user_name, request);
        }


        this.Create = function(jsonObject)
        {
            var request = {};
            request.professor = jsonObject.professor;
            request.access_token = $localStorage['access_token'];
            return RequestService.post('/professors/', request);
        };

        this.Update = function(id, jsonObject)
        {
            var request = {};
            request.professor = jsonObject.professor;
            request.access_token = $localStorage['access_token'];
            return RequestService.put('/professors/'+id, request);
        };

        this.Delete = function(id)
        {
            var request = {};
            request.access_token = $localStorage['access_token'];
            return RequestService.delete('/professors/'+id, request);
        }
        
    }
})();