(function() {
    'use strict';

    angular
        .module('silabi.sidebar')
        .directive('professorsSidebar', ProfessorsSideBar);

    function ProfessorsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'scripts/other/sidebar/professors/professors-sidebar.html',
            controller: ProfessorsSideBarController,
            controllerAs: 'vm',
            bindToController: true
        };

        return directive;
    }

    ProfessorsSideBarController.$inject = ['$location'];

    function ProfessorsSideBarController($location) {
      var vm = this;

      vm.isActive = isActive;
      

      function isActive(viewLocation) 
      {
        return viewLocation === $location.path()
      }


    }
})();
