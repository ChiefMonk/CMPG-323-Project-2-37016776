
# CMPG 323 Project 2 : IoT Device Management System
<img src="https://github.com/ChiefMonk/CMPG-323-Overview-37016776/blob/main/nwu_logo.jpg" width="200px" style="text-align:center;float:center;" />

### Table of Contents
1. [Introduction](#intro) 
2. [Technology Stack](#tech)
3. [Project Structure and WebAPI Endpoints](#struc)
4. [Azure API Manager](#apiman)
5. [Dependencies](#nuget)
6. [Contributors](#cont)
7. [References](#refs)

<a name="intro"></a>
## 1. Introduction
This is the second project of the CMPG 323 module deliverables. Smart devices such as voice controllers, security lights, smart locks and Wi-Fi-enabled devices can communicate and exchange data over the Internet. Devices form distributed ecosystems that can perform environmental monitoring of homes and buildings.

An IoT Device Management System keeps track of the whereabouts of all IoT devices deployed by the organisation. Depending on the type of organisation, different categories of devices are used. Each IoT device is initially categorised and registered. Then, IoT devices are deployed throughout the organisation's buildings in predefined zones. Administrators can view all IoT devices, update their properties, add new devices and move them to other zones.


### 1.1 Entity Information
Therefore, for the complete and satisfactory operations of the IoT Device Management System, the following information is stored in the database about each entity:
* System User
    * User Name
    * Password
    * Email Address
    * Phone Number
    * Role

* Category
    * Category ID
    * Category Name
    * Category Description
    * Date and Time Created

* Zone
    * Zone ID
    * Zone Name
    * Zone Description
    * Date and Time Created

* Device
    * Device ID
    * Device Name
    * Category ID
    * Zone ID
    * Status
    * Is Active
    * Date and Time Created

* User Session
    * Session Token
    * Login Date and Time
    * Logout Date and Time


### 1.2 Entity Rules and Restrictions
The above entity information is stored in a relational database. The database tables do not have a complete set of constraints that could prevent or limit, for example the deletion of mandatory information.
Therefore most of the data integrity rules are enforced with the application (WebAPI), and the following are some of the applicable rules:
* System User
    * Must have a non-empty Username
    * Must have a non-empty strong password
    * Must have a non-empty email address
    * Must have a non-empty phone Number
    * The role can only be Admin or User 

* Category
    * Must have a valid and unique GUID as a category id
    * Must have a non-empty category name
    * Must have a non-empty category description
    * Must have a valid creation date and time 
    * On creation, the category id is checked for uniqueness
    * A category with linked devices can not be deleted

* Zone
    * Must have a valid and unique GUID as a zone id
    * Must have a non-empty zone name
    * Must have a non-empty zone description
    * Must have a valid creation date and time 
    * On creation, the zone id is checked for uniqueness
    * A zone with linked devices can not be deleted

* Device
    * Must have a valid and unique GUID as a device id
    * Must have a non-empty device name
    * Must be assigned to a valid a category
    * Must be assigned to a valid a zone
    * Must have a valid and current status
    * Can be set to be active or not
    * Must have a valid creation date and time 
    * On creation, the zone id is checked for uniqueness
    * On creation or update, the zone id is checked if valid
    * On creation or update, the category id is checked if valid

* User Session
    * A GUID as session token is created and stored on user login
    * The login date and time is stored on user login
    * The logout date and time is null by default and is update on user logout

<a name="tech"></a>
## 2. Technology Stack
Representational State Transfer (REST) is a model and architectural style for web services over HTTP. When this model is used for API design, IoT devices can be managed using the Cloud. Therefore, the IoT Device Management System should be implemented as a set of RESTful APIs.
Representational State Transfer (REST) is a model and architectural style for web services over HTTP. When this model is used for API design, IoT devices can be managed using the Cloud. Therefore, the IoT Device Management System should be implemented as a set of RESTful APIs.

The diagram below described a typical client-webapi relationship via a restful service.
<img src="restful.png" width="500px" style="display: inline-block; margin: 0 auto; max-width: 300px" />
<p><sup></sup><em>image source: https://medium.com/@subhangdxt/beginners-guide-to-client-server-communication-8099cf0ac3af</em><sub></sub></p>

The archive the above architecture, the following technology stack was employed:
* WebAPI using .NET Core
* Swagger and OpenAPI to document the Web API 
* Microsoft SQL Server Database in Azure
* Entity Framework Core for Object Relational Mapping (ORM)
* Microsoft Identity for User management, Authentication and Authorisation
* JSON Web Tokens (JWT) for representing claims securely between client and server.
* Deployment to Microsoft Azure App Service 
* WebAPI management with Microsoft Azure API Management service

<a name="struc"></a>
## 3. Project Structure and WebAPI Endpoints
The whole solution, named CMPG323.D37016776.Project2.sln, is created using Visual Studio 2019 community edition. The solution only includes a single project, Project2.WebAPI.cproj. The project has the following layers and services:
* Data Access Layer (DAL)
* Business Logic Layer (BLL)
* WebAPI Layer and Presentation via Swagger
* WebAPI Security using JWT
* WebAPI and Database hosting and management in Microsoft Azure

The WebAPI for this project is deployed at https://cmpg323-37016776-project2-webapi.azurewebsites.net. The WebAPI is documented using Swagger and can be accessed and tested at https://cmpg323-37016776-project2-webapi.azurewebsites.net/swagger/index.html. 

Most of the endpoints require an authenticated admin user. You can use the following steps to get access:
* Register as an admin user via swagger the endpoint api/security/register/admin
* Login to get a JWT token via swagger the endpoint api/security/login
* Add the JWT token to the Authorise option on the swagger page
* Run and Test any endpoint and functionality you want.

 The following are the endpoints exposed, grouped per applicable controller:
* CategoryController (api/categories)
    * api/categories/get-all
        * Action: GET
        * Description: Gets all categories
        * Request: None
        * Response: IList<DtoCategory>
        * Authorize: Yes
        * Roles: Admin
    * api/categories/get/{id} 
        * Action: GET
        * Description: Gets a particular category by its id
        * Request: GUID as id
        * Response: DtoCategory
        * Authorize: Yes
        * Roles: Admin
    * api/categories/get-num-of-zones-by-category/{id} 
        * Action: GET
        * Description: Gets the number of zones with devices linked to a category
        * Request: GUID as id
        * Response: number
        * Authorize: Yes
        * Roles: Admin
    * api/categories/create 
        * Action: POST
        * Description: Creates a new category
        * Request: DtoCategory
        * Response: DtoCategory
        * Authorize: Yes
        * Roles: Admin
    * api/categories/update/{id} 
        * Action: PATCH
        * Description: Updates or patches an existing category
        * Request: GUID as id and DtoCategory
        * Response: DtoCategory
        * Authorize: Yes
        * Roles: Admin
    * api/categories/delete/{id} 
        * Action: DELETE
        * Description: Deletes an existing category if no linked devices
        * Request: GUID as id
        * Response: Id of the deleted category
        * Authorize: Yes
        * Roles: Admin

* ZoneController (api/zones)
    * api/zones/get-all
        * Action: GET
        * Description: Gets all zones
        * Request: None
        * Response: IList<DtoZone>
        * Authorize: Yes
        * Roles: Admin
    * api/zones/get/{id} 
        * Action: GET
        * Description: Gets a particular zone by its id
        * Request: GUID as id
        * Response: DtoZone
        * Authorize: Yes
        * Roles: Admin
    * api/zones/get-num-of-categories-by-zone/{id} 
        * Action: GET
        * Description: Gets the number of categories with devices linked to a zone
        * Request: GUID as id
        * Response: number
        * Authorize: Yes
        * Roles: Admin
    * api/zones/create 
        * Action: POST
        * Description: Creates a new zone
        * Request: DtoZone
        * Response: DtoZone   
        * Authorize: Yes
        * Roles: Admin
    * api/categories/update/{id} 
        * Action: PATCH
        * Description: Updates or patches an existing zone
        * Request: GUID as id and DtoZone
        * Response: DtoZone
        * Authorize: Yes
        * Roles: Admin
    * api/zones/delete/{id} 
        * Action: DELETE
        * Description: Deletes an existing zone if no linked devices
        * Request: GUID as id
        * Response: Id of the deleted zone
        * Authorize: Yes
        * Roles: Admin

* DeviceController (api/devices)
    * api/devices/get-all
        * Action: GET
        * Description: Gets all devices
        * Request: GUID as id
        * Response: IList<DtoDevice>
        * Authorize: Yes
        * Roles: Admin
    * api/devices/get-all-by-zone/{zoneId}
        * Action: GET
        * Description: Gets all devices by zone id
        * Request: UID as zone id
        * Response: IList<DtoDevice>
        * Authorize: Yes
        * Roles: Admin
    * api/devices/get-all-by-category/{categoryId}
        * Action: GET
        * Description: Gets all devices by category id
        * Request: UID as category id
        * Response: IList<DtoDevice>
        * Authorize: Yes
        * Roles: Admin
    * api/devices/get/{id} 
        * Action: GET
        * Description: Gets a particular device by its id
        * Request: GUID as id
        * Response: DtoDevice
        * Authorize: Yes
        * Roles: Admin
    * api/devices/create 
        * Action: POST
        * Description: Creates a new device
        * Request: DtoDevice
        * Response: DtoDevice
        * Authorize: Yes
        * Roles: Admin
    * api/devices/update/{id} 
        * Action: PATCH
        * Description: Updates or patches an existing device
        * Request: GUID as id and DtoDevice
        * Response: DtoDevice
        * Authorize: Yes
        * Roles: Admin
    * api/devices/delete/{id} 
        * Action: DELETE
        * Description: Deletes an existing device
        * Request: GUID as id
        * Response: Id of the deleted device
        * Authorize: Yes
        * Roles: Admin

* SecurityController (api/security)
    * api/security/login/{username}/{password}
        * Action: GET
        * Description: Logs in a system user - admin or normal user
        * Request: username and password
        * Response: DtoUserAuthenticationResponse
        * Authorize: No
        * Roles: Any
    * api/security/get/{id} 
        * Action: GET
        * Description: Gets a particular system user by its id
        * Request: GUID as id
        * Response: DtoSystemUser
        * Authorize: Yes
        * Roles: Admin
    * api/security/register/admin 
        * Action: POST
        * Description: Creates and registers an admin system user
        * Request: DtoUserRegistrationRequest
        * Response: DtoUserRegistrationResponse       
        * Authorize: No
        * Roles: Any
    * api/security/register/user 
        * Action: POST
        * Description: Creates and registers a normal system user
        * Request: DtoUserRegistrationRequest
        * Response: DtoUserRegistrationResponse
        * Authorize: No
        * Roles: Any
    * api/security/logout 
        * Action: DELETE
        * Description: Logs out an authenticated user
        * Request: GUID as id
        * Response:String message
        * Authorize: Yes
        * Roles: Any

<a name="apiman"></a>
## 4. Azure API Manager
The image below is a screenshot of the API endpoints under the Azure API manager.
<img src="api_manager.png" width="95%" style="text-align:center;float:center" />

The image below is a screenshot of a test done under the Azure API manager for the login endpoint, api/security/login/{username}/{password}.
<img src="api_manager_login_test.png" width="95%" style="text-align:center;float:center" />

<a name="nuget"></a>
## 5. Dependencies
The following nuget packages are referenced by the Project2.WebAPI project.

 | Package  |  Version  |  License  |
 | ---  |  ---  |  ---  |
 | [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/3.1.28/)  |  3.1.28  |  [Apache 2.0](https://licenses.nuget.org/Apache-2.0)  |
 | [Microsoft.AspNetCore.Identity](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity/2.2.0/)  |  2.2.0  |  [Apache 2.0](https://licenses.nuget.org/Apache-2.0)  |
 | [Microsoft.AspNetCore.Identity.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.EntityFrameworkCore/3.1.28/)  |  3.1.28  |  [Apache 2.0](https://licenses.nuget.org/Apache-2.0)  |
 | [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/3.1.28/)  |  3.1.28  |  [Apache 2.0](https://licenses.nuget.org/Apache-2.0)  |
 | [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/3.1.28/)  |  3.1.28  |  [Apache 2.0](https://licenses.nuget.org/Apache-2.0)  |
 | [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/3.1.28/)  |  3.1.28  |  [Apache 2.0](https://licenses.nuget.org/Apache-2.0)  |
 | [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/3.1.28/)  |  3.1.28  |  [Apache 2.0](https://licenses.nuget.org/Apache-2.0)  |
 | [Microsoft.OpenApi](https://www.nuget.org/packages/Microsoft.OpenApi/1.3.2/)  |  1.3.2  |  [Apache 2.0](https://licenses.nuget.org/Apache-2.0)  |
 | [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/6.4.0/)  |  6.4.0  |  [Apache 2.0](https://licenses.nuget.org/Apache-2.0)  |

<a name="cont"></a>
## 6. Contributors
* [Chipo Hamayobe (37016776)](https://github.com/ChiefMonk) - Project Lead

<a name="refs"></a>
## 7. References
### .NET Core WebAPIs
* [Tutorial: Create a web API with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api)
* [Create web APIs with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/web-api)
* [Create a web API with ASP.NET Core controllers](https://docs.microsoft.com/en-us/learn/modules/build-web-api-aspnet-core)
* [Controller action return types in ASP.NET Core web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types)
* [Dependency injection in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1)
* [ASP.NET Core web API documentation with Swagger / OpenAPI](https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-3.1)
* [Create microservices with .NET and ASP.NET Core](https://docs.microsoft.com/en-us/learn/paths/create-microservices-with-dotnet/)
* [Build your first microservice with .NET](https://docs.microsoft.com/en-us/learn/modules/dotnet-microservices/)
* [Automating ASP.NET Core Web API Creation That Communicates With Your Database in 60 Seconds or Less](https://thejpanda.com/2020/08/10/python-automating-asp-net-core-web-api-creation-that-communicates-with-your-database-in-60-seconds-or-less/)
* [Beginners Guide to Client Server Communication](https://medium.com/@subhangdxt/beginners-guide-to-client-server-communication-8099cf0ac3af)
### Entity Framework Core
* [Entity Framework Core in ASP.NET Core 3.1 – Getting Started](https://procodeguide.com/programming/entity-framework-core-in-asp-net-core/)
* [Getting Started with EF Core](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-3.1)
* [Tutorial: Get started with EF Core in an ASP.NET MVC web app](https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)
* [Join two entities in .NET Core, using lambda and Entity Framework Core](https://jd-bots.com/2022/01/24/join-two-entities-in-net-core-using-lambda-and-entity-framework-core/)
* [Microsoft.AspNetCore.Identity.EntityFrameworkCore Namespace](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore?view=aspnetcore-1.1)
* [Connection Strings](https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-strings)
* [Identity model customization in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1)
* [Entity Framework Core Example](https://github.com/procodeguide/EFCore.Sample)
### WebAPI Security
* [Overview of ASP.NET Core authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-3.1)
* [How to implement JWT authentication in ASP.NET Core](https://www.infoworld.com/article/3669188/how-to-implement-jwt-authentication-in-aspnet-core-6.html)
* [Introduction to JSON Web Tokens](https://jwt.io/introduction)
* [JWT Handbook](https://auth0.com/resources/ebooks/jwt-handbook)
* [Manage JSON Web Tokens in development with dotnet user-jwts](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn?view=aspnetcore-7.0&tabs=windows&viewFallbackFrom=aspnetcore-3.1)
* [Authentication and authorization in Azure App Service and Azure Functions](https://docs.microsoft.com/en-us/azure/app-service/overview-authentication-authorization?toc=%2Faspnet%2Fcore%2Ftoc.json&bc=%2Faspnet%2Fcore%2Fbreadcrumb%2Ftoc.json&view=aspnetcore-3.1)
### Microsoft Azure
* [Microsoft Azure Fundamentals: Describe cloud concepts](https://docs.microsoft.com/en-us/learn/paths/microsoft-azure-fundamentals-describe-cloud-concepts/)
* [Describe cloud computing](https://docs.microsoft.com/en-us/learn/modules/describe-cloud-compute/)
* [Describe the benefits of using cloud services](https://docs.microsoft.com/en-us/learn/modules/describe-benefits-use-cloud-services/)
* [Describe cloud service types](https://docs.microsoft.com/en-us/learn/modules/describe-cloud-service-types/)
* [Introduction to Azure API Management](https://docs.microsoft.com/en-us/learn/modules/introduction-to-azure-api-management/)
* [Explore API Management](https://docs.microsoft.com/en-us/learn/modules/explore-api-management/)
* [Describe cloud service types](https://docs.microsoft.com/en-us/learn/modules/describe-cloud-service-types/)
* [Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/)



