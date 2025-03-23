using BarbellaInventory.Configurations;
using BarbellaInventory.Models;
using BarbellaInventory.Repositories;
using BarbellaInventory.Services;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Try to read MongoDB connection string from environment variable
var connectionString = Environment.GetEnvironmentVariable("COSMOSDB_CONNECTIONSTRING");

// If environment variable is not found, fallback to appsettings.json
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("⚠️ Environment variable not found. Checking appsettings.json.");
    var config = builder.Configuration;
    connectionString = config.GetSection("MongoDb:ConnectionString").Value;
}

// If no connection string found, use in-memory repository fallback
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("⚠️ No MongoDB connection string found. Using In-Memory fallback repository.");
    builder.Services.AddSingleton<IBarbellaRepository, InMemoryRepository>();
}
else
{
    Console.WriteLine("✅ Using MongoDB connection string.");
    builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
    builder.Services.AddScoped<IBarbellaRepository, MongoDbRepository>();
}
// Register your service
builder.Services.AddScoped<IBarbellaService, BarbellaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
