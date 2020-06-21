using System.Net;

using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder.Extensions
{
	// ReSharper disable once InconsistentNaming
	public static class IApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
		{
			app.UseExceptionHandler(appBuilder =>
			{
				appBuilder.Run(async context =>
					{
						context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
						await context.Response.WriteAsync("An unexpected fault happened. Please, try again later.");
					}
				);
			});
			return app;
		}
	}
}
