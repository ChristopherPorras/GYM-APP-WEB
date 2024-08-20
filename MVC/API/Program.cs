using DTO;
using BL;
using DataAccess.CRUD;
using Microsoft.AspNetCore.Identity.UI.Services;
using WebPWrecover.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("AuthMessageSenderOptions"));

// Register UserCrudFactory and UserManager
builder.Services.AddSingleton<UserCrudFactory>();
builder.Services.AddScoped<UserManager>();
// Registro de RutinaManager y RutinaCrudFactory
builder.Services.AddScoped<RutinaManager>();
builder.Services.AddSingleton<RutinaCrudFactory>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SchemaGeneratorOptions = new Swashbuckle.AspNetCore.SwaggerGen.SchemaGeneratorOptions
    {
        SchemaIdSelector = type => type.FullName
    };
});

// Add session support (required for session state)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Configure authentication and authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.None; // Necesario si estás enviando cookies entre sitios
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

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configure CORS policy with specific origin and credentials
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.WithOrigins("https://localhost:7078")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials(); // Permite credenciales
        });
});

// Registro de CitaManager
builder.Services.AddScoped<CitaManager>();

// Agrega la configuración para el ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Habilita la página de excepción de desarrollador
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ** Enable CORS after UseRouting and before UseAuthentication, UseAuthorization **
app.UseCors("CorsPolicy");

// Enable session before using it
app.UseSession();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
