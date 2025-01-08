using Microsoft.AspNetCore.Identity;
using Serilog;
using UserService.API.Middlewares;
using UserService.Application;
using UserService.Domain.Entities.Auth;
using UserService.Infrastructure;
using UserService.Infrastructure.Configurations;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Seeders;

namespace UserService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            builder.Host.UseSerilog(logger);

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection(nameof(JWTConfiguration)));

            builder.Services.AddScoped<DataSeeder>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<TimingMiddleware>();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                seeder.SeedDataAsync().GetAwaiter().GetResult();
            }

            //using (var scope = app.Services.CreateScope())
            //{
            //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            //    var roles = builder.Configuration.GetSection("Roles").Get<string[]>();

            //    for (short i = 0; i < roles!.Length; i++)
            //    {
            //        if (!roleManager.RoleExistsAsync(roles[i]).Result)
            //            roleManager.CreateAsync(new IdentityRole<Guid>(roles[i])).Wait();
            //    }
            //}

            //using (var scope = app.Services.CreateScope())
            //{
            //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            //    string login = builder.Configuration["AdminSettings:Name"]!.ToString();
            //    string password = builder.Configuration["AdminSettings:Password"]!.ToString();

            //    if (userManager.FindByNameAsync(login).Result == null)
            //    {
            //        var user = new User()
            //        {
            //            Name = login,
            //            UserName = login,
            //            Email = builder.Configuration["AdminSettings:Email"]!.ToString(),
            //            PhoneNumber = builder.Configuration["AdminSettings:PhoneNumber"]!.ToString(),
            //            Wallet = 100000000000,
            //            Birthday = new DateTime(2005, 8, 14),
            //            ProfilePicture = "https://ih1.redbubble.net/image.2955130987.9629/raf,360x360,075,t,fafafa:ca443f4786.jpg",
            //        };

            //        userManager.CreateAsync(user, password).Wait();
            //        userManager.AddToRoleAsync(user, "Admin").Wait();
            //    }
            //}

            app.Run();
        }
    }
}
