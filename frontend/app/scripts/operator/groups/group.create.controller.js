(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('GroupCreateController', GroupCreateController);

    GroupCreateController.$inject = ['$scope', 'GroupService', 'CourseService', 'StudentService', 'ProfessorService', 'PeriodService', 'MessageService', 'lodash'];

    function GroupCreateController($scope, GroupService, CourseService, StudentService, ProfessorService, PeriodService, MessageService, _) {
      var vm = this;

      vm.group = {};
      vm.professor = {};
      vm.course = {};
      vm.periods = [];
      vm.students = [];
      vm.slicedStudents = [];
      vm.page = 1;
      vm.limit = 15;
      vm.currentYear = new Date().getFullYear();
      vm.year = vm.currentYear;

      vm.studentRequest = {
        fields: 'id,username,full_name'
      };

      vm.sliceStudents = sliceStudents;
      vm.getProfessors = getProfessors;
      vm.getCourses = getCourses;
      vm.setProfessor = setProfessor;
      vm.setCourse = setCourse;
      vm.checkProfessor = checkProfessor;
      vm.checkCourse = checkCourse;
      vm.searchStudent = searchStudent;
      vm.create = create;
      vm.deleteStudent = deleteStudent;

      activate();

      function activate() {
        getPeriods();
      }

      function getPeriods() {
        PeriodService.GetAll()
        .then(setPeriods)
        .catch(handleError);
      }

      function getProfessors(name) {
        var request = {
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
        if (vm['professor_name'] !== vm.professor['full_name'] || _.isEmpty(vm.professor)) {
          vm['professor_name'] = '';
          vm.professor = {};
        }
      }

      function checkCourse() {
        if (vm['course_name'] !== vm.course.name || _.isEmpty(vm.course)) {
          vm['course_name'] = '';
          vm.course = {};
        }
      }

      function searchStudent() {
        vm.student = {};
        if (vm['student_username']) {
          StudentService.GetOne(vm['student_username'], vm.studentRequest)
          .then(addStudent)
          .catch(handleError);
        }
      }

      function create() {
        if (!_.isEmpty(vm.professor) && !_.isEmpty(vm.period) && !_.isEmpty(vm.course) && vm.year &&  vm.group.number) {
          vm.period.year = vm.year;
          vm.group.period = vm.period;
          vm.group.course = vm.course.code;
          vm.group.professor = vm.professor.username;
          vm.group.students = getStudentsUsername();

          GroupService.Create(vm.group)
          .then(handleCreateSuccess)
          .catch(handleError);
        }
      }

      function setPeriods(periods) {
        vm.periods = periods;
        vm.period = periods[0];
      }

      function setCourse(course) {
        vm.course = course;
      }

      function setProfessor(user) {
        vm.professor = user;
      }

      function addStudent(user) {
        if (!contains(user)) {
          vm.students.unshift(user);
          vm['student_username'] = '';
          sliceStudents();
        }
        else {
          MessageService.info('El estudiante seleccionado ya se encuentra en la lista.');
        }
      }

      function deleteStudent (id) {
        for (var i = 0; i < vm.students.length; i++) {
          if (vm.students[i].id === id) {
            vm.students.splice(i, 1);
            sliceStudents();
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

      function handleCreateSuccess() {
        MessageService.success('Grupo creado.');
        $scope.$broadcast('show-errors-reset');
        vm.group = {};
        vm.professor = {};
        vm.students = [];
        vm.course = {};
        delete vm['student_username'];
        delete vm['professor_name'];
        delete vm['course_name'];
        sliceStudents();
      }

      function handleError(data) {
        MessageService.error(data.description);
      }

      function sliceStudents() {
        vm.slicedStudents = vm.students.slice((vm.page - 1) * vm.limit, vm.page * vm.limit);
      }
    }
})();
