using System.Security.Claims;
using TaskTrackerWebAPI.Extensions;
using TaskTrackerWebAPI.Filters;
using TaskTrackerWebAPI.Services;

namespace TaskTrackerWebAPI
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
            builder.Services.AddScoped<UserService>();
            
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