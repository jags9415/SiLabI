(function() {
    'use strict';

    angular
        .module('silabi')
        .service('StateService', StateService);

    StateService.$inject = ['$q'];

    function StateService($q) {
      this.GetStudentStates = GetStudentStates;
      this.GetProfessorStates = GetStudentStates;
      this.GetAdministradorStates = GetAdministradorStates;
      this.GetOperatorStates = GetAdministradorStates;
      this.GetGroupStates = GetGroupStates;
      this.GetLabStates = GetLabStates;

      function GetStudentStates() {
        var defer = $q.defer();
        var states = [
          {
            name: 'Cualquiera',
            value: '*'
          },
          {
            name: 'Activo',
            value: 'Activo'
          },
          {
            name: 'Inactivo',
            value: 'Inactivo'
          },
          {
            name: 'Bloqueado',
            value: 'Bloqueado'
          }
        ];
        defer.resolve(states);
        return defer.promise;
      }

      function GetAdministradorStates() {
        var defer = $q.defer();
        var states = [
          {
            name: 'Cualquiera',
            value: '*'
          },
          {
            name: 'Activo',
            value: 'Activo'
          },
          {
            name: 'Inactivo',
            value: 'Inactivo'
          }
        ];
        defer.resolve(states);
        return defer.promise;
      }

      function GetGroupStates() {
        var defer = $q.defer();
        var states = [
          {
            name: 'Cualquiera',
            value: '*'
          },
          {
            name: 'Activo',
            value: 'Activo'
          },
          {
            name: 'Inactivo',
            value: 'Inactivo'
          }
        ];
        defer.resolve(states);
        return defer.promise;
      }

      function GetLabStates() {
        var defer = $q.defer();
        var states = [
          {
            name: 'Cualquiera',
            value: '*'
          },
          {
            name: 'Activo',
            value: 'Activo'
          },
          {
            name: 'Inactivo',
            value: 'Inactivo'
          }
        ];
        defer.resolve(states);
        return defer.promise;
      }
    }
})();
