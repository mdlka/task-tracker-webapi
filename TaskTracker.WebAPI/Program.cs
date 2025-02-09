using System.Security.Claims;
using TaskTracker.Core.Repositories;
using TaskTracker.Core.Services;
using TaskTracker.Infrastructure.Extensions;
using TaskTracker.WebAPI.Filters;
using TaskTracker.Infrastructure.Repositories;
using TaskTracker.Infrastructure.Services;

namespace TaskTracker.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ApiExceptionFilterAttribute());
            });

            builder.Services.ConfigureCors();
            builder.Services.AddSwagger();
            builder.Services.AddDbContext(builder.Configuration);
            builder.Services.ConfigureJwtConfig(builder.Configuration);
            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddScoped<TodoItemsService>();
            builder.Services.AddScoped<BoardService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<CurrentUserService>();

            builder.Services.AddScoped<ICoreRepositoryWrapper, CoreRepositoryWrapper>();
            builder.Services.AddScoped<IAuthRepositoryWrapper, AuthRepositoryWrapper>();
            
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient(provider => 
                provider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User 
                ?? new ClaimsPrincipal());

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
                app.ConfigureSwagger();

            app.UseHttpsRedirection();

            app.UseCors("EnableCORS");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}