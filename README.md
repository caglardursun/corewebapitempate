# DotnetCore Simple API

This project is basically an rest service for my ordinary project. 

### SQL Folder 
Holds the basic GeneralUsers table which we use for authentication 

### Logs Folder 
Every logging information will be place in here. Since we use serilog. 
We can put them into another database or table ...


### Source 

* API : API controller will be in this place

* Contacts : All the interfaces used by dependency injection will be placed in here.

* Data : All the database releated models and entities will be in here 

* DTO : Request and Response data types will be in here 

* Services : Al the services will be in here (Except the worker serice)

* Infrastructure : API's dependency mappings, healthcheck configurations, JWT Authentication, Swagger configuration will be in here  
