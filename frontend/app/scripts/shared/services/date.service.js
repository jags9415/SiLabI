(function() {
  'use strict';

  angular
    .module('silabi')
    .service('DateService', DateService);

  DateService.$inject = ['RequestService', '$localStorage', 'moment'];

  function DateService(RequestService, $localStorage, moment) {
    this.GetHourRange = GetHourRange;

    function GetHourRange(start, end) {
      var hours = [];
      var now = moment().startOf('day');

      for (var i = start; i <= end; i++) {
        now.hour(i);
        hours.push(now.format());
      }

      return hours;
    }
  }
})();
