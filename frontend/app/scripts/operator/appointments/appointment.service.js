(function() {
  'use strict';

angular
  .module('silabi')
  .service('AppointmentService', AppointmentService);

AppointmentService.$inject = ['RequestService', '$localStorage'];

function AppointmentService(RequestService, $localStorage) {
  this.GetAll = GetAll;
  this.GetOne = GetOne;
  this.Update = Update;
  this.Create = Create;
  this.Delete = Delete;
  this.GetAvailable = GetAvailable;
  this.ParseAvailableDates = ParseAvailableDates;

  function GetAll(request) {
    if (!request) request = {};
    request.access_token = $localStorage['access_token'];
    return RequestService.get('/appointments', request);
  }


  function GetOne(AppointmentID) {
    var request = {};
    request.access_token = $localStorage['access_token'];
    return RequestService.get('/appointments/' + AppointmentID, request);
  }

  function Update(AppointmentID, NewAppointmentInfo) {
    var requestBody = {};
    requestBody.appointment = NewAppointmentInfo;
    requestBody.access_token = $localStorage['access_token'];
    return RequestService.put('/appointments/' + AppointmentID, requestBody);
  }

  function Create(Appointment) {
    var requestBody = {};
    requestBody.appointment = Appointment;
    requestBody.access_token = $localStorage['access_token'];
    return RequestService.post('/appointments', requestBody);
  }

  function Delete(AppointmentID) {
    var requestBody = {};
    requestBody.access_token = $localStorage['access_token'];
    return RequestService.delete('/appointments/' + AppointmentID, requestBody);
  }

  function GetAvailable(request, Username){
    if(!request) request = {};
    request.access_token = $localStorage['access_token'];
    return RequestService.get('/students/' + Username+ '/appointments/available', request);
  }

  /*
  Returns:
   [
    {
      day: "2015-10-09"
      hoursByLab: 
        [
          {
            full_date:"2015-10-09T11:00:00.000",
            hour : "11:00"
            laboratory : {name: "Laboratorio B"}
          }
        ]
    }
   ]
  */
  function ParseAvailableDates(data)
  {
    var available_dates = [];
    for (var i = 0; i < data.length; i++) {
      var date = data[i].date;
      var current_date = date.substring(0, date.indexOf("T"));
      if(getRepetitions(current_date, available_dates) == 0)
      {
        var current_json = {"day":current_date, "hoursByLab":getHoursByLab(current_date, data)};
        available_dates.push(current_json);
      }
    }
    return available_dates;
  }

  function getRepetitions(date, array)
  {
    var count = 0;
    for (var i = 0; i < array.length; i++) {
      var current_date = array[i].day;
      if(current_date === date)
      {
        count++;
      }
    }
    return count;
  }

function getHoursByLab(date, datesArray)
{
  var hours = [];
  for (var i = 0; i < datesArray.length; i++) 
  {
    var new_date = datesArray[i].date;
    var current_date = new_date.substring(0, new_date.indexOf("T"));
    if(current_date === date)
    {
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
