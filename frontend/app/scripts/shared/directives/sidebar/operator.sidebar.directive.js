(function() {
    'use strict';

    angular
        .module('silabi')
        .directive('operatorsSidebar', OperatorsSideBar);

    function OperatorsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'views/shared/sidebar/operator.html',
            controller: 'SideBarController',
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }
})();
