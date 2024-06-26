using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.Controllers;
using SistemaGestionRiesgos.Models;
using SistemaGestionRiesgos.Services;
using SistemaGestionRiesgos.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add dbContext 

builder.Services.AddDbContext<GestionDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBCONNECTION")));

builder.Services.AddScoped<IBitacoraService, BitacoraService>();
builder.Services.AddScoped<IPlanesService, PlanesService>();
builder.Services.AddScoped<IRiesgosService, RiesgosService>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();



builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
});



builder.Services.AddAuthentication("CookieAuthentication").AddCookie("CookieAuthentication",
    config =>
    {
        config.Cookie.Name = "UserLoginCookie";
        config.LoginPath = "/Usuarios/Login";
    });

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

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();