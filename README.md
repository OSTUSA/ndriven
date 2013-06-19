NDriven
=======
A baseline for domain driven .NET applications.  The easiest way to start is to leverage the NDriven CLI (https://github.com/OSTUSA/ndriven-cli) to create a baseline project.

Project Structure
-----------------
NDriven is an extraction of the project [Pomodoro Power](https://github.com/OSTUSA/pomodoro-power).
The architecture in play is an onion architecture.

After cloning, the project will have the following directories:

```bash
.nuget
bin
lib
src
```

The `bin` directory contains a packaged `Migrate.exe` executable for handling migrations via [FluentMigrator](https://github.com/schambers/fluentmigrator).

The `lib` directory contains any depdendencies not handled by NuGet. It is packaged with the FluentMigrator.Runner.dll as it is a dependency of the Infrastructure.Migrations project, as well as the Test.Integration project.

src
---
The src directory contains all the application projects.

###Core###
Core contains the meat and potatoes of the application. It ships with the Domain namespace containing a sensible entity base as well as a generic repository interface.

Core has one and only one dependency on the [BCrypt package](http://nuget.org/packages/BCrypt/)

###Infrastructure.IoC###
This is where dependency resolution is contained. It currently contains one namespace, NHibernate, that handles resolving NHibernate repositories, thread safe session factories, and sessions.

It supports multiple session factory injection by working with the Infrastructure.NHibernate project's `SessionFactoryAttribute`. If multiple session factories aren't needed, this coupling can be dropped.

###Infrastructure.Migrations###
Migrations are handled via FluentMigrator, and are broken up into two namespaces: Migrations and Profiles. NDriven ships with a migration for creating the User table as well as a single profile for creating a development user.

In addition to migrations and profiles, this project contains a runner for executing migrations and profiles from code. This tool is used within Test.Integration to restore the database to a known state before each test.

###Infrastructure.NHibernate###
This contains everything needed to get started with the [NHibernate ORM](http://nhforge.org/). This project contains a `SessionFactoryBuilder` capable of creating session factories in a thread safe matter.

The one repository included implements the generic `IRepository` interface and covers most use cases.

###Presentation.Web###
NDriven contains an MVC presentation layer out of the box. The only infrastructure project it relies on is Infrastructure.IoC.

This presentation layer is packaged with display and input models, validation attributes, views, an auth service, and a `UserController` that provide basic forms authentication out of the box (without use of the traditional membership provider).

Presentatin.Web makes use of the "DefaultConnection" connection string for its data access.

The default UI kit is Twitter Bootstrap.

###Test.Unit###
All unit tests are included here

###Test.Integration###
NHibernate repository tests

App.config contains the connection string used for testing.

Take a look at the [DatabaseTestState](https://github.com/OSTUSA/ndriven/blob/master/src/Test.Integration/DatabaseTestState.cs) to see how a consistent test state is maintained in tests that use
the database. This class is also useful for loading profile data.

###Test.UI.Web.Features###
SpecFlow tests coupled with WebDriver for high level acceptance tests.

Building
--------
Thanks to NuGet, building is a snap. Just clone the repo, open the solution, and build. Building should take care of fetching all the dependencies.

The Database
------------
Out of the box, NDriven assumes a database named NDriven. You will likely want to changes this about the same time you change the solution name.

Assuming your local environment is using SQLExpress, creating the initial database is as easy as opening `cmd`, navigating to the project directory and firing this bad boy off:

```
bin\Migrate.exe -c "server=.\SQLExpress;database=NDriven;Integrated Security=SSPI" -db sqlserver2008
-a "src\Infrastructure.Migrations\bin\Debug\Infrastructure.Migrations.dll" -t migrate:up
```
