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
builder.Services.AddEntityRepositories();

// 3rd-party dependencies setup
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();