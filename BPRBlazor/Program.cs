using BPR.Mediator.Interfaces;
using BPR.Mediator.Services.Messaging;
using BPRBlazor;
using BPRBlazor.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddValidators();
builder.Services.AddServices();
builder.Services.AddDbConfiguration(builder.Configuration);

// Database config
builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    WriteIndented = true
});

builder.Services.AddScoped<ISender, RabbitMqSender>();
builder.Services.AddSingleton<RabbitMqConsumer>();
builder.Services.AddHostedService<RabbitMqBackgroundService>();

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

Extensions.CleanUp();

app.Run();
