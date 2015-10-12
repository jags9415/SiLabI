(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('AppointmentCreateController', AppointmentCreateController);

  AppointmentCreateController.$inject = ['$scope', 'AppointmentService', 'AppointmentDateService', 'MessageService', 'StudentService', 'SoftwareService', 'PeriodService', 'LabService', '$location'];

  function AppointmentCreateController($scope, AppointmentService, AppointmentDateService, MessageService, StudentService, SoftwareService, PeriodService, LabService, $location) {
    var vm = this;
    vm.student = {};
    vm.software_list = [];
    vm.groups = [];
    vm.laboratories = [];
    vm.available_dates = [];
    vm.available_hours = [];

    vm.date_request = {
      fields: "date,laboratory.name"
    };

    vm.student_request = {
      fields: "id,username,full_name"
    }

    vm.groups_request = {
      fields: "id,course.name"
    };

    vm.software_request = {
      fields: "id,code,name"
    };

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
      return !_.isEmpty(vm.student) &&
             !_.isEmpty(vm.selected_laboratory) &&
             !_.isEmpty(vm.selected_software) &&
             !_.isEmpty(vm.selected_date) &&
             !_.isEmpty(vm.selected_hour) &&
             !_.isEmpty(vm.group);
    }

    function searchStudent() {
      clearFields();
      if (vm.student_username) {
        StudentService.GetOne(vm.student_username, vm.student_request)
        .then(setStudent)
        .then(setGroups)
        .then(setAvailableDates)
        .catch(handleError);
      }
    }

    function getLaboratories () {
      LabService.GetAll()
      .then(setLaboratories)
      .catch(handleError);
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

      return StudentService.GetGroups(vm.student_username, vm.groups_request);
    }

    function getAvailableDates() {
        return AppointmentService.GetAvailable(vm.student_username, vm.date_request);
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

    function setLaboratories (data) {
      vm.laboratories = data.results;
    }

    function setSoftware (data) {
      vm.selected_software = data;
    }

    function setStudent (student) {
      vm.student = student;
      return getGroups();
    }

    function setGroups (groups) {
      vm.groups = groups;

      if (vm.groups.length > 0) {
        vm.group = vm.groups[0];
      }

      return getAvailableDates();
    }

    function setAvailableDates (dates) {
      vm.available_dates = AppointmentDateService.ParseAvailableDates(dates);

      if (vm.available_dates.length > 0) {
        vm.selected_date = vm.available_dates[0];
        setAvailableHours();
      }
    }

    function setAvailableHours() {
      if (!_.isEmpty(vm.selected_date)) {
        vm.available_hours = vm.selected_date.hoursByLab;
        if (vm.available_hours.length > 0) {
          vm.selected_hour = vm.available_hours[0];
          changeLaboratory();
        }
      }
    }

    function changeLaboratory () {
      if (vm.selected_hour) {
        vm.selected_laboratory = vm.selected_hour.laboratory;
      }
    }

    function createAppointment () {
      var app = {
        "student": vm.student.username,
        "laboratory": vm.selected_laboratory.name,
        "software": vm.selected_software.code,
        "date": vm.selected_date.day + "T" + vm.selected_hour.hour + ":00.000",
        "group": vm.group.id
      };

      AppointmentService.Create(app)
      .then(handleSuccess)
      .catch(handleError);
    }

    function clearFields() {
      $scope.$broadcast('show-errors-reset');

      delete vm.student;
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
