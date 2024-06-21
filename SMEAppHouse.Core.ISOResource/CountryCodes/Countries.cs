using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SMEAppHouse.Core.ISOResource.CountryCodes
{
    public sealed class Countries : List<Country>
    {
        private static Countries instance = null;
        private static readonly object padlock = new object();

        private Countries()
        {
        }

        public static Countries Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Countries();

                        var assembly = Assembly.GetExecutingAssembly();
                        var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("countries.json"));
                        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var jsonContent = reader.ReadToEnd(); //Make string equal to full file
                            var countries = Country.FromJson(jsonContent).ToList();

                            if (countries != null && countries.Any())
                            {
                                countries.ForEach(c =>
                                {
                                    instance.Add(c);
                                });
                            }
                        }



                    }
                    return instance;
                }
            }
        }
    }
}