(function() {
    'use strict';

    angular
      .module('silabi')
      .config(['showErrorsConfigProvider', configShowErrors])
      .config(['toastr', configToaster])
      .value('cgBusyDefaults', {
        message: 'Cargando...',
        backdrop: true,
        delay: 50,
        minDuration: 0
      });

    function configShowErrors(showErrorsConfigProvider) {
      showErrorsConfigProvider.showSuccess(false);
    }

    function configToaster(toastr) {
      toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
      }
    }

    if (typeof String.prototype.endsWith !== 'function') {
        String.prototype.endsWith = function(suffix) {
            return this.indexOf(suffix, this.length - suffix.length) !== -1;
        };
    }

    if (typeof String.prototype.startsWith != 'function') {
      String.prototype.startsWith = function (prefix) {
        return this.indexOf(prefix) === 0;
      };
    }

})();
