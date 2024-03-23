This project contains codes for the NextJs Todo application available at this repo:https://github.com/cizu64/todoapp

1. Create the tables by running ef migration:

```
 dotnet ef migration add initial
```
2. If you don't have DotNet SDK or Runtime installed, visit https://dotnet.microsoft.com/en-us/download/dotnet/8.0 and install the runtime. The runtime is only needed to run the application. You can also install the SDK if you are having difficulty installing the runtime because it does not provide an installer for mac users.

<i>Note: Make sure you install the runtime or sdk version 8.0</i>
   
3. To get the project up and running, run this command:

```
dotnet run -lp https 
```

And open the swagger page for the API description

https://localhost:7233/swagger/index.html

