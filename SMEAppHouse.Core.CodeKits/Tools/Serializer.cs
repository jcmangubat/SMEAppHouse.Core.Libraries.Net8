using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace SMEAppHouse.Core.CodeKits.Tools
{
    public enum SerializationFormatterEnum
    {
        Binary,
        Xml,
        Json
    }

    public static class Serializer
    {
        /// <summary>
        /// 
        /// </summary>
        #region Serialization methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeToXml<T>(T value)
        {
            try
            {
                //var serializer = new XmlSerializer(obj.GetType());
                //using (var writer = new StringWriter())
                //{
                //    serializer.Serialize(writer, obj);
                //    return writer.ToString();
                //}

                if (value == null)
                    return null;

                var rootAttr = new XmlRootAttribute()
                {
                    Namespace = typeof(T).Namespace,
                    ElementName = typeof(T).Name,
                    IsNullable = true
                };

                var serializer = new XmlSerializer(typeof(T), rootAttr);
                var settings = new XmlWriterSettings
                {
                    Encoding = new UnicodeEncoding(false, false),
                    Indent = false,
                    OmitXmlDeclaration = false
                };

                using var textWriter = new StringWriter();
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                return textWriter.ToString();

            }
            catch (SerializationException sX)
            {
                var errMsg = $"Unable to serialize {value}";
                throw new Exception(errMsg, sX);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string xml)
        {
            try
            {
                if (string.IsNullOrEmpty(xml))
                    return default(T);

                var rootAttr = new XmlRootAttribute()
                {
                    Namespace = typeof(T).Namespace,
                    ElementName = typeof(T).Name,
                    IsNullable = true
                };

                var serializer = new XmlSerializer(typeof(T), rootAttr);

                var settings = new XmlReaderSettings();

                // No settings need modifying here
                using var textReader = new StringReader(xml);
                using var xmlReader = XmlReader.Create(textReader, settings);
                return (T)serializer.Deserialize(xmlReader);

                //var serializer = new XmlSerializer(typeof(T));
                //using (var reader = new StringReader(xml))
                //{
                //    return (T)serializer.Deserialize(reader);
                //}
            }
            catch (SerializationException sX)
            {
                var errMsg = $"Unable to deserialize XML string into {typeof(T)}";
                throw new Exception(errMsg, sX);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToJson<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serData"></param>
        /// <returns></returns>
        public static T DeserializeFromJson<T>(string jsonData)
        {
            return JsonSerializer.Deserialize<T>(jsonData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="pathSpec"></param>
        /// <param name="serializationFormatterEnum"></param>
        public static void SerializeToFile<T>(T obj, string pathSpec, SerializationFormatterEnum serializationFormatterEnum)
        {
            try
            {
                switch (serializationFormatterEnum)
                {
                    case SerializationFormatterEnum.Binary:
                        // Binary serialization is deprecated, consider using other formats like JSON
                        throw new NotSupportedException("Binary serialization is deprecated. Consider using JSON serialization.");

                    case SerializationFormatterEnum.Xml:
                        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                        using (var textWriter = new StreamWriter(pathSpec))
                        {
                            serializer.Serialize(textWriter, obj);
                        }
                        break;

                    case SerializationFormatterEnum.Json:
                        var options = new JsonSerializerOptions
                        {
                            WriteIndented = true // Optional: Makes the output human-readable
                        };
                        var jsonString = JsonSerializer.Serialize(obj, options);
                        File.WriteAllText(pathSpec, jsonString);
                        break;

                    default:
                        throw new ArgumentException("Invalid Formatter option", nameof(serializationFormatterEnum));
                }
            }
            catch (Exception ex)
            {
                var errMsg = $"Unable to serialize {obj} into file {pathSpec}. Detail: {ex.Message}";
                throw new Exception(errMsg, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pathSpec"></param>
        /// <param name="serializationFormatterEnum"></param>
        /// <returns></returns>
        public static T DeserializeFromFile<T>(string pathSpec, SerializationFormatterEnum serializationFormatterEnum) where T : class
        {
            try
            {
                switch (serializationFormatterEnum)
                {
                    case SerializationFormatterEnum.Binary:
                        throw new InvalidOperationException("Binary serialization is no longer recommended.");

                    case SerializationFormatterEnum.Xml:
                        using (var strm = new FileStream(pathSpec, FileMode.Open, FileAccess.Read))
                        {
                            var serializer = new XmlSerializer(typeof(T));
                            var obj = (T)serializer.Deserialize(strm);
                            return obj;
                        }

                    case SerializationFormatterEnum.Json:
                        string json = File.ReadAllText(pathSpec);
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        return JsonSerializer.Deserialize<T>(json, options);

                    default:
                        throw new ArgumentException("Invalid Formatter option");
                }
            }
            catch (IOException ex)
            {
                throw new Exception($"Error reading file {pathSpec}", ex);
            }
            catch (SerializationException sX)
            {
                var errMsg = $"Unable to deserialize {typeof(T)} from file {pathSpec}";
                throw new Exception(errMsg, sX);
            }
        }

        #endregion Serialization methods
    }

}
