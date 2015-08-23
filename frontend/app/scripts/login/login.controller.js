(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['LoginService'];

    function LoginController(LoginService) {
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
        alert('Bienvenido ' + vm.user.name + '!');
        return vm.user;
      }

      function handleErrorInfo(error) {
        vm.requestHasError = true;
        if (error.status === 404)
          alert("No se pudo conectar con el servidor");
        else
          alert(error.data.description);
        vm.requestHasError = false;
      }

    }
})();
