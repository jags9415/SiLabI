(function() {
    'use strict';

    angular
        .module('silabi.sidebar')
        .directive('defaultSidebar', DefaultSideBar);

    function DefaultSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'scripts/shared/directives/sidebar/default/sidebar.html',
            controller: 'SideBarController',
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }
})();
