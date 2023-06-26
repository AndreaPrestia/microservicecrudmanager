# microservicecrudmanager
A little experiment to use only 5 minimal API endpoint to manage CRUD operations by entity name specified in the query string.

**What is the idea behind this?**

The idea is to have only 5 implementations of the http endpoints of minimal API and reach every data manager for an entity through reflection.

I want to avoid to have a lot of endpoint for basic CRUD operations.

**How it works?** 

In the **WebApi** project there is an example.

There is and implementation of the **IAddEntity<T>** interface as **TestEntityAdd** that manages the add of an entity that must be a class.

It is invoked through reflection passing in the http uri the entity name.

I want to add the cache to keep track of the entity types and implementations of the storages.

**How to use it**

The usage is very simple, just like in the **WebApi** example project you have to add only two lines of code:

```
using MicroservicesCrudManager.Core;
using WebApi.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.AddMscm();
builder.Services.AddScoped<TestAdd>();

var app = builder.Build();

app.MapMscmEndpoints();

app.Run();
```

As you can see there is an extension **AddMscm** that provides the injection of the StorageManager class in the service provider.

After the build of the builder, you have only to add **MapMscmEndpoints** that adds the 5 needed endpoints.

The rest is yours, implement your repository for every entity and go :) 
