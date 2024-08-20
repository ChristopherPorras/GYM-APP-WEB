using BL;
using DataAccess.CRUD;
using DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore; // Aseg�rate de tener este using
using System.Net.Http.Headers;
using System.Net;
using WebPWrecover.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddFile("Logs/app-{Date}.log");

// Verificar la URL base
var configuration = builder.Configuration;
var mvcAppBaseUrl = configuration["MvcAppBaseUrl"];
Console.WriteLine($"MVC App Base URL: {mvcAppBaseUrl}");

// Agregar la configuraci�n de ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Configure authentication and authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.None; // Necesario si est�s enviando cookies entre sitios
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.LoginPath = "/Home/LogPage";
        options.AccessDeniedPath = "/User/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("RequireRecepcionistaRole", policy => policy.RequireRole("Recepcionista"));
    options.AddPolicy("RequireEntrenadorRole", policy => policy.RequireRole("Entrenador"));
});

// Add HttpClient service
builder.Services.AddHttpClient("MyAPIClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7280/"); // URL del API
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new CookieContainer()
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {

        builder.WithOrigins("https://localhost:7280") // URL del API
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // Permitir el env�o de cookies/credenciales

    });
});

// Register IEmailSender and dependencies
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

// Register UserManager and dependencies
builder.Services.AddSingleton<UserCrudFactory>();
builder.Services.AddTransient<UserManager>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Enable CORS
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
