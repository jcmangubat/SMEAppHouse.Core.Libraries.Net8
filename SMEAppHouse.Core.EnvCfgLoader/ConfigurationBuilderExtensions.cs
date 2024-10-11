using Microsoft.Extensions.Configuration;

namespace SMEAppHouse.Core.EnvCfgLoader;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddEnvFile(this IConfigurationBuilder builder, string filePath = ".env")
    {
        return builder.Add(new EnvFileConfigurationSource(filePath));
    }
}