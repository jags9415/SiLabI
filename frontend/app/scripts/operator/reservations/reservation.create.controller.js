(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('ReservationCreateController', AppointmentCreateController);

  AppointmentCreateController.$inject = ['ReservationService', 'MessageService', 'ProfessorService', 'SoftwareService', 'PeriodService', 'LabService', '$location'];

  function AppointmentCreateController(ReservationService, MessageService, ProfessorService, SoftwareService, PeriodService, LabService, $location) {
    var vm = this;
    vm.professor = {};

    vm.professor_request = {
      fields: "id,username,full_name"
    }

    vm.selected_date;
    vm.datepicker_open = false;
    vm.min_date = new Date();

    vm.openDatePicker = openDatePicker;
    vm.searchProfessor = searchProfessor;

    activate();

    function activate() {
      //getLaboratories();
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
