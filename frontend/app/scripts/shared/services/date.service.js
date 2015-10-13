(function() {
  'use strict';

  angular
    .module('silabi')
    .service('DateService', DateService);

  DateService.$inject = ['RequestService', '$localStorage'];

  function DateService(RequestService, $localStorage) {
    this.GetReservationStartHours = GetReservationStartHours;
    this.GetReservationEndHours = GetReservationEndHours;

    function GetReservationStartHours () {
      var defer = $q.defer();
      var hours = [
      {
        hour: "8:00am",
        value: "08:00:00.000"
      },
      {
        hour: "9:00am",
        value: "09:00:00.000"
      },
      {
        hour: "10:00am",
        value: "10:00:00.000"
      },
      {
        hour: "11:00am",
        value: "11:00:00.000"
      },
      {
        hour: "12:00pm",
        value: "12:00:00.000"
      },
      {
        hour: "01:00pm",
        value: "13:00:00.000"
      },
      {
        hour: "02:00pm",
        value: "14:00:00.000"
      },
      {
        hour: "03:00pm",
        value: "15:00:00.000"
      },
      {
        hour: "04:00pm",
        value: "16:00:00.000"
      },
      {
        hour: "05:00pm",
        value: "17:00:00.000"
      }
      ];
      defer.resolve(hours);
      return defer.promise;
    }

    function GetReservationEndHours () {
      var defer = $q.defer();
      var hours = [
      {
        hour: "9:00am",
        value: "09:00:00.000"
      },
      {
        hour: "10:00am",
        value: "10:00:00.000"
      },
      {
        hour: "11:00am",
        value: "11:00:00.000"
      },
      {
        hour: "12:00pm",
        value: "12:00:00.000"
      },
      {
        hour: "01:00pm",
        value: "13:00:00.000"
      },
      {
        hour: "02:00pm",
        value: "14:00:00.000"
      },
      {
        hour: "03:00pm",
        value: "15:00:00.000"
      },
      {
        hour: "04:00pm",
        value: "16:00:00.000"
      },
      {
        hour: "05:00pm",
        value: "17:00:00.000"
      },
      {
        hour: "06:00pm",
        value: "18:00:00.000"
      }
      ];
      defer.resolve(hours);
      return defer.promise;
    }
  }
})();