(function() {
    'use strict';

    angular
    .module('silabi.sidebar')
    .directive('administratorsSidebar', AdministratorsSideBar);

    function AdministratorsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'scripts/other/sidebar/administrators/administrators-sidebar.html',
            controller: 'SideBarController',
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }
})();
