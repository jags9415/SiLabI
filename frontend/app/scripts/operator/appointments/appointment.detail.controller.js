(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('AppointmentDetailController', AppointmentDetailController);

  AppointmentDetailController.$inject = ['$scope', 'AppointmentService', 'AppointmentDateService', 'MessageService', 'StudentService', 'SoftwareService', 'PeriodService', 'LabService', 'StateService', '$location', '$routeParams'];

function AppointmentDetailController($scope, AppointmentService, AppointmentDateService, MessageService, StudentService, SoftwareService, PeriodService, LabService, StateService, $location, $routeParams) {
  var vm = this;
  vm.student = {};
  vm.software = {};
  vm.laboratory = {};
  vm.software_list = [];
  vm.courses = [];  
  vm.laboratories = [];
  vm.states = [];  
  vm.groups = [];
  vm.available_dates = [];
  vm.available_hours = [];
  vm.groups_request = {fields : "id,course"};
  vm.software_request = {};
  vm.selected_state = {};

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
    getAppointment();
    getLaboratories();
  }

  function getAppointment () {
    AppointmentService.GetOne(vm.id)
    .then(setAppointment)
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
  StudentService.GetGroups(vm.student.username, vm.groups_request)
  .then(setGroups)
  .catch(handleError);
}


function getStates () {
StateService.GetAppointmentStates()
  .then(setStates)
  .catch(handleError);
}

function setStates(states) {
  vm.states = states;
}

function getLaboratories () {

  LabService.GetAll()
  .then(setLaboratories)
  .catch(handleError);
}

function setAppointment (data) {
  vm.appointment = data;
  vm.student = data.student;
  console.log(vm.student.username);
  vm.date = data.date;
  vm.laboratory = data.laboratory;
  vm.software = data.software;
  getGroups();
  getAvailableDates();
  getStates();
}

function setLaboratories (data) {
  vm.laboratories = data.results;
}

function setSoftware (data) {
  vm.selected_software = data;
}

function setGroups (groups) {
  vm.groups = groups;
}

function getAvailableDates()
{
  if(vm.student.username)
  {
    AppointmentService.GetAvailable(vm.request, vm.student.username)
    .then(setAvailableDates)
  .catch(handleError);
  }
}

function setAvailableDates (dates) {
  vm.available_dates = AppointmentDateService.ParseAvailableDates(dates);
}


function setAvailableHours() {
  if(vm.selected_date)
  {
    vm.available_hours = vm.selected_date.hoursByLab;
  }
}

function changeLaboratory () {
  if(vm.selected_hour)
  {
    vm.selected_laboratory = vm.selected_hour.laboratory;
  }
}

function updateAppointment () {
  var app =
  {
    "laboratory": vm.selected_laboratory.name,
    "software": vm.software.code,
    "date": vm.selected_date.day+"T"+vm.selected_hour.hour+":00.000",
    "group": vm.group.id,
    "state": vm.selected_state.name
  }
  AppointmentService.Update(vm.id, app)
  .then(handleSuccess)
  .catch(handleError);
}

function deleteAppointment() {
  MessageService.confirm("¿Desea realmente eliminar esta cita?")
  .then(function() {
    AppointmentService.Delete(vm.id)
    .then(loadPage)
    .catch(handleError);
  });
}

function handleSuccess (data) {
  MessageService.success("Cita actualizada con éxito.");
  setAppointment(data);
}

function handleError(data) {
  MessageService.error(data.description);
}
}
})();
