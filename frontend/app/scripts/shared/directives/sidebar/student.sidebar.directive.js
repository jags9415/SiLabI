(function() {
    'use strict';

    angular
        .module('silabi')
        .directive('studentsSidebar', StudentsSideBar);

    function StudentsSideBar() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'views/shared/sidebar/student.html',
            controller: studentsSidebarController,
            controllerAs: 'SideBar',
            bindToController: true
        };
        return directive;
    }

    studentsSidebarController.$inject = ['$location', '$localStorage'];

    function studentsSidebarController($location, $localStorage) {
      var vm = this;
    }
})();
