using Microsoft.Extensions.Configuration;

namespace SMEAppHouse.Core.EnvCfgLoader;

public class EnvFileConfigurationProvider(string filePath, bool optional = false) : ConfigurationProvider
{
    private readonly string _filePath = filePath;
    private readonly bool _optional = optional;

    public override void Load()
    {
        if (!File.Exists(_filePath))
            return;

        /*var data = new Dictionary<string, string?>();

        foreach (var line in File.ReadAllLines(_filePath))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                continue;

            var parts = line.Split('=', 2);
            if (parts.Length == 2)
                data[parts[0].Trim()] = parts[1].Trim();
        }

        Data = data;*/


        try
        {

            foreach (var ln in File.ReadAllLines(_filePath))
            {
                var line = ln.Trim();

                // Filter comments
                if (line.StartsWith('#')) continue;

                // Filter empty
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Split at first equality, treat the rest as the value
                var eqIx = line.IndexOf('=');

                // If no equality, skip
                if (eqIx < 0) continue;         

                var key = line[..eqIx];         // Key without the equals
                var value = line[(eqIx + 1)..]; // Rest, omitting the equals

                // Empty keys not allowed
                if (string.IsNullOrWhiteSpace(key)) continue;

                // But empty values are
                if (value == null) continue;

                // By convention, environment variables use double
                // underscore as section separators. Replace it with the internally-used colon.
                key = key.Replace("__", ":");

                base.Set(key, value);
            }
        }
        catch (FileNotFoundException)
        {
            if (_optional) return;
            throw;
        }
    }
}
