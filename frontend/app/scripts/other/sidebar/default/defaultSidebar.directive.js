(function() {
    'use strict';

    angular
        .module('silabi.sidebar')
        .directive('defaultSidebar', DefaultSideBar);

    function DefaultSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'scripts/other/sidebar/default/default-sidebar.html',
            controller: 'SideBarController',
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }
})();
