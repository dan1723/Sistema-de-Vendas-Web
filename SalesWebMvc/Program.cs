using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Internal;
using SalesWebMvc.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("SalesWebMvcContext");

builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 26)), MySqlOptions => MySqlOptions.MigrationsAssembly("SalesWebMvc")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SalesRecordService>();




var app = builder.Build();

// Obter o servi�o de seeding e chamar o m�todo de seeding
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var seedingService = services.GetRequiredService<SeedingService>();
    seedingService.See();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    var enUS = new CultureInfo("en-US");
    var localizationOptions = new RequestLocalizationOptions {
        DefaultRequestCulture = new RequestCulture(enUS),
        SupportedCultures = new List<CultureInfo> { enUS },
        SupportedUICultures = new List<CultureInfo> { enUS }
    };

    app.UseRequestLocalization(localizationOptions);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
