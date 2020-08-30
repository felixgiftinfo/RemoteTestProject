using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject
{
    public static class AppErrorHandler
    {
        public static void CatchError(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                var response = new { Error = exception.Message };

                var result = JsonConvert.SerializeObject(response);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));
        }
    }
}
