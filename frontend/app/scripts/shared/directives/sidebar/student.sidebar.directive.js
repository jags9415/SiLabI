(function() {
    'use strict';

    angular
        .module('silabi')
        .directive('studentsSidebar', StudentsSideBar);

    function StudentsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'views/shared/sidebar/student.html',
            controller: 'SideBarController',
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }
})();
