(function() {
  'use strict';

  angular
    .module('silabi')
    .service('FileService', FileService);

  FileService.$inject = ['RequestService', '$localStorage'];

  function FileService(RequestService, $localStorage) {

    this.htmlToPDF = htmlToPDF;
    this.downloadHtmlToPDF = downloadHtmlToPDF;
    this.createFromHTML = createFromHTML;

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
      doc.fromHTML(html, 20, 5, {
        'width': 100, 
        'elementHandlers': specialElementHandlers
      });
      return doc;
    }

    function createFromHTML (html, docname) {
      var pdf = new jsPDF('p','pt','a4');
      pdf.addHTML(html,function() {
      pdf.save(docname+".pdf");
    });
    }



  }
})();