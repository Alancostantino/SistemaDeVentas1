using SistemaDeVentas1.Components;
using SistemaDeVentas1.Interfaces;
using SistemaDeVentas1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient();

builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7299/"); // Dirección de tu API REST
});

builder.Services.AddHttpClient<IVentaRepositorio, VentaRepositorio>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7299/"); // Dirección de tu API REST
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IVentaRepositorio, VentaRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
