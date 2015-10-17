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

    vm.searchProfessor = searchProfessor;

    activate();

    function activate() {
      //getLaboratories();
    }

    function searchProfessor() {
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


    function clearFields() {
      $scope.$broadcast('show-errors-reset');

      delete vm.professor;
      delete vm.groups;
      delete vm.available_dates;
      delete vm.available_hours;
      delete vm.software_list;
      delete vm.selected_date;
      delete vm.selected_software;
      delete vm.selected_hour;
      delete vm.selected_laboratory;
      delete vm.group;
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
