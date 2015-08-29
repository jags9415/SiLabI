(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentListController', StudentListController);

    StudentListController.$inject = ['$location', 'StudentService'];

    function StudentListController($location, StudentService) {
      var vm = this;
        vm.students = [];
        vm.pageNumber = 1;
        vm.noStudents = noStudents;
        vm.loadStudents = loadStudents;
        vm.goToNextPage = goToNextPage;
        vm.goToPreviuosPage = goToPreviuosPage;
        vm.seeStudentDetail = seeStudentDetail;

        function loadStudents() {
          var page = vm.pageNumber;
          StudentService.GetAll(page)
          .then(getStudents)
          .catch(showError);
        }

        function goToNextPage() {
          vm.pageNumber++;
          vm.loadStudents();
        }

        function goToPreviuosPage() {
          vm.pageNumber--;
          vm.loadStudents();
        }

        function seeStudentDetail(studentUsername) {
          $location.path('/Operador/Estudiantes/' + studentUsername);
        }

        function getStudents(result) {
          vm.students = result.results;
          return vm.students;
        }

        function showError(error) {
          if (error.status === 404)
            alert("No se pudo conectar con el servidor");
          else
            alert(error.data.description);
        }

        function noStudents(){
          return vm.students.length == 0;
        }
    }
})();
