(function() {
    'use strict';

    angular
        .module('silabi')
        .constant('API_URL','http://localhost/api/v1')
        .constant('toastr', toastr)
        .constant('bootbox', bootbox)
        .constant('CryptoJS', CryptoJS);
})();
