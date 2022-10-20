using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
              c.BaseAddress = new Uri(configuration.GetValue<string>("ApiSettings:CatalogUrl")));

builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
    c.BaseAddress = new Uri(configuration.GetValue<string>("ApiSettings:BasketUrl")));

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
    c.BaseAddress = new Uri(configuration.GetValue<string>("ApiSettings:OrderingUrl")));

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
});


builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri($"{configuration.GetValue<string>("ApiSettings:CatalogUrl")}/swagger/index.html"), "Catalog.API", HealthStatus.Degraded)
    .AddUrlGroup(new Uri($"{configuration.GetValue<string>("ApiSettings:BasketUrl")}/swagger/index.html"), "Basket.API", HealthStatus.Degraded)
    .AddUrlGroup(new Uri($"{configuration.GetValue<string>("ApiSettings:OrderingUrl")}/swagger/index.html"), "Ordering.API", HealthStatus.Degraded);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
}

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
});

app.Run();