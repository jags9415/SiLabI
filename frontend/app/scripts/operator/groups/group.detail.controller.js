(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('GroupDetailController', GroupDetailController);

    GroupDetailController.$inject = ['$routeParams', '$location', 'GroupService', 'StudentService', 'MessageService'];

    function GroupDetailController($routeParams, $location, GroupService, StudentService, MessageService) {
      var vm = this;
      vm.group = {};
      vm.students = [];
      vm.slicedStudents = [];
      vm.student = {};
      vm.id = $routeParams.id;
      vm.page = 1;
      vm.limit = 15;

      vm.update = updateGroup;
      vm.delete = deleteGroup;
      vm.deleteStudent = deleteStudent;
      vm.searchStudent = searchStudent;
      vm.sliceStudents = sliceStudents;

      activate();

      function activate() {
        GroupService.GetOne(vm.id)
        .then(setGroup)
        .catch(handleError);
      }

      function fieldsReady ()
      {
        return vm.group.number != "";
      }

      function updateGroup() {
        if (vm.id) {
          var stds = getStudentsId();
          if(stds.length > 0)
          {
            vm.group.students = stds;
          }

          var request =
          {
              "students": vm.group.students,
              "number": vm.group.number
          };
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
            .then(redirectTogroups)
            .catch(handleError);
            }
          );
        }
      }

      function setGroup(group) {
        vm.group = group;
        getStudents();
      }

      function showUpdate (group) {
        setGroup(group);
        MessageService.success("Grupo actualizado con éxito.");
      }

      function setStudent(user) {
        if(!contains(user))
        {
          vm.students.push(user);
        }
      }

      function getStudents() {
        GroupService.GetStudents(vm.group.id)
        .then(setStudents)
        .catch(handleError);
      }

      function setStudents(students) {
        vm.students = students;
        sliceStudents();
      }

      function deleteStudent (id) {
        var i;
        for (i = 0; i < vm.students.length; i++) {
          if(vm.students[i].id == id) {
            vm.students.splice(i, 1);
            sliceStudents();
            break;
          }
        }
      }

      function contains (element) {
        for (var i = 0; i < vm.students.length; i++)
        {
          if(vm.students[i].id == element.id)
          {
            return true;
          }
        }
        return false;
      }

      function searchStudent() {
        vm.student = {};
        if (vm.student_username) {
          StudentService.GetOne(vm.student_username)
          .then(setStudent)
          .catch(handleError);
        }
      }

      function getStudentsId () {
        var stds = [];
        for (var i = 0; i < vm.students.length; i++)
        {
          stds.push(vm.students[i].username);
        }
        return stds;
      }

      function redirectTogroups(result) {
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
