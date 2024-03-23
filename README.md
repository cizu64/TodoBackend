This project contains codes for the NextJs Todo application available at this repo:https://github.com/cizu64/todoapp

1. Create the tables by running ef migration:

```
 dotnet ef migration add initial
```
2. If you don't have DotNet SDK or Runtime installed, visit https://dotnet.microsoft.com/en-us/download/dotnet/8.0 and install the runtime. The runtime is only needed to run the application. You can also install the SDK if you are having difficulty installing the runtime because it does not provide an installer for mac users.

   <i>Note: Make sure you install the runtime or sdk version 8.0</i>
   
3. Navigate to the project root path in the terminal and run this command:

```
dotnet run -lp https 
```

<i>You can also install vscode and the C# Dev kit extension to open the project and run directly from visual studio code.</i>

Finally, open the swagger page for the API description

https://localhost:7233/swagger/index.html

