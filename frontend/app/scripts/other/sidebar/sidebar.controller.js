(function() {
    'use strict';

    angular
    .module('silabi.sidebar')
    .controller('SideBarController', SideBarController);

    SideBarController.$inject = ['$location'];

    function SideBarController($location) {
      var vm = this;
      vm.isActive = isActive;

      function isActive(view) {
        return view === $location.path();
      }
    }
})();
