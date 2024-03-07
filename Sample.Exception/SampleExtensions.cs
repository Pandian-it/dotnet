using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.ExceptionHandling
{
    public static class SampleExtensions
    {

        public static void ConfigureModelBindingExceptionHandling(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    ValidationProblemDetails error = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new ValidationProblemDetails(actionContext.ModelState)).FirstOrDefault();

                    // Here you can add logging to you log file or to your Application Insights.
                    // For example, using Serilog:
                    // Log.Error("{{@RequestPath}} received invalid message format: {{@Exception}}",
                    //   actionContext.HttpContext.Request.Path.Value,
                    //   error.Errors.Values);
                    return new BadRequestObjectResult(error);
                };
            });
        }
    }
}
