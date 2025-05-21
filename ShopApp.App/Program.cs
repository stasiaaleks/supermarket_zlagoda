using Microsoft.AspNetCore.Authentication.Cookies;
using ShopApp.DAL.Extensions;
using ShopApp.Extensions;

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
builder.Services.AddBusinessLogicServices();
builder.Services.AddQueryProviders();

//  
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.Run();
app.UseAuthentication();
app.UseAuthorization();