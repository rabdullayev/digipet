# DigiPet

DigiPet is a virtual pet game where you can adopt and feed animals, and most importantly make them happy!


### Built with & SDK

* [dotnet-core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

### Prerequisites

* [ASP.NET Core Runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1)

### Usage

From cmd prompt, you can use:

* `publish.bat` : to publish the code to `release` folder
* `run.bat` : to run the app from the `release` folder (make sure you publish it first before you run it). The app will launch at default kestrel port `5000`.

Testing : 

* if you have `postman`, you can import the `DigiPet.postman_collection.json` collection from `docs` folder and use collection-runner feature in postman to insert sample data. 
* alternatively you can open `http://localhost:5000/` in the browser and use Swagger UI. 

# Content

### Endpoints 

Base address : `http://localhost:5000`

* `GET /api/animals` : returns the list and details of all the animal templates available to adopt
* `GET /api/users` : returns the list of all users
* `GET /api/users/{id:int}` : returns the user with the specified id
* `POST /api/users` : creates a use with the specified `[FromBody] username`
* `GET /api/users/{userId:int}/animals` : returns all adopted animals for the user 
* `GET /api/users/{userId:int}/animals/{animalId:int}` : returns the adopted animal for the user
* `POST /api/users/{userId:int}/animals/adopt` : adopts the animal with the specified `[FromBody] code`
* `POST /api/users/{userId:int}/animals/stroke` : strokes the animal with the specified `[FromBody] animalId`
* `POST /api/users/{userId:int}/animals/feed` : feeds the animal with the specified `[FromBody] animalId` 

### Key Design Ideas

* `AnimalOptions` : contains the code, type and metric deltas for the animals. Injecting `AnimalOptions` through `appsettings.json` enables data-based elasticity for additional animals. You can easily add/remove animals to/from the list or change the metric details in `appsettings.json`.
* `HostedGameServer` : publishes `TickEvent` on a constant interval. Adopted animals subscibe to `TickEvent` to update their metrics.
* `UniqueIdProvider` : provides sequential integer ids for Domain Entities. Although, I'd prefer GUIDs, in order to make testing easier for you I decided to use int ids as primary keys.
* `InMemoryUserRepository` : encapsulates `MemoryCache`, which acts as an in-memory database. Again another design choice to make testing easier.


# Improvements

* Add Integration tests for error cases as well.
* Add service layer/mediator to make controllers thin.
* Add more info logging
* Although changing the `AnimalStat` from config is good, it's not perfect. I'd say storing it in a database table and providing a UI for the game designer would be an improvement.
* Since the server uses an in-memory Database, data won't survive server restarts. Replace `MemoryCache` with something that can backup data into the disk (such as Redis.)
* Add some business logic to Metrics (Maybe a min-max boundary, as negative Hunger doesn't make sense.)
