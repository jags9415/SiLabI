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
  vm.groups = [];  
  vm.laboratories = [];
  vm.available_dates = [];
  vm.available_hours = [];
  vm.request = {
      fields : "date,laboratory"
    };

  vm.groups_request = {
  fields : "id,course"
};

  vm.searchStudent = searchStudent;
  vm.fieldsReady = fieldsReady;
  vm.create = createAppointment;
  vm.setAvailableHours = setAvailableHours;
  vm.changeLaboratory = changeLaboratory;

  activate();

  function activate() {
    getSoftware();
    getLaboratories();
  }

  function fieldsReady () {
    return vm.student_username && vm.laboratory  && vm.software && vm.selected_date && vm.selected_date && vm.group;
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

function setSoftware(data) {
vm.software_list = data.results;
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
  vm.available_dates = AppointmentService.ParseAvailableDates(dates);
  console.log(vm.available_dates);
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
    console.log(vm.selected_hour);
  }
}

function createAppointment () {
  var app =
  {
    "student": vm.student_username,
    "laboratory": vm.laboratory.name,
    "software": vm.selected_software.code,
    "date": vm.selected_date.day+"T"+vm.selected_hour,
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
        delete vm.courses;
        delete vm.available_dates;
        delete vm.available_hours;
}

function handleError(data) {
  MessageService.error(data.description);
}
}
})();
