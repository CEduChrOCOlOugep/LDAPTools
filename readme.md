# Active Directory API

* [**LdapTools.Models**](./src/server/LdapTools.Models/) - Defines the data models for optimizing AD data capture.
* [**LdapTools.Services**](./src/server/LdapTools.Services/) - Defines the methods for retrieving AD data.
* [**LdapTools.Api**](./src/server/LdapTools.Api/) - API that exposes public service methods as REST endpoints for retrieving AD data.

## Getting Started

To run, change into the `src/server/LdapTools.Api` directory and run `dotnet run`.

Open `http://localhost:5000/swagger` to hit the swagger endpoint for the API.