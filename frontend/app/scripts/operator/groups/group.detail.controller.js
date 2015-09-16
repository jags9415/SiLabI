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
      vm.student = {};
      vm.id = $routeParams.id;
      vm.update = updateGroup;
      vm.delete = deleteGroup;

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
          GroupService.Update(vm.group.id, vm.group)
          .then(setGroup)
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
        //setStudents();
      }

      function setStudent(user) {
        if(!contains(user))
        {
          vm.students.push(user);
          vm.group.students.push(user.id);
        }
      }

      function setStudents () {
        for (var i = 0; i < vm.group.students.length; i++) 
        {
          StudentService.GetOne(vm.student_username).then(
            function(data) 
            {
              vm.students.push(data);
            },
            function(error)
            {
              handleError(error);
            });
        }
      }

      function deleteStudent (id) {
        var i;
        for (i = 0; i < vm.students.length; i++) 
        {
          if(vm.students[i].id == id)
          {
            vm.students.splice(i, 1);
            vm.group.students.splice(i, 1)
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

      function redirectTogroups(result) {
        $location.path('/Operador/Grupos');
      }

      function handleError(data) {
        MessageService.error(data.description);
      }
    }
})();
