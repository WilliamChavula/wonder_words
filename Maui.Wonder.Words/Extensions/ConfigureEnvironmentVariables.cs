using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Maui.Wonder.Words.Extensions;

public static class ConfigureEnvironmentVariables
{
  public static MauiAppBuilder AddEnvironmentVariables(this MauiAppBuilder appBuilder)
  {
    var a = Assembly.GetExecutingAssembly();
    using var stream = a.GetManifestResourceStream("Maui.Wonder.Words.appsettings.json");

    ArgumentNullException.ThrowIfNull(stream, "File 'Maui.Wonder.Words.appsettings.json' not found in Assembly");

    var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();


    appBuilder.Configuration.AddConfiguration(config);

    return appBuilder;
  }
}