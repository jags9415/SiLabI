(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['AuthenticationService', 'MessageService', '$route', '$location', '$localStorage', 'jwtHelper', 'CryptoJS'];

    function LoginController(AuthenticationService, MessageService, $route, $location, $localStorage, jwtHelper, CryptoJS) {
      var vm = this;
      vm.logIn = logIn;
      vm.logOut = logOut
      vm.isAuthenticated = isAuthenticated;
      vm.$storage = $localStorage;

      function isAuthenticated() {
        return AuthenticationService.isAuthenticated();
      }

      function logIn() {
        if (vm.username && vm.password) {
          var hash = CryptoJS.SHA256(vm.password).toString(CryptoJS.enc.Hex);
          AuthenticationService.authenticate(vm.username, hash)
          .then(handleSuccess)
          .catch(handleError);
        }
      }

      function logOut() {
        delete vm.$storage['access_token'];
        delete vm.$storage['user_id'];
        delete vm.$storage['user_name'];
        delete vm.$storage['user_type'];
        $location.path('/Login');
        $route.reload();
      }

      function handleSuccess(result) {
        vm.$storage['access_token'] = result.access_token;
        vm.$storage['user_id'] = result.user.id.toString();
        vm.$storage['user_name'] = result.user.full_name;
        vm.$storage['user_type'] = result.user.type;
        $location.path('/' + result.user.type);
        $route.reload();
      }

      function handleError(error) {
        MessageService.error(error.description);
      }
    }
})();
