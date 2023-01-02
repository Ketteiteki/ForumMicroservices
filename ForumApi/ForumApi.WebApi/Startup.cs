using Core.Events;
using ForumApi.BusinessLogic.ApiCommands.Users;
using ForumApi.Domain.Constants;
using ForumApi.Persistence;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ForumApi.WebApi;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        var databaseConnectionString = _configuration[AppSettingsConstants.ForumDatabaseConnectionString];
        
        serviceCollection.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString);
        });
        
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();
        
        serviceCollection.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "Messenger Api"
            });
			
            options.AddSecurityDefinition(
                "token",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization
                }
            );
			
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "token"
                            },
                        },
                        Array.Empty<string>()
                    }
                }
            );
        });

        serviceCollection.AddMediatR(typeof(RegistrationCommand).Assembly);

        serviceCollection.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                
                cfg.ConfigureJsonSerializer(options =>
                {
                    options.DefaultValueHandling = DefaultValueHandling.Include;

                    return options;
                });
            });
            
            config.AddRequestClient<RegistrationEvent>();
            config.AddRequestClient<LoginEvent>();
            config.AddRequestClient<ValidateAccessTokenEvent>();
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