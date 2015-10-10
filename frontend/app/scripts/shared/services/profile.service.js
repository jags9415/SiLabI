(function() {
    'use strict';

    angular
        .module('silabi')
        .service('ProfileService', ProfileService);

    ProfileService.$inject = ['RequestService', '$localStorage'];

    function ProfileService(RequestService, $localStorage) {
        this.Get = GetProfile;
        this.Update = UpdateProfile;

        function GetProfile() {
          var request = {
            access_token: $localStorage['access_token']
          };
          return RequestService.get('/me', request);
        }

        function UpdateProfile(profile) {
          var request = {
            access_token: $localStorage['access_token'],
            user: profile
          };
          return RequestService.put('/me', request);
        }
    }
})();
