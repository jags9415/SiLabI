(function() {
  'use strict';

  angular
    .module('silabi')
    .service('AppointmentDateService', AppointmentService);

  AppointmentService.$inject = ['RequestService', '$localStorage'];

  function AppointmentService(RequestService, $localStorage) {
    this.GetAvailable = GetAvailable;
    this.ParseAvailableDates = ParseAvailableDates;

    function GetAvailable(request, username){
      if (!request) { request = {}; }
      request['access_token'] = $localStorage['access_token'];
      return RequestService.get('/students/' + username + '/appointments/available', request);
    }

    function ParseAvailableDates(data) {
      var availableDates = [];
      for (var i = 0; i < data.length; i++) {
        var date = data[i].date;
        var currentDate = date.substring(0, date.indexOf('T'));
        if (getRepetitions(currentDate, availableDates) === 0) {
          var currentJson = {
            'day': currentDate,
            'hoursByLab': getHoursByLab(currentDate, data)
          };
          availableDates.push(currentJson);
        }
      }
      return availableDates;
    }

    function getRepetitions(date, array){
      var count = 0;
      for (var i = 0; i < array.length; i++) {
        var currentDate = array[i].day;
        if (currentDate === date) {
          count++;
        }
      }
      return count;
    }

  function getHoursByLab(date, datesArray){
    var hours = [];
    for (var i = 0; i < datesArray.length; i++){
      var newDate = datesArray[i].date;
      var currentDate = newDate.substring(0, newDate.indexOf('T'));
      if(currentDate === date){
        var index = newDate.indexOf('T') + 1;
        var hour = newDate.substring(index, newDate.length - 7);
        var json = {'fullDate':newDate, 'hour':hour,'laboratory': {'name':datesArray[i].laboratory.name}};
        hours.push(json);
      }
    }
    return hours;
  }
  }
})();
