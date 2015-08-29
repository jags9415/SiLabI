(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentListController', StudentListController);

    StudentListController.$inject = ['$location', 'StudentService', 'MessageService'];

    function StudentListController($location, StudentService, MessageService) {
      var vm = this;
        vm.students = [];
        vm.pageNumber = 1;
        vm.noStudents = noStudents;
        vm.loadStudents = loadStudents;
        vm.goToNextPage = goToNextPage;
        vm.goToPreviuosPage = goToPreviuosPage;
        vm.seeStudentDetail = seeStudentDetail;
        vm.delete = deleteStudent;

        function loadStudents() {
          var page = vm.pageNumber;
          StudentService.GetAll(page)
          .then(handleGetAllSuccess)
          .catch(handleRequestError);
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

        function handleGetAllSuccess(result) {
          vm.students = result.results;
          return vm.students;
        }

        function handleRequestError(data) {
          if (data.status === 404)
            MessageService.error("No se pudo conectar con el servidor");
          else
            MessageService.error(data.description);
        }

        function deleteStudent(StudentID) {
          StudentService.Delete(StudentID)
          .then(handleDeleteSuccess)
          .catch(handleRequestError);
        }

        function handleDeleteSuccess(result) {
          MessageService.success("Estudiante eliminado.");
          loadStudents();
        }

        function noStudents(){
          return vm.students.length == 0;
        }
    }
})();
