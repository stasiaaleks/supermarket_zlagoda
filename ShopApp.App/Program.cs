using ShopApp.DAL.Extensions;
using ShopApp.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DevDBConnection") 
                          ?? throw new InvalidOperationException("Connection string 'DevDBConnection' not found.");

builder.Services.AddControllers();
builder.Services.AddSingleton(connectionString);
builder.Services.AddDataServices();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();