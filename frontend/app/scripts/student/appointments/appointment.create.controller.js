(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('StudentAppCreateController', AppointmentCreateController);

  AppointmentCreateController.$inject = ['StudentAppService', 'AppointmentDateService', 'MessageService', 'StudentService', 'SoftwareService', 'PeriodService', '$localStorage'];

  function AppointmentCreateController(StudentAppService, AppointmentDateService, MessageService, StudentService, SoftwareService, PeriodService, $localStorage) {
    var vm = this;
    vm.groups = [];
    vm.available_dates = [];
    vm.available_hours = [];
    vm.request = {
        fields : "date,laboratory"
      };

    vm.groups_request = {
      fields : "id,course"
    };

    vm.software_request = {
      fields: "code,name",
      limit: 10
    };

    vm.$storage = $localStorage;

    vm.searchSoftware = searchSoftware;
    vm.setSoftware = setSoftware;
    vm.create = createAppointment;
    vm.setAvailableHours = setAvailableHours;
    vm.changeLaboratory = changeLaboratory;

    activate();

    function activate() {
      vm.student_id = vm.$storage['username'];
      getAvailableDates();
      getGroups();
    }

    function searchSoftware (input) {
      vm.software_request.query = {};

      vm.software_request.query.code = {
        operation: "like",
        value: '*' + input + '*'
      }

      return SoftwareService.GetAll(vm.software_request)
        .then(function(data) {
          return data.results;
        });
    }

    function setSoftware (data) {
      vm.selected_software = data;
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

      StudentService.GetGroups(vm.student_id, vm.groups_request)
      .then(setGroups)
      .catch(handleError);
    }

    function setGroups (groups) {
      vm.groups = groups;
    }

    function getAvailableDates() {
      if(vm.student_id){
        AppointmentDateService.GetAvailable(vm.request, vm.student_id)
        .then(setAvailableDates)
        .catch(handleError);
      }
    }

    function setAvailableDates (dates) {
      vm.available_dates = AppointmentDateService.ParseAvailableDates(dates);
    }


    function setAvailableHours() {
      if(vm.selected_date){
        vm.available_hours = vm.selected_date.hoursByLab;
      }
    }

    function changeLaboratory () {
      if(vm.selected_hour){
        vm.selected_laboratory = vm.selected_hour.laboratory;
      }
    }

    function createAppointment () {
      var app = {
        "software": vm.selected_software.code,
        "date": vm.selected_hour.full_date,
        "group": vm.group.id
      }
      StudentAppService.Create(app, vm.student_id)
      .then(handleSuccess)
      .catch(handleError);
    }

    function handleSuccess (data) {
      MessageService.success("Cita creada con éxito.");
      delete vm.appointment;
      delete vm.selected_software;
      delete vm.selected_hour;
      delete vm.group;
      delete vm.selected_laboratory;
      delete vm.selected_date
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
