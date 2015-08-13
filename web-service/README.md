SiLabI Web Service
==============

Web Service desarrollado en C# para el sistema de citas SiLabI.  
_ _ _

##Para publicar el web service en IIS:  

####Requisitos:  

1. Instalar IIS y tener el sitio predeterminado [Default Web Site] habilitado.  
2. Instalar SQL Server 2008 y ejecutar los scripts para la creación y el llenado de la base de datos.  

####Instrucciones:

1. Ejecutar Visual Studio con permisos de administrator.  
2. Abrir el archivo de solución 'SiLabI.sln'.  
3. En el menu Compilar:  
  a. Presionar 'Publicar SiLabI Web Service'.  
  b. Seleccionar el perfil 'RESTful API v1'.  
  c. Presionar 'Publicar'.  

El web service podrá ser accedido a través de la dirección [http://localhost/api/v1/](http://localhost/api/v1/)