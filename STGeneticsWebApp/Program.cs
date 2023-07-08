using DevExpress.Blazor;
using DevExpress.Blazor.Configuration;
using Microsoft.EntityFrameworkCore;
using STGeneticsWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<STGeneticsDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("STGeneticsDBConnectionString")));
builder.Services.AddScoped<STGeneticsService>();

builder.Services.Configure<GlobalOptions>(builder.Configuration.GetSection("DevExpress"));

builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
