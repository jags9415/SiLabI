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

      vm.authenticate = authenticate;

      function authenticate() {
        var username = vm.username;
        var password = vm.password;
        var result = LoginService.getUser(username, password);
        console.log(username);
        console.log(password);
        console.log(result);
      }

    }
})();
