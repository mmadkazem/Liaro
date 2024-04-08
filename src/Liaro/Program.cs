var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
{
    services.RegisterApiServices(builder.Configuration);
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

