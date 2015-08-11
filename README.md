SiLabI
======

Sistema de citas en línea para los laboratorios de inglés de la Escuela de Ciencias del Lenguaje del Instituto Tecnológico de Costa Rica.

- - -

Instituto Tecnológico de Costa Rica  
Ingeniería en Computación  
Proyecto de Software  
II Semestre 2015  

####Desarrolladores

José Andrés García Sáenz  
Leonardo Madrigal Valverde  
Emmanuel Murillo Sánchez  

_ _ _

###Para instalar y ejecutar el sitio:

La parte que se muestra al cliente de la aplicación presenta una estructura generada por [Yeoman](http://yeoman.io/).

Con NodeJS y npm previamente instalados, se ejecuta en una terminal en el folder de silabi-frontend:

####Desde Linux


```shell
sudo npm install
```

Esto instala las dependencias del sitio.

```shell
grunt test
```

El comando anterior levanta un servidor en el navegador predeterminado y hospeda una previsualización de la página.


####Desde Windows

Abrir el shell de node con permisos de administrador y ejecutar:

```shell
npm install
```

Las demás instrucciones son las mismas en ambas consolas.

#####Para correr las pruebas:

```shell
grunt serve
```

Una guía más completa se encuentra en la página oficial de [Yeoman](http://yeoman.io/codelab.html).
