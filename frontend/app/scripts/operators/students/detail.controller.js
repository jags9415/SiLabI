(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('StudentsDetailController', StudentsDetail);

    StudentsDetail.$inject = ['$routeParams', 'StudentService'];

    function StudentsDetail($routeParams, StudentService) {
      var vm = this;
        vm.student = {};
        vm.username = $routeParams.username;
        vm.userID;
        vm.requestForUser = requestForUser;
        vm.updateInfo = updateInfo;
        vm.genders = [
          { name: 'Masculino' },
          { name: 'Femenino' }
        ];

        function requestForUser() {
          var username = vm.username;
          StudentService.GetOne(username)
          .then(getUserInfo)
          .catch(showError);
        }

        function getUserInfo(result) {
          vm.student = result;
          vm.userID = result.id;
          return vm.student;
        }

        function showError(error) {
          if (error.status === 404)
            alert("No se pudo conectar con el servidor");
          else
            alert(error.data);
        }

        function updateInfo() {
          var newUserInfo = vm.student;
          var userID = vm.userID;
          StudentService.Update(userID, newUserInfo)
          .then(function(result) {
            vm.student = result;
          })
          .catch(showError);
        }

    }
})();
