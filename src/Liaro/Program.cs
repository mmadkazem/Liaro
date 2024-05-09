var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var services = builder.Services;
{
           services.RegisterApplicationServices()
            .RegisterInfrastructureServices(configuration)
            .RegisterSharedServices()
            .RegisterApiServices(configuration);
}
var app = builder.Build();

{// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    var dbInitializer = app.Services.GetService<IDbInitializerService>();
    dbInitializer.Initialize();
    dbInitializer.SeedData();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseShared();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

