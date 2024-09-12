# Active Directory API

* [**LDAPTools.Models**](./src/server/LDAPTools.Models/) - Defines the data models for optimizing AD data capture.
* [**LDAPTools.Services**](./src/server/LDAPTools.Services/) - Defines the methods for retrieving AD data.
* [**LDAPTools.Api**](./src/server/LDAPTools.Api/) - API that exposes public service methods as REST endpoints for retrieving AD data.

## Getting Started

To run, change into the `src/server/LDAPTools.Api` directory and run `dotnet run`.

Open `http://localhost:5000/swagger` to hit the swagger endpoint for the API.