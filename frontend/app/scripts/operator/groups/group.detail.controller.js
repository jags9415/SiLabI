(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('GroupDetailController', GroupDetailController);

    GroupDetailController.$inject = ['$scope', '$routeParams', '$location', 'GroupService', 'StudentService', 'MessageService'];

    function GroupDetailController($scope, $routeParams, $location, GroupService, StudentService, MessageService) {
      var vm = this;
      vm.group = {};
      vm.students = [];
      vm.slicedStudents = [];
      vm.student = {};
      vm.id = $routeParams.id;
      vm.isStudentsModified = false;
      vm.page = 1;
      vm.limit = 15;

      vm.update = updateGroup;
      vm.delete = deleteGroup;
      vm.deleteStudent = deleteStudent;
      vm.searchStudent = searchStudent;
      vm.sliceStudents = sliceStudents;

      vm.studentRequest = {
        fields: "id,username,full_name"
      }

      activate();

      function activate() {
        GroupService.GetOne(vm.id)
        .then(setGroup)
        .then(setStudents)
        .catch(handleError);
      }

      function updateGroup() {
        if (vm.group) {
          var request = {
              "number": vm.group.number
          };

          if (vm.isStudentsModified) {
            request.students = getStudentsUsername();
          }

          GroupService.Update(vm.group.id, request)
          .then(showUpdate)
          .catch(handleError);
        }
      }

      function deleteGroup() {
        if (vm.group) {
          MessageService.confirm("¿Desea realmente eliminar este grupo?")
          .then(function () {
            GroupService.Delete(vm.group.id)
            .then(redirectToGroups)
            .catch(handleError);
            }
          );
        }
      }

      function setGroup(group) {
        vm.group = group;
        return GroupService.GetStudents(vm.group.id, vm.studentRequest);
      }

      function setStudents(students) {
        vm.students = students;
        sliceStudents();
      }

      function showUpdate(group) {
        setGroup(group);
        $scope.$broadcast('show-errors-reset');
        vm.isStudentsModified = false;
        MessageService.success("Grupo actualizado.");
      }

      function searchStudent() {
        if (vm.student_username) {
          StudentService.GetOne(vm.student_username, vm.studentRequest)
          .then(addStudent)
          .catch(handleError);
        }
      }

      function addStudent(user) {
        if (!contains(user)) {
          vm.students.unshift(user);
          sliceStudents();
          vm.isStudentsModified = true;
        }
        else {
          MessageService.info("El estudiante seleccionado ya se encuentra en la lista.");
        }
      }

      function deleteStudent(id) {
        for (var i = 0; i < vm.students.length; i++) {
          if (vm.students[i].id == id) {
            vm.students.splice(i, 1);
            sliceStudents();
            vm.isStudentsModified = true;
            break;
          }
        }
      }

      function contains(student) {
        return _.any(vm.students, _.matches(student));
      }

      function getStudentsUsername() {
        return _.map(vm.students, 'username');
      }

      function redirectToGroups(result) {
        $location.path('/Operador/Grupos');
      }

      function sliceStudents() {
        vm.slicedStudents = vm.students.slice((vm.page - 1) * vm.limit, vm.page * vm.limit);
      }

      function handleError(data) {
        MessageService.error(data.description);
      }
    }
})();
