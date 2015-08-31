(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentListController', StudentListController);

    StudentListController.$inject = ['StudentService', 'MessageService', '$location'];

    function StudentListController(StudentService, MessageService, $location) {
        var vm = this;
        vm.loaded = false;
        vm.students = [];
        vm.searched = {};
        vm.limit = 20;
        vm.request = {
          fields : "id,full_name,email,phone,username,state"
        };

        vm.createStudent = createStudent;
        vm.open = openStudent;
        vm.delete = deleteStudent;
        vm.search = searchStudents;
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

          StudentService.GetAll(vm.request)
          .then(handleGetSuccess)
          .catch(handleError);
        }

        function createStudent() {
          $location.path('/Operador/Estudiantes/Agregar');
        }

        function openStudent(username) {
          $location.url('/Operador/Estudiantes/' + username);
        }

        function deleteStudent(id) {
          StudentService.Delete(id)
          .then(handleDeleteSuccess)
          .catch(handleError);
        }

        function searchStudents() {
          vm.request.query = {};

          if (vm.searched.full_name) {
            vm.request.query.full_name = {
              operation: "like",
              value: '*' + vm.searched.full_name.replace(' ', '*') + '*'
            }
          }

          loadPage();
        }

        function isEmpty() {
          return vm.students.length == 0;
        }

        function isLoaded() {
          return vm.loaded;
        }

        function handleDeleteSuccess() {
          MessageService.success("Estudiante eliminado.");
          loadPage();
        }

        function handleGetSuccess(data) {
          vm.students = data.results;
          vm.page = data.current_page;
          vm.totalPages = data.total_pages;
          vm.totalItems = vm.limit * vm.totalPages;
          vm.loaded = true;
        }

        function handleError(data) {
          MessageService.error(data.description);
          vm.loaded = true;
        }
    }
})();
