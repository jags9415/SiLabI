(function() {
    'use strict';

    angular
        .module('silabi.sidebar')
        .directive('administratorsSidebar', AdministratorsSideBar);

    function AdministratorsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'scripts/other/sidebar/administrators/administrators-sidebar.html',
            controller: AdministratorsSideBarController,
            controllerAs: 'vm',
            bindToController: true
        };

        return directive;
    }

    AdministratorsSideBarController.$inject = ['$location'];

    function  AdministratorsSideBarController($location) {
      var vm = this;

      vm.isActive = isActive;
      vm.goProfessors = function goProfessors()
      {
        $location.path("/Operador/Docentes");
      };
      vm.nombre = "s";

      

      function isActive(viewLocation) 
      {
        return viewLocation === $location.path()
      }


    }
})();
