# Module

A library to facilitate developing with minimal APIs and vertical slicing your application to improve cohesion in your codebase. Each module can configure the services in the installation phase, configure the application and define endpoints in the build phase.

## API

### Modules

The `Module` class is the base class for all modules. It provides three virtual methods that you can override to add services, configure the application, and define endpoints:

- `ConfigureServices` to configure your application using the `IServiceCollection` parameter in the installation phase. Calling base is not needed.
- `ConfigureApplication` to configure your application using the `WebApplication` parameter in the build phase. Calling base is not needed.
- `ConfigureEndpoints` to define your application endpoints using the `IEndpointRouteBuilder` parameter in the build phase. Calling base is not needed.

### Application Entry Point

In your program entry point, you have two extension methods to install and configure your modules:

- `Install` to install your module using the `WebApplicationBuilder` parameter in the installation phase.
- `Configure` to configure your module using the `WebApplication` parameter in the build phase.

## Usage

First, activate the module in the entry point of your application:

```cs
// FILE: "./Program.cs"
WebApplication.CreateBuilder(args)
	.Install<MyApplication>().Build()
	.Configure<MyApplication>().Run();

// This is just a marker class to identify the assembly that contains the modules.
public class MyApplication { }
```

Then you can create modules to organize your application.

For example, consider a part of your application deals with Accounts and you want to organize it into a module. You can create `AccountsModule`:

```csharp
// FILE "./Accounts/AccountsModule.cs"

namespace MyApplication.Accounts;

// Assuming this module has a DbContext, GetAccount, and AuthenticateAccount command classes.

public class AccountsModule : Module
{
	public override void ConfigureServices(IServiceCollection services)
	{
		services.AddDbContext<AccountDbContext>();
		services.AddTransient<GetAccount>();
		services.AddTransient<AuthenticateAccount>();
	}

	public override void ConfigureApplication(WebApplication application)
	{
		application.UseAuthentication();
		application.UseAuthorization();
	}

	public override void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
	{
		endpoints.MapGet("/accounts/{id}", (GetAccount getAccount, Guid id) => getAccount.ExecuteAsync(id));
		endpoints.MapPost("/accounts/authenticate", (AuthenticateAccount authenticateAccount, AuthenticateAccount.Request request) => authenticateAccount.ExecuteAsync(request));
	}
}
```