(function() {
    'use strict';

    angular
        .module('silabi.sidebar')
        .directive('studentsSidebar', StudentsSideBar);

    function StudentsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'scripts/other/sidebar/students/students-sidebar.html',
            controller: StudentsSideBarController,
            controllerAs: 'vm',
            bindToController: true
        };

        return directive;
    }

    StudentsSideBarController.$inject = ['$location'];

    function  StudentsSideBarController($location) {
      var vm = this;

      vm.isActive = isActive;
    

      function isActive(viewLocation) 
      {
        return viewLocation === $location.path()
      }


    }
})();
