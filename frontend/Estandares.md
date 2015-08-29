# Estándares y Patrones de Diseño

Se deben utilizar la guía de estilos y buenas prácticas de [Jhon Papa](https://github.com/johnpapa/angular-styleguide/blob/master/README.md).

## Nombres de Archivos

**Tipo de Archivo:** Servicio de Angular (.js)  
**Nombre:** objeto.service.js  
**Ejemplo:** student.service.js  

**Tipo de Archivo:** Controlador de Angular (.js)  
**Nombre:** objeto.controller.js  
**Ejemplo:** student.controller.js  

**Tipo de Archivo:** Vista de listar objetos (.html)  
**Nombre:** objeto.list.html  
**Ejemplo:** student.list.html  

**Tipo de Archivo:** Vista de crear objetos (.html)  
**Nombre:** objeto.create.html  
**Ejemplo:** student.create.html  

**Tipo de Archivo:** Vista de visualizar objeto (.html)  
**Nombre:** objeto.detail.html  
**Ejemplo:** student.detail.html  

## Servicios de Acceso a Datos

Los servicios encargados se hacer los CRUDs de los objetos deben tener el siguiente formato:

**Nombre:** *objeto* + Service  
**Ejemplo:** StudentService  

A su vez deben contener unicamente 5 métodos públicos:

1. `GetAll`

**Parámetros:** (request: object)  
**Descripción:** Recibe un objeto request y devuelve todos los objetos que cumplan con el request.

```javascript
request
{
  "fields": string,
  "sort" : {
    "field": string,
    "type": string {"ASC", "DESC"}
  },
  "query": object,
  "page": number,
  "limit": number,
  "access_token": string
}

ejemplo
{
  "fields": "id,name,email",
  "sort" : {
    "field": "name",
    "type": "ASC"
  },
  "query": {
    "field1": {
      "operation": "like",
      "value": "*jhon*"
    },
    "field2": {
      ...
    }
  },
  "page": 2,
  "limit": 20,
  "access_token": "..."
}
```

Los elemento del objeto request (fields, sort, query, page, limit) son OPCIONALES.

2. `GetOne`

**Parámetros:** (id: number)  
**Descripción:** Recibe un identificador y devuelve los datos asociados a ese objeto.

3. `Create`

**Parámetros:** (object: object)  
**Descripción:** Crea un objeto.

4. `Update`

**Parámetros:** (id: number, object: object)  
**Descripción:** Recibe un identificador y un objeto. Modifica los datos asociados a ese identificador.

5. `Delete`

**Parámetros:** (id: number)  
**Descripción:** Recibe un identificador y borra los datos asociados a ese objeto.

#### Plantilla

```javascript
(function() {
    'use strict';

    angular
        .module('module')
        .service('StudentService', StudentService);

    Service.$inject = ['dependencies'];

    function StudentService(dependencies) {
        this.GetAll = GetAll;
        this.GetOne = GetOne;
        this.Create = Create;
        this.Update = Update;
        this.Delete = Delete;

        function GetAll(request) {

        }

        function GetOne(id) {

        }

        function Create(object) {

        }

        function Update(id, object) {

        }

        function Delete(id) {

        }
    }
})();
```

## Controladores

Se debe usar la variable `vm` en vez del objecto `$scope`.  

**Nombre:** *objeto* + Controller  
**Ejemplo:** StudentController  

#### Plantilla

```javascript
(function() {
    'use strict';

    angular
        .module('module')
        .controller('StudentController', Controller);

    Controller.$inject = ['dependencies'];

    function Controller(dependencies) {
        var vm = this;

        activate();

        // Initial logic goes here.
        function activate() {

        }
    }
})();
```
