This project contains codes for the NextJs Todo application available at this repo: https://github.com/cizu64/todoapp 

Before you begin, ensure that you have Postgres sql installed on your local machine because this project currently uses postgres as the database. To install postgress visit https://www.postgresql.org/ 

1. Create the "Todo" database in postgres
   
2. Create the tables by applying ef migration:

```
 dotnet ef database update
```

3. If you don't have DotNet SDK or Runtime installed, visit https://dotnet.microsoft.com/en-us/download/dotnet/8.0 and install the runtime. The runtime is only needed to run the application. You can also install the SDK if you are having difficulty installing the runtime because it does not provide an installer for mac users.

   <i>Note: Make sure you install the runtime or sdk version 8.0</i>
   
3. Navigate to the project root path in the terminal and run this command:

```
dotnet run -lp https 
```

<i>You can also install vscode and the C# Dev kit extension to open the project and run directly from visual studio code.</i>

Finally, open the swagger page for the API description

https://localhost:7233/swagger/index.html

