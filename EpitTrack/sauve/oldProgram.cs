using EpitTrack.Data;
using EpitTrack.Services;
using Microsoft.EntityFrameworkCore;
using static EpitTrack.Controllers.PlanningController;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Ajoutez les services de session ici
builder.Services.AddSession(options =>
{
    // Définir le délai d'expiration de la session
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Important pour le GDPR
});

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddSignalR();

// Configuration de la connexion PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("EpiDataConnect");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddHttpClient<IGeocodageService, GeocodageService>();
builder.Services.AddHttpClient<ICalculTempsTrajetService, CalculTempsTrajetService>();

builder.Services.AddSwaggerGen(c => c.UseDateOnlyTimeOnlyStringConverters());

//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2VlhhQlJCfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5adENiW31bc31dR2he");
builder.Services.AddSyncfusionBlazor();

var app = builder.Build();

// Utilisez le middleware de session
app.UseSession();


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

app.UseAuthorization();
//app.MapHub<ProgressHub>("/progressHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();



