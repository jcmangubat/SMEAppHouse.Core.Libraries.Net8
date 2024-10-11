using Microsoft.Extensions.Configuration;

namespace SMEAppHouse.Core.EnvCfgLoader;

public class EnvFileConfigurationSource(string filePath) : IConfigurationSource
{
    private readonly string _filePath = filePath;

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new EnvFileConfigurationProvider(_filePath);
    }
}
