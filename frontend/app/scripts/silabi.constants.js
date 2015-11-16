/*global toastr, bootbox, moment, CryptoJS, _*/
(function() {
    'use strict';

    angular
        .module('silabi')
        .constant('API_URL','https://localhost/api/v1')
        .constant('CryptoJS', CryptoJS)
        .constant('toastr', toastr)
        .constant('bootbox', bootbox)
        .constant('moment', moment)
        .constant('lodash', _);
})();
