# CMPG 323 Project 1
<img src="https://github.com/ChiefMonk/CMPG-323-Overview-37016776/blob/main/nwu_logo.jpg" width="100" /> 

The overview document explains the overall project structure for the semester, and the general branching strategy for each project repository and also serves as a general guide to everything about this semester's projects for CMPG 323.

## Project Structure
For the work to be done this semester, a single Kanban project (<a href="https://github.com/users/ChiefMonk/projects/5">CMPG 323 Semester Project Plan Kanban Guide</a>) is created to outline and plan for all the work to be done. The scheduled work will be done over 8 Sprints, each lasting 10 working days (2 calendar weeks). At the beginning of every sprint, mostly on the first day, a sprint planning session is convened to categorise and itemise what will be done and allocate appropriate resources and time to each issue. 

However, to properly manage the project and meet the various sprint deadlines, the work is further planned and divided weekly. This also helps to negate any time challenges that could be encountered over the 2-week fixed time period.
 
## Branching Strategy
The main idea behind any branching strategy is to isolate the work into different types of branches. There are lots of ways of structuring the braches to meet various organisational and strategic needs. However, for my project, only the following resource branches will be developed:
* <strong>main</strong> : will serve as the master branch with the most current working, relatively non-buggy code.
* <strong>develop</strong> : will serve as the development branch code that will be merged into the <strong>main</strong> branch once properly tested. Any new work must be done on the <strong>develop</strong> 
* <strong>release</strong> : Once a release is required for a particular environment (e.g. prod), a <strong>release</strong> branch will be created off the <strong>main</strong> branch and appropriately numbered and tagged.
* <strong>hotfix</strong> : If an urgent bug has been discovered in a particular release or <strong>main</strong> branch, a <strong>hotfix</strong> branch will be created off that branch and the effective fix applied. Once applied, the code will then be merged back via pull-requests into the applicable branch, and where necessary also into the <strong>develop</strong> branch.

## Repositories per Project
The semester’s work plan consists of five (5) project deliverables. To distinctly plan and separate the commitments per deliverable, five(5) GitHub repositories have been created for the entire work of the semester:
* Project 1 : [CMPG-323-Overview-37016776](https://github.com/ChiefMonk/CMPG-323-Overview-37016776) due on 18 August 2022.
* Project 2 : [CMPG-323-Project-2-37016776](https://github.com/ChiefMonk/CMPG-323-Project-2-37016776) due on 8 September 2022.
* Project 3 : [CMPG-323-Project-3-37016776](https://github.com/ChiefMonk/CMPG-323-Project-3-37016776) due on 29 September 2022.
* Project 4 : [CMPG-323-Project-4-37016776](https://github.com/ChiefMonk/CMPG-323-Project-4-37016776) due on 4 November 2022.
* Project 5 : [CMPG-323-Project-5-37016776](https://github.com/ChiefMonk/CMPG-323-Project-5-37016776) due on 5 November 2022.

## Storing API and other Credentials
Since all project releases will possibly be deployed to Microsofts's cloud porta, [Azure](https://azure.microsoft.com/en-us/), it will be appropriate and sufficient that all credentials and related secrets be stored in [Azure’s Key Vault](https://azure.microsoft.com/en-us/services/key-vault/). 

## .gitignore File
In general, Git sees every file in your working copy as one of three things:
* tracked - a file which has been previously staged or committed;
* untracked - a file which has not been staged or committed; or
* ignored - a file which Git has been explicitly told to ignore.
* 
Ignored files are usually build artifacts and machine generated files that can be derived from your repository source or should otherwise not be committed. Some common examples are:
* dependency caches, such as the contents of /node_modules or /packages
* compiled code, such as .o, .pyc, and .class files
* build output directories, such as /bin, /out, or /target
* files generated at runtime, such as .log, .lock, or .tmp
* hidden system files, such as .DS_Store or Thumbs.db
* personal IDE config files, such as .idea/workspace.xml

## Contributors
* [Chipo Hamayobe](https://github.com/ChiefMonk) - Project Lead

## References
### A. Web APIs
* 1. [Tutorial: Create a web API with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api)
* 2. [Create web APIs with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/web-api)
* 3. [Create a web API with ASP.NET Core controllers](https://docs.microsoft.com/en-us/learn/modules/build-web-api-aspnet-core)
* 4. [Controller action return types in ASP.NET Core web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types)
* 5. [Dependency injection in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1)
* 6. [ASP.NET Core web API documentation with Swagger / OpenAPI](https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-3.1)
* 7. [Create microservices with .NET and ASP.NET Core](https://docs.microsoft.com/en-us/learn/paths/create-microservices-with-dotnet/)
* 8. [Build your first microservice with .NET](https://docs.microsoft.com/en-us/learn/modules/dotnet-microservices/)
* 9. [Automating ASP.NET Core Web API Creation That Communicates With Your Database in 60 Seconds or Less](https://thejpanda.com/2020/08/10/python-automating-asp-net-core-web-api-creation-that-communicates-with-your-database-in-60-seconds-or-less/)
### B. Entity Framework
* 1. [Entity Framework Core in ASP.NET Core 3.1 – Getting Started](https://procodeguide.com/programming/entity-framework-core-in-asp-net-core/)
* 2. [Getting Started with EF Core](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-3.1)
* 3. [Tutorial: Get started with EF Core in an ASP.NET MVC web app](https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)
* 4. [Join two entities in .NET Core, using lambda and Entity Framework Core](https://jd-bots.com/2022/01/24/join-two-entities-in-net-core-using-lambda-and-entity-framework-core/)
* 5. [Microsoft.AspNetCore.Identity.EntityFrameworkCore Namespace](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore?view=aspnetcore-1.1)
* 6. [Connection Strings](https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-strings)
* 7. [Identity model customization in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1)
* 8. [Entity Framework Core Example](https://github.com/procodeguide/EFCore.Sample)
### C. WebAPI Security
* 1. [Overview of ASP.NET Core authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-3.1)
* 2. [How to implement JWT authentication in ASP.NET Core](https://www.infoworld.com/article/3669188/how-to-implement-jwt-authentication-in-aspnet-core-6.html)
* 3. [Introduction to JSON Web Tokens](https://jwt.io/introduction)
* 4. [JWT Handbook](https://auth0.com/resources/ebooks/jwt-handbook)
* 5. [Manage JSON Web Tokens in development with dotnet user-jwts](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn?view=aspnetcore-7.0&tabs=windows&viewFallbackFrom=aspnetcore-3.1)
* 6. [Authentication and authorization in Azure App Service and Azure Functions](https://docs.microsoft.com/en-us/azure/app-service/overview-authentication-authorization?toc=%2Faspnet%2Fcore%2Ftoc.json&bc=%2Faspnet%2Fcore%2Fbreadcrumb%2Ftoc.json&view=aspnetcore-3.1)
### D. Microsoft Azure
* 1. [Microsoft Azure Fundamentals: Describe cloud concepts](https://docs.microsoft.com/en-us/learn/paths/microsoft-azure-fundamentals-describe-cloud-concepts/)
* 2. [Describe cloud computing](https://docs.microsoft.com/en-us/learn/modules/describe-cloud-compute/)
* 3. [Describe the benefits of using cloud services](https://docs.microsoft.com/en-us/learn/modules/describe-benefits-use-cloud-services/)
* 4. [Describe cloud service types](https://docs.microsoft.com/en-us/learn/modules/describe-cloud-service-types/)
* 5. [Introduction to Azure API Management](https://docs.microsoft.com/en-us/learn/modules/introduction-to-azure-api-management/)
* 6. [Explore API Management](https://docs.microsoft.com/en-us/learn/modules/explore-api-management/)
* 7. [Describe cloud service types](https://docs.microsoft.com/en-us/learn/modules/describe-cloud-service-types/)
* 8. [Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/)
### .gitignore
* [Git .gitignore File](https://www.atlassian.com/git/tutorials/saving-changes/gitignore)



