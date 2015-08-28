(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentsDetailController', StudentsDetail);

    StudentsDetail.$inject = ['$scope','$routeParams', 'StudentService'];

    function StudentsDetail($scope, $routeParams, StudentService) {
        $scope.student = {};
        $scope.username = $routeParams.username;
        $scope.userID;
        $scope.requestForUser = requestForUser;
        $scope.updateInfo = updateInfo;

        function requestForUser() {
          var username = $scope.username;
          StudentService.getByUsername(username)
          .then(getUserInfo)
          .catch(showError);
        }

        function getUserInfo(result) {
          $scope.student = result;
          $scope.userID = result.id;
          return $scope.student;
        }

        function showError(error) {
          if (error.status === 404)
            alert("No se pudo conectar con el servidor");
          else
            alert(error.data.description);
        }

        function updateInfo() {
          var newUserInfo = $scope.student;
          var userID = $scope.userID;
          StudentService.update(userID, newUserInfo)
          .then(function(result) {
            $scope.student = result;
          })
          .catch(showError);
        }

    }
})();
