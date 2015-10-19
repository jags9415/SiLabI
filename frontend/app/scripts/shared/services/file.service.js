(function() {
  'use strict';

  angular
    .module('silabi')
    .service('FileService', FileService);

  FileService.$inject = ['RequestService', '$localStorage'];

  function FileService(RequestService, $localStorage) {

    function appointmentsByStudentToPDF (apps, htmlID) {
      var doc = new jsPDF();

      var specialElementHandlers = {
        '#editor': function(element, renderer){
          return true;
        }
      };

      doc.fromHTML(html, 15, 15, {
        'width': 170, 
        'elementHandlers': specialElementHandlers
      });

      return doc.output();
    }



  }
})();