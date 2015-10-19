(function() {
  'use strict';

  angular
    .module('silabi')
    .service('FileService', FileService);

  FileService.$inject = ['RequestService', '$localStorage'];

  function FileService(RequestService, $localStorage) {

    function appointmentsByStudentToPDF (apps) {
      var doc = new jsPDF();

      var specialElementHandlers = {
        '#editor': function(element, renderer){
          return true;
        }
      };

      var html = document.createElement("DIV");
      var table = 
      "<table class='table table-striped'>
      <thead>
        <tr>
          <th>Fecha</th>
          <th>Software</th>
          <th>Laboratorio</th>
        </tr>
      </thead>
      <tbody>
      </tbody>
      </table>";

      var tbody = table.getElementsByTagName('tbody');

      for (var i = 0; i < apps.length 0; i++) 
      {
        var newRow = tbody.insertRow(tbody.rows.length);
        var lab = newRow.insertCell(0);
        var software = newRow.insertCell(0);
        var date = newRow.insertCell(0);
        lab.innerHTML = apps[i].laboratory.name;
        software.innerHTML = apps[i].software.code;
        date.innerHTML = apps[i].date;
      }
      table.getElementsByTagName('tbody') = tbody;
      html.appendChild(table);

      doc.fromHTML(html, 15, 15, {
        'width': 170, 
        'elementHandlers': specialElementHandlers
      });

      return doc.output();
    }



  }
})();