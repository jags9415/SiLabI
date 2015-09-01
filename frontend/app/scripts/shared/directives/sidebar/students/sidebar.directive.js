(function() {
    'use strict';

    angular
        .module('silabi.sidebar')
        .directive('studentsSidebar', StudentsSideBar);

    function StudentsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'scripts/shared/directives/sidebar/students/sidebar.html',
            controller: 'SideBarController',
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }
})();
