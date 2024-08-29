using Application.Commands.Courier;
using Application.Commands.Motorcycle;
using Application.Commands.Plan;
using Application.Commands.RentOut;
using Application.Handlers.Courier;
using Application.Handlers.Motorcycle;
using Application.Handlers.Plan;
using Application.Handlers.RentOut;
using Domain.Messaging;
using Domain.Repositories;
using Domain.Services.Auth;
using Domain.Services.Contracts;
using Domain.Services.Courier;
using Domain.Services.Motorcycle;
using Domain.Services.Order;
using Domain.Services.Plan;
using Domain.Services.RentOut;
using Infrastructure.Context;
using Infrastructure.Messaging.RabbitMQ;
using Infrastructure.Repositories;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Handlers;
using System.Text;

namespace RentApi.Infrastructure
{
    public static class DependencyInjectService
    {
        public static void AddDependencyInject(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
            }, ServiceLifetime.Scoped);

            services.RegisterAuthentication(configuration);
            services.RegisterRepository();
            services.RegisterService();
            services.RegisterHandle();
            services.RegisterBus();

            services.AddHostedService<RabbitMQConsumerHostedService>();
        }

        private static void RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"].ToString()))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Courier", policy => policy.RequireRole("Courier"));
            });
        }

        private static void RegisterRepository(this IServiceCollection services)
        {
            services.AddScoped<ICourierRespository, CourierRespository>();
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IRentReposotory, RentReposotory>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IMotorcycleNotificationRepository, MotorcycleNotificationRepository>();

            services.AddSingleton<IStorageRepository, SupabaseStorage>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var url = configuration["SupabaseStorage:Url"] ?? "";
                var bucketName = configuration["SupabaseStorage:BucketName"] ?? "";
                var accesskeyID = configuration["SupabaseStorage:AccesskeyID"] ?? "";

                var logger = provider.GetRequiredService<ILogger<SupabaseStorage>>();

                return new SupabaseStorage(url, accesskeyID, bucketName, logger);
            });
        }

        private static void RegisterService(this IServiceCollection services)
        {
            services.AddScoped<IFindPlanService, FindPlanService>();

            // Motorcycle
            services.AddScoped<ICreateMotorcycleService, CreateMotorcycleService>();
            services.AddScoped<IUpdateMotorcycleService, UpdateMotorcycleService>();
            services.AddScoped<IFindMotorcycleService, FindMotorcycleService>();
            services.AddScoped<IDeleteMotorcycleService, DeleteMotorcycleService>();

            // Courier
            services.AddScoped<ICreateCourierService, CreateCourierService>();
            services.AddScoped<IUpdateDrivingLicenseService, UpdateDrivingLicenseService>();

            // Rent
            services.AddScoped<ICreateRentOutService, CreateRentOutService>();
            services.AddScoped<ISimulateRentOutService, SimulateRentOutService>();
            services.AddScoped<ICompleteRentService, CompleteRentService>();

            // Auth
            services.AddScoped<IAuthService, AuthService>();

            // Order
            services.AddScoped<IFindOrdersAvailableForDeliveryService, FindOrdersAvailableForDeliveryService>();
            services.AddScoped<IOrderDeliveryAssignmentService, OrderDeliveryAssignmentService>();
            services.AddScoped<ICompleteOrderDeliveryService, CompleteOrderDeliveryService>();
            services.AddScoped<IFindOrdersByCourierService, FindOrdersByCourierService>(); 
        }

        private static void RegisterHandle(this IServiceCollection services)
        {
            services.AddScoped<IHandler<FindAllPlansCommand>, FindPlanHandle>();

            // Motorcycle
            services.AddScoped<IHandler<FindAllMotorcyclesCommand>, FindMotorcycleHandle>();
            services.AddScoped<IHandler<FindMotorcycleByLicensePlateCommand>, FindMotorcycleHandle>();
            services.AddScoped<IHandler<FindAllAvailableMotorcyclesCommand>, FindMotorcycleHandle>();
            services.AddScoped<IHandler<CreateMotorcycleCommand>, CreateMotorcycleHandle>();
            services.AddScoped<IHandler<UpdateMotorcycleCommand>, UpdateMotorcycleHandle>();
            services.AddScoped<IHandler<DeleteMotorcycleCommand>, DeleteMotorcycleHandle>();

            // Courier
            services.AddScoped<IHandler<CreateCourierCommand>, CreateCourierHandle>();
            services.AddScoped<IHandler<UploadDrivingLicenseCommand>, UploadDrivingLicenseHandle>();

            // Rent
            services.AddScoped<IHandler<CreateRentOutCommand>, CreateRentOutHandle>();
            services.AddScoped<IHandler<SimulateRentOutCommand>, SimulateRentOutHandle>();
            services.AddScoped<IHandler<CompleteRentCommand>, CompleteRentHandle>();
        }

        private static void RegisterBus(this IServiceCollection services)
        {
            // Message Bus
            services.AddScoped<IMessageBus, RabbitMQEventPublisher>();
            services.AddSingleton<RabbitMQEventConsumer>();
        }
    }
}
