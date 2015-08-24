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
        var fullName = '';
        fullName += vm.user.name + ' ' || '';
        fullName += vm.user.last_name_1 + ' ' || '';
        fullName += vm.user.last_name_2 || '';
        sessionStorage.setItem('access_token', result.access_token);
        sessionStorage.setItem('user_id', vm.user.id.toString());
        sessionStorage.setItem('user_name', fullName);
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
