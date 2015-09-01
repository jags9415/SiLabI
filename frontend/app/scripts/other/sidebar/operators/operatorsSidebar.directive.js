(function() {
    'use strict';

    angular
        .module('silabi.sidebar')
        .directive('operatorsSidebar', OperatorsSideBar);

    function OperatorsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'scripts/other/sidebar/operators/operators-sidebar.html',
            controller: 'SideBarController',
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }
})();
