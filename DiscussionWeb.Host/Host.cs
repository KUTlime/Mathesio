using System;

using DiscussionWeb.Data.DbContexts;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscussionWeb.Host
{
	public class Host
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

			// migrate the database.  Best practice = in Main, using service scope
			using (var scope = host.Services.CreateScope())
			{
				try
				{
					var context = scope.ServiceProvider.GetService<DiscussionWebDbContext>();
					// for demo purposes only
					context.Database.EnsureDeleted();
					context.Database.Migrate();
				}
				catch (Exception ex)
				{
					var logger = scope.ServiceProvider.GetRequiredService<ILogger<Host>>();
					logger.LogError(ex, "An error occurred while migrating the database.");
				}
			}

			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
