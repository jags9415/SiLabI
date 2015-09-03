(function() {
    'use strict';

    angular
    .module('silabi')
    .directive('administratorsSidebar', AdministratorsSideBar);

    function AdministratorsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'views/shared/sidebar/administrator.html',
            controller: 'SideBarController',
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }
})();
