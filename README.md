# CKO Payment Gateway test

The solution is developed in VS2019, targetting .NET 5.0. Atleast through this project, I got to work on .NET 5.0 :). Thanks for the opportunity.

## Overview
The solution is divided into 5 projects:
### 1. PaymentGateway
This is the main project which has ASP.Net Core end points implemented in it. This is the starting point and as such setup the Web Host, services in container, logging, routing etc.
This references all other projects to complete the requirement of building the payment gateway API.
### 2. Dto
This is project holds Data Transfer Objects which are used to receive incoming data and send the responses.
### 3. Services
This is the project which has the business logic in it. All the server side validations happen here and PaymentGateway project calls into this directly. This project is also responsible to call the persistance layer to read/write data from data store.
### 4. Repository
This project is responsible for interacting with persistance store. For the sake of this exercise, the persistance store is just a ConcurrentDictionary.
### 5. ServiceTests
This project has unit tests for classes in Services project. Core of the business logic is well tested. It makes use of nUnit and xUnit.

## How to run
[Swagger](https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-5.0) is used in this project to help with documentation/running the project.

From Visual Studio, press F5 or Ctrl+F5 to see the Swagger UI as shown below:

![alt-text](https://github.com/vikramrkin/PaymentGateway/blob/master/DocImages/1.png)


Chose the end point you want to call, fill out the parameters and press Execute button:

![alt-text](https://github.com/vikramrkin/PaymentGateway/blob/master/DocImages/2.png)


End point is called and results will shown as below:

![alt-text](https://github.com/vikramrkin/PaymentGateway/blob/master/DocImages/3.PNG)


## Features implemented

1. All end points asked in the test are implemented. They can be found in PaymentGateway\Controllers\PaymentGatewayController.cs.
2. Logging - Using nLog, all requests/errors are logged. Log files can be found in PaymentGateway\bin\Debug\net5.0\logs\PaymentGateway-log-yyyy-mm-dd.log.
3. Luhn check on credit card number along with xUnit tests.

## Enhancements/Improvements needed

Current solution was built in around 3 hours of time (including documentation). Some of the below features/enhancements could have been done if more time was spent on this:

1. Instead of a dictionary, use something like [LiteDb](https://www.litedb.org/).
2. Add checks for credit card end date and CVV. These fields are not used at present.
3. Current design has made the DTO layer common to all projects which is not ideal. To maintain full separation of concerns, use different set of objects and use AutoMapper to build these objects.
4. Implement client authentication using one of the several ways in ASP.Net Core.
