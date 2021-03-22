# CKO Payment Gateway test

The solution is developed in VS2019, targetting .NET 5.0. Atleast through this project, I got to work on .NET 5.0 :).

The solution is divided into 5 projects:
## 1. PaymentGateway
This is the main project which has ASP.Net Core end points implemented in it. This is the starting point and as such setup the Web Host, services in container, logging, routing etc.
This references all other projects to complete the requirement of building the payment gateway API.
## 2. Dto
This is project holds Data Transfer Objects which are used in other projects.
##3. Services
This is the project which has the business logic in it. All the server side validations happen here and PaymentGateway project calls into this directly.
##4. Repository
This project is responsible for interacting with persistance store. For the sake of this exercise, the persistance store is just a ConcurrentDictionary.
##5 ServiceTests
This project has unit tests for classes in Services project. Core of the business logic is well tested. It makes use of nUnit and xUnit.
