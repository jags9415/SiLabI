(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['AuthenticationService', 'MessageService', '$route', '$location', '$localStorage', 'jwtHelper', 'CryptoJS'];

    function LoginController(AuthenticationService, MessageService, $route, $location, $localStorage, jwtHelper, CryptoJS) {
      var vm = this;

      vm.logIn = logIn;
      vm.logOut = logOut;
      vm.isAuthenticated = isAuthenticated;
      vm.$storage = $localStorage;
      vm.loading = false;

      function isAuthenticated() {
        return AuthenticationService.isAuthenticated();
      }

      function logIn() {
        if (vm.username && vm.password) {
          var hash = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
          vm.loading = true;
          AuthenticationService.authenticate(vm.username, hash)
          .then(handleSuccess)
          .catch(handleError);
        }
      }

      function logOut() {
        vm.$storage.$reset();
      }

      function handleSuccess(result) {
        vm.$storage['username'] = result.user['username'];
        vm.$storage['user_id'] = result.user['id'].toString();
        vm.$storage['user_name'] = result.user['full_name'];
        vm.$storage['user_type'] = result.user['type'];
        vm.$storage['access_token'] = result['access_token'];
        console.log(vm.$storage['access_token']);
      }

      function handleError(error) {
        vm.loading = false;
        MessageService.error(error.description);
      }
    }
})();
