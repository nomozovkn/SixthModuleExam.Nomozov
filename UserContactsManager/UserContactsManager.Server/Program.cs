using UserContacts.Server.Endpoints;
using UserContactsManager.Server.Configurations;
using UserContactsManager.Server.Middlewares;

namespace UserContactsManager.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.WebHost.ConfigureKestrel(options =>
            //{
            //    options.ListenAnyIP(5000); 
            //});



            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.ConfigurationJwtAuth();
            builder.Configuration();
            builder.ConfigureJwtSettings();
           
            builder.Services.ConfigureDependecies();
            builder.Services.AddMemoryCache();
            builder.Services.AddMemoryCache();
            var app = builder.Build();

            app.MapUserEndpoints();
            app.MapAuthEndpoints();
            app.MapRoleEndpoints();
            app.MapContactEndpoints();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<RequestDurationMiddleware>();
            //app.UseMiddleware<NightBlockMiddleware>();
            app.UseMiddleware<MaintenanceMiddleware>();
            app.UseMiddleware<GeoBlockMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
