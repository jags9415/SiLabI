(function() {
    'use strict';

    angular
      .module('silabi')
      .config(['showErrorsConfigProvider', configShowErrors])
      .config(['toastr', configToaster])
      .run(configureHttpCache)
      .value('cgBusyDefaults', {
        message: 'Cargando...',
        backdrop: true,
        delay: 50,
        minDuration: 0
      });

    configureHttpCache.$inject = ['$http', 'CacheFactory', 'lodash'];

    function configureHttpCache($http, CacheFactory, _) {
      $http.defaults.cache = new CacheFactory('$http', {
        maxAge: 30 * 60 * 1000,             // Items added to this cache expire after 30 minutes.
        cacheFlushInterval: 60 * 60 * 1000, // This cache will clear itself every hour.
        deleteOnExpire: 'aggressive'        // Items will be deleted from this cache when they expire.
      });

      $http.defaults.cache.removePrefix = function(prefix) {
        var predicate = function(key) {
          return _.startsWith(key, prefix);
        };

        _.each(_.select(this.keys(), predicate), this.remove);
      };
    }

    function configShowErrors(showErrorsConfigProvider) {
      showErrorsConfigProvider.showSuccess(false);
    }

    function configToaster(toastr) {
      toastr.options = {
        'closeButton': true,
        'debug': false,
        'newestOnTop': true,
        'progressBar': false,
        'positionClass': 'toast-top-center',
        'preventDuplicates': true,
        'onclick': null,
        'showDuration': '300',
        'hideDuration': '1000',
        'timeOut': '5000',
        'extendedTimeOut': '1000',
        'showEasing': 'swing',
        'hideEasing': 'linear',
        'showMethod': 'fadeIn',
        'hideMethod': 'fadeOut'
      };
    }
})();
