(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('AppointmentCreateController', AppointmentCreateController);

  AppointmentCreateController.$inject = ['$scope', '$location', 'AppointmentService', 'AppointmentDateService', 'MessageService', 'StudentService', 'SoftwareService', 'PeriodService', 'LabService', 'lodash'];

  function AppointmentCreateController($scope, $location, AppointmentService, AppointmentDateService, MessageService, StudentService, SoftwareService, PeriodService, LabService, _) {
    var vm = this;

    vm.student = {};
    vm.softwareList = [];
    vm.groups = [];
    vm.laboratories = [];
    vm.availableDates = [];
    vm.availableHours = [];

    vm.dateRequest = {
      fields: 'date,laboratory.name'
    };

    vm.studentRequest = {
      fields: 'id,username,full_name'
    };

    vm.groupRequest = {
      fields: 'id,number,course.name'
    };

    vm.softwareRequest = {
      fields: 'id,code,name',
      limit: 10
    };

    vm.searchStudent = searchStudent;
    vm.fieldsReady = fieldsReady;
    vm.create = createAppointment;
    vm.setAvailableHours = setAvailableHours;
    vm.changeLaboratory = changeLaboratory;
    vm.searchSoftware = searchSoftware;
    vm.setSoftware = setSoftware;
    vm.formatSoftware = formatSoftware;

    activate();

    function activate() {
      getLaboratories();
    }

    function fieldsReady () {
      return !_.isEmpty(vm.student) &&
             !_.isEmpty(vm.selectedLaboratory) &&
             !_.isEmpty(vm.selectedSoftware) &&
             !_.isEmpty(vm.selectedDate) &&
             !_.isEmpty(vm.selectedHour) &&
             !_.isEmpty(vm.group);
    }

    function searchStudent() {
      clearFields();
      if (vm.studentUsername) {
        StudentService.GetOne(vm.studentUsername, vm.studentRequest)
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
      vm.groupRequest.query = {};

      vm.groupRequest.query['period.type'] = {
        operation: 'eq',
        value: 'Semestre'
      };

      vm.groupRequest.query['period.value'] = {
        operation: 'eq',
        value: period.value
      };

      vm.groupRequest.query['period.year'] = {
        operation: 'eq',
        value: period.year
      };

      return StudentService.GetGroups(vm.studentUsername, vm.groupRequest);
    }

    function getAvailableDates() {
        return AppointmentDateService.GetAvailableForCreate(vm.dateRequest, vm.studentUsername);
    }

    function searchSoftware (input) {
      vm.softwareRequest.query = {};

      vm.softwareRequest.query.code = {
        operation: 'like',
        value: '*' + input + '*'
      };

      return SoftwareService.GetAll(vm.softwareRequest)
        .then(function(data) {
          return data.results;
        });
    }

    function setLaboratories (data) {
      vm.laboratories = data.results;
    }

    function setSoftware (data) {
      vm.selectedSoftware = data;
    }

    function formatSoftware(model) {
      if (model) {
        return model.code;
      }
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
      vm.availableDates = AppointmentDateService.ParseAvailableDates(dates);

      if (vm.availableDates.length > 0) {
        vm.selectedDate = vm.availableDates[0];
      }
      else {
        delete vm.selectedDate;
        MessageService.error('El estudiante no posee fechas disponibles.');
      }

      setAvailableHours();
    }

    function setAvailableHours() {
      if (!_.isEmpty(vm.selectedDate)) {
        vm.availableHours = vm.selectedDate.hoursByLab;
        if (vm.availableHours.length > 0) {
          vm.selectedHour = vm.availableHours[0];
        }
      }
      else {
        delete vm.availableHours;
        delete vm.selectedHour;
      }

      changeLaboratory();
    }

    function changeLaboratory () {
      if (vm.selectedHour) {
        vm.selectedLaboratory = vm.selectedHour.laboratory;
      }
      else {
        delete vm.selectedLaboratory;
      }
    }

    function createAppointment () {
      var app = {
        'student': vm.student.username,
        'laboratory': vm.selectedLaboratory.name,
        'software': vm.selectedSoftware.code,
        'date': moment(vm.selectedHour.fullDate).toJSON(),
        'group': vm.group.id
      };

      AppointmentService.Create(app)
      .then(handleSuccess)
      .catch(handleError);
    }

    function clearFields() {
       $scope.$broadcast('show-errors-reset');

       delete vm.student;
       delete vm.groups;
       delete vm.availableDates;
       delete vm.availableHours;
       delete vm.softwareList;
       delete vm.selectedDate;
       delete vm.selectedSoftware;
       delete vm.selectedHour;
       delete vm.selectedLaboratory;
       delete vm.group;
     }

    function handleSuccess(data) {
      MessageService.success('Cita creada.');
      $scope.$broadcast('show-errors-reset');
      clearFields();
      delete vm.studentUsername;
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
