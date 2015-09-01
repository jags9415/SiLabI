(function() {
    'use strict';

    angular
    .module('silabi.sidebar')
    .directive('administratorsSidebar', AdministratorsSideBar);

    function AdministratorsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'scripts/shared/directives/sidebar/administrators/sidebar.html',
            controller: 'SideBarController',
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }
})();
