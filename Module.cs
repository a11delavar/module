namespace A11d.Module;

public abstract class Module
{
	/// <summary>
	/// Use this method to add services to the container. You don't need to call base.ConfigureServices() if you override this method.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
	public virtual void ConfigureServices(IServiceCollection services) { }

	/// <summary>
	/// Use this method to configure your application. You don't need to call base.ConfigureApplication() if you override this method.
	/// </summary>
	/// <param name="application">The <see cref="WebApplication"/> to configure.</param>
	public virtual void ConfigureApplication(WebApplication application) { }

	/// <summary>
	/// Use this method to define the endpoints for your application. You don't need to call base.ConfigureEndpoints() if you override this method.
	/// </summary>
	/// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to define endpoints for.</param>
	public virtual void ConfigureEndpoints(IEndpointRouteBuilder endpoints) { }
}