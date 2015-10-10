(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('StudentAppCreateController', AppointmentCreateController);

  AppointmentCreateController.$inject = ['StudentAppService', 'MessageService', 'StudentService', 'SoftwareService', '$routeParams'];

function AppointmentCreateController(StudentAppService, MessageService, StudentService, SoftwareService, $routeParams) {
  var vm = this;
  vm.software_list = [];
  vm.groups = [];
  vm.available_dates = [];
  vm.available_hours = [];
  vm.request = {
      fields : "date,laboratory"
    };

  vm.groups_request = {
    fields : "id,course"
  };

  vm.fieldsReady = fieldsReady;
  vm.create = createAppointment;
  vm.setAvailableHours = setAvailableHours;
  vm.changeLaboratory = changeLaboratory;

  activate();

  function activate() {
    vm.student_id = $routeParams.student_id;
    getAvailableDates();
    getGroups();
    getSoftware();
  }

  function fieldsReady () {
    return vm.laboratory  && vm.software && vm.selected_date && vm.selected_date && vm.group;
  }


function getSoftware () {
  SoftwareService.GetAll()
  .then(setSoftware)
  .catch(handleError);
}

function getGroups () {
  StudentService.GetGroups(vm.student_id, vm.groups_request)
  .then(setGroups)
  .catch(handleError);
}

function setSoftware(data) {
vm.software_list = data.results;
}

function setGroups (groups) {
  vm.groups = groups;
}

function getAvailableDates()
{
  if(vm.student_id)
  {
    StudentAppService.GetAvailable(vm.request, vm.student_id)
    .then(setAvailableDates)
    .catch(handleError);
  }
}

function setAvailableDates (dates) {
  vm.available_dates = StudentAppService.ParseAvailableDates(dates);
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
  var app = {
    "software": vm.selected_software.code,
    "date": vm.selected_date.day+"T"+vm.selected_hour,
    "group": vm.group.id
  }
  StudentAppService.Create(app)
  .then(handleSuccess)
  .catch(handleError);
}

function handleSuccess (data) {
  MessageService.success("Cita creada con éxito.");
        delete vm.available_dates;
        delete vm.available_hours;
}

function handleError(data) {
  MessageService.error(data.description);
}
}
})();
