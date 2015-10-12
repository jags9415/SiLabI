(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('AppointmentDetailController', AppointmentDetailController);

  AppointmentDetailController.$inject = ['$scope', 'AppointmentService', 'AppointmentDateService', 'MessageService', 'StudentService', 'SoftwareService', 'PeriodService', 'LabService', 'StateService', '$location', '$routeParams'];

  function AppointmentDetailController($scope, AppointmentService, AppointmentDateService, MessageService, StudentService, SoftwareService, PeriodService, LabService, StateService, $location, $routeParams) {
    var vm = this;
    vm.selected_software = {};
    vm.software_list = [];
    vm.courses = [];
    vm.laboratories = [];
    vm.states = [];
    vm.groups = [];
    vm.available_dates = [];
    vm.available_hours = [];
    vm.selected_state = {};
    vm.selected_laboratory = {};

    vm.request = {
      fields: "id,date,state,created_at,updated_at,student.full_name,student.username,group.number,group.course.name,software.code,software.name,laboratory.name"
    }

    vm.lab_request = {
      fields: "name"
    }

    vm.date_request = {
      fields: "date,laboratory.name"
    };

    vm.groups_request = {
      fields : "id,number,course.name"
    };

    vm.software_request = {
      fields: "code,name"
    };

    vm.setAvailableHours = setAvailableHours;
    vm.changeLaboratory = changeLaboratory;
    vm.searchSoftware = searchSoftware;
    vm.setSoftware = setSoftware;
    vm.fieldsReady = fieldsReady;
    vm.update = updateAppointment;
    vm.delete = deleteAppointment;

    activate();

    function activate() {
      vm.id = $routeParams.id;
      getStates();
      getLaboratories();
      getAppointment();
    }

    function getAppointment () {
      AppointmentService.GetOne(vm.id, vm.request)
      .then(setAppointment)
      .then(setGroups)
      .then(setAvailableDates)
      .catch(handleError);
    }

    function fieldsReady () {
      return vm.laboratory  && vm.software && vm.date && vm.hour;
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

      return StudentService.GetGroups(vm.appointment.student.username, vm.groups_request);
    }

    function getAvailableDates() {
        return AppointmentService.GetAvailable(vm.appointment.student.username, vm.date_request);
    }

    function getStates () {
      StateService.GetAppointmentStates()
      .then(setStates)
      .catch(handleError);
    }

    function getLaboratories () {
      LabService.GetAll(vm.lab_request)
      .then(setLaboratories)
      .catch(handleError);
    }

    function setAppointment (data) {
      vm.appointment = data;
      return getGroups();
    }

    function setStates(states) {
      vm.states = states;
      vm.states = _.reject(vm.states, function (state) { return state.value == "*" });
      vm.states = _.map(vm.states, 'name');
    }

    function setLaboratories (data) {
      vm.laboratories = data.results;
    }

    function setSoftware (data) {
      vm.appointment.software = data;
    }

    function setGroups (groups) {
      vm.groups = groups;
      return getAvailableDates();
    }

    function setAvailableDates (dates) {
      vm.available_dates = AppointmentDateService.ParseAvailableDates(dates);

      for (var i = 0; i < vm.available_dates.length; i++) {
        if (moment(vm.available_dates[i].day).isSame(vm.appointment.date, 'day')) {
          vm.selected_date = vm.available_dates[i];
          setAvailableHours();
          return;
        }
      }
    }

    function setAvailableHours() {
      if (vm.selected_date) {
        vm.available_hours = vm.selected_date.hoursByLab;

        for (var i = 0; i < vm.available_hours.length; i++) {
          if (moment(vm.available_hours[i].full_date).isSame(vm.appointment.date, 'hour')) {
            vm.selected_hour = vm.available_hours[i];
            return;
          }
        }
      }
    }

    function changeLaboratory () {
      if (vm.selected_hour) {
        vm.selected_laboratory = vm.selected_hour.laboratory;
      }
    }

    function updateAppointment () {
      if (vm.selected_date && vm.selected_hour) {
        vm.date = vm.selected_date.day + "T" + vm.selected_hour.hour + ":00.000";
      }

      var app = {
        "date": vm.date,
        "laboratory": vm.appointment.laboratory.name,
        "software": vm.appointment.software.code,
        "group": vm.appointment.group.id,
        "state": vm.appointment.state
      }

      AppointmentService.Update(vm.id, app)
      .then(handleSuccess)
      .catch(handleError);
    }

    function deleteAppointment() {
      MessageService.confirm("¿Desea realmente eliminar esta cita?")
      .then(function() {
        AppointmentService.Delete(vm.id)
        .then(redirectPage)
        .catch(handleError);
      });
    }

    function redirectPage () {
      $location.path("/Operador/Citas");
    }

    function handleSuccess (data) {
      MessageService.success("Cita actualizada.");
      setAppointment(data);
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
