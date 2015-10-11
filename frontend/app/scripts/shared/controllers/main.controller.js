(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('MainController', MainController);

    MainController.$inject = ['$rootScope', '$localStorage'];

    function MainController($rootScope, $localStorage) {
      var vm = this;
      vm.$storage = $localStorage;
      vm.AccessToken = GetAccessToken;
      vm.UserId = GetUserId;
      vm.UserName = GetUserName;
      vm.UserType = GetUserType;

      function GetAccessToken() {
        return vm.$storage['access_token'];
      }

      function GetUserId() {
        return vm.$storage['user_id'];
      }

      function GetUserName() {
        return vm.$storage['user_name'];
      }

      function GetUserType() {
        return vm.$storage['user_type'];
      }
    }
})();
