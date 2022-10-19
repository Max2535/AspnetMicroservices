using AspnetRunBasics.Data;
using AspnetRunBasics.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
// Add services to the container.
builder.Services.AddRazorPages();
#region database services
// add database dependecy
builder.Services.AddDbContext<AspnetRunContext>(c =>
    c.UseSqlServer(configuration.GetValue<string>("ConnectionStrings:AspnetRunConnection")));
#endregion


#region project services

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
    c.BaseAddress = new Uri(configuration.GetValue<string>("ApiSettings:GatewayAddress")));
builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
    c.BaseAddress = new Uri(configuration.GetValue<string>("ApiSettings:GatewayAddress")));
builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
    c.BaseAddress = new Uri(configuration.GetValue<string>("ApiSettings:GatewayAddress")));
#endregion



var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

//    try
//    {
//        var aspnetRunContext = services.GetRequiredService<AspnetRunContext>();
//        AspnetRunContextSeed.SeedAsync(aspnetRunContext, loggerFactory).Wait();
//    }
//    catch (Exception exception)
//    {
//        var logger = loggerFactory.CreateLogger<Program>();
//        logger.LogError(exception, "An error occurred seeding the DB.");
//    }
//}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
