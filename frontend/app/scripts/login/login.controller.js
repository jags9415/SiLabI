(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['LoginService', 'MessageService', '$location', '$sessionStorage', 'jwtHelper'];

    function LoginController(LoginService, MessageService, $location, $sessionStorage, jwtHelper) {
      var vm = this;
      vm.username;
      vm.password;
      vm.logIn = logIn;
      vm.logOut = logOut
      vm.isLoggedIn = isLoggedIn;
      vm.$storage = $sessionStorage;

      function isLoggedIn() {
        var token = vm.$storage['access_token'];
        if (!token) return false;
        else return !jwtHelper.isTokenExpired(token);
      }

      function logIn() {
        if (vm.username && vm.password) {
          LoginService.authenticate(vm.username, vm.password)
          .then(handleSuccess)
          .catch(handleError);
        }
      }

      function logOut() {
        delete vm.$storage['access_token'];
        delete vm.$storage['user_id'];
        delete vm.$storage['user_name'];
        $location.path('/');
      }

      function handleSuccess(result) {
        vm.$storage['access_token'] = result.access_token;
        vm.$storage['user_id'] = result.user.id.toString();
        vm.$storage['user_name'] = result.user.full_name;
        redirectTo(result.user.type);
      }

      function handleError(error) {
        MessageService.error(error.description);
      }

      function redirectTo(userType) {
        $location.path('/' + userType);
      }
    }
})();
