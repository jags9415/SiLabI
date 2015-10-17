(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('ReservationCreateController', AppointmentCreateController);

  AppointmentCreateController.$inject = ['$scope', 'ReservationService', 'MessageService', 'DateService', 'ProfessorService', 'SoftwareService', 'PeriodService', 'LabService', 'GroupService', '$location'];

  function AppointmentCreateController($scope, ReservationService, MessageService, DateService, ProfessorService, SoftwareService, PeriodService, LabService, GroupService, $location) {
    var vm = this;
    vm.professor = {};
    vm.start_hours = [];
    vm.end_hours = [];
    vm.end_hours_sliced = [];
    vm.groups = [];
    vm.selected_group = null;

    vm.professor_request = {
      fields: "id,username,full_name"
    };

    vm.groups_request = {
      fields: "id,number,course.name"
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
      getGroups();
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

    function getGroups () {
      var period = PeriodService.GetCurrentPeriod('Semestre');
      vm.groups_request.query = {};

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

      vm.groups_request.query["professor.username"] = {
        operation: "eq",
        value: vm.professor.username
      };

      GroupService.GetAll(vm.groups_request)
      .then(setGroups)
      .catch(handleError);
    }

    function setGroups (data) {
      vm.groups = data.results;
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
