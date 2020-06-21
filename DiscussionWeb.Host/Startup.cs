using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace DiscussionWeb.Host
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
				.AddControllers(options => options.ReturnHttpNotAcceptable = true)
				.AddXmlDataContractSerializerFormatters()
				.ConfigureApiBehaviorOptions(setupAction =>
				{
					setupAction.InvalidModelStateResponseFactory = context =>
					{
						// create a problem details object
						var problemDetailsFactory = context.HttpContext.RequestServices
							.GetRequiredService<ProblemDetailsFactory>();
						var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
							context.HttpContext,
							context.ModelState);

						// add additional info not added by default
						problemDetails.Detail = "See the errors field for details.";
						problemDetails.Instance = context.HttpContext.Request.Path;

						// find out which status code to use
						var actionExecutingContext =
							context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

						// if there are modelstate errors & all keys were correctly
						// found/parsed we're dealing with validation errors
						//
						// if the context couldn't be cast to an ActionExecutingContext
						// because it's a ControllerContext, we're dealing with an issue 
						// that happened after the initial input was correctly parsed.  
						// This happens, for example, when manually validating an object inside
						// of a controller action.  That means that by then all keys
						// WERE correctly found and parsed.  In that case, we're
						// thus also dealing with a validation error.
						if (context.ModelState.ErrorCount > 0 &&
							(context is ControllerContext ||
							 actionExecutingContext?.ActionArguments.Count ==
							 context.ActionDescriptor.Parameters.Count))
						{
							problemDetails.Type = "https://courselibrary.com/modelvalidationproblem";
							problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
							problemDetails.Title = "One or more validation errors occurred.";

							return new UnprocessableEntityObjectResult(problemDetails)
							{
								ContentTypes = { "application/problem+json" }
							};
						}

						// if one of the keys wasn't correctly found / couldn't be parsed
						// we're dealing with null/unparsable input
						problemDetails.Status = StatusCodes.Status400BadRequest;
						problemDetails.Title = "One or more errors on input occurred.";
						return new BadRequestObjectResult(problemDetails)
						{
							ContentTypes = { "application/problem+json" }
						};
					};
				});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app = env.IsDevelopment() ? app.UseDeveloperExceptionPage() : app.UseCustomExceptionHandler();
			app
				.UseHttpsRedirection()
				.UseRouting()
				.UseAuthorization()
				.UseEndpoints(endpoints => endpoints.MapControllers());
		}

		internal static IActionResult ProblemDetailsInvalidModelStateResponse(
			ProblemDetailsFactory problemDetailsFactory, ActionContext context)
		{
			var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);
			var result = (HttpStatusCode?)problemDetails.Status == HttpStatusCode.BadRequest ? new BadRequestObjectResult(problemDetails) : new ObjectResult(problemDetails);

			result.ContentTypes.Add("application/problem+json");
			result.ContentTypes.Add("application/problem+xml");

			return result;
		}
	}
}
