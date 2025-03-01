using Blazored.Toast;
using GestionPrestamos.Components;
using GestionPrestamos.Context;
using GestionPrestamos.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var culture = new CultureInfo("es-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Inyeccion del contexto
var ConStr = builder.Configuration.GetConnectionString("SqlConStr");
//builder.Services.AddDbContext<Contexto>(o => o.UseSqlServer(ConStr));
builder.Services.AddDbContextFactory<Contexto>(o => o.UseSqlServer(ConStr));

//Inyeccion del service
builder.Services.AddScoped<PrestamosService>();
builder.Services.AddScoped<DeudoresService>();
builder.Services.AddScoped<CobrosService>();

// Inyeccion del servicio de Bootstrap
builder.Services.AddBlazorBootstrap();
builder.Services.AddBlazoredToast();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
