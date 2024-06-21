using System;
using System.Collections.Generic;
using System.Text;

namespace SMEAppHouse.Core.ISOResource.Helpers
{
    /// <summary>
    /// https://www.infoworld.com/article/3006630/how-to-work-with-attributes-in-c.html
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ISOFieldAttribute:Attribute
    {
        public ISOFieldAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }

    
}
