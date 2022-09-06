# CMPG 323 Project 2
<img src="https://github.com/ChiefMonk/CMPG-323-Overview-37016776/blob/main/nwu_logo.jpg" width="100" /> 

## Table of Contents
1. [Introduction](#intro)
3. [Technology Stack](#tech)
2. [Project Structure](#struc)
4. [Referenced Nugets](#nuget)
5. [Contributors](#cont)
6. [References](#refs)

<a name="intro"></a>
## 1. Introduction
The overview document explains the overall project structure for the semester, and the general branching strategy for each project repository and also serves as a general guide to everything about this semester's projects for CMPG 323.

<a name="tech"></a>
## 2. Technology Stack
The overview document explains the overall project structure for the semester, and the general branching strategy for each project repository and also serves as a general guide to everything about this semester's projects for CMPG 323.

<a name="struc"></a>
## Project Structure
For the work to be done this semester, a single Kanban project (<a href="https://github.com/users/ChiefMonk/projects/5">CMPG 323 Semester Project Plan Kanban Guide</a>) is created to outline and plan for all the work to be done. The scheduled work will be done over 8 Sprints, each lasting 10 working days (2 calendar weeks). At the beginning of every sprint, mostly on the first day, a sprint planning session is convened to categorise and itemise what will be done and allocate appropriate resources and time to each issue. 

However, to properly manage the project and meet the various sprint deadlines, the work is further planned and divided weekly. This also helps to negate any time challenges that could be encountered over the 2-week fixed time period.
 
<a name="nuget"></a>
## Referenced Nugets
The main idea behind any branching strategy is to isolate the work into different types of branches. There are lots of ways of structuring the braches to meet various organisational and strategic needs. However, for my project, only the following resource branches will be developed:
* <strong>main</strong> : will serve as the master branch with the most current working, relatively non-buggy code.
* <strong>develop</strong> : will serve as the development branch code that will be merged into the <strong>main</strong> branch once properly tested. Any new work must be done on the <strong>develop</strong> 
* <strong>release</strong> : Once a release is required for a particular environment (e.g. prod), a <strong>release</strong> branch will be created off the <strong>main</strong> branch and appropriately numbered and tagged.
* <strong>hotfix</strong> : If an urgent bug has been discovered in a particular release or <strong>main</strong> branch, a <strong>hotfix</strong> branch will be created off that branch and the effective fix applied. Once applied, the code will then be merged back via pull-requests into the applicable branch, and where necessary also into the <strong>develop</strong> branch.

<a name="cont"></a>
## Contributors
* [Chipo Hamayobe](https://github.com/ChiefMonk) - Project Lead

<a name="refs"></a>
## References
### A. .NET Core WebAPIs
* [Tutorial: Create a web API with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api)
* [Create web APIs with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/web-api)
* [Create a web API with ASP.NET Core controllers](https://docs.microsoft.com/en-us/learn/modules/build-web-api-aspnet-core)
* [Controller action return types in ASP.NET Core web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types)
* [Dependency injection in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1)
* [ASP.NET Core web API documentation with Swagger / OpenAPI](https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-3.1)
* [Create microservices with .NET and ASP.NET Core](https://docs.microsoft.com/en-us/learn/paths/create-microservices-with-dotnet/)
* [Build your first microservice with .NET](https://docs.microsoft.com/en-us/learn/modules/dotnet-microservices/)
* [Automating ASP.NET Core Web API Creation That Communicates With Your Database in 60 Seconds or Less](https://thejpanda.com/2020/08/10/python-automating-asp-net-core-web-api-creation-that-communicates-with-your-database-in-60-seconds-or-less/)
### B. Entity Framework Core
* [Entity Framework Core in ASP.NET Core 3.1 – Getting Started](https://procodeguide.com/programming/entity-framework-core-in-asp-net-core/)
* [Getting Started with EF Core](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-3.1)
* [Tutorial: Get started with EF Core in an ASP.NET MVC web app](https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)
* [Join two entities in .NET Core, using lambda and Entity Framework Core](https://jd-bots.com/2022/01/24/join-two-entities-in-net-core-using-lambda-and-entity-framework-core/)
* [Microsoft.AspNetCore.Identity.EntityFrameworkCore Namespace](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore?view=aspnetcore-1.1)
* [Connection Strings](https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-strings)
* [Identity model customization in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1)
* [Entity Framework Core Example](https://github.com/procodeguide/EFCore.Sample)
### C. WebAPI Security
* [Overview of ASP.NET Core authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-3.1)
* [How to implement JWT authentication in ASP.NET Core](https://www.infoworld.com/article/3669188/how-to-implement-jwt-authentication-in-aspnet-core-6.html)
* [Introduction to JSON Web Tokens](https://jwt.io/introduction)
* [JWT Handbook](https://auth0.com/resources/ebooks/jwt-handbook)
* [Manage JSON Web Tokens in development with dotnet user-jwts](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn?view=aspnetcore-7.0&tabs=windows&viewFallbackFrom=aspnetcore-3.1)
* [Authentication and authorization in Azure App Service and Azure Functions](https://docs.microsoft.com/en-us/azure/app-service/overview-authentication-authorization?toc=%2Faspnet%2Fcore%2Ftoc.json&bc=%2Faspnet%2Fcore%2Fbreadcrumb%2Ftoc.json&view=aspnetcore-3.1)
### D. Microsoft Azure
* [Microsoft Azure Fundamentals: Describe cloud concepts](https://docs.microsoft.com/en-us/learn/paths/microsoft-azure-fundamentals-describe-cloud-concepts/)
* [Describe cloud computing](https://docs.microsoft.com/en-us/learn/modules/describe-cloud-compute/)
* [Describe the benefits of using cloud services](https://docs.microsoft.com/en-us/learn/modules/describe-benefits-use-cloud-services/)
* [Describe cloud service types](https://docs.microsoft.com/en-us/learn/modules/describe-cloud-service-types/)
* [Introduction to Azure API Management](https://docs.microsoft.com/en-us/learn/modules/introduction-to-azure-api-management/)
* [Explore API Management](https://docs.microsoft.com/en-us/learn/modules/explore-api-management/)
* [Describe cloud service types](https://docs.microsoft.com/en-us/learn/modules/describe-cloud-service-types/)
* [Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/)
### .gitignore
* [Git .gitignore File](https://www.atlassian.com/git/tutorials/saving-changes/gitignore)



