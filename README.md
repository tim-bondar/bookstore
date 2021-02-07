## Project Info
Simple application for CRUD operation with Books metadata.
 Allowed cover images are PNG and JPG formats with maximum size of 10 MB.

.Net 5.0 was used to develop API. EF Core was used to develop in-memory DB.

Was used CQS (command-query separation) pattern as an approach to develop this application.
MediatR package was used to set up this pattern.

Also was used Automapper to map transform similar objects. 
xUnit and FakeItEasy used to develop unit tests.

Decided to separate cover image content from Book metadata so client can easily cache images by links.
(Also in future images might be stored in different DB)

## How to use

### Run from source code
1) Build and run API project with default configuration
2) Navigate to swagger info page [http://localhost:5000/swagger](http://localhost:5000/swagger)

## Testing
### e2e
Import postman collection [] into Postman and run requests.
NOTE: Files are not attached to the collection and default mass-execution will fail.

### Unit
Run test from `Tests` project in IDE. Or execute `dotnet test` command in the root folder.
