(function() {
    'use strict';

    angular
        .module('silabi.sidebar')
        .directive('defaultSidebar', DefaultSideBar);

    function DefaultSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'scripts/other/sidebar/default/default-sidebar.html',
            controller: DefaultSideBarController,
            controllerAs: 'vm',
            bindToController: true
        };

        return directive;
    }

    DefaultSideBarController.$inject = ['$location'];

    function DefaultSideBarController($location) {
      var vm = this;

      vm.isActive = isActive;

      function isActive(viewLocation) {
        return viewLocation === $location.path()
      }


    }
})();
