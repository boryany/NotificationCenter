# NotificationCenter

NotificationCenter is a solution that demonstrates a real-time notification system using:

•	.NET (API, Console Test),

•	EF Core (Database),

•	SignalR (WebSockets),

•	Angular (Frontend).

Prerequisites
  1.	NET 6 SDK (or later)
  2.	Node.js (16+ recommended) and npm (or yarn) for the Angular app
  3.	SQL Server or another supported database (for EF Core)
     
- Clone the Repository 
git clone [https://github.com/boryany/NotificationCenter.git]
 
- Configure Database Connection Strings
  1.	Open appsettings.json inside NotificationCenter.API 
  2.	Update the "DefaultConnection" with your local SQL Server or other DB details.
 
- Run EF Core Migrations and Update the Database
  From the NotificationCenter.Infrastructure or NotificationCenter.API project folder run: 

  dotnet ef migrations add InitialCreate

  dotnet ef database update

  This creates initial EF Core migrations and creates or updates your database schema.
 
- Build the Angular Frontend
  1.	Navigate to the Angular app folder NotificationCenter.API\wwwroot\angular-app
  2.	Install dependencies:
   npm install

  4.	Build for development or production: 
   ng build
 
   ng build --configuration production

  	The build artifacts will go to dist  folder in wwwroot\angular-app 

- Set Startup Projects (NotificationCenter.API + SimpleConsoleTestingAppBackend)
  In Visual Studio or VS Code, do the following:
  1.	Set multiple startup projects:
   -	NotificationCenter.API (the ASP.NET Core project)
   -	SimpleConsoleTestingAppBackend (the console test app)
   
- Launch and Test
   Run the solution and make sure both projects run in parallel:
    - The API starts on https://localhost:44320/ 
    - The console app will prompt you to enter commands to simulate new notification events.
    - Authenticate with one of the test users below. You can then see the notifications in real time.

- Test Users
  The following users are available for testing purposes (all have the same password password123):
  1.	ivan.petrov
  2.	ivana.ivanova
  3.	art.soft
  4.	dandn.soft
  	
  You can log in with any of these usernames + password123 to see different sets of notifications and to test real-time events.

