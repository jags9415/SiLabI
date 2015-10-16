(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('MainController', MainController);

    MainController.$inject = ['$scope', '$location', '$route', '$localStorage'];

    function MainController($scope, $location, $route, $localStorage) {
      var vm = this;

      $scope.$storage = $localStorage;
      vm.$storage = $localStorage;

      vm.AccessToken = GetAccessToken;
      vm.UserId = GetUserId;
      vm.UserName = GetUserName;
      vm.UserType = GetUserType;

      $scope.$watch('$storage.access_token', function(newValue, oldValue) {
        if (_.isEmpty(newValue)) {
          $location.path('/Login');
        }
        else if ($location.path() == '/Login') {
          $location.path('/' + GetUserType());
        }
      });

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
