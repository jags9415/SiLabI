(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['LoginService', 'MessageService', '$location'];

    function LoginController(LoginService, MessageService, $location) {
      var vm = this;
      vm.username;
      vm.password;
      vm.authenticate = authenticate;

      function authenticate() {
        LoginService.authenticate(vm.username, vm.password)
        .then(handleSuccess)
        .catch(handleError);
      }

      function handleSuccess(result) {
        sessionStorage.setItem('access_token', result.access_token);
        sessionStorage.setItem('user_id', result.user.id.toString());
        sessionStorage.setItem('user_name', result.user.full_name);
        redirectTo(result.user.type);
      }

      function handleError(error) {
        MessageService.error(error.description);
      }

      function redirectTo(userType) {
        $location.path('/' + userType + '/Inicio');
      }
    }
})();
