namespace A11d.Module;

using System.Reflection;

public static class ModuleExtensions
{
	/// <summary>
	/// Installs all <see cref="Module"/>s in the assembly of the specified marker type.
	/// </summary>
	/// <param name="builder">The <see cref="WebApplicationBuilder"/> to install modules to.</param>
	/// <typeparam name="TMarker">Any type in your assembly. This type is solely used to find your assembly.</typeparam>
	/// <returns>Returns the <see cref="WebApplicationBuilder"/> for chaining.</returns>
	public static WebApplicationBuilder Install<TMarker>(this WebApplicationBuilder builder)
	{
		return Install(builder, typeof(TMarker).Assembly);
	}

	/// <summary>
	/// Installs all <see cref="Module"/>s in the specified assembly.
	/// </summary>
	/// <param name="builder">The <see cref="WebApplicationBuilder"/> to install modules to.</param>
	/// <param name="assembly">The assembly to install modules from.</param>
	/// <returns>Returns the <see cref="WebApplicationBuilder"/> for chaining.</returns>
	public static WebApplicationBuilder Install(this WebApplicationBuilder builder, Assembly assembly)
	{
		builder.Services.ConfigureModulesServices(assembly);
		return builder;
	}

	/// <summary>
	/// Configures all <see cref="Module"/>s in the assembly of the specified marker type.
	/// </summary>
	/// <param name="application">The <see cref="WebApplication"/> to operate configurations on.</param>
	/// <typeparam name="TMarker">Any type in your assembly. This type is solely used to find your assembly.</typeparam>
	/// <returns>Returns the <see cref="WebApplication"/> for chaining.</returns>
	public static WebApplication Configure<TMarker>(this WebApplication application)
	{
		return Configure(application, typeof(TMarker).Assembly);
	}

	/// <summary>
	/// Configures all <see cref="Module"/>s in the specified assembly.
	/// </summary>
	/// <param name="application">The <see cref="WebApplication"/> to operate configurations on.</param>
	/// <param name="assembly">The assembly to configure modules from.</param>
	/// <returns>Returns the <see cref="WebApplication"/> for chaining.</returns>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "ASP0014", Justification = "This is a wrapper around all the modules.")]
	public static WebApplication Configure(this WebApplication application, Assembly assembly)
	{
		application.ConfigureModulesApplication(assembly);
		application.UseEndpoints(endpoints => endpoints.ConfigureModulesEndpoints(assembly));
		return application;
	}

	private static void ConfigureModulesServices(this IServiceCollection services, Assembly assembly)
		=> GetModules(assembly).ForEach(module => module.ConfigureServices(services));

	private static void ConfigureModulesApplication(this WebApplication application, Assembly assembly)
		=> GetModules(assembly).ForEach(module => module.ConfigureApplication(application));

	private static void ConfigureModulesEndpoints(this IEndpointRouteBuilder endpoints, Assembly assembly)
		=> GetModules(assembly).ForEach(module => module.ConfigureEndpoints(endpoints));

	private static List<Module> GetModules(Assembly assembly) => assembly
		.GetTypes()
		.Where(p => p.IsClass && p.IsAssignableTo(typeof(Module)) && p.IsAbstract == false)
		.Select(Activator.CreateInstance)
		.Cast<Module>()
		.ToList();
}