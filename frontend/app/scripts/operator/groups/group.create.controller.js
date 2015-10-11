(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('GroupCreateController', GroupCreateController);

    GroupCreateController.$inject = ['GroupService', 'CourseService', 'StudentService', 'ProfessorService', 'PeriodService', 'MessageService', '$location', '$localStorage'];

    function GroupCreateController(GroupService, CourseService, StudentService, ProfessorService, PeriodService, MessageService, $location, $localStorage) {
      var vm = this;

      vm.group = {};
      vm.professor = {};
      vm.course = {};
      vm.periods = [];
      vm.students = [];
      vm.current_page = [];
      vm.currentYear = new Date().getFullYear();
      vm.year = vm.currentYear;
      vm.totalItems = 10;
      vm.limit = 10;
      vm.page = 1;

      vm.getProfessors = getProfessors;
      vm.getCourses = getCourses;
      vm.setProfessor = setProfessor;
      vm.setCourse = setCourse;
      vm.checkProfessor = checkProfessor;
      vm.checkCourse = checkCourse;
      vm.searchStudent = searchStudent;
      vm.create = create;
      vm.deleteStudent = deleteStudent;
      vm.fieldsReady = fieldsReady;
      vm.loadPage = loadPage;

      activate();

      function activate() {
        getPeriods();
        updateStudentsList();
      }

      function fieldsReady () {
        return vm.professor && vm.period && vm.year && vm.course && vm.group.number;
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
            full_name: {
              operation: "like",
              value: '*' + name + '*'
            }
          }
        }

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
              operation: "like",
              value: '*' + name + '*'
            }
          }
        }

        return CourseService.GetAll(request)
        .then(function(data) {
          return data.results;
        });
      }

      function checkProfessor() {
        if (vm.professor_name != vm.professor.full_name || _.isEmpty(vm.professor)) {
          vm.professor_name = "";
          vm.professor = {};
        }
      }

      function checkCourse() {
        if (vm.course_name != vm.course.name || _.isEmpty(vm.course)) {
          vm.course_name = "";
          vm.course = {};
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
        console.log(vm.professor);
        if (vm.professor && vm.period && vm.year && vm.course && vm.group.number) {
          vm.period.year = vm.year;
          vm.group.period = vm.period;
          vm.group.course = vm.course.code;
          vm.group.professor = vm.professor.username;
          vm.group.students = getStudents();

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
        console.log(user);
        vm.professor = user;
      }

      function setStudent(user) {
        if(!contains(user)) {
          vm.students.push(user);
          updateStudentsList();
          $localStorage['current_array'] = vm.students;
        }
      }

      function loadPage() {
        $location.search('page', vm.page);
      }

      function updateStudentsList() {
        var start = (vm.page - 1) * 10; 
        var end = vm.page * 10;
        vm.current_page = vm.students.slice(start, end);
        if(vm.totalItems < vm.students.length)
        {
          vm.totalItems += 10;
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

      function contains(element) {
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
