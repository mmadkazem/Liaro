var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var services = builder.Services;
{
    services.RegisterApiServices(configuration)
            .RegisterInfrastructureServices(configuration);
}
var app = builder.Build();

{// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}

