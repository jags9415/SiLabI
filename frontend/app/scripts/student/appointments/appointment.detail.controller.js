(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('StudentAppDetailController', AppointmentDetailController);

  AppointmentDetailController.$inject = ['StudentAppService', 'AppointmentDateService', 'MessageService', 'StudentService', 'SoftwareService', '$routeParams'];

function AppointmentDetailController(StudentAppService, AppointmentDateService, MessageService, StudentService, SoftwareService, $routeParams) {

  var vm = this;
  vm.software = {};
  vm.software_list = [];
  vm.courses = [];
  vm.states = [];

  vm.update = updateAppointment;

  activate();

  function activate() {
    vm.student_id = $routeParams.student_id;
    vm.app_id = $routeParams.app_id;
    getAppointment();
    getSoftware();
  }

  function getAppointment () {
    StudentAppService.GetOne(vm.student_id, vm.app_id)
    .then(setAppointment)
    .catch(handleError);
  }

function getSoftware () {

  SoftwareService.GetAll()
  .then(setSoftware)
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

function setAppointment (data) {
  console.log(data);
  vm.appointment = data;
  vm.student = data.student;
  vm.date = data.date;
  vm.laboratory = data.laboratory;
  vm.software = data.software;
  var i = data.date.indexOf("T");
  var date_day = data.date.substring(0, i);
  var date_hour = data.date.substring(i+1, data.date.length - 7);
  vm.hour = date_hour;
}

function setLaboratories (data) {
  vm.laboratories = data.results;
}

function setSoftware(data) {
vm.software_list = data.results;
}

function updateAppointment () {
  var date = new Date(vm.date.getFullYear(), vm.date.getMonth(), vm.date.getUTCDate(), vm.hour.slice(0, 2));
  console.log(date.toJSON());
  var app =
  {
    "laboratory": vm.laboratory.name,
    "software": vm.software.code,
    "date": date.toJSON()
  }
  StudentAppService.Update(vm.student_id, vm.app_id, app)
  .then(handleSuccess)
  .catch(handleError);
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
