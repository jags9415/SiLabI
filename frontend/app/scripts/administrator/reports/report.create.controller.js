(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('ReportCreateController', ReportCreateController);

  ReportCreateController.$inject = ['$scope', '$routeParams', 'MessageService', 'PeriodService', 'GroupService', 'DateService', 'StudentService', 'ProfessorService', 'ReportingService', '$location', '$localStorage'];

  function ReportCreateController($scope, $routeParams, MessageService, PeriodService, GroupService,  DateService, StudentService, ProfessorService, ReportingService, $location, $localStorage) {
    var vm = this;
    vm.groups = [];
    vm.start_hours = [];
    vm.end_hours = [];
    vm.end_hours_sliced = [];
    vm.min_date = new Date();
    vm.datepicker_open = false;
    vm.datepicker2_open = false;
    vm.$storage = $localStorage;
    vm.selected_start_date = null;
    vm.selected_end_date = null; 
 
    vm.groups_request = {
      fields: "id,number,course.name",
      limit: 10
    };

    vm.software_request = {
      fields: "id,code,name",
      limit: 10
    };

    vm.reports = [
      {
        name:"Citas por estudiante",
        value: 1
      },
      {
        name:"Citas por grupo",
        value: 2
      },
      {
        name:"Reservaciones por docente",
        value: 3
      },
      {
        name:"Reservaciones por grupo",
        value: 4
      }];
    
    vm.searchGroup = searchGroup;
    vm.setGroup = setGroup;
    vm.openDatePicker1 = openDatePicker1;
    vm.openDatePicker2 = openDatePicker2;
    vm.loadEndHours = loadEndHours;
    vm.getProfessors = getProfessors;
    vm.getStudents = getStudents;
    vm.setProfessor = setProfessor;
    vm.setStudent = setStudent;
    vm.fieldsReady = fieldsReady;
    vm.generateReport =  generateReport;
    vm.setMinDate = setMinDate;

    activate();

  
    function activate() {
      
    }

    function openDatePicker1($event){
      if ($event) 
      {
        $event.preventDefault();
        $event.stopPropagation(); 
      }
      vm.datepicker_open = true;
    }

    function setMinDate () {
      if(vm.selected_start_date)
        {
          vm.min_date = new Date(vm.selected_start_date);
        }
    }

    function openDatePicker2($event){
      if ($event) 
      {
        $event.preventDefault();
        $event.stopPropagation(); 
      }
      vm.datepicker2_open = true;
    }

    function fieldsReady () {
      return !_.isEmpty(vm.selected_report) && ( !_.isEmpty(vm.student) || !_.isEmpty(vm.professor) || !_.isEmpty(vm.group)) 
    }

    function loadEndHours () {
      var index = -1;

      for (var i = 0; i < vm.end_hours.length; i++) 
      {
        if(vm.selected_start_time.value == vm.end_hours[i].value)
        {
          index = i;
        }  
      }
      if(index >= 0)
      {
        vm.end_hours_sliced = vm.end_hours.slice(index+1, vm.end_hours.length);
      }
      else
      {
        vm.end_hours_sliced = vm.end_hours;
      }
    }

    function getProfessors(name) {
      var request = {
        limit: 10,
        query: {
          'username': {
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

    function getStudents(name) {
      var request = {
        limit: 10,
        query: {
          'username': {
            operation: 'like',
            value: '*' + name + '*'
          }
        }
      };

      return StudentService.GetAll(request)
      .then(function(data) {
        return data.results;
      });
    }

    function searchGroup (input) {
      var period = PeriodService.GetCurrentPeriod('Semestre');
      vm.groups_request.query = {};
      vm.groups_request
      vm.groups_request.query["course.name"] = {
        operation: "like",
        value: '*' + input + '*'
      }

      vm.groups_request.query["period.type"] = {
        operation: "eq",
        value: "Semestre"
      };

      vm.groups_request.query["period.value"] = {
        operation: "eq",
        value: period.value
      };

      vm.groups_request.query["period.year"] = {
        operation: "eq",
        value: period.year
      };

      return GroupService.GetAll(vm.groups_request)
      .then(function(data) {
          return data.results;
        });
    }



    function setGroup (data) {
      vm.group = data;
    }

    function setStudent(data) {
      vm.student = data;
    }

    function setProfessor(data) {
      vm.professor = data;
    }


  function generateReport () {
    if(!_.isEmpty(vm.selected_report))
    {
      switch(vm.selected_report.value)
      {
        case 1:
          generateAppointmentsByStudent();
          break;
      }
    }
  }

  function generateAppointmentsByStudent () {
    if(!_.isEmpty(vm.student))
    {
      var start_date = null;
      var end_date = null;
      if(vm.selected_start_date != null)
      {
        start_date = new Date(vm.selected_start_date);
        start_date.setHours(08, 0, 0);
      }
      if(vm.selected_end_date != null)
      {
        end_date = new Date(vm.selected_end_date);
        end_date.setHours(18, 0, 0);
      }
      

      var app_request = 
      {
        period:
        {
          start_date: start_date != null ? start_date.toJSON() : null,
          end_date: end_date != null ? end_date.toJSON() : null
        },
        student: vm.student.username
      };
      ReportingService.setAppointmentsByStudentRequest(app_request);
      redirectToAppsByStudent ();
    }
    else
    {
      MessageService.info("Debe buscar un estudiante.");
    }
  }

  function redirectToAppsByStudent () {
    $location.path('Administrador/Reportes/Citas_Por_Estudiante');
  }

  function handleError(data) {
    MessageService.error(data.description);
  }
}
})();
