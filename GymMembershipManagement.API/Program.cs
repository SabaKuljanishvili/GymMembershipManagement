using GymMembershipManagement.DAL.Repositories;
using GymMembershipManagement.DATA;
using GymMembershipManagement.SERVICE;
using GymMembershipManagement.SERVICE.Interfaces;
using GymMembershipManagement.SERVICE.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GymMembershipManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DB connection
            builder.Services.AddDbContext<GymDbContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("GymDbConnection"));
            });

            builder.Services.AddScoped<DbContext, GymDbContext>();

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), typeof(MappingProfile).Assembly);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Gym API", Version = "v1" });
            });

            // Repositories
            builder.Services.AddScoped<IGymClassRepository, GymClassRepository>();
            builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();
            builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();

            // Services
            builder.Services.AddScoped<IGymClassService, GymClassService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();

            var app = builder.Build();

            // Swagger კონფიგურაცია - მოვაცილეთ IsDevelopment შემოწმება, რომ ონლაინაც გამოჩნდეს
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gym API V1");
                // RoutePrefix დავტოვოთ "swagger", რომ ორივეგან ერთ მისამართზე იყოს
                c.RoutePrefix = "swagger";
            });

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // დავამატოთ გადამისამართება მთავარი გვერდიდან Swagger-ზე (სურვილისამებრ)
            app.MapGet("/", context => {
                context.Response.Redirect("/swagger/index.html");
                return Task.CompletedTask;
            });

            app.Run();
        }
    }
}