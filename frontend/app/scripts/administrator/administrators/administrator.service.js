(function() {
    'use strict';

    angular
        .module('silabi')
        .service('AdminService', AdminService);

    AdminService.$inject = ['RequestService', '$localStorage'];

    function AdminService(RequestService, $localStorage) {

        this.GetAll = function(request)
        {
            if (!request) request = {};
            request.access_token = $localStorage['access_token'];
            return RequestService.get('/administrators', request);
        };

        this.GetOne = function(id)
        {
            var request = {};
            request.access_token = $localStorage['access_token'];
            return RequestService.get('/administrators/' + id, request);

        };

        this.Create = function(id)
        {
            var request = {}
            request.access_token = $localStorage['access_token'];
            return RequestService.post('/operators/' + id, request);
        }

        this.Delete = function(id)
        {
            var request = {};
            request.access_token = $localStorage['access_token'];
            return RequestService.delete('/administrators/' + id, request);
        }

    }
})();