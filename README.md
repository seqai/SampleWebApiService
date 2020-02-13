# SampleWebApiService
Sample .NET Core Web Api Service using language-ext and EF Core

## Running the Service

The service can be run either using .NET Core runtime directly or in Docker

### Prerequisites

- .NET Core 3.1
- Docker

### Dotnet runtime

Simply run
```
dotnet run
```
By default the service will create and migrate SQLite database (this was done for testing purposes and can be turned off via configuration)
Adjust appsettings.json or provide environment variables. Service will run on default ports 5000 and 5001 for TLS. Automatic HTTPS redirect is turned off.

### Docker

The image is available in Docker Hub `seqai/sample-web-service`. Is is recommended to create volume for SQLite db.
To build and run locally run with superuser privileges

```
docker build -f SampleWebApiService/Dockerfile -t sample-web-service .
docker volume create testdb
docker run --mount source=testdb,target=/db -p 7777:80 -e "WEBAPI_PERSISTENCE__CONNECTIONSTRING=Data Source=/db/test.db" sample-web-service
```

This will route service HTTP endpoints to port 7777

### Working with the service

Navigate to [swagger/index.html](http://localhost:5000/swagger/index.html) to use Swagger UI or use `/calendar` endpoints directly

### Implementation notes

**As agreed via email** I adjusted several things in requirements to make service better compliant to common RESTful design (RFC 7231):

- For POST/PUT API as well as in responses `Members` field is now an array instead of comma separated values in example
- PUT endpoint expects full resource and return 204 on Success as per HTTP PUT specification
- PATCH endpoint was introduced for partial updates and uses JSON Patch standard
- GET individual resource is done by providing id in URL, e.g. `/calendars/1` 
- Instead of query and sort endpoint URL parameters for GET `/calendars` can be used and combined, e.g. `/calendars?name=EventName&eventOrganizer=Organizer&location=Location&sortType=Desc"

Main application architecture ideas:
- Use classic layered architecture and separating related layer classes into entities, business-objects and DTOs
- Use language-ext to promote more declarative and type-safe style, especially avoiding `null`s
- Additional industry-standard libraries were used: Autofac as IoC-container and Serilog to manage logging 

### Further development and problems out of the scope

- It is important to note that scheduling using UTC time and location is hard to manage because usually users are interested in local time for events (e.g. meeting at 2 PM in Amsterdam) and Timezones are subject to change/adjust and hard to predict, like reducing number of timezones or abolishing of Daylight Saving Time. Handling time should be engineered with great care and taking workflow and management into account
- Potential database concurrency issues for PUT and PATCH endpoints are not handled in this solution
- All code put in the single project to keep it simple. Real-life scalable solution will probably have separate projects for api, data-access layer, business orchestration, etc
- Mapping of between different related classes, entities, business-objects and DTOs, can be done more elegantly, e.g. using AutoMapper
- In order to implement JSONPatch easily I used business-object as a part of a API contract, while I would prefer to avoid it if possible 
- Configuration management kept simple and minimal 
- Persistence layer is rudimentary. SQLIte with EF Core is used will slight additional complexity for event members (additional table created). NoSql solution can be used easily for current requirements too
- DELETE operation might return 204 instead of 404, as this is a design choice
- Testing, mostly for mapping, which better to replace with AutoMapper