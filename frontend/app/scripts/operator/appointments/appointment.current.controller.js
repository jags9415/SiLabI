(function() {
    'use strict';

    angular
        .module('silabi')
        .controller('AppointmentCurrentController', AppointmentCurrentController);

    AppointmentCurrentController.$inject = ['$scope', 'moment', 'AppointmentService', 'MessageService'];

  function AppointmentCurrentController($scope, moment, AppointmentService, MessageService) {
    var vm = this;
    vm.opened = false;
    vm.loaded = false;
    vm.today = new Date();
    vm.appointments = [];
    vm.hours = [];
    vm.laboratories = ["Laboratorio A", "Laboratorio B"];
    vm.selected_laboratory = vm.laboratories[1];
    vm.selected_date = vm.today;
    vm.mark = mark;
    vm.isLoaded = isLoaded;
    vm.isEmpty = isEmpty;
    vm.isDisabledDate = isDisabledDate;
    vm.openDatePicker = openDatePicker;
    vm.loadAppointments = loadAppointments;
    vm.loadHours = loadHours;

    vm.request = {
      fields : "id,state,attendance,student.username,student.full_name,software.name",
      sort: [{field: "student.name"}],
      query: {}
    };

    activate();

    function activate() {
      loadHours();
      loadAppointments();
    }

    function loadHours() {
      vm.hours = [];

      var date = new Date();
      var minHour = 8;
      var maxHour = 17;

      if (moment(vm.selected_date).isSame(vm.today, 'day') && date.getHours() < maxHour) {
        var maxHour = Math.max(minHour, date.getHours());
      }

      for (var i = minHour; i <= maxHour; i++) {
        date = new Date();
        date.setHours(i, 0, 0, 0);
        vm.hours.push(date);

        if (i == (new Date().getHours())) {
          vm.selected_hour = vm.hours[vm.hours.length - 1];
        }
      }

      if (!vm.selected_hour) {
        vm.selected_hour = vm.hours[vm.hours.length - 1];
      }

      loadAppointments();
    }

    function loadAppointments() {
      vm.request.query = {};

      vm.request.query["state"] = {
        operation: 'ne',
        value: 'Cancelada'
      }

      vm.request.query["laboratory.name"] = {
        operation: 'eq',
        value: vm.selected_laboratory
      }

      vm.request.query["date"] = {
        operation: 'eq',
        value: getSelectedDateTime().toISOString()
      }

      AppointmentService.GetAll(vm.request)
      .then(setAppointments)
      .catch(handleError);
    }

    function mark(id, attendance) {
      var appointment = {
        "attendance": attendance,
        "state": "Finalizada"
      }

      AppointmentService.Update(id, appointment)
      .then(updateAppointment)
      .catch(handleError);
    }

    function getSelectedDateTime() {
      if (vm.selected_date == null) {
        vm.selected_date = new Date();
      }

      var date = new Date();

      date.setDate(vm.selected_date.getDate());
      date.setMonth(vm.selected_date.getMonth());
      date.setFullYear(vm.selected_date.getFullYear());
      date.setHours(vm.selected_hour.getHours());
      date.setMinutes(vm.selected_hour.getMinutes());
      date.setSeconds(vm.selected_hour.getSeconds());
      date.setMilliseconds(vm.selected_hour.getMilliseconds());

      return date;
    }

    function openDatePicker($event) {
      vm.opened = true;
    };

    function isDisabledDate(date, mode) {
      return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    function isEmpty() {
      return vm.appointments.length == 0;
    }

    function isLoaded() {
      return vm.loaded;
    }

    function setAppointments(data) {
      vm.appointments = data.results;
      vm.loaded = true;
    }

    function updateAppointment(appointment) {
      for (var i = 0; i < vm.appointments.length; i++) {
        if (vm.appointments[i].id == appointment.id) {
          vm.appointments[i] = appointment;
          return;
        }
      }
    }

    function handleError(data) {
      MessageService.error(data.description);
    }
  }
})();
