(function() {
    'use strict';

    angular
      .module('silabi')
      .controller('GroupDetailController', GroupDetailController);

    GroupDetailController.$inject = ['$scope', '$routeParams', '$location', 'GroupService', 'ProfessorService', 'CourseService', 'StudentService', 'MessageService', 'PeriodService', 'StateService', 'lodash'];

    function GroupDetailController($scope, $routeParams, $location, GroupService, ProfessorService, CourseService, StudentService, MessageService, PeriodService, StateService, _) {
      var vm = this;

      vm.group = {};
      vm.periods = [];
      vm.students = [];
      vm.slicedStudents = [];
      vm.student = {};
      vm.id = $routeParams.id;
      vm.isStudentsModified = false;
      vm.page = 1;
      vm.limit = 15;

      vm.groupRequest = {
        fields: 'id,period,number,state,created_at,updated_at,professor.full_name,professor.username,course.code,course.name'
      };

      vm.studentRequest = {
        fields: 'id,username,full_name'
      };

      vm.update = updateGroup;
      vm.delete = deleteGroup;
      vm.deleteStudent = deleteStudent;
      vm.searchStudent = searchStudent;
      vm.sliceStudents = sliceStudents;
      vm.getProfessors = getProfessors;
      vm.setProfessor = setProfessor;
      vm.checkProfessor = checkProfessor;
      vm.getCourses = getCourses;
      vm.setCourse = setCourse;
      vm.checkCourse = checkCourse;

      activate();

      function activate() {
        PeriodService.GetAll()
        .then(setPeriods)
        .catch(handleError);

        StateService.GetLabStates()
        .then(setStates)
        .catch(handleError);

        GroupService.GetOne(vm.id, vm.groupRequest)
        .then(setGroup)
        .then(setStudents)
        .catch(handleError);
      }

      function updateGroup() {
        if (!_.isEmpty(vm.group)) {
          vm.group.period.year = vm.year;

          var request = {
              'number': vm.group.number,
              'professor': vm.group.professor.username,
              'course': vm.group.course.code,
              'period': vm.group.period,
              'state': vm.group.state
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
        if (!_.isEmpty(vm.group)) {
          MessageService.confirm('¿Desea realmente eliminar este grupo?')
          .then(function () {
            GroupService.Delete(vm.group.id)
            .then(redirectToGroups)
            .catch(handleError);
            }
          );
        }
      }

      function getProfessors(name) {
        var request = {
          fields: 'id,username,full_name',
          limit: 10,
          query: {
            'full_name': {
              operation: 'like',
              value: '*' + name + '*'
            }
          }
        };

        return ProfessorService.GetAll(request)
        .then(function(data) {
          return data.results;
        });
      }

      function getCourses(name) {
        var request = {
          field: 'id,code,name',
          limit: 10,
          query: {
            name: {
              operation: 'like',
              value: '*' + name + '*'
            }
          }
        };

        return CourseService.GetAll(request)
        .then(function(data) {
          return data.results;
        });
      }

      function checkProfessor() {
        if (vm['professor_name'] !== vm.group.professor['full_name'] || _.isEmpty(vm.group.professor)) {
          vm['professor_name'] = '';
          vm.group.professor = {};
        }
      }

      function checkCourse() {
        if (vm['course_name'] !== vm.group.course.name || _.isEmpty(vm.group.course)) {
          vm['course_name'] = '';
          vm.group.course = {};
        }
      }

      function setProfessor(user) {
        vm.group.professor = user;
        vm['professor_name'] = user['full_name'];
      }

      function setCourse(course) {
        vm.group.course = course;
        vm['course_name'] = course.name;
      }

      function setGroup(group) {
        vm.group = group;
        vm['professor_name'] = group.professor['full_name'];
        vm['course_name'] = group.course.name;
        vm.year = group.period.year;
        return GroupService.GetStudents(vm.group.id, vm.studentRequest);
      }

      function setStudents(students) {
        vm.students = students;
        sliceStudents();
      }

      function setPeriods(periods) {
        vm.periods = periods;
      }

      function setStates(States) {
        vm.states = [];
        for (var i = 0; i < States.length; i++) {
          if(States[i].value  != "*")
          {
            vm.states.push(States[i].value);
          }
        };
      }

      function showUpdate(group) {
        setGroup(group);
        vm.isStudentsModified = false;
        $scope.$broadcast('show-errors-reset');
        MessageService.success('Grupo actualizado.');
      }

      function searchStudent() {
        if (vm['student_username']) {
          StudentService.GetOne(vm['student_username'], vm.studentRequest)
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
          MessageService.info('El estudiante seleccionado ya se encuentra en la lista.');
        }
      }

      function deleteStudent(id) {
        for (var i = 0; i < vm.students.length; i++) {
          if (vm.students[i].id === id) {
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
