(function() {
    'use strict';

    angular
        .module('silabi.navbar')
        .directive('defaultNavBar', DefaultNavBar);

    function DefaultNavBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'scripts/navbar/default/default-navbar.html',
            controller: DefaultNavBarController,
            controllerAs: 'vm',
            bindToController: true
        };

        return directive;
    }

    DefaultNavBarController.$inject = ['$location'];

    function DefaultNavBarController($location) {
      var vm = this;

      vm.isActive = isActive;

      function isActive(viewLocation) {
        return viewLocation === $location.path()
      }


    }
})();
