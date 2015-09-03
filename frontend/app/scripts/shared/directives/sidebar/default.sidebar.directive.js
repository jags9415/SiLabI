(function() {
    'use strict';

    angular
        .module('silabi')
        .directive('defaultSidebar', DefaultSideBar);

    function DefaultSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'views/shared/sidebar/default.html',
            controller: 'SideBarController',
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }
})();
