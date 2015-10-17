(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('CourseListController', CourseListController);

    CourseListController.$inject = ['$location', 'CourseService', 'MessageService', 'StateService'];

    function CourseListController($location, CourseService, MessageService, StateService) {
        var vm = this;

        vm.loaded = false;
        vm.advanceSearch = false;
        vm.courses = [];
        vm.searched = {};
        vm.limit = 20;
        vm.request = {
          fields : 'id,code,name,state',
          sort: [{field: 'code', type: 'ASC'}]
        };

        vm.open = openCourse;
        vm.delete = deleteCourse;
        vm.search = searchCourse;
        vm.isEmpty = isEmpty;
        vm.isLoaded = isLoaded;
        vm.loadPage = loadPage;
        vm.toggleAdvanceSearch = toggleAdvanceSearch;

        activate();

        function activate() {
          var page = parseInt($location.search()['page']);

          if (isNaN(page)) {
            page = 1;
          }

          vm.totalPages = page;
          vm.page = page;
          loadPage();

          StateService.GetLabStates()
          .then(setStates)
          .catch(handleError);
        }

        function loadPage() {
          $location.search('page', vm.page);

          vm.request.page = vm.page;
          vm.request.limit = vm.limit;

          vm.promise = CourseService.GetAll(vm.request)
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
              operation: 'eq',
              value: vm.searched.code
            };
          }

          if (vm.searched.name) {
            vm.request.query.name = {
              operation: 'like',
              value: '*' + vm.searched.name.replace(' ', '*') + '*'
            };
          }

          if (vm.searched.state) {
            vm.request.query.state = {
              operation: 'like',
              value: vm.searched.state.value
            };
          }

          loadPage();
        }

        function isEmpty() {
          return vm.courses.length === 0;
        }

        function isLoaded() {
          return vm.loaded;
        }

        function toggleAdvanceSearch() {
          vm.advanceSearch = !vm.advanceSearch;
          delete vm.searched.code;
          delete vm.searched.name;
          delete vm.searched.state;
        }

        function setCourses(data) {
          vm.courses = data.results;
          vm.page = data['current_page'];
          vm.totalPages = data['total_pages'];
          vm.totalItems = vm.limit * vm.totalPages;
          vm.loaded = true;
        }

        function deleteCourse(id) {
          MessageService.confirm('¿Desea realmente eliminar este curso?')
          .then(function() {
            CourseService.Delete(id)
            .then(loadPage)
            .catch(handleError);
          });
        }


        function setStates(states) {
          vm.states = states;
        }

        function handleError(data) {
          MessageService.error(data.description);
        }
    }
})();
