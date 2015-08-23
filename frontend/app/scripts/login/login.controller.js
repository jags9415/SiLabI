(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['LoginService', '$location'];

    function LoginController(LoginService, $location) {
      var vm = this;
      vm.username;
      vm.password;

      vm.user = {};

      vm.requestHasError = false;

      vm.authenticate = authenticate;

      function authenticate() {
        var username = vm.username;
        var password = vm.password;
        LoginService.getUser(username, password)
        .then(getUserInfo)
        .catch(handleErrorInfo);
      }

      function getUserInfo(result) {
        vm.user = result.user;
        redirectTo(vm.user.type);
        return vm.user;
      }

      function redirectTo(userTypeUrl) {
        $location.path('/Inicio/' + userTypeUrl);
      }

      function handleErrorInfo(error) {
        vm.requestHasError = true;
        showError(error);
        vm.requestHasError = false;
      }

      function showError(error) {
        if (error.status === 404)
          alert("No se pudo conectar con el servidor");
        else
          alert(error.data.description);
      }

    }
})();
