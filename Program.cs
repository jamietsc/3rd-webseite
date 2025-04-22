using _3rd_webseite.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using ThirdWebseite.DataAccessLayer.Data;
using ThirdWebseite.DataAccessLayer.Repositories;
using ThirdWebseite.DataAccessLayer.SqlDataAccess;
using ThirdWebseite.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ThirdWebseite.Controllers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var secret = builder.Configuration.GetValue<string>("Jwt:Secret");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourdomain.com",
            ValidAudience = "yourdomain.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IUserData<RegisterViewModel>, UserData>();


builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
