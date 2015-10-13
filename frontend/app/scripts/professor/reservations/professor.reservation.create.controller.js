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
    vm.valuationDate = new Date();
    vm.datepicker_open = false;
    vm.openDatePicker = openDatePicker;

    activate();


  
    function activate() {
      getLaboratories();
      getHours();
      if(vm.$storage['username'])
      {
        vm.username = vm.$storage['username'];
      }
      getGroups ();
    }

    function openDatePicker($event){
      if ($event) 
      {
        $event.preventDefault();
        $event.stopPropagation(); 
      }
      vm.datepicker_open = true;
    }

    function fieldsReady () {
      return vm.selected_laboratory &&
             vm.selected_date &&
             vm.selected_start_time &&
             vm.selected_end_time;
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
      var start_time = vm.selected_date.getFullYear()+"-"+ (vm.selected_date.getMonth() + 1)+ "-" + vm.selected_date.getUTCDate() + "T" + vm.selected_start_time.value;
      var end_time = vm.selected_date.getFullYear()+"-"+ (vm.selected_date.getMonth() + 1)+ "-" + vm.selected_date.getUTCDate() + "T" + vm.selected_end_time.value;
      var res = {
        "laboratory": vm.selected_laboratory.name,
        "software": software_code,
        "start_time": start_time,
        "end_time": end_time,
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
