using Greenhouse.Bluetooth;
using Greenhouse.Core.Setup;
using Greenhouse.Core.Setup.Abstractions;
using Greenhouse.Storage.Configuration;
using Greenhouse.Mqtt;
using Greenhouse.UI.Components;
using Greenhouse.UI.Infrastructure;
using Microsoft.AspNetCore.DataProtection;

namespace Greenhouse.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddSingleton<IMainConfigRepository>(_ =>
                new JsonMainConfigRepository(
                    Path.Combine(builder.Environment.ContentRootPath, "App_Data", "main-config.json")));
            builder.Services.AddSingleton<INetworkService, DevelopmentNetworkService>();
            builder.Services.AddScoped<SetupApplicationService>();
            builder.Services.AddBluetoothDiscovery();
            builder.Services.AddMessaging(builder.Configuration);
            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(
                    new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtectionKeys")));

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
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
