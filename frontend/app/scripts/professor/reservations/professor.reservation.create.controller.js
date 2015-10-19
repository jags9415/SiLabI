(function() {
  'use strict';

  angular
      .module('silabi')
      .controller('ProfessorReservationCreateController', ProfessorReservationCreateController);

  ProfessorReservationCreateController.$inject = ['$scope', 'ProfessorReservationService', 'MessageService', 'SoftwareService', 'PeriodService', 'GroupService', 'LabService', 'DateService', '$location', '$localStorage'];

  function ProfessorReservationCreateController($scope, ProfessorReservationService, MessageService, SoftwareService, PeriodService, GroupService, LabService, DateService, $location, $localStorage) {
    var vm = this;

    vm.software_list = [];
    vm.groups = [];
    vm.laboratories = [];
    vm.start_hours = [];
    vm.end_hours = [];
    vm.end_hours_sliced = [];
    vm.selected_software = null;
    vm.selected_group = null;
    vm.$storage = $localStorage;

    vm.groups_request = {
      fields: 'id,number,course.name'
    };

    vm.software_request = {
      fields: 'id,code,name',
      limit: 10
    };

    vm.fieldsReady = fieldsReady;
    vm.create = createReservation;
    vm.searchSoftware = searchSoftware;
    vm.setSoftware = setSoftware;
    vm.min_date = new Date();
    vm.datepicker_open = false;
    vm.openDatePicker = openDatePicker;
    vm.loadEndHours = loadEndHours;
    vm.setHours = setHours;

    activate();

    function activate() {
      getLaboratories();
      setHours();

      if (vm.$storage['username']) {
        vm.username = vm.$storage['username'];
      }

      getGroups();
    }

    function loadEndHours() {
      var index = _.indexOf(vm.end_hours, vm.selected_start_time);

      if (index >= 0) {
        vm.end_hours_sliced = vm.end_hours.slice(index + 1, vm.end_hours.length);
      }
      else {
        vm.end_hours_sliced = vm.end_hours;
      }

      if (!_.contains(vm.end_hours_sliced, vm.selected_end_time)) {
        vm.selected_end_time = vm.end_hours_sliced[0];
      }
    }

    function openDatePicker($event){
      if ($event) {
        $event.preventDefault();
        $event.stopPropagation();
      }
      vm.datepicker_open = true;
    }

    function fieldsReady() {
      return vm.selected_laboratory &&
             vm.selected_date &&
             vm.selected_start_time &&
             vm.selected_end_time;
    }

    function getLaboratories() {
      LabService.GetAll()
      .then(setLaboratories)
      .catch(handleError);
    }

    function getGroups() {
      var period = PeriodService.GetCurrentPeriod('Semestre');
      vm.groups_request.query = {};

      vm.groups_request.query['period.type'] = {
        operation: 'eq',
        value: 'Semestre'
      };

      vm.groups_request.query['period.value'] = {
        operation: 'eq',
        value: period.value
      };

      vm.groups_request.query['period.year'] = {
        operation: 'eq',
        value: period.year
      };

      vm.groups_request.query['professor.username'] = {
        operation: 'eq',
        value: vm.username
      };

      GroupService.GetAll(vm.groups_request)
      .then(setGroups)
      .catch(handleError);
    }

    function setHours() {
      vm.start_hours = DateService.GetReservationStartHours(vm.selected_date);
      vm.end_hours = DateService.GetReservationEndHours(vm.selected_date);
    }

    function searchSoftware(input) {
      vm.software_request.query = {};

      vm.software_request.query.code = {
        operation: 'like',
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

    function setGroups (data) {
      vm.groups = data.results;
    }

    function createReservation() {
      var start_time = moment(vm.selected_date).hour(moment(vm.selected_start_time).hour());
      var end_time = moment(vm.selected_date).hour(moment(vm.selected_end_time).hour());

      var res = {
        'laboratory': vm.selected_laboratory.name,
        'start_time': start_time.format(),
        'end_time': end_time.format(),
      };

      if (vm.selected_group) {
        res['group'] = vm.selected_group.id;
      }

      if (vm.selected_software) {
        res['software']  = vm.selected_software.code;
      }

      ProfessorReservationService.Create(res, vm.username)
      .then(handleSuccess)
      .catch(handleError);
    }

    function clearFields() {
      $scope.$broadcast('show-errors-reset');
      delete vm.groups;
      delete vm.selected_start_time;
      delete vm.selected_end_time;
      delete vm.software_list;
      delete vm.selected_date;
      delete vm.selected_software;
      delete vm.selected_laboratory;
      delete vm.selected_group;
    }

    function handleSuccess (data) {
      MessageService.success('Reservación creada.');
      clearFields();
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
