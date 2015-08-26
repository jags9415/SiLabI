(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentListController', StudentListController);

    StudentListController.$inject = ['$scope','$location', 'StudentService'];

    function StudentListController($scope, $location, StudentService) {
        $scope.students = [];
        $scope.pageNumber = 1;
        $scope.noStudents = noStudents;
        $scope.loadStudents = loadStudents;
        $scope.goToNextPage = goToNextPage;
        $scope.goToPreviuosPage = goToPreviuosPage;
        $scope.seeStudentDetail = seeStudentDetail;

        function loadStudents() {
          var page = $scope.pageNumber;
          StudentService.getByPage(page)
          .then(getStudents)
          .catch(showError);
        }

        function goToNextPage() {
          $scope.pageNumber++;
          $scope.loadStudents();
        }

        function goToPreviuosPage() {
          $scope.pageNumber--;
          $scope.loadStudents();
        }

        function seeStudentDetail(studentUsername) {
          $location.path('/Operador/Estudiantes/' + studentUsername);
        }

        function getStudents(result) {
          $scope.students = result.results;
          return $scope.students;
        }

        function showError(error) {
          if (error.status === 404)
            alert("No se pudo conectar con el servidor");
          else
            alert(error.data.description);
        }

        function noStudents(){
          return $scope.students.length == 0;
        }
    }
})();
