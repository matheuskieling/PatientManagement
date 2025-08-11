using DotNetEnv;
using FicharioDigital.Infrastructure;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Env.Load();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerDocumentation();
builder.Services.AddDependencies(builder.Configuration);
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.AllowAnyOrigin() // Replace with your allowed origin(s)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.WebHost.UseUrls("https://0.0.0.0:5000");

var app = builder.Build();


app.Migrate();
app.UseCors("AllowSpecificOrigins");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().WithMetadata(new RouteAttribute("api/[controller]"));

app.Run();