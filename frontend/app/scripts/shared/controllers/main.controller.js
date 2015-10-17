(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('MainController', MainController);

    MainController.$inject = ['$scope', '$location', '$route', '$localStorage', 'lodash'];

    function MainController($scope, $location, $route, $localStorage, _) {
      var vm = this;

      $scope.$storage = $localStorage;
      vm.$storage = $localStorage;

      vm.AccessToken = getAccessToken;
      vm.UserId = getUserId;
      vm.UserName = getUserName;
      vm.UserType = getUserType;

      $scope.$watch('$storage.access_token', function(newValue, oldValue) {
        if (_.isEmpty(newValue)) {
          $location.path('/Login');
        }
        else if ($location.path() === '/Login') {
          $location.path('/' + getUserType());
        }
      });

      function getAccessToken() {
        return vm.$storage['access_token'];
      }

      function getUserId() {
        return vm.$storage['user_id'];
      }

      function getUserName() {
        return vm.$storage['user_name'];
      }

      function getUserType() {
        return vm.$storage['user_type'];
      }
    }
})();
