(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('CourseListController', CourseListController);

    CourseListController.$inject = ['$location', 'CourseService', 'MessageService'];

    function CourseListController($location, CourseService, MessageService) {
        var vm = this;

        vm.loaded = false;
        vm.courses = [];
        vm.searched = {};
        vm.limit = 20;
        vm.request = {
          fields : "id,code,name"
        };

        vm.open = openCourse;
        vm.delete = deleteCourse;
        vm.search = searchCourse;
        vm.isEmpty = isEmpty;
        vm.isLoaded = isLoaded;
        vm.loadPage = loadPage;

        activate();

        function activate() {
          var page = parseInt($location.search()['page']);

          if (isNaN(page)) {
            page = 1;
          }

          vm.totalPages = page;
          vm.page = page;
          loadPage();
        }

        function loadPage() {
          $location.search('page', vm.page);

          vm.request.page = vm.page;
          vm.request.limit = vm.limit;

          CourseService.GetAll(vm.request)
          .then(setCourses)
          .catch(handleError);
        }

        function openCourse(id) {
          $location.url('/Operador/Cursos/' + id);
        }

        function searchCourse() {
          vm.request.query = {};

          if (vm.searched.code) {
            vm.request.query.code = {
              operation: "like",
              value: '*' + vm.searched.code.replace(' ', '*') + '*'
            }
          }

          if (vm.searched.name) {
            vm.request.query.name = {
              operation: "like",
              value: '*' + vm.searched.name.replace(' ', '*') + '*'
            }
          }

          loadPage();
        }

        function isEmpty() {
          return vm.courses.length == 0;
        }

        function isLoaded() {
          return vm.loaded;
        }

        function setCourses(data) {
          vm.courses = data.results;
          vm.page = data.current_page;
          vm.totalPages = data.total_pages;
          vm.totalItems = vm.limit * vm.totalPages;
          vm.loaded = true;
        }

        function deleteCourse(id) {
          MessageService.confirm("¿Desea realmente eliminar este curso?")
          .then(function() {
            CourseService.Delete(id)
            .then(loadPage)
            .catch(handleError);
          });
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
