namespace Liaro.Common;
public static class ApiExtension
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthenticationConfig(configuration)
            .AddSwaggerConfig()
            .AddCommonConfig()
            .AddEndpointsApiExplorer()
            .AddControllers();

        return services;
    }
    private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Liaro", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                Array.Empty<string>()
                            }
            });
        });

        return services;
    }

    private static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(CustomRoles.Admin, policy => policy.RequireRole(CustomRoles.Admin));
            options.AddPolicy(CustomRoles.User, policy => policy.RequireRole(CustomRoles.User));
            options.AddPolicy(CustomRoles.Editor, policy => policy.RequireRole(CustomRoles.Editor));
        });
        // Needed for jwt auth.
        services
            .AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["BearerTokens:Issuer"], // site that makes the token
                    ValidateIssuer = false, // TODO: change this to avoid forwarding attacks
                    ValidAudience = configuration["BearerTokens:Audience"], // site that consumes the token
                    ValidateAudience = false, // TODO: change this to avoid forwarding attacks
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["BearerTokens:Key"])),
                    ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                    ValidateLifetime = true, // validate the expiration
                    ClockSkew = TimeSpan.Zero // tolerance for the expiration date
                };
                cfg.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        logger.LogError("Authentication failed. Exception:{}", context.Exception);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatorService>();
                        return tokenValidatorService.ValidateAsync(context);
                    },
                    OnMessageReceived = context =>
                    {
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        logger.LogError("OnChallenge error Exception:{}, Description:{}", context.Error, context.ErrorDescription);
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });

        return services;
    }

    private static IServiceCollection AddCommonConfig(this IServiceCollection services)
    {

        services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));

        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
        return services;
    }

}