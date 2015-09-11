(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('CourseListController', CourseListController);

    CourseListController.$inject = ['$routeParams'];

    function CourseListController($routeParams) {
        var vm = this;

        vm.courses = [{
          code: "IC-3847",
          name: "Inglés 2 para Computación"
        },{
          code: "IC-8734",
          name: "Inglés 3 para Computación"
        }];
    }
})();
