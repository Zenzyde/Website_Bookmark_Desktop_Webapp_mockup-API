# Website_Bookmark_Desktop_Webapp_mockup-API
My first ASP.NET Web API backend CRUD project, this is a mock-up-API for a webapp idea which involves being able to save website-bookmarks to a separate website which would display the bookmarks in a desktop-like manner (this would also make website bookmarks browser-agnostic)

The project uses SQLite and code-first migration with dotnet ef for the database, and user authentification with IdentityUser and JWT tokens for authorization.

# Testing
If you want to test it for yourself:
* Download the project, navigate into the "/bin/Release/net7.0"-folder and execute the "Website_Bookmark_Desktop_App_API".exe
* Open a browser tab and enter "http://localhost:5000" in the URL search field -- this should connect to the API's Swagger UI
* Have fun testing!