
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.API.BLL.Data.Contexts;
using OnlineStore.API.Core.Models;
using OnlineStore.API.Core.Repositories;
using OnlineStore.API.Core.Services;
using OnlineStore.API.PL.Extention;
using OnlineStore.API.PL.Mapping;
using OnlineStore.API.PL.SeedData;
using OnlineStore.API.Repository.RealTime;
using OnlineStore.API.Repository.Repositories;
using OnlineStore.API.Services;

namespace OnlineStore.API.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var builder = WebApplication.CreateBuilder(args);

            #region Services
            // Add services to the container.

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://127.0.0.1:5500" , "https://project-college-mu.vercel.app/" , "https://project-college-mu.vercel.app");
                                      //policy.AllowAnyOrigin();
                                      policy.AllowAnyMethod();
                                      policy.AllowAnyHeader();
                                      policy.AllowCredentials();
                                  });
            });


            builder.Services.AddSignalR();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //Connection String
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options => {
                Options.User.RequireUniqueEmail = true;
                Options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();         //Allow DI to Store

            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAutoMapper(M => M.AddProfile(new ApplicationProfile(builder.Configuration)));

            builder.Services.AddSwaggerGenJwtAuth();

            builder.Services.AddCustomJwtAuth(builder.Configuration);
            #endregion


            var app = builder.Build();


            // Seed roles
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await RoleSeed.SeedRolesAsync(services);
            }

            // Configure the HTTP request pipeline.

            app.UseSwagger();
                app.UseSwaggerUI();


            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<ChatHub>("/chatHub");

            app.Run();
        }
    }
}
