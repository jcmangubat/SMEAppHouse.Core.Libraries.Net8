using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SMEAppHouse.Core.CodeKits.Tools;

namespace SMEAppHouse.Core.ScraperBox.Models
{
    [Serializable()]
    public class HtmlSource
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("urlpattern")]
        public string UrlPattern { get; set; }

        [XmlAttribute("pagerangemin")]
        public int PageNoMin { get; set; }

        [XmlAttribute("pagerangemax")]
        public int PageNoMax { get; set; }

        [XmlAttribute("ignore")]
        public bool Ignore { get; set; }

        [XmlAttribute("contentsize")]
        public int ContentSize { get; set; }

        [XmlIgnore]
        public string UrlPatternDecoded => HttpUtility.UrlDecode(this.UrlPattern);


        public class Sources : List<HtmlSource>
        {
            public Sources()
            {
            }

            public Sources(string sourceFile)
            {
                try
                {
                    var sources = Serializer.DeserializeFromFile<List<HtmlSource>>(sourceFile, SerializationFormatterEnum.Xml);
                    if (sources != null && sources.Any())
                    {
                        sources.ForEach(Add);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            public void Save(string xmlFilename)
            {
                try
                {
                    Serializer.SerializeToFile(this, xmlFilename, SerializationFormatterEnum.Xml);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }
    }
}

