(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('ReservationCreateController', AppointmentCreateController);

  AppointmentCreateController.$inject = ['$scope', 'ReservationService', 'MessageService', 'DateService', 'ProfessorService', 'SoftwareService', 'PeriodService', 'LabService', '$location'];

  function AppointmentCreateController($scope, ReservationService, MessageService, DateService, ProfessorService, SoftwareService, PeriodService, LabService, $location) {
    var vm = this;
    vm.professor = {};
    vm.start_hours = [];
    vm.end_hours = [];
    vm.end_hours_sliced = [];

    vm.professor_request = {
      fields: "id,username,full_name"
    };

    vm.selected_date;
    vm.datepicker_open = false;
    vm.min_date = new Date();

    vm.openDatePicker = openDatePicker;
    vm.loadEndHours = loadEndHours;
    vm.searchProfessor = searchProfessor;

    activate();

    function activate() {
      //getLaboratories();
      getHours();
    }

    function searchProfessor() {
      clearFields();
      ProfessorService.GetOne(vm.professor_username, vm.professor_request)
      .then(setProfessor)
      .catch(handleError);
    }

    function getLaboratories () {
      LabService.GetAll()
      .then(setLaboratories)
      .catch(handleError);
    }

    function setProfessor (professor) {
      vm.professor = professor;
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

    function getHours() {
      vm.start_hours = DateService.GetReservationStartHours();
      vm.end_hours = DateService.GetReservationEndHours();
    }

    function openDatePicker($event){
      if ($event)
      {
        $event.preventDefault();
        $event.stopPropagation();
      }
      vm.datepicker_open = true;
    }


    function clearFields() {
      $scope.$broadcast('show-errors-reset');

      delete vm.professor;
      delete vm.selected_date;
    }

    function handleSuccess (data) {
      MessageService.success("Cita creada.");
      clearFields();
      delete vm.student_username;
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
