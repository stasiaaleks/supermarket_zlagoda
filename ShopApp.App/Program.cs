using ShopApp.Data;
using ShopApp.Data.Repositories;
using ShopApp.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DevDBConnection") 
                          ?? throw new InvalidOperationException("Connection string 'DevDBConnection' not found.");

builder.Services.AddControllers();
builder.Services.AddSingleton(connectionString);
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddDataServices();

builder.Services.AddSingleton(typeof(SqlQueryRegistry));

var serviceProvider = builder.Services.BuildServiceProvider();
var registry = serviceProvider.GetRequiredService<SqlQueryRegistry>();
registry.FindAndMapAllQueries();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();