(function() {
    'use strict';

    angular
        .module('silabi')
        .service('PeriodService', PeriodService);

    PeriodService.$inject = ['$q'];

    function PeriodService($q) {
      this.GetAll = GetAll;
      this.GetCurrentPeriod = GetCurrentPeriod;

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

    function GetCurrentPeriod(periodType)
    {
      var currentYear = new Date().getFullYear();
      var res = {};
      switch(periodType)
      {
        case 'Semestre':
          res =
          {
            value: getSemester(),
            year: currentYear
          };
          break;
        case 'Cuatrimestre':
          res =
          {
            value: getQuarter(),
            year: currentYear
          };
          break;
        case 'Trimestre':
          res =
          {
            value: getTrimester(),
            year: currentYear
          };
          break;
        case 'Bimestre':
          res =
          {
            value: getBimester(),
            year: currentYear
          };
      }
      return res;
    }

    function getSemester ()
    {
      var month = new Date().getMonth();
      var res = 1;
      if (month > 5) { res = 2; }
      return res;
    }

    function getQuarter() {
      var month = new Date().getMonth();
      var res = 0;
      switch(month)
      {
        case month > 3 && month <=7:
          res = 2;
          break;
        case month > 7 && month <=11:
          res = 3;
          break;
        default:
          res = 1;
      }
      return res;
    }

    function getTrimester()
    {
      var month = new Date().getMonth();
      var res = 0;
      switch(month)
      {
        case month > 2 && month <=5:
          res = 2;
          break;
        case month > 5 && month <=8:
          res = 3;
          break;
        case month > 8 && month <=11:
          res = 4;
          break;
        default:
          res = 1;
      }
    }

    function getBimester()
    {
      var month = new Date().getMonth();
      var res = 0;
      switch(month)
      {
        case month > 1 && month <=3:
          res = 2;
          break;
        case month > 3 && month <=5:
          res = 3;
          break;
        case month > 5 && month <=7:
          res = 4;
          break;
        case month > 7 && month <=9:
          res = 5;
          break;
        case month > 9 && month <=11:
          res = 6;
          break;
        default:
          res = 1;
      }
      return res;
    }
    }
})();
