# Active Directory API

* [**DomainTools.Models**](./src/server/DomainTools.Models/) - Defines the data models for optimizing AD data capture.
* [**DomainTools.Services**](./src/server/DomainTools.Services/) - Defines the methods for retrieving AD data.
* [**DomainTools.Api**](./src/server/DomainTools.Api/) - API that exposes public service methods as REST endpoints for retrieving AD data.

## Getting Started

To run, change into the `src/server/DomainTools.Api` directory and run `dotnet run`.

Open `http://localhost:5000/swagger` to hit the swagger endpoint for the API.