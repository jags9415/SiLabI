(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('AppointmentCreateController', AppointmentCreateController);

  AppointmentCreateController.$inject = ['$scope', 'AppointmentService', 'MessageService', 'StudentService', 'SoftwareService', 'LabService', '$location'];

function AppointmentCreateController($scope, AppointmentService, MessageService, StudentService, SoftwareService, LabService, $location) {
  var vm = this;
  vm.student = {};
  vm.software_list = [];
  vm.courses = [];  
  vm.laboratories = [];

  vm.searchStudent = searchStudent;
  vm.fieldsReady = fieldsReady;
  vm.create = createAppointment;

  activate();

  function activate() {
    getSoftware();
    getLaboratories();
  }

  function fieldsReady () {
    return vm.student_username && vm.laboratory  && vm.software && vm.date && vm.hour;
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

function getLaboratories () {

  LabService.GetAll()
  .then(setLaboratories)
  .catch(handleError);
}

function setStudent (student) {
  vm.student = student;
}

function setLaboratories (data) {
  vm.laboratories = data.results;
}

function setSoftware(data) {
vm.software_list = data.results;
}

function createAppointment () {
  var date = new Date(vm.date.getFullYear(), vm.date.getMonth(), vm.date.getUTCDate(), vm.hour.slice(0, 2));
  var app =
  {
    "student": vm.student_username,
    "laboratory": vm.laboratory.name,
    "software": vm.software.code,
    "date": date.toJSON()
  }
  AppointmentService.Create(app)
  .then(handleSuccess)
  .catch(handleError);
}

function handleSuccess (data) {
  MessageService.success("Cita creada con éxito.");
        delete vm.student;
        delete vm.student_username;
        delete vm.courses;
}

function handleError(data) {
  MessageService.error(data.description);
}
}
})();
