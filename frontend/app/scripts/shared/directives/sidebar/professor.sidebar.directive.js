(function() {
    'use strict';

    angular
        .module('silabi')
        .directive('professorsSidebar', ProfessorsSideBar);

    function ProfessorsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'views/shared/sidebar/professor.html',
            controller: 'SideBarController',
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }
})();
