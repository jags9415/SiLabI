(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('ReportCreateController', ReportCreateController);

  ReportCreateController.$inject = ['$scope', '$routeParams', 'ProfessorReservationService', 'MessageService',  'PeriodService', 'GroupService', 'DateService', 'StudentAppService', 'StudentService', 'ProfessorService', '$location', '$localStorage'];

  function ReportCreateController($scope, $routeParams, ProfessorReservationService, MessageService,  PeriodService, GroupService,  DateService, StudentAppService, StudentService, ProfessorService, $location, $localStorage) {
    var vm = this;
    vm.groups = [];
    vm.start_hours = [];
    vm.end_hours = [];
    vm.end_hours_sliced = [];
    vm.min_date = new Date();
    vm.datepicker_open = false;
    vm.datepicker2_open = false;
    vm.$storage = $localStorage;
 
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

    activate();

  
    function activate() {
      getHours();
    }

    function openDatePicker1($event){
      if ($event) 
      {
        $event.preventDefault();
        $event.stopPropagation(); 
      }
      vm.datepicker_open = true;
    }

    function openDatePicker2($event){
      if ($event) 
      {
        $event.preventDefault();
        $event.stopPropagation(); 
      }
      vm.datepicker2_open = true;
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

    function getHours() {
      vm.start_hours = DateService.GetReservationStartHours();
      vm.end_hours = DateService.GetReservationStartHours();
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

    function setStartHour() {
      var start_time = vm.reservation.start_time;
      var start_hour = start_time.substring(start_time.indexOf("T") + 1, start_time.length);
      for (var i = 0; i < vm.start_hours.length; i++) {
        var current_hour = vm.start_hours[i];
        if(start_hour === current_hour.value)
        {
          vm.selected_start_time = current_hour;
          return;
        }
    }
  }

   function setEndHour() {
    var end_time = vm.reservation.end_time;
    var end_hour = end_time.substring(end_time.indexOf("T") + 1, end_time.length);
    for (var i = 0; i < vm.end_hours.length; i++) {
      var current_hour = vm.end_hours[i];
      if(end_hour === current_hour.value)
      {
        vm.selected_end_time = current_hour;
        return;
      }
    }
  }

  function generateReport () {
    if(vm.selected_report)
    {
      switch(vm.selected_report.value)
      {

      }
    }
  }

  function handleSuccess (data) {
    MessageService.success("Reservación actualizada.");
  }

  function handleError(data) {
    MessageService.error(data.description);
  }
}
})();
