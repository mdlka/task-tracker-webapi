using Microsoft.AspNetCore.Builder;

namespace TaskTracker.Infrastructure.Extensions
{
    public static class AppExtensions
    {
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}