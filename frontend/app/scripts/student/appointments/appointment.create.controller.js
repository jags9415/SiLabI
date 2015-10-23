(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('StudentAppCreateController', AppointmentCreateController);

  AppointmentCreateController.$inject = ['$localStorage', 'StudentAppService', 'AppointmentDateService', 'MessageService', 'StudentService', 'SoftwareService', 'PeriodService', 'lodash'];

  function AppointmentCreateController($localStorage, StudentAppService, AppointmentDateService, MessageService, StudentService, SoftwareService, PeriodService, _) {
    var vm = this;

    vm.groups = [];
    vm.availableDates = [];
    vm.availableHours = [];

    vm.request = {
      fields : 'date,laboratory'
    };

    vm.groupRequest = {
      fields : 'id,course.name'
    };

    vm.softwareRequest = {
      fields: 'code,name',
      limit: 10
    };

    vm.$storage = $localStorage;
    vm.searchSoftware = searchSoftware;
    vm.setSoftware = setSoftware;
    vm.create = createAppointment;
    vm.setAvailableHours = setAvailableHours;
    vm.changeLaboratory = changeLaboratory;
    vm.formatSoftware = formatSoftware;

    activate();

    function activate() {
      vm.username = vm.$storage['username'];

      getGroups()
      .then(setGroups)
      .then(setAvailableDates)
      .catch(handleError);
    }

    function searchSoftware(input) {
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

    function setSoftware(data) {
      vm.selectedSoftware = data;
    }

    function formatSoftware(model) {
      if (model) {
        return model.code;
      }
    }

    function getGroups() {
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

      return StudentService.GetGroups(vm.username, vm.groupRequest);
    }

    function setGroups(groups) {
      vm.groups = groups;

      if (vm.groups.length > 0) {
        vm.group = vm.groups[0];
      }

      return getAvailableDates();
    }

    function getAvailableDates() {
        return AppointmentDateService.GetAvailableForCreate(vm.request, vm.username);
    }

    function setAvailableDates(dates) {
      vm.availableDates = AppointmentDateService.ParseAvailableDates(dates);

      if (vm.availableDates.length > 0) {
        vm.selectedDate = vm.availableDates[0];
      }
      else {
        delete vm.selectedDate;
        MessageService.error('No posee fechas disponibles.');
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

    function changeLaboratory() {
      if (!_.isEmpty(vm.selectedHour)) {
        vm.selectedLaboratory = vm.selectedHour.laboratory;
      }
      else {
        delete vm.selectedLaboratory;
      }
    }

    function createAppointment () {
      var app = {
        'software': vm.selectedSoftware.code,
        'date': moment(vm.selectedHour.fullDate).toJSON(),
        'group': vm.group.id
      };

      StudentAppService.Create(app, vm.username)
      .then(handleSuccess)
      .catch(handleError);
    }

    function handleSuccess (data) {
      MessageService.success('Cita creada.');

      delete vm.appointment;
      delete vm.selectedSoftware;

      getAvailableDates()
      .then(setAvailableDates)
      .catch(handleError);
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
