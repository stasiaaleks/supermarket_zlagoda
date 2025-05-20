using Microsoft.AspNetCore.Authentication.Cookies;
using ShopApp.DAL.Extensions;
using ShopApp.Data.QueriesAccess;
using ShopApp.Services;
using ShopApp.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DevDBConnection") 
                          ?? throw new InvalidOperationException("Connection string 'DevDBConnection' not found.");


// minimal current cookie-based auth model
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => { options.LoginPath = "/login"; });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSingleton(connectionString);
builder.Services.AddDataServices();
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// entities services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// auth services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();


// sql queries providers
// TODO: consider refactoring as a factory instead of a concrete implementation
builder.Services.AddScoped<ProductQueryProvider>();
builder.Services.AddScoped<EmployeeQueryProvider>();
builder.Services.AddScoped<CategoryQueryProvider>();
builder.Services.AddScoped<UserQueryProvider>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();
app.UseAuthentication();
app.UseAuthorization();