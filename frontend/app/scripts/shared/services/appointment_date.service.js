(function() {
  'use strict';

  angular
    .module('silabi')
    .service('AppointmentDateService', AppointmentService);

  AppointmentService.$inject = ['RequestService', '$localStorage', 'moment', 'lodash'];

  function AppointmentService(RequestService, $localStorage, moment, _) {

    this.GetAvailableForCreate = GetAvailableForCreate;
    this.GetAvailableForUpdate = GetAvailableForUpdate;
    this.ParseAvailableDates = ParseAvailableDates;

    function GetAvailableForCreate(request, username) {
      if (!request) { request = {}; }
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/students/' + username + '/appointments/available', request);
    }

    function GetAvailableForUpdate(request, username, appointment) {
      if (!request) { request = {}; }
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/students/' + username + '/appointments/' + appointment + '/available', request);
    }

    function ParseAvailableDates(data) {
      var result = [];

      for (var i = 0; i < data.length; i++) {
        var current = moment(data[i].date).format("YYYY-MM-DD");

        if (!_.find(result, 'day', current)) {
          result.push({
            'day': current,
            'hoursByLab': getHoursByLab(current, data)
          });
        }
      }

      return result;
    }

    function getHoursByLab(date, data) {
      var hours = [];

      for (var i = 0; i < data.length; i++) {
        var current = moment(data[i].date).format("YYYY-MM-DD");

        if (current === date) {
          hours.push({
            'fullDate': data[i].date,
            'laboratory': {
              'name': data[i].laboratory.name
            }
          });
        }
      }

      return hours;
    }
  }
})();
