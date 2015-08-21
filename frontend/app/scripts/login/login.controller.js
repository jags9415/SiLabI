(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('LoginController', LoginController);

    function LoginController() {
      var vm = this;
      vm.email;
      vm.password;
    }
})();
