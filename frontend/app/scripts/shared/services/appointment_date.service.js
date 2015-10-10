(function() {
  'use strict';

  angular
    .module('silabi')
    .service('AppointmentDateService', AppointmentService);

  AppointmentService.$inject = ['RequestService', '$localStorage'];

  function AppointmentService(RequestService, $localStorage) {
    this.GetAvailable = GetAvailable;
    this.ParseAvailableDates = ParseAvailableDates;

    function GetAvailable(request, Username){
      if(!request) request = {};
      request.access_token = $localStorage['access_token'];
      return RequestService.get('/students/' + Username+ '/appointments/available', request);
    }

    function ParseAvailableDates(data) {
      var available_dates = [];
      for (var i = 0; i < data.length; i++) {
        var date = data[i].date;
        var current_date = date.substring(0, date.indexOf("T"));
        if(getRepetitions(current_date, available_dates) == 0){
          var current_json = {"day":current_date, "hoursByLab":getHoursByLab(current_date, data)};
          available_dates.push(current_json);
        }
      }
      return available_dates;
    }

    function getRepetitions(date, array){
      var count = 0;
      for (var i = 0; i < array.length; i++) {
        var current_date = array[i].day;
        if(current_date === date){
          count++;          
        }
      }
      return count;
    }

  function getHoursByLab(date, datesArray){
    var hours = [];
    for (var i = 0; i < datesArray.length; i++){
      var new_date = datesArray[i].date;
      var current_date = new_date.substring(0, new_date.indexOf("T"));
      if(current_date === date){
        var index = new_date.indexOf("T") + 1;
        var hour = new_date.substring(index, new_date.length - 7);
        var json = {"full_date":new_date, "hour":hour,"laboratory": {"name":datesArray[i].laboratory.name}};
        hours.push(json);
      }
    }
    return hours;
  }
  }
})();
