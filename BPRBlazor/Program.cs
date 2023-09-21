using BPRBE.Config;
using BPRBE.Models.Persistence;
using BPRBE.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
// Services
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IMongoDependencyRepository, MongoDependencyRepository>();
// Validators
builder.Services.AddScoped<IValidator<ArchitecturalModel>, ArchitecturalModelValidator>();
builder.Services.AddScoped<IValidator<ArchitecturalComponent>, ArchitecturalComponentValidator>();
// Database config
builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection(DatabaseConfig.Section));
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
