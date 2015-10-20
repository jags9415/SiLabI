(function() {
  'use strict';

  angular
    .module('silabi')
    .service('FileService', FileService);

  FileService.$inject = ['RequestService', '$localStorage'];

  function FileService(RequestService, $localStorage) {

    this.htmlToPDF = htmlToPDF;
    this.downloadHtmlToPDF = downloadHtmlToPDF;

    function downloadHtmlToPDF (html, docname) {
      var doc = new jsPDF();

      doc = createPDF(html, doc);

      doc.save(docname+".pdf");
    }

    function htmlToPDF (html) {
      var doc = new jsPDF();

      doc = createPDF(html, doc);

      doc.output("dataurlnewwindow");
    }

    function createPDF (html, doc) {

      var specialElementHandlers = {
        '#editor': function(element, renderer){
          return true;
        }
      };
      doc.fromHTML(html, 15, 15, {
        'width': 200, 
        'elementHandlers': specialElementHandlers
      });
      return doc;
    }



  }
})();