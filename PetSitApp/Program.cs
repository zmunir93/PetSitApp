using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using PetSitApp.AutoMapperProfiles;
using PetSitApp.DTOs.SitterSearchDTO;
using PetSitApp.Models;

var builder = WebApplication.CreateBuilder(args);

//Console.WriteLine($"Connection string: {connectionString}");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PetSitApp.Models.PetSitAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString)
//    );
//builder.Services.AddAuthentication().AddFacebook(options =>
//{
//    options.AppId = Environment.GetEnvironmentVariable("AppId");
//    options.AppSecret = Environment.GetEnvironmentVariable("AppSecret");
//});
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//Builder to add Authorization with JWT
//builder.Services.AddAuthentication(JwtBearerCookieDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

//        };
//    });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
    });



var apiKey = builder.Configuration["GoogleGeocodingAPI:ApiKey"];


builder.Services.AddHttpClient();



builder.Services.AddAutoMapper(typeof(SitterProfile));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

builder.Logging.AddConsole();  // Ensure this is present.


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();
app.Run();
