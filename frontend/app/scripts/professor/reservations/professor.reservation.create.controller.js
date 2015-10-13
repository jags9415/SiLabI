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
    vm.selected_software = null;
    vm.selected_group = null;
    vm.selected_date = new Date();
 
    vm.groups_request = {
      fields: "id,number,course.name"
    };

    vm.software_request = {
      fields: "id,code,name",
      limit: 10
    };
    vm.$storage = $localStorage;

    vm.fieldsReady = fieldsReady;
    vm.create = createReservation;
    vm.searchSoftware = searchSoftware;
    vm.setSoftware = setSoftware;

    activate();
    vm.minDate =  new Date();

    function activate() {
      getLaboratories();
      getHours();
      if(vm.$storage['username'])
      {
        vm.username = vm.$storage['username'];
      }
    }

    function fieldsReady () {
      return !_.isEmpty(vm.selected_laboratory) &&
             !_.isEmpty(vm.selected_date) &&
             !_.isEmpty(vm.selected_start_time) &&
             !_.isEmpty(vm.selected_end_time);
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

      vm.groups_request.query["professor.username"] = {
        operation: "eq",
        value: vm.username
      };

      GroupService.GetAll(vm.groups_request)
      .then(setGroups)
      .catch(handleError);
    }

    function getHours() {
      vm.start_hours = DateService.GetReservationStartHours();
      vm.end_hours = DateService.GetReservationEndHours();
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

    function setGroups (data) {
      vm.groups = data.results;
    }

    function createReservation () {
      var group_id = null;
      var software_code = null;
      if(vm.selected_group)
      {
        group_id = vm.selected_group.id;
      }
      if(vm.selected_software)
      {
        software_code = vm.selected_software.code;
      }
      var res = {
        "laboratory": vm.selected_laboratory.name,
        "software": software_code,
        "start_time": vm.selected_date + "T" + vm.selected_start_time.value,
        "end_time": vm.selected_date + "T" + vm.selected_end_time.value,
        "group": group_id
      };

      ProfessorReservationService.Create(res, vm.username)
      .then(handleSuccess)
      .catch(handleError);
    }

    function clearFields() {
      $scope.$broadcast('show-errors-reset');
      delete vm.groups;
      delete vm.aselected_start_time;
      delete vm.aselected_end_time;
      delete vm.software_list;
      delete vm.selected_date;
      delete vm.selected_software;
      delete vm.selected_laboratory;
      delete vm.selected_group;
    }

    function handleSuccess (data) {
      MessageService.success("Reservación creada.");
      clearFields();
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
