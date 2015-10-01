(function() {
    'use strict';

    angular
        .module('silabi')
        .service('PeriodService', PeriodService);

    PeriodService.$inject = ['$q'];

    function PeriodService($q) {
      this.GetAll = GetAll;

      function GetAll() {
        var defer = $q.defer();

        var periods = [
          {
            value: 1,
            type: 'Semestre'
          },
          {
            value: 2,
            type: 'Semestre'
          },
          {
            value: 1,
            type: 'Cuatrimestre'
          },
          {
            value: 2,
            type: 'Cuatrimestre'
          },
          {
            value: 3,
            type: 'Cuatrimestre'
          },
          {
            value: 1,
            type: 'Trimestre'
          },
          {
            value: 2,
            type: 'Trimestre'
          },
          {
            value: 3,
            type: 'Trimestre'
          },
          {
            value: 4,
            type: 'Trimestre'
          },
          {
            value: 1,
            type: 'Bimestre'
          },
          {
            value: 2,
            type: 'Bimestre'
          },
          {
            value: 3,
            type: 'Bimestre'
          },
          {
            value: 4,
            type: 'Bimestre'
          },
          {
            value: 5,
            type: 'Bimestre'
          }
        ];

        defer.resolve(periods);
        return defer.promise;
      }
    }
})();
