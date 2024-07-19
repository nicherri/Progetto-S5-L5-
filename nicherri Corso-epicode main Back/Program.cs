using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Aggiungi i servizi al contenitore
builder.Services.AddRazorPages();
builder.Services.AddSingleton<SqlConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

var app = builder.Build();

// Configura la pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "trasgressori",
    pattern: "{controller=Trasgressori}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "violazioni",
    pattern: "{controller=Violazioni}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "verbali",
    pattern: "{controller=Verbali}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "reports",
    pattern: "{controller=Reports}/{action=TotaleVerbaliPerTrasgressore}/{id?}");

app.Run();
