(function() {
    'use strict';

    angular
        .module('silabi', [
          'ngAnimate',
          'ngRoute',
          'ngTouch',
          'ngStorage',
          'ngSanitize',
          'angular-jwt',
          'ui.select',
          'ui.mask',
          'ui.bootstrap',
          'ui.bootstrap.showErrors',
          'silabi.sidebar',
          'ngPasswordStrength'
        ]);
})();
