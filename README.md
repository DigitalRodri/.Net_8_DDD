# .NET_8_DDD
## DDD Project based on .NET 8

Migrated from this .NET Framework 4.8.1 project: https://github.com/DigitalRodri/.Net_Framework_DDD

*Run using IIS Express and ApplicationCore as Startup Project.*
*Postman collection available in Postman folder.*

Contains:
* Basic CRUD operations using Entity Framework Core V8 with SQLServer
* Dependency injection
* Entity-Dto mapping using Automapper
* Language handling using Resources file
* JWT Authentication with Authorization tag
* Password hashing with Pbkdf2
* Account GUID and creation/modification dates generated directly in SQL
* Unit testing
* Integration testing
* Centralized package versioning

Designed with Microsoft guidelines: 
* https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/net-core-microservice-domain-model
* https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice
