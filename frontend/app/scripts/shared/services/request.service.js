(function() {
    'use strict';

    angular
        .module('silabi')
        .service('RequestService', RequestService);

    RequestService.$inject = ['$http', '$q', 'CacheFactory', 'API_URL', 'lodash'];

    function RequestService($http, $q, CacheFactory, API_URL, _) {

      this.pdf = pdfRequest;
      this.get = getRequest;
      this.put = putRequest;
      this.post = postRequest;
      this.delete = deleteRequest;

      /**
      * Join a base URL and a endpoint.
      * @param baseUlr The base URL.
      * @param endpoint The endpoint.
      * @return The joined url.
      * @example join('http://localhost/api/v1/', '/students/201242273') => 'http://localhost/api/v1/students/201242273'
      */
      function join(baseUrl, endpoint) {
        if (_.endsWith(baseUrl, '/')) {
          baseUrl = baseUrl.substring(0, baseUrl.length - 1);
        }
        if (_.endsWith(endpoint, '/')) {
          endpoint = endpoint.substring(0, endpoint.length - 1);
        }
        if (!_.startsWith(endpoint, '/')) {
          endpoint = '/' + endpoint;
        }
        return baseUrl + endpoint;
      }

      function parseFields(fields) {
        if (_.isArray(fields)) {
          var result = '';
          for (var i = 0; i < fields.length; i++) {
            result += fields[i];
            if (i < fields.length - 1) { result += ','; }
          }
          return result;
        }
        else {
          return fields;
        }
      }

      function parseSortField(obj) {
        var result = '';
        if (obj.type === 'DESC') {
          result += '-';
        }
        result += obj.field;
        return result;
      }

      function parseSort(sort) {
        var result = '';

        if (_.isArray(sort)) {
          for (var i = 0; i < sort.length; i++) {
            result += parseSortField(sort[i]);
            if (i < sort.length - 1) { result += ','; }
          }
        }
        else if (_.isString(sort)) {
          result = sort;
        }
        else if (_.isObject(sort)) {
          result = parseSortField(sort);
        }

        return result;
      }

      function parseQuery(query) {
        var result = '';
        var current;

        for (var property in query) {
          if (query.hasOwnProperty(property)) {
            current = query[property];
            if (_.isArray(current)) {
              for (var i = 0; i < current.length; i++) {
                result += property + '+' + current[i].operation + '+' + current[i].value + ',';
              }
            }
            else {
              result += property + '+' + current.operation + '+' + current.value + ',';
            }
          }
        }

        return result.substring(0, result.length - 1);
      }

      /**
      * Create a query string based on a request object.
      * @param request The request.
      * @return The query string.
      *
      * @example createQueryString({
      *   "page": 2,
      *   "limit": 30,
      *   "sort": { "type": "ASC", "field": "name" },
      *   "query": {
      *     "course.code": {
      *       "operation": "ge",
      *       "value": 312
      *     },
      *     "id": {
      *       "operation": "ne",
      *       "value": 12
      *     }
      *   }
      *   "fields": ["id", "period"]
      * }) => "page=2&limit=30&sort=name&q=course.code+ge+312,id+ne+12&fields=id,period"
      *
      * @example createQueryString({
      *   "sort": [{ "type": "ASC", "field": "name" }, { "type": "DESC", "field": "created_at" }],
      *   "query": {
      *     "id": [
      *       {
      *         "operation": "ge",
      *         "value": 300
      *       },
      *       {
      *         "operation": "le",
      *         "value": 400
      *       }
      *     ],
      *   },
      *   "fields": "id,period",
      *   "other1": "test1",
      *   "other2": "test2"
      * }) => "sort=name,-created_at&q=id+ge+300,id+le+400&fields=id,period&other1=test1&other2=test2"
      */
      function createQueryString(request) {
        var query = '';
        var addAmpersand = false;

        if (request['access_token']) {
          if (addAmpersand) { query += '&'; }
          query += 'access_token=' + request['access_token'];
          addAmpersand = true;
          delete request['access_token'];
        }

        if (request.fields) {
          if (addAmpersand) { query += '&'; }
          query += 'fields=' + parseFields(request.fields);
          addAmpersand = true;
          delete request['fields'];
        }

        if (request.page) {
          if (addAmpersand) { query += '&'; }
          query += 'page=' + request.page;
          addAmpersand = true;
          delete request['page'];
        }

        if (request.limit) {
          if (addAmpersand) { query += '&'; }
          query += 'limit=' + request.limit;
          addAmpersand = true;
          delete request['limit'];
        }

        if (request.sort && !_.isEmpty(request.sort)) {
          if (addAmpersand) { query += '&'; }
          query += 'sort=' + parseSort(request.sort);
          addAmpersand = true;
          delete request['sort'];
        }

        if (request.query && !_.isEmpty(request.query)) {
          if (addAmpersand) { query += '&'; }
          query += 'q=' + parseQuery(request.query);
          addAmpersand = true;
          delete request['query'];
        }

        for (var property in request) {
          if (request.hasOwnProperty(property)) {
            if (addAmpersand) { query += '&'; }
            query += property + '=' + request[property];
            addAmpersand = true;
          }
        }

        return query;
      }

      function serviceUnavailableResponse() {
        return {
          code: 503 ,
          error: 'ServiceUnavailable',
          description: 'El servidor no se encuentra disponible. Inténtelo luego.'
        };
      }

      function pdfRequest(endpoint, request, cached) {
        var defer = $q.defer();
        var url = join(API_URL, endpoint);

        if (_.isNull(cached) || _.isUndefined(cached)) {
          cached = false;
        }

        if (request) {
          var queryString = createQueryString(request);
          url += '?' + queryString;
        }

        var config = {
          cache: cached,
          headers: {
              accept: 'application/pdf'
          },
          responseType: 'arraybuffer',
          transformResponse: function (data) {
              var pdf;
              if (data) {
                  pdf = new Blob([data], {
                      type: 'application/pdf'
                  });
              }
              return pdf;
          }
        };

        $http.get(url, config)
        .then(
          function(response) {
            defer.resolve(response.data);
          },
          function(response) {
            if (response.status === 0) {
              response.data = serviceUnavailableResponse();
            }
            defer.reject(response.data);
          }
        );

        return defer.promise;
      }

      /**
      * Perform a GET request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param request The request.
      * @param cached Flag that indicate if the response should be inserted in the cache.
      * @return A promise.
      * @example getRequest('/students', {fields: '...', sort: '...'}, true)
      */
      function getRequest(endpoint, request, cached) {
        var defer = $q.defer();
        var url = join(API_URL, endpoint);

        if (_.isNull(cached) || _.isUndefined(cached)) {
          cached = false;
        }

        if (request) {
          var queryString = createQueryString(request);
          url += '?' + queryString;
        }

        $http.get(url, { cache: cached })
        .then(
          function(response) {
            defer.resolve(response.data);
          },
          function(response) {
            if (response.status === 0) {
              response.data = serviceUnavailableResponse();
            }
            defer.reject(response.data);
          }
        );

        return defer.promise;
      }

      /**
      * Perform a POST request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param data The data to sent.
      * @return A promise.
      * @example postRequest('/students', {'name': '...', 'username': '...'})
      */
      function postRequest(endpoint, data) {
        var url = join(API_URL, endpoint);
        var defer = $q.defer();

        $http.post(url, data)
        .then(
          function(response) {
            CacheFactory.get('$http').removePrefix(url);
            defer.resolve(response.data);
          },
          function(response) {
            if (response.status === 0) {
              response.data = serviceUnavailableResponse();
            }
            defer.reject(response.data);
          }
        );

        return defer.promise;
      }

      /**
      * Perform a PUT request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param data The data to sent.
      * @return A promise.
      * @example putRequest('/students/201242273', {'name': '...', 'username': '...'})
      */
      function putRequest(endpoint, data) {
        var url = join(API_URL, endpoint);
        var defer = $q.defer();

        $http.put(url, data)
        .then(
          function(response) {
            CacheFactory.get('$http').removePrefix(url.substring(0, url.lastIndexOf('/')));
            defer.resolve(response.data);
          },
          function(response) {
            if (response.status === 0) {
              response.data = serviceUnavailableResponse();
            }
            defer.reject(response.data);
          }
        );

        return defer.promise;
      }

      /**
      * Perform a DELETE request to the SiLabI web service.
      * @param endpoint The endpoint.
      * @param data The data to sent.
      * @return A promise.
      * @example deleteRequest('/students/201242273', {'access_token': '...'})
      */
      function deleteRequest(endpoint, data) {
        var url = join(API_URL, endpoint);
        var defer = $q.defer();

        var config = {
          method: 'DELETE',
          headers: {'Content-Type': 'application/json'},
          url: url,
          data: data
        };

        $http(config)
        .then(
          function(response) {
            CacheFactory.get('$http').removePrefix(url.substring(0, url.lastIndexOf('/')));
            defer.resolve(response.data);
          },
          function(response) {
            if (response.status === 0) {
              response.data = serviceUnavailableResponse();
            }
            defer.reject(response.data);
          }
        );

        return defer.promise;
      }
    }
})();
