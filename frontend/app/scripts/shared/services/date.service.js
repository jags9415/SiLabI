(function() {
  'use strict';

  angular
    .module('silabi')
    .service('DateService', DateService);

  DateService.$inject = ['RequestService', '$localStorage', 'moment'];

  function DateService(RequestService, $localStorage, moment) {

    this.GetReservationStartHours = GetReservationStartHours;
    this.GetReservationEndHours = GetReservationEndHours;

    function GetReservationStartHours(date) {
      var hours = [];
      var now = moment(date).startOf('day');

      for (var i = 8; i <= 17; i++) {
        now.hour(i);
        hours.push(now.format());
      }

      return hours;
    }

    function GetReservationEndHours(date) {
      var hours = [];
      var now = moment(date).startOf('day');

      for (var i = 9; i <= 18; i++) {
        now.hour(i);
        hours.push(now.format());
      }

      return hours;
    }
  }
})();
