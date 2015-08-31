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
        vm.advanceSearch = false;
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

          vm.states = [
            {
              name: 'Cualquiera',
              value: '*'
            },
            {
              name: 'Activo',
              value: 'Activo'
            },
            {
              name: 'Inactivo',
              value: 'Inactivo'
            },
            {
              name: 'Bloqueado',
              value: 'Bloqueado'
            }
          ];
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
          MessageService.confirm("¿Desea realmente eliminar este estudiante?")
          .then(function() {
            StudentService.Delete(id)
            .then(handleDeleteSuccess)
            .catch(handleError);
          });
        }

        function searchStudents() {
          vm.request.query = {};

          if (vm.searched.full_name) {
            vm.request.query.full_name = {
              operation: "like",
              value: '*' + vm.searched.full_name.replace(' ', '*') + '*'
            }
          }

          if (vm.searched.username) {
            vm.request.query.username = {
              operation: "like",
              value: '*' + vm.searched.username.replace(' ', '*') + '*'
            }
          }

          if (vm.searched.state) {
            vm.request.query.state = {
              operation: "like",
              value: vm.searched.state.value
            }
          }

          if (vm.searched.email) {
            vm.request.query.email = {
              operation: "like",
              value: '*' + vm.searched.email.replace(' ', '*') + '*'
            }
          }

          loadPage();
        }

        function toggleAdvanceSearch() {
          vm.advanceSearch = !vm.advanceSearch;
          delete vm.searched.state;
          delete vm.searched.email;
          delete vm.searched.username;
        }


        function isEmpty() {
          return vm.students.length == 0;
        }

        function isLoaded() {
          return vm.loaded;
        }

        function handleDeleteSuccess() {
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
