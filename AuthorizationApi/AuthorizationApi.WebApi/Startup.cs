using AuthorizationApi.Application.Interfaces;
using AuthorizationApi.Application.Services;
using AuthorizationApi.BusinessLogic.EventHandlers;
using AuthorizationApi.Domain.Constants;
using AuthorizationApi.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AuthorizationApi.WebApi;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        var connectionString = _configuration[AppSettingsConstants.AuthorizationServiceDatabaseConnectionString];
        
        serviceCollection.AddDbContext<DatabaseContext>(options =>
        {
        options.UseNpgsql(connectionString);
        });
        
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();

        serviceCollection.AddScoped<IJwtTokenService, JwtTokenService>();
        serviceCollection.AddScoped<IHashService, HashService>();
        
        serviceCollection.AddMassTransit(config =>
        {
            config.AddConsumer<RegistrationEventHandler>();
            config.AddConsumer<LoginEventHandler>();
            config.AddConsumer<ValidateAccessTokenEventHandler>();
            config.AddConsumer<DeleteUserEventHandler>();
            
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                
                cfg.ReceiveEndpoint("message-queue", c =>
                {
                    c.ConfigureConsumer<RegistrationEventHandler>(ctx);
                    c.ConfigureConsumer<LoginEventHandler>(ctx);
                    c.ConfigureConsumer<ValidateAccessTokenEventHandler>(ctx);
                    c.ConfigureConsumer<DeleteUserEventHandler>(ctx);
                });
                
                cfg.ConfigureJsonSerializer(options =>
                {
                    options.DefaultValueHandling = DefaultValueHandling.Include;

                    return options;
                });
            });
        });

        serviceCollection.AddMassTransitHostedService();
    }
    
    public void Configure(IApplicationBuilder applicationBuilder, IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment.IsDevelopment())
        {
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI();
        }

        applicationBuilder.UseHttpsRedirection();

        applicationBuilder.UseRouting();
        
        applicationBuilder.UseEndpoints(builder =>
        {
            builder.MapControllers();
        });
    }
}