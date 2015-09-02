(function() {
    'use strict';

    angular
        .module('silabi')
        .service('GenderService', GenderService);

    GenderService.$inject = ['$q'];

    function GenderService($q) {
      this.GetAll = GetAll;

      function GetAll() {
        var defer = $q.defer();
        defer.resolve(['Masculino', 'Femenino']);
        return defer.promise;
      }
    }
})();
