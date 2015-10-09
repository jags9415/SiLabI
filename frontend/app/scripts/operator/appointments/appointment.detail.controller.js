(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('AppointmentDetailController', AppointmentDetailController);

  AppointmentDetailController.$inject = ['$scope', 'AppointmentService', 'MessageService', 'StudentService', 'SoftwareService', 'LabService', '$location', '$routeParams'];

function AppointmentDetailController($scope, AppointmentService, MessageService, StudentService, SoftwareService, LabService, $location, $routeParams) {
  var vm = this;
  vm.student = {};
  vm.software = {};
  vm.laboratory = {};
  vm.software_list = [];
  vm.courses = [];  
  vm.laboratories = [];
  vm.states = [];  

  vm.searchStudent = searchStudent;
  vm.fieldsReady = fieldsReady;
  vm.update = updateAppointment;

  activate();

  function activate() {
    vm.id = $routeParams.id;
    getAppointment();
    getSoftware();
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

  function searchStudent()
  {
    if(vm.student_username)
    {
      StudentService.GetOne(vm.student_username)
    .then(setStudent)
    .catch(handleError);
    }
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

function getLaboratories () {

  LabService.GetAll()
  .then(setLaboratories)
  .catch(handleError);
}

function setAppointment (data) {
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
  AppointmentService.Update(vm.id, app)
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
