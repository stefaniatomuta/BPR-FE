using System.Text.Json;
using BPRBlazor.Services;
using BPRBE.Services;
using BPRBE.Persistence;
using BPRBE;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
// Services
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IDependencyRepository, DependencyRepository>();
builder.Services.AddScoped<IDependencyComponentService, DependencyComponentService>();
builder.Services.AddScoped<ICodebaseService, CodebaseService>();
builder.Services.AddValidators();
builder.Services.AddDbConfiguration(builder.Configuration);
// Database config

builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    WriteIndented = true
});

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
