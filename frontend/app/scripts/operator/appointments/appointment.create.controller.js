(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('AppointmentCreateController', AppointmentCreateController);

  AppointmentCreateController.$inject = ['$scope', 'AppointmentService', 'AppointmentDateService', 'MessageService', 'StudentService', 'SoftwareService', 'LabService', '$location'];

function AppointmentCreateController($scope, AppointmentService, AppointmentDateService, MessageService, StudentService, SoftwareService, LabService, $location) {
  var vm = this;
  vm.student = {};
  vm.software_list = [];
  vm.groups = [];  
  vm.laboratories = [];
  vm.available_dates = [];
  vm.available_hours = [];
  vm.request = {fields : "date,laboratory"};
  vm.groups_request = {fields : "id,course"};
  vm.software_request = {};
  vm.searchStudent = searchStudent;
  vm.fieldsReady = fieldsReady;
  vm.create = createAppointment;
  vm.setAvailableHours = setAvailableHours;
  vm.changeLaboratory = changeLaboratory;
  vm.searchSoftware = searchSoftware;
  vm.setSoftware = setSoftware;

  activate();

  function activate() {
    getLaboratories();
  }

  function fieldsReady () {
    return vm.student_username && vm.selected_laboratory  && vm.selected_software && vm.selected_date && vm.selected_date && vm.group;
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


function getLaboratories () {

  LabService.GetAll()
  .then(setLaboratories)
  .catch(handleError);
}

function getGroups () {
  StudentService.GetGroups(vm.student_username, vm.groups_request)
  .then(setGroups)
  .catch(handleError);
}

function setStudent (student) {
  vm.student = student;
  getAvailableDates();
  getGroups();
}

function setLaboratories (data) {
  vm.laboratories = data.results;
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

function setGroups (groups) {
  vm.groups = groups;
}

function getAvailableDates()
{
  if(vm.student_username)
  {
    AppointmentService.GetAvailable(vm.request, vm.student_username)
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

function createAppointment () {
  var app =
  {
    "student": vm.student_username,
    "laboratory": vm.selected_laboratory.name,
    "software": vm.selected_software.code,
    "date": vm.selected_date.day+"T"+vm.selected_hour.hour+":00.000",
    "group": vm.group.id
  }
  AppointmentService.Create(app)
  .then(handleSuccess)
  .catch(handleError);
}

function handleSuccess (data) {
  MessageService.success("Cita creada con éxito.");
  delete vm.student;
  delete vm.student_username;
  delete vm.groups;
  delete vm.available_dates;
  delete vm.available_hours;
  delete vm.software_list;
}

function handleError(data) {
  MessageService.error(data.description);
}
}
})();
