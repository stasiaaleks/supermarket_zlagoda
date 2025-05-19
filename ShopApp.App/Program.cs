using ShopApp.DAL.Extensions;
using ShopApp.Data.QueriesAccess;
using ShopApp.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DevDBConnection") 
                          ?? throw new InvalidOperationException("Connection string 'DevDBConnection' not found.");

builder.Services.AddControllers();
builder.Services.AddSingleton(connectionString);
builder.Services.AddDataServices();

// entities services
builder.Services.AddScoped<IProductService, ProductService>();

// sql queries providers
// TODO: consider refactoring as a factory instead of a concrete implementation
builder.Services.AddScoped<ProductQueryProvider>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();