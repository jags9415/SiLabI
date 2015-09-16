(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('GroupCreateController', GroupCreateController);

    GroupCreateController.$inject = ['GroupService', 'CourseService', 'StudentService', 'ProfessorService', 'PeriodService', 'MessageService'];

    function GroupCreateController(GroupService, CourseService, StudentService, ProfessorService, PeriodService, MessageService) {
      var vm = this;
      vm.periods = [];
      vm.courses = [];
      vm.students = [];
      vm.currentYear = new Date().getFullYear();
      vm.professor = {};
      vm.course = {};
      vm.year = vm.currentYear;
      vm.searchProfessor = searchProfessor;
      vm.searchStudent = searchStudent;
      vm.create = create;
      vm.create_group = {};
      vm.deleteStudent = deleteStudent;
      vm.fieldsReady = fieldsReady;
      vm.course_request = {
        fields : "id,code,name"
      };

      activate();

      function activate() {
        getPeriods();
        getCourses();
      }

      function fieldsReady () 
      {
        return vm.professor && vm.period && vm.year && vm.course && vm.create_group.number; 
      }

      function getPeriods() {
        PeriodService.GetAll()
        .then(setPeriods)
        .catch(handleError);
      }

      function getCourses () {
        CourseService.GetAll(vm.course_request)
        .then(setCourses)
        .catch(handleError);
      }

      function searchProfessor() {
        vm.professor = {};
        if (vm.professor_username) {
          ProfessorService.GetOne(vm.professor_username)
          .then(setProfessor)
          .catch(handleError);
        }
      }

      function searchStudent() {
        vm.student = {};
        if (vm.student_username) {
          StudentService.GetOne(vm.student_username)
          .then(setStudent)
          .catch(handleError);
        }
      }

      function create() {
        if (vm.professor && vm.period && vm.year && vm.course && vm.create_group.number) 
        {
          vm.period.year = vm.year;
          vm.create_group.period = vm.period;
          vm.create_group.course = vm.course.code;
          vm.create_group.professor = vm.professor.username;
          vm.create_group.students = getStudents();
          GroupService.Create(vm.create_group)
          .then(handleCreateSuccess)
          .catch(handleError);
        }
      }

      function setPeriods(periods) {
        vm.periods = periods;
        vm.period = periods[0];
      }

      function setCourses(courses) {
        vm.courses = courses.results;
        vm.course = course[0];
      }

      function setProfessor(user) {
        vm.professor = user;
      }

      function setStudent(user) {
        if(!contains(user))
        {
          vm.students.push(user);
        }
      }

      function deleteStudent (id) {
        var i;
        for (i = 0; i < vm.students.length; i++) 
        {
          if(vm.students[i].id == id)
          {
            vm.students.splice(i, 1);
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

      function getStudents () {
        var stds = [];
        for (var i = 0; i < vm.students.length; i++) 
        {
          stds.push(vm.students[i].username);
        }
        return stds;
      }

      function handleCreateSuccess() {
        MessageService.success("Grupo creado con éxito.");
        delete vm.professor;
        delete vm.students;
      }

      function handleError(data) {
        MessageService.error(data.description);
      }
    }
})();
