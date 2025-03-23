using BarbellaInventory.Configurations;
using BarbellaInventory.Models;
using BarbellaInventory.Repositories;
using BarbellaInventory.Services;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

DotNetEnv.Env.Load();

// Try to read MongoDB connection string from environment variable
var connectionString = Environment.GetEnvironmentVariable("COSMOSDB_CONNECTIONSTRING");

/// If connection string is missing from environment variables, fallback to appsettings.Development.json
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("⚠️ No Cosmos DB connection string found in environment. Checking appsettings.json or appsettings.Development.json.");
    connectionString = builder.Configuration["MongoDb:ConnectionString"];
}

// If connection string is still missing, fall back to using an in-memory repository (useful for testing or development)
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("⚠️ No MongoDB connection string found. Using In-Memory repository as fallback.");
    builder.Services.AddSingleton<IBarbellaRepository, InMemoryRepository>();
}
else
{
    // Use MongoDB client if connection string is found
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
